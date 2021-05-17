using UnityEngine;

namespace Player.Weapons.Pistol
{
	public class PistolBullet : MonoBehaviour
	{
		private BaseWeapon _weapon;
		private float _velocity;
		private Vector2 _direction;
		public Rigidbody2D RigidBody;
		private Vector3 _origin;

		public static GameObject Make(GameObject bulletPrefab, BaseWeapon weapon, Vector2 direction)
		{
			GameObject bullet = Instantiate(bulletPrefab);
			bullet.transform.position = weapon.transform.parent.position;

			PistolBullet pistolBullet = bullet.GetComponent<PistolBullet>();
			pistolBullet._weapon = weapon;
			pistolBullet._velocity = weapon.Velocity;

			pistolBullet._direction = direction;
			pistolBullet._direction.Normalize();

			return bullet;
		}

		private void FixedUpdate()
		{
			Debug.Assert(_origin != null, "PreAlphaProjectile was not initialized");
			FixedUpdateMoveLogic();
			FixedUpdateDespawnLogic();
		}

		private void FixedUpdateMoveLogic()
		{
			Vector2 position = RigidBody.position + _direction * (_velocity * Time.fixedDeltaTime);
			RigidBody.MovePosition(position);
		}

		private void FixedUpdateDespawnLogic()
		{
			float distanceFromOrigin = (_origin - transform.position).sqrMagnitude;
			if (distanceFromOrigin > 20f)
			{
				Destroy(gameObject);
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			// If the collision occured with the player, ignore it.
			if (other.gameObject == _weapon.transform.parent.gameObject) return;

			//if (other.gameObject == _parent) return;

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
	}
}
