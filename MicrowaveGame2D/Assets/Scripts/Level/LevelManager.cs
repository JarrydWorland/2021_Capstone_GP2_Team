using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Level
{
	public class LevelManager : MonoBehaviour
	{
		private Vector3 _cameraPosition = new Vector3();
		public GameObject startingRoomPrefab;
		public int depth;

		public Room CurrentRoom { get; private set; }

		public static LevelManager Instance => FindObjectOfType<LevelManager>();

		private void Start()
		{
			_cameraPosition = Camera.main.transform.position;
			CurrentRoom = LevelGenerator.GenerateLevel(transform, startingRoomPrefab, depth);
		}

		void Update()
		{
			Camera.main.transform.position = Vector3.Lerp(
				Camera.main.transform.position,
				_cameraPosition,
				0.05f
			);
		}

		public void ChangeRoom(Door door)
		{
			Room newRoom = door.ConnectingDoor.GetComponentInParent<Room>();
			CurrentRoom = newRoom;

			_cameraPosition = new Vector3(newRoom.transform.position.x, newRoom.transform.position.y, -10);
		}
	}
}
