using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Menus
{
	public class MenuPausedBehaviour : MenuBehaviour
	{
		/// <summary>
		/// Sets the current menu to the previous menu.
		/// Called when the "Done" button is pressed.
		/// </summary>
		public void OnResumeButtonPressed()
		{
			MenuManager.GoBack();
			Time.timeScale = 1.0f;
		}

		/// <summary>
		/// Sets the current menu to the "Controls" menu.
		/// Called when the "Controls" button is pressed.
		/// </summary>
		public void OnControlsButtonPressed()
		{
			MenuControlsBehaviour controlsMenu =
				transform.parent.Find("MenuControls").GetComponent<MenuControlsBehaviour>();

			MenuManager.GoInto(controlsMenu);
		}

		/// <summary>
		/// Sets the current scene to the "Menu" scene.
		/// Called when the "Exit To Main Menu" button is pressed.
		/// </summary>
		public void OnExitButtonPressed() => SceneManager.LoadScene("Menu");
	}
}