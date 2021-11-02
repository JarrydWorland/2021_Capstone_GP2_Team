using Scripts.Audio;
using Scripts.Inventory;
using Scripts.Player;
using UnityEngine;

namespace Scripts.Items
{
	public class ItemRateOfFireBehaviour : ItemBehaviour
	{
		/// <summary>
		/// The addition fire rate.
		/// </summary>
		public float IncreaseValue;

		private PlayerShootBehaviour _playerShootBehaviour;

		/// <summary>
		/// The drop item audio clip.
		/// </summary>
		public AudioClip DropItemAudioClip;

		public AudioClip RateOfFireSFX;

		public override void Start()
		{
			base.Start();
			_playerShootBehaviour = GameObject.Find("Player").GetComponent<PlayerShootBehaviour>();
		}

		public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			AudioManager.Play(RateOfFireSFX, AudioCategory.Effect);
			_playerShootBehaviour.FireRate += IncreaseValue;
			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceLoop");
		}

		public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			_playerShootBehaviour.FireRate -= IncreaseValue;

			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
			AudioManager.Play(DropItemAudioClip, AudioCategory.Effect, 0.55f);

			return true;
		}
	}
}