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
				AudioManager.Play(BackgroundMusicAudioClip, AudioCategory.Music, 0.7f, true);
			}
		}

		/// <summary>
		/// Sets the current scene to the "OpeningCutscene" scene.
		/// Called when the "Start" button is pressed.
		/// </summary>
		public void OnStartButtonPressed() 
		{
			//if (Persistent.FirstTimeInHub)
			//{
			//	SceneFaderBehaviour.Instance.FadeInto("OpeningCutscene");
			//}
			//else
			//{
				SceneFaderBehaviour.Instance.FadeInto("Hub");
			//}
		}

		/// <summary>
		/// Sets the current menu to the "Settings" menu.
		/// Called when the "Settings" button is pressed.
		/// </summary>
		public void OnSettingsButtonPressed() => MenuManager.GoInto("MenuSettings");

		/// <summary>
		/// Sets the current scene to the "TeamCredits" scene.
		/// Called when the "Credits" button is pressed.
		/// </summary>
		//public void OnCreditsButtonPressed() => SceneFaderBehaviour.Instance.FadeInto("TeamCredits"); //TeamCredits
		public void OnCreditsButtonPressed() => MenuManager.GoInto("MenuCredits"); //TeamCredits

		/// <summary>
		/// Quits the game or exits the playing mode of the editor depending on the current executing context.
		/// Called when the "Quit" button is pressed.
		/// </summary>
		public void OnQuitButtonPressed() => GameState.Quit();
	}
}
