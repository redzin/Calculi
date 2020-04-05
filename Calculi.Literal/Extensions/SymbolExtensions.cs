using System;
using Calculi.Literal.Types;

namespace Calculi.Literal.Extensions
{
    static class SymbolExtensions
    {
        public static string ToString(this Symbol symbol)
        {
            return Converters.SymbolToString(symbol);
        }
        public static bool IsLeftParenthesisEquivalent(this Symbol symbol)
        {
            return Symbols.LeftParenthesisEquivalents.Contains(symbol);
        }
        public static bool IsNumeral(this Symbol symbol)
        {
            return Symbols.Numerals.Contains(symbol);
        }
        public static int ToInteger(this Symbol symbol)
        {
            switch (symbol)
            {
                case Symbol.ZERO:
                    return 0;
                case Symbol.ONE:
                    return 1;
                case Symbol.TWO:
                    return 2;
                case Symbol.THREE:
                    return 3;
                case Symbol.FOUR:
                    return 4;
                case Symbol.FIVE:
                    return 5;
                case Symbol.SIX:
                    return 6;
                case Symbol.SEVEN:
                    return 7;
                case Symbol.EIGHT:
                    return 8;
                case Symbol.NINE:
                    return 9;
                default:
                    throw new Exception("Cannot convert non-numeral symbol to integer");
            }
        }
    }
}
