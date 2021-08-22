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
			base.Update();
			GameObject Player = GameObject.Find("Player");
			float distance = Vector3.Distance(Player.transform.position, transform.position);
			_radius = 4;
			transform.position += (Vector3)Direction;
			if (distance > _radius) Destroy(gameObject);
		}
	}
}
