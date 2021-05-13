using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Weapons.Pistol
{
	public class PistolWeapon : BaseWeapon
	{
		public override string Name => "Pistol";
		public override string Description => "A basic weapon for start off with.";

		public override int Damage => 1;
		public override float Velocity => 10.0f;
		public override float FireRate => 5.0f;

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

		private void SpawnBullet()
		{
			// Get the direction the player is currently aiming in.
			Vector2 mousePosition = Mouse.current.position.ReadValue();
			Vector2 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
			Vector2 direction = mousePositionInWorld - (Vector2) transform.parent.position;

			// Create a bullet instance.
			PistolBullet.Make(BulletPrefab, this, direction);
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
	}
}
