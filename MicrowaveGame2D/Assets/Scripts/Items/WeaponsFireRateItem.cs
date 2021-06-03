using Weapons;
using Player;
using UnityEngine;

namespace Items
{
	public class WeaponsFireRateItem : BaseItem
	{
		// This isn't used for passive items as they aren't items you activate or consume.
		public override bool Used => false;

		private BaseWeapon _increaseRate;


		private void Start()
		{
			_increaseRate = GameObject.Find("Player").GetComponent<PlayerShootBehaviour>().Weapon;
		}

		public override void OnUseItem() { }

		public override void OnItemUpdate() { }

		public override void OnPickupItem() 
		{
			_increaseRate.FireRate += 5.0f;
		}

		public override void OnDropItem()
		{
			_increaseRate.FireRate -= 5.0f;
		}
	}
}