using Scripts.StatusEffects;
using UnityEngine;

namespace Scripts.Hazards
{
	public class HazardOilSpillBehaviour : HazardBehaviour
	{
		private void OnTriggerStay2D(Collider2D other)
		{
			if (other.gameObject.name == "Player")
				other.gameObject.GetComponent<StatusEffectBehaviour>().Apply<StatusEffectSlowed>();
		}
	}
}