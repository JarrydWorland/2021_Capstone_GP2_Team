using UnityEngine;
using Scripts.Utilities;

namespace Scripts.Enemies.EnemyLamp
{
	[RequireComponent(typeof(Animator))]
	public class EnemyLampShootBehaviour : MonoBehaviour
	{
		/// <summary>
		/// The projectile to fire at the player.
		/// </summary>
		public GameObject ProjectilePrefab;

		/// <summary>
		/// The speed of the fired projectile.
		/// </summary>
		public float ProjectileSpeed;

		/// <summary>
		/// The damage dealt in hitpoints by the fired projectile.
		/// </summary>
		public int ProjectileDamage;

		private GameObject _player;
		private Transform _projectileSpawn;
		private Animator _animator;
		private Vector3 _shootingDirection;

		// Start is called before the first frame update
		void Start()
		{
			_player = GameObject.Find("Player");
			_projectileSpawn = transform.Find("ProjectileSpawn");
			_animator = GetComponent<Animator>();
		}

		// Update is called once per frame
		void Update()
		{
			_shootingDirection = (_player.transform.position - transform.position).normalized;
			Vector2 shootingDirectionAnimator = _shootingDirection.ToDirection().ToVector2();
			_animator.SetFloat("ShootingDirectionX", shootingDirectionAnimator.x);
			_animator.SetFloat("ShootingDirectionY", shootingDirectionAnimator.y);
		}

		/// <summary>
		/// Shoot a projectile, this is called by a callback within the lamps
		/// animations.
		/// </summary>
		public void Shoot(int direction)
		{
			InstanceFactory.InstantiateProjectile(
				ProjectilePrefab,
				_projectileSpawn.position,
				_shootingDirection,
				ProjectileSpeed,
				ProjectileDamage,
				"Player"
			);
		}
	}
}
