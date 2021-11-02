using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Scripts.Audio;

namespace Scripts.Menus
{
	public class MenuSelectableBehaviour : MonoBehaviour, IPointerEnterHandler, ISelectHandler
	{
		public AudioClip ScrollAudioClip;

		public void OnPointerEnter(PointerEventData eventData)
		{
			Selectable selectable = eventData.pointerEnter.GetComponentInParent<Selectable>();
			if (selectable != null) selectable.Select();
		}

		public void OnSelect(BaseEventData eventData)
		{
			Selectable selectable = eventData.selectedObject.GetComponent<Selectable>();
			if (selectable == null) return;

			// Update the current selectable reference for the current menu.
			MenuManager.Current.CurrentSelectable = selectable;
			AudioManager.Play(ScrollAudioClip, AudioCategory.Effect, 0.55f);
		}
	}
}