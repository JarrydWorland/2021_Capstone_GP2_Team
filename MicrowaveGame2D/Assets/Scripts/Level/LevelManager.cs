using System.Collections.Generic;
using UnityEngine;

namespace Level
{
	public class LevelManager : MonoBehaviour
	{
		public GameObject StartingRoomPrefab;
		public int Depth;
		public bool AlwaysShowRooms;

		private Camera _camera;
		private Vector3 _cameraTargetPosition;
		private Vector3 _cameraVelocity;
		private Room _currentRoom;
		private List<Room> _roomsToDisable = new List<Room>();

		[SerializeField] private AudioClip roomTransition;
		private AudioSource soundSource;

		public static LevelManager Instance => FindObjectOfType<LevelManager>();

		void Start()
		{
			_camera = Camera.main;
			_cameraTargetPosition = _camera.transform.position;
			_currentRoom = LevelGenerator.GenerateLevel(StartingRoomPrefab, transform, Depth, !AlwaysShowRooms);

			soundSource = GetComponent<AudioSource>();
			soundSource.loop = false;
			soundSource.playOnAwake = false;

			if (roomTransition != null)
			{
				soundSource.clip = roomTransition;
			}
		}

		void Update()
		{
			Vector3 start = _camera.transform.position;
			Vector3 end = _cameraTargetPosition;
			//_camera.transform.position = Vector3.Lerp(start, end, 20 * Time.deltaTime);
			_camera.transform.position = Vector3.SmoothDamp(start, end, ref _cameraVelocity, 0.1f);

			if (!AlwaysShowRooms)
			{
				if ((end - start).sqrMagnitude < 1.0f)
				{
					foreach(Room room in _roomsToDisable)
					{
						room.gameObject.SetActive(false);
					}
				}
			}
			else
			{
				_roomsToDisable.Clear();
			}
		}

		public void ChangeRoom(Door door)
		{
			// queue current room to be disabled
			_roomsToDisable.Add(_currentRoom);


			// set current room to new room and activate it
			Room newRoom = door.ConnectingDoor.GetComponentsInParent<Room>(true)[0];
			_currentRoom = newRoom;
			_currentRoom.gameObject.SetActive(true);

			if (roomTransition != null) soundSource.Play();

			// ensure the new room is not in the rooms to disable queue
			_roomsToDisable.Remove(_currentRoom);

			_cameraTargetPosition = new Vector3
			{
				x = newRoom.transform.position.x,
				y = newRoom.transform.position.y,
				z = _camera.transform.position.z,
			};
		}
	}
}
