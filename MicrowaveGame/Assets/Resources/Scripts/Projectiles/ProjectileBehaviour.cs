using Scripts.Utilities;
using UnityEngine;

namespace Scripts.Projectiles
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class ProjectileBehaviour : MonoBehaviour
	{
		/// <summary>
		/// The speed the projectile will move at.
		/// </summary>
		public float Speed { get; protected set; }

		/// <summary>
		/// The direction the projectile will move towards.
		/// </summary>
		public Vector2 Direction { get; protected set; }

		/// <summary>
		/// The hitpoints the projectile will deal on impact.
		/// </summary>
		public int Damage { get; protected set; }

		/// <summary>
		/// The TagBehaviour tag the projectile is targeting.
		/// </summary>
		public string TargetTag { get; protected set; }

		private BoxCollider2D _boxCollider2D;
		private Vector3 _startingPosition;

		private void Start()
		{
			_boxCollider2D = GetComponent<BoxCollider2D>();
		}

		protected virtual void Update()
		{
			Vector2 distance = _startingPosition - transform.position;
			if (distance.sqrMagnitude > 1000) Destroy(gameObject);
		}

		public virtual void Init(Vector2 position, Vector2 direction, float speed, int damage, string targetTag)
		{
			transform.position = position;
			transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
			_startingPosition = position;
			Direction = direction;
			Speed = speed;
			Damage = damage;
			TargetTag = targetTag;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			TagBehaviour tagBehaviour = other.GetComponent<TagBehaviour>();
			if (tagBehaviour == null) return;

			// Destroy when impacting solid objects
			if (tagBehaviour.HasTag("Solid"))
			{
				Destroy(gameObject);
			}

			// Damage target
			if (tagBehaviour.HasTag(TargetTag))
			{
				HealthBehaviour healthBehaviour = other.GetComponent<HealthBehaviour>();
				if (healthBehaviour != null)
				{
					healthBehaviour.Value -= Damage;
					Destroy(gameObject);
				}
			}
		}
	}
}
