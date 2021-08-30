using System.Collections.Generic;
using Scripts.Camera;
using Scripts.Doors;
using Scripts.Events;
using Scripts.Levels;
using Scripts.Rooms;
using Scripts.Utilities;
using UnityEngine;

namespace Scripts.Minimap
{
    public class MinimapBehaviour : MonoBehaviour
	{
		public Sprite Sprite;

		public Color StartingRoomColor;
		public Color CurrentRoomColor;
		public Color ExploredRoomColor;
		public Color UnexploredRoomColor;
		public Color LineColor;

		private EventId<RoomTraversedEventArgs> _roomTraversedEventId;
		private Dictionary<Vector2Int,GameObject> _roomNodes = new Dictionary<Vector2Int,GameObject>();
		private Dictionary<Vector2,GameObject> _lineNodes = new Dictionary<Vector2,GameObject>();

		private GameObject _currentRoomNode;
		private Transform _roomOffset;
		private Lerped<Vector3> _targetOffset;

		private const float _spacing = 0.25f;
	
		private void OnEnable()
		{
			// NOTE: Here we are using OnEnable() instead of Start() so that
			// the event can be registered before it is emitted by
			// LevelTraversalBehaviour.

			float panDuration = UnityEngine.Camera.main.GetComponent<CameraPanBehaviour>().PanDuration;
			_roomTraversedEventId = EventManager.Register<RoomTraversedEventArgs>(OnRoomTraversed);
			_roomOffset ??= transform.Find("Background/RoomOffset");
			_targetOffset ??= new Lerped<Vector3>(Vector3.zero, panDuration, Easing.EaseInOut, true);
		}

		private void OnDisable() =>  EventManager.Unregister(_roomTraversedEventId);

		private void Update()
		{
			_roomOffset.localPosition = _targetOffset.Value;
		}

		/// <summary>
		/// Updates the minimap when the player traverses between rooms, this
		/// is called by the EventManager when RoomTraversedEventArgs is
		/// emitted.
		/// </summary>
		private void OnRoomTraversed(RoomTraversedEventArgs eventArgs)
		{
			RoomConnectionBehaviour roomConnectionBehaviour = eventArgs.CurrentRoom;

			GameObject roomNode = HasRoomNode(roomConnectionBehaviour)
				? GetRoomNode(roomConnectionBehaviour)
				: AddRoomNode(roomConnectionBehaviour);

			foreach(DoorConnectionBehaviour doorConnectionBehaviour in eventArgs.CurrentRoom.Doors)
			{
				RoomConnectionBehaviour connectingRoomConnectionBehaviour = doorConnectionBehaviour.ConnectingDoor.GetComponentsInParent<RoomConnectionBehaviour>(true)[0];
				
				// add connecting room node
				if (!HasRoomNode(connectingRoomConnectionBehaviour))
				{
					AddRoomNode(connectingRoomConnectionBehaviour);
				}

				// add line between room node and connecting room node
				if (!HasLineNode(roomConnectionBehaviour, connectingRoomConnectionBehaviour))
				{
					AddLineNode(roomConnectionBehaviour, connectingRoomConnectionBehaviour);
				}
			}

			// update room node colors
			if (_currentRoomNode != null)
			{
				bool startingRoom = _currentRoomNode.transform.localPosition == Vector3.zero;
				_currentRoomNode.GetComponent<SpriteRenderer>().color = startingRoom ? StartingRoomColor : ExploredRoomColor;
			}
			_currentRoomNode = roomNode;
			_currentRoomNode.GetComponent<SpriteRenderer>().color = CurrentRoomColor;

			// update minimap target position
			_targetOffset.Value = new Vector3
			{
				x = -eventArgs.CurrentRoom.Position.x * _roomOffset.localScale.x * _spacing,
				y = -eventArgs.CurrentRoom.Position.y * _roomOffset.localScale.y * _spacing,
				z = _roomOffset.position.z,
			};
		}

		private GameObject AddRoomNode(RoomConnectionBehaviour roomConnectionBehaviour)
		{
			// create room node
			GameObject roomNode = new GameObject("RoomNode");

			// initialize transform
			roomNode.transform.SetParent(_roomOffset, false);
			roomNode.transform.localPosition = new Vector3
			{
				x = roomConnectionBehaviour.Position.x * _spacing,
				y = roomConnectionBehaviour.Position.y * _spacing,
				z = roomNode.transform.position.z,
			};
			
			// initialize sprite renderer
			SpriteRenderer spriteRenderer = roomNode.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = Sprite;
			spriteRenderer.color = UnexploredRoomColor;
			spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
			spriteRenderer.sortingLayerID = SortingLayer.NameToID("User Interface");
			spriteRenderer.sortingOrder = 2;

			// remember room node
			Debug.Assert(!HasRoomNode(roomConnectionBehaviour), "Creating a room node that already exists");
			_roomNodes.Add(roomConnectionBehaviour.Position, roomNode);

			return roomNode;
		}

		private GameObject GetRoomNode(RoomConnectionBehaviour roomConnectionBehaviour)
		{
			return _roomNodes[roomConnectionBehaviour.Position];
		}

		private bool HasRoomNode(RoomConnectionBehaviour roomConnectionBehaviour)
		{
			return _roomNodes.ContainsKey(roomConnectionBehaviour.Position);
		}

		private GameObject AddLineNode(RoomConnectionBehaviour lhs, RoomConnectionBehaviour rhs)
		{
			Vector2 linePosition = GetLineNodePosition(lhs, rhs);
			Vector3 direction = (rhs.transform.localPosition - lhs.transform.localPosition).normalized;

			// create line
			GameObject lineNode = new GameObject("LineNode");

			// initialize transform
			lineNode.transform.SetParent(_roomOffset, false);
			lineNode.transform.localPosition = linePosition;
			lineNode.transform.localScale = new Vector3(2, 0.4f, 1);
			lineNode.transform.Rotate(0, 0, Mathf.Abs(direction.y) > Mathf.Abs(direction.x) ? 90 : 0);

			// initialize sprite renderer
			SpriteRenderer spriteRenderer = lineNode.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = Sprite;
			spriteRenderer.color = LineColor;
			spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
			spriteRenderer.sortingLayerID = SortingLayer.NameToID("User Interface");
			spriteRenderer.sortingOrder = 1;

			// remember line node
			Debug.Assert(!HasLineNode(lhs, rhs), "Creating a line node that already exists");
			_lineNodes.Add(linePosition, lineNode);

			return lineNode;
		}

		private GameObject GetLineNode(RoomConnectionBehaviour lhs, RoomConnectionBehaviour rhs)
		{
			Vector2 linePosition = GetLineNodePosition(lhs, rhs);
			return _lineNodes[linePosition];
		}

		private bool HasLineNode(RoomConnectionBehaviour lhs, RoomConnectionBehaviour rhs)
		{
			Vector2 linePosition = GetLineNodePosition(lhs, rhs);
			return _lineNodes.ContainsKey(linePosition);
		}

		private Vector2 GetLineNodePosition(RoomConnectionBehaviour lhs, RoomConnectionBehaviour rhs)
		{
			Vector3 lhsNodePosition = GetRoomNode(lhs).transform.localPosition;
			Vector3 rhsNodePosition = GetRoomNode(rhs).transform.localPosition;
			return Vector3.Lerp(lhsNodePosition, rhsNodePosition, 0.5f);
		}
	}
}
