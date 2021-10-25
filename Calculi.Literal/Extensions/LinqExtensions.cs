using System.Collections.Generic;
using System.Linq;
using Calculi.Literal.Types;
using Calculi.Support;

namespace Calculi.Literal.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> AllButLast<T>(this IEnumerable<T> enumerable)
        {
            int count = enumerable.Count();
            int counter = 0;
            foreach (T element in enumerable)
            {
                counter++;
                if (counter >= count)
                {
                    yield break;
                }
                yield return element;
            }
        }

        public static Option<Expression> ToExpression(this IEnumerable<Symbol> symbols)
        {
            if (symbols.Count() > 0)
                return new Expression(symbols.ToList());
            else
                return Option.None<Expression>();
        }
    }
}
