using UnityEngine;
using Scripts.Utilities;

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
		public bool IsStationary => Position.Interpolation == 1.0f;

		/// <summary>
		// The position to pan towards.
		/// </summary>
		public Lerped<Vector2> Position;

		/// <summary>
		// Stores the state of isStationary for one additional
		// update tick so that the position can finish being
		// applied.
		/// </summary>
		private bool _delayedIsStationary;

		private void Start()
		{
			Position = new Lerped<Vector2>(transform.position, PanDuration, Easing.EaseInOut, true);
		}

		private void Update()
		{
			bool isStationary = IsStationary;
			if (!isStationary || !_delayedIsStationary)
			{
				Vector2 position = Position.Value;
				transform.position = new Vector3(position.x, position.y, transform.position.z);
				_delayedIsStationary = isStationary;
			}
		}
	}
}
