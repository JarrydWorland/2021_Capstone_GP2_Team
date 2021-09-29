using UnityEngine;

namespace Scripts.StatusEffects
{
	public class StatusEffectMelting : IStatusEffect
	{
		public int Duration { get; }
		private readonly int _damage;
		private readonly float _rateInverse;

		private readonly HealthBehaviour _healthBehaviour;
		private float _time;

		public StatusEffectMelting(int duration, int damage, float rate)
		{
			Duration = duration;
			_damage = damage;
			_rateInverse = 1.0f / rate;

			_healthBehaviour = GameObject.Find("Player").GetComponent<HealthBehaviour>();
		}

		public void Update()
		{
			_time += Time.deltaTime;

			if (_time >= _rateInverse)
			{
				_healthBehaviour.Value -= _damage;
				_time -= _rateInverse;
			}
		}

		public void Remove() { }
	}
}