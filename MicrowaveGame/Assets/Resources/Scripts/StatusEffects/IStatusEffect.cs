namespace Scripts.StatusEffects
{
	public interface IStatusEffect
	{
		/// <summary>
		/// The amount of time the effect should be active, in seconds.
		/// </summary>
		int Duration { get; }

		/// <summary>
		/// Continuously called while the status effect is active.
		/// </summary>
		void Update();

		/// <summary>
		/// Called when the effect has finished.
		/// </summary>
		void Remove();
	}
}