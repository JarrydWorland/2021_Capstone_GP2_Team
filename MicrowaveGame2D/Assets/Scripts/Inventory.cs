using Events;
using Helpers;
using Items;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
	private BaseItem[] _slots;
	private CircularQueue<GameObject> _nearbyItems;

	private GameObject _selectedItemIndicatorObject;

	private GameObject[] _slotObjects;

	[SerializeField]
	private AudioClip itemPickup;

	private AudioSource soundSource;

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

		_selectedItemIndicatorObject = GameObject.Find("SelectedItemIndicator");
		_selectedItemIndicatorObject.SetActive(false);

		_slotObjects = new GameObject[]
		{
			GameObject.Find("SlotItemOne"),
			GameObject.Find("SlotItemTwo"),
			GameObject.Find("SlotItemThree"),
			GameObject.Find("SlotItemFour")
		};

		EventManager.Register<ItemConsumedEventArgs>(OnItemConsumed);

		soundSource = GetComponent<AudioSource>();
		soundSource.loop = false;
		soundSource.playOnAwake = false;

		if (itemPickup != null) soundSource.clip = itemPickup;
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
		UpdateSelectedItemIndicator();

		foreach (BaseItem item in _slots)
		{
			if (item != null)
			{
				item.OnItemUpdate();

				if (item.IsActivated && !item.IsConsumed)
				{
					float value = MapValueBetween(Mathf.Sin(Time.frameCount * 0.005f), -1.0f, 1.0f, 1.2f, 1.3f);
					_slotObjects[item.SlotId.Value].transform.localScale = new Vector3(value, value, 1.0f);
				}
			}
		}
	}

	private void OnItemConsumed(ItemConsumedEventArgs eventArgs)
	{
		GameObject itemObject = eventArgs.Item.gameObject;
		GameObject itemSlotObject = _slotObjects[itemObject.GetComponent<BaseItem>().SlotId.Value];

		itemSlotObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("HUD/ItemSlot");
		itemSlotObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.0f);

		Destroy(itemObject);
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

		BaseItem slotItem = _slots[slotId];

		// If the slot has an item, drop it.
		if (slotItem != null)
		{
			// If the item has been activated, it cannot be dropped.
			if (slotItem.IsActivated && !slotItem.IsPassive) return;
			DropItem(slotId);
		}

		// If the count was larger than zero, there must be another item on the floor.
		// Therefore we can pick it up and perform a "swap" rather than just a "drop".
		if (count > 0) PickupItem(slotId);
	}

	private void OnUseItem(InputAction.CallbackContext context, int slotId)
	{
		if (!context.performed || Keyboard.current.shiftKey.isPressed) return;

		// Get the item in the slot.
		BaseItem slotItem = _slots[slotId];

		// If the item is not null, call its "OnUseItem()" method.
		// Note that we can't use "slotItem?.OnUseItem()" as that bypasses the game object's life time check.
		if (slotItem != null)
		{
			slotItem.OnUseItem();
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

		// Set the slot ID on the item.
		BaseItem item = itemObject.GetComponent<BaseItem>();
		item.SlotId = slotId;

		// Update the slot with the new item.
		_slots[slotId] = item;

		// Deactivate the item object so it can no longer be interacted with.
		itemObject.SetActive(false);

		SpriteRenderer itemSpriteRenderer = itemObject.GetComponent<SpriteRenderer>();

		// Get the item slot object and sprite render.
		GameObject itemSlotObject = _slotObjects[slotId];
		SpriteRenderer itemSlotSpriteRenderer = itemSlotObject.GetComponent<SpriteRenderer>();

		// Set the item slot sprite to the item sprite.
		itemSlotSpriteRenderer.sprite = itemSpriteRenderer.sprite;

		Debug.Log($"Put item \"{_slots[slotId].name}\" in slot \"{slotId}\".");

		// Call the item's "OnPickupItem()" method.
		item.OnPickupItem();

		if (itemPickup != null)
			soundSource.Play();

		Debug.Log("It should have played!");

		Debug.Log($"Put item \"{item.name}\" in slot \"{slotId}\".");
	}

	private void DropItem(int slotId)
	{
		Debug.Log($"Dropped item \"{_slots[slotId].name}\" from slot \"{slotId}\".");

		// Get the current item object in the slot.
		GameObject droppedItemObject = _slots[slotId].gameObject;

		// Update the item slot ID to be null.
		droppedItemObject.GetComponent<BaseItem>().SlotId = null;

		// Add the item to the nearby items queue.
		_nearbyItems.Enqueue(droppedItemObject);

		// Set the position to the current player's position.
		droppedItemObject.transform.position = GameObject.Find("Player").transform.position;

		// Active the item so it can be interacted with.
		droppedItemObject.SetActive(true);

		// Update the item slot sprite to be empty.
		_slotObjects[slotId].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("HUD/ItemSlot");

		// Set the slot to null to indicate there is no item available.
		_slots[slotId] = null;
	}

	private void UpdateSelectedItemIndicator()
	{
		if (_nearbyItems.Count > 0)
		{
			GameObject itemObject = _nearbyItems.Peek();

			_selectedItemIndicatorObject.transform.position = new Vector3(itemObject.transform.position.x,
				itemObject.transform.position.y + 0.75f, itemObject.transform.position.z);

			_selectedItemIndicatorObject.SetActive(true);
		}
		else _selectedItemIndicatorObject.SetActive(false);
	}

	private static float MapValueBetween(float value, float oldMinimum, float oldMaximum, float newMinimum,
		float newMaximum) => (value - oldMinimum) / (oldMaximum - oldMinimum) * (newMaximum - newMinimum) + newMinimum;
}