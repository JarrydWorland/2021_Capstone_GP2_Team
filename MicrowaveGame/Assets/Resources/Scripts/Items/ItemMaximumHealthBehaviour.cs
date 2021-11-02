using Scripts.Inventory;
using UnityEngine;
using Scripts.Audio;

namespace Scripts.Items
{
    public class ItemMaximumHealthBehaviour : ItemBehaviour
    {
        public AudioClip ItemDrop;
        public AudioClip HealthSFX;
        public AudioClip ItemPickup;

        private bool _isUsed;

        private HealthBehaviour _playerHealthBehaviour;

        public override void Start()
        {
            base.Start();

            _playerHealthBehaviour = GameObject.Find("Player").GetComponent<HealthBehaviour>();
        }

        public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour)
        {
            AudioManager.Play(ItemPickup, AudioCategory.Effect);
            inventorySlotBehaviour.PlayAnimation("InventorySlotBounceExpand");
        }

        public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour)
        {
            if (_playerHealthBehaviour.Value != _playerHealthBehaviour.MaxHealth)
            {
            	_isUsed = true;
                _playerHealthBehaviour.Value = _playerHealthBehaviour.MaxHealth;
                Debug.Log($"{_playerHealthBehaviour.transform.name}'s Health Reset to Max");
                AudioManager.Play(HealthSFX, AudioCategory.Effect);
                inventorySlotBehaviour.DropItem();
                Destroy(gameObject);
            } 
        }

        public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour) { }

        public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
        {
            if (!_isUsed)
            {
            	inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
            	AudioManager.Play(ItemDrop, AudioCategory.Effect, 0.55f);
            }
            return true;
        }
    }
}
