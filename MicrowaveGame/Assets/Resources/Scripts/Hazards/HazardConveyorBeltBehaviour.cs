using Scripts.Player;
using UnityEngine;

namespace Scripts.Hazards
{
	public class HazardConveyorBeltBehaviour : HazardBehaviour
	{
		// TODO: Support interacting with enemies.

		// We only want to check if the wheel of the player is colliding with the conveyor belt.
		// There's a separate sub-object on the player called "Wheel" that contains a BoxCollider2D component
		// for the wheel area that we check for collision on.

		/// <summary>
		/// The factor to increase the friction by.
		/// </summary>
		public float FrictionFactor = 1.5f;

		private PlayerMovementBehaviour _playerMovementBehaviour;

		private bool _colliding;

		private void Start()
		{
			_playerMovementBehaviour = GameObject.Find("Player").GetComponent<PlayerMovementBehaviour>();
		}

		private void Update()
		{
			if (_colliding)
			{
				float radians = (gameObject.transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad;
				_playerMovementBehaviour.Velocity += new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.name == "Wheel")
			{
				_playerMovementBehaviour.Friction *= FrictionFactor;
				_colliding = true;
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.gameObject.name == "Wheel")
			{
				_playerMovementBehaviour.Friction /= FrictionFactor;
				_colliding = false;
			}
		}
	}
}