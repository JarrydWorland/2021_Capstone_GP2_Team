using Scripts.Audio;
using Scripts.Inventory;
using UnityEngine;

namespace Scripts.Items
{
    public class ItemHealthUpBehaviour : ItemBehaviour
    {
        /// <summary>
        /// Amount to increase player's max health by
        /// </summary>
        public int IncreaseValue;

        private HealthBehaviour _healthBehaviour;

        public AudioClip itemDrop;
        public AudioClip healthSFX;

        public override void Start()
        {
            base.Start();

            Description = string.Format(Description, IncreaseValue);
            _healthBehaviour = GameObject.Find("Player").GetComponent<HealthBehaviour>();
        }

        public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour)
        {
            inventorySlotBehaviour.PlayAnimation("InventorySlotBounceLoop");

            _healthBehaviour.MaxHealth += IncreaseValue;
			_healthBehaviour.Value += IncreaseValue;

            AudioManager.Play(healthSFX, AudioCategory.Effect, 0.75f, false);
        }


        public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour) { }

        public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour) { }

        public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
        {
            _healthBehaviour.MaxHealth -= IncreaseValue;
			_healthBehaviour.Value -= IncreaseValue;

            inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
            AudioManager.Play(itemDrop, AudioCategory.Effect, 0.45f);

            return true;
        }
    }
}

