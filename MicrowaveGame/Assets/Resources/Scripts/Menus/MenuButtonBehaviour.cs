using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Scripts.Audio;

namespace Scripts.Menus
{
    public class MenuButtonBehaviour : MonoBehaviour, IPointerEnterHandler, ISelectHandler
	{
		public AudioClip ButtonScroll;

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
			AudioManager.Play(ButtonScroll, 0.55f);
		}
	}
}
