using System;
using System.Globalization;
using Calculi.Literal.Parsing;
using Calculi.Literal.Types;
using Calculi.Support;

namespace Calculi.Literal.Extensions
{
    static class ExpressionExtensions
    {
        public static Try<Calculation> ParseToCalculation(this Expression expression)
        {
            return ExpressionParser.Parse(expression, null);
        }
        public static Try<Calculation> ParseToCalculation(this Expression expression, Calculation history)
        {
            return ExpressionParser.Parse(expression, history);
        }
        public static Try<double> ParseToDouble(this Expression expression)
        {
            return Try.Invoke(() => expression.ParseToCalculation().Unwrap().ToDouble());
        }
        public static Try<string> ParseToString(this Expression expression)
        {
            return Try.Invoke(() => expression.ParseToDouble().Unwrap().ToString(CultureInfo.InvariantCulture));
        }
        public static string ToString(this Expression expression)
        {
            return Converters.ExpressionToString == null ? "" : Converters.ExpressionToString(expression);
        }
    }
}
