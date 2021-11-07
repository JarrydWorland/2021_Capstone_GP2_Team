using Scripts.Audio;
using UnityEngine;

namespace Scripts.Projectiles
{
    public class ProjectileBulletBehaviour : ProjectileBehaviour
	{
		private const float HomingStrengthScale = 350.0f;

		public AudioClip ttlSfx;

		protected override void Update()
		{
			Vector2 distance = _startingPosition - transform.position;
			base.Update();
			transform.position += (Vector3)Direction * (Speed * Time.deltaTime);
			
			if (HomingTarget != null)
			{
				float strength = (HomingStrength / Vector3.Distance(transform.position, HomingTarget.transform.position)) * HomingStrengthScale;
				Vector3 targetDirection = HomingTarget.transform.position -  transform.position;
				Direction = Vector3.RotateTowards(Direction, targetDirection, strength * Mathf.Deg2Rad * Time.deltaTime, 0.0f);
			}

			if (distance.sqrMagnitude > 1000)
			{
				Destroy(gameObject);
				AudioManager.Play(ttlSfx, AudioCategory.Effect, 0.25f);
			}
		}
	}
}
