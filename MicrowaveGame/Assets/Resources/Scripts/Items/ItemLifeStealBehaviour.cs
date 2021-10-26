using Scripts.Audio;
using Scripts.Inventory;
using Scripts.Utilities;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Items
{

    public class ItemLifeStealBehaviour : ItemBehaviour
    {
        public int IncreaseValue;

        private HealthBehaviour _healthBehaviour;
        private HealthBehaviour _enemyHealthBehaviour;

        public AudioClip itemDrop;
        public AudioClip healthSFX;

        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();

            Description = string.Format(Description, IncreaseValue);
            _healthBehaviour = GameObject.Find("Player").GetComponent<HealthBehaviour>();
        }

        public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour)
        {
            inventorySlotBehaviour.PlayAnimation("InventorySlotBounceLoop");
        }
        public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour) { }

        public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour)
        {
            List<GameObject> enemies = TagBehaviour.FindWithTag("Enemy").ToList();
            foreach (GameObject enemy in enemies)
            {
                _enemyHealthBehaviour = enemy.GetComponent<HealthBehaviour>();
                if (_enemyHealthBehaviour.Value <= 0)
                {
                    if (_healthBehaviour.Value < _healthBehaviour.MaxHealth)
                    {
                        _healthBehaviour.Value += IncreaseValue;
                        AudioManager.Play(healthSFX, 0.75f, false);

                    }
                }
            }
        }

        public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
        {
            inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
            AudioManager.Play(itemDrop, 0.45f);
            return true;
        }
    }
}
