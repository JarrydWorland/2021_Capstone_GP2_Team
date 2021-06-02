using System;
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

	public void OnPickupDropItemSlotOne(InputAction.CallbackContext context) => OnPickupDropItem(context, 0);
	public void OnPickupDropItemSlotTwo(InputAction.CallbackContext context) => OnPickupDropItem(context, 1);
	public void OnPickupDropItemSlotThree(InputAction.CallbackContext context) => OnPickupDropItem(context, 2);
	public void OnPickupDropItemSlotFour(InputAction.CallbackContext context) => OnPickupDropItem(context, 3);

	public void OnUseItemSlotOne(InputAction.CallbackContext context) => OnUseItem(context, 0);
	public void OnUseItemSlotTwo(InputAction.CallbackContext context) => OnUseItem(context, 1);
	public void OnUseItemSlotThree(InputAction.CallbackContext context) => OnUseItem(context, 2);
	public void OnUseItemSlotFour(InputAction.CallbackContext context) => OnUseItem(context, 3);

	public void OnSelectItemLeft(InputAction.CallbackContext context) => OnSelectItem(context, false);
	public void OnSelectItemRight(InputAction.CallbackContext context) => OnSelectItem(context, true);

	private void OnPickupDropItem(InputAction.CallbackContext context, int slotId)
	{
		if (!context.performed) return;

		// Store the count before potentially dropping an item to reduce if statements later on.
		int count = _nearbyItems.Count;

		// If the slot has an item, drop it.
		if (_slots[slotId] != null) DropItem(slotId);

		// If the count was larger than zero, there must be another item on the floor.
		// Therefore we can pick it up and perform a "swap" rather than just a "drop".
		if (count > 0) PickupItem(slotId);
	}

	private void OnUseItem(InputAction.CallbackContext context, int slotId)
	{
		if (!context.performed || Keyboard.current.shiftKey.isPressed) return;

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

	private void OnSelectItem(InputAction.CallbackContext context, bool reverse)
	{
		if (!context.performed || _nearbyItems.Count == 0) return;

		_nearbyItems.Requeue(reverse);
		Debug.Log($"Changed selected item to \"{_nearbyItems.Peek().name}\".");
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

		// Note: The following scaling code does not account for scaling in the transform of the object.

		SpriteRenderer itemSpriteRenderer = itemObject.GetComponent<SpriteRenderer>();

		// Get the item slot object and sprite render.
		GameObject itemSlotObject = GetSlotObject("SlotItem", slotId);
		SpriteRenderer itemSlotSpriteRenderer = itemSlotObject.GetComponent<SpriteRenderer>();

		// Set the item slot sprite to the item sprite.
		itemSlotSpriteRenderer.sprite = itemSpriteRenderer.sprite;

		// Set the item slot colour to the item colour.
		// This is temporary as the current item sprites are the same with different colour applied in Unity.
		itemSlotSpriteRenderer.color = itemSpriteRenderer.color;

		// Get the item slot background object and sprite renderer.
		GameObject itemSlotBackgroundObject = GetSlotObject("SlotBackground", slotId);
		SpriteRenderer itemSlotBackgroundSpriteRenderer = itemSlotBackgroundObject.GetComponent<SpriteRenderer>();

		// Get the x and y scale of the item slot background against the item.
		float scaleX = itemSlotBackgroundSpriteRenderer.size.x / itemSpriteRenderer.size.x;
		float scaleY = itemSlotBackgroundSpriteRenderer.size.y / itemSpriteRenderer.size.y;

		// Set the scale to the larger scale value.
		// Note that the "* 0.3f" is temporary due to the current item sprites being 1px wide / tall.
		float scale = (itemSpriteRenderer.size.x > itemSpriteRenderer.size.y ? scaleX : scaleY) * 0.3f;
		itemSlotSpriteRenderer.transform.localScale = new Vector3(scale, scale, 1.0f);

		Debug.Log(scaleX + ", " + scaleY + ", " + scale);

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

			// Update the item slot sprite to be empty.
			GetSlotObject("SlotItem", slotId).GetComponent<SpriteRenderer>().sprite = null;

			// Set the slot to null to indicate there is no item available.
			_slots[slotId] = null;
		}
	}

	private GameObject GetSlotObject(string common, int slotId)
	{
		return slotId switch
		{
			0 => GameObject.Find(common + "One"),
			1 => GameObject.Find(common + "Two"),
			2 => GameObject.Find(common + "Three"),
			3 => GameObject.Find(common + "Four"),
			_ => null
		};
	}
}