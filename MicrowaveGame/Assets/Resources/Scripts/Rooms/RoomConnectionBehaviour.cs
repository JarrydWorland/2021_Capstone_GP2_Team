using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Scripts.Utilities;
using Scripts.Doors;

namespace Scripts.Rooms
{
	public class RoomConnectionBehaviour : MonoBehaviour
	{
		/// <summary>
		/// The doors within this room.
		/// </summary>
		/// <returns>Returns a list of DoorConnectionBehaviour from the door objects within the room.</returns>
		public IEnumerable<DoorConnectionBehaviour> Doors => GetComponentsInChildren<DoorConnectionBehaviour>(true);

		/// <summary>
		/// The directions the doors within the room are facing.
		/// </summary>
		/// <returns>Returns a list of directions from the door objects within the room.</returns>
		public IEnumerable<Direction> Directions => Doors.Select(door => door.Direction);

		/// <summary>
		/// The position of this room in the grid of rooms. The units used are
		/// relative to the starting room. So 0,0 is the starting room. 1,0 is
		/// the room east of that, etc.
		/// </summary>
		public Vector2Int Position { get; private set; }

		/// <summary>
		/// Used to initialise this component when an object is instantiated.
		/// This is called for you within InstanceFactory.InstantiateRoom().
		/// </summary>
		/// <param name="position">
		/// The position of this room in the grid of rooms. The units used are
		/// relative to the starting room. So 0,0 is the starting room. 1,0 is
		/// the room east of that, etc.
		/// </param>
		/// <returns>Returns "this" to allow for chain calling.</returns>
		public RoomConnectionBehaviour Init(Vector2Int position)
		{
			Position = position;
			return this;
		}

		/// <summary>
		/// Whether this room contains a door facing the given direction.
		/// </summary>
		/// <param name="direction">The direction to look for</param>
		/// <returns>Returns true if the room contains a door facing the given direction, otherwise false.</returns>
		public bool HasDoorFacing(Direction direction)
		{
			return Doors.Any(door => door.Direction == direction);
		}

		/// <summary>
		/// Get the door within the room facing the given direction.
		/// </summary>
		/// <param name="direction">The direction to look for</param>
		/// <returns>Returns the DoorConnectionBehaviour of the door within the room facing the given direction, otherwise returns null if no such door exists.</returns>
		public DoorConnectionBehaviour GetDoorFacing(Direction direction)
		{
			return Doors.FirstOrDefault(door => door.Direction == direction);
		}
	}
}
