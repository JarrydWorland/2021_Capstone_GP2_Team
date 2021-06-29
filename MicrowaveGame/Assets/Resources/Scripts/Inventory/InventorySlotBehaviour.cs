using Scripts.Items;
using UnityEngine;

namespace Scripts.Inventory
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class InventorySlotBehaviour : MonoBehaviour
	{
		public ItemBehaviour ItemBehaviour { get; private set; }

		private SpriteRenderer _inventorySpriteRenderer;
		private Sprite _inventorySlotSprite;

		private Vector3 _inventorySlotInitialScale;

		private void Start()
		{
			_inventorySpriteRenderer = GetComponent<SpriteRenderer>();
			_inventorySlotSprite = _inventorySpriteRenderer.sprite;

			_inventorySlotInitialScale = _inventorySpriteRenderer.transform.localScale;
		}

		private void Update()
		{
			if (ItemBehaviour != null) ItemBehaviour.OnUpdateItem(this);
		}

		public void PickupItem(ItemBehaviour itemBehaviour)
		{
			if (itemBehaviour == null) return;

			if (ItemBehaviour != null) DropItem();
			ItemBehaviour = itemBehaviour;

			SetSprite(ItemBehaviour.GetComponent<SpriteRenderer>().sprite, ItemBehaviour.transform.localScale);
			ItemBehaviour.OnPickupItem(this);
		}

		public void UseItem()
		{
			if (ItemBehaviour != null) ItemBehaviour.OnUseItem(this);
		}

		public void UpdateItem()
		{
			if (ItemBehaviour != null) ItemBehaviour.OnUpdateItem(this);
		}

		public ItemBehaviour DropItem()
		{
			ItemBehaviour.OnDropItem(this);

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

			_inventorySpriteRenderer.sprite = sprite;
		}
	}
}