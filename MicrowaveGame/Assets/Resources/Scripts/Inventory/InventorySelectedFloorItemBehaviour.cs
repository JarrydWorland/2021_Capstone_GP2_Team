using Scripts.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Inventory
{
	public class InventorySelectedFloorItemBehaviour : MonoBehaviour
	{
		private InventoryBehaviour _inventoryBehaviour;

		private GameObject _itemPickerObject;

		private GameObject _informationPanelObject;
		private Text _textName, _textDescription;

		private GameObject _playerObject;

		private void Start()
		{
			_inventoryBehaviour = GameObject.Find("Inventory").GetComponent<InventoryBehaviour>();

			_itemPickerObject = GameObject.Find("ItemPicker").gameObject;
			_informationPanelObject = GameObject.Find("InformationPanel").gameObject;

			_textName = _informationPanelObject.transform.Find("Name").GetComponent<Text>();
			_textDescription = _informationPanelObject.transform.Find("Description").GetComponent<Text>();

			_playerObject = GameObject.Find("Player");
		}

		private void Update()
		{
			ItemBehaviour itemBehaviour = _inventoryBehaviour.GetNearbyItem();

			if (itemBehaviour != null)
			{
				Vector3 itemPosition = itemBehaviour.transform.position;

				transform.position = itemPosition + new Vector3(0.0f, 0.75f, 0.0f);

				float informationPanelPosition = _playerObject.transform.position.x > itemPosition.x ? -2.75f : 2.75f;

				_informationPanelObject.transform.position =
					itemPosition + new Vector3(informationPanelPosition, 0.0f, 0.0f);

				_textName.text = itemBehaviour.Name;
				_textDescription.text = itemBehaviour.Description;

				_itemPickerObject.SetActive(true);
				_informationPanelObject.SetActive(true);
			}
			else
			{
				_itemPickerObject.SetActive(false);
				_informationPanelObject.SetActive(false);
			}
		}
	}
}