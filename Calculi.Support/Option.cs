using System;

namespace Calculi.Support
{
    public class Option<T>
    {
        private readonly T _value;
        private readonly bool _some;

        public Option()
        {
            _some = false;
        }

        public Option(T value)
        {
            _some = true;
            _value = value;
        }

        public V Match<V>(Func<T, V> some, Func<V> none)
        {
            return _some ? some(_value) : none();
        }

        public void Match(Action<T> some, Action none)
        {
            if (_some)
            {
                some(_value);
            }
            else
            {
                none();
            }
        }

        public void Match(Action<T> some)
        {
            if (_some)
            {
                some(_value);
            }
        }

        public Option<V> Map<V>(Func<T, V> function)
        {
            if (_some)
                return Option.Some<V>(function(_value));
            return Option.None<V>();
        }

        public T Unwrap ()
        {
            if (!_some)
                throw new UnwrappedEmptyException();
            return _value;
        }

        public T UnwrapOr(Func<T> a)
        {
            if (!_some)
                return a.Invoke();
            return _value;
        }

        public static implicit operator Option<T>(T value) => new Option<T>(value);

    }

    public static class Option
    {
        public static Option<T> None<T>()
        {
            return new Option<T>();
        }

        public static Option<T> Some<T>(T value)
        {
            return value;
        }
    }

    public class UnwrappedEmptyException : Exception
    {

    }
}
