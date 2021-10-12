using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Scripts.Utilities;
using Scripts.Rooms;
using Scripts.Doors;

namespace Scripts.Levels
{
    public static class LevelConnector
	{
		// NOTE: many of the operations here are expected to fail if presented
		// with an invalid level configuration, error checking has been moved
		// into the LevelConnectorEditor class to keep the logic here simpler.

		/// <summary>
		/// If the rooms comprising the level were manually placed instead of
		/// automatically generated, this method is used instead of
		/// LevelGenerator.GenerateLevel() to put the existing rooms in the
		/// scene into a valid play-state.
		/// </summary>
		/// <param name="parent">
		/// The transform of the parent gameobject, the instantiated rooms
		/// comprising the level will be a child of the given transform.
		/// </param>
		/// <returns>
		/// Returns the fully instantiated and initialized starting room. All
		/// rooms in the level are accessible via the starting room through the
		/// connected doors in each room which form a "level graph" of
		/// interconnected rooms.
		/// </returns>
		public static GameObject ConnectLevel(Transform parent)
		{
			Dictionary<Vector2Int,RoomConnectionBehaviour> roomGrid = CreateRoomGrid(parent);
			ConnectDoors(roomGrid);
			return GetStartingRoom(roomGrid);
		}

		private static Dictionary<Vector2Int,RoomConnectionBehaviour> CreateRoomGrid(Transform parent)
		{
			Dictionary<Vector2Int,RoomConnectionBehaviour> roomGrid = new Dictionary<Vector2Int,RoomConnectionBehaviour>();
			RoomConnectionBehaviour[] roomConnectionBehaviours = GameObject.FindObjectsOfType<RoomConnectionBehaviour>();
			foreach (RoomConnectionBehaviour roomConnectionBehaviour in roomConnectionBehaviours)
			{
				// assuming room is aligned to roomGrid
				Vector2Int position = Vector2Int.RoundToInt(roomConnectionBehaviour.transform.localPosition / LevelGenerator.RoomPlacementOffset);

				// initialize room
				roomConnectionBehaviour.Init(position);
				roomConnectionBehaviour.gameObject.SetActive(false);

				// expected to error if rooms overlap
				roomGrid.Add(position, roomConnectionBehaviour);
			}
			return roomGrid;
		}

		private static void ConnectDoors(Dictionary<Vector2Int,RoomConnectionBehaviour> roomGrid)
		{
			foreach(var pair in roomGrid)
			{
				Vector2Int position = pair.Key;
				RoomConnectionBehaviour room = pair.Value;

				foreach(DoorConnectionBehaviour door in room.Doors)
				{
					Vector2Int neighbouringPosition = position + door.Direction.ToVector2Int();

					// expected to error if neighbouring room does not exist
					RoomConnectionBehaviour neighbouringRoom = roomGrid[neighbouringPosition];
					DoorConnectionBehaviour neighbouringDoor = neighbouringRoom.GetDoorFacing(door.Direction.Opposite());

					// expected to error if neighbouring door does not exist
					DoorConnectionBehaviour.Connect(door, neighbouringDoor);
				}
			}
		}

		private static GameObject GetStartingRoom(Dictionary<Vector2Int,RoomConnectionBehaviour> roomGrid)
		{
			// expected to error if no starting room exists
			GameObject startingRoom = roomGrid[Vector2Int.zero].gameObject;
			startingRoom.SetActive(true);
			return startingRoom;
		}
	}

#if UNITY_EDITOR
	[InitializeOnLoadAttribute]
	public static class LevelConnectorEditor
	{
		private static Rect? _roomErrorRect = null;

		static LevelConnectorEditor()
		{
			EditorApplication.playModeStateChanged += VerifyLevel;
			EditorApplication.update += DrawRoomErrorRect;
		}

		private static void VerifyLevel(PlayModeStateChange state)
		{
			if (state != PlayModeStateChange.ExitingEditMode) return;

			_roomErrorRect = null;

			if (GameObject.FindObjectsOfType<RoomConnectionBehaviour>().Length == 0) return;

			VerifyRoomsDoNotOverlap();
			VerifyAllDoorsConnect();
			VerifyStartRoomExists();
			VerifyRoomsAlignToRoomGrid();
			VerifyRoomsAreChildrenOfLevel();
		}

		private static void VerifyRoomsDoNotOverlap()
		{
			HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
			RoomConnectionBehaviour[] roomConnectionBehaviours = GameObject.FindObjectsOfType<RoomConnectionBehaviour>();
			foreach (RoomConnectionBehaviour roomConnectionBehaviour in roomConnectionBehaviours)
			{
				Vector2 position = roomConnectionBehaviour.transform.localPosition;
				Vector2Int roomPosition = Vector2Int.RoundToInt(position / LevelGenerator.RoomPlacementOffset);
				if (roomPositions.Contains(roomPosition))
				{
					Error($"Detected overlapping manually placed rooms at {Log.Red(roomPosition * LevelGenerator.RoomPlacementOffset)}.", position);
					return;
				}
				roomPositions.Add(roomPosition);
			}
		}

		private static void VerifyAllDoorsConnect()
		{
			// create room grid
			Dictionary<Vector2Int,RoomConnectionBehaviour> roomGrid = new Dictionary<Vector2Int,RoomConnectionBehaviour>();
			RoomConnectionBehaviour[] roomConnectionBehaviours = GameObject.FindObjectsOfType<RoomConnectionBehaviour>();
			foreach (RoomConnectionBehaviour roomConnectionBehaviour in roomConnectionBehaviours)
			{
				Vector2 position = roomConnectionBehaviour.transform.localPosition;
				Vector2Int roomPosition = Vector2Int.RoundToInt(position / LevelGenerator.RoomPlacementOffset);
				if (roomGrid.ContainsKey(roomPosition)) continue;
				roomGrid.Add(roomPosition, roomConnectionBehaviour);
			}

			// ensure that all doors in the grid connect to eachother
			foreach(var pair in roomGrid)
			{
				Vector2Int roomPosition = pair.Key;
				RoomConnectionBehaviour room = pair.Value;

				foreach(DoorConnectionBehaviour door in room.Doors)
				{
					Vector2Int neighbouringPosition = roomPosition + door.Direction.ToVector2Int();

					if (!roomGrid.ContainsKey(neighbouringPosition))
					{
						Error($"Detected door(s) that lead nowhere. Room {Log.Red(room.name)} located at {Log.Red(roomPosition * LevelGenerator.RoomPlacementOffset)} requires a {Log.Red(door.Direction)} neighbouring room.", room.transform.position);
						return;
					}

					RoomConnectionBehaviour neighbouringRoom = roomGrid[neighbouringPosition];
					DoorConnectionBehaviour neighbouringDoor = neighbouringRoom.GetDoorFacing(door.Direction.Opposite());

					if (neighbouringDoor == null)
					{
						Error($"Detected door(s) that lead into a wall. Room {Log.Red(room.name)} located at {Log.Red(roomPosition * LevelGenerator.RoomPlacementOffset)} has a {Log.Red(door.Direction)} neighbouring room that does not contain a {Log.Red(door.Direction.Opposite())} door.", room.transform.position);
						return;
					}
				}
			}
		}

		private static void VerifyStartRoomExists()
		{
			RoomConnectionBehaviour[] roomConnectionBehaviours = GameObject.FindObjectsOfType<RoomConnectionBehaviour>();
			foreach (RoomConnectionBehaviour roomConnectionBehaviour in roomConnectionBehaviours)
			{
				Vector2 position = roomConnectionBehaviour.transform.localPosition;
				Vector2Int roomPosition = Vector2Int.RoundToInt(position / LevelGenerator.RoomPlacementOffset);
				if (roomPosition == Vector2Int.zero) return;
			}
			Error($"Could not find a manually placed starting room. The starting room should be placed at {Log.Cyan(Vector2.zero)}", Vector3.zero);
		}


		private static void VerifyRoomsAlignToRoomGrid()
		{
			RoomConnectionBehaviour[] roomConnectionBehaviours = GameObject.FindObjectsOfType<RoomConnectionBehaviour>();
			foreach (RoomConnectionBehaviour roomConnectionBehaviour in roomConnectionBehaviours)
			{
				Vector2 position = roomConnectionBehaviour.transform.localPosition;
				Vector2 roomSpacePosition = position / LevelGenerator.RoomPlacementOffset;
				if (roomSpacePosition.x % 1 != 0 || roomSpacePosition.y % 1 != 0)
				{
					Error($"Detected manually placed room {Log.Red(roomConnectionBehaviour.name)} that is not aligned to the room grid.", roomConnectionBehaviour.transform.position);
					return;
				}
			}
		}


		private static void VerifyRoomsAreChildrenOfLevel()
		{
			int roomsInLevelCount = GameObject.Find("Level").transform.childCount;
			int roomsInSceneCount = GameObject.FindObjectsOfType<RoomConnectionBehaviour>().Length;
			if (roomsInLevelCount != roomsInSceneCount)
			{
				int misplacedRoomCount = roomsInSceneCount - roomsInLevelCount;
				Error($"Detected {Log.Red(misplacedRoomCount)} manually placed room(s) outside of the Level object. Please drag them under the Level object for organization.");
			}
		}

		private static void DrawRoomErrorRect()
		{
			if (_roomErrorRect == null) return;

			float xMin = _roomErrorRect.Value.xMin;
			float yMin = _roomErrorRect.Value.yMin;
			float xMax = _roomErrorRect.Value.xMax;
			float yMax = _roomErrorRect.Value.yMax;

			Debug.DrawLine(new Vector3(xMin,yMin, 0), new Vector3(xMax, yMin), Color.red);
			Debug.DrawLine(new Vector3(xMax,yMin, 0), new Vector3(xMax, yMax), Color.red);
			Debug.DrawLine(new Vector3(xMax,yMax, 0), new Vector3(xMin, yMax), Color.red);
			Debug.DrawLine(new Vector3(xMin,yMax, 0), new Vector3(xMin, yMin), Color.red);
			Debug.DrawLine(new Vector3(xMin,yMin, 0), new Vector3(xMax, yMax), Color.red);
			Debug.DrawLine(new Vector3(xMax,yMin, 0), new Vector3(xMin, yMax), Color.red);
		}

		private static void Error(string reason, Vector3? roomPosition = null)
		{
			if (roomPosition != null && _roomErrorRect == null)
			{
				_roomErrorRect = new Rect
				{
					x = roomPosition.Value.x - LevelGenerator.RoomPlacementOffset.x/2,
					y = roomPosition.Value.y - LevelGenerator.RoomPlacementOffset.y/2,
					width = LevelGenerator.RoomPlacementOffset.x,
					height = LevelGenerator.RoomPlacementOffset.y,
				};
			}

			Log.Error($"{Log.Red("Level misconfiguration:")} {reason}", LogCategory.LevelGeneration);
			EditorApplication.ExitPlaymode();
			EditorApplication.Beep();
		}
	}
#endif
}
