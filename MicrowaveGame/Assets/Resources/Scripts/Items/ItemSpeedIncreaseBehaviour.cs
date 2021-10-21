using Scripts.Inventory;
using Scripts.Utilities;
using Scripts.StatusEffects;
using UnityEngine;

namespace Scripts.Items
{
	public class ItemSpeedIncreaseBehaviour : ItemBehaviour
	{
		/// <summary>
		/// The amount of additional speed when the item is active.
		/// </summary>
		public float IncreaseValue;

		/// <summary>
		/// The duration of the item.
		/// </summary>
		public int DurationValue;

		private bool _isActive, _isUsed;
		private float _time;

		public AudioClip itemDrop;

		public override void Start()
		{
			base.Start();
			Description = string.Format(Description, IncreaseValue * 100.0f, DurationValue);
		}

		public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour) =>
			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceExpand");

		public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			if (_isActive) return;
			_isActive = _isUsed = true;

			GameObject.Find("Player").GetComponent<StatusEffectBehaviour>()
				.Apply<StatusEffectFaster>(DurationValue, IncreaseValue);

			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceLoop");
		}

		public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			if (!_isActive) return;

			_time += Time.deltaTime;
			if (_time < DurationValue) return;

			_isActive = false;

			inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
			inventorySlotBehaviour.DropItem();
			Destroy(gameObject);
		}

		public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			if (!_isUsed)
			{
				inventorySlotBehaviour.PlayAnimation("InventorySlotBounceContract");
				AudioManager.Play(itemDrop, 0.55f);
			}

			return !_isActive;
		}
	}
}