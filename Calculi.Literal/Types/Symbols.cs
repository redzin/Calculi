using System;
using System.Collections.Generic;

namespace Calculi.Shared.Types
{
    public enum Symbol
    {
        EOF,
        ZERO,
        ONE,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX,
        SEVEN,
        EIGHT,
        NINE,
        POINT,
        LEFT_PARENTHESIS,
        RIGHT_PARENTHESIS,
        ADD,
        SUBTRACT,
        MULTIPLY,
        DIVIDE,
        MODULO,
        EXP,
        POWER,
        SQR,
        SQRT,
        LOGARITHM,
        NATURAL_LOGARITHM,
        ANSWER,
        SINE,
        COSINE,
        TANGENT,
        SECANT,
        COSECANT,
        COTANGENT
    }
    public static class Symbols
    {
        public static List<Symbol> LeftParenthesisEquivalents
        {
            get {
                return new List<Symbol>
            {
                Symbol.LEFT_PARENTHESIS,
                Symbol.EXP,
                Symbol.LOGARITHM,
                Symbol.NATURAL_LOGARITHM,
                Symbol.SQRT,
                Symbol.SINE,
                Symbol.COSINE,
                Symbol.TANGENT,
                Symbol.COSECANT,
                Symbol.SECANT,
                Symbol.COTANGENT
            }; ;
            }
        }
        public static List<Symbol> Numerals
        {
            get
            {
                return new List<Symbol>
                {
                    Symbol.ZERO,
                    Symbol.ONE,
                    Symbol.TWO,
                    Symbol.THREE,
                    Symbol.FOUR,
                    Symbol.FIVE,
                    Symbol.SIX,
                    Symbol.SEVEN,
                    Symbol.EIGHT,
                    Symbol.NINE
                };
            }
        }
    }
}
