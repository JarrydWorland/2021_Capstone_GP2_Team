namespace Scripts.Menus
{
	public class MenuPlayingBehaviour : MenuBehaviour
	{
		public override void OnEnter()
		{
			base.OnEnter();
			MenuManager.Resume();
		}

		public override void OnLeave()
		{
			MenuManager.Pause();
			base.OnLeave();
		}

		public override void OnReturn() => MenuManager.Resume();
	}
}