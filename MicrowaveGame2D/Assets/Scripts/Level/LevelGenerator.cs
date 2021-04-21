using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Level
{
	class LevelGenerator
	{
		private static IEnumerable<GameObject> _roomPrefabs = AssetDatabase
			.FindAssets("", new[] { "Assets/Prefabs/Rooms" })
			.Select(guid => AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid)))
			.Where(roomPrefab => roomPrefab?.GetComponent<Room>() != null);
		
		private static IEnumerable<HashSet<DoorDirection>> _roomPrefabDirections = _roomPrefabs
			.Select(roomPrefab => roomPrefab.GetComponent<Room>())
			.Select(room => room.Doors.Select(door => door.direction))
			.Select(directions => new HashSet<DoorDirection>(directions));

		public static HashSet<DoorDirection> RandomDirections()
		{
			int random = UnityEngine.Random.Range(0, _roomPrefabDirections.Count());
			return _roomPrefabDirections.ElementAt(random);
		}

		public static Room GenerateLevel(Transform parent, GameObject startingRoomPrefab, int depth)
		{
			Dictionary<Vector2Int, Room> grid = new Dictionary<Vector2Int, Room>();

			Vector2Int center = new Vector2Int(0, 0);
			Room startingRoom = Room.Make(startingRoomPrefab, parent, center);
			grid.Add(center, startingRoom);

			SpawnRooms(startingRoom, parent, grid, depth);
			return startingRoom;
		}

		public static void SpawnRooms(Room room, Transform parent, Dictionary<Vector2Int,Room> grid, int depth)
		{
			foreach(Door door in room.Doors.Where(door => door.ConnectingDoor == null))
			{
				Vector2Int coordinates = room.coordinates + door.direction.ToVector2Int();
				GameObject roomPrefab = FindRandomPrefabFor(door, grid, coordinates, depth == 1);
				Room newRoom = Room.Make(roomPrefab, parent, coordinates);
				grid.Add(coordinates, newRoom);
				newRoom.transform.position = new Vector3(coordinates.x * 25, coordinates.y * 15, 0);

				// connect doors
				foreach (Door newDoor in newRoom.Doors)
				{
					Vector2Int newCoordinate = coordinates + newDoor.direction.ToVector2Int();
					if (grid.ContainsKey(newCoordinate) && grid[newCoordinate].Directions.Contains(newDoor.direction.Opposite()))
					{
						Door adjacentDoor = grid[newCoordinate].Doors.First(door => door.direction == newDoor.direction.Opposite());
						Door.Connect(newDoor, adjacentDoor);
					}
				}
				
				SpawnRooms(newRoom, parent, grid, depth - 1);
			}
		}

		private static GameObject FindRandomPrefabFor(Door door, Dictionary<Vector2Int,Room> grid, Vector2Int coordinates, bool deadend)
		{
			// TODO: this implementation of obtaining requiredDirections is
			// terrible, do not leave it this, just temporary.
			List<DoorDirection> requiredDirections = new List<DoorDirection>();		

			if (grid.ContainsKey(coordinates + Vector2Int.up) &&
				grid[coordinates + Vector2Int.up].Directions.Contains(DoorDirection.South))
			{
				requiredDirections.Add(DoorDirection.North);
			}

			if (grid.ContainsKey(coordinates + Vector2Int.right) &&
				grid[coordinates + Vector2Int.right].Directions.Contains(DoorDirection.West))
			{
				requiredDirections.Add(DoorDirection.East);
			}

			if (grid.ContainsKey(coordinates + Vector2Int.down) &&
				grid[coordinates + Vector2Int.down].Directions.Contains(DoorDirection.North))
			{
				requiredDirections.Add(DoorDirection.South);
			}

			if (grid.ContainsKey(coordinates + Vector2Int.left) &&
				grid[coordinates + Vector2Int.left].Directions.Contains(DoorDirection.East))
			{
				requiredDirections.Add(DoorDirection.West);
			}

			Debug.Log(
				"Finding Prefab With Required Directions "
				+ (requiredDirections.Contains(DoorDirection.North) ? "N" : "")
				+ (requiredDirections.Contains(DoorDirection.East) ? "E" : "")
				+ (requiredDirections.Contains(DoorDirection.South) ? "S" : "")
				+ (requiredDirections.Contains(DoorDirection.West) ? "W" : "")
			);

			List<GameObject> viablePrefabs = _roomPrefabs.ToList();
			foreach (DoorDirection direction in requiredDirections)
			{
				viablePrefabs = viablePrefabs
					.Where(prefab => prefab.GetComponent<Room>().Directions.Contains(direction))
					.ToList();
			}

			if (deadend)
			{
				viablePrefabs = viablePrefabs
					.Where(prefab => new HashSet<DoorDirection>(prefab.GetComponent<Room>().Directions).SetEquals(requiredDirections))
					.ToList();
			}

			Debug.Log(deadend);

			return viablePrefabs[UnityEngine.Random.Range(0, viablePrefabs.Count)];
		}

	}
}


