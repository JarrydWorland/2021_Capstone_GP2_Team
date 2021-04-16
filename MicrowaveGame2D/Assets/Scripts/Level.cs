using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameObject StartingRoomPrefab;
    public int Depth;

    private IEnumerable<GameObject> _roomPrefabs;

    private void Start()
    {
        // Get all room prefabs from the "Assets/Prefabs/Rooms" directory.
        _roomPrefabs = AssetDatabase
            .FindAssets("", new[] { "Assets/Prefabs/Rooms" })
            .Select(x => AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(x)))
            .Where(x => x != null && x.GetComponent<Room>() != null);

        // Instantiate the starting room inside of the level object.
        GameObject startingRoomObject = Instantiate(StartingRoomPrefab, transform, true);
        Room startingRoom = startingRoomObject.GetComponent<Room>();

        SpawnRooms(startingRoom, Depth);
    }

    /// <summary>
    /// Get a random door that is opposite to the given door's direction.
    /// </summary>
    /// <param name="door">The door to find an opposite door from.</param>
    /// <param name="deadEndsOnly">A flag indicating if the returned door should be from a single door room or not.</param>
    /// <returns></returns>
    private GameObject GetRandomOppositeDoorPrefab(Door door, bool deadEndsOnly = false)
    {
        DoorDirection opposite = door.Direction.Opposite();
        List<GameObject> viableDoorObjects = new List<GameObject>();

        foreach (GameObject roomPrefab in _roomPrefabs)
        {
            IEnumerable<Door> oppositeDoors = roomPrefab.GetComponentsInChildren<Door>();

            foreach (Door oppositeDoor in oppositeDoors)
            {
                // If the door is opposite to the given door
                // (and optionally a single door room when deadEndsOnly is true), add it to the viable door list.
                if (oppositeDoor.Direction == opposite &&
                    (!deadEndsOnly || roomPrefab.GetComponent<Room>().Doors.Count() == 1))
                {
                    viableDoorObjects.Add(oppositeDoor.gameObject);
                }
            }
        }

        // Choose a random door from the viable door list.
        GameObject randomDoorObject = viableDoorObjects.ElementAt(Random.Range(0, viableDoorObjects.Count));
        return randomDoorObject;
    }

    /// <summary>
    /// Recursively spawn rooms until the given depth reaches 0.
    /// </summary>
    /// <param name="room">The room to create more rooms off of.</param>
    /// <param name="depth">The depth of the room.</param>
    private void SpawnRooms(Room room, int depth)
    {
        // If the depth has reached 0, then we don't need to spawns more rooms.
        if (depth == 0) return;

        Door currentDoor;

        // Loop while there is a door with no connecting / opposite door.
        while ((currentDoor = room.Doors.FirstOrDefault(x => x.ConnectingDoor == null)) != null)
        {
            // Get a door that is opposite to the given room.
            GameObject oppositeDoorPrefab = GetRandomOppositeDoorPrefab(currentDoor, depth == 1);

            // Get the room the opposite door prefab belongs to.
            GameObject oppositeRoomPrefab = oppositeDoorPrefab.transform.parent.gameObject;

            // Create an instance of the opposite room inside of the level object.
            GameObject oppositeRoomObject = Instantiate(oppositeRoomPrefab, transform, true);

            Room oppositeRoom = oppositeRoomObject.GetComponent<Room>();

            // Get the opposite door from the room based on its position compared to the prefab.
            Door oppositeDoor = oppositeRoom.Doors.First(x =>
                (int) x.transform.position.x == (int) oppositeDoorPrefab.transform.position.x &&
                (int) x.transform.position.y == (int) oppositeDoorPrefab.transform.position.y);


            // For debugging / visualisation purposes, move the room's location so the connecting doors
            // are next to each other.
            oppositeRoomObject.transform.position =
                currentDoor.transform.position - oppositeDoor.transform.localPosition * 1.25f;

            // Connect the current and opposite doors together.
            Door.Connect(currentDoor, oppositeDoor);

            // Continue to spawn more rooms off of the current room.
            SpawnRooms(oppositeRoom, depth - 1);
        }
    }
}