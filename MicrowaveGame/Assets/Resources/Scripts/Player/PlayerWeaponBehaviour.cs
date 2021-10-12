using Scripts.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;
using Scripts.Utilities;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Scripts.Player
{
	public class PlayerWeaponBehaviour : MonoBehaviour
	{
		/// <summary>
		/// The default weapon prefab.
		/// </summary>
		public GameObject DefaultWeapon;

		/// <summary>
		/// The angle in degrees of the cone of activation for controller aim
		/// assist.
		/// </summary>
		[Range(0.0f, 360.0f)]
		public float AimAssistConeAngle = 45.0f;

		public Transform ProjectileSpawn { get; private set; }

		private WeaponBehaviour _defaultWeaponBehaviour;

		private Transform _aimIndicator;

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
			private set => _direction = value.normalized;
		}

		/// <summary>
		/// The direction the player is looking towards.
		/// </summary>
		public Vector2 InputDirection
		{
			get => _inputDirection;
			private set => _inputDirection = value.normalized;
		}

		private Vector2 _direction = Vector2.up;
		private Vector2 _inputDirection = Vector2.up;


		/// <summary>
		/// Whether or not the player is currently shooting.
		/// </summary>
		public bool Shooting { get; private set; }

		private const float AimDeadzone = 0.1f;

		/// <summary>
		/// Any additional animation the player may deal on top of the weapon's damage.
		/// </summary>
		public int AdditionalDamage { get; set; }

		private bool _isCurrentInputMouse = true;
		private Vector2 _lastMousePositionInWorld;

		private void Start()
		{
			_defaultWeaponBehaviour = DefaultWeapon.GetComponent<WeaponBehaviour>();
			EquippedWeaponBehaviour = _defaultWeaponBehaviour;
			// default weapon is never instantiated so manually run start method
			_defaultWeaponBehaviour.Start();

			ProjectileSpawn = transform.Find("ProjectileSpawn");
			_aimIndicator = transform.Find("AimIndicator");
		}

		private void Update()
		{
			if (_isCurrentInputMouse)
			{
				_aimIndicator.gameObject.SetActive(false);
				Direction = _lastMousePositionInWorld - (Vector2) ProjectileSpawn.position;
				InputDirection = _lastMousePositionInWorld - (Vector2) transform.position;
				Look();
			}
			else
			{
				float aimAssistConeAngle = AimAssistConeAngle.MapBetween(0, 360, 1.0f, -1.0f);
				List<GameObject> enemies = TagBehaviour.FindWithTag("Enemy").ToList();
				enemies.Sort((lhs, rhs) => Vector3
					.Distance(transform.position, lhs.transform.position)
					.CompareTo(Vector3.Distance(transform.position, rhs.transform.position)));

				foreach (GameObject enemy in enemies)
				{
					Vector3 enemyDirection = (enemy.transform.position - ProjectileSpawn.position).normalized;
					Vector3 enemyInputDirection = (enemy.transform.position - transform.position).normalized;
					if (Vector3.Dot(InputDirection, enemyInputDirection) > aimAssistConeAngle)
					{
						Direction = enemyDirection;
						break;
					}
				}

				_aimIndicator.gameObject.SetActive(true);
				_aimIndicator.GetComponentInChildren<SpriteRenderer>().color = Color.white;
				_aimIndicator.transform.rotation = Quaternion.FromToRotation(Vector3.right, InputDirection);
			}

			// default weapon is never instantiated so manually run update item method
			_defaultWeaponBehaviour.OnUpdateItem(null);
		}

		/// <summary>
		/// Called on look event from unity input system.
		/// </summary>
		public void OnLook(InputAction.CallbackContext context)
		{
			if (!SceneManager.GetActiveScene().isLoaded) return;

			_isCurrentInputMouse = true;

			Vector2 mousePosition = Mouse.current.position.ReadValue();
			_lastMousePositionInWorld = (Vector2) UnityEngine.Camera.main.ScreenToWorldPoint(mousePosition);
		}

		/// <summary>
		/// Copy of On look event from unity input system, just applied in Update.
		/// </summary>
		private void Look()
		{
			if (!SceneManager.GetActiveScene().isLoaded) return;

			_isCurrentInputMouse = true;

			Vector2 mousePosition = Mouse.current.position.ReadValue();
			_lastMousePositionInWorld = (Vector2)UnityEngine.Camera.main.ScreenToWorldPoint(mousePosition);
		}

		/// <summary>
		/// Called on look gamepad event from unity input system.
		/// </summary>
		public void OnLookGamepad(InputAction.CallbackContext context)
		{
			if (!SceneManager.GetActiveScene().isLoaded) return;

			_isCurrentInputMouse = false;

			Vector2 value = context.ReadValue<Vector2>();
			if (value.sqrMagnitude >= AimDeadzone) Direction = InputDirection = value;
		}

		/// <summary>
		/// Called on shoot event from unity input system.
		/// </summary>
		public void OnShoot(InputAction.CallbackContext context)
		{
			if (!SceneManager.GetActiveScene().isLoaded) return;
		
			if (!enabled) return;

			if (context.performed) Shooting = true;
			else if (context.canceled) Shooting = false;
		}
	}
}
