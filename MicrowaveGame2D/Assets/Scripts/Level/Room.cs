using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Events;
using Enemy;
using Items;
using Helpers;

namespace Level
{
	public class Room : MonoBehaviour
	{
		public IEnumerable<Door> Doors => GetComponentsInChildren<Door>(true);
		public IEnumerable<DoorDirection> Directions => Doors.Select(door => door.Direction);
		public Vector2Int Position { get; private set; }

		// for room rewards
		private int _enemiesInRoom;
		private EventId<HealthChangedEventArgs> _healthChangedEventId;

		public bool HasDoorFacing(DoorDirection direction)
		{
			return Doors.Any(door => door.Direction == direction);
		}

		public Door GetDoorFacing(DoorDirection direction)
		{
			return Doors.FirstOrDefault(door => door.Direction == direction);
		}

		public static Room Make(GameObject original, Transform parent, Vector2Int position)
		{
			GameObject gameObject = Instantiate(original, parent, true);

			Room room = gameObject.GetComponent<Room>();
			room.Position = position;

			return room;
		}

		private void Start()
		{
			_enemiesInRoom = GetComponentsInChildren<EnemyHealthBehaviour>().Length;
		}

		private void OnEnable()
		{
			// register the OnHealthChangedEvent whenever the room becomes active
			_healthChangedEventId = EventManager.Register<HealthChangedEventArgs>(OnHealthChangedEvent);
		}

		private void OnDisable()
		{
			// unregister the OnHealthChangedEvent whenever the room becomes inactive
			EventManager.Unregister(_healthChangedEventId);
		}

		private void OnHealthChangedEvent(HealthChangedEventArgs eventArgs)
		{
			bool isEnemy = eventArgs.GameObject.GetComponent<EnemyHealthBehaviour>() != null;
			bool isDead = eventArgs.NewValue == 0;
			bool isWithinRoom = eventArgs.GameObject.transform.parent == transform;

			if (isEnemy && isDead && isWithinRoom)
			{
				_enemiesInRoom -= 1;
				if (_enemiesInRoom <= 0)
				{
					OnRoomCleared();
				}
			}
		}

		private void OnRoomCleared()
		{
			// this should probably be cached
			IEnumerable<GameObject> itemPrefabs = Resources
				.LoadAll<GameObject>("Prefabs/Items")
				.Where(itemPrefab => itemPrefab?.GetComponent<BaseItem>() != null);

			// select a random item
			GameObject randomItemPrefab = itemPrefabs.RandomElement();

			// spawn the random item
			Instantiate(randomItemPrefab, transform);
		}
	}
}
