using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.Menus
{
	public class MenuButtonBehaviour : MonoBehaviour, IPointerEnterHandler
	{
		public void OnPointerEnter(PointerEventData eventData)
		{
			Selectable selectable = eventData.pointerEnter.GetComponentInParent<Selectable>();
			if (selectable != null) selectable.Select();
		}
	}
}