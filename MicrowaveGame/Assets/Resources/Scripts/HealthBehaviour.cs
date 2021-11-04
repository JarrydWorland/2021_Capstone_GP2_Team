using UnityEngine;
using Scripts.Events;
using Scripts.Utilities;
using System;
using System.Collections.Generic;
using Scripts.Audio;

namespace Scripts
{
	class HealthBehaviour : MonoBehaviour
	{
		/// <summary>
		/// How much health the object has in total.
		/// </summary>
		public int MaxHealth = 5;

		/// <summary>
		/// Prevents the health from becoming less than one.
		/// </summary>
		public bool Invincible = false;

		/// <summary>
		/// The audio clip to play when damage is received.
		/// </summary>
		public AudioClip DamageAudioClip;

		/// <summary>
		/// The particle system to play when damage is received.
		/// </summary>
		public ParticleSystem DamageParticleSystem;

		private float _flashTimer = 1.0f;
		private const float _flashDurationSeconds = 0.25f;
		private const float _flashDurationSecondsInverse = 1.0f / _flashDurationSeconds;
		private List<SpriteRenderer> _spriteRenderers
		{
			get
			{
				SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
				List<SpriteRenderer> childSpriteRenderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
				if (spriteRenderer) childSpriteRenderers.Add(spriteRenderer);
				return childSpriteRenderers;
			}
		}

		/// <summary>
		/// The current health of the object.
		/// </summary>
		public int Value
		{
			get => _value;
			set
			{
				int oldValue = _value;
				_value = value.Clamp(Invincible ? 1 : 0, MaxHealth);
				
				if (oldValue >= _value)
				{
					AudioManager.Play(DamageAudioClip, 0.75f, false, UnityEngine.Random.Range(0.55f, 1.35f));
					DamageParticleSystem.Play();
					_flashTimer = 0.0f;
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

		private void Start()
		{
			_value = MaxHealth;
		}

		private void Update()
		{
			_flashTimer += Time.deltaTime * _flashDurationSecondsInverse;
			float white = 1f - Mathf.Min(_flashTimer, 1f);
			foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
			{
				spriteRenderer.color = new Color(white, 0.27f * white, 0.27f * white);
			}
		}
	}

	public class HealthChangedEventArgs : EventArgs
	{
		public GameObject GameObject;
		public int OldValue;
		public int NewValue;
	}
}
