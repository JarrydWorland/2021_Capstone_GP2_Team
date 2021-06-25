using UnityEngine;
using Scripts.Rooms;

namespace Scripts.Levels
{
	[RequireComponent(typeof(LevelGenerationBehaviour))]
	public class LevelTraversalBehaviour : MonoBehaviour
	{
		private RoomConnectionBehaviour _currentRoom;

		private void PostStart()
		{
			_currentRoom = GetComponent<LevelGenerationBehaviour>().StartingRoom.GetComponent<RoomConnectionBehaviour>();
		}
	}
}
