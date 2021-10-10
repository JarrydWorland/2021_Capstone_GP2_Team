using Scripts.Player;
using UnityEngine;
using Scripts.Utilities;

namespace Scripts.Hazards
{
	public class HazardSpikeBehaviour : HazardBehaviour
	{
		/// <summary>
		/// The amount of damage that will be dealt to the player.
		/// </summary>
		public int Damage = 1;

		/// <summary>
		/// The rate at which damage is dealt each every second (e.g. 2.0f will deal damage twice every second or once every half second).
		/// </summary>
		public float DamageRate = 2.0f;

		/// <summary>
		/// The factor of which the speed of the player will be slowed down by (e.g. 0.5f will slow the player down by half of their current speed).
		/// </summary>
		public float SlowdownFactor = 0.5f;

		private PlayerMovementBehaviour _playerMovementBehaviour;
		private HealthBehaviour _healthBehaviour;

		private float _damageRateInverse;
		private float _time;

		private bool _colliding;

		public AudioClip sfx;

		private void Start()
		{
			GameObject playerObject = GameObject.Find("Player");

			_playerMovementBehaviour = playerObject.GetComponent<PlayerMovementBehaviour>();
			_healthBehaviour = playerObject.GetComponent<HealthBehaviour>();

			_damageRateInverse = 1.0f / DamageRate;
		}

		private void Update()
		{
			if (_colliding && _playerMovementBehaviour.Direction != Vector2.zero)
			{
				_time += Time.deltaTime;

				if (_time >= _damageRateInverse)
				{
					_healthBehaviour.Value -= Damage;
					_time -= _damageRateInverse;
					AudioManager.Play(sfx, 0.5f);
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if(other.gameObject.name == "Wheel")
            {
				_colliding = true;
				_playerMovementBehaviour.MaxVelocity *= SlowdownFactor;
				_time = _damageRateInverse;
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if(other.gameObject.name == "Wheel")
            {
				_colliding = false;
				_playerMovementBehaviour.MaxVelocity /= SlowdownFactor;
			}
		}
	}
}