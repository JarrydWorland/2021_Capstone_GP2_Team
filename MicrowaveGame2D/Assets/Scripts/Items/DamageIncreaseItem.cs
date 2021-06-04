using Weapons;
using Player;
using UnityEngine;

namespace Items
{
	public class DamageIncreaseItem : BaseItem
	{
		public int IncreaseValue;

		// Passive items are always active.
		public override bool IsActivated => true;

		// Passive items are never consumed.
		public override bool IsConsumed => false;

		private BaseWeapon _playerWeapon;

		private void Start()
		{
			IsPassive = true;
			_playerWeapon = GameObject.Find("Player").GetComponent<PlayerShootBehaviour>().Weapon;
		}

		public override void OnUseItem() { }

		public override void OnItemUpdate() { }

		public override void OnPickupItem()
		{
			_playerWeapon.Damage += IncreaseValue;
		}

		public override void OnDropItem()
		{
			_playerWeapon.Damage -= IncreaseValue;
		}
	}
}