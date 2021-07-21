using Scripts.Inventory;
using UnityEngine;

namespace Scripts.Items
{
	[RequireComponent(typeof(BoxCollider2D))]
	public abstract class ItemBehaviour : MonoBehaviour
	{
		/// <summary>
		/// The name of the item.
		/// </summary>
		public string Name;

		/// <summary>
		/// The description of the item.
		/// </summary>
		public string Description;

		private InventoryBehaviour _inventoryBehaviour;

		/// <summary>
		/// A callback method that is called to pick up an item.
		/// </summary>
		/// <param name="inventorySlotBehaviour">The inventory slot the item will be stored in.</param>
		public abstract void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour);

		/// <summary>
		/// A callback method that is called to use the item.
		/// </summary>
		/// <param name="inventorySlotBehaviour">The inventory slot that the item is stored in.</param>
		/// <returns>Returns true if the item is ready to be used, otherwise returns false.</returns>
		public abstract bool OnUseItem(InventorySlotBehaviour inventorySlotBehaviour);

		/// <summary>
		/// A callback method that is called to update the item.
		/// </summary>
		/// <param name="inventorySlotBehaviour">The inventory slot that the item is stored in.</param>
		public abstract void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour);

		/// <summary>
		/// A callback method that is called to drop the item.
		/// </summary>
		/// <param name="inventorySlotBehaviour">The inventory slot that the item is stored in.</param>
		/// <returns>Returns true if the item is ready to be dropped, otherwise returns false.</returns>
		public abstract bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour);

		public virtual void Start()
		{
			_inventoryBehaviour = GameObject.Find("Inventory").GetComponent<InventoryBehaviour>();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.name == "Player") _inventoryBehaviour.AddNearbyItem(this);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.gameObject.name == "Player") _inventoryBehaviour.RemoveNearbyItem(this);
		}
	}
}
