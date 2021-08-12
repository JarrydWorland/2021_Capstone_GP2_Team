using UnityEngine.SceneManagement;
using UnityEngine;

namespace Scripts.Menus
{
	public class MenuDeathBehaviour : MenuBehaviour
	{
		/// <summary>
		/// Sets the timeScale to 0.1f so that the game continues in the
		/// background.
		/// </summary>
		public override void OnEnter() => Time.timeScale = 0.1f;

		/// <summary>
		/// Sets the current scene to the "Gameplay" scene.
		/// Called when the "Try Again" button is pressed.
		/// </summary>
		public void OnAgainButtonPressed() => SceneManager.LoadScene("Gameplay");

		/// <summary>
		/// Sets the current scene to the "Menu" scene.
		/// Called when the "Exit To Main Menu" button is pressed.
		/// </summary>
		public void OnExitButtonPressed() => sceneFader.FadeTo("Menu");
	}
}
