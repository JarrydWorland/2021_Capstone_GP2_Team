using Scripts.Inventory;
using Scripts.Utilities;
using Scripts.Player;
using UnityEngine;

namespace Scripts.Items
{
	public class ItemDamageIncreaseBehaviour : ItemBehaviour
	{
		/// <summary>
		/// The amount of additional damage dealt by the player when the item is active.
		/// </summary>
		public int IncreaseValue;

		private PlayerWeaponBehaviour _playerWeaponBehaviour;

		public AudioClip itemDrop;

		public override void Start()
		{
			base.Start();

			Description = string.Format(Description, IncreaseValue);
			_playerWeaponBehaviour = GameObject.Find("Player").GetComponent<PlayerWeaponBehaviour>();
		}

		public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			_playerWeaponBehaviour.AdditionalDamage += IncreaseValue;
			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceLoop");
		}

		public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			_playerWeaponBehaviour.AdditionalDamage -= IncreaseValue;
			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
			AudioManager.Play(itemDrop, 0.55f);

			return true;
		}
	}
}