using System;
using UnityEngine;

namespace Items
{
	public abstract class BaseItem : MonoBehaviour
	{
		public int? SlotId { get; set; }

		public abstract bool IsActivated { get; }

		public abstract bool IsConsumed { get; }

		// Items are by default not passive.
		// Set this to true a startup method like Start().
		public bool IsPassive { get; protected set; }

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

	public class ItemConsumedEventArgs : EventArgs
	{
		public BaseItem Item;
	}
}