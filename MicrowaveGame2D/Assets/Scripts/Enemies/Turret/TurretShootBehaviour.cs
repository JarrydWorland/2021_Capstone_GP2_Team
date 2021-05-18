using UnityEngine;
using Weapons;

namespace Enemy.Turret
{
	public class TurretShootBehaviour : MonoBehaviour
	{
		[Tooltip("Weapon to shoot with")]
		public BaseWeapon Weapon;

		private GameObject _target;

		private void Start()
		{
			_target = GameObject.Find("Player");
			Weapon.Shoot();
		}

		private void Update()
		{
			if (_target != null)
			{
				Weapon.Direction = _target.transform.position - transform.position;
			}
		}
	}
}
