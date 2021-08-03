using UnityEngine;

namespace Scripts.Hazards.FlameThrower
{
	public class HazardFlameThrowerBehaviour : HazardBehaviour
	{
		/// <summary>
		/// The rate at which the flames are toggled (e.g. 2.0f will toggle the flames every 2 seconds).
		/// </summary>
		public float Interval = 2.0f;

		private HazardFlameThrowerFlameBehaviour[] _flameBehaviours;

		private float _time;
		private bool _enabled;

		private void Start()
		{
			_flameBehaviours = GetComponentsInChildren<HazardFlameThrowerFlameBehaviour>(true);
		}

		private void Update()
		{
			_time += Time.deltaTime;

			if (_time >= Interval)
			{
				_enabled = !_enabled;

				foreach (HazardFlameThrowerFlameBehaviour flameBehaviour in _flameBehaviours)
					flameBehaviour.gameObject.SetActive(_enabled);

				_time -= Interval;
			}
		}
	}
}