using UnityEngine.SceneManagement;
using Scripts.Utilities;
using Scripts.Dialogue;
using UnityEngine;

namespace Scripts.Menus
{
	public class MenuPlayingBehaviour : MenuBehaviour
	{
		public AudioClip Pause;
		public AudioClip Unpause;
		public override void OnEnter()
		{
			base.OnEnter();
			GameState.Resume();
			//AudioManager.Play(Unpause);
		}

		public override void OnLeave()
		{
			GameState.Pause();
			AudioManager.Play(Pause);
			base.OnLeave();
		}

		private void Start()
		{
			MenuManager.Init(this);
			if (Persistent.CollectedKeycardCount >= Persistent.RequiredKeycardCount)
				MenuManager.ShowDialogue(GameObject.Find("CardCountDisplay").GetComponent<DialogueContentBehaviour>().DialogueContent);
		}

		public override void OnReturn() => GameState.Resume();
	}
}
