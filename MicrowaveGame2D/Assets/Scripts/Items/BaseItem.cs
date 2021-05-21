using UnityEngine;

namespace Items
{
	public abstract class BaseItem : MonoBehaviour
	{
		// A flag to indicate if the item has been used or not.
		// If the item has been used and the player drops the item,
		// the item will be destroyed.
		public abstract bool Used { get; }

		// Called when the "Use Item" action is called
		// for the slot this item is in.
		public abstract void OnUseItem();

		// Continuously called while this item is in a slot.
		public abstract void OnItemUpdate();

		// Note: "Update()" will only work when this item is on the floor.
		// The object is inactive when it is in a slot.

		// Called when the Item is picked up.
		public abstract void OnPickupItem();

		// Called when the item is dropped.
		public abstract void OnDropItem();
	}
}