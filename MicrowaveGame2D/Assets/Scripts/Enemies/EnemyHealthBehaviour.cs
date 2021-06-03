using UnityEngine;

namespace Enemy
{
	[RequireComponent(typeof(Health))]
	public class EnemyHealthBehaviour : MonoBehaviour
	{

		[SerializeField] private AudioClip enemyDestroyed;
		protected AudioSource soundSource;

		private Health _health;

		private void Start()
		{
			_health = GetComponent<Health>();

			soundSource = this.gameObject.AddComponent<AudioSource>();
			soundSource.loop = false;
			soundSource.playOnAwake = false;

			if (enemyDestroyed != null)
				soundSource.clip = enemyDestroyed;
		}

		private void Update()
		{
			if (_health.Value == 0)
			{
				if (enemyDestroyed != null)
					soundSource.pitch = Random.Range(0.9f, 1.5f);
				soundSource.Play();
				Destroy(gameObject);
			}
		}
	}
}
