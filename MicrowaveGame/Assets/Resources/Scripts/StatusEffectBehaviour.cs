using System.Collections.Generic;
using Scripts.StatusEffects;
using Scripts.Utilities;
using UnityEngine;

namespace Scripts
{
	public class StatusEffectBehaviour : MonoBehaviour
	{
		private List<IStatusEffect> _statusEffects;
		private List<float> _statusEffectStartTimes;

		private void Start()
		{
			_statusEffects = new List<IStatusEffect>();
			_statusEffectStartTimes = new List<float>();
		}

		private void Update()
		{
			float time = Time.time;

			for (int i = 0; i < _statusEffects.Count; i++)
			{
				IStatusEffect statusEffect = _statusEffects[i];
				statusEffect.Update();

				if (time - _statusEffectStartTimes[i] >= statusEffect.Duration) Remove(i);
			}
		}

		/// <summary>
		/// Create and apply the given status effect type.
		/// </summary>
		public void Apply<T>() where T : IStatusEffect, new()
		{
			T statusEffect = new T();

			int index = _statusEffects.FindIndex(x => x.GetType() == statusEffect.GetType());

			if (index > -1) _statusEffectStartTimes[index] = Time.time;
			else Add(statusEffect);
		}

		private void Add(IStatusEffect statusEffect)
		{
			_statusEffects.Add(statusEffect);
			_statusEffectStartTimes.Add(Time.time);
			statusEffect.Apply();

			Log.Info($"Applied {Log.Lime(statusEffect.GetType().Name)} status effect for {Log.Lime(gameObject.name)}.", LogCategory.StatusEffect);
		}

		private void Remove(int index)
		{
			Log.Info($"Removed {Log.Lime(_statusEffects[index].GetType().Name)} status effect for {Log.Lime(gameObject.name)}.", LogCategory.StatusEffect);

			_statusEffects[index].Remove();
			_statusEffects.RemoveAt(index);
			_statusEffectStartTimes.RemoveAt(index);
		}
	}
}