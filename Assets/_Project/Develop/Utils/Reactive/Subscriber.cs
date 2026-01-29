using System;

namespace _Project.Develop.Runtime.Utils.ReactiveManagement
{
    public class Subscriber<T, K> : IDisposable
    {
        private Action<T, K> _action;
        private Action<Subscriber<T, K>> _unsubscribe;

        public Subscriber(Action<T, K> action, Action<Subscriber<T, K>> unsubscribe)
        {
            _action = action;
            _unsubscribe = unsubscribe;
        }
        
        public void Invoke(T oldValue, K newValue) => _action?.Invoke(oldValue, newValue);
        
        public void Dispose()
        {
            _unsubscribe?.Invoke(this);
        }
    }
}