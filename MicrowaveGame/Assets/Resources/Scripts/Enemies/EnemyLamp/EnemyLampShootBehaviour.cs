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
			_animator.SetFloat("ShootingDirectionX", _shootingDirection.x);
			_animator.SetFloat("ShootingDirectionY", _shootingDirection.y);
		}

		/// <summary>
		/// Shoot a projectile, this is called by a callback within the lamps
		/// animations.
		/// </summary>
		public void Shoot(int direction)
		{
			Vector3[] directions = { Vector3.up, Vector3.right, Vector3.down, Vector3.left };
			Vector3 animationDirection = directions[direction];

			// unity is attempting to call two animation events due to the
			// blendtree, so if this function was called from an animation
			// further than 45 degrees from the actual direction dismiss it.
			if (Vector3.Angle(_shootingDirection, animationDirection) >= 45) return;

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
