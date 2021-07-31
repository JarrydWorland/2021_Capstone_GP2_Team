using Scripts.Events;
using Scripts.Menus;
using UnityEngine;

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
			if (eventArgs.NewValue == 0)
			{
				Time.timeScale = 0.0f;

				// Surely there's a better way to find an inactive object...
				MenuDeathBehaviour deathMenu =
					transform.parent.transform.parent.Find("MenuDeath").GetComponent<MenuDeathBehaviour>();

				MenuManager.GoInto(deathMenu);
			}
		}

		private void OnDestroy() => EventManager.Unregister(_healthChangedEventId);
	}
}