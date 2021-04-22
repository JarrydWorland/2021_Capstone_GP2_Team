using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Level
{
    public static class LevelGenerator
    {
		/*=======================*/
		/* private static fields */
		/*=======================*/

        private static IEnumerable<GameObject> _roomPrefabs = AssetDatabase
            .FindAssets("", new[] { "Assets/Prefabs/Rooms" })
            .Select(guid => AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(roomPrefab => roomPrefab?.GetComponent<Room>() != null);

		/*=======================*/
		/* public static methods */
		/*=======================*/

        public static Room GenerateLevel(GameObject startingRoomPrefab, Transform parent, int depth)
        {
			RoomGrid grid = new RoomGrid();
			Vector2Int centre = new Vector2Int(0, 0);
			Room startingRoom = Room.Make(startingRoomPrefab, parent, centre);
			grid.Add(startingRoom);
			SpawnChildRooms(startingRoom, parent, grid, depth);
			return startingRoom;
        }

		/*========================*/
		/* private static methods */
		/*========================*/

        private static void SpawnChildRooms(Room room, Transform parent, RoomGrid grid, int depth)
        {
			bool enclosedPrefabRequired = depth == 1;
			bool DoorIsNotConnected(Door door) => door.ConnectingDoor == null;
            foreach (Door currentDoor in room.Doors.Where(DoorIsNotConnected))
            {
				// find room prefab
                Vector2Int newPosition = room.Position + currentDoor.Direction.ToVector2Int();
                GameObject newRoomPrefab = grid.FindRandomPrefabFor(newPosition, enclosedPrefabRequired);

				// create room using prefab
                Room newRoom = Room.Make(newRoomPrefab, parent, newPosition);
                grid.Add(newRoom);

				// position room in scene
				int roomPlacementDistance = 25;
                newRoom.transform.position = new Vector3
				{
					x = newPosition.x * roomPlacementDistance,
					y = newPosition.y * roomPlacementDistance / 2,
					z = 0,
				};

				// recursively spawn child-rooms
                SpawnChildRooms(newRoom, parent, grid, depth - 1);
            }
        }

		/*=================*/
		/* private classes */
		/*=================*/

		private class RoomGrid
		{
			/*================*/
			/* private fields */
			/*================*/

			private Dictionary<Vector2Int, Room> _grid = new Dictionary<Vector2Int, Room>();

			/*================*/
			/* public methods */
			/*================*/

			public void Add(Room room)
			{
				Debug.Assert(room != null);
				_grid.Add(room.Position, room);
				ConnectDoorsAt(room.Position);
			}

			public GameObject FindRandomPrefabFor(Vector2Int position, bool enclosedPrefabRequired)
			{
				HashSet<DoorDirection> requiredDirections = FindRequiredDirectionsFor(position);

				bool PrefabHasRequiredDirections(GameObject prefab)
				{
					Room prefabRoom = prefab.GetComponent<Room>();
					return requiredDirections.All(direction => prefabRoom.Directions.Contains(direction));
				}

				bool PrefabIsEnclosedIfRequired(GameObject prefab)
				{
					if (!enclosedPrefabRequired) return true;
					Room prefabRoom = prefab.GetComponent<Room>();
					return requiredDirections.SetEquals(prefabRoom.Directions);
				}

				bool PrefabHasSpaceInFrontOfDoors(GameObject prefab)
				{
					Room prefabRoom = prefab.GetComponent<Room>();
					bool DoorNotRequired(Door door) => !requiredDirections.Contains(door.Direction);
					IEnumerable<Door> nonRequiredDoors = prefabRoom.Doors.Where(DoorNotRequired);
					return nonRequiredDoors.All(door => PositionInDirectionIsEmpty(position, door.Direction));
				}

				IEnumerable<GameObject> viablePrefabs = _roomPrefabs
					.Where(PrefabHasRequiredDirections)
					.Where(PrefabIsEnclosedIfRequired)
					.Where(PrefabHasSpaceInFrontOfDoors);

				return viablePrefabs.RandomElement();
			}

			/*=================*/
			/* private methods */
			/*=================*/

			private void ConnectDoorsAt(Vector2Int position)
			{
				Room room = _grid[position];
				HashSet<DoorDirection> requiredDirections = FindRequiredDirectionsFor(position);

                foreach (Door door in room.Doors)
                {
					if (requiredDirections.Contains(door.Direction))
					{
						Vector2Int neighbouringPosition = position + door.Direction.ToVector2Int();
						Room neighbouringRoom = _grid[neighbouringPosition];
						Door neighbouringDoor = neighbouringRoom.GetDoorFacing(door.Direction.Opposite());
						Door.Connect(door, neighbouringDoor);
					}
                }
			}

			private HashSet<DoorDirection> FindRequiredDirectionsFor(Vector2Int position)
			{
				DoorDirection[] directions =
				{
					DoorDirection.North,
					DoorDirection.East,
					DoorDirection.South,
					DoorDirection.West,
				};

				bool DirectionIsRequired(DoorDirection direction)
				{
					Vector2Int positionInDirection = position + direction.ToVector2Int();
					if (!_grid.ContainsKey(positionInDirection)) return false;
					return _grid[positionInDirection].HasDoorFacing(direction.Opposite());
				}
				
				return directions.Where(DirectionIsRequired).ToHashSet();
			}

			private bool PositionInDirectionIsEmpty(Vector2Int position, DoorDirection direction)
			{
				return !_grid.ContainsKey(position + direction.ToVector2Int());
			}
		}
    }
}
