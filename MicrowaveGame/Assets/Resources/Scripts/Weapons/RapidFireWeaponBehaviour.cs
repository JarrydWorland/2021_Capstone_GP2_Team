using UnityEngine;
using Scripts.Utilities;

namespace Scripts.Weapons
{
    public class RapidFireWeaponBehaviour : WeaponBehaviour
	{
		/// <summary>
		/// The projectile prefab to spawn while shooting. The prefab must
		/// contain a ProjectileBehaviour component.
		/// </summary>
		public GameObject ProjectilePrefab;

		private float _fireRateInverse;
		private float _time;

		public override void Start()
		{
			base.Start();
			_fireRateInverse = 1.0f / FireRate;
		}

		/// <summary>
		/// A callback method that is called while the weapon is equipped by
		/// the player. Only one weapon can be equipped at a time.
		/// </summary>
		/// <param name="position">The position that the weapon should spawn projectiles at</param>
		/// <param name="direction">The direction that the weapon should fire projectiles towards</param>
		/// <param name="direction">Whether or not the weapon is being fired (is the trigger being pulled).</param>
		public override void OnWeaponUpdate(Vector2 position, Vector2 direction, bool shooting)
		{
			_time += Time.deltaTime;
			if (shooting && _time >= _fireRateInverse)
			{
				InstanceFactory.InstantiateProjectile(ProjectilePrefab, position, direction, ProjectileSpeed, Damage, "Enemy");
				_time = 0;
			}
		}

		/// <summary>
		/// A callback method that is called when the weapon is equipped by the
		/// player.
		/// </summary>
		public override void OnWeaponEquip()
		{

		}

		/// <summary>
		/// A callback method that is called when the weapon is unequiped by the
		/// player.
		/// </summary>
		public override void OnWeaponUnequip()
		{

		}
	}
}
