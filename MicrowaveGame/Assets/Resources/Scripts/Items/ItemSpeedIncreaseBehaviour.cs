using Scripts.Inventory;
using Scripts.Player;
using UnityEngine;
using Scripts.Audio;

namespace Scripts.Items
{
    public class ItemSpeedIncreaseBehaviour : ItemBehaviour
	{
		/// <summary>
		/// The amount of additional speed when the item is active.
		/// </summary>
		public float IncreaseValue;

		public AudioClip ItemDrop;

		private PlayerMovementBehaviour _playerMovementBehaviour;

		public override void Start()
		{
			base.Start();
			_playerMovementBehaviour = GameObject.Find("Player").GetComponent<PlayerMovementBehaviour>();
		}

		public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			_playerMovementBehaviour.MaxVelocity += IncreaseValue;
			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceLoop");
		}

		public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			_playerMovementBehaviour.MaxVelocity -= IncreaseValue;
			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
			AudioManager.Play(ItemDrop, 0.55f);
			return true;
		}
	}
}
