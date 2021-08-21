using UnityEngine;
using Scripts.Utilities;

namespace Scripts.Weapons
{
    public class BeamWeaponBehaviour : WeaponBehaviour
	{
		/// <summary>
		/// The projectile prefab to spawn while shooting. The prefab must
		/// contain a ProjectileBehaviour component.
		/// </summary>
		public GameObject ProjectilePrefab;

		public AudioClip ShootAudioClip;

		public float Spread = 0;

		private float _fireRateInverse;
		private float _time;
		private bool _firing = true;

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
		/// <param name="shooting">Whether or not the weapon is being fired (is the trigger being pulled).</param>
		/// <param name="additionalDamage">Any additional damage the player may deal on top of the weapon's damage.</param>
		public override void OnWeaponUpdate(Vector2 position, Vector2 direction, bool shooting, int additionalDamage = 0)
		{
			if (_firing)
			{
				float spreadAngle = Spread * Random.Range(0.0f, 1.0f) - (Spread/2.0f);
				Vector2 spread = new Vector2(Mathf.Cos(spreadAngle), Mathf.Sin(spreadAngle));
				Quaternion q = Quaternion.Euler(0, 0, spreadAngle);
				AudioManager.Play(ShootAudioClip);
				InstanceFactory.InstantiateProjectile(ProjectilePrefab, position, direction, ProjectileSpeed, Damage + additionalDamage, "Enemy");
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
