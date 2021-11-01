using UnityEngine;
using Scripts.Audio;

namespace Scripts.Projectiles
{
    public class ProjectileBulletBehaviour : ProjectileBehaviour
	{
		public AudioClip ttlSfx;
		protected override void Update()
		{
			Vector2 distance = _startingPosition - transform.position;
			base.Update();
			transform.position += (Vector3)Direction * (Speed * Time.deltaTime);
			
			if (distance.sqrMagnitude > 1000)
			{
				Destroy(gameObject);
				AudioManager.Play(ttlSfx, 0.55f);
			}
			
		}
	}
}
