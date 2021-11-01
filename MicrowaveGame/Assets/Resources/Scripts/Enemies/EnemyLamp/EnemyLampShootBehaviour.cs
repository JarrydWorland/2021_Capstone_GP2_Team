using UnityEngine;
using Scripts.Utilities;
using Scripts.Audio;

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

		public AudioClip weaponSfx;

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
			_shootingDirection = (_player.transform.position - _projectileSpawn.transform.position).normalized;
			Vector2 shootingDirectionAnimator =  (_player.transform.position - transform.position).ToDirection().ToVector2();
			_animator.SetFloat("ShootingDirectionX", shootingDirectionAnimator.x);
			_animator.SetFloat("ShootingDirectionY", shootingDirectionAnimator.y);
		}

		/// <summary>
		/// Shoot a projectile, this is called by a callback within the lamps
		/// animations.
		/// </summary>d
		public void Shoot(int direction)
		{
			if (_player.GetComponent<HealthBehaviour>().Value <= 0 && _animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
			{
				_animator.Play("Idle");
			}
			else
			{
				InstanceFactory.InstantiateProjectile(
				ProjectilePrefab,
				_projectileSpawn.position,
				_shootingDirection,
				ProjectileSpeed,
				ProjectileDamage,
				"Player"
				);
				AudioManager.Play(weaponSfx, AudioCategory.Effect, 0.55f, false, Random.Range(0.85f, 1.25f));
			}
		}
	}
}
