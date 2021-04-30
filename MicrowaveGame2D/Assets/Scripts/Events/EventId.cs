namespace Events
{
    public readonly struct EventId
    {
        private readonly ulong _value;

        private EventId(ulong value)
        {
            _value = value;
        }

        public static implicit operator EventId(ulong value) => new EventId(value);
        public static implicit operator ulong(EventId value) => value._value;
    }
}