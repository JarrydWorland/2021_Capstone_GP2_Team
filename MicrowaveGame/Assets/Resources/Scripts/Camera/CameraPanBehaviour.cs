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

		private void Start()
		{
			Position = new Lerped<Vector2>(transform.position, PanDuration, Easing.EaseInOut);
		}

		private void Update()
		{
			Vector2 position = Position.Value;
			transform.position = new Vector3(position.x, position.y, transform.position.z);
		}
	}
}
