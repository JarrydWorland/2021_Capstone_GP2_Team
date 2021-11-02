using Scripts.Audio;
using Scripts.Inventory;
using Scripts.Player;
using UnityEngine;

namespace Scripts.Items
{
	public class ItemHomingShotBehaviour : ItemBehaviour
	{
		private PlayerShootBehaviour _playerShootBehaviour;

		/// <summary>
		/// The drop item audio clip.
		/// </summary>
		public AudioClip DropItemAudioClip;

		public override void Start()
		{
			base.Start();
			_playerShootBehaviour = GameObject.Find("Player").GetComponent<PlayerShootBehaviour>();
		}

		public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			_playerShootBehaviour.HomingStrength += 1;
			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceLoop");
		}

		public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			_playerShootBehaviour.HomingStrength -= 1;

			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
			AudioManager.Play(DropItemAudioClip, 0.55f);

			return true;
		}
	}
}
