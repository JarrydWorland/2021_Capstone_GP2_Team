using Scripts.Items;
using UnityEngine;

namespace Scripts.Inventory
{
	public class InventorySlotBehaviour : MonoBehaviour
	{
		/// <summary>
		/// The current item (null if empty).
		/// </summary>
		public ItemBehaviour ItemBehaviour { get; private set; }

		private SpriteRenderer _inventorySpriteRenderer;
		private Sprite _inventorySlotSprite;

		private Vector3 _inventorySlotInitialScale;

		private Animator _animator;

		private void Start()
		{
			_inventorySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
			_inventorySlotSprite = _inventorySpriteRenderer.sprite;

			_inventorySlotInitialScale = _inventorySpriteRenderer.transform.localScale;

			_animator = GetComponentInChildren<Animator>();
		}

		private void Update()
		{
			if (ItemBehaviour != null) ItemBehaviour.OnUpdateItem(this);
		}

		/// <summary>
		/// Pick up the given item, dropping any existing item in the slot, and update the slot sprite.
		/// </summary>
		/// <param name="itemBehaviour">The item to be added to the slot.</param>
		public void PickupItem(ItemBehaviour itemBehaviour)
		{
			if (itemBehaviour == null) return;

			if (ItemBehaviour != null) DropItem();
			ItemBehaviour = itemBehaviour;

			SetSprite(ItemBehaviour.GetComponent<SpriteRenderer>().sprite, ItemBehaviour.transform.localScale);
			ItemBehaviour.OnPickupItem(this);
		}

		/// <summary>
		/// Attempt to use the item if there is one in the slot.
		/// </summary>
		public void UseItem()
		{
			if (ItemBehaviour != null) ItemBehaviour.OnUseItem(this);
		}

		/// <summary>
		/// Attempt to update the item if there is one in the slot.
		/// </summary>
		public void UpdateItem()
		{
			if (ItemBehaviour != null) ItemBehaviour.OnUpdateItem(this);
		}

		/// <summary>
		/// Attempt to drop the item if there is one in the slot.
		/// </summary>
		/// <returns>The dropped item (null if empty).</returns>
		public ItemBehaviour DropItem()
		{
			if (ItemBehaviour == null) return null;

			bool shouldDrop = ItemBehaviour.OnDropItem(this);
			if (!shouldDrop) return null;

			SetSprite(_inventorySlotSprite, Vector3.one);

			ItemBehaviour itemBehaviour = ItemBehaviour;
			ItemBehaviour = null;

			return itemBehaviour;
		}

		/// <summary>
		/// Set the sprite of the slot to the given sprite, scaled to fit the slot and to retain the sprite's aspect ratio.
		/// </summary>
		/// <param name="sprite">The new sprite.</param>
		/// <param name="scale">The scale of the slot.</param>
		private void SetSprite(Sprite sprite, Vector3 scale)
		{
			float spriteX = sprite.bounds.size.x * scale.x;
			float spriteY = sprite.bounds.size.y * scale.y;

			float finalScale = spriteX > spriteY
				? _inventorySlotSprite.bounds.size.x / spriteX
				: _inventorySlotSprite.bounds.size.y / spriteY;

			transform.localScale = new Vector3(
				_inventorySlotInitialScale.x * finalScale * scale.x,
				_inventorySlotInitialScale.y * finalScale * scale.y,
				1.0f
			);

			_inventorySpriteRenderer.transform.localScale = Vector3.one;
			_inventorySpriteRenderer.sprite = sprite;
		}

		public void PlayAnimation(string name) => _animator.Play(name);
	}
}