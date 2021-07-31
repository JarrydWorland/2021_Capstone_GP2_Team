using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Menus
{
	public class MenuPlayingBehaviour : MenuBehaviour
	{
		private void Start()
		{
			Time.timeScale = 1.0f;
			MenuManager.Init(this);
		}

		/// <summary>
		/// Called via Unity's new input system when the user presses the "Escape" key.
		/// Sets the current menu to the "Controls" menu.
		/// Called when the "Controls" button is pressed.
		/// </summary>
		/// <param name="context"></param>
		public void OnPause(InputAction.CallbackContext context)
		{
			if (!context.performed) return;

			Time.timeScale = 0.0f;
			
			MenuPausedBehaviour pausedMenu =
				transform.parent.Find("MenuPaused").GetComponent<MenuPausedBehaviour>();

			MenuManager.GoInto(pausedMenu);
		}
	}
}