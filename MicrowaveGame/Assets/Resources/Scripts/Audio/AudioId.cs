namespace Scripts.Audio
{
	public readonly struct AudioId
	{
		private readonly ulong _value;

		private AudioId(ulong value)
		{
			_value = value;
		}

		public static implicit operator AudioId(ulong value) => new AudioId(value);
		public static implicit operator ulong(AudioId value) => value._value;
	}
}
