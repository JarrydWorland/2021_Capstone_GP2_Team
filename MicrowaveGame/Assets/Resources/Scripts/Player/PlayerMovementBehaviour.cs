using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Player
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class PlayerMovementBehaviour : MonoBehaviour
	{
		/// <summary>
		/// The players movement speed.
		/// </summary>
		public float Speed;

		/// <summary>
		/// The direction the player is moving towards.
		/// </summary>
		public Vector2 Direction { get; private set; }

		private Rigidbody2D _rigidBody;

		private void Start()
		{
			_rigidBody = GetComponent<Rigidbody2D>();
		}

		private void FixedUpdate()
		{
			_rigidBody.MovePosition(_rigidBody.position + Direction * (Speed * Time.fixedDeltaTime));
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
