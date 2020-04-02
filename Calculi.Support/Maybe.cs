using System;

namespace Calculi.Support
{
    class Maybe<T>
    {
        private readonly T _value;
        private readonly bool _some;

        public Maybe()
        {
            _some = false;
        }

        public Maybe(T value)
        {
            _some = true;
            _value = value;
        }


        public V Match<V>(Func<T, V> some, Func<V> none)
        {
            return _some ? some(_value) : none();
        }
    }
}
