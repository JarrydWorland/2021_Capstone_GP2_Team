using UnityEngine.SceneManagement;
using Scripts.Utilities;

namespace Scripts.Menus
{
	public class MenuPlayingBehaviour : MenuBehaviour
	{
		public override void OnEnter()
		{
			base.OnEnter();
			GameState.Resume();
		}

		public override void OnLeave()
		{
			GameState.Pause();
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
