using Scripts.Items;
using UnityEngine;

namespace Scripts.Inventory
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class InventoryItemPickerBehaviour : MonoBehaviour
	{
		private SpriteRenderer _spriteRenderer;

		private InventoryBehaviour _inventoryBehaviour;

		private void Start()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();
			_inventoryBehaviour = GameObject.Find("Inventory").GetComponent<InventoryBehaviour>();
		}

		private void Update()
		{
			ItemBehaviour itemBehaviour = _inventoryBehaviour.GetNearbyItem();

			if (itemBehaviour != null)
			{
				transform.position = itemBehaviour.transform.position + new Vector3(0.0f, 0.75f, 0.0f);
				_spriteRenderer.enabled = true;
			}
			else _spriteRenderer.enabled = false;
		}
	}
}