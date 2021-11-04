using UnityEngine;
using Scripts.Audio;
using Scripts.Camera;

namespace Scripts.Enemies.EnemyCharger
{
    [RequireComponent(typeof(Animator))]
	public class EnemyChargerBehaviour : MonoBehaviour
	{
		private const float CameraShakeStrength = 3.0f;
		private const float LazerSpeed = 4.0f;
		private const float LazerActiveFrame = 22.0f / 50.0f; // frame 22 of 50

		private CameraShakeBehaviour _cameraShakeBehaviour;
		private Transform _projectileSpawn;
		private Animator _animator;
		private GameObject _lazerUpPrefab;
		private GameObject _lazerDownPrefab;
		private GameObject _lazerScorchTrail;
		private GameObject _player;
		private Vector3 _lazerUpOffset;
		private Vector3 _lazerDownOffset;

		private GameObject _lazerUpInstance;
		private GameObject _lazerDownInstance;
		private GameObject _lazerScorchInstance;

		public AudioClip chargeSfx;
		public AudioClip dischargeSfx;

		private AudioId _chargeSfxId;
		private AudioId _dischargeSfxId;

		private void Start()
		{
			_cameraShakeBehaviour = UnityEngine.Camera.main.GetComponent<CameraShakeBehaviour>();
			_projectileSpawn = transform.Find("ProjectileSpawn");
			_animator = GetComponent<Animator>();
			_lazerUpPrefab = Resources.Load<GameObject>("Prefabs/Enemies/EnemyCharger/Lazer/EnemyChargerLazerUp");
			_lazerDownPrefab = Resources.Load<GameObject>("Prefabs/Enemies/EnemyCharger/Lazer/EnemyChargerLazerDown");
			_lazerScorchTrail = Resources.Load<GameObject>("Prefabs/Enemies/EnemyCharger/Lazer/TrailObject");
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
			_lazerScorchInstance.transform.position = Vector3.MoveTowards(_lazerDownInstance.transform.position, targetPosition, LazerSpeed * Time.deltaTime);
			// determine lazer state from animation
			Animator lazerDownAnimator = _lazerDownInstance.GetComponent<Animator>();_lazerDownInstance.GetComponent<Animator>();
			bool lazerActive = lazerDownAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= LazerActiveFrame;
			bool lazerFinished = lazerDownAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;

			if (lazerActive)
			{
				if(_lazerDownInstance.GetComponent<BoxCollider2D>().enabled == false)
                {
					//_lazerDownInstance.GetComponentsInChildren<TrailRenderer>(true)[0].gameObject.SetActive(true);
					_lazerScorchInstance.GetComponentInChildren<TrailRenderer>(true).gameObject.SetActive(true);
					_dischargeSfxId = AudioManager.Play(dischargeSfx, AudioCategory.Effect);
				}

				// enable damage collider
				_lazerDownInstance.GetComponent<BoxCollider2D>().enabled = true;

				_cameraShakeBehaviour.Shake(CameraShakeStrength, 0.1f);
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
			AudioManager.Stop(_chargeSfxId);
			AudioManager.Stop(_dischargeSfxId);
		}

		private void ResetLazer()
		{
			// destroy instances
			Destroy(_lazerUpInstance);
			Destroy(_lazerDownInstance);
		}

		/// <summary>
		/// Fires the chargers lazer prefabs, this is called by a callback
		/// within the enemy charger animation.
		/// </summary>
		public void Fire()
		{
			if (_player.GetComponent<HealthBehaviour>().Value <= 0 && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
			{
				_animator.Play("Idle");
			}
			else
			{
				_lazerUpInstance = GameObject.Instantiate(_lazerUpPrefab, _projectileSpawn.position + _lazerUpOffset, Quaternion.identity);
				_lazerDownInstance = GameObject.Instantiate(_lazerDownPrefab, _player.transform.position + _lazerDownOffset, Quaternion.identity);
				_lazerDownInstance.AddComponent<LazerDownDamageBehaviour>();
				_chargeSfxId = AudioManager.Play(chargeSfx, AudioCategory.Effect);
				if (_lazerScorchInstance != null)
                {
					Destroy(_lazerScorchInstance);
					_lazerScorchInstance = GameObject.Instantiate(_lazerScorchTrail, _player.transform.position + _lazerDownOffset, Quaternion.identity);
				}
				else _lazerScorchInstance = GameObject.Instantiate(_lazerScorchTrail, _player.transform.position + _lazerDownOffset, Quaternion.identity);
			}

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
