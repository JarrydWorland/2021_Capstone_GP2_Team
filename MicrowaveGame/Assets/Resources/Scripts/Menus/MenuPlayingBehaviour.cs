using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Scripts.Menus
{
	public class MenuPlayingBehaviour : MenuBehaviour
	{
		public override void OnEnter() => MenuManager.Resume();

		public override void OnLeave() => MenuManager.Pause();

		/// <summary>
		/// Called via Unity's new input system when the user presses the "Escape" key.
		/// Sets the current menu to the "Controls" menu.
		/// Called when the "Controls" button is pressed.
		/// </summary>
		/// <param name="context"></param>
		public void OnPause(InputAction.CallbackContext context)
		{
			if (context.performed) MenuManager.GoInto("MenuPaused");
		}

		private void Start()
		{
			if (SceneManager.GetActiveScene().name == "Gameplay")
			{
				MenuManager.Init(this);
			}
		}
	}
}
