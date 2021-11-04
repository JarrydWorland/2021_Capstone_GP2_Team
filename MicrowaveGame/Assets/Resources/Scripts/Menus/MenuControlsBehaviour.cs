namespace Scripts.Menus
{
	public class MenuControlsBehaviour : MenuBehaviour
	{
		/// <summary>
		/// Deactivates the keyboard & mouse section and activates the controller section.
		/// Called when the "Controller" button is pressed.
		/// </summary>
		public void OnControllerButtonPressed()
		{
			MenuManager.Current.transform.Find("ControllerSection").gameObject.SetActive(true);
			MenuManager.Current.transform.Find("KeyboardMouseSection").gameObject.SetActive(false);
		}

		/// <summary>
		/// Deactivates the controller section and activates the keyboard & mouse section.
		/// Called when the "Keyboard & Mouse" button is pressed.
		/// </summary>
		public void OnKeyboardMouseButtonPressed()
		{
			MenuManager.Current.transform.Find("ControllerSection").gameObject.SetActive(false);
			MenuManager.Current.transform.Find("KeyboardMouseSection").gameObject.SetActive(true);
		}

		/// <summary>
		/// Sets the current menu to the previous menu.
		/// Called when the "Done" button is pressed.
		/// </summary>
		public void OnDoneButtonPressed() => MenuManager.GoBack();
	}
}