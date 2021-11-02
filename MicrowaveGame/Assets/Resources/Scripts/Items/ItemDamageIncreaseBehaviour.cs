using Scripts.Inventory;
using Scripts.Player;
using UnityEngine;
using Scripts.Audio;

namespace Scripts.Items
{
	public class ItemDamageIncreaseBehaviour : ItemBehaviour
	{
		/// <summary>
		/// The amount of additional damage dealt by the player when the item is active.
		/// </summary>
		public int IncreaseValue;

		private PlayerShootBehaviour _playerShootBehaviour;

		public AudioClip itemDrop;
		public AudioClip WeaponDamageSFX;

		public override void Start()
		{
			base.Start();

			Description = string.Format(Description, IncreaseValue);
			_playerShootBehaviour = GameObject.Find("Player").GetComponent<PlayerShootBehaviour>();
		}

		public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			AudioManager.Play(WeaponDamageSFX, AudioCategory.Effect);
			_playerShootBehaviour.AdditionalDamage += IncreaseValue;
			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceLoop");
		}

		public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			_playerShootBehaviour.AdditionalDamage -= IncreaseValue;
			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
			AudioManager.Play(itemDrop, AudioCategory.Effect, 0.55f);

			return true;
		}
	}
}