using System;
using System.Security.Cryptography;

namespace Calculi.Support
{
    public class Subscription<T>
    {
        private readonly Action<T> _action;
        private readonly Observable<T> _parentObservable;
        public Subscription(Action<T> action, Observable<T> parentObservable)
        {
            _action = action;
            _parentObservable = parentObservable;
        }

        public void Invoke(T input)
        {
            _action(input);
        }

        public void Unsubscribe()
        {
            _parentObservable.Unsubscribe(this);
        }
    }
}