using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Utilities
{
	public static class GameState
	{
		/// <summary>
		/// Freezes the time scale (= 0.0f) and disables player input (moving, looking, etc.).
		/// </summary>
		public static void Pause()
		{
			Time.timeScale = 0.0f;

			GameObject.Find("Player").GetComponent<PlayerInput>().currentActionMap.Disable();
			GameObject.Find("Inventory").GetComponent<PlayerInput>().currentActionMap.Disable();
		}

		/// <summary>
		/// Unfreezes the time scale (= 1.0f) and enables player input (moving, looking, etc.).
		/// </summary>
		public static void Resume()
		{
			GameObject.Find("Inventory").GetComponent<PlayerInput>().currentActionMap.Enable();
			GameObject.Find("Player").GetComponent<PlayerInput>().currentActionMap.Enable();

			Time.timeScale = 1.0f;
		}

		/// <summary>
		/// Quits out of playing mode if in the editor, or exits the game if it's a built version.
		/// </summary>
		public static void Quit()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}