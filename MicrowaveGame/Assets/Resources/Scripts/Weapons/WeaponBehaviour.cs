using UnityEngine;
using Scripts.Items;
using Scripts.Inventory;
using Scripts.Player;

namespace Scripts.Weapons
{
	public abstract class WeaponBehaviour : ItemBehaviour
	{
		/// <summary>
		/// Number of hit points dealt by the weapon
		/// </summary>
		public int Damage;

		/// <summary>
		/// How frequently the weapon fires. 0 if not applicable.
		/// </summary>
		public float FireRate;

		/// <summary>
		/// How fast the projectiles it produces travel. 0 if not applicable.
		/// </summary>
		public float ProjectileSpeed;

		private PlayerWeaponBehaviour _playerWeaponBehaviour;
		private bool isWeaponEquipped => _playerWeaponBehaviour.EquippedWeaponBehaviour == this;

		public override void Start()
		{
			base.Start();
			_playerWeaponBehaviour = GameObject.Find("Player").GetComponent<PlayerWeaponBehaviour>();
		}

		/// <summary>
		/// A callback method that is called while the weapon is equipped by
		/// the player. Only one weapon can be equipped at a time.
		/// </summary>
		/// <param name="position">The position that the weapon should spawn projectiles at</param>
		/// <param name="direction">The direction that the weapon should fire projectiles towards</param>
		/// <param name="direction">Whether or not the weapon is being fired (is the trigger being pulled).</param>
		public abstract void OnWeaponUpdate(Vector2 position, Vector2 direction, bool shooting);

		/// <summary>
		/// A callback method that is called when the weapon is equipped by the
		/// player.
		/// </summary>
		public abstract void OnWeaponEquip();

		/// <summary>
		/// A callback method that is called when the weapon is unequiped by the
		/// player.
		/// </summary>
		public abstract void OnWeaponUnequip();

		/// <summary>
		/// A callback method that is called when the weapon is picked up into
		/// the players inventory
		/// </summary>
		/// <param name="inventorySlotBehaviour">The inventory slot the weapon will be stored in.</param>
		public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			
		}

		/// <summary>
		/// A callback method that is called when the weapon is "used" as an
		/// item. This is used to toggle this weapon as the currently equipped weapon.
		/// </summary>
		/// <param name="inventorySlotBehaviour">The inventory slot that the weapon is stored in.</param>
		/// <returns>Returns true if the item is equipped, otherwise returns false.</returns>
		public override bool OnUseItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			_playerWeaponBehaviour.EquippedWeaponBehaviour.OnWeaponUnequip();
			_playerWeaponBehaviour.EquippedWeaponBehaviour = isWeaponEquipped ? null : this;
			_playerWeaponBehaviour.EquippedWeaponBehaviour.OnWeaponEquip();
			
			return isWeaponEquipped;
		}

		/// <summary>
		/// A callback method that is called while the weapon is in the players
		/// inventory.
		/// </summary>
		/// <param name="inventorySlotBehaviour">The inventory slot that the weapon is stored in.</param>
		public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			if (isWeaponEquipped)
			{
				OnWeaponUpdate(
					_playerWeaponBehaviour.transform.position,
					_playerWeaponBehaviour.Direction,
					_playerWeaponBehaviour.Shooting
				);
			}
		}

		/// <summary>
		/// A callback method that is called when the weapon is dropped. This
		/// unequips the weapon if it was equipped.
		/// </summary>
		/// <param name="inventorySlotBehaviour">The inventory slot the weapon will be stored in.</param>
		/// <returns>Returns true if the weapon is ready to be dropped, otherwise returns false.</returns>
		public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			if (isWeaponEquipped)
			{
				_playerWeaponBehaviour.EquippedWeaponBehaviour.OnWeaponUnequip();
				_playerWeaponBehaviour.EquippedWeaponBehaviour= null;
				_playerWeaponBehaviour.EquippedWeaponBehaviour.OnWeaponEquip();
			}
			return true;
		}
	}
}
