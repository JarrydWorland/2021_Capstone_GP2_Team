using Weapons;
using Player;
using UnityEngine;

namespace Items
{
	public class FireRateIncreaseItem : BaseItem
	{
		public float IncreaseValue;

		// Passive items are always active.
		public override bool IsActivated => true;

		// Passive items are never consumed.
		public override bool IsConsumed => false;

		private BaseWeapon _increaseRate;

		private void Start()
		{
			IsPassive = true;
			_increaseRate = GameObject.Find("Player").GetComponent<PlayerShootBehaviour>().Weapon;
		}

		public override void OnUseItem() { }

		public override void OnItemUpdate() { }

		public override void OnPickupItem()
		{
			_increaseRate.FireRate += IncreaseValue;
		}

		public override void OnDropItem()
		{
			_increaseRate.FireRate -= IncreaseValue;
		}
	}
}