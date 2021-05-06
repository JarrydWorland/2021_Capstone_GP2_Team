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
		private float _speed = 10.0f;
		private float _maxSpeed = 20.0f;
		private float _minSpeed = 5.0f;
		private float _maxVelocity = 20.0f;
		private float _minVelocity = 5.0f;
		private float _increaseSpeed = 5.0f;
		private float _decreaseSpeed = 5.0f;
		//private float _meltDamage = 0.2f;

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
				_velocity.x = value.x.Clamp(_minVelocity, _maxVelocity);
				_velocity.y = value.y.Clamp(_minVelocity, _maxVelocity);
			}
		}

		private void FixedUpdate()
		{
			RigidBody.MovePosition(RigidBody.position + Velocity * (Speed * Time.fixedDeltaTime));
		}

		public IEnumerator SpeedTimer()
		{
			_speed += _increaseSpeed;
			yield return new WaitForSecondsRealtime(5.0f);
			_speed -= _decreaseSpeed;
		}

		public IEnumerator SlowTimer()
		{
			_speed -= _decreaseSpeed;
			yield return new WaitForSecondsRealtime(5.0f);
			_speed += _increaseSpeed;
		}

		public IEnumerator MeltTimer()
		{
			_speed -= _decreaseSpeed;
			//Health.value -= _meltDamage * Time.deltaTme; Will decrease health by 0.2f.
			yield return new WaitForSecondsRealtime(5.0f);
			_speed += _increaseSpeed;
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