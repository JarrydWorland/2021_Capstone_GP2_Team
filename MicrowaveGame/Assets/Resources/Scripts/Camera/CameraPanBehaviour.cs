using UnityEngine;

namespace Scripts.Camera
{
	public class CameraPanBehaviour : MonoBehaviour
	{
		/// <summary>
		/// The duration of the pan animation in seconds.
		/// </summary>
		public float PanDuration = 1.0f;

		/// <summary>
		// Whether the camera is currently stationary or moving.
		/// </summary>
		public bool IsStationary => (Vector2)transform.position == TargetPosition;

		/// <summary>
		// The position to pan towards.
		/// </summary>
		public Vector2 TargetPosition
		{
			get => _targetPosition;
			set
			{
				_timer = 0;
				_startPosition = transform.position;
				_targetPosition = value;
			}
		}
		private float _timer;
		private Vector2 _startPosition;
		private Vector2 _targetPosition;

		/// <summary>
		/// A value between 0.0f and 1.0f representing how much of the
		/// animation has been completed.
		/// </summary>
		private float _panInterpolation => Mathf.Min(_timer / PanDuration, 1.0f);

		private void Start()
		{
			TargetPosition = transform.position;
		}

		private void Update()
		{
			// https://www.desmos.com/calculator/za8sugou91
			float easeInOut(float n) => n <= 0.5
				? +2 * Mathf.Pow(n, 2)
				: -2 * Mathf.Pow(n - 1, 2) + 1;

			_timer += Time.deltaTime;
			Vector2 lerpedPosition = Vector2.LerpUnclamped(_startPosition, TargetPosition, easeInOut(_panInterpolation));
			transform.position = new Vector3(lerpedPosition.x, lerpedPosition.y, transform.position.z);
		}
	}
}
