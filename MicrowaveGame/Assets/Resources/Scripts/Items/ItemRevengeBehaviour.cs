using Scripts.Audio;
using Scripts.Events;
using Scripts.Inventory;
using Scripts.Player;
using UnityEngine;

namespace Scripts.Items
{
	public class ItemRevengeBehaviour : ItemBehaviour
	{
		public int IncreaseValue;
		private PlayerShootBehaviour _playerShootBehaviour;
		private EventId<HealthChangedEventArgs> _healthChangedEventId;

		public AudioClip itemDrop;
		public AudioClip WeaponDamageSFX;

		public override void Start()
		{
			base.Start();

			Description = string.Format(Description);
			_playerShootBehaviour = GameObject.Find("Player").GetComponent<PlayerShootBehaviour>();
		}

		private void OnHealthChanged(HealthChangedEventArgs eventArgs)
        {
			if (eventArgs.GameObject.name != "Player") return;

			if (eventArgs.NewValue < eventArgs.OldValue)
            {
				 IncreaseValue += eventArgs.OldValue - eventArgs.NewValue;
				_playerShootBehaviour.AdditionalDamage += IncreaseValue;
			}
        }

		public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			AudioManager.Play(WeaponDamageSFX, 0.75f, false);
			_healthChangedEventId = EventManager.Register<HealthChangedEventArgs>(OnHealthChanged);
			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceLoop");
		}

		public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			EventManager.Unregister(_healthChangedEventId);
			_playerShootBehaviour.AdditionalDamage = 0;
			IncreaseValue = 0;
			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
			AudioManager.Play(itemDrop, 0.55f);

			return true;
		}
	} 
}
