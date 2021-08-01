using Scripts.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Player
{
    public class PlayerWeaponBehaviour : MonoBehaviour
	{
		/// <summary>
		/// The default weapon prefab.
		/// </summary>
		public GameObject DefaultWeapon;
		private WeaponBehaviour _defaultWeaponBehaviour;


		/// <summary>
		/// The WeaponBehaviour of the currently equipped weapon.
		/// </summary>
		public WeaponBehaviour EquippedWeaponBehaviour
		{ 
			get => _equippedWeaponBehaviour;
			set => _equippedWeaponBehaviour = value ?? _defaultWeaponBehaviour;
		}
		private WeaponBehaviour _equippedWeaponBehaviour;

		/// <summary>
		/// The direction the player is shooting towards.
		/// </summary>
		public Vector2 Direction
		{
			get => _direction;
			set => _direction = value.normalized;
		}
		private Vector2 _direction;


		/// <summary>
		/// Whether or not the player is currently shooting.
		/// </summary>
		public bool Shooting { get; private set; }

		private const float AimDeadzone = 0.1f;
		
		/// <summary>
		/// Any additional animation the player may deal on top of the weapon's damage.
		/// </summary>
		public int AdditionalDamage { get; set; }

		private void Start()
		{
			_defaultWeaponBehaviour = DefaultWeapon.GetComponent<WeaponBehaviour>();
			EquippedWeaponBehaviour = _defaultWeaponBehaviour;

			// default weapon is never instantiated so manually run start method
			_defaultWeaponBehaviour.Start();
			
		}

		private void Update()
		{
			// default weapon is never instantiated so manually run update item method
			_defaultWeaponBehaviour.OnUpdateItem(null);
		}

		/// <summary>
		/// Called on look event from unity input system.
		/// </summary>
		public void OnLook(InputAction.CallbackContext context)
		{
			if (context.control.device.name == "Mouse")
			{
				// If the current device is keyboard and mouse, aim towards the cursor.
				Vector2 mousePosition = Mouse.current.position.ReadValue();
				Vector2 mousePositionInWorld = UnityEngine.Camera.main.ScreenToWorldPoint(mousePosition);
				Direction = mousePositionInWorld - (Vector2) transform.position;
			}
			else
			{
				// If the current device is not keyboard and mouse, we assume it's a controller
				// as it's the only other device specified in the InputActions file.

				// Aim in the direction of the right stick movement.
				Vector2 value = context.ReadValue<Vector2>();
				if (value.sqrMagnitude >= AimDeadzone) Direction = value;
			}
		}

		/// <summary>
		/// Called on shoot event from unity input system.
		/// </summary>
		public void OnShoot(InputAction.CallbackContext context)
		{
			if (context.performed) Shooting = true;
			else if (context.canceled) Shooting = false;
		}
	}
}
