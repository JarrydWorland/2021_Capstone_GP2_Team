using Scripts.Player;
using UnityEngine;

namespace Scripts.StatusEffects
{
	public class StatusEffectFaster : IStatusEffect
	{
		public int Duration { get; }
		private readonly float _increaseValue;

		private PlayerMovementBehaviour _playerMovementBehaviour;

		public StatusEffectFaster(int duration, float increaseValue)
		{
			Duration = duration;
			_increaseValue = increaseValue;
		}

		public void Apply()
		{
			_playerMovementBehaviour = GameObject.Find("Player").GetComponent<PlayerMovementBehaviour>();
			_playerMovementBehaviour.MaxVelocity += _increaseValue;
		}

		public void Update() { }

		public void Remove()
		{
			_playerMovementBehaviour.MaxVelocity -= _increaseValue;
		}
	}
}