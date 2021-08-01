namespace Scripts.Menus
{
	public class MenuCreditsBehaviour : MenuBehaviour
	{
		/// <summary>
		/// Sets the current menu to the previous menu.
		/// Called when the "Done" button is pressed.
		/// </summary>
		public void OnDoneButtonPressed() => MenuManager.GoBack();
	}
}