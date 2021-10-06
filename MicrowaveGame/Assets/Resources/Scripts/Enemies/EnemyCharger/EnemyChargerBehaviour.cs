using UnityEngine;

namespace Scripts.Enemies.EnemyCharger
{
    [RequireComponent(typeof(Animator))]
	public class EnemyChargerBehaviour : MonoBehaviour
	{
		private const float CameraShakeStrength = 5.0f;
		private const float CameraShakeDamping = 50.0f;
		private const float LazerSpeed = 4.0f;
		private const float LazerActiveFrame = 22.0f / 50.0f; // frame 22 of 50

		private Transform _projectileSpawn;
		private Animator _animator;
		private GameObject _lazerUpPrefab;
		private GameObject _lazerDownPrefab;
		private GameObject _player;
		private Vector3 _lazerUpOffset;
		private Vector3 _lazerDownOffset;

		private GameObject _lazerUpInstance;
		private GameObject _lazerDownInstance;

		private void Start()
		{
			_projectileSpawn = transform.Find("ProjectileSpawn");
			_animator = GetComponent<Animator>();
			_lazerUpPrefab = Resources.Load<GameObject>("Prefabs/Enemies/EnemyCharger/Lazer/EnemyChargerLazerUp");
			_lazerDownPrefab = Resources.Load<GameObject>("Prefabs/Enemies/EnemyCharger/Lazer/EnemyChargerLazerDown");
			_lazerUpOffset = new Vector3(0, _lazerUpPrefab.GetComponent<SpriteRenderer>().bounds.extents.y, 0);
			_lazerDownOffset = new Vector3(0, _lazerDownPrefab.GetComponent<SpriteRenderer>().bounds.extents.y - 1.0f, 0);
			_player = GameObject.Find("Player");
		}

		private void Update()
		{
			if (_lazerDownInstance == null) return;

			// move lazer towards player
			Vector3 targetPosition = _player.transform.position + _lazerDownOffset;
			_lazerDownInstance.transform.position = Vector3.MoveTowards(_lazerDownInstance.transform.position, targetPosition, LazerSpeed * Time.deltaTime);

			// determine lazer state from animation
			Animator lazerDownAnimator = _lazerDownInstance.GetComponent<Animator>();_lazerDownInstance.GetComponent<Animator>();
			bool lazerActive = lazerDownAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= LazerActiveFrame;
			bool lazerFinished = lazerDownAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;

			if (lazerActive)
			{
				// enable damage collider
				_lazerDownInstance.GetComponent<BoxCollider2D>().enabled = true;

				// shake camera
				float zRotation = UnityEngine.Random.Range(0.0f, CameraShakeStrength) - (CameraShakeStrength/2.0f);
				UnityEngine.Camera.main.transform.rotation = Quaternion.Lerp(
					UnityEngine.Camera.main.transform.rotation,
					Quaternion.Euler(0, 0, zRotation),
					CameraShakeDamping * Time.deltaTime
				);
			}

			if (lazerFinished)
			{
				ResetLazer();

				// reset animation
				_animator.Play("Reset");
			}
		}

		private void OnDisable()
		{
			ResetLazer();
		}

		private void ResetLazer()
		{
			// destroy instances
			Destroy(_lazerUpInstance);
			Destroy(_lazerDownInstance);
			
			// reset camera rotation
			UnityEngine.Camera.main.transform.rotation = Quaternion.identity;
		}

		/// <summary>
		/// Fires the chargers lazer prefabs, this is called by a callback
		/// within the enemy charger animation.
		/// </summary>
		public void Fire()
		{
			_lazerUpInstance = GameObject.Instantiate(_lazerUpPrefab, _projectileSpawn.position + _lazerUpOffset, Quaternion.identity);
			_lazerDownInstance = GameObject.Instantiate(_lazerDownPrefab, _player.transform.position + _lazerDownOffset, Quaternion.identity);
			_lazerDownInstance.AddComponent<LazerDownDamageBehaviour>();

		}
		
		private class LazerDownDamageBehaviour : MonoBehaviour
		{
			private float _lastDamageTime;
			private void OnTriggerStay2D(Collider2D other)
			{
				if (other.name == "Player" && Time.time - _lastDamageTime >= 1.0f/3.0f)
				{
					other.GetComponent<HealthBehaviour>().Value -= 1;
					_lastDamageTime = Time.time;
				}
			}
		}
	}
}
