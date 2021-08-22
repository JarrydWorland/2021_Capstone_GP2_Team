using System;
using UnityEngine;

namespace Scripts.Dialogue
{
	public class DialogueContentBehaviour : MonoBehaviour
	{
		public DialogueContent dialogueContent;
	}

	[Serializable]
	public class DialogueContent
	{
		/// <summary>
		/// The name of the speaker.
		/// </summary>
		public string Speaker;

		/// <summary>
		/// The sentences that will be displayed.
		/// </summary>
		[TextArea(3, 10)]
		public string[] Sentences;
	}
}