using Scripts.Scenes;
using Scripts.Utilities;
using UnityEngine;

namespace Scripts.DungeonEntrance
{
	class DungeonEntranceBehaviour : MonoBehaviour
	{
		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.name != "Player") return;

			GameState.Pause();
			SceneFaderBehaviour.Instance.FadeInto("Gameplay");
		}
	}
}