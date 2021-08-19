using UnityEngine;

namespace Scripts.Dialogue
{
	[System.Serializable]
	public class Dialogue
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