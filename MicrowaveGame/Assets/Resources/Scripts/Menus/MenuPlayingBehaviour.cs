using UnityEngine.InputSystem;
using Scripts.Dialogue;

namespace Scripts.Menus
{
	public class MenuPlayingBehaviour : MenuBehaviour
	{
		/// <summary>
		/// Only display the intro narrative when the player first loads in.
		/// </summary>
		private bool _firstLaunch = true;

		public override void OnEnter()
		{
			DialogueTriggerBehaviour dialogueTriggerBehaviour = GetComponent<DialogueTriggerBehaviour>();

			if (_firstLaunch)
			{
				_firstLaunch = false;
				dialogueTriggerBehaviour.TriggerDialogue();
			}
			else if (!dialogueTriggerBehaviour.DialogueBehaviour.InDialogue) MenuManager.Resume();
		}

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
	}
}