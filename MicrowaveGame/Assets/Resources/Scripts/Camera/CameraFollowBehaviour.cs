using UnityEngine;
using Scripts.Utilities;

namespace Scripts.Camera
{
	public class CameraFollowBehaviour : MonoBehaviour
	{
		public GameObject Target;
		private Vector3 _velocity;

		private void FixedUpdate()
		{
			Vector3 target = new Vector3(Target.transform.position.x, Target.transform.position.y, transform.position.z);
			transform.position = Vector3.SmoothDamp(transform.position, target, ref _velocity, 0.1f);
		}
	}
}
