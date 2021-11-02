using Scripts.Audio;
using Scripts.Inventory;
using Scripts.HealthBar;
using System.Collections;
using System.Collections.Generic;
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
        private HealthBarBehaviour _healthBarBehaviour;

        public AudioClip itemDrop;
        public AudioClip healthSFX;

        public override void Start()
        {
            base.Start();

            Description = string.Format("Increases maximum health by {0}", IncreaseValue);
            _healthBehaviour = GameObject.Find("Player").GetComponent<HealthBehaviour>();
            _healthBarBehaviour = GameObject.Find("HealthBar").GetComponent<HealthBarBehaviour>();
        }

        public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour)
        {
            inventorySlotBehaviour.PlayAnimation("InventorySlotBounceLoop");

            _healthBehaviour.MaxHealth += IncreaseValue;
            _healthBarBehaviour.AddCells(IncreaseValue, _healthBehaviour.MaxHealth);

            AudioManager.Play(healthSFX, 0.75f, false);
        }


        public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour) { }

        public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour) { }

        public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
        {
            _healthBehaviour.MaxHealth -= IncreaseValue;
            Destroy(_healthBarBehaviour.transform.GetChild(_healthBarBehaviour.transform.childCount-1).gameObject);
            inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
            AudioManager.Play(itemDrop, 0.45f);
            return true;
        }
    }
}

