using UnityEngine;

namespace Items
{
	public class HealthIncreaseItem : BaseItem
	{
		public int IncreaseValue;

		private bool _used;
		public override bool Used => _used;


		public void Start() {}

        public override void OnUseItem()
		{


			if (_used) return;

			GameObject playerObject = GameObject.Find("Player");
			Health playerHealth = playerObject.GetComponent<Health>();


			if (playerHealth.Value < playerHealth.MaxHealth)
			{
				playerHealth.Value += IncreaseValue;
				_used = true;

				Destroy(gameObject);
			}
		}

		public override void OnItemUpdate() { }

		public override void OnPickupItem() { }

		public override void OnDropItem() { }
	}
}