using Scripts.Scenes;

namespace Scripts.Menus
{
	public class MenuPausedBehaviour : MenuBehaviour
	{
		/// <summary>
		/// Sets the current menu to the previous menu.
		/// Called when the "Done" button is pressed.
		/// </summary>
		public void OnResumeButtonPressed() => MenuManager.GoBack();

		/// <summary>
		/// Sets the current menu to the "Controls" menu.
		/// Called when the "Controls" button is pressed.
		/// </summary>
		public void OnControlsButtonPressed() => MenuManager.GoInto("MenuControls");

		/// <summary>
		/// Sets the current scene to the "Menu" scene.
		/// Called when the "Exit To Main Menu" button is pressed.
		/// </summary>
		public void OnExitButtonPressed() => SceneFaderBehaviour.Instance.FadeInto("Menu");
	}
}