using UnityEngine;
using Scripts.Rooms;

namespace Scripts.Utilities
{
	public static class InstanceFactory
	{
		/// <summary>
		/// Instantiate a room instance from a given room prefab.
		/// </summary>
		/// <param name="roomPrefab">
		/// The room prefab to instantiate.
		/// </param>
		/// <param name="parent">
		/// The transform of the parent gameobject instance, the instantiated
		/// room will be a child of the given transform.
		/// </param>
		/// <param name="position">
		/// The position of this room in the grid of rooms. The units used are
		/// relative to the starting room. So 0,0 is the starting room. 1,0 is
		/// the room east of that, etc.
		/// </param>
		/// <returns> Returns an instantiated and initialised room gameobject instance.</returns>
		public static GameObject InstantiateRoom(GameObject roomPrefab, Transform parent, Vector2Int position)
		{
			GameObject room = GameObject.Instantiate(roomPrefab, parent, true);
			room.GetComponent<RoomConnectionBehaviour>().Init(position);
			return room;
		}
	}
}
