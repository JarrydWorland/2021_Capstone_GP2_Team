using UnityEngine;

namespace Scripts
{
	class SpinBehaviour : MonoBehaviour
	{
		/// <summary>
		/// How fast the object should spin in rotations per second.
		/// </summary>
		public float SpinSpeed = 1f;

		private void Update()
		{
			transform.Rotate(0, 0, SpinSpeed * 360 * Time.deltaTime);
		}
	}
}
