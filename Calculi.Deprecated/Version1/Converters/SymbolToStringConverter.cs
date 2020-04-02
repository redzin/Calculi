using System;
using System.Collections.Generic;
using System.Text;

namespace Calculi.Shared.Deprecated.Version1.Converters
{
    internal class SymbolToStringConverter : IConverter<Symbol, string>
    {
        private Dictionary<Symbol, string> translate;
        public SymbolToStringConverter()
        {
            translate = new Dictionary<Symbol, string>() {
                {Symbol.ZERO, ""},
                {Symbol.ONE, ""},
                {Symbol.TWO, ""},
                {Symbol.THREE, ""},
                {Symbol.FOUR, ""},
                {Symbol.FIVE, ""},
                {Symbol.SIX, ""},
                {Symbol.SEVEN, ""},
                {Symbol.EIGHT, ""},
                {Symbol.NINE, ""},
                {Symbol.POINT, ""},
                {Symbol.LEFT_PARENTHESIS, ""},
                {Symbol.RIGHT_PARENTHESIS, ""},
                {Symbol.ADD, ""},
                {Symbol.SUBTRACT, ""},
                {Symbol.MULTIPLY, ""},
                {Symbol.DIVIDE, ""},
                {Symbol.MODULO, ""},
                {Symbol.EXP, ""},
                {Symbol.POWER, ""},
                {Symbol.SQR, ""},
                {Symbol.SQRT, ""},
                {Symbol.LOGARITHM, ""},
                {Symbol.NATURAL_LOGARITHM, ""},
                {Symbol.ANSWER, ""},
                {Symbol.SINE, ""},
                {Symbol.COSINE, ""},
                {Symbol.TANGENT, ""},
                {Symbol.SECANT, ""},
                {Symbol.COSECANT, ""},
                {Symbol.COTANGENT, ""}
            };
        }
        public SymbolToStringConverter(Dictionary<Symbol, string> translate)
        {
            this.translate = translate;
        }
        public string Convert(Symbol symbol)
        {
            return translate[symbol];
        }
    }
}
