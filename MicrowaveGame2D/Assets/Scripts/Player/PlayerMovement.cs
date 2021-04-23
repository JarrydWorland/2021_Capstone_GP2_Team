using UnityEngine;
using Level;

namespace Player
{
	public class PlayerMovement : MonoBehaviour
	{
		public Rigidbody2D RigidBody;
		public float Speed = 5.0f;

		private Vector2 _velocity;

		//Update called once /frame, based on frame
		void Update()
		{
			_velocity.x = Input.GetAxisRaw("Horizontal");
			_velocity.y = Input.GetAxisRaw("Vertical");
		}

		// Update not based on frame
		void FixedUpdate()
		{
			RigidBody.MovePosition(RigidBody.position + _velocity * (Speed * Time.fixedDeltaTime));
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
	}
}
