using UnityEngine;
using Weapons;
using UnityEngine.InputSystem;

namespace Player
{
	public class PlayerShootBehaviour : MonoBehaviour
	{
		[Tooltip("The players default weapon")]
		public BaseWeapon DefaultWeapon;

		private BaseWeapon _weapon;
		private Animator[] _animators;

		private const float AimDeadzone = 0.1f;

		private void Start()
		{
			_weapon = DefaultWeapon;
			_animators = GetComponentsInChildren<Animator>();
		}

		public void OnAim(InputAction.CallbackContext context)
		{
			if (context.control.device.name == "Mouse")
			{
				// If the current device is keyboard and mouse, aim towards the cursor.
				Vector2 mousePosition = Mouse.current.position.ReadValue();
				Vector2 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
				Vector2 direction = mousePositionInWorld - (Vector2) transform.position;
				_weapon.Direction = direction;
			}
			else
			{
				// If the current device is not keyboard and mouse, we assume it's a controller
				// as it's the only other device specified in the InputActions file.

				// Aim in the direction of the right stick movement.
				Vector2 value = context.ReadValue<Vector2>();
				if (value.sqrMagnitude >= AimDeadzone) _weapon.Direction = value;
			}
		}

		// Called by Unitys input system
		public void OnShoot(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				_weapon.Shoot();
				foreach (Animator animator in _animators)
				{
					animator.SetTrigger("Aiming");
				}
			}
			else if (context.canceled)
			{
				_weapon.Holster();
				foreach (Animator animator in _animators)
				{
					animator.SetTrigger("Holster");
				}
			}
		}

		public void SetWeapon(BaseWeapon weapon)
		{
			_weapon = weapon;
		}
	}
}