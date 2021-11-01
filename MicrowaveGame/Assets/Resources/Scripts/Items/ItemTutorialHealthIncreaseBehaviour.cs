using Scripts.Inventory;
using UnityEngine;
using Scripts.Events;
using Scripts.Audio;

namespace Scripts.Items
{
    public class ItemTutorialHealthIncreaseBehaviour : ItemBehaviour
	{
		/// <summary>
		/// The amount of health to heal the player when the item is used.
		/// </summary>
		public int IncreaseValue;

		private HealthBehaviour _healthBehaviour;

		private bool _isUsed;

		public AudioClip itemDrop;
		public AudioClip healthSFX;

		public override void Start()
		{
			base.Start();

			Description = string.Format(Description, IncreaseValue);
		}

		private void OnEnable()
		{
			_healthBehaviour = GameObject.Find("Player").GetComponent<HealthBehaviour>();
			if (_healthBehaviour != null)
			{
				int targetHealth = _healthBehaviour.MaxHealth - IncreaseValue;
				if (_healthBehaviour.Value != targetHealth) _healthBehaviour.Value = targetHealth;
			}
		}

		public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour) =>
			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceExpand");

		public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			if (_healthBehaviour.Value < _healthBehaviour.MaxHealth)
			{
				_isUsed = true;

				_healthBehaviour.Value += IncreaseValue;
				AudioManager.Play(healthSFX, AudioCategory.Effect, 1.0f);
				inventorySlotBehaviour.PlayAnimation("InventorySlotBounceExpand");
				inventorySlotBehaviour.DropItem();

				// emit health event so tutorial room unlocks
				EventManager.Emit(new HealthChangedEventArgs
				{
					GameObject = gameObject,
					OldValue = 0,
					NewValue = 0,
				});

				Destroy(gameObject);
			}
		}

		public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			if (!_isUsed)
			{
				inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
				AudioManager.Play(itemDrop, AudioCategory.Effect, 0.55f);
			}
			return true;
		}
	}
}
