using Scripts.Menus;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scripts.Dialogue;

namespace Scripts.DungeonEntrance
{
    class DungeonEntranceBehaviour : MonoBehaviour
	{
		DialogueContentBehaviour DungeonEnterDialogueContent;

		private void Start()
		{
			DungeonEnterDialogueContent = GetComponent<DialogueContentBehaviour>();
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.name != "Player") return;
			MenuManager.ShowDialogue(DungeonEnterDialogueContent.DialogueContent, () =>
			{
				SceneManager.LoadScene("Gameplay");
			});
		}
	}
}
