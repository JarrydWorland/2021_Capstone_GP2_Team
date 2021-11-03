using Scripts.Audio;
using Scripts.Inventory;
using Scripts.Player;
using UnityEngine;

namespace Scripts.Items
{
    public class ItemProjectileSizeBehaviour : ItemBehaviour
    {
		/// <summary>
		/// The amount the size should be increased when the item is active.
		/// </summary>
		public float IncreaseValue;

		private PlayerShootBehaviour _playerShootBehaviour;

		public AudioClip itemDrop;

		public override void Start()
		{
			base.Start();
			_playerShootBehaviour = GameObject.Find("Player").GetComponent<PlayerShootBehaviour>();
		}

		public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			_playerShootBehaviour.ProjectileScale += IncreaseValue;
			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceLoop");
		}

		public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			_playerShootBehaviour.ProjectileScale -= IncreaseValue;
			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
			AudioManager.Play(itemDrop, AudioCategory.Effect, 0.55f);

			return true;
		}
    }
}
