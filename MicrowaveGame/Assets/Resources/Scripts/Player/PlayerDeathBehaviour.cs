using Scripts.Events;
using Scripts.Menus;
using UnityEngine;
using Scripts.Utilities;

namespace Scripts.Player
{
	public class PlayerDeathBehaviour : MonoBehaviour
	{
		private EventId<HealthChangedEventArgs> _healthChangedEventId;

		public AudioClip PlayerDeath;
		private void Start()
		{
			_healthChangedEventId = EventManager.Register<HealthChangedEventArgs>(OnHealthChanged);
		}

		private void OnHealthChanged(HealthChangedEventArgs eventArgs)
		{
			if (eventArgs.GameObject.name == "Player" && eventArgs.NewValue == 0)
			{
				MenuManager.GoInto("MenuDeath");
				AudioManager.Play(PlayerDeath);
			}
		}

		private void OnDestroy() => EventManager.Unregister(_healthChangedEventId);
	}
}