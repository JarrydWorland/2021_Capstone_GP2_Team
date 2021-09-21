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
			Vector3 spawnPosition = new Vector3(transform.position.x,transform.position.y,transform.position.z-2); //have to use -2 because sprite ordering is too difficult to implement this late in the project.
			explosionEffect.transform.position = spawnPosition;
			
			//play it
			explosionEffect.Play();

			//destroy the particle system when its duration is up
			Destroy(explosionEffect.gameObject, explosionEffect.main.duration);
		}
	}
}
