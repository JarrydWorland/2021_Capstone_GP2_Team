using Scripts.Events;
using Scripts.Menus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Player
{
	public class PlayerDeathBehaviour : MonoBehaviour
	{
		private EventId<HealthChangedEventArgs> _healthChangedEventId;

		private void Start()
		{
			_healthChangedEventId = EventManager.Register<HealthChangedEventArgs>(OnHealthChanged);
		}

		private void OnHealthChanged(HealthChangedEventArgs eventArgs)
		{
			if (eventArgs.GameObject.name == "Player" && eventArgs.NewValue == 0)
			{
				Time.timeScale = 0.0f;
				GameObject.Find("Player").GetComponent<PlayerInput>().actions.Disable();
				MenuManager.GoInto("MenuDeath");
			}
		}

		private void OnDestroy() => EventManager.Unregister(_healthChangedEventId);
	}
}