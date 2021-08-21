using UnityEngine;

namespace Scripts.Projectiles
{
	public class ProjectileBeamBehaviour : ProjectileBehaviour
	{
		//Designed to create a range around the player where he is shooting a Beam. 
		//Currently it creates a range around the player only based on the transform.position of the spawn of the starting room.
		protected override void Update()
		{
			Vector2 dist = transform.position;
			if (dist.magnitude > 2) Destroy(gameObject); 
			transform.position += (Vector3)Direction;
		}
	}
}
