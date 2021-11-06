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

		private Vector2? _lastScreenSize;

		private const float MenuIndicatorLerpStrength = 12.5f;

		private void Update()
		{
			_lastScreenSize ??= new Vector2(Screen.width, Screen.height);

			Vector2 screenSize = new Vector2(Screen.width, Screen.height);
			if (_lastScreenSize != screenSize) UpdateIndicatorPosition();
			_lastScreenSize = screenSize;

			if (!_hasSelectables || _indicatorTransform == null) return;

			float unscaledDeltaTime = Time.unscaledDeltaTime;

			_indicatorTransform.position = Vector3.Lerp(_indicatorTransform.position, _indicatorPosition,
				unscaledDeltaTime * MenuIndicatorLerpStrength);

			_indicatorTransform.sizeDelta = Vector3.Lerp(_indicatorTransform.sizeDelta, _indicatorSizeDelta,
				unscaledDeltaTime * MenuIndicatorLerpStrength);
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