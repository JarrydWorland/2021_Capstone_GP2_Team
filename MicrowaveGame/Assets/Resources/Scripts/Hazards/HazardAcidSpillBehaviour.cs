using Scripts.StatusEffects;
using UnityEngine;
using Scripts.Utilities;

namespace Scripts.Hazards
{
	public class HazardAcidSpillBehaviour : HazardBehaviour
	{
		/// <summary>
		/// The amount of damage that will be dealt to the player.
		/// </summary>
		public int Damage = 1;

		/// <summary>
		/// The rate at which damage is dealt each every second (e.g. 2.0f will deal damage twice every second or once every half second).
		/// </summary>
		public float DamageRate = 1.0f;

		/// <summary>
		/// The duration the acid spill effect should last for once the player is no
		/// longer colliding with the acid spill.
		/// </summary>
		public int Duration = 5;

		public AudioClip sfx;
		private void OnTriggerStay2D(Collider2D other)
		{
			StatusEffectBehaviour statusEffectBehaviour = other.gameObject.GetComponent<StatusEffectBehaviour>();
			if (statusEffectBehaviour != null)
			{
				statusEffectBehaviour.Apply<StatusEffectMelting>(Duration, Damage, DamageRate,sfx);
			}
		}
	}
}
