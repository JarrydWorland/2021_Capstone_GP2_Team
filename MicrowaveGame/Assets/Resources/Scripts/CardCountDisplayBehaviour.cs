using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
	public class CardCountDisplayBehaviour : MonoBehaviour
	{
		private void Start()
		{
			GetComponentInChildren<Text>().text =
				$"{Persistent.CollectedKeyCardCount} / {Persistent.RequiredKeyCardCount}";
		}
	}
}