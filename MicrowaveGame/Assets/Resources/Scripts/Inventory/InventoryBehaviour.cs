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
		private CircularQueue<ItemBehaviour> _nearbyItems;

		private GameObject _player;

		private void Start()
		{
			_slots = GetComponentsInChildren<InventorySlotBehaviour>();
			_nearbyItems = new CircularQueue<ItemBehaviour>();
			_player = Extensions.FindInactiveObjectByName("Player");
		}

		// --- The below methods are called by Unity's new input system.

		public void OnSelectItemLeft(InputAction.CallbackContext context) => OnSelectItem(context, false);
		public void OnSelectItemRight(InputAction.CallbackContext context) => OnSelectItem(context, true);

		public void OnPickupDropItemSlotOne(InputAction.CallbackContext context) => OnPickupDropItem(context, 0);
		public void OnPickupDropItemSlotTwo(InputAction.CallbackContext context) => OnPickupDropItem(context, 1);
		public void OnPickupDropItemSlotThree(InputAction.CallbackContext context) => OnPickupDropItem(context, 2);
		public void OnPickupDropItemSlotFour(InputAction.CallbackContext context) => OnPickupDropItem(context, 3);

		public void OnUseItemSlotOne(InputAction.CallbackContext context) => OnUseItem(context, 0);
		public void OnUseItemSlotTwo(InputAction.CallbackContext context) => OnUseItem(context, 1);
		public void OnUseItemSlotThree(InputAction.CallbackContext context) => OnUseItem(context, 2);
		public void OnUseItemSlotFour(InputAction.CallbackContext context) => OnUseItem(context, 3);

		// ---

		/// <summary>
		/// Changes the currently selected item on the floor.
		/// Called by the "select item" input action.
		/// </summary>
		/// <param name="context">The context of the input action.</param>
		/// <param name="reverse">Rotates the circular queue clockwise if true, or counter-clockwise if false.</param>
		private void OnSelectItem(InputAction.CallbackContext context, bool reverse)
		{
			if (!context.performed || _nearbyItems.Count == 0) return;
			_nearbyItems.Requeue(reverse);
		}

		/// <summary>
		/// Picks up an item if one is nearby, or drops an item if one is in the slot.
		/// Called by the "pick up / drop item" input action.
		/// </summary>
		/// <param name="context">The context of the input action.</param>
		/// <param name="slotId">The ID of the slot.</param>
		private void OnPickupDropItem(InputAction.CallbackContext context, int slotId)
		{
			if (!context.performed) return;

			InventorySlotBehaviour inventorySlotBehaviour = _slots[slotId];
			ItemBehaviour selectedItem = _nearbyItems.Peek();

			if (inventorySlotBehaviour.ItemBehaviour == null)
			{
				if (selectedItem == null) return;

				inventorySlotBehaviour.PickupItem(selectedItem);
				AudioManager.Play(PickupItemAudioClip);
				selectedItem.gameObject.SetActive(false);
			}
			else
			{
				ItemBehaviour itemBehaviour = _slots[slotId].DropItem();
				if (itemBehaviour == null) return;

				itemBehaviour.transform.position = _player.transform.position;
				itemBehaviour.gameObject.SetActive(true);

				if (selectedItem != null)
				{
					inventorySlotBehaviour.PickupItem(selectedItem);
					selectedItem.gameObject.SetActive(false);
				}
			}
		}

		/// <summary>
		/// Attempts to use an item in the given slot.
		/// </summary>
		/// <param name="context">The context of the input action.</param>
		/// <param name="slotId">The ID of the slot.</param>
		private void OnUseItem(InputAction.CallbackContext context, int slotId)
		{
			if (!context.performed || Keyboard.current.shiftKey.isPressed) return;
			_slots[slotId].UseItem();
		}

		/// <summary>
		/// Gets the item at the top of the queue (i.e. the selected item).
		/// </summary>
		/// <returns>The item at the top of the queue.</returns>
		public ItemBehaviour GetNearbyItem() => _nearbyItems.Peek();

		/// <summary>
		/// Adds an item to the nearby item list.
		/// </summary>
		/// <param name="itemBehaviour">The item to be added.</param>
		public void AddNearbyItem(ItemBehaviour itemBehaviour)
		{
			if (itemBehaviour != null && !_nearbyItems.Contains(itemBehaviour)) _nearbyItems.Enqueue(itemBehaviour);
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
