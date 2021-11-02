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
		private Selectable _currentSelectable;

		/// <summary>
		/// The currently selected object.
		/// </summary>
		public Selectable CurrentSelectable
		{
			get => _currentSelectable;
			set
			{
				_currentSelectable = value;
				UpdateIndicatorPosition();
			}
		}

		private RectTransform _indicatorTransform;
		private RectTransform _indicatorImageLeftTransform;
		private RectTransform _indicatorImageRightTransform;

		private Vector3 _indicatorPosition;
		private Vector3 _indicatorImageLeftPosition;
		private Vector3 _indicatorImageRightPosition;

		private void Update()
		{
			if (_indicatorTransform != null)
				_indicatorTransform.position = Vector3.Lerp(_indicatorTransform.position, _indicatorPosition, 0.075f);

			if (_indicatorImageLeftTransform != null)
			{
				_indicatorImageLeftTransform.localPosition = Vector3.Lerp(_indicatorImageLeftTransform.localPosition,
					_indicatorImageLeftPosition, 0.075f);
			}

			if (_indicatorImageRightTransform != null)
			{
				_indicatorImageRightTransform.localPosition = Vector3.Lerp(_indicatorImageRightTransform.localPosition,
					_indicatorImageRightPosition, 0.075f);
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
				CurrentSelectable.Select();
				UpdateIndicatorPosition(true);
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
			if (CurrentSelectable != null)
			{
				CurrentSelectable.Select();
				UpdateIndicatorPosition(true);
			}
		}

		private void UpdateIndicatorPosition(bool immediate = false)
		{
			if (_indicatorTransform == null)
				_indicatorTransform = transform.Find("MenuSelectableIndicator").GetComponent<RectTransform>();

			if (_indicatorImageLeftTransform == null)
				_indicatorImageLeftTransform = _indicatorTransform.Find("ImageLeft").GetComponent<RectTransform>();

			if (_indicatorImageRightTransform == null)
				_indicatorImageRightTransform = _indicatorTransform.Find("ImageRight").GetComponent<RectTransform>();

			RectTransform currentSelectableTransform = _currentSelectable.GetComponent<RectTransform>();

			Vector3 offset = new Vector3(
				currentSelectableTransform.rect.width / 2.0f * currentSelectableTransform.localScale.x,
				0.0f,
				0.0f);

			if (immediate)
			{
				_indicatorTransform.position = CurrentSelectable.transform.position;
				_indicatorImageLeftTransform.localPosition = _indicatorImageLeftPosition;
				_indicatorImageRightTransform.localPosition = _indicatorImageRightPosition;
			}
			else
			{
				_indicatorPosition = CurrentSelectable.transform.position;
				_indicatorImageLeftPosition = currentSelectableTransform.position - offset;
				_indicatorImageRightPosition = currentSelectableTransform.position + offset;
			}
		}
	}
}