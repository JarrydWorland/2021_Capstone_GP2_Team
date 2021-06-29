using Scripts.Inventory;
using UnityEngine;

namespace Scripts.Items
{
	[RequireComponent(typeof(BoxCollider2D))]
	public abstract class ItemBehaviour : MonoBehaviour
	{
		public string Name;
		public string Description;

		private InventoryBehaviour _inventoryBehaviour;

		public abstract void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour);
		public abstract void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour);
		public abstract void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour);
		public abstract void OnDropItem(InventorySlotBehaviour inventorySlotBehaviour);

		protected virtual void Start()
		{
			_inventoryBehaviour = GameObject.Find("Inventory").GetComponent<InventoryBehaviour>();
		}

		private void OnTriggerEnter2D(Collider2D other) => _inventoryBehaviour.AddNearbyItem(this);
		private void OnTriggerExit2D(Collider2D other) => _inventoryBehaviour.RemoveNearbyItem(this);
	}
}