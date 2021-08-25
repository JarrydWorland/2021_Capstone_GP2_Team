using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Scripts.Menus
{
	public class MenuInputBehaviour : MonoBehaviour
	{
		/// <summary>
		/// Called by unity's input system when a navigational button (D-Pad, WASD keys, etc.) is pressed.
		/// </summary>
		public void OnNavigate(InputAction.CallbackContext context)
		{
			if (!context.performed || MenuManager.Current.name == "MenuPlaying") return;

			Vector2 direction = context.ReadValue<Vector2>();

			Selectable nextSelectable = MenuManager.Current.CurrentSelectable.FindSelectable(direction);
			if (nextSelectable == null) return;

			MenuManager.Current.CurrentSelectable = nextSelectable;
			MenuManager.Current.CurrentSelectable.Select();
		}

		/// <summary>
		/// Called by unity's input system when the back button is pressed.
		/// </summary>
		public void OnCancel(InputAction.CallbackContext context)
		{
			if (!context.performed) return;

			if (MenuManager.Current.name == "MenuControls" || MenuManager.Current.name == "MenuCredits" ||
			    MenuManager.Current.name == "MenuPaused")
			{
				MenuManager.GoBack();
			}
		}

		/// <summary>
		/// Called by unity's input system when the pause button is pressed.
		/// </summary>
		public void OnPause(InputAction.CallbackContext context)
		{
			if (MenuManager.Current.name == "MenuPlaying") MenuManager.GoInto("MenuPaused");
			else if (MenuManager.Current.name == "MenuPaused") MenuManager.GoBack();
		}
	}
}