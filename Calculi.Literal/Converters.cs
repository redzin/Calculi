using System;
using System.Collections.Generic;
using System.Linq;
using Calculi.Shared.Types;

namespace Calculi.Shared
{
    static class Converters
    {
        public static Func<Symbol, string> SymbolToString; // to be supplied from main program

        public static Func<string, Symbol> StringToSymbol; // to be supplied from main program

        public static Func<Calculation, double> CalculationToDouble = (Calculation calculation) =>
        {
            return calculation.Function(calculation.Children.Select(c => Converters.CalculationToDouble(c)).ToList());
        };

        public static Func<Expression, string> ExpressionToString = (Expression expression) =>
        {
            return SymbolToString == null
                ? "Calculi.Shared-Converters.SymbolToStringMap not set"
                : expression.Aggregate("", (result, symbol) => result + SymbolToString(symbol));
        };

        public static Func<string, Expression> StringToExpression = (string sourceString) =>
        {
            Expression expr = new Expression();
            string extraZeroes = "";
            string baseValue = sourceString.ToList().TakeWhile(c => c != 'E').ToList().Aggregate("", (result, c) => result + c);
            if (sourceString.ToList().Exists(c => c == 'E'))
            {
                double exponent = System.Convert.ToDouble(sourceString.SkipWhile(c => c != 'E').Skip(2).ToList().Aggregate("", (result, c) => result + c));
                double numDecimals = baseValue.SkipWhile(c => c != '.').Skip(1).Count();
                double numZeroes = exponent - numDecimals;
                for (int i = 0; i < numZeroes; i++)
                {
                    extraZeroes += "0";
                }
                baseValue = baseValue.Split(".").Aggregate("", (result, c) => result + c);
            }
            return new Expression((baseValue + extraZeroes).ToList().Select(s => StringToSymbol(s.ToString())).ToList());
        };

    }
}
