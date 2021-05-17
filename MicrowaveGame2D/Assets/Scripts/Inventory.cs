using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
	private BaseItem[] _slots;
	private List<GameObject> _nearbyItems;

	private void Start()
	{
		_slots = new BaseItem[]
		{
			null,
			null,
			null,
			null
		};

		_nearbyItems = new List<GameObject>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!_nearbyItems.Contains(other.gameObject)) _nearbyItems.Add(other.gameObject);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (_nearbyItems.Contains(other.gameObject)) _nearbyItems.Remove(other.gameObject);
	}

	public void OnUseItem(InputAction.CallbackContext context)
	{
		if (!context.performed) return;

		int slotId = GetSlotId(context.control.path);
		BaseItem slotItem = _slots[slotId];
		if (slotItem != null) slotItem.Use();
	}

	public void OnItemInteraction(InputAction.CallbackContext context)
	{
		// Implementation is done!
		// Just need to clean up and reduce code duplication.
		
		if (!context.performed) return;

		int slotId = GetSlotId(context.control.path);
		BaseItem slotItem = _slots[slotId];

		if (slotItem == null)
		{
			Debug.Log($"OnItemInteraction(): No item in slot \"{slotId}\".");

			if (_nearbyItems.Count <= 0) return;

			Debug.Log("OnItemInteraction(): Found nearby item.");

			GameObject itemObject = _nearbyItems[0];
			_nearbyItems.RemoveAt(0);

			_slots[slotId] = itemObject.GetComponent<BaseItem>();

			Debug.Log("Is this null? " + _slots[slotId]);

			itemObject.SetActive(false);

			Debug.Log("OnItemInteraction(): Picked up nearby item.");
		}
		else
		{
			Debug.Log($"OnItemInteraction(): Found item in slot \"{slotId}\".");

			GameObject droppedItemObject = slotItem.gameObject;
			droppedItemObject.SetActive(true);
			_slots[slotId] = null;

			Debug.Log($"OnItemInteraction(): Dropped item in slot \"{slotId}\".");

			if (_nearbyItems.Count > 0)
			{
				Debug.Log("OnItemInteraction(): Found nearby item.");

				GameObject itemObject = _nearbyItems[0];
				_nearbyItems.RemoveAt(0);

				_slots[slotId] = itemObject.GetComponent<BaseItem>();
				itemObject.SetActive(false);

				Debug.Log("OnItemInteraction(): Picked up nearby item.");
			}
		}
	}

	private int GetSlotId(string raw)
	{
		string key = raw.Substring(raw.LastIndexOf('/') + 1);
		return int.TryParse(key, out int slot) ? slot - 1 : -1;
	}
}