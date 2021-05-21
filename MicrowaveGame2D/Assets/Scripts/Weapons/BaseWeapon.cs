using UnityEngine;

namespace Weapons
{
	public abstract class BaseWeapon : MonoBehaviour
	{
		public abstract string Name { get; }
		public abstract string Description { get; }

		// The amount of damage (1 = half a heart) each bullet will deal.
		public abstract int Damage { get; set; }

		// The rate at which bullets are created / instantiated.
		// 1.0f = 1 bullet per second.
		// 5.0f = 5 bullets per second.
		// etc.
		public abstract float FireRate { get; set; }

		// The direction the weapon will shoot towards
		private Vector2 _direction = Vector2.right;
		public Vector2 Direction
		{
			get => _direction;
			set => _direction = value.normalized;
		}

		// An abstract method which should be used to create
		// a bullet instance, set its position, rotation, etc.
		public abstract void Shoot();

		// An abstract method which should be used to stop firing
		// a weapon (useful for weapons that shoot continuously while
		// the user is holding the shoot button).
		public abstract void Holster();
	}
}
