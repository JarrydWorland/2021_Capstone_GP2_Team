using UnityEngine;
using Level;
using UnityEngine.InputSystem;
using System.Collections;

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

		private void FixedUpdate()
		{
			RigidBody.MovePosition(RigidBody.position + _velocity * (Speed * Time.fixedDeltaTime));
		}

		public void check()
		{
			if (Speed > _maxSpeed)
			{
				Speed = _maxSpeed;
			}
			if (Speed < _minSpeed)
			{
				Speed = _minSpeed;
			}
		}

		public IEnumerator SpeedTimer()
		{
			check();
			Speed += _increaseSpeed;
			yield return new WaitForSecondsRealtime(5.0f);
			Speed -= _decreaseSpeed;
			check();
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