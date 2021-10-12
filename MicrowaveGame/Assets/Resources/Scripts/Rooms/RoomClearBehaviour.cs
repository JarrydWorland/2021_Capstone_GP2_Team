using System.Collections.Generic;
using System.Linq;
using Scripts.Doors;
using Scripts.Events;
using Scripts.Items;
using Scripts.Levels;
using Scripts.Utilities;
using UnityEngine;

namespace Scripts.Rooms
{
	public class RoomClearBehaviour : MonoBehaviour
	{
		private EventId<RoomTraversedEventArgs> _roomTraversedEventId;
		private DoorConnectionBehaviour[] _doorConnectionBehaviours;

		private EventId<HealthChangedEventArgs> _healthChangedEventId;
		private int _enemyCount;

		private static IEnumerable<GameObject> _itemPrefabs;

		public AudioClip DoorOpen;

		public bool SpawnItemOnClear = true;

		// We use "Awake()" here so every room has its events and enemy count calculated before
		// we enter the room which is necessary for the "OnRoomTraversed()" method behaviour.
		private void Awake()
		{
			InitItemPrefabs();

			_roomTraversedEventId = EventManager.Register<RoomTraversedEventArgs>(OnRoomTraversed);
			_doorConnectionBehaviours = GetComponentsInChildren<DoorConnectionBehaviour>();

			_healthChangedEventId = EventManager.Register<HealthChangedEventArgs>(OnHealthChanged);
			_enemyCount = GetComponentsInChildren<TagBehaviour>().Count(tagBehaviour => tagBehaviour.HasTag("Enemy"));
		}

		private void OnDestroy()
		{
			EventManager.Unregister(_roomTraversedEventId);
			EventManager.Unregister(_healthChangedEventId);
		}

		private void InitItemPrefabs()
		{
			if (_itemPrefabs != null) return;

			IEnumerable<GameObject> itemPrefabs = Resources
				.LoadAll<GameObject>("Prefabs/Items")
				.Where(itemPrefab => itemPrefab.HasComponent<ItemBehaviour>());

			IEnumerable<GameObject> weaponPrefabs = Resources
				.LoadAll<GameObject>("Prefabs/Weapons")
				.Where(itemPrefab =>
					itemPrefab.HasComponent<ItemBehaviour>());

			_itemPrefabs = Enumerable.Empty<GameObject>()
				.Concat(itemPrefabs)
				.Concat(weaponPrefabs);
		}

		private void OnRoomTraversed(RoomTraversedEventArgs eventArgs)
		{
			RoomClearBehaviour roomClearBehaviour =
				eventArgs.CurrentRoom.GetComponent<RoomClearBehaviour>();

			if (roomClearBehaviour._enemyCount == 0) return;

			foreach (DoorConnectionBehaviour doorConnectionBehaviour in roomClearBehaviour._doorConnectionBehaviours)
			{
				if (doorConnectionBehaviour.IsOpen) doorConnectionBehaviour.Close();
			}
		}

		private void OnRoomCleared()
		{
			if (SpawnItemOnClear)
			{
				GameObject randomItemPrefab = _itemPrefabs
					.GetRandomElementWithProbability(itemPrefab => itemPrefab.GetComponent<ItemBehaviour>().SpawnProbability);

				GameObject randomItem = Instantiate(randomItemPrefab);
				randomItem.transform.position = transform.position;
			}

			AudioManager.Play(DoorOpen);

			foreach (var doorConnectionBehaviour in _doorConnectionBehaviours)
			{
				if (!doorConnectionBehaviour.IsOpen) doorConnectionBehaviour.Open();
			}
		}

		private void OnHealthChanged(HealthChangedEventArgs eventArgs)
		{
			TagBehaviour tagBehaviour = eventArgs.GameObject.GetComponent<TagBehaviour>();

			bool isEnemy = tagBehaviour != null && tagBehaviour.HasTag("Enemy");
			bool isDead = eventArgs.NewValue == 0;
			bool isWithinRoom = eventArgs.GameObject.transform.parent == transform;

			if (isEnemy && isDead && isWithinRoom)
			{
				_enemyCount -= 1;

				if (_enemyCount <= 0)
				{
					OnRoomCleared();
					EventManager.Unregister(_healthChangedEventId);
				}
			}
		}
	}
}
