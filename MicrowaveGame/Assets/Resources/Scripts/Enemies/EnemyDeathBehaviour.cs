using UnityEngine;

namespace Scripts.Enemies
{
	[RequireComponent(typeof(HealthBehaviour))]
	class EnemyDeathBehaviour : MonoBehaviour
	{
		private HealthBehaviour _healthBehaviour;
		public ParticleSystem deathParticle;

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
		}

		void Explode()
		{
			//Instantiate our one-off particle system
			ParticleSystem explosionEffect = Instantiate(deathParticle) as ParticleSystem;
			Vector3 spawnPosition = new Vector3(transform.position.x,transform.position.y,transform.position.z-2);
			explosionEffect.transform.position = spawnPosition;
			
			//play it
			//explosionEffect.main.loop = false;
			explosionEffect.Play();

			//destroy the particle system when its duration is up, right
			//it would play a second time.
			Destroy(explosionEffect.gameObject, explosionEffect.main.duration);
		}
	}
}
