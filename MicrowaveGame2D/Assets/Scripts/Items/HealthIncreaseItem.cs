using Events;
using UnityEngine;

namespace Items
{
	public class HealthIncreaseItem : BaseItem
	{
		public int IncreaseValue;

		private Health _playerHealth;

		private bool _isConsumed;
		public override bool IsConsumed => _isConsumed;

		private bool _isActivated;

		private void Start()
		{
			_playerHealth = GameObject.Find("Player").GetComponent<Health>();
		}

		public override void Use()
		{
			if (_isActivated) return;

			if (_playerHealth.Value < _playerHealth.MaxHealth)
			{
				_isActivated = true;

				_playerHealth.Value += IncreaseValue;

				EventManager.Emit(new ItemConsumedEventArgs
				{
					Item = this
				});

				_isConsumed = true;
			}
		}

		public override void ItemUpdate() { }
	}
}