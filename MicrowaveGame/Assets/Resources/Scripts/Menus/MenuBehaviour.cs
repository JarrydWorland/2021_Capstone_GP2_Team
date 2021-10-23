using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.Menus
{
    /// <summary>
    /// The base menu behaviour to extend off of.
    /// </summary>
    public abstract class MenuBehaviour : MonoBehaviour
	{

		public Selectable CurrentSelectable { get; set; }

		private Transform _buttonIndicatorTransform;

		private void Update()
		{
			if (_buttonIndicatorTransform != null)
			{
				_buttonIndicatorTransform.position = Vector3.Lerp(_buttonIndicatorTransform.position, CurrentSelectable.transform.position, 0.075f);
			}
		}

		/// <summary>
		/// Called when entering the menu.
		/// </summary>
		public virtual void OnEnter()
		{
			CurrentSelectable ??= GetComponentsInChildren<Selectable>().FirstOrDefault();

			if (CurrentSelectable != null)
			{
				InitButtonIndicator(CurrentSelectable.transform.position);
				CurrentSelectable.Select();
			}
		}

		/// <summary>
		/// Called when leaving the menu.
		/// </summary>
		public virtual void OnLeave()
		{
			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;

			if (currentSelectedGameObject != null)
				CurrentSelectable = currentSelectedGameObject.GetComponent<Selectable>();
		}

		/// <summary>
		/// Called when returning to a menu via GoBack().
		/// </summary>
		public virtual void OnReturn()
		{
			if (CurrentSelectable != null) CurrentSelectable.Select();
		}

		private void InitButtonIndicator(Vector3 initialPosition)
		{
			if (_buttonIndicatorTransform != null) return;

			_buttonIndicatorTransform = transform.Find("MenuButtonIndicator");
			if (_buttonIndicatorTransform == null) return;

			Button[] buttons = GetComponentsInChildren<Button>();
			float maximumButtonWidth = float.MinValue;

			foreach (Button button in buttons)
			{
				float width = button.GetComponent<RectTransform>().rect.width;
				if (width > maximumButtonWidth) maximumButtonWidth = width;
			}

			Vector3 offset = new Vector3(maximumButtonWidth / 2.0f, 0.0f, 0.0f);

			_buttonIndicatorTransform.Find("ImageLeft").GetComponent<RectTransform>().localPosition -= offset;
			_buttonIndicatorTransform.Find("ImageRight").GetComponent<RectTransform>().localPosition += offset;
			_buttonIndicatorTransform.position = initialPosition;
		}
	}
}
