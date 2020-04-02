using System;
using System.Collections.Generic;
using System.Text;

namespace Calculi.Shared.Deprecated.Version1.Extensions
{
    public static class SymbolExtensions
    {
        public static bool IsLeftParenthsisEquivalent(this Symbol symbol)
        {
            return Symbols.LeftParenthesisEquivalents.Contains(symbol);
        }
        public static bool IsNumeral(this Symbol symbol)
        {
            return Symbols.Numerals.Contains(symbol);
        }
        public static int ToInt(this Symbol symbol)
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
