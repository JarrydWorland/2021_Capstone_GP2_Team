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

		public override void OnReturn() => GameState.Resume();
	}
}