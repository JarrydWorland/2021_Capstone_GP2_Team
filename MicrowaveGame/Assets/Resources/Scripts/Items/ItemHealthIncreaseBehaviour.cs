using Scripts.Audio;
using Scripts.Inventory;
using UnityEngine;

namespace Scripts.Items
{
    public class ItemHealthIncreaseBehaviour : ItemBehaviour
	{
		/// <summary>
		/// The amount of health to heal the player when the item is used.
		/// </summary>
		public int IncreaseValue;

		private HealthBehaviour _healthBehaviour;

		private bool _isUsed;

		public AudioClip itemDrop;

		public override void Start()
		{
			base.Start();

			Description = string.Format(Description, IncreaseValue);
			_healthBehaviour = GameObject.Find("Player").GetComponent<HealthBehaviour>();
		}

		public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour) =>
			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceExpand");

		public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			if (_healthBehaviour.Value < _healthBehaviour.MaxHealth)
			{
				_isUsed = true;

				_healthBehaviour.Value += IncreaseValue;

				inventorySlotBehaviour.PlayAnimation("InventorySlotBounceExpand");
				inventorySlotBehaviour.DropItem();
				Destroy(gameObject);
			}
		}

		public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			if (!_isUsed) inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
			AudioManager.Play(itemDrop, 0.55f);
			return true;
		}
	}
}
