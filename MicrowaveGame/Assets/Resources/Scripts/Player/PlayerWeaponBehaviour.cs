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

		public Transform ProjectileSpawn { get; private set; }

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

		/// <summary>
		/// The direction the player is looking towards.
		/// </summary>
		public Vector2 LookDirection
		{
			get => _lookDirection;
			set => _lookDirection = value.normalized;
		}




		private Vector2 _direction = Vector2.up;
		private Vector2 _lookDirection = Vector2.up;


		/// <summary>
		/// Whether or not the player is currently shooting.
		/// </summary>
		public bool Shooting { get; private set; }

		private const float AimDeadzone = 0.1f;

		/// <summary>
		/// Any additional animation the player may deal on top of the weapon's damage.
		/// </summary>
		public int AdditionalDamage { get; set; }

		private bool _isCurrentInputMouse;
		private Vector2 _lastMousePositionInWorld;

		private void Start()
		{
			ProjectileSpawn = transform.Find("ProjectileSpawn");
			_defaultWeaponBehaviour = DefaultWeapon.GetComponent<WeaponBehaviour>();
			EquippedWeaponBehaviour = _defaultWeaponBehaviour;

			// default weapon is never instantiated so manually run start method
			_defaultWeaponBehaviour.Start();
		}

		private void Update()
		{
			if (_isCurrentInputMouse)
			{
				Direction = _lastMousePositionInWorld - (Vector2) ProjectileSpawn.position;
				LookDirection = _lastMousePositionInWorld - (Vector2) transform.position;
			}

			// default weapon is never instantiated so manually run update item method
			_defaultWeaponBehaviour.OnUpdateItem(null);
		}

		/// <summary>
		/// Called on look event from unity input system.
		/// </summary>
		public void OnLook(InputAction.CallbackContext context)
		{
			_isCurrentInputMouse = true;

			Vector2 mousePosition = Mouse.current.position.ReadValue();
			_lastMousePositionInWorld = (Vector2) UnityEngine.Camera.main.ScreenToWorldPoint(mousePosition);
		}

		/// <summary>
		/// Called on look gamepad event from unity input system.
		/// </summary>
		public void OnLookGamepad(InputAction.CallbackContext context)
		{
			_isCurrentInputMouse = false;

			Vector2 value = context.ReadValue<Vector2>();
			if (value.sqrMagnitude >= AimDeadzone) Direction = LookDirection = value;
		}

		/// <summary>
		/// Called on shoot event from unity input system.
		/// </summary>
		public void OnShoot(InputAction.CallbackContext context)
		{
			if (!enabled) return;

			if (context.performed) Shooting = true;
			else if (context.canceled) Shooting = false;
		}
	}
}
