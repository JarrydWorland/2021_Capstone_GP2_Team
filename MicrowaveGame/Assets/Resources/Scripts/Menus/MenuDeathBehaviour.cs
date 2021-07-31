using UnityEngine.SceneManagement;

namespace Scripts.Menus
{
	public class MenuDeathBehaviour : MenuBehaviour
	{
		/// <summary>
		/// Sets the current scene to the "Gameplay" scene.
		/// Called when the "Try Again" button is pressed.
		/// </summary>
		public void OnAgainButtonPressed() => SceneManager.LoadScene("Gameplay");

		/// <summary>
		/// Sets the current scene to the "Menu" scene.
		/// Called when the "Exit To Main Menu" button is pressed.
		/// </summary>
		public void OnExitButtonPressed() => SceneManager.LoadScene("Menu");
	}
}