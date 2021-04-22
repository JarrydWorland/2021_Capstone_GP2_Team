using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Level
{
    public class Room : MonoBehaviour
    {
        public IEnumerable<Door> Doors => GetComponentsInChildren<Door>(true);
        public IEnumerable<DoorDirection> Directions => Doors.Select(door => door.Direction);

        public Vector2Int Position { get; private set; }

		public bool HasDoorFacing(DoorDirection direction)
		{
			return Doors.Any(door => door.Direction == direction);
		}

		public Door GetDoorFacing(DoorDirection direction)
		{
			return Doors.FirstOrDefault(door => door.Direction == direction);
		}

        public static Room Make(GameObject original, Transform parent, Vector2Int position)
        {
            GameObject gameObject = Instantiate(original, parent, true);

            Room room = gameObject.GetComponent<Room>();
            room.Position = position;

            return room;
        }
    }
}
