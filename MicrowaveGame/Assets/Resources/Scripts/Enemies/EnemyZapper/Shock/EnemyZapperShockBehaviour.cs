using UnityEngine;

namespace Scripts.Enemies.EnemyZapper.Shock
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class EnemyZapperShockBehaviour : MonoBehaviour
	{
		/// <summary>
		/// Amount of damage to deal to the player.
		/// </summary>
		public int Damage;

		/// <summary>
		/// How long the shock should exist for.
		/// </summary>
		public float DurationSeconds;

		private float _startTime;
		private BoxCollider2D _boxCollider2D;

		private void Start()
		{
			_startTime = Time.time;
			_boxCollider2D = GetComponent<BoxCollider2D>();
		}

		private void Update()
		{
			if (Time.time - _startTime > DurationSeconds)
			{
				Destroy(gameObject);
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.name == "Player")
			{
				HealthBehaviour healthBehaviour = other.GetComponent<HealthBehaviour>();
				healthBehaviour.Value -= Damage;
				_boxCollider2D.enabled = false;
			}
		}
	}
}
