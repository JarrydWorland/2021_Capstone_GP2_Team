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

        public static Room Make(GameObject original, Transform parent, Vector2Int coordinates)
        {
            GameObject gameObject = Instantiate(original, parent, true);

            Room room = gameObject.GetComponent<Room>();
            room.Position = coordinates;

            return room;
        }
    }
}