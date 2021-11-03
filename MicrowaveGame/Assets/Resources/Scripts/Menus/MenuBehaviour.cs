using System.Linq;
using TMPro;
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
				if (value == null) return;

				_currentSelectable = value;
				UpdateIndicatorPosition();
			}
		}

		private RectTransform _indicatorTransform;

		private Vector3 _indicatorPosition;
		private Vector2 _indicatorSizeDelta;

		private bool _hasSelectables;

		private void Update()
		{
			if (!_hasSelectables || _indicatorTransform == null) return;

			_indicatorTransform.position = Vector3.Lerp(_indicatorTransform.position, _indicatorPosition, 0.075f);
			_indicatorTransform.sizeDelta = Vector3.Lerp(_indicatorTransform.sizeDelta, _indicatorSizeDelta, 0.075f);
		}

		/// <summary>
		/// Called when entering the menu.
		/// </summary>
		public virtual void OnEnter()
		{
			_hasSelectables = GetComponentsInChildren<Selectable>().Length > 0;
			if (!_hasSelectables) return;

			CurrentSelectable ??= GetComponentsInChildren<Selectable>().First();
			CurrentSelectable.Select();

			UpdateIndicatorPosition(true);
		}

		/// <summary>
		/// Called when leaving the menu.
		/// </summary>
		public virtual void OnLeave()
		{
			if (!_hasSelectables) return;

			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
			CurrentSelectable = currentSelectedGameObject.GetComponent<Selectable>();
		}

		/// <summary>
		/// Called when returning to a menu via GoBack().
		/// </summary>
		public virtual void OnReturn()
		{
			if (!_hasSelectables) return;

			CurrentSelectable.Select();
			UpdateIndicatorPosition(true);
		}

		private void UpdateIndicatorPosition(bool immediate = false)
		{
			if (!_hasSelectables) return;

			if (_indicatorTransform == null)
				_indicatorTransform = transform.Find("MenuSelectableIndicator").GetComponent<RectTransform>();

			RectTransform currentSelectableTransform = _currentSelectable.GetComponent<RectTransform>();

			const float padding = 50.0f;

			if (immediate)
			{
				_indicatorTransform.position = CurrentSelectable.transform.position;

				_indicatorTransform.sizeDelta = new Vector2(
					currentSelectableTransform.rect.width * currentSelectableTransform.localScale.x + padding,
					_indicatorTransform.rect.height);
			}
			else
			{
				_indicatorPosition = CurrentSelectable.transform.position;

				_indicatorSizeDelta = new Vector2(
					currentSelectableTransform.rect.width * currentSelectableTransform.localScale.x + padding,
					_indicatorTransform.rect.height);
			}
		}
	}
}