using Weapons;
using Player;
using UnityEngine;

namespace Items
{
	public class DamageItem : BaseItem
	{
		// This isn't used for passive items as they aren't items you activate or consume.
		public override bool Used => false;

		private BaseWeapon _increaseDamage;

		private void Start()
		{
			_increaseDamage = GameObject.Find("Player").GetComponent<PlayerShootBehaviour>().Weapon;
		}

		public override void OnUseItem() { }

		public override void OnItemUpdate() { }

		public override void OnPickupItem() { }

		public override void OnDropItem() { _increaseDamage.Damage -= 1; }
	}
}