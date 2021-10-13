using System.Linq;
using Scripts.Utilities;
using UnityEngine;

namespace Scripts.Inventory
{
	public class InventorySlotIndicatorBehaviour : MonoBehaviour
	{
		private SpriteRenderer _spriteRenderer;

		private Lerped<Vector3> _position;

		private bool _visible;

		/// <summary>
		/// Whether the slot indicator is visible or not.
		/// </summary>
		public bool Visible
		{
			get => _visible;
			set => _spriteRenderer.enabled = _visible = value;
		}

		private void Start()
		{
			_spriteRenderer = GetComponent<SpriteRenderer>();

			Vector3 initialPosition = GameObject.Find("Inventory").GetComponentsInChildren<InventorySlotBehaviour>()
				.First().transform.localPosition;

			_position = new Lerped<Vector3>(initialPosition, 0.15f, Easing.EaseInOut);

			Visible = false;
		}

		private void Update()
		{
			transform.localPosition = _position.Value;
		}

		/// <summary>
		/// Sets the lerped position's value to the given position.
		/// </summary>
		/// <param name="position">The position to move to.</param>
		/// <param name="instant">Whether the position should be instant or not.</param>
		public void MoveTo(Vector3 position, bool instant = false)
		{
			_position.Value = position;
			if (instant) _position.Finish();
		}
	}
}