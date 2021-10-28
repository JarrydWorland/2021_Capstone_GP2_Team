using Scripts.Audio;
using Scripts.Inventory;
using Scripts.Utilities;
using UnityEngine;
using System.Collections.Generic;

namespace Scripts.Items
{
    public class ItemLifeStealBehaviour : ItemBehaviour
    {
        public int IncreaseValue;

        public AudioClip ItemDrop;
        public AudioClip HealthSFX;

        private HealthBehaviour _healthBehaviour;
        private HealthBehaviour _enemyHealthBehaviour;

        public override void Start()
        {
            base.Start();

            _healthBehaviour = GameObject.Find("Player").GetComponent<HealthBehaviour>();
        }

        public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour)
        {
            inventorySlotBehaviour.PlayAnimation("InventorySlotBounceLoop");
        }

        public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour) { }

        public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour)
        {
            IEnumerable<GameObject> enemies = TagBehaviour.FindWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                _enemyHealthBehaviour = enemy.GetComponent<HealthBehaviour>();
                if (_enemyHealthBehaviour.Value <= 0)
                {
                    if (_healthBehaviour.Value < _healthBehaviour.MaxHealth)
                    {
                        _healthBehaviour.Value += IncreaseValue;
                        AudioManager.Play(HealthSFX, 0.75f);
                    }
                }
            }
        }

        public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
        {
            inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
            AudioManager.Play(ItemDrop, 0.45f);
            return true;
        }
    }
}