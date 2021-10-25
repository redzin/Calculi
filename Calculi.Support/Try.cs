using System;

namespace Calculi.Support
{
    public class Try<T>
    {
        public Either<T, Exception> Result { get; }

        public Try(Exception e)
        {
            Result = new Either<T, Exception>(e);
        }
        public Try(Func<T> function)
        {
            try
            {
                Result = new Either<T, Exception>(function());
            }
            catch (Exception e)
            {
                Result = new Either<T, Exception>(e);
            }
        }

        public Try<V> Select<V>(Func<T, V> func)
        {
            return Result.Match(
                left: result => new Try<V>(() => func(result)),
                right: e => new Try<V>(e)
            );
        }

        public void Match(Action<T> success, Action<Exception> error)
        {
            Result.Match(success, error);
        }

        public T Unwrap()
        {
            return Result.Match(v => v, e => throw e);
        }

        public T UnwrapOr(Func<Exception, T> exceptionHandler)
        {
            return Result.Match(v => v, e => exceptionHandler(e));
        }

        public static implicit operator Try<T>(Func<T> function) => new Try<T>(function);
        public static implicit operator Either<T, Exception>(Try<T> t) => t.Result;
    }

    public static class Try
    {
        public static Try<T> Invoke<T>(Func<T> function)
        {
            return function;
        }

        public static Try<T> Throw<T>(Exception e)
        {
            return new Try<T>(e);
        }
    }
}