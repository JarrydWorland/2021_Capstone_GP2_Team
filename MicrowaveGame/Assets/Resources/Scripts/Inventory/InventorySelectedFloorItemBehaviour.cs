using Scripts.Items;
using UnityEngine;

namespace Scripts.Inventory
{
	public class InventorySelectedFloorItemBehaviour : MonoBehaviour
	{
		private InventoryBehaviour _inventoryBehaviour;

		private GameObject _itemPickerObject;
		private InventoryInformationPanelBehaviour _inventoryInformationPanelBehaviour;

		private GameObject _playerObject;

		private void Start()
		{
			_inventoryBehaviour = GameObject.Find("Inventory").GetComponent<InventoryBehaviour>();

			_itemPickerObject = GameObject.Find("ItemPicker").gameObject;
			_inventoryInformationPanelBehaviour = GetComponentInChildren<InventoryInformationPanelBehaviour>();

			_playerObject = GameObject.Find("Player");
		}

		private void Update()
		{
			ItemBehaviour itemBehaviour = _inventoryBehaviour.GetNearbyItem();

			if (itemBehaviour != null)
			{
				Vector3 itemPosition = itemBehaviour.transform.position;
				_itemPickerObject.transform.position = itemPosition + new Vector3(0.0f, 0.75f, 0.0f);

				const float spacing = 2.75f, doubleSpacing = spacing * 2.0f, screenSpacing = 250.0f;

				Vector3 informationPanelPosition =
					itemPosition + new Vector3(_playerObject.transform.position.x > itemPosition.x ? -spacing : spacing,
						0.0f, 0.0f);

				Vector3 informationPanelScreenPosition =
					UnityEngine.Camera.main.WorldToScreenPoint(informationPanelPosition);

				if (informationPanelScreenPosition.x + screenSpacing > Screen.width)
					informationPanelPosition.x -= doubleSpacing;
				else if (informationPanelScreenPosition.x - screenSpacing < 0.0f)
					informationPanelPosition.x += doubleSpacing;

				_itemPickerObject.SetActive(true);
				_inventoryInformationPanelBehaviour.Show(itemBehaviour, informationPanelPosition);
			}
			else
			{
				_inventoryInformationPanelBehaviour.Hide();
				_itemPickerObject.SetActive(false);
			}
		}
	}
}