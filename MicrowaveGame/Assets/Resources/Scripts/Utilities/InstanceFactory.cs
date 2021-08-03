using UnityEngine;
using Scripts.Rooms;
using Scripts.Projectiles;

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

		/// <summary>
		/// Instantiate a projectile instance from a given projectile prefab.
		/// </summary>
		/// <param name="roomPrefab">
		/// The projectile prefab to instantiate.
		/// </param>
		/// <param name="position">
		/// The position to spawn the projectile at.
		/// </param>
		/// <param name="direction">
		/// The direction the projectile should travel towards.
		/// </param>
		/// <param name="speed">
		/// The speed the projectile should travel at.
		/// </param>
		/// <param name="damage">
		/// The number of hit points the projectile should deal.
		/// </param>
		/// <param name="targetTag">
		/// The TagBehaviour tag the projectile will target.
		/// </param>
		/// <returns> Returns an instantiated and initialised room gameobject instance.</returns>
		public static GameObject InstantiateProjectile(
			GameObject projectilePrefab,
			Vector2 position,
			Vector2 direction,
			float speed,
			int damage,
			string targetTag)
		{
			GameObject projectile = GameObject.Instantiate(projectilePrefab);
			projectile.GetComponent<ProjectileBehaviour>().Init(position, direction, speed, damage, targetTag);
			return projectile;
		}
	}
}
