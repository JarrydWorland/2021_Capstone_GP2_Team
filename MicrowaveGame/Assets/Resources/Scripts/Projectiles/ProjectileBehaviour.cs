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
			_startingPosition = position;
			Direction = direction;
			Speed = speed;
			Damage = damage;
			TargetTag = targetTag;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.GetComponent<TagBehaviour>()?.HasTag(TargetTag) ?? false)
			{
				// TODO: deal damage
			}
		}
	}
}
