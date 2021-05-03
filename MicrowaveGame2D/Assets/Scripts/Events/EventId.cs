using System;

namespace Events
{
    public readonly struct EventId<T> where T : EventArgs
    {
        private readonly ulong _value;

        private EventId(ulong value)
        {
            _value = value;
        }

        public static implicit operator EventId<T>(ulong value) => new EventId<T>(value);
        public static implicit operator ulong(EventId<T> value) => value._value;
    }
}