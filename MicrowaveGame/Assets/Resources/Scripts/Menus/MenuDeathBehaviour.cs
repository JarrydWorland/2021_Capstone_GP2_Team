using Scripts.Scenes;
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
		public override void OnEnter()
		{
			base.OnEnter();

			// This menu is only ever entered via the "MenuPlaying" menu which calls
			// "GameState.Pause()" in its "OnLeave()" method, thus no need to call it here.

			Time.timeScale = 0.1f;
		}

		/// <summary>
		/// Sets the current scene to the "Gameplay" scene.
		/// Called when the "Try Again" button is pressed.
		/// </summary>
		public void OnAgainButtonPressed() => SceneManager.LoadScene("Gameplay");

		/// <summary>
		/// Sets the current scene to the "Menu" scene.
		/// Called when the "Exit To Main Menu" button is pressed.
		/// </summary>
		public void OnExitButtonPressed() => SceneFaderBehaviour.Instance.FadeInto("Menu");
	}
}