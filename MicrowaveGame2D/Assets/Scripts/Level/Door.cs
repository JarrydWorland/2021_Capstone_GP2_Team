using UnityEngine;

namespace Level
{
    public class Door : MonoBehaviour
    {
        public DoorDirection Direction;
        public Door ConnectingDoor { get; private set; }

        public static void Connect(Door left, Door right)
        {
            left.ConnectingDoor = right;
            right.ConnectingDoor = left;
        }
    }
}