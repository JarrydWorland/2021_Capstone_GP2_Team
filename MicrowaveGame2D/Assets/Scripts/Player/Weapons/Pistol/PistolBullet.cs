using UnityEngine;

namespace Player.Weapons.Pistol
{
	public class PistolBullet : MonoBehaviour
	{
		private float _velocity;
		private Vector3 _direction;

		private void Update()
		{
			// Move the bullet in the given direction.
			transform.position = transform.position + _direction * (_velocity * Time.deltaTime);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			// If the collision occured with the player, ignore it.
			if (other.gameObject.GetComponent<PlayerMovement>() != null) return;

			// Attempt to fetch the health component from the other object.
			Health health = other.gameObject.GetComponent<Health>();

			// If it has a health component, reduce it's health by the damage
			// of the weapon the bullet was fired from.
			if (health != null) health.Value -= gameObject.GetComponentInParent<BaseWeapon>().Damage;

			Destroy(gameObject);
		}

		public static GameObject Make(GameObject bulletPrefab, Vector3 initialPosition, float velocity,
			Vector2 direction)
		{
			GameObject bulletObject = Instantiate(bulletPrefab);
			bulletObject.transform.position = initialPosition;

			PistolBullet pistolBullet = bulletObject.GetComponent<PistolBullet>();
			pistolBullet._velocity = velocity;

			pistolBullet._direction = direction;
			pistolBullet._direction.Normalize();

			return bulletObject;
		}
	}
}