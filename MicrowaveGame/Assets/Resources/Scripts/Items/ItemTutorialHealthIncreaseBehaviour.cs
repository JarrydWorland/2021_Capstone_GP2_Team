using Scripts.Inventory;
using UnityEngine;
using Scripts.Events;
using Scripts.Audio;
using Scripts.Levels;

namespace Scripts.Items
{
    public class ItemTutorialHealthIncreaseBehaviour : ItemBehaviour
	{
		/// <summary>
		/// The amount of health to heal the player when the item is used.
		/// </summary>
		public int IncreaseValue;

		private HealthBehaviour _playerHealthBehaviour;
		private EventId<RoomTraversedEventArgs> _roomTraversedEventId;

		private bool _isUsed;

		public AudioClip itemDrop;
		public AudioClip healthSFX;
		public AudioClip ItemPickup;

		private void Awake()
		{
			_playerHealthBehaviour = GameObject.Find("Player").GetComponent<HealthBehaviour>();
			_roomTraversedEventId = EventManager.Register<RoomTraversedEventArgs>(OnRoomTraversedEvent);

			Description = string.Format(Description, IncreaseValue);
		}

		private void OnDestroy()
		{
			EventManager.Unregister(_roomTraversedEventId);
		}

		public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			AudioManager.Play(ItemPickup, AudioCategory.Effect);
			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceExpand");
		}

		public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			if (_playerHealthBehaviour.Value < _playerHealthBehaviour.MaxHealth)
			{
				_isUsed = true;

				_playerHealthBehaviour.Value += IncreaseValue;
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

		private void OnRoomTraversedEvent(RoomTraversedEventArgs args)
		{
			if (args.CurrentRoom.gameObject != transform.parent.gameObject) return;

			int targetHealth = _playerHealthBehaviour.MaxHealth - IncreaseValue;
			if (_playerHealthBehaviour.Value != targetHealth) _playerHealthBehaviour.Value = targetHealth;
		}
	}
}
