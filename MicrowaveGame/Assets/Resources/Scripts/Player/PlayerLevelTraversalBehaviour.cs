using UnityEngine;
using Scripts.Levels;
using Scripts.Doors;
using Scripts.Utilities;

namespace Scripts.Player
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class PlayerLevelTraversalBehaviour : MonoBehaviour
	{
		private LevelTraversalBehaviour _levelTraversalBehaviour;
		private PlayerMovementBehaviour _playerMovementBehaviour;

		private void Start()
		{
			_levelTraversalBehaviour = GameObject.Find("Level").GetComponent<LevelTraversalBehaviour>();
			_playerMovementBehaviour = GameObject.Find("Player").GetComponent<PlayerMovementBehaviour>();
		}

		public void OnTriggerStay2D(Collider2D other)
		{
			DoorConnectionBehaviour doorConnectionBehaviour = other.GetComponent<DoorConnectionBehaviour>();

			if (doorConnectionBehaviour != null)
			{
				Direction direction = doorConnectionBehaviour.Direction;

				float dotproduct = Vector2.Dot(direction.ToVector2(), _playerMovementBehaviour.Direction);
				float angle = Extensions.MapBetween(dotproduct, -1.0f, 1.0f, 180, 0);
				if(angle < 50)
				{
					GetComponent<PlayerMovementBehaviour>().Velocity = Vector2.zero;
					_levelTraversalBehaviour.ChangeRoom(doorConnectionBehaviour);
				}
			}
		}
	}
}
