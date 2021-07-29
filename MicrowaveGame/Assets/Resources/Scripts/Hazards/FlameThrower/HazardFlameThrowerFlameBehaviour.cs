using UnityEngine;

namespace Scripts.Hazards.FlameThrower
{
	public class HazardFlameThrowerFlameBehaviour : MonoBehaviour
	{
		// TODO: This should apply a burning status effect that deals damage even when the player isn't touching the flame anymore.
		// However, because we disable the flame objects, the burning effect can't continue to count down / apply the damage.
		// We're at the point were we need to implement the status effect system.

		/// <summary>
		/// The amount of damage that will be dealt to the player.
		/// </summary>
		public int Damage = 1;

		/// <summary>
		/// The rate at which damage is dealt each every second (e.g. 2.0f will deal damage twice every second or once every half second).
		/// </summary>
		public float DamageRate = 1.0f;

		private HealthBehaviour _healthBehaviour;

		private float _damageRateInverse;
		private float _damageRateTime;

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
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.name != "Player")
			{
				_damageRateTime = _damageRateInverse;
				_applied = true;
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.gameObject.name != "Player") _applied = false;
		}
	}
}