using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.DungeonEntrance
{
	class DungeonEntranceBehaviour : MonoBehaviour
	{
		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.name != "Player") return;
			SceneManager.LoadScene("Gameplay");
		}
	}
}