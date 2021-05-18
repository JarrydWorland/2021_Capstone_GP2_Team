using UnityEngine;
using Level;
using UnityEngine.InputSystem;
using System.Collections;
using Player.Weapons;
using System;

namespace Player
{
	public class PlayerMovement : MonoBehaviour
	{
		public GameObject WeaponPrefab;
		private GameObject _weaponObject;
		private BaseWeapon _weapon;

		public Rigidbody2D RigidBody;
		private float _speed = 10.0f;
		private float _maxSpeed = 20.0f;
		private float _minSpeed = 5.0f;
		//private float _maxVelocity = 20.0f;
		//private float _minVelocity = 5.0f;

		private Vector2 _velocity;

		public float Speed
		{
			get => _speed;
			set { _speed = value.Clamp(_minSpeed, _maxSpeed); }
		}

		public Vector2 Velocity
		{
			get => _velocity;
            set
            {
				_velocity.x = value.x; //.Clamp(_minVelocity, _maxVelocity);
				_velocity.y = value.y; //.Clamp(_minVelocity, _maxVelocity);

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

		private void Start()
		{
			_weaponObject = Instantiate(WeaponPrefab, transform, true);
			_weaponObject.transform.position = transform.position;
			_weapon = _weaponObject.GetComponent<BaseWeapon>();
		}

		private void FixedUpdate()
		{
			RigidBody.MovePosition(RigidBody.position + Velocity * (Speed * Time.fixedDeltaTime));
		}

		private void Update()
		{
			// Get the direction the player is currently aiming in.
			Vector2 mousePosition = Mouse.current.position.ReadValue();
			Vector2 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
			Vector2 direction = mousePositionInWorld - (Vector2)transform.position;
			direction.Normalize();

			foreach(Animator animator in GetComponentsInChildren<Animator>())
			{
				animator.SetFloat("AimingX", direction.x);
				animator.SetFloat("AimingY", direction.y);
			}

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

		public void OnShoot(InputAction.CallbackContext context)
		{
			if (context.performed) 
			{
				_weapon.Shoot();
				foreach(Animator animator in GetComponentsInChildren<Animator>())
				{
					animator.SetTrigger("Aiming");
				}
			}
			else if (context.canceled)
			{
				_weapon.Holster();
				foreach(Animator animator in GetComponentsInChildren<Animator>())
				{
					animator.SetTrigger("Holster");
				}
			}
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
