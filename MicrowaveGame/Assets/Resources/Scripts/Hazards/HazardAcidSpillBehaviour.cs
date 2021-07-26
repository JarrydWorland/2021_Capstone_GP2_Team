using Scripts.Player;
using UnityEngine;

namespace Scripts.Hazards
{
	public class HazardAcidSpillBehaviour : HazardBehaviour
	{
		/// <summary>
		/// The amount of damage that will be dealt to the player.
		/// </summary>
		public int Damage = 1;

		/// <summary>
		/// The rate at which damage is dealt each every second (e.g. 2.0f will deal damage twice every second or once every half second).
		/// </summary>
		public float DamageRate = 1.0f;

		/// <summary>
		/// The duration the acid spill effect should last for once the player is no
		/// longer colliding with the acid spill.
		/// </summary>
		public int Duration = 5;

		private HealthBehaviour _healthBehaviour;

		private float _damageRateInverse;
		private float _damageRateTime;

		private float _durationTime;

		private bool _applied;

		private void Start()
		{
			_healthBehaviour = GameObject.Find("Player").GetComponent<HealthBehaviour>();
			_damageRateInverse = 1.0f / DamageRate;
		}

		private void Update()
		{
			if (!_applied) return;

			_damageRateTime += Time.deltaTime;

			if (_damageRateTime >= _damageRateInverse)
			{
				_healthBehaviour.Value -= Damage;
				_damageRateTime -= _damageRateInverse;
			}

			_durationTime += Time.deltaTime;
			if (_durationTime > Duration) _applied = false;
		}

		private void OnTriggerStay2D(Collider2D other)
		{
			if (other.gameObject.name != "Player") return;

			_durationTime = 0;
			_applied = true;
		}
	}
}