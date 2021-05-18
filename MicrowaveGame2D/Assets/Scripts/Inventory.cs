using Helpers;
using Items;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
	private BaseItem[] _slots;
	private CircularQueue<GameObject> _nearbyItems;

	private void Start()
	{
		_slots = new BaseItem[]
		{
			null,
			null,
			null,
			null
		};

		_nearbyItems = new CircularQueue<GameObject>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		// If the colliding object is an item and it is not already on the nearby list, add it.
		if (other.gameObject.HasComponent<BaseItem>() && !_nearbyItems.Contains(other.gameObject))
			_nearbyItems.Enqueue(other.gameObject);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		// If the colliding object is an item and it is not already on the nearby list, add it.
		if (other.gameObject.HasComponent<BaseItem>() && _nearbyItems.Contains(other.gameObject))
			_nearbyItems.Remove(other.gameObject);
	}

	// TODO: Add "Keyboard: Q and E; Controller: Left Bumper and Right Bumper" for cycling through nearby items.
	// OnCycleItem().

	public void OnUseItem(InputAction.CallbackContext context)
	{
		if (!context.performed) return;

		// Get the slot ID based on the key pressed.
		int slotId = GetSlotId(context.control.path);

		// Get the item in the slot.
		BaseItem slotItem = _slots[slotId];

		// If the item is not null, call its "Use()" method.
		// Note that we can't use "slotItem?.Use()" as that bypasses the game object's life time check.
		if (slotItem != null) slotItem.Use();
	}

	public void OnItemInteraction(InputAction.CallbackContext context)
	{
		if (!context.performed) return;

		// Get the slot ID based on the key pressed.
		int slotId = GetSlotId(context.control.path);

		// If the slot has an item, drop it.
		if (_slots[slotId] != null) DropItem(slotId);

		// If there is an item nearby, pick it up.
		if (_nearbyItems.Count > 0) PickupItem(slotId);
	}

	private void PickupItem(int slotId)
	{
		// Grab the first nearby item object.
		// This will be the currently selected item (i.e. its information is currently displayed). 
		GameObject itemObject = _nearbyItems.Dequeue();

		// Update the slot with the new item.
		_slots[slotId] = itemObject.GetComponent<BaseItem>();

		// Deactivate the item object so it can no longer be interacted with.
		itemObject.SetActive(false);
	}

	private void DropItem(int slotId)
	{
		// Get the current item object in the slot.
		GameObject droppedItemObject = _slots[slotId].gameObject;

		// Add the item to the nearby items queue.
		_nearbyItems.Enqueue(droppedItemObject);

		// Active the item so it can be interacted with.
		droppedItemObject.SetActive(true);

		// Set the slot to null to indicate there is null item available.
		_slots[slotId] = null;
	}

	private int GetSlotId(string raw)
	{
		string key = raw.Substring(raw.LastIndexOf('/') + 1);
		return int.TryParse(key, out int slot) ? slot - 1 : -1;
	}
}