using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.Menus
{
	public class MenuButtonBehaviour : MonoBehaviour, IPointerEnterHandler, ISelectHandler
	{
		public void OnPointerEnter(PointerEventData eventData)
		{
			Button button = eventData.pointerEnter.GetComponentInParent<Button>();
			if (button != null) button.Select();
		}

		public void OnSelect(BaseEventData eventData)
		{
			Button button = eventData.selectedObject.GetComponent<Button>();
			if (button == null) return;

			// Update the currently selected button reference for the current menu.
			MenuManager.Current.CurrentSelectable = button;

			// Update the button indicators if the menu has them.
			if (MenuManager.Current.ButtonIndicatorPosition != null)
			{
				MenuManager.Current.ButtonIndicatorPosition.Value = MenuManager.Current.CurrentSelectable.transform.position;
			}
		}
	}
}