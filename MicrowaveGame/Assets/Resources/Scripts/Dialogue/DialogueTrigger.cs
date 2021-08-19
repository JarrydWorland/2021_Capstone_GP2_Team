using UnityEngine;

namespace Scripts.Dialogue
{
	public class DialogueTrigger : MonoBehaviour
	{
		/// <summary>
		/// The dialogue information to be displayed.
		/// </summary>
		public Dialogue Dialogue;

		/// <summary>
		/// The dialogue behaviour object that will process the dialogue object.
		/// </summary>
		public DialogueBehaviour DialogueBehaviour;

		/// <summary>
		/// Enables the given dialogue behaviour object and starts the dialogue sequence.
		/// </summary>
		public void TriggerDialogue()
		{
			DialogueBehaviour.gameObject.SetActive(true);
			DialogueBehaviour.StartDialogue(Dialogue);
		}
	}
}