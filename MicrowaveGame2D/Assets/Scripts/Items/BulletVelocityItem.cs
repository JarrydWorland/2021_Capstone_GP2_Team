namespace Items
{
	public class BulletVelocityItem : BaseItem
	{
		// This isn't used for passive items as they aren't items you activate or consume.
		public override bool Used => false;

		public override void OnUseItem() { }

		public override void OnItemUpdate() { }

		public override void OnPickupItem() { }

		public override void OnDropItem() { }
	}
}