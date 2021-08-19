using UnityEngine;
using UnityEngine.InputSystem;
using Scripts.DialogueUI;

namespace Scripts.Menus
{
	public class MenuPlayingBehaviour : MenuBehaviour
	{
		public override void OnEnter()
		{
			GameObject.Find("Player").GetComponent<PlayerInput>().actions.Enable();
			gameObject.GetComponent<DialogueTrigger>().TriggerDialogue();
			Time.timeScale = 1.0f;
		}

		public override void OnLeave()
		{
			Time.timeScale = 0.0f;
			GameObject.Find("Player").GetComponent<PlayerInput>().actions.Disable();
		}

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