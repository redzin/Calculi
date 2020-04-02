using System;
using System.Collections.Generic;
using System.Text;

namespace Calculi.Shared.Deprecated.Version1.Converters
{
    internal class StringToSymbolConverter : IConverter<string, Symbol>
    {
        private Dictionary<string, Symbol> translate;
        public StringToSymbolConverter()
        {
            translate = new Dictionary<string, Symbol>()
            {
                {"0", Symbol.ZERO },
                {"1", Symbol.ONE },
                {"2", Symbol.TWO },
                {"3", Symbol.THREE },
                {"4", Symbol.FOUR },
                {"5", Symbol.FIVE },
                {"6", Symbol.SIX },
                {"7", Symbol.SEVEN },
                {"8", Symbol.EIGHT },
                {"9", Symbol.NINE },
                {".", Symbol.POINT },
                {"-", Symbol.SUBTRACT }
            };
        }
        public StringToSymbolConverter(Dictionary<string, Symbol> translate)
        {
            this.translate = translate;
        }
        public Symbol Convert(string source_string)
        {
            return translate[source_string];
        }
    }
}
