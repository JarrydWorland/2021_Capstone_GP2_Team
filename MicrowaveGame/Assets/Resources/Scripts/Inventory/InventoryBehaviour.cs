using System.Collections.Generic;
using System.Linq;
using Scripts.Items;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Inventory
{
	public class InventoryBehaviour : MonoBehaviour
	{
		public AudioClip PickupItemAudioClip;

		private InventorySlotBehaviour[] _slots;
		private int _currentSlotIndex;

		private List<ItemBehaviour> _nearbyItems;
		private InventoryInformationPanelBehaviour _inventoryInformationPanelBehaviour;

		private GameObject _player;

		private bool _showInformationPanel;

		private void Start()
		{
			_slots = GetComponentsInChildren<InventorySlotBehaviour>();

			_nearbyItems = new List<ItemBehaviour>();

			_inventoryInformationPanelBehaviour = GetComponentInChildren<InventoryInformationPanelBehaviour>();
			_inventoryInformationPanelBehaviour.Hide();

			_inventoryInformationPanelBehaviour.transform.position =
				_inventoryInformationPanelBehaviour.transform.position += new Vector3(0.0f, 5.75f, 0.0f);

			_player = GameObject.Find("Player");
		}

		private void Update()
		{
			if (_showInformationPanel && _slots[_currentSlotIndex].ItemBehaviour != null)
				_inventoryInformationPanelBehaviour.Show(_slots[_currentSlotIndex].ItemBehaviour);
			else _inventoryInformationPanelBehaviour.Hide();
		}

		/// <summary>
		/// Picks up an item if one is nearby.
		/// Called by the "pickup item" input action.
		/// </summary>
		/// <param name="context">The context of the input action.</param>
		public void OnPickupItem(InputAction.CallbackContext context)
		{
			if (!context.performed) return;

			ItemBehaviour selectedItem = GetNearbyItem();
			if (selectedItem == null) return;

			InventorySlotBehaviour nextAvailableSlot = _slots.FirstOrDefault(x => x.ItemBehaviour == null);

			if (nextAvailableSlot == null)
			{
				// All inventory slots are taken.
				// Swap the current item with the nearby item.

				OnDropItem(context);
				OnPickupItem(context);
			}
			else
			{
				// Otherwise pickup the item into the next available slot.

				nextAvailableSlot.PickupItem(selectedItem);
				selectedItem.gameObject.SetActive(false);
			}

			AudioManager.Play(PickupItemAudioClip);
		}

		/// <summary>
		/// Drops an item if one is in the slot.
		/// Called by the "drop item" input action.
		/// </summary>
		/// <param name="context">The context of the input action.</param>
		public void OnDropItem(InputAction.CallbackContext context)
		{
			if (!context.performed) return;

			ItemBehaviour itemBehaviour = _slots[_currentSlotIndex].DropItem();
			if (itemBehaviour == null) return;

			itemBehaviour.transform.position = _player.transform.position;
			itemBehaviour.gameObject.SetActive(true);
		}

		/// <summary>
		/// Attempts to use the item in the current slot, if any.
		/// </summary>
		/// <param name="context">The context of the input action.</param>
		public void OnUseItem(InputAction.CallbackContext context)
		{
			if (!context.performed) return;
			_slots[_currentSlotIndex].UseItem();
		}

		/// <summary>
		/// Shows / hides the information panel for the currently selected item.
		/// </summary>
		/// <param name="context">The context of the input action.</param>
		public void OnViewItem(InputAction.CallbackContext context)
		{
			if (context.performed) _showInformationPanel = true;
			else if (context.canceled) _showInformationPanel = false;
		}

		/// <summary>
		/// Switches the currently selected item.
		/// Called by the "switch item" input action.
		/// </summary>
		/// <param name="context">The context of the input action.</param>
		public void OnSwitchItem(InputAction.CallbackContext context)
		{
			if (!context.performed || _slots.All(x => x.ItemBehaviour == null)) return;

			Vector2 direction = context.ReadValue<Vector2>();
			int lastSlotIndex = _currentSlotIndex;

			do
			{
				_currentSlotIndex += (int) direction.x;

				if (_currentSlotIndex < 0) _currentSlotIndex = _slots.Length - 1;
				else if (_currentSlotIndex >= _slots.Length) _currentSlotIndex = 0;
			} while (_slots[_currentSlotIndex].ItemBehaviour == null);

			Log.Info($"Last slot: {lastSlotIndex}, Current slot: {_currentSlotIndex}");
		}

		/// <summary>
		/// Gets the item at the top of the queue (i.e. the selected item).
		/// </summary>
		/// <returns>The item at the top of the queue.</returns>
		public ItemBehaviour GetNearbyItem()
		{
			ItemBehaviour closestItem = null;
			float closestDistance = float.MaxValue;

			foreach (ItemBehaviour item in _nearbyItems)
			{
				float distance = Vector3.Distance(_player.transform.position, item.transform.position);

				if (distance < closestDistance)
				{
					closestItem = item;
					closestDistance = distance;
				}
			}

			return closestItem;
		}

		/// <summary>
		/// Adds an item to the nearby item list.
		/// </summary>
		/// <param name="itemBehaviour">The item to be added.</param>
		public void AddNearbyItem(ItemBehaviour itemBehaviour)
		{
			if (itemBehaviour != null && !_nearbyItems.Contains(itemBehaviour)) _nearbyItems.Add(itemBehaviour);
		}

		/// <summary>
		/// Removes an item from the nearby item list.
		/// </summary>
		/// <param name="itemBehaviour">The item to be removed.</param>
		public void RemoveNearbyItem(ItemBehaviour itemBehaviour)
		{
			if (itemBehaviour != null && _nearbyItems.Contains(itemBehaviour)) _nearbyItems.Remove(itemBehaviour);
		}
	}
}