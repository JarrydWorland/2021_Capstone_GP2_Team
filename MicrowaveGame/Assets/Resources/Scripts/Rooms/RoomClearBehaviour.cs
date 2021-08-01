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
			_itemPrefabs ??= Resources.LoadAll<GameObject>("Prefabs/Items")
				.Where(itemPrefab => itemPrefab.HasComponent<ItemBehaviour>());

			foreach (Transform childTransform in transform)
			{
				TagBehaviour tagBehaviour = childTransform.GetComponent<TagBehaviour>();
				if (tagBehaviour != null && tagBehaviour.HasTag("Enemy")) _enemyCount++;
			}

			_healthChangedEventId = EventManager.Register<HealthChangedEventArgs>(OnHealthChanged);
		}

		private void OnHealthChanged(HealthChangedEventArgs eventArgs)
		{
			if (eventArgs.NewValue > 0) return;

			TagBehaviour tagBehaviour = eventArgs.GameObject.GetComponent<TagBehaviour>();
			if (tagBehaviour == null || !tagBehaviour.HasTag("Enemy")) return;

			_enemyCount--;
			if (_enemyCount != 0) return;

			GameObject randomItemPrefab = _itemPrefabs.GetRandomElement();
			Instantiate(randomItemPrefab, transform);

			EventManager.Unregister(_healthChangedEventId);
		}
	}
}