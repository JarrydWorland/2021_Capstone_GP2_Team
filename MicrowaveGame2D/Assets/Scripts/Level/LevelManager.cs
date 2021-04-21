using UnityEngine;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        private Vector3 _cameraPosition;
        public GameObject StartingRoomPrefab;
        public int Depth;

        private Room _currentRoom;

        public static LevelManager Instance => FindObjectOfType<LevelManager>();

        void Start()
        {
            _cameraPosition = Camera.main.transform.position;
            _currentRoom = LevelGenerator.GenerateLevel(transform, StartingRoomPrefab, Depth);
        }

        void Update()
        {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, _cameraPosition, 0.05f);
        }

        public void ChangeRoom(Door door)
        {
            Room newRoom = door.ConnectingDoor.GetComponentInParent<Room>();
            _currentRoom = newRoom;

            _cameraPosition = new Vector3(newRoom.transform.position.x, newRoom.transform.position.y, -10);
        }
    }
}