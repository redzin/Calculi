using System;

namespace Calculi.Support
{
    public class Try<T>
    {
        public Either<Exception, T> Result { get; }

        private Try(Exception e)
        {
            Result = Result = new Either<Exception, T>(e);
        }
        public Try(Func<T> @try)
        {
            try
            {
                Result = new Either<Exception, T>(@try());
            }
            catch (Exception e)
            {
                Result = new Either<Exception, T>(e);
            }
        }

        public Try<V> Select<V>(Func<T, V> func)
        {
            return Result.Match(
                left: e => new Try<V>(e),
                right: result => new Try<V>(() => func(result)));
        }

    }
}