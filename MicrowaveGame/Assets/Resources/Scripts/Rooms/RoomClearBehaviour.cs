using System.Collections.Generic;
using System.Linq;
using Scripts.Events;
using Scripts.Items;
using Scripts.Utilities;
using UnityEngine;

namespace Scripts.Rooms
{
	public class RoomClearBehaviour : MonoBehaviour
	{
		private EventId<HealthChangedEventArgs> _healthChangedEventId;
		private int _enemyCount;

		private static IEnumerable<GameObject> _itemPrefabs;

		private void Start()
		{
			InitItemPrefabs();

			_enemyCount = GetComponentsInChildren<TagBehaviour>().Where(tag => tag.HasTag("Enemy")).Count();

			_healthChangedEventId = EventManager.Register<HealthChangedEventArgs>(OnHealthChanged);
		}

		private void OnDestroy()
		{
			EventManager.Unregister(_healthChangedEventId);
		}

		private void InitItemPrefabs()
		{
			if (_itemPrefabs == null)
			{
				IEnumerable<GameObject> itemPrefabs = Resources
					.LoadAll<GameObject>("Prefabs/Items")
					.Where(itemPrefab => itemPrefab.HasComponent<ItemBehaviour>());

				IEnumerable<GameObject> weaponPrefabs = Resources
					.LoadAll<GameObject>("Prefabs/Weapons")
					.Where(itemPrefab => itemPrefab.HasComponent<ItemBehaviour>() && itemPrefab.name != "WeaponDefault");

				_itemPrefabs = Enumerable.Empty<GameObject>()
					.Concat(itemPrefabs)
					.Concat(weaponPrefabs);
			}
		}

		private void OnRoomCleared()
		{
			GameObject randomItemPrefab = _itemPrefabs.GetRandomElement();
			GameObject randomItem = Instantiate(randomItemPrefab);
			randomItem.transform.position = transform.position;
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
