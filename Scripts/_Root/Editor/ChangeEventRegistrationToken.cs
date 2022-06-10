using System;
using UnityEngine.UIElements;

namespace NoMoney
{
    /// <summary>
    /// Hold a list of these to easily unregister from value changed events.
    /// </summary>
    public class ChangeEventRegistrationToken<T> : IDisposable
    {
        public VisualElement Element;
        public EventCallback<ChangeEvent<T>> Event;
        public void Dispose()
        {
            Element.UnregisterCallback(Event);
        }
    }
}
