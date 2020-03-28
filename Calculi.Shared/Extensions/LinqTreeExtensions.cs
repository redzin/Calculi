using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculi.Shared.Extensions
{
    internal static class LinqTreeExtension
    {
        public static IEnumerable<T> SelectNestedChildren<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> selector)
        {
            foreach (T item in source)
            {
                yield return item;
                foreach (T subItem in SelectNestedChildren(selector(item), selector))
                {
                    yield return subItem;
                }
            }
        }
        internal static IExpression ToExpression(this IEnumerable<Symbol> symbols)
        {
            return symbols.ToList().ToExpression();
        }
        internal static IExpression ToExpression(this List<Symbol> symbols)
        {
            return new Expression(symbols);
        }
    }

}
