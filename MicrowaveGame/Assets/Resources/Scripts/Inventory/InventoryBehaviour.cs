using Scripts.Items;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Inventory
{
	public class InventoryBehaviour : MonoBehaviour
	{
		private InventorySlotBehaviour[] _slots;
		private CircularQueue<ItemBehaviour> _nearbyItems;

		private GameObject _player;

		private void Start()
		{
			_slots = GetComponentsInChildren<InventorySlotBehaviour>();
			_nearbyItems = new CircularQueue<ItemBehaviour>();
			_player = GameObject.Find("Player");
		}

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

		private void OnSelectItem(InputAction.CallbackContext context, bool reverse)
		{
			if (!context.performed || _nearbyItems.Count == 0) return;
			_nearbyItems.Requeue(reverse);
		}

		private void OnPickupDropItem(InputAction.CallbackContext context, int slotId)
		{
			if (!context.performed) return;

			InventorySlotBehaviour inventorySlotBehaviour = _slots[slotId];

			if (inventorySlotBehaviour.ItemBehaviour == null)
			{
				ItemBehaviour selectedItem = _nearbyItems.Peek();
				inventorySlotBehaviour.PickupItem(selectedItem);
				selectedItem.gameObject.SetActive(false);
			}
			else
			{
				ItemBehaviour itemBehaviour = _slots[slotId].DropItem();
				itemBehaviour.transform.position = _player.transform.position;
				itemBehaviour.gameObject.SetActive(true);
			}
		}

		private void OnUseItem(InputAction.CallbackContext context, int slotId)
		{
			if (!context.performed || Keyboard.current.shiftKey.isPressed) return;
			_slots[slotId].UseItem();
		}

		public void AddNearbyItem(ItemBehaviour itemBehaviour)
		{
			if (itemBehaviour != null && !_nearbyItems.Contains(itemBehaviour)) _nearbyItems.Enqueue(itemBehaviour);
		}

		public void RemoveNearbyItem(ItemBehaviour itemBehaviour)
		{
			if (itemBehaviour != null && _nearbyItems.Contains(itemBehaviour)) _nearbyItems.Remove(itemBehaviour);
		}
	}
}