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

		private void Start()
		{
			_target = GameObject.Find("Player");
			_animator = GetComponent<Animator>();
			Weapon.Shoot();
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
