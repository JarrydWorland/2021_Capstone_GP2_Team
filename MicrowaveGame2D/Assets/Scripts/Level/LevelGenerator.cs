using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Level
{
    public static class LevelGenerator
    {
        private static IEnumerable<GameObject> _roomPrefabs = AssetDatabase
            .FindAssets("", new[] { "Assets/Prefabs/Rooms" })
            .Select(guid => AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(roomPrefab => roomPrefab?.GetComponent<Room>() != null);

        public static Room GenerateLevel(Transform parent, GameObject startingRoomPrefab, int depth)
        {
            Dictionary<Vector2Int, Room> grid = new Dictionary<Vector2Int, Room>();

            Vector2Int centre = new Vector2Int(0, 0);
            Room startingRoom = Room.Make(startingRoomPrefab, parent, centre);
            grid.Add(centre, startingRoom);

            SpawnRooms(startingRoom, parent, grid, depth);
            return startingRoom;
        }

        private static void SpawnRooms(Room room, Transform parent, Dictionary<Vector2Int, Room> grid, int depth)
        {
            foreach (Door currentDoor in room.Doors.Where(door => door.ConnectingDoor == null))
            {
                Vector2Int newPosition = room.Position + currentDoor.Direction.ToVector2Int();
                GameObject newRoomPrefab = FindRandomPrefab(grid, newPosition, depth == 1);

                Room newRoom = Room.Make(newRoomPrefab, parent, newPosition);
                grid.Add(newPosition, newRoom);

                newRoom.transform.position = new Vector3(newPosition.x * 25, newPosition.y * 15, 0);

                foreach (Door newDoor in newRoom.Doors)
                {
                    Vector2Int newCoordinate = newPosition + newDoor.Direction.ToVector2Int();

                    if (grid.ContainsKey(newCoordinate) &&
                        grid[newCoordinate].Directions.Contains(newDoor.Direction.Opposite()))
                    {
                        Door adjacentDoor = grid[newCoordinate].Doors
                            .First(door => door.Direction == newDoor.Direction.Opposite());

                        Door.Connect(newDoor, adjacentDoor);
                    }
                }

                SpawnRooms(newRoom, parent, grid, depth - 1);
            }
        }

        private static GameObject FindRandomPrefab(Dictionary<Vector2Int, Room> grid, Vector2Int position,
            bool deadEndsOnly)
        {
            // TODO: Temporary solution. Make this not as hardcoded :)
            List<DoorDirection> requiredDirections = new List<DoorDirection>();

            if (grid.ContainsKey(position + Vector2Int.up) &&
                grid[position + Vector2Int.up].Directions.Contains(DoorDirection.South))
                requiredDirections.Add(DoorDirection.North);

            if (grid.ContainsKey(position + Vector2Int.right) &&
                grid[position + Vector2Int.right].Directions.Contains(DoorDirection.West))
                requiredDirections.Add(DoorDirection.East);

            if (grid.ContainsKey(position + Vector2Int.down) &&
                grid[position + Vector2Int.down].Directions.Contains(DoorDirection.North))
                requiredDirections.Add(DoorDirection.South);

            if (grid.ContainsKey(position + Vector2Int.left) &&
                grid[position + Vector2Int.left].Directions.Contains(DoorDirection.East))
                requiredDirections.Add(DoorDirection.West);

            Debug.Log("Finding prefab with required directions " +
                      (requiredDirections.Contains(DoorDirection.North) ? "N" : "") +
                      (requiredDirections.Contains(DoorDirection.East) ? "E" : "") +
                      (requiredDirections.Contains(DoorDirection.South) ? "S" : "") +
                      (requiredDirections.Contains(DoorDirection.West) ? "W" : ""));

            List<GameObject> viablePrefabs = _roomPrefabs.ToList();

            foreach (DoorDirection direction in requiredDirections)
            {
                viablePrefabs = viablePrefabs
                    .Where(prefab => prefab.GetComponent<Room>().Directions.Contains(direction)).ToList();
            }

            if (deadEndsOnly)
            {
                viablePrefabs = viablePrefabs.Where(prefab =>
                        new HashSet<DoorDirection>(prefab.GetComponent<Room>().Directions)
                            .SetEquals(requiredDirections))
                    .ToList();
            }

            Debug.Log(deadEndsOnly);

            return viablePrefabs[Random.Range(0, viablePrefabs.Count)];
        }
    }
}