using UnityEngine;
using Scripts.Camera;
using Scripts.Scenes;
using Scripts.Events;
using Scripts.Levels;

namespace Scripts.Rooms
{
    public class RoomEndGameBehaviour : MonoBehaviour
	{
		private EventId<RoomTraversedEventArgs> _roomTraversedEventId;
		private CameraPanBehaviour _cameraPanBehaviour;
		private bool _ending;

		private void Awake()
		{
			_cameraPanBehaviour = UnityEngine.Camera.main.GetComponent<CameraPanBehaviour>();
			_roomTraversedEventId = EventManager.Register<RoomTraversedEventArgs>(OnRoomTraversedEvent);
		}

		private void OnDestroy()
		{
			EventManager.Unregister(_roomTraversedEventId);
		}

		private void Update()
		{
			if (_ending)
			{
				_cameraPanBehaviour.Position.Value = Vector3.zero;
			}
		}

		private void OnRoomTraversedEvent(RoomTraversedEventArgs args)
		{
			if (args.CurrentRoom.gameObject != gameObject) return;

			_ending = true;
     		SceneFaderBehaviour.Instance.FadeInto("Menu");
			//TODO: Play ending cutscene
		}
	}
}
