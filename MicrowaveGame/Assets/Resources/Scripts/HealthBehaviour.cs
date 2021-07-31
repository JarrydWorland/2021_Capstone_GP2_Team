using UnityEngine;
using Scripts.Events;
using Scripts.Utilities;
using System;

namespace Scripts
{
	class HealthBehaviour : MonoBehaviour
	{
		/// <summary>
		/// How much health the object has in total.
		/// </summary>
		public int MaxHealth = 5;

		/// <summary>
		/// The current health of the object.
		/// </summary>
		public int Value
		{
			get => _value;
			set
			{
				int oldValue = _value;
				_value = value.Clamp(0, MaxHealth);
				
				if (oldValue > _value)
				{
					AudioManager.Play(DamageAudioClip);
				}
				
				EventManager.Emit(new HealthChangedEventArgs
				{
					GameObject = gameObject,
					OldValue = oldValue,
					NewValue = _value,
				});
			}
		}
		private int _value;


		/// <summary>
		/// The audio clip to play when damage is received.
		/// </summary>
		public AudioClip DamageAudioClip;

		private void Start()
		{
			_value = MaxHealth;
		}
	}

	public class HealthChangedEventArgs : EventArgs
	{
		public GameObject GameObject;
		public int OldValue;
		public int NewValue;
	}
}
