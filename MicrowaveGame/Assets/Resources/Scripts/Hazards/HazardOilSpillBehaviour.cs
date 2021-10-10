using Scripts.StatusEffects;
using UnityEngine;
using Scripts.Utilities;

namespace Scripts.Hazards
{
	public class HazardOilSpillBehaviour : HazardBehaviour
	{
		/// <summary>
		/// The duration the oil spill effect should last for once the player is no
		/// longer colliding with the oil spill.
		/// </summary>
		public int Duration = 5;

		/// <summary>
		/// The factor to reduce the acceleration by (e.g. 0.5f is 50% slower).
		/// </summary>
		public float AccelerationFactor = 0.5f;

		/// <summary>
		/// The factor to reduce the friction by (e.g. 0.25f is 75% more slippery).
		/// </summary>
		public float FrictionFactor = 0.25f;

		public AudioClip sfx;
		private void OnTriggerStay2D(Collider2D other)
		{
			if (other.gameObject.name == "Wheel")
			{
				StatusEffectBehaviour statusEffectBehaviour = other.gameObject.GetComponentInParent<StatusEffectBehaviour>();

				if (statusEffectBehaviour != null)
				{
					statusEffectBehaviour.Apply<StatusEffectSlower>(Duration, AccelerationFactor, FrictionFactor, sfx);
				}
			}
		}

	}
}