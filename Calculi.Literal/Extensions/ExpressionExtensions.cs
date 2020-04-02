using System;
using System.Globalization;
using Calculi.Shared.Parsing;
using Calculi.Shared.Types;
using Calculi.Support;

namespace Calculi.Shared.Extensions
{
    static class ExpressionExtensions
    {
        public static Calculation ParseToCalculation(this Expression expression)
        {
            return ExpressionParser.Parse(expression, null);
        }
        public static Calculation ParseToCalculation(this Expression expression, Calculation history)
        {
            return ExpressionParser.Parse(expression, history);
        }
        public static double ParseToDouble(this Expression expression)
        {
            return expression.ParseToCalculation().ToDouble();
        }
        public static Either<Exception, string> ParseToString(this Expression expression)
        {
            return new Try<string>(() => expression.ParseToDouble().ToString(CultureInfo.InvariantCulture)).Result;
        }
        public static string ToString(this Expression expression)
        {
            return Converters.ExpressionToString == null ? "" : Converters.ExpressionToString(expression);
        }
    }
}
