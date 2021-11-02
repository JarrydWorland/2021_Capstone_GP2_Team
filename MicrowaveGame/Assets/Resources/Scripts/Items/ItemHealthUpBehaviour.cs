using Scripts.Audio;
using Scripts.Inventory;
using Scripts.HealthBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Items
{
    /// <summary>
    /// Known Issues:
    ///     - If the Player heals and then closes the game after increasing max health, the health bar will have the added cell on subsequent plays even though the Max Health starts at 5 in the inspector
    ///         - This is related to Line 54 in HealthBarBehaviour that updates _targetHealthBehaviour's MaxHealth variable, but the script's reference to the player's max health doesn't change without it
    ///     - An error related to an animator will trigger if the player drops a HealthUp item then tries to pick it up again
    ///         - This is creating duplicates of the item on the ground that can be picked up until all inventory slots are filled.
    /// </summary>

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
            _healthBarBehaviour.AddCells(IncreaseValue);

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

