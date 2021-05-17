using UnityEngine;

namespace Player.Weapons.Pistol
{
	public class PistolBullet : MonoBehaviour
	{
		private BaseWeapon _weapon;
		private float _velocity;
		private Vector2 _direction;
		public Rigidbody2D RigidBody;

		private void FixedUpdate()
		{
			//RigidBody.MovePosition(RigidBody.position + Velocity * (Speed * Time.fixedDeltaTime));
			// Move the bullet in the given direction.
			RigidBody.MovePosition(RigidBody.position + _direction * (_velocity * Time.deltaTime));
		}

		//private void OnTriggerExit2D(Collider2D other)
  //      {
		//	// If the collision occured with the player, ignore it.
		//	if (other.gameObject == _weapon.transform.parent.gameObject) return;
		//	GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
		//	GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
		//	GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

		//	foreach (GameObject floor in floors)
  //          {
		//		if (other.gameObject == floor) { Destroy(gameObject); }
		//	}

		//	foreach (GameObject wall in walls)
		//	{
		//		if (other.gameObject == wall) { Destroy(gameObject); }
		//	}

		//	foreach (GameObject door in doors)
		//	{
		//		if (other.gameObject == door) { Destroy(gameObject); }
		//	}
		//}

		private void OnTriggerEnter2D(Collider2D other)
		{
			// If the collision occured with the player, ignore it.
			if (other.gameObject == _weapon.transform.parent.gameObject) return;

			GameObject[] collidables = GameObject.FindGameObjectsWithTag("Collidable");

			foreach (GameObject collidable in collidables)
			{
				if (other.gameObject == collidable) { Destroy(gameObject); }
			}

			// Attempt to fetch the health component from the other object.
			Health health = other.GetComponent<Health>();

			// If it has a health component, reduce it's health by the damage
			// of the weapon the bullet was fired from.
			if (health != null)
			{
				health.Value -= _weapon.Damage;
				Destroy(gameObject);
			}
		}

		public static GameObject Make(GameObject bulletPrefab, BaseWeapon weapon, Vector2 direction)
		{
			GameObject bulletObject = Instantiate(bulletPrefab);
			bulletObject.transform.position = weapon.transform.parent.position;

			PistolBullet pistolBullet = bulletObject.GetComponent<PistolBullet>();
			pistolBullet._weapon = weapon;
			pistolBullet._velocity = weapon.Velocity;

			pistolBullet._direction = direction;
			pistolBullet._direction.Normalize();

			return bulletObject;
		}
	}
}
