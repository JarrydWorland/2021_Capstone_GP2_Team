using UnityEngine;

namespace Weapons
{
	public class RapidFireWeapon : BaseWeapon
	{
		public override string Name => "Rapid Fire Weapon";
		public override string Description => "A weapon that shoots bullets at a constant rate";

		public override int Damage => BulletDamage;
		public override float FireRate => BulletFireRate;

		public int BulletDamage = 1;
		public float BulletFireRate = 1.0f;
		public float BulletVelocity = 20.0f;
		public Sprite BulletSprite;
		public float BulletScale = 1.0f;

		private float _fireRateInverse;
		private float _time;

		private bool _isFiring;

		private void Start()
		{
			_fireRateInverse = 1.0f / FireRate;
		}

		public override void Shoot()
		{
			_isFiring = true;
		}

		private GameObject SpawnBullet()
		{
			// Wether or not a weapon uses a built-in projectile type or allows
			// for an interchangable projectile prefab is a choice for each
			// weapon to make, for now RapidFireWeapon uses the former.
			GameObject bullet = new GameObject();
			bullet.AddComponent<SpriteRenderer>().sprite = BulletSprite;
			bullet.AddComponent<Rigidbody2D>();
			bullet.AddComponent<BoxCollider2D>().isTrigger = true;
			bullet.AddComponent<BulletBehaviour>().Init(this);
			bullet.transform.localScale = new Vector3(BulletScale, BulletScale, BulletScale);
			return bullet;
		}

		private void Update()
		{
			// We use < 0.0f to indicate semi-automatic firing.
			if (!_isFiring || FireRate <= 0.0f) return;

			_time += Time.deltaTime;

			if (_time >= _fireRateInverse)
			{
				SpawnBullet();
				_time -= _fireRateInverse;
			}
		}

		public override void Holster()
		{
			_isFiring = false;
			_time = _fireRateInverse;
		}


		[RequireComponent(typeof(Rigidbody2D))]
		private class BulletBehaviour : MonoBehaviour
		{
			private const float _velocity = 20.0f;

			private Rigidbody2D _rigidBody;
			private BaseWeapon _weapon;
			private Vector2 _direction;
			private Vector3 _origin;
			private int _damage;

			public void Init(BaseWeapon weapon)
			{
				_weapon = weapon;
				_direction = weapon.Direction;
				_origin = weapon.transform.position;
				_damage = weapon.Damage;
				transform.position = _origin;
			}

			private void Start()
			{
				_rigidBody = GetComponent<Rigidbody2D>();
			}

			private void FixedUpdate()
			{
				Debug.Assert(_origin != null, "PreAlphaProjectile was not initialized");
				FixedUpdateMoveLogic();
				FixedUpdateDespawnLogic();
			}

			private void FixedUpdateMoveLogic()
			{
				Vector2 position = _rigidBody.position + _direction * (_velocity * Time.fixedDeltaTime);
				_rigidBody.MovePosition(position);
			}

			private void FixedUpdateDespawnLogic()
			{
				float distanceFromOrigin = (_origin - transform.position).sqrMagnitude;
				if (distanceFromOrigin > 100f)
				{
					Destroy(gameObject);
				}
			}

			private void OnTriggerEnter2D(Collider2D other)
			{
				// If the collision occured with the player, ignore it.
				if (_weapon != null && other.gameObject == _weapon.gameObject) return;

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
					health.Value -= _damage;
					Destroy(gameObject);
				}
			}
		}
	}
}
