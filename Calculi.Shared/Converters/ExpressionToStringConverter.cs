using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculi.Shared.Converters
{
    internal class IExpressionToStringConverter : IConverter<IExpression, string>
    {
        IConverter<Symbol, string> symbolToStringConverter;
        public IExpressionToStringConverter(IConverter<Symbol, string> symbolToStringConverter)
        {
            this.symbolToStringConverter = symbolToStringConverter;
        }
        public string Convert(IExpression expression)
        {
            return expression.Aggregate("", (result, symbol) => result + symbolToStringConverter.Convert(symbol));
        }
    }
}
