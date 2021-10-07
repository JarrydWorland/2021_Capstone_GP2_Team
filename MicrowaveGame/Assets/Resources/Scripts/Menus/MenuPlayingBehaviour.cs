using UnityEngine.SceneManagement;
using Scripts.Utilities;
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
			if (SceneManager.GetActiveScene().name == "Gameplay")
			{
				MenuManager.Init(this);
			}
		}

		public override void OnReturn() => GameState.Resume();
	}
}
