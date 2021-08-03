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

		private bool _isActive, _isUsed;

		private PlayerMovementBehaviour _playerMovementBehaviour;
		private float _time;

		public override void Start()
		{
			base.Start();

			Description = string.Format(Description, IncreaseValue, DurationValue);
			_playerMovementBehaviour = GameObject.Find("Player").GetComponent<PlayerMovementBehaviour>();
		}

		public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour) =>
			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceExpand");

		public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			if (_isActive) return;
			_isActive = _isUsed = true;

			_playerMovementBehaviour.MaxVelocity += IncreaseValue;

			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceLoop");
		}

		public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			if (!_isActive) return;

			_time += Time.deltaTime;
			if (_time < DurationValue) return;

			_isActive = false;

			_playerMovementBehaviour.MaxVelocity -= IncreaseValue;

			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceExpand");
			inventorySlotBehaviour.DropItem();
			Destroy(gameObject);
		}

		public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			if (!_isUsed) inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
			return !_isActive;
		}
	}
}
