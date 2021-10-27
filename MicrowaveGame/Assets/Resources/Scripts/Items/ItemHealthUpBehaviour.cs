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

        private bool _isUsed;

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
        {/*
            //_isUsed = true;

            _healthBehaviour.MaxHealth += IncreaseValue;
            _healthBarBehaviour.AddCells(IncreaseValue, _healthBehaviour.MaxHealth);
            
            Debug.Log($"{_healthBehaviour.transform.name}'s Maximum Health Increased by {IncreaseValue}");
            Debug.Log($"Health Behaviour: {_healthBehaviour.transform.name}'s Maximum Health is {_healthBehaviour.MaxHealth}");

            AudioManager.Play(healthSFX, 0.75f, false);
            inventorySlotBehaviour.PlayAnimation("InventorySlotBounceExpand");
            inventorySlotBehaviour.DropItem();
            Destroy(gameObject);
            */
        }


        public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour) { }

        public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour) { }

        public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
        {
            if (!_isUsed)
            {
                inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
                AudioManager.Play(itemDrop, 0.45f);
            }
            return true;
        }
    }
}

