using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
			if (!SceneManager.GetActiveScene().isLoaded) return;
			if (!context.performed || MenuManager.Current.name == "MenuPlaying") return;

			Vector2 direction = context.ReadValue<Vector2>();

			Selectable nextSelectable = MenuManager.Current.CurrentSelectable.FindSelectable(direction);
			if (nextSelectable == null) return;

			nextSelectable.Select();
			MenuManager.Current.CurrentSelectable = nextSelectable;
		}

		/// <summary>
		/// Called by unity's input system when the pause button is pressed.
		/// </summary>
		public void OnPause(InputAction.CallbackContext context)
		{
			if (!SceneManager.GetActiveScene().isLoaded) return;
			if (context.performed)
            {
				if (MenuManager.Current.name == "MenuPlaying") MenuManager.GoInto("MenuPaused");
				else if (MenuManager.Current.name == "MenuControls" || MenuManager.Current.name == "MenuCredits" ||  MenuManager.Current.name == "MenuPaused") MenuManager.GoBack();
			}
		}
	}
}
