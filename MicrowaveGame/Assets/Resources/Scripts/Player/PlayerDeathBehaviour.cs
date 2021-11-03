using Scripts.Audio;
using Scripts.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using Scripts.Camera;
using Scripts.Menus;

namespace Scripts.Player
{
    public class PlayerDeathBehaviour : MonoBehaviour
	{
		private const float CameraShakeStrength = 1.5f;
		private const float CameraShakeDuration = 0.2f;

		private CameraShakeBehaviour _cameraShakeBehaviour;
		private EventId<HealthChangedEventArgs> _healthChangedEventId;

		/// <summary>
		/// The particle system to play when damage is healed.
		/// </summary>
		public ParticleSystem HealthParticleSystem;
		public ParticleSystem deathParticles;
		public AudioClip PlayerDeath;

		private void Start()
		{
			_cameraShakeBehaviour = UnityEngine.Camera.main.GetComponent<CameraShakeBehaviour>();
			_healthChangedEventId = EventManager.Register<HealthChangedEventArgs>(OnHealthChanged);
		}

        private void OnHealthChanged(HealthChangedEventArgs eventArgs)
		{
			if (eventArgs.GameObject.name == "Player" && eventArgs.NewValue == 0)
			{
				Time.timeScale = 0.0f;
				Explode();
				AudioManager.Play(PlayerDeath, AudioCategory.Effect, 1.0f);

				Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
				foreach (Collider2D collider in colliders) collider.enabled = false;

				SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
				foreach (SpriteRenderer spriteRenderer in spriteRenderers) spriteRenderer.enabled = false;

				GameObject.Find("Player").GetComponent<PlayerInput>().actions.Disable();
				MenuManager.GoInto("MenuDeath");
			}
			else if (eventArgs.GameObject.name == "Player" && eventArgs.NewValue <= eventArgs.OldValue)
			{
				_cameraShakeBehaviour.Shake(CameraShakeStrength, CameraShakeDuration);
			}
			else if (eventArgs.GameObject.name == "Player" && eventArgs.NewValue >  eventArgs.OldValue)
			{
				HealthParticleSystem.Play();
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
