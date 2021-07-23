using UnityEngine;

namespace Scripts.Projectiles
{
	public class ProjectileBulletBehaviour : ProjectileBehaviour
	{
		protected override void Update()
		{
			base.Update();
			transform.position += (Vector3)Direction * (Speed * Time.deltaTime);
		}
	}
}
