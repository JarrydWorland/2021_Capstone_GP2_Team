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

			// Enable the indicators for the selected button.
			SetButtonState(button, true);

			// Disable the indicators for the previously selected button.
			if (MenuManager.Current.CurrentSelectable is Button previousButton && previousButton != button)
				SetButtonState(previousButton, false);

			// Update the currently selected button reference for the current menu.
			MenuManager.Current.CurrentSelectable = button;
		}

		private void SetButtonState(Button button, bool state)
		{
			Image[] images = button.GetComponentsInChildren<Image>();
			foreach (Image image in images) image.enabled = state;
		}
	}
}