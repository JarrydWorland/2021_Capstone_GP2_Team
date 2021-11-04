using UnityEngine.SceneManagement;
using Scripts.Utilities;
using Scripts.Dialogue;
using UnityEngine;
using Scripts.Audio;

namespace Scripts.Menus
{
	public class MenuPlayingBehaviour : MenuBehaviour
	{
		public AudioClip HubLoopAudioClip;
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
			if (SceneManager.GetActiveScene().name == "Hub")
			{
				MenuManager.Init(this);

				// Bug causing audio to not be played first time it is called, so doing redundant call.
				AudioManager.Play(HubLoopAudioClip, AudioCategory.Music, 0.0f);
				AudioManager.Play(HubLoopAudioClip, AudioCategory.Music, 0.4f, true);

				if (Persistent.CollectedKeyCardCount >= Persistent.RequiredKeyCardCount)
					MenuManager.ShowDialogue(GameObject.Find("CardCountDisplay").GetComponent<DialogueContentBehaviour>().DialogueContent);
			}
			else if (SceneManager.GetActiveScene().name == "Gameplay")
			{
				MenuManager.Init(this);

				// Bug causing audio to not be played first time it is called, so doing redundant call.
				AudioManager.Play(GameplayLoopAudioClip, AudioCategory.Music, 0.0f);
				AudioManager.Play(GameplayLoopAudioClip, AudioCategory.Music, 0.4f, true);
			}
		}

		public override void OnReturn() => GameState.Resume();
	}
}