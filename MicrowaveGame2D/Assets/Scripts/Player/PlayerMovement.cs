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
		private float _speed = 5.0f;
		private float _maxSpeed = 20.0f;
		private float _minSpeed = 5.0f;
		private float _maxVelocity = 20.0f;
		private float _increaseSpeed = 5.0f;
		private float _decreaseSpeed = 5.0f;

		private Vector2 _velocity;

		public float Speed
        {
			get => _speed;
			set
            {
				_speed.Clamp(_minSpeed, _maxSpeed);
			}
        }

		public Vector2 Velocity
		{
			get => _velocity;
            set
            {
				_velocity.x = value.x;
				_velocity.y = value.y;

				foreach(Animator animator in GetComponentsInChildren<Animator>())
				{
					// only set animator velocity if its not zero so that idle
					// knows the last facing direction
					if (Math.Abs(Velocity.x) > 0 || Math.Abs(Velocity.y) > 0)
					{
						animator.SetFloat("VelocityX", Velocity.x);
						animator.SetFloat("VelocityY", Velocity.y);
					}
					animator.SetFloat("Speed", Velocity.sqrMagnitude);
				}
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
			Velocity = context.ReadValue<Vector2>();
		}

		// TODO: Temporary input events for debugging, remove here and from
		// input system.

		public void OnHeal(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				GetComponent<Health>().Value += 4;
				print("Pressed?");
			}
		}

		public void OnDamage(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				GetComponent<Health>().Value -= 3;
				print("Pressed?");
			}
		}
	}
}
