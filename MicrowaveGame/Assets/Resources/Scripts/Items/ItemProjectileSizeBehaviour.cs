using Scripts.Audio;
using Scripts.Inventory;
using Scripts.Player;
using UnityEngine;

namespace Scripts.Items
{
    public class ItemProjectileSizeBehaviour : ItemBehaviour
    {
		private PlayerShootBehaviour _playerShootBehaviour;

		private GameObject _projectilePrefab;

		private Vector3 AddScale = new Vector3(0.05f,0.05f,0);
		public AudioClip itemDrop;

		public override void Start()
		{
			base.Start();

			Description = string.Format(Description);
			_projectilePrefab = Resources.Load<GameObject>("Prefabs/Projectiles/ProjectileBullet");
			_playerShootBehaviour = GameObject.Find("Player").GetComponent<PlayerShootBehaviour>();
		}

		public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			_projectilePrefab.transform.localScale += AddScale;
			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceLoop");
		}

		public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			_projectilePrefab.transform.localScale -= AddScale;
			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
			AudioManager.Play(itemDrop, 0.55f);

			return true;
		}
    }
}