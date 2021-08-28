using UnityEngine;
using Scripts.Levels;
using Scripts.Doors;

namespace Scripts.Player
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class PlayerLevelTraversalBehaviour : MonoBehaviour
	{
		private LevelTraversalBehaviour _levelTraversalBehaviour;

		private void Start()
		{
			_levelTraversalBehaviour = GameObject.Find("Level").GetComponent<LevelTraversalBehaviour>();
		}

		public void OnTriggerEnter2D(Collider2D other)
		{
			DoorConnectionBehaviour doorConnectionBehaviour = other.GetComponent<DoorConnectionBehaviour>();

			if (doorConnectionBehaviour != null)
			{
				GetComponent<PlayerMovementBehaviour>().Velocity = Vector2.zero;
				_levelTraversalBehaviour.ChangeRoom(doorConnectionBehaviour);
			}
		}
	}
}