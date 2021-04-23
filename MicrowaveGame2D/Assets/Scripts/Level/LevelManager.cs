using UnityEngine;

namespace Level
{
	public class LevelManager : MonoBehaviour
	{
		public GameObject StartingRoomPrefab;
		public int Depth;

		private Camera _camera;
		private Vector3 _cameraTargetPosition;
		private Room _currentRoom;

		public static LevelManager Instance => FindObjectOfType<LevelManager>();

		void Start()
		{
			_camera = Camera.main;
			_cameraTargetPosition = _camera.transform.position;
			_currentRoom = LevelGenerator.GenerateLevel(StartingRoomPrefab, transform, Depth);
		}

		void Update()
		{
			Vector3 start = _camera.transform.position;
			Vector3 end = _cameraTargetPosition;
			_camera.transform.position = Vector3.Lerp(start, end, 0.05f);
		}

		public void ChangeRoom(Door door)
		{
			Room newRoom = door.ConnectingDoor.GetComponentInParent<Room>();
			_currentRoom = newRoom;
			_cameraTargetPosition = new Vector3
			{
				x = newRoom.transform.position.x,
				y = newRoom.transform.position.y,
				z = _camera.transform.position.z,
			};
		}
	}
}
