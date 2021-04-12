using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using static Direction;
using DirectionExtensions;
using GameObjectExtensions;

public class LevelGenerator : MonoBehaviour
{
	public GameObject StartingRoom;
	private IEnumerable<GameObject> RoomsCache = null;
	private IEnumerable<GameObject> Rooms
	{
		// find all room prefabs and store them in RoomsCache since the value
		// wont change and searching could be slow. currently to further speed
		// up the search it only looks in "Assets/2. Prefabs/Rooms" but it can
		// changed to look through all prefabs in the project by calling
		// AssetDatabase.FindAssets("") instead.
		get => RoomsCache ??= AssetDatabase.FindAssets("", new string[]{"Assets/2. Prefabs/Rooms"})
			.Select(guid => AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid)))
			.Where(prefab => prefab != null)
			.Where(prefab => prefab.GetComponent<Room>() != null);
	}

	private Tuple<Room,Doorway> RandomRoomConnectableTo(Doorway doorway) => Rooms
		.Select(obj => obj.GetComponent<Room>())
		.Select(room => new Tuple<Room,IEnumerable<Doorway>>(room, room.Doorways.Where(d => d.Direction.Opposite() == doorway.Direction)))
		.Where(roomDoorwaysPair => roomDoorwaysPair.Item2.Count() > 0)
		.Select(roomDoorwaysPair => new Tuple<Room,Doorway>(roomDoorwaysPair.Item1, roomDoorwaysPair.Item2.ElementAt(UnityEngine.Random.Range(0, roomDoorwaysPair.Item2.Count()))))
		.OrderBy(_ => UnityEngine.Random.value)
		.First();
		// 1. for each room component.
		// 2. pair the room together with a list all its doorways facing the
		//    direction we want (opposite of the passed in doorway, doorway on
		//    left connects to a doorway on the right).
		// 3. filter out the pairs that dont have any doorways facing the
		//    direction we want.
		// 4. for each pair, change the list of doorways into a single random
		//    doorway in the list.
		// 5. randomly order the pairs.
		// 6. select the first pair.
	
	void SpawnRooms(GameObject roomObject, int depth)
	{
		Room room = roomObject.GetComponent<Room>();
		foreach (Doorway doorway in room.Doorways)
		{
			(Room newRoom, Doorway newDoorway) = RandomRoomConnectableTo(doorway);
			Vector2 offset = roomObject.transform.position
						   + doorway.transform.position
						   + newRoom.transform.position
						   - newDoorway.transform.position;
			Instantiate(newRoom, offset, Quaternion.identity);
			// TODO: depth. and handle when no random room found?
		}
	}

    // Start is called before the first frame update
    void Start()
    {	
		GameObject startingRoom = Instantiate(StartingRoom, new Vector3(0, 0, 0), Quaternion.identity);
		SpawnRooms(startingRoom, 1);

		/* foreach (GameObject room in Rooms) */
		/* { */
		/* 	//GameObject x = Instantiate(Room, new Vector3(0, 0, 0), Quaternion.identity); */
		/* 	//print(x.GetBounds()); */
		/* 	//Instantiate(Room, new Vector3(x.GetBounds().center.x + x.GetBounds().size.x, 0, 0), Quaternion.identity); */
		/* } */
    }

    // Update is called once per frame
    void Update()
    {
    }
}
