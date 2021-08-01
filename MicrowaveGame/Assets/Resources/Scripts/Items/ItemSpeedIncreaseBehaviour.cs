using Scripts.Inventory;
using Scripts.Player;
using UnityEngine;

namespace Scripts.Items
{
	public class ItemSpeedIncreaseBehaviour : ItemBehaviour
	{
		/// <summary>
		/// The amount of additional speed when the item is active.
		/// </summary>
		public float IncreaseValue;

		/// <summary>
		/// The duration of the item.
		/// </summary>
		public int DurationValue;

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

		public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			if (_isActive) return;
			_isActive = true;

			_playerMovementBehaviour.MaxVelocity += IncreaseValue;

			inventorySlotBehaviour.PlayAnimation("InventorySlotUseItem");
		}

		public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			if (!_isActive) return;

			_time += Time.deltaTime;
			if (_time < DurationValue) return;

			_isActive = false;

			_playerMovementBehaviour.MaxVelocity -= IncreaseValue;

			inventorySlotBehaviour.DropItem();
			Destroy(this);
		}

		public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			return !_isActive;
		}
	}
}