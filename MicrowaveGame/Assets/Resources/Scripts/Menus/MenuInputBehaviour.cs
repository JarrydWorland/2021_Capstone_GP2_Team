using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.Menus
{
	public class MenuInputBehaviour : MonoBehaviour
	{
		public AudioClip ScrollAudioClip;

		/// <summary>
		/// Called by unity's input system when a navigational button (D-Pad, WASD keys, etc.) is pressed.
		/// </summary>
		/// <param name="context">The callback context.</param>
		public void OnNavigate(InputAction.CallbackContext context)
		{
			if (!SceneManager.GetActiveScene().isLoaded) return;
			if (!context.performed || MenuManager.Current.name == "MenuPlaying") return;

			Vector2 direction = context.ReadValue<Vector2>();

			// If we navigate left or right while a slider is selected, update the slider value.
			if (MenuManager.Current.CurrentSelectable is Slider slider && Math.Abs(direction.x) > Math.Abs(direction.y))
			{
				const float incrementValue = 0.05f;
				slider.value = (float) Math.Round(slider.value + (direction.x < 0 ? -incrementValue : incrementValue),
					2);

				return;
			}

			Selectable nextSelectable = MenuManager.Current.CurrentSelectable.FindSelectable(direction);
			if (nextSelectable == null) return;

			nextSelectable.Select();
			MenuManager.Current.CurrentSelectable = nextSelectable;

			AudioManager.Play(ScrollAudioClip, AudioCategory.Effect, 0.55f);
		}

		/// <summary>
		/// Called by unity's input system when a submit button is pressed.
		/// </summary>
		/// <param name="context">The callback context.</param>
		public void OnSubmit(InputAction.CallbackContext context)
		{
			if (!SceneManager.GetActiveScene().isLoaded) return;
			if (!context.performed || MenuManager.Current.name == "MenuPlaying") return;

			bool isPressed = context.ReadValueAsButton();

			if (isPressed && MenuManager.Current.CurrentSelectable is Button button)
			{
				PointerEventData pointerData = GetMousePointerEventData();
				button.OnPointerClick(pointerData);

				AudioManager.Play(ScrollAudioClip, AudioCategory.Effect, 0.55f);
			}
		}

		/// <summary>
		/// Called by unity's input system when a cancel button is pressed.
		/// </summary>
		/// <param name="context">The callback context.</param>
		public void OnCancel(InputAction.CallbackContext context)
		{
			if (!SceneManager.GetActiveScene().isLoaded) return;
			if (!context.performed) return;

			if (Keyboard.current.escapeKey.wasPressedThisFrame)
			{
				if (MenuManager.Current.name == "MenuPlaying")
				{
					MenuManager.GoInto("MenuPaused");
					return;
				}

				if (MenuManager.Current.name == "MenuPaused")
				{
					MenuManager.GoInto("MenuPlaying");
					return;
				}
			}

			if (MenuManager.Current.name == "MenuControls" || MenuManager.Current.name == "MenuCredits" ||
			    MenuManager.Current.name == "MenuSettings" || MenuManager.Current.name == "MenuSounds")
			{
				string lastMenuName = MenuManager.Current.name;

				MenuManager.GoBack();

				if (MenuManager.Current.name != lastMenuName)
					AudioManager.Play(ScrollAudioClip, AudioCategory.Effect, 0.55f);
			}
		}

		/// <summary>
		/// Called by unity's input system when the mouse is moved.
		/// </summary>
		/// <param name="context">The callback context.</param>
		public void OnPoint(InputAction.CallbackContext context)
		{
			if (!SceneManager.GetActiveScene().isLoaded) return;
			if (!context.performed || MenuManager.Current.name == "MenuPlaying") return;

			if (MenuManager.Current.CurrentSelectable is Slider currentSlider && Mouse.current.leftButton.isPressed)
			{
				PointerEventData pointerEventData = GetMousePointerEventData();
				currentSlider.OnDrag(pointerEventData);

				return;
			}

			Selectable selectable = GetSelectablesAtMousePosition().FirstOrDefault();
			if (selectable == null) return;

			if (MenuManager.Current.CurrentSelectable != selectable)
			{
				selectable.Select();
				MenuManager.Current.CurrentSelectable = selectable;

				AudioManager.Play(ScrollAudioClip, AudioCategory.Effect, 0.55f);
			}

			if (selectable is Slider slider)
			{
				PointerEventData pointerEventData = GetMousePointerEventData();
				if (Mouse.current.leftButton.isPressed) slider.OnDrag(pointerEventData);
			}
		}

		/// <summary>
		/// Called by unity's input system when a mouse button is pressed or released.
		/// </summary>
		/// <param name="context">The callback context.</param>
		public void OnClick(InputAction.CallbackContext context)
		{
			if (!SceneManager.GetActiveScene().isLoaded) return;
			if (!context.performed || MenuManager.Current.name == "MenuPlaying") return;

			Selectable selectable = GetSelectablesAtMousePosition().FirstOrDefault();
			if (selectable == null) return;

			bool isPressed = (int) context.ReadValue<float>() == 1;
			PointerEventData pointerEventData = GetMousePointerEventData();

			if (selectable is Button button && isPressed)
			{
				button.OnPointerClick(pointerEventData);
				AudioManager.Play(ScrollAudioClip, AudioCategory.Effect, 0.55f);
			}
			else if (selectable is Slider slider)
			{
				if (isPressed) slider.OnPointerDown(pointerEventData);
				else slider.OnPointerUp(pointerEventData);
			}
		}

		/// <summary>
		/// Called by unity's input system when the controller pause button is pressed.
		/// </summary>
		public void OnPauseGamepad(InputAction.CallbackContext context)
		{
			if (!SceneManager.GetActiveScene().isLoaded) return;

			if (context.performed)
			{
				if (MenuManager.Current.name == "MenuPlaying") MenuManager.GoInto("MenuPaused");
				else if (MenuManager.Current.name == "MenuPaused") MenuManager.GoBack();
			}
		}

		private Selectable[] GetSelectablesAtMousePosition()
		{
			PointerEventData pointerData = GetMousePointerEventData();

			List<RaycastResult> raycastResults = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerData, raycastResults);

			List<Selectable> selectables = new List<Selectable>();

			foreach (RaycastResult raycastResult in raycastResults)
			{
				Selectable selectable = raycastResult.gameObject.GetComponentInParent<Selectable>();
				if (selectable != null) selectables.Add(selectable);
			}

			return selectables.ToArray();
		}

		/// <summary>
		/// A modified version of <see cref="PointerInputModule"/>'s GetMousePointerEventData() method.
		/// </summary>
		/// <remarks>This does not include middle mouse or right mouse data.</remarks>
		/// <returns>The current mouse's pointer event data.</returns>
		private PointerEventData GetMousePointerEventData()
		{
			Mouse currentMouse = Mouse.current;

			PointerEventData pointerEventData = new PointerEventData(null)
			{
				pointerId = currentMouse.deviceId,
				position = currentMouse.position.ReadValue()
			};

			pointerEventData.Reset();

			Vector2 currentPosition = pointerEventData.position;

			if (Cursor.lockState == CursorLockMode.Locked)
			{
				pointerEventData.position = new Vector2(-1.0f, -1.0f);
				pointerEventData.delta = Vector2.zero;
			}
			else
			{
				pointerEventData.delta = currentPosition - pointerEventData.position;
				pointerEventData.position = currentPosition;
			}

			pointerEventData.scrollDelta = Mouse.current.scroll.ReadValue();
			pointerEventData.button = PointerEventData.InputButton.Left;

			List<RaycastResult> raycastResult = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerEventData, raycastResult);

			var raycast = FindFirstRaycast(raycastResult);
			pointerEventData.pointerCurrentRaycast = raycast;

			raycastResult.Clear();

			// I can't find a nice way to replicate this behaviour here.
			// We aren't using this information anyway, so it's okay for now.
			// m_MouseState.SetButtonState(PointerEventData.InputButton.Left, StateForMouseButton(0), leftData);

			return pointerEventData;
		}

		/// <summary>
		/// A carbon copy of <see cref="PointerInputModule"/>'s FindFirstRaycast() method.
		/// </summary>
		/// <param name="candidates">A list of raycast results to search through.</param>
		/// <returns>The first raycast result with a non-null game object.</returns>
		private static RaycastResult FindFirstRaycast(List<RaycastResult> candidates)
		{
			var candidatesCount = candidates.Count;

			for (var i = 0; i < candidatesCount; ++i)
			{
				if (candidates[i].gameObject == null) continue;
				return candidates[i];
			}

			return new RaycastResult();
		}
	}
}