using System.Globalization;
using Calculi.Shared.Parsing;
using Calculi.Shared.Types;

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
        public static string ParseToString(this Expression expression)
        {
            return expression.ParseToDouble().ToString(CultureInfo.InvariantCulture);
        }
        public static string ToString(this Expression expression)
        {
            return Converters.ExpressionToString == null ? "" : Converters.ExpressionToString(expression);
        }
    }
}
