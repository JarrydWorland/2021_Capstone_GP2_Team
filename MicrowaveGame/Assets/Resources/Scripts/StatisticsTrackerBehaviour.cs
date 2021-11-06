using System.Collections.Generic;
using Scripts.Events;
using Scripts.Levels;
using Scripts.Utilities;
using UnityEngine;

namespace Scripts
{
	public class StatisticsTrackerBehaviour : MonoBehaviour
	{
		public float StartTime { get; private set; }
		public int EnemiesDefeated { get; private set; }
		public int DamageDealt { get; private set; }
		public int DamageTaken { get; private set; }
		public int RoomsExplored => _visitedRooms.Count;

		private readonly HashSet<int> _visitedRooms = new HashSet<int>();

		private EventId<HealthChangedEventArgs> _healthChangedEventId;
		private EventId<RoomTraversedEventArgs> _roomTraversedEventId;

		private void Start()
		{
			_healthChangedEventId = EventManager.Register<HealthChangedEventArgs>(OnHealthChanged);
			_roomTraversedEventId = EventManager.Register<RoomTraversedEventArgs>(OnRoomTraversed);

			StartTime = Time.time;
		}

		private void OnDestroy()
		{
			EventManager.Unregister(_healthChangedEventId);
			EventManager.Unregister(_roomTraversedEventId);
		}

		private void OnHealthChanged(HealthChangedEventArgs eventArgs)
		{
			if (eventArgs.GameObject.name == "Player" && eventArgs.NewValue < eventArgs.OldValue)
				DamageTaken += eventArgs.OldValue - eventArgs.NewValue;

			TagBehaviour tagBehaviour = eventArgs.GameObject.GetComponent<TagBehaviour>();

			if (tagBehaviour != null && tagBehaviour.HasTag("Enemy") && eventArgs.NewValue < eventArgs.OldValue)
			{
				DamageDealt += eventArgs.OldValue - eventArgs.NewValue;
				if (eventArgs.NewValue <= 0) EnemiesDefeated++;
			}
		}

		private void OnRoomTraversed(RoomTraversedEventArgs eventArgs)
		{
			int id = eventArgs.CurrentRoom.gameObject.GetInstanceID();
			_visitedRooms.Add(id);
		}
	}
}