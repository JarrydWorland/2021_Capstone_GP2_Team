using Scripts.Menus;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scripts.Dialogue;
using Scripts.Utilities;

namespace Scripts.DungeonEntrance
{
    class DungeonEntranceBehaviour : MonoBehaviour
	{
		private void Start()
		{
			// TODO: temporarily print number of key cards
			Log.Info($"Keycards: {Persistent.CollectedKeycardCount}");
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.name != "Player") return;
			SceneManager.LoadScene("Gameplay");
		}
	}
}
