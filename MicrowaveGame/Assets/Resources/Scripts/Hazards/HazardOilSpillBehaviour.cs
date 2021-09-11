using Scripts.StatusEffects;
using UnityEngine;

namespace Scripts.Hazards
{
	public class HazardOilSpillBehaviour : HazardBehaviour
	{
		private void OnTriggerStay2D(Collider2D other)
		{
			StatusEffectBehaviour statusEffectBehaviour = other.gameObject.GetComponent<StatusEffectBehaviour>();
			if (statusEffectBehaviour != null) statusEffectBehaviour.Apply(new StatusEffectSlower());
		}
	}
}