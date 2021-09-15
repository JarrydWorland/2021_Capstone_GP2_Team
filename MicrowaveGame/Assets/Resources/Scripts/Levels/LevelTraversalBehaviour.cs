using UnityEngine;
using System.Collections.Generic;
using Scripts.Rooms;
using Scripts.Doors;
using Scripts.Camera;
using Scripts.Utilities;
using System;
using Scripts.Events;

namespace Scripts.Levels
{
	[RequireComponent(typeof(LevelGenerationBehaviour))]
	public class LevelTraversalBehaviour : MonoBehaviour
	{
		/// <summary>
		/// The room the player is currently in.
		/// </summary>
		public RoomConnectionBehaviour CurrentRoom
		{
			get => _currentRoom;
			set
			{
				RoomConnectionBehaviour previousRoom = _currentRoom;
				_currentRoom = value;
				EventManager.Emit(new RoomTraversedEventArgs
				{
					PreviousRoom = previousRoom,
					CurrentRoom = CurrentRoom,
				});
			}
		}
		private RoomConnectionBehaviour _currentRoom;

		private LevelGenerationBehaviour _levelGenerationBehaviour;
		private GameObject _player;
		private CameraPanBehaviour _cameraPanBehaviour;

		private readonly List<GameObject> _roomsToDisable = new List<GameObject>();

		private void Start()
		{
			_levelGenerationBehaviour = GetComponent<LevelGenerationBehaviour>();
			_player = GameObject.Find("Player");
			_cameraPanBehaviour = UnityEngine.Camera.main.GetComponent<CameraPanBehaviour>();
		}

		private void Update()
		{
			// if the camera is stationary that means it has settled on the
			// CurrentRoom. So we can now disable any previous rooms knowing
			// that they are outside the view of the camera.
			if (_roomsToDisable.Count > 0 && _cameraPanBehaviour.IsStationary && !_levelGenerationBehaviour.DebugAlwaysShowRooms)
			{
				// Resume input and time as the changing room sequence has finished.
				GameState.Resume();
				
				_roomsToDisable.ForEach(room => room.SetActive(false));
				_roomsToDisable.Clear();
			}
		}
	
		/// <summary>
		/// Change the CurrentRoom by traveling through the given door.
		/// </summary>
		/// <param name="doorConnectionBehaviour">
		/// The door to travel through to change the current room.
		/// </param>
		public void ChangeRoom(DoorConnectionBehaviour doorConnectionBehaviour)
		{
			// Don't attempt to change the room if the door is closed / locked.
			if (!doorConnectionBehaviour.IsOpen) return;
			
			// Freeze input and time while changing room.
			GameState.Pause();
			
			AudioManager.Play(doorConnectionBehaviour.EnterAudioClip);

			// queue current room to be disabled
			_roomsToDisable.Add(CurrentRoom.gameObject);

			// find and enable the connecting room
			RoomConnectionBehaviour connectingRoom = doorConnectionBehaviour.ConnectingDoor.GetComponentsInParent<RoomConnectionBehaviour>(true)[0];
			_roomsToDisable.Remove(connectingRoom.gameObject); // ensure the connecting room is not queued to be disabled
			connectingRoom.gameObject.SetActive(true);

			CurrentRoom = connectingRoom;
			_cameraPanBehaviour.Position.Value = CurrentRoom.transform.position;

			_player.transform.position = doorConnectionBehaviour.ConnectingDoor.transform.position
									   + doorConnectionBehaviour.Direction.ToVector3()
									   * 1.75f;
		}
	}

	public class RoomTraversedEventArgs : EventArgs
	{
		public RoomConnectionBehaviour PreviousRoom;
		public RoomConnectionBehaviour CurrentRoom;
	}
}
