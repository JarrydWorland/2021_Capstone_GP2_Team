using UnityEngine;
using Weapons;
using UnityEngine.InputSystem;

namespace Player
{
	public class PlayerShootBehaviour : MonoBehaviour
	{
		[Tooltip("The players default weapon")]
		public BaseWeapon DefaultWeapon;

		public BaseWeapon Weapon => DefaultWeapon;
		private Animator[] _animators;

		[SerializeField] private AudioClip weaponFired;
		protected AudioSource soundSource;

		private const float AimDeadzone = 0.1f;

		private void Start()
		{
			_animators = GetComponentsInChildren<Animator>();

			soundSource = this.gameObject.AddComponent<AudioSource>();
			soundSource.loop = false;
			soundSource.playOnAwake = false;
			soundSource.volume = 0.5f;

			if (weaponFired != null)
				soundSource.clip = weaponFired;
		}

		public void Update()
		{
			foreach(Animator animator in GetComponentsInChildren<Animator>())
			{
				animator.SetFloat("AimingX", Weapon.Direction.x);
				animator.SetFloat("AimingY", Weapon.Direction.y);
			}
		}

		public void OnAim(InputAction.CallbackContext context)
		{
			if (context.control.device.name == "Mouse")
			{
				// If the current device is keyboard and mouse, aim towards the cursor.
				Vector2 mousePosition = Mouse.current.position.ReadValue();
				Vector2 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
				Vector2 direction = mousePositionInWorld - (Vector2) transform.position;
				Weapon.Direction = direction;
			}
			else
			{
				// If the current device is not keyboard and mouse, we assume it's a controller
				// as it's the only other device specified in the InputActions file.

				// Aim in the direction of the right stick movement.
				Vector2 value = context.ReadValue<Vector2>();
				if (value.sqrMagnitude >= AimDeadzone) Weapon.Direction = value;
			}
		}

		// Called by Unitys input system
		public void OnShoot(InputAction.CallbackContext context)
		{
			if (context.performed)
			{
				Weapon.Shoot();
				if (weaponFired != null)
				{
					soundSource.pitch = Random.Range(0.7f, 1.5f);
					soundSource.Play();
				}

				foreach (Animator animator in _animators)
				{
					animator.SetTrigger("Aiming");
				}
			}
			else if (context.canceled)
			{
				Weapon.Holster();
				foreach (Animator animator in _animators)
				{
					animator.SetTrigger("Holster");
				}
			}
		}

		//public void SetWeapon(BaseWeapon weapon)
		//{
		//	Weapon = weapon;
		//}
	}
}
