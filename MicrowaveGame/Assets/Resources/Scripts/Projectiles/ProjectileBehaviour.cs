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

		/// <summary>
		/// The specific game object that is being homed in on if any.
		/// </summary>
		public GameObject HomingTarget { get; protected set; }

		/// <summary>
		/// The strength that the projectiles should home in on its taget. null for none.
		/// </summary>
		public int HomingStrength { get; protected set; }

		/// <summary>
		/// The audio clip to play when wall is hit.
		/// </summary>

		private BoxCollider2D _boxCollider2D;
		protected Vector3 _startingPosition;
		private ParticleSystem _sparksEffect;

		private void Start()
		{
			_boxCollider2D = GetComponent<BoxCollider2D>();
			_sparksEffect = GetComponentInChildren<ParticleSystem>();
		}

		protected virtual void Update()
		{
			if (HomingTarget != null && HomingStrength > 0)
			{
				transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg);
			}

			Vector2 distance = _startingPosition - transform.position;
			if (distance.sqrMagnitude > 1000) Destroy(gameObject);
		}

		public virtual void Init(Vector2 position, Vector2 direction, float scale, float speed, int damage, string targetTag, GameObject homingTarget, int homingStrength)
		{
			transform.position = position;
			transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
			transform.localScale *= scale;
			_startingPosition = position;
			Direction = direction;
			Speed = speed;
			Damage = damage;
			TargetTag = targetTag;
			HomingTarget = homingTarget;
			HomingStrength = homingStrength;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			TagBehaviour tagBehaviour = other.GetComponent<TagBehaviour>();
			if (tagBehaviour == null) return;

			// Destroy when impacting solid objects
			if (tagBehaviour.HasTag("Solid"))
			{
				Sparks();
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

		void Sparks()
        {
			//Instantiate our one-off particle system
			ParticleSystem sparksEffect = Instantiate(_sparksEffect) as ParticleSystem;
			Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2); //have to use -2 because sprite ordering is too difficult to implement this late in the project.
			sparksEffect.transform.position = spawnPosition;

			//play it
			sparksEffect.Play();

			//destroy the particle system when its duration is up
			Destroy(sparksEffect.gameObject, sparksEffect.main.duration);
		}
	}
}
