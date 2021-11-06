using System.Linq;
using Scripts.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Inventory
{
	public class InventoryInformationPanelBehaviour : MonoBehaviour
	{
		private GameObject _informationPanelObject;
		private Text _textName, _textDescription;

		private void OnEnable()
		{
			_informationPanelObject = GetComponentsInChildren<RectTransform>()
				.First(x => x.name == "InformationPanel").gameObject;

			_textName = _informationPanelObject.transform.Find("Name").GetComponent<Text>();
			_textDescription = _informationPanelObject.transform.Find("Description").GetComponent<Text>();
		}

		public void Show(ItemBehaviour itemBehaviour, Vector3? position = null)
		{
			if (position.HasValue) _informationPanelObject.transform.position = position.Value;

			_textName.text = itemBehaviour.Name;
			_textDescription.text = itemBehaviour.Description;
			
			// set z to 0
			_informationPanelObject.transform.position = new Vector3
			{
				x = _informationPanelObject.transform.position.x,
				y = _informationPanelObject.transform.position.y,
				z = 0,
			};
		}

		public void Hide()
		{
			// set z to above camera
			_informationPanelObject.transform.position = new Vector3
			{
				x = _informationPanelObject.transform.position.x,
				y = _informationPanelObject.transform.position.y,
				z = -100,
			};
		}
	}
}
