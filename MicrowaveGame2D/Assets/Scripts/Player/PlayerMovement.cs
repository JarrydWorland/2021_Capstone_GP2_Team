using UnityEngine;

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
    }
}