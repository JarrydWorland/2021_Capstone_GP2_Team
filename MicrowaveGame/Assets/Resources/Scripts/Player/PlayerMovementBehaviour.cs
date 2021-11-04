using UnityEngine;
using UnityEngine.InputSystem;
using Scripts.Utilities;
using UnityEngine.SceneManagement;
using Scripts.Audio;

namespace Scripts.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
	public class PlayerMovementBehaviour : MonoBehaviour
	{
		/// <summary>
		/// The players movement acceleration.
		/// </summary>
		public float Acceleration = 1.0f;

		/// <summary>
		/// The players movement friction.
		/// </summary>
		public float Friction = 0.01f;

		/// <summary>
		/// The players maximum movement velocity.
		/// </summary>
		public float MaxVelocity = 0.15f;

		/// <summary>
		/// The direction the player is moving towards.
		/// </summary>
		public Vector2 Direction { get; private set; }

		/// <summary>
		/// The players movement velocity.
		/// </summary>
		public Vector2 Velocity { get; set; }

		private Rigidbody2D _rigidBody;

		public AudioClip moveSfx;

		private void Start()
		{
			_rigidBody = GetComponent<Rigidbody2D>();
		}

		private void FixedUpdate()
		{
			// update velocity
			Velocity += Direction * (Acceleration * Time.fixedDeltaTime);

			// apply friction
			Velocity = new Vector2(
				(Mathf.Abs(Velocity.x) - Friction).Clamp(0, MaxVelocity) * Mathf.Sign(Velocity.x),
				(Mathf.Abs(Velocity.y) - Friction).Clamp(0, MaxVelocity) * Mathf.Sign(Velocity.y)
			);

			// apply velocity
			_rigidBody.MovePosition(_rigidBody.position + Velocity);
		}

		/// <summary>
		/// Called on move event from unity input system.
		/// </summary>
		public void OnMove(InputAction.CallbackContext context)
		{
			if (!SceneManager.GetActiveScene().isLoaded) return;

			Direction = context.ReadValue<Vector2>();
			AudioManager.Play(moveSfx, AudioCategory.Effect, 0.07f);
		}

		public void OnTriggerEnter2D(Collider2D other)
		{
			// TODO: velocity should be reset when hitting solid objects, below
			// is a possible way to do that.

			/* TagBehaviour tagBehaviour = other.GetComponent<TagBehaviour>(); */
			/* if (tagBehaviour != null && tagBehaviour.HasTag("Solid")) */
			/* { */
			/* 	Velocity = Vector2.zero; */
			/* } */
		}
	}
}
