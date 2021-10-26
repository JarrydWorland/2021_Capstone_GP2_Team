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

        private Color setColor = new Color(61, 255, 98, 255);
        private Color makeColor;

        private HealthBehaviour _playerHealthBehaviour;
        private HealthBarBehaviour healthBarBehaviour;
        List<Animator> _cellAnimators = new List<Animator>();

        public GameObject HealthBar;
        public GameObject Cell;

        public AudioClip itemDrop;
        private int _pMax;

        public override void Start()
        {
            base.Start();

            Description = string.Format(Description, IncreaseValue);
            _playerHealthBehaviour = GameObject.Find("Player").GetComponent<HealthBehaviour>();
            healthBarBehaviour = GameObject.Find("HealthBar").GetComponent<HealthBarBehaviour>();
            _pMax = _playerHealthBehaviour.MaxHealth;
        }

        public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour)
        {
            _playerHealthBehaviour.MaxHealth += IncreaseValue;
            _playerHealthBehaviour.Value = _playerHealthBehaviour.MaxHealth;
            healthBarBehaviour.Target = GameObject.Find("Player");
            CreateCells();
            inventorySlotBehaviour.PlayAnimation("InventorySlotBounceLoop");
        }

        public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour) { }

        public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour) { }

        public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
        {
            _playerHealthBehaviour.MaxHealth -= IncreaseValue;
            _playerHealthBehaviour.Value = _playerHealthBehaviour.MaxHealth;
            Destroy(Cell);
            inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
            AudioManager.Play(itemDrop, 0.55f);
            return true;
        }

        private void CreateCells()
        {
            const float margin = 0.9f;
            float cellHeight = Cell.GetComponent<SpriteRenderer>().bounds.size.y;
            Cell.GetComponent<SpriteRenderer>().color = setColor;
            for (int i = _pMax; i < _playerHealthBehaviour.MaxHealth; i++)
            {
                HealthBar = Instantiate(Cell, healthBarBehaviour.transform);
                HealthBar.transform.position += new Vector3((cellHeight * margin * i), 0, 0);
                makeColor = Cell.GetComponent<SpriteRenderer>().color;
                Cell.GetComponent<SpriteRenderer>().color = setColor;
                _cellAnimators.Add(HealthBar.GetComponent<Animator>());
            }
        }
    }
}