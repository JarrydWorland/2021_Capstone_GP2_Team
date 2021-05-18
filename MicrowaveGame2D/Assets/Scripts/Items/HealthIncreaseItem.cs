using UnityEngine;

namespace Items
{
	public class HealthIncreaseItem : BaseItem
	{
		public override void Use()
		{
			GameObject playerObject = GameObject.Find("Player");
			Health playerHealth = playerObject.GetComponent<Health>();

			if (playerHealth.Value < playerHealth.MaxHealth)
			{
				playerHealth.Value += 4;
				Destroy(gameObject);
			}
		}
	}
}