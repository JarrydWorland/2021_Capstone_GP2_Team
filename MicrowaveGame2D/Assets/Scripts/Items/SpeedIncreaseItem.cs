using Events;
using Player;
using UnityEngine;

namespace Items
{
	public class SpeedIncreaseItem : BaseItem
	{
		public float IncreaseValue, DurationValue;

		private PlayerMovement _playerMovement;
		private float _time;

		private bool _isConsumed;
		public override bool IsConsumed => _isConsumed;

		private bool _isActivated;

		public override bool IsActivated => _isActivated;

		private void Start()
		{
			_playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
		}

		public override void OnUseItem()
		{
			if (_isActivated) return;
			_isActivated = true;

			_playerMovement.Speed += IncreaseValue;
		}

		public override void OnItemUpdate()
		{
			if (!_isActivated || _isConsumed) return;

			_time += Time.deltaTime;
			if (_time < DurationValue) return;

			_playerMovement.Speed -= IncreaseValue;

			EventManager.Emit(new ItemConsumedEventArgs
			{
				Item = this
			});

			_isConsumed = true;
		}

		public override void OnPickupItem() { }

		public override void OnDropItem() { }
	}
}