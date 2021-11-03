using UnityEngine;
using UnityEngine.InputSystem;
using Scripts.Utilities;
using System.Linq;
using System.Collections.Generic;
using Scripts.Audio;
using UnityEngine.SceneManagement;

namespace Scripts.Player
{
	public class PlayerShootBehaviour : MonoBehaviour
	{
		/// <summary>
		/// The audio clip used when shooting.
		/// </summary>
		public AudioClip ShootAudioClip;

		/// <summary>
		/// How many bullets should be fired every second (i.e. 2.0f will shoot two bullets every second).
		/// </summary>
		public float FireRate;

		/// <summary>
		/// The amount of damage a bullet will deal to an entity.
		/// </summary>
		public int ProjectileDamage;

		/// <summary>
		/// The speed at which bullets will travel at.
		/// </summary>
		public float ProjectileSpeed;

		/// <summary>
		/// The angle, in degrees, of the cone of activation for controller aim assist.
		/// </summary>
		[Range(0.0f, 360.0f)]
		public float AimAssistConeAngle = 45.0f;

		private Vector2 _direction = Vector2.up;

		/// <summary>
		/// The direction the player is currently aiming in.
		/// <remarks>When using a controller, the direction bullets are fired at are different
		/// to the raw direction the player is inputting.</remarks>
		/// </summary>
		private Vector2 Direction
		{
			get => _direction;
			set => _direction = value.normalized;
		}

		private Vector2 _rawDirection = Vector2.up;

		/// <summary>
		/// The raw direction the player is currently aiming in.
		/// </summary>
		public Vector2 RawDirection
		{
			get => _rawDirection;
			private set => _rawDirection = value.normalized;
		}

		/// <summary>
		/// Whether or not the player is currently shooting.
		/// </summary>
		public bool Shooting { get; private set; }

		/// <summary>
		/// Any additional animation the player may deal on top of the weapon's damage.
		/// </summary>
		public int AdditionalDamage { get; set; }

		private const float AimDeadzone = 0.1f;

		private GameObject _projectilePrefab;

		private Transform _aimIndicatorTransform;
		private Transform _projectileSpawnTransform;

		private float _time;

		private bool _isCurrentInputMouse = true;
		private Vector2 _lastMousePositionInWorld;

		private void Start()
		{
			_projectilePrefab = Resources.Load<GameObject>("Prefabs/Projectiles/ProjectileBullet");

			_projectileSpawnTransform = transform.Find("ProjectileSpawn");
			_aimIndicatorTransform = transform.Find("AimIndicator");
			_projectilePrefab.transform.localScale = new Vector3(0.1f, 0.1f, 1);

			_time = 1.0f / FireRate;
		}

		private void Update()
		{
			if (_isCurrentInputMouse)
			{
				_aimIndicatorTransform.gameObject.SetActive(false);

				Direction = _lastMousePositionInWorld - (Vector2) _projectileSpawnTransform.position;
				RawDirection = _lastMousePositionInWorld - (Vector2) transform.position;

				UpdateLastMousePositionInWorld();
			}
			else
			{
				float aimAssistConeAngle = AimAssistConeAngle.MapBetween(0.0f, 360.0f, 1.0f, -1.0f);
				Vector3 position = transform.position;

				List<GameObject> enemies = TagBehaviour.FindWithTag("Enemy").ToList();

				enemies.Sort((lhs, rhs) =>
					Vector3.Distance(position, lhs.transform.position)
						.CompareTo(Vector3.Distance(position, rhs.transform.position)));

				foreach (GameObject enemy in enemies)
				{
					Vector3 enemyPosition = enemy.transform.position;

					Vector3 enemyDirection = (enemyPosition - _projectileSpawnTransform.position).normalized;
					Vector3 enemyInputDirection = (enemyPosition - transform.position).normalized;

					if (Vector3.Dot(RawDirection, enemyInputDirection) > aimAssistConeAngle)
					{
						Direction = enemyDirection;
						break;
					}
				}

				_aimIndicatorTransform.gameObject.SetActive(true);
				_aimIndicatorTransform.GetComponentInChildren<SpriteRenderer>().color = Color.white;
				_aimIndicatorTransform.transform.rotation = Quaternion.FromToRotation(Vector3.right, RawDirection);
			}

			_time += Time.deltaTime;

			if (Shooting && _time >= 1.0f / FireRate)
			{
				AudioManager.Play(ShootAudioClip, 0.75f, false, Random.Range(0.55f, 1.35f));

				InstanceFactory.InstantiateProjectile(_projectilePrefab, _projectileSpawnTransform.position, Direction,
					ProjectileSpeed, ProjectileDamage + AdditionalDamage, "Enemy");

				_time = 0;
			}
		}

		/// <summary>
		/// Called by the "aim" input action.
		/// </summary>
		public void OnLook(InputAction.CallbackContext context)
		{
			if (!context.performed || !SceneManager.GetActiveScene().isLoaded) return;
			UpdateLastMousePositionInWorld();
		}

		/// <summary>
		/// Called by the "aim (gamepad)" input action.
		/// </summary>
		public void OnLookGamepad(InputAction.CallbackContext context)
		{
			if (!context.performed || !SceneManager.GetActiveScene().isLoaded) return;
			_isCurrentInputMouse = false;

			Vector2 value = context.ReadValue<Vector2>();
			if (value.sqrMagnitude >= AimDeadzone) Direction = RawDirection = value;
		}

		/// <summary>
		/// Called by the "shoot" input action.
		/// </summary>
		public void OnShoot(InputAction.CallbackContext context)
		{
			if (!SceneManager.GetActiveScene().isLoaded || !enabled) return;

			if (context.performed) Shooting = true;
			else if (context.canceled) Shooting = false;
		}

		/// <summary>
		/// Updates the last mouse position in world field.
		/// </summary>
		private void UpdateLastMousePositionInWorld()
		{
			_isCurrentInputMouse = true;

			Vector2 mousePosition = Mouse.current.position.ReadValue();
			_lastMousePositionInWorld = UnityEngine.Camera.main!.ScreenToWorldPoint(mousePosition);
		}
	}
}