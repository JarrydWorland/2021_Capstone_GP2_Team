using Scripts.Scenes;
using UnityEngine;
using Scripts.Utilities;
using Scripts.Audio;

namespace Scripts.Menus
{
	public class MenuMainBehaviour : MenuBehaviour
	{
		/// <summary>
		/// The background music.
		/// </summary>
		public AudioClip BackgroundMusicAudioClip;

		/// <summary>
		/// A debug flag to disable the background music.
		/// </summary>
		public bool DebugDisableBackgroundMusic;

		public override void OnEnter()
		{
			base.OnEnter();
			Time.timeScale = 1.0f;
		}

		private void Start()
		{
			MenuManager.Init(this);

			if (!DebugDisableBackgroundMusic)
			{
				AudioManager.Play(BackgroundMusicAudioClip, 0.7f, true);
			}
		}

		/// <summary>
		/// Sets the current scene to the "Gameplay" scene.
		/// Called when the "Start" button is pressed.
		/// </summary>
		public void OnStartButtonPressed() => SceneFaderBehaviour.Instance.FadeInto("Hub");

		/// <summary>
		/// Sets the current menu to the "Controls" menu.
		/// Called when the "Controls" button is pressed.
		/// </summary>
		public void OnControlsButtonPressed() => MenuManager.GoInto("MenuControls");

		/// <summary>
		/// Sets the current menu to the "Credits" menu.
		/// Called when the "Credits" button is pressed.
		/// </summary>
		public void OnCreditsButtonPressed() => MenuManager.GoInto("MenuCredits");

		/// <summary>
		/// Quits the game or exits the playing mode of the editor depending on the current executing context.
		/// Called when the "Quit" button is pressed.
		/// </summary>
		public void OnQuitButtonPressed() => GameState.Quit();
	}
}
