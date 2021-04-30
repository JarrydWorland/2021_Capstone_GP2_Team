using UnityEngine;
using Level;
using UnityEngine.InputSystem;
using System.Collections;
using System;

namespace Player
{
	public class PlayerMovement : MonoBehaviour
	{
		public Rigidbody2D RigidBody;
		public float Speed = 5.0f;
		private float _maxSpeed = 20.0f;
		private float _minSpeed = 5.0f;
		private float _increaseSpeed = 5.0f;
		private float _decreaseSpeed = 5.0f;

		private Vector2 _velocity;

		public Vector2 Velocity
		{
			get => _velocity;
            set
            {
				_velocity.x = value.x.Clamp( _minSpeed, _maxSpeed);
				_velocity.y = value.y.Clamp( _minSpeed, _maxSpeed);
			}
		}

		private void FixedUpdate()
		{
			RigidBody.MovePosition(RigidBody.position + Velocity * (Speed * Time.fixedDeltaTime));
		}

		public IEnumerator SpeedTimer()
		{
			Speed += _increaseSpeed;
			yield return new WaitForSecondsRealtime(5.0f);
			Speed -= _decreaseSpeed;
		}


		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.GetComponent<Door>() != null)
			{
				Door door = other.gameObject.GetComponent<Door>();
				LevelManager.Instance.ChangeRoom(door);

				Vector2 otherDoor = door.ConnectingDoor.transform.position;
				otherDoor += 1.25f * door.Direction.ToVector2();

				transform.position = new Vector3(otherDoor.x, otherDoor.y, transform.position.z);
			}

		}

		public void OnMove(InputAction.CallbackContext context)
		{
			_velocity = context.ReadValue<Vector2>();
		}
	}
}