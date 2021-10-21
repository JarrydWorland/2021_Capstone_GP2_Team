using UnityEngine;
using System.Collections;
using Scripts.Utilities;

namespace Scripts.Enemies
{
	[RequireComponent(typeof(HealthBehaviour))]
	class EnemyDeathBehaviour : MonoBehaviour
	{
		private HealthBehaviour _healthBehaviour;
		public ParticleSystem _deathParticle;
		public ParticleSystem _smokeParticle;
		public AudioClip _sfxExposion;

		private void Start()
		{
			_healthBehaviour = GetComponent<HealthBehaviour>();
		}

		private void Update()
		{
			if (_healthBehaviour.Value == 0)
			{
				Explode();
				Destroy(gameObject);
			}

			if(_healthBehaviour.Value <= 2 && !_smokeParticle.isPlaying)
            {
				_smokeParticle.Play();
			}
		}

		void Explode()
		{
			//Instantiate our one-off particle system
			ParticleSystem explosionEffect = Instantiate(_deathParticle) as ParticleSystem;
			Vector3 spawnPosition = new Vector3(transform.position.x,transform.position.y,transform.position.z-2); //have to use -2 because sprite ordering is too difficult to implement this late in the project.
			explosionEffect.transform.position = spawnPosition;

			//play it
			AudioManager.Play(_sfxExposion, 0.75f, false, Random.Range(0.85f, 1.25f));
			explosionEffect.Play();

			//destroy the particle system when its duration is up
			Destroy(explosionEffect.gameObject, explosionEffect.main.duration);
		}
	}
}
