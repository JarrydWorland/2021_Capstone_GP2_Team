using UnityEngine;

namespace Weapons
{
	public class RapidFireWeapon : BaseWeapon
	{
		public override string Name => "Rapid Fire Weapon";
		public override string Description => "A weapon that shoots bullets at a constant rate";

		public override int Damage
		{
			get => BulletDamage;
			set => BulletDamage = value;
		}

		public override float FireRate
		{
			get => BulletFireRate;
			set
            {
				BulletFireRate = value;
				_fireRateInverse = 1.0f / value;
            }
		}

		public float BulletFireRate;
        
		public int BulletDamage = 1;
		public float BulletVelocity = 20.0f;
		public Sprite BulletSprite;
		public float BulletScale = 1.0f;
		public float BulletSpin = 0.0f;
		public float BulletSpawnOffset = 0.0f;

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
			bullet.AddComponent<BulletBehaviour>().Init(this, BulletSpawnOffset, BulletVelocity, BulletSpin);
			bullet.transform.Rotate(Vector3.forward, Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg);
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

			private Rigidbody2D _rigidBody;
			private BaseWeapon _weapon;
			private Vector2 _direction;
			private Vector3 _origin;
			private int _damage;
			private float _spin;
			private float _velocity = 20.0f;

			public void Init(BaseWeapon weapon, float offset, float velocity, float spin)
			{
				_weapon = weapon;
				_direction = weapon.Direction;
				_origin = weapon.transform.position + ((Vector3)weapon.Direction) * offset;
				_damage = weapon.Damage;
				_velocity = velocity;
				_spin = spin;
				transform.position = _origin;
			}

			private void Start()
			{
				_rigidBody = GetComponent<Rigidbody2D>();
			}

			private void Update()
			{
				UpdateSpin();
			}

			private void UpdateSpin()
			{
				transform.Rotate(Vector3.forward, _spin * Time.deltaTime);
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
				if (distanceFromOrigin > 1000f)
				{
					Destroy(gameObject);
				}
			}

			private void OnTriggerEnter2D(Collider2D other)
			{
				// TODO: Temporarily, until we implement proper collsion
				// groups, the bullet will use an arbitrary property of the
				// thing it collides with (in this case the sprite of the
				// attached RapidFireWeapon) to determine if the target is
				// friendly or not. THIS MUST BE CHANGED.
				RapidFireWeapon otherWeapon = other.GetComponent<RapidFireWeapon>();
				if (_weapon != null && otherWeapon != null && ((RapidFireWeapon)_weapon).BulletSprite == otherWeapon.BulletSprite) return;

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
