using System;
using System.Collections.Generic;

namespace Calculi.Literal.Types
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
        FACTORIAL,
        EXP,
        POWER,
        SQR,
        SQRT,
        LOGARITHM,
        NATURAL_LOGARITHM,
        ANSWER, // Deprecated
        SINE,
        COSINE,
        TANGENT,
        SECANT,
        COSECANT,
        COTANGENT,
        PI,
        EULER_CONSTANT
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

        public static List<Symbol> Constants
        {
            get
            {
                return new List<Symbol>()
                {
                    Symbol.PI,
                    Symbol.EULER_CONSTANT
                };
            }
        }
    }
}
