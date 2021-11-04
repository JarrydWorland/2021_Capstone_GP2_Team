using Scripts.Player;
using UnityEngine;
using Scripts.Audio;

namespace Scripts.StatusEffects
{
    public class StatusEffectSlower : IStatusEffect
	{
		public int Duration { get; }

		/// <summary>
		/// The factor to reduce the acceleration by (e.g. 0.5f is 50% slower).
		/// </summary>
		private readonly float _accelerationFactor;

		/// <summary>
		/// The factor to reduce the friction by (e.g. 0.25f is 75% more slippery).
		/// </summary>
		private readonly float _frictionFactor;

		private readonly AudioClip _clip;

		private readonly PlayerMovementBehaviour _playerMovementBehaviour;

		public StatusEffectSlower(int duration, float accelerationFactor, float frictionFactor, AudioClip clip)
		{
			Duration = duration;
			_accelerationFactor = accelerationFactor;
			_frictionFactor = frictionFactor;
			_clip = clip;

			AudioManager.Play(_clip, AudioCategory.Effect);
			_playerMovementBehaviour = GameObject.Find("Player").GetComponent<PlayerMovementBehaviour>();
			_playerMovementBehaviour.Acceleration *= _accelerationFactor;
			_playerMovementBehaviour.Friction *= _frictionFactor;
		}

		public void Update() {}

		public void Remove()
		{
			_playerMovementBehaviour.Acceleration /= _accelerationFactor;
			_playerMovementBehaviour.Friction /= _frictionFactor;
		}
	}
}
