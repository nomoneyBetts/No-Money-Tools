using UnityEngine;

namespace NoMoney.InputSystem
{
    public class InputData
    {
        public readonly string Name;
        public readonly float Timestamp = Time.realtimeSinceStartup;
        public readonly IBroadcaster Broadcaster;

        // Intended to be a primitive value or struct.
        private readonly object _value;

        public InputData(string name, object value, IBroadcaster broadcaster)
        {
            Name = name;
            _value = value;
            Broadcaster = broadcaster;
        }

        public T ReadValue<T>() => (T)_value;
    }
}
