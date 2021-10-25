using System;
using System.Collections.Generic;
using System.Text;

namespace Calculi.Support
{
    public class Either<L, R>
    {
        private readonly L _left;
        private readonly R _right;
        private readonly bool _isLeft;

        public Either(L left)
        {
            _left = left;
            _isLeft = true;
        }

        public Either(R right)
        {
            _right = right;
            _isLeft = false;
        }

        public T Match<T>(Func<L, T> left, Func<R, T> right)
        {
            return _isLeft ? left(_left) : right(_right);
        }

        public void Match(Action<L> left, Action<R> right)
        {
            if (_isLeft)
            {
                left(_left);
            }
            else
            {
                right(_right);
            }
        }

        public Either<TL, TR> Select<TL, TR>(Func<L, TL> left, Func<R, TR> right)
        {
            return _isLeft
                ? new Either<TL, TR>(left(_left))
                : new Either<TL, TR>(right(_right));
        }

        public static implicit operator Either<L, R>(L left) => new Either<L, R>(left);
        public static implicit operator Either<L, R>(R right) => new Either<L, R>(right);
    }
}
