using UnityEngine.SceneManagement;
using Scripts.Utilities;
using Scripts.Dialogue;
using UnityEngine;
using Scripts.Audio;

namespace Scripts.Menus
{
	public class MenuPlayingBehaviour : MenuBehaviour
	{
		public AudioClip GameplayLoopAudioClip;
		public AudioClip PauseAudioClip;

		public override void OnEnter()
		{
			base.OnEnter();
			GameState.Resume();
		}

		public override void OnLeave()
		{
			GameState.Pause();
			AudioManager.Play(PauseAudioClip, AudioCategory.Effect);

			base.OnLeave();
		}

		private void Start()
		{
			MenuManager.Init(this);

			if (Persistent.CollectedKeyCardCount >= Persistent.RequiredKeyCardCount)
				MenuManager.ShowDialogue(GameObject.Find("CardCountDisplay").GetComponent<DialogueContentBehaviour>().DialogueContent);

			if (SceneManager.GetActiveScene().name == "Gameplay")
			{
				// Bug causing audio to not be played first time it is called, so doing redundant call.
				AudioManager.Play(GameplayLoopAudioClip, AudioCategory.Music, 0.0f, false);

				AudioManager.Play(GameplayLoopAudioClip, AudioCategory.Music, 0.4f, true);
			}
		}

		public override void OnReturn() => GameState.Resume();
	}
}