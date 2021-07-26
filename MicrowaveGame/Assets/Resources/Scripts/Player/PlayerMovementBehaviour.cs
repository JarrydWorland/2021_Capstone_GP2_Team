using UnityEngine;
using UnityEngine.InputSystem;
using Scripts.Utilities;

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
		public Vector2 Velocity { get; private set; }
		

		private Rigidbody2D _rigidBody;


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
			Direction = context.ReadValue<Vector2>();
		}
	}
}
