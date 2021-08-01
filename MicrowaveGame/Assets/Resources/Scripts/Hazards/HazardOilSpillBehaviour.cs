using Scripts.Player;
using UnityEngine;

namespace Scripts.Hazards
{
	public class HazardOilSpillBehaviour : HazardBehaviour
	{
		/// <summary>
		/// The duration the oil spill effect should last for once the player is no
		/// longer colliding with the oil spill.
		/// </summary>
		public int Duration = 5;

		/// <summary>
		/// The factor to reduce the acceleration by (e.g. 0.5f is 50% slower).
		/// </summary>
		public float AccelerationFactor = 0.5f;

		/// <summary>
		/// The factor to reduce the friction by (e.g. 0.25f is 75% more slippery).
		/// </summary>
		public float FrictionFactor = 0.25f;

		private PlayerMovementBehaviour _playerMovementBehaviour;

		private float _time;
		private bool _applied;

		private void Start()
		{
			_playerMovementBehaviour = GameObject.Find("Player").GetComponent<PlayerMovementBehaviour>();
		}

		private void Update()
		{
			if (!_applied) return;
			_time += Time.deltaTime;

			if (_time >= Duration)
			{
				_playerMovementBehaviour.Acceleration /= AccelerationFactor;
				_playerMovementBehaviour.Friction /= FrictionFactor;

				_applied = false;
			}
		}

		private void OnTriggerStay2D(Collider2D other)
		{
			if (other.gameObject.name != "Player") return;
			_time = 0;

			if (!_applied)
			{
				_playerMovementBehaviour.Acceleration *= AccelerationFactor;
				_playerMovementBehaviour.Friction *= FrictionFactor;

				_applied = true;
			}
		}
	}
}