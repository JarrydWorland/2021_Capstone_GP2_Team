using Scripts.Events;
using Scripts.Menus;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace Scripts.Player
{
	public class PlayerDeathBehaviour : MonoBehaviour
	{
		private const float CameraShakeStrength = 1.0f;
		private const float CameraShakeDamping = 10.0f;

		private EventId<HealthChangedEventArgs> _healthChangedEventId;

		public ParticleSystem deathParticles;
		public AudioClip PlayerDeath;

		private void Start()
		{
			_healthChangedEventId = EventManager.Register<HealthChangedEventArgs>(OnHealthChanged);
			transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
			transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
			transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
		}

        private void OnHealthChanged(HealthChangedEventArgs eventArgs)
		{
			if (eventArgs.GameObject.name == "Player" && eventArgs.NewValue == 0)
			{
				Time.timeScale = 0.0f;
				Explode();
				AudioManager.Play(PlayerDeath, 1f);
				GameObject.Find("Player").GetComponent<Collider2D>().enabled = false;
				transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
				transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
				transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
				GameObject.Find("Player").GetComponent<PlayerInput>().actions.Disable();
				MenuManager.GoInto("MenuDeath");
			}
			else if(eventArgs.GameObject.name == "Player" && eventArgs.NewValue < eventArgs.OldValue)
            {
				StartCoroutine(Shake());
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

		private IEnumerator Shake()
        {
			float zRotation = UnityEngine.Random.Range(0f, CameraShakeDamping) - (CameraShakeStrength / 2.0f);
			UnityEngine.Camera.main.transform.rotation = Quaternion.Lerp(
				UnityEngine.Camera.main.transform.rotation,
				Quaternion.Euler(0, 0, zRotation),
				CameraShakeDamping * Time.deltaTime
				);
			yield return new WaitForSecondsRealtime(0.25f);
			UnityEngine.Camera camera = UnityEngine.Camera.main;
			if (camera != null) camera.transform.rotation = Quaternion.identity;
		}
    }
}