using UnityEngine;
using Scripts.Dialogue;
using Scripts.Menus;

namespace Scripts.Doors
{
    [RequireComponent(typeof(DoorConnectionBehaviour), typeof(DialogueContentBehaviour))]
	public class DoorClosedMessageBehaviour : MonoBehaviour
	{
		private DoorConnectionBehaviour _doorConnectionBehaviour;
		private DialogueContentBehaviour _dialogueContentBehaviour;

		private void Start()
		{
			_doorConnectionBehaviour = GetComponent<DoorConnectionBehaviour>();
 			_dialogueContentBehaviour = GetComponent<DialogueContentBehaviour>();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.name == "Player" && !_doorConnectionBehaviour.IsOpen)
			{
				MenuManager.ShowDialogue(_dialogueContentBehaviour.DialogueContent);
			}
		}
	}
}
