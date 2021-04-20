using UnityEngine;
using static DoorDirection;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public Rigidbody2D rigidBody;
        public float speed = 5.0f;

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
            rigidBody.MovePosition(rigidBody.position + _velocity * (speed * Time.fixedDeltaTime));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Door door = other.gameObject.GetComponent<Door>();
            LevelManager.Instance.ChangeRoom(door);

            Vector3 otherDoor = door.ConnectingDoor.transform.position;

            otherDoor += 1.25f * door.direction switch
            {
                North => Vector3.up,
                East => Vector3.right,
                South => Vector3.down,
                West => Vector3.left
            };

            transform.position = new Vector3(otherDoor.x, otherDoor.y, transform.position.z);
        }
    }
}