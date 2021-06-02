using Player;
using UnityEngine;

namespace Items
{
	public class SpeedIncreaseItem : BaseItem
	{
		public float IncreaseValue, DurationValue;

		private PlayerMovement _playerMovement;
		private float _time;

		private bool _used;
		public override bool Used => _used;

		private void Start()
		{
			_playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
		}

		public override void OnUseItem()
		{
			if (_used) return;

			_playerMovement.Speed += IncreaseValue;
			_used = true;
		}

		public override void OnItemUpdate()
		{
			if (!_used) return;

			_time += Time.deltaTime;
			if (_time >= DurationValue) Destroy(gameObject);
		}

		public override void OnPickupItem() { }

		public override void OnDropItem() { }

		private void OnDestroy()
		{
			_playerMovement.Speed -= IncreaseValue;
		}
	}
}