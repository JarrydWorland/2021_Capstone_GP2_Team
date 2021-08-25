using UnityEngine;
using Scripts.Utilities;

namespace Scripts.Camera
{
	public class CameraFollowBehaviour : MonoBehaviour
	{
		/// <summary>
		/// The target gameobject that the camera should follow.
		/// </summary>
		public GameObject Target;

		/// <summary>
		/// The bounds that the camera should be restricted to.
		/// Bounds range from -x -> x and -y -> y.
		/// </summary>
		public Vector2 Bounds;

		/// <summary>
		/// Prevent camera movement on the X axis.
		/// </summary>
		public bool LockX;

		/// <summary>
		/// Prevent camera movement on the Y axis.
		/// </summary>
		public bool LockY;

		private Vector3 _velocity;

		private void FixedUpdate()
		{
			// find target position
			Vector3 target = new Vector3
			{
				x = !LockX ? Target.transform.position.x : transform.position.x,
				y = !LockY ? Target.transform.position.y : transform.position.y,
				z = transform.position.z
			};

			// clamp camera within bounds if provided
			if (Bounds.x > 0) target.x = target.x.Clamp(-Bounds.x, Bounds.x);
			if (Bounds.y > 0) target.y = target.y.Clamp(-Bounds.y, Bounds.y);

			// lerp towards target position
			transform.position = Vector3.SmoothDamp(transform.position, target, ref _velocity, 0.1f);
		}
	}
}
