using UnityEngine;

namespace Scripts.Projectiles
{
	public class ProjectileBeamBehaviour : ProjectileBehaviour
	{
		private float _radius;
		//Designed to create a range around the player where he is shooting a Beam. 
		//Currently it creates a range around the player only based on the transform.position of the spawn of the starting room.
		protected override void Update()
		{
			_radius = 5;
			transform.position += (Vector3)Direction;
			if (transform.position.sqrMagnitude > _radius) Destroy(gameObject);
		}
	}
}
