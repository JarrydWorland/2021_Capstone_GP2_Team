using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Scripts.Utilities;
using Scripts.Rooms;
using Scripts.Doors;

namespace Scripts.Levels
{
    public static class LevelGenerator
	{
		/// <summary>
		/// A list containing all the enabled room prefabs that are within the
		/// Assets/Resources/Prefabs/Rooms directory (including sub-directories).
		/// Whether or not the prefab is enabled is determined via the Enabled
		/// field on the rooms RoomGenerationBehaviour component.
		/// </summary>
		private static IEnumerable<GameObject> _roomPrefabs = Resources
			.LoadAll<GameObject>("Prefabs/Rooms")
			.Where(roomPrefab => roomPrefab?.GetComponent<RoomGenerationBehaviour>()?.Enabled ?? false);

		/// <summary>
		/// Generate a new randomised level. This is an expensive method and
		/// should be called infrequently.
		/// </summary>
		/// <param name="startingRoomPrefab">
		/// A room prefab to use as the starting room.
		/// </param>
		/// <param name="parent">
		/// The transform of the parent gameobject, the instantiated rooms
		/// comprising the level will be a child of the given transform.
		/// </param>
		/// <param name="depth">
		/// The depth of the level to be generated. Depth should be a value
		/// larger than zero. A larger depth value will result in a larger
		/// level being generated.
		/// </param>
		/// <param name="disableChildRooms">
		/// Whether or not child-rooms (any room that is not the starting room)
		/// should be initially disabled. Normally rooms should only be enabled
		/// when the player enters them however it is sometimes useful for
		/// debugging to have all rooms enabled.
		/// </param>
		/// <returns>
		/// Returns the fully instantiated and initialized starting room. All
		/// rooms in the level are accessible via the starting room through the
		/// connected doors in each room which form a "level graph" of
		/// interconnected rooms.
		/// </returns>
		public static GameObject GenerateLevel(GameObject startingRoomPrefab, Transform parent, int depth, bool disableChildRooms = true)
		{
			RoomGrid grid = new RoomGrid();
			Vector2Int centre = new Vector2Int(0, 0);
			GameObject startingRoomInstance = InstanceFactory.InstantiateRoom(startingRoomPrefab, parent, centre);
			RoomConnectionBehaviour startingRoom = startingRoomInstance.GetComponent<RoomConnectionBehaviour>();
			grid.Add(startingRoom);
			SpawnChildRooms(startingRoom, parent, grid, depth, disableChildRooms);
			return startingRoomInstance;
		}

		/// <summary>
		/// This method is called recursively to generate the level. Given a
		/// room that is partially initialized, in that only some of its doors
		/// are connected to other rooms, spawn new rooms for the remaining
		/// doors that do not have an associated room. It will then call
		/// SpawnChildRooms on those new rooms.
		/// </summary>
		/// <param name="room">
		/// The RoomConnectionBehaviour of a partially initialized room.
		/// Partially initialized meaning it has some doors that are not yet
		/// connected to other rooms.
		/// </param>
		/// <param name="parent">
		/// The transform of the parent gameobject, the instantiated rooms
		/// comprising the level will be a child of the given transform.
		/// </param>
		/// <param name="grid">
		/// The RoomGrid to use while constructing the level.
		/// </param>
		/// <param name="depth">
		/// The depth of the level to be generated. Depth should be a value
		/// larger than zero. The depth is used to limit the recursion of this
		/// method.
		/// </param>
		/// <param name="disableChildRooms">
		/// Whether or not child-rooms (any room that is not the starting room)
		/// should be initially disabled. Normally rooms should only be enabled
		/// when the player enters them however it is sometimes useful for
		/// debugging to have all rooms enabled.
		/// </param>
		private static void SpawnChildRooms(RoomConnectionBehaviour room, Transform parent, RoomGrid grid, int depth, bool disableChildRooms)
		{
			bool enclosedPrefabRequired = depth == 1;
			bool DoorIsNotConnected(DoorConnectionBehaviour door) => door.ConnectingDoor == null;
			foreach (DoorConnectionBehaviour currentDoor in room.Doors.Where(DoorIsNotConnected))
			{
				// find room prefab
				Vector2Int newPosition = room.Position + currentDoor.Direction.ToVector2Int();
				GameObject newRoomPrefab = grid.FindRandomPrefabFor(newPosition, enclosedPrefabRequired);

				// create room using prefab
				GameObject newRoomInstance = InstanceFactory.InstantiateRoom(newRoomPrefab, parent, newPosition);
				RoomConnectionBehaviour newRoom = newRoomInstance.GetComponent<RoomConnectionBehaviour>();
				if (disableChildRooms) newRoom.gameObject.SetActive(false);
				grid.Add(newRoom);

				// position room in scene
				int roomPlacementDistance = 35;
				newRoom.transform.position = new Vector3
				{
					x = newPosition.x * roomPlacementDistance / 1.25f,
					y = newPosition.y * roomPlacementDistance / 2,
					z = 0,
				};

				// recursively spawn child-rooms
				SpawnChildRooms(newRoom, parent, grid, depth - 1, disableChildRooms);
			}
		}

		/// <summary>
		/// A temporary internal representation of the level used during level
		/// generation. When adding a room instance to the grid it will connect
		/// the doors of the new room to its neighbours ensuring a completed
		/// graph structure.
		/// </summary>
		private class RoomGrid
		{
			private Dictionary<Vector2Int, RoomConnectionBehaviour> _grid = new Dictionary<Vector2Int, RoomConnectionBehaviour>();

			/// <summary>
			/// Add a new room to a position in grid, this should be used with
			/// caution as the added room may not fit the requirements for the
			/// position. Use FindRandomPrefabFor to find a prefab for a room
			/// that meets the positions requirements. This method will connect
			/// the doors of the given room to its neighbours in the grid if
			/// those neighbours exist.
			/// </summary>
			/// <param name="room">
			/// The RoomConnectionBehaviour of a partially initialized room.
			/// Partially initialized meaning it has some doors that are not yet
			/// connected to other rooms.
			/// </param>
			public void Add(RoomConnectionBehaviour room)
			{
				Debug.Assert(room != null);
				_grid.Add(room.Position, room);
				ConnectDoorsAt(room.Position);
			}

			/// <summary>
			/// Given an empty position in the grid, find a random room prefab
			/// for that position that meets the following requirements:
			/// - PrefabHasRequiredDirections: Neighbouring rooms may have
			///   doors that need to lead into a room at the given position, and the
			///   random prefab must have associated doors so that the doors in
			///   the neighbouring rooms are not blocked off.
			/// - PrefabIsEnclosedIfRequired: At the end of level generation,
			///   the final chosen room prefabs must be fully enclosed so that the level
			///   does not contain doors that lead to nowhere.
			/// - PrefabHasSpaceInFrontOfDoors: Similar to PrefabHasRequiredDirections,
			///   neighbouring rooms may NOT have a door that leads into a room at
			///   the given position, so the chosen prefab must not have a door
			///   in that direction as it would be blocked off by the existing
			///   neighbouring room.
			/// </summary>
			/// <param name="position">
			/// The position in the grid of rooms. The units used are relative
			/// to the starting room. So 0,0 is the starting room. 1,0 is the
			/// room east of that, etc.
			/// </param>
			/// <param name="enclosedPrefabRequired">
			/// Whether or not the random prefab needs to be enclosed, meaning
			/// all of its doors will connect to already existing rooms and the
			/// room will contain no unconnected doors.
			/// </param>
			/// <returns>
			/// A random room prefab that meets the requirements for the given position.
			/// </returns>
			public GameObject FindRandomPrefabFor(Vector2Int position, bool enclosedPrefabRequired)
			{
				HashSet<Direction> requiredDirections = FindRequiredDirectionsFor(position);

				bool PrefabHasRequiredDirections(GameObject prefab)
				{
					RoomConnectionBehaviour prefabRoom = prefab.GetComponent<RoomConnectionBehaviour>();
					return requiredDirections.All(direction => prefabRoom.Directions.Contains(direction));
				}

				bool PrefabIsEnclosedIfRequired(GameObject prefab)
				{
					if (!enclosedPrefabRequired) return true;
					RoomConnectionBehaviour prefabRoom = prefab.GetComponent<RoomConnectionBehaviour>();
					return requiredDirections.SetEquals(prefabRoom.Directions);
				}

				bool PrefabHasSpaceInFrontOfDoors(GameObject prefab)
				{
					RoomConnectionBehaviour prefabRoom = prefab.GetComponent<RoomConnectionBehaviour>();
					bool DoorNotRequired(DoorConnectionBehaviour door) => !requiredDirections.Contains(door.Direction);
					IEnumerable<DoorConnectionBehaviour> nonRequiredDoors = prefabRoom.Doors.Where(DoorNotRequired);
					return nonRequiredDoors.All(door => PositionInDirectionIsEmpty(position, door.Direction));
				}

				IEnumerable<GameObject> viablePrefabs = _roomPrefabs
					.Where(PrefabHasRequiredDirections)
					.Where(PrefabIsEnclosedIfRequired)
					.Where(PrefabHasSpaceInFrontOfDoors);

				return viablePrefabs.GetRandomElement();
			}

			private void ConnectDoorsAt(Vector2Int position)
			{
				RoomConnectionBehaviour room = _grid[position];
				HashSet<Direction> requiredDirections = FindRequiredDirectionsFor(position);

				foreach (DoorConnectionBehaviour door in room.Doors)
				{
					if (requiredDirections.Contains(door.Direction))
					{
						Vector2Int neighbouringPosition = position + door.Direction.ToVector2Int();
						RoomConnectionBehaviour neighbouringRoom = _grid[neighbouringPosition];
						DoorConnectionBehaviour neighbouringDoor = neighbouringRoom.GetDoorFacing(door.Direction.Opposite());
						DoorConnectionBehaviour.Connect(door, neighbouringDoor);
					}
				}
			}

			private HashSet<Direction> FindRequiredDirectionsFor(Vector2Int position)
			{
				Direction[] directions =
				{
					Direction.North,
					Direction.East,
					Direction.South,
					Direction.West,
				};

				bool DirectionIsRequired(Direction direction)
				{
					Vector2Int positionInDirection = position + direction.ToVector2Int();
					if (!_grid.ContainsKey(positionInDirection)) return false;
					return _grid[positionInDirection].HasDoorFacing(direction.Opposite());
				}
				
				return directions.Where(DirectionIsRequired).ToHashSet();
			}

			private bool PositionInDirectionIsEmpty(Vector2Int position, Direction direction)
			{
				return !_grid.ContainsKey(position + direction.ToVector2Int());
			}
		}
	}
}
