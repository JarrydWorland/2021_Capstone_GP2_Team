using UnityEngine;

namespace Items
{
	public abstract class BaseItem : MonoBehaviour
	{
		public abstract bool Used { get; }

		// Called when the "Use Item" action is called
		// for the slot this item is in.
		public abstract void Use();

		// Continuously called while this item is in a slot.
		public abstract void ItemUpdate();

		// Note: "Update()" will only work when this item is on the floor.
		// The object is inactive when it is in a slot.
	}
}