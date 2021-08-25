using UnityEngine;
using Scripts.Utilities;

namespace Scripts.Camera
{
	public class CameraFollowBehaviour : MonoBehaviour
	{
		public GameObject Target;
		public Vector2 Bounds;
		public bool LockX;
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
