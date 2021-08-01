using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Menus
{
	public class MenuMainBehaviour : MenuBehaviour
	{
		private void Start()
		{
			Time.timeScale = 1.0f;
			MenuManager.Init(this);
		}

		/// <summary>
		/// Sets the current scene to the "Gameplay" scene.
		/// Called when the "Start" button is pressed.
		/// </summary>
		public void OnStartButtonPressed() => SceneManager.LoadScene("Gameplay");

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
		/// Sets the current menu to the "Credits" menu.
		/// Called when the "Credits" button is pressed.
		/// </summary>
		public void OnCreditsButtonPressed()
		{
			MenuCreditsBehaviour creditsMenu =
				transform.parent.Find("MenuCredits").GetComponent<MenuCreditsBehaviour>();

			MenuManager.GoInto(creditsMenu);
		}

		/// <summary>
		/// Quits the game or exits the playing mode of the editor depending on the current executing context.
		/// Called when the "Quit" button is pressed.
		/// </summary>
		public void OnQuitButtonPressed()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}