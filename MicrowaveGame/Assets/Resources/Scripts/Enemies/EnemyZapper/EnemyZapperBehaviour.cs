using UnityEngine;
using Scripts.Utilities;

namespace Scripts.Enemies.EnemyZapper
{
	[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
	public class EnemyZapperBehaviour : MonoBehaviour
	{
		/// <summary>
		/// The speed the zapper is traveling at.
		/// </summary>
		public float Speed;

		/// <summary>
		/// The minimum distance from the player that the zapper will attempt
		/// to attack within.
		/// </summary>
		public float AttackDistance;
		
		/// <summary>
		/// The direction the zapper is moving towards.
		/// </summary>
		public Vector2 Direction { get; private set; }

		private bool IsWalking => _animator.GetCurrentAnimatorStateInfo(0).IsName("Walk");
		
		private Rigidbody2D _rigidBody;
		private Animator _animator;
		private GameObject _shockGameObject;

		private void Start()
		{
			Direction = Vector2.up;

			_rigidBody = GetComponent<Rigidbody2D>();
			_animator = GetComponent<Animator>();
			_shockGameObject = Resources.Load<GameObject>("Prefabs/Enemies/EnemyZapper/EnemyZapperShock");
		}

		private void Update()
		{
			if (!IsWalking) return;

			// update direction
			Transform player = GameObject.Find("Player").transform;
			Direction = (player.position - transform.position).normalized;

			// update animator
			Vector2 direction = Direction.ToDirection().ToVector2();
			GetComponent<Animator>().SetFloat("DirectionX", direction.x);
			GetComponent<Animator>().SetFloat("DirectionY", direction.y);

			// attack if close to player
			if (Vector2.Distance(player.position, transform.position) < AttackDistance)
			{
				_animator.Play("Charge");
			}
		}

		/// <summary>
		/// Spawn a EnemyZapperShock, this is called by a callback within the
		/// EnemyZapper attack animation.
		/// </summary>
		public void Attack()
		{
			Instantiate(_shockGameObject, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
		}

		private void FixedUpdate()
		{
			if (!IsWalking) return;
			Vector2 movement = Direction * (Speed * Time.fixedDeltaTime);
			_rigidBody.MovePosition(_rigidBody.position + movement);
		}
	}
}