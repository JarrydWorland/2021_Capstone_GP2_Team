using UnityEngine;
using Weapons;

namespace Enemy.Turret
{
	[RequireComponent(typeof(Animator))]
	public class LampEnemyShootBehaviour : MonoBehaviour
	{
		[Tooltip("Weapon to shoot with")]
		public BaseWeapon Weapon;

		private GameObject _target;
		private Animator _animator;

		[SerializeField] private AudioClip weaponFired;
		protected AudioSource soundSource;

		private void Start()
		{
			_target = GameObject.Find("Player");
			_animator = GetComponent<Animator>();

			soundSource = this.gameObject.AddComponent<AudioSource>();
			soundSource.loop = false;
			soundSource.playOnAwake = false;
			soundSource.volume = 0.5f;

			if (weaponFired != null)
				soundSource.clip = weaponFired;

			Weapon.Shoot();
			if (weaponFired != null)
			{
				soundSource.pitch = Random.Range(0.7f, 1.5f);
				soundSource.Play();
			}
		}

		private void Update()
		{
			if (_target != null)
			{
				Weapon.Direction = _target.transform.position - transform.position;
				_animator.SetFloat("DirectionX", Weapon.Direction.x);
				_animator.SetFloat("DirectionY", Weapon.Direction.y);
				_animator.SetFloat("FireRate", Weapon.FireRate);
			}
		}
	}
}
