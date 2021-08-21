using Scripts.Dialogue;
using Scripts.Menus;
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
		public Vector2 Velocity { get; set; }

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

		// Temporary for testing the dialogue menu.
		private bool _firstLaunch;

		/// <summary>
		/// Called on move event from unity input system.
		/// </summary>
		public void OnMove(InputAction.CallbackContext context)
		{
			if (!_firstLaunch)
			{
				_firstLaunch = true;

				// Temporarily show the dialogue menu for testing.
				DialogueContentBehaviour dialogueContentBehaviour = GetComponent<DialogueContentBehaviour>();
				MenuManager.ShowDialogue(dialogueContentBehaviour.dialogueContent);
			}

			Direction = context.ReadValue<Vector2>();
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