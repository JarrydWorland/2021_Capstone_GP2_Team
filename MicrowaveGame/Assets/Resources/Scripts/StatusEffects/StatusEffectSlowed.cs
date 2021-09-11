using Scripts.Player;
using UnityEngine;

namespace Scripts.StatusEffects
{
	public class StatusEffectSlowed : IStatusEffect
	{
		/// <summary>
		/// The duration the oil spill effect should last for once the player is no
		/// longer colliding with the oil spill.
		/// </summary>
		public int Duration => 5;

		/// <summary>
		/// The factor to reduce the acceleration by (e.g. 0.5f is 50% slower).
		/// </summary>
		private float _accelerationFactor = 0.5f;

		/// <summary>
		/// The factor to reduce the friction by (e.g. 0.25f is 75% more slippery).
		/// </summary>
		private float _frictionFactor = 0.25f;

		private PlayerMovementBehaviour _playerMovementBehaviour;

		public void Apply()
		{
			_playerMovementBehaviour ??= GameObject.Find("Player").GetComponent<PlayerMovementBehaviour>();

			_playerMovementBehaviour.Acceleration *= _accelerationFactor;
			_playerMovementBehaviour.Friction *= _frictionFactor;
		}

		public void Update() { }

		public void Remove()
		{
			_playerMovementBehaviour.Acceleration /= _accelerationFactor;
			_playerMovementBehaviour.Friction /= _frictionFactor;
		}
	}
}