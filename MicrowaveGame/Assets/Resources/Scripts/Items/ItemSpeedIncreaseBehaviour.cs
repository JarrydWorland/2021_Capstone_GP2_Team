using Scripts.Inventory;
using Scripts.Player;
using UnityEngine;

namespace Scripts.Items
{
	public class ItemSpeedIncreaseBehaviour : ItemBehaviour
	{
		public int IncreaseValue, DurationValue;

		private bool _isActive;

		private PlayerMovementBehaviour _playerMovementBehaviour;
		private float _time;

		public override void Start()
		{
			base.Start();

			Description = string.Format(Description, IncreaseValue, DurationValue);
			_playerMovementBehaviour = GameObject.Find("Player").GetComponent<PlayerMovementBehaviour>();
		}

		public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override bool OnUseItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			if (_isActive) return false;
			_isActive = true;

			_playerMovementBehaviour.Speed += IncreaseValue;

			return true;
		}

		public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			if (!_isActive) return;

			_time += Time.deltaTime;
			if (_time < DurationValue) return;

			_isActive = false;

			_playerMovementBehaviour.Speed -= IncreaseValue;

			inventorySlotBehaviour.DropItem();
			Destroy(this);
		}

		public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			return !_isActive;
		}
	}
}
