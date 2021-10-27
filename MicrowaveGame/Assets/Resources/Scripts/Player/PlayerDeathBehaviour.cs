using Scripts.Audio;
using Scripts.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using Scripts.Menus;

namespace Scripts.Player
{
    public class PlayerDeathBehaviour : MonoBehaviour
	{
		private EventId<HealthChangedEventArgs> _healthChangedEventId;

		public ParticleSystem deathParticles;
		public AudioClip PlayerDeath;

		private void Start()
		{
			_healthChangedEventId = EventManager.Register<HealthChangedEventArgs>(OnHealthChanged);
		}

		private void OnHealthChanged(HealthChangedEventArgs eventArgs)
		{
			if (eventArgs.GameObject.name == "Player" && eventArgs.NewValue == 0)
			{
				Time.timeScale = 0.0f;
				Explode();
				AudioManager.Play(PlayerDeath, 1f);

				Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
				foreach (Collider2D collider in colliders) collider.enabled = false;

				SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
				foreach (SpriteRenderer spriteRenderer in spriteRenderers) spriteRenderer.enabled = false;

				GameObject.Find("Player").GetComponent<PlayerInput>().actions.Disable();
				MenuManager.GoInto("MenuDeath");
			}
		}

		private void OnDestroy() => EventManager.Unregister(_healthChangedEventId);

		void Explode()
		{
			//Instantiate our one-off particle system
			ParticleSystem explosionEffect = Instantiate(deathParticles) as ParticleSystem;
			Vector3 spawnPosition = new Vector3(transform.position.x,transform.position.y,transform.position.z-2); //have to use -2 because sprite ordering is too difficult to implement this late in the project.
			explosionEffect.transform.position = spawnPosition;
			
			//play it
			explosionEffect.Play();

			//destroy the particle system when its duration is up
			Destroy(explosionEffect.gameObject, explosionEffect.main.duration);
		}
	}
}
