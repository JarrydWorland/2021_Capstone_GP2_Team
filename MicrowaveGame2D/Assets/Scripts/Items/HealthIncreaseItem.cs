using UnityEngine;

namespace Items
{
	public class HealthIncreaseItem : BaseItem
	{
		public override void Use()
		{
			GameObject playerObject = GameObject.Find("Player");
			playerObject.GetComponent<Health>().Value += 5;
		}
	}
}