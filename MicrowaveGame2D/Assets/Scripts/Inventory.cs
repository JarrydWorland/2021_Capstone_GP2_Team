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

	private void Update()
	{
		foreach (BaseItem item in _slots)
		{
			if (item != null) item.ItemUpdate();
		}
	}

	// TODO: Separate actions to allow for easier extensibility / customisation of controls.
	// Example for OnPickupDropItem():
	// - Each slot will have its own action / callback (OnPickupDropItemSlotOne(), etc.).
	//   From there, we use the existing OnPickupDropItem() method (renamed and with an integer parameter)
	//   and parse in the slot ID that way.

	public void OnUseItem(InputAction.CallbackContext context)
	{
		if (!context.performed || Keyboard.current.shiftKey.isPressed) return;

		// Get the slot ID based on the key pressed.
		int slotId = GetSlotId(context.control.path);

		// Get the item in the slot.
		BaseItem slotItem = _slots[slotId];

		// If the item is not null, call its "Use()" method.
		// Note that we can't use "slotItem?.Use()" as that bypasses the game object's life time check.
		if (slotItem != null)
		{
			slotItem.Use();
			Debug.Log($"Used item \"{_slots[slotId].name}\" in slot \"{slotId}\".");
		}
	}

	public void OnSelectItem(InputAction.CallbackContext context)
	{
		if (!context.performed || _nearbyItems.Count == 0) return;

		// Get the key from the context.
		string key = GetKeyFromControlPath(context.control.path);

		// If the key is "q", requeue in reverse (last becomes first).
		// Otherwise if it is "e", requeue normally (first becomes last).
		if (key == "q") _nearbyItems.Requeue(true);
		else if (key == "e") _nearbyItems.Requeue();

		Debug.Log($"Changed selected item to \"{_nearbyItems.Peek().name}\".");
	}

	public void OnPickupDropItem(InputAction.CallbackContext context)
	{
		if (!context.performed) return;

		// Get the slot ID based on the key pressed.
		int slotId = GetSlotId(context.control.path);

		bool droppedItem = false;

		// If the slot has an item, drop it.
		if (_slots[slotId] != null)
		{
			DropItem(slotId);
			droppedItem = true;
		}

		// If no item was dropped and there is exactly one item, pick up the selected item.
		// If an item was dropped and there is more than one item nearby, pick up the selected item.
		if (droppedItem && _nearbyItems.Count > 1 || !droppedItem && _nearbyItems.Count == 1) PickupItem(slotId);
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

		Debug.Log($"Put item \"{_slots[slotId].name}\" in slot \"{slotId}\".");
	}

	private void DropItem(int slotId)
	{
		Debug.Log($"Dropped item \"{_slots[slotId].name}\" from slot \"{slotId}\".");

		// Get the current item object in the slot.
		GameObject droppedItemObject = _slots[slotId].gameObject;

		if (_slots[slotId].Used)
		{
			// If an item is being dropped but has already been used,
			// destroy the item object.
			Destroy(droppedItemObject);
		}
		else
		{
			// Add the item to the nearby items queue.
			_nearbyItems.Enqueue(droppedItemObject);

			// Set the position to the current player's position.
			droppedItemObject.transform.position = GameObject.Find("Player").transform.position;

			// Active the item so it can be interacted with.
			droppedItemObject.SetActive(true);

			// Set the slot to null to indicate there is no item available.
			_slots[slotId] = null;
		}
	}

	private string GetKeyFromControlPath(string raw) => raw.Substring(raw.LastIndexOf('/') + 1);
	private int GetSlotId(string raw) => int.TryParse(GetKeyFromControlPath(raw), out int slot) ? slot - 1 : -1;
}