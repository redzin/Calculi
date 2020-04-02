using System;
using System.Collections.Generic;
using System.Text;

namespace Calculi.Support
{
    class Observable<T>
    {
        public T Value { get; private set; }
        private readonly List<Subscription<T>> _subscriptions = new List<Subscription<T>>();
        public Observable(T value)
        {
            Value = value;
        }

        public void Next(T value)
        {
            Value = value;
            _subscriptions.ForEach(sub => sub.Invoke(value));
        }

        public Subscription<T> Subscribe(Action<T> action)
        {
            Subscription<T> sub = new Subscription<T>(action, this);
            _subscriptions.Add(sub);
            return sub;
        }
        public void Unsubscribe(Subscription<T> subscription)
        {
            _subscriptions.Remove(subscription);
        }
    }
}
