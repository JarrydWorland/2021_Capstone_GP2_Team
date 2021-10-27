using Scripts.Inventory;
using UnityEngine;
using Scripts.Audio;
using Scripts.HealthBar;
using Scripts.Events;
using System.Collections.Generic;

namespace Scripts.Items
{
    public class ItemMaximumHealthBehaviour : ItemBehaviour
    {
        public int IncreaseValue;

        private bool _isUsed;

        private HealthBehaviour _playerHealthBehaviour;

        public AudioClip itemDrop;
        public AudioClip healthSFX;

        public override void Start()
        {
            base.Start();

            Description = string.Format(Description, IncreaseValue);
            _playerHealthBehaviour = GameObject.Find("Player").GetComponent<HealthBehaviour>();
        }

        public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour)
        {
            inventorySlotBehaviour.PlayAnimation("InventorySlotBounceExpand");
        }

        public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour)
        {
            _isUsed = true;

            if (_playerHealthBehaviour.Value != _playerHealthBehaviour.MaxHealth)
            {
                _playerHealthBehaviour.Value = _playerHealthBehaviour.MaxHealth;
                Debug.Log($"{_playerHealthBehaviour.transform.name}'s Health Reset to Max");
                AudioManager.Play(healthSFX, 0.75f, false);
                inventorySlotBehaviour.DropItem();
                Destroy(gameObject);
            } 
        }

        public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour) { }

        public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
        {
            if(!_isUsed)
            {
            inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
            AudioManager.Play(itemDrop, 0.55f);
            }
            return true;
        }
    }
}