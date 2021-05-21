using Weapons;
using Player;
using UnityEngine;

namespace Items
{
	public class WeaponVelocityItem : BaseItem
	{
		// This isn't used for passive items as they aren't items you activate or consume.
		public override bool Used => false;

		private BaseWeapon _rapidFire;

		private void Start()
		{
			_rapidFire = GameObject.Find("Player").GetComponent<PlayerShootBehaviour>().Weapon;
		}

		public override void OnUseItem() { }

		public override void OnItemUpdate() { }

		public override void OnPickupItem() { _rapidFire.Damage += 1; }

		public override void OnDropItem() { _rapidFire.Damage -= 1; }
	}
}