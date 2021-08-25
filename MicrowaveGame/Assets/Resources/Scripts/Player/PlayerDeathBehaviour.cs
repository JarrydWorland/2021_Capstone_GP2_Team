using Scripts.Events;
using Scripts.Menus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Player
{
	public class PlayerDeathBehaviour : MonoBehaviour
	{
		private EventId<HealthChangedEventArgs> _healthChangedEventId;

		public ParticleSystem deathParticles;

		private void Start()
		{
			_healthChangedEventId = EventManager.Register<HealthChangedEventArgs>(OnHealthChanged);
			deathParticles = transform.GetChild(6).GetComponent<ParticleSystem>();
			transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
			transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
			transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
		}

		private void OnHealthChanged(HealthChangedEventArgs eventArgs)
		{
			if (eventArgs.GameObject.name == "Player" && eventArgs.NewValue == 0)
			{
				Time.timeScale = 0.0f;
				deathParticles.Play();
				transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
				transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
				transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
				GameObject.Find("Player").GetComponent<PlayerInput>().actions.Disable();
				MenuManager.GoInto("MenuDeath");
			}
		}

		private void OnDestroy() => EventManager.Unregister(_healthChangedEventId);
	}
}