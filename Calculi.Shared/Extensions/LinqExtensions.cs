using System.Collections.Generic;
using System.Linq;
using Calculi.Shared.Types;

namespace Calculi.Shared.Extensions
{
    static class LinqExtensions
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

        public static Expression ToExpression(this IEnumerable<Symbol> symbols)
        {
            return new Expression(symbols.ToList());
        }
    }
}
