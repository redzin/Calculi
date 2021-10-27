using System;
using System.Collections.Generic;
using System.Linq;
using Calculi.Literal.Errors;
using Calculi.Literal.Types;
using Calculi.Literal.Extensions;
using Calculi.Support;

namespace Calculi.Literal.Parsing
{
    static class ExpressionParser
    {
        public static Try<Calculation> Parse(Expression expression, Calculation history)
        {
            return Try.Invoke(() => ParseExpression(expression, history));
        }

        private static Calculation ParseExpression(Expression expression, Calculation history)
        {
            if (expression.Count == 0)
                throw new Error(ErrorCode.EMPTY_EXPRESSION);
            
            return ParseTerms(expression, history);
        }

        private static Calculation ParseTerms(Expression expression, Calculation history)
        {
            if (expression.Count == 0)
                throw new Error(ErrorCode.MISSING_TERM);
            
            int symbolIndex = GetIndexOfFirstGlobalOperatorOfTypes(expression, new List<Symbol>() {{Symbol.ADD}, {Symbol.SUBTRACT}}, true);
            Symbol operation = symbolIndex >= 0 ? expression[symbolIndex] : Symbol.EOF;
            switch (operation)
            {
                case Symbol.ADD:
                    return new Calculation(
                            new List<Calculation>() {
                                ParseTerms(expression.Where((symbol, index) => index < symbolIndex).ToExpression().UnwrapOr(() => new Expression()), history),
                                ParseTerms(expression.Where((symbol, index) => index > symbolIndex).ToExpression().UnwrapOr(() => new Expression()), history)
                            },
                            (a) => a[0] + a[1]
                        );
                case Symbol.SUBTRACT:

                    return new Calculation(new List<Calculation>() {
                                ParseTerms(expression.Where((symbol, index) => index < symbolIndex).ToExpression().Match(some: lhs => lhs.Count > 0 ? lhs : new Expression(new List<Symbol>() {Symbol.ZERO}), none: () => new Expression()), history),
                                ParseTerms(expression.Where((symbol, index) => index > symbolIndex).ToExpression().UnwrapOr(() => new Expression()), history)
                            },
                        (a) => a[0] - a[1]
                    );
            }

            return ParseFactor(expression, history);
        }

        private static Calculation ParseFactor(Expression expression, Calculation history)
        {
            if (expression.Count == 0)
                throw new Error(ErrorCode.MISSING_FACTOR);

            //int leftParenthsisSymbolIndex = GetIndexOfFirstGlobalOperatorOfTypes(expression, Symbols.LeftParenthesisEquivalents, true);
            //if (leftParenthsisSymbolIndex > 0 && (expression.ElementAt(leftParenthsisSymbolIndex - 1).IsConstant() || expression.ElementAt(leftParenthsisSymbolIndex - 1).IsNumeral() || expression.ElementAt(leftParenthsisSymbolIndex - 1).Equals(Symbol.RIGHT_PARENTHESIS)))
            //    return new Calculation(new List<Calculation>()
            //            {
            //                ParseFactor(expression.ToList().GetRange(0, leftParenthsisSymbolIndex).ToExpression().Unwrap(), history),
            //                ParseFactor(expression.ToList().GetRange(leftParenthsisSymbolIndex, expression.Count-leftParenthsisSymbolIndex).ToExpression().Unwrap(), history)
            //            },
            //            a => a[0] * a[1]);

            //int rightParenthsisSymbolIndex = GetIndexOfFirstGlobalOperatorOfTypes(expression, new List<Symbol>() { Symbol.RIGHT_PARENTHESIS }, true);
            //return new Calculation(new List<Calculation>()
            //            {
            //                ParseFactor(expression.ToList().GetRange(0, leftParenthsisSymbolIndex).ToExpression().Unwrap(), history),
            //                ParseFactor(expression.ToList().GetRange(leftParenthsisSymbolIndex, expression.Count-leftParenthsisSymbolIndex).ToExpression().Unwrap(), history)
            //            },
            //            a => a[0] * a[1]);

            int symbolIndex = GetIndexOfFirstGlobalOperatorOfTypes(expression, new List<Symbol>() {{Symbol.MULTIPLY}, {Symbol.MODULO}, {Symbol.POWER}, {Symbol.SQR}}, true);
            Symbol operation = symbolIndex >= 0 ? expression[symbolIndex] : Symbol.EOF;

            if (symbolIndex < 0)
                return ParseDivision(expression, history);

            List<Calculation> factors;
            switch (operation)
            {
                case Symbol.MULTIPLY:
                    factors = new List<Calculation>() {
                        ParseFactor(expression.Where((symbol, index) => index < symbolIndex).ToExpression().UnwrapOr(() => new Expression()), history),
                        ParseFactor(expression.Where((symbol, index) => index > symbolIndex).ToExpression().UnwrapOr(() => new Expression()), history)
                    };
                    return new Calculation(factors, a => a[0] * a[1]);
                case Symbol.MODULO:
                    factors = new List<Calculation>() {
                        ParseFactor(expression.Where((symbol, index) => index < symbolIndex).ToExpression().UnwrapOr(() => new Expression()), history),
                        ParseFactor(expression.Where((symbol, index) => index > symbolIndex).ToExpression().UnwrapOr(() => new Expression()), history)
                    };
                    return new Calculation(factors, a => a[0] % a[1]);
                case Symbol.POWER:
                    factors = new List<Calculation>() {
                        ParseFactor(expression.Where((symbol, index) => index < symbolIndex).ToExpression().UnwrapOr(() => new Expression()), history),
                        ParseFactor(expression.Where((symbol, index) => index > symbolIndex).ToExpression().UnwrapOr(() => throw new Error(ErrorCode.MISSING_EXPONENT)), history)
                    };
                    return new Calculation(factors, a => Math.Pow(a[0], a[1]));
                case Symbol.SQR:
                    factors = new List<Calculation>() {
                        ParseFactor(expression.Where((symbol, index) => index < symbolIndex).ToExpression().UnwrapOr(() => new Expression()), history),
                        ParseFactor(expression.Where((symbol, index) => index > symbolIndex).ToExpression().UnwrapOr(() => throw new Error(ErrorCode.MISSING_EXPONENT)), history)
                    };
                    return new Calculation(factors, a => a[0] * a[0]);
            }

            throw new Error(ErrorCode.UNKNOWN_ERROR);
        }

        private static Calculation ParseDivision(Expression expression, Calculation history)
        {
            int symbolIndex =
                GetIndexOfFirstGlobalOperatorOfTypes(expression, new List<Symbol>() {{Symbol.DIVIDE}}, false);
            Symbol operation = symbolIndex >= 0 ? expression[symbolIndex] : Symbol.EOF;

            switch (operation)
            {
                case Symbol.DIVIDE:
                    return new Calculation(
                        Try.Invoke(() =>
                            {
                                List<Calculation> children = new List<Calculation>();

                                expression.Where((symbol, index) => index < symbolIndex).ToExpression().Match(
                                    some: (leftHandSide) =>
                                        Try.Invoke(() => ParseDivision(leftHandSide, history)).Match(
                                            success: (leftChild) => children.Add(leftChild),
                                            error: (e) => throw e
                                        ),
                                    none: () => throw new Error(ErrorCode.MISSING_NUMERATOR)
                                );

                                expression.Where((symbol, index) => index > symbolIndex).ToExpression().Match(
                                    some: (rightHandSide) => Try.Invoke(() => ParseDivision(rightHandSide, history)).Match(
                                        success: (rightChild) => children.Add(rightChild),
                                        error: (e) => throw e
                                        ),
                                    none: () => throw new Error(ErrorCode.MISSING_DENOMINATOR)
                                );

                                return children;

                            }).Unwrap(),
                        a => a[1] != 0 ? a[0] / a[1] : throw new Error(ErrorCode.DIVISION_BY_ZERO)
                    );
            }

            return ParseParenthesis(expression, history);
        }

        private static Calculation ParseParenthesis(Expression expression, Calculation history)
        {
            Symbol operation = expression.Count > 0 ? expression.First() : Symbol.EOF;

            switch (operation)
            {

                case Symbol.LOGARITHM:
                    return ParseParenthesisHelper(expression, history, x => Math.Log10(x[0]));
                case Symbol.NATURAL_LOGARITHM:
                    return ParseParenthesisHelper(expression, history, x => Math.Log(x[0]));
                case Symbol.EXP:
                    return ParseParenthesisHelper(expression, history, x => Math.Exp(x[0]));
                case Symbol.SQRT:
                    return ParseParenthesisHelper(expression, history, x => Math.Sqrt(x[0]));
                case Symbol.SINE:
                    return ParseParenthesisHelper(expression, history, x => CalculiMath.Sin(x[0]));
                case Symbol.COSINE:
                    return ParseParenthesisHelper(expression, history, x => CalculiMath.Cos(x[0]));
                case Symbol.TANGENT:
                    return ParseParenthesisHelper(expression, history, x => CalculiMath.Tan(x[0]));
                case Symbol.SECANT:
                    return ParseParenthesisHelper(expression, history, x => 1 / CalculiMath.Cos(x[0]));
                case Symbol.COSECANT:
                    return ParseParenthesisHelper(expression, history, x => 1 / CalculiMath.Sin(x[0]));
                case Symbol.COTANGENT:
                    return ParseParenthesisHelper(expression, history, x => 1 / CalculiMath.Tan(x[0]));
                case Symbol.LEFT_PARENTHESIS:
                    return ParseParenthesisHelper(expression, history, x => x[0]);
            }

            return ParseConstant(expression, history);
        }

        private static Calculation ParseParenthesisHelper(Expression expression, Calculation history, Func<List<double>, double>  function)
        {
            if (expression.Count < 3 || !expression.Last().Equals(Symbol.RIGHT_PARENTHESIS))
            {
                throw new Error(ErrorCode.MISMATCHING_PARENTHESIS);
            }

            return expression.Where((symbol, index) => index != 0 && index != expression.Count - 1).ToExpression().Match(
                some: (expr) => new Calculation( new List<Calculation>() { ParseExpression(expr, history) }, function ),
                none: () =>new Calculation(new List<Calculation>() { ParseExpression(new Expression(), history) }, function)
            );
        }

        private static Calculation ParseConstant(Expression expression, Calculation history)
        {
            int symbolIndex = GetIndexOfFirstGlobalOperatorOfTypes(expression, new List<Symbol>() { { Symbol.PI }, { Symbol.EULER_CONSTANT } }, true);

            if (expression.Count > 1 && symbolIndex >= 0)
                return new Calculation(
                    new List<Calculation>()
                    {
                        ParseConstant(expression.Where((symbol, index) => index < symbolIndex).ToExpression().UnwrapOr(() => new Expression(new List<Symbol>() { Symbol.ONE})), history),
                        ParseConstant(expression.Where((symbol, index) => index == symbolIndex).ToExpression().Unwrap(), history),
                        ParseConstant(expression.Where((symbol, index) => index > symbolIndex).ToExpression().UnwrapOr(() => new Expression(new List<Symbol>() { Symbol.ONE})), history),
                    },
                    (a) => a[0] * a[1] * a[2]);

            List<Symbol> constants = expression.ToList().FindAll((s) => s == Symbol.PI || s == Symbol.EULER_CONSTANT);
            switch (constants.FirstOrDefault())
            {
                case Symbol.PI:
                    return Calculation.FromConstant(Math.PI);
                case Symbol.EULER_CONSTANT:
                    return Calculation.FromConstant(Math.E);
            }

            return ParsePoint(expression, history);
        }

        private static Calculation ParsePoint(Expression expression, Calculation history)
        {
            if (expression.Contains(Symbol.POINT))
            {
                Expression lhs = expression.TakeWhile(symbol => !symbol.Equals(Symbol.POINT)).ToExpression().UnwrapOr(() => new Expression(new List<Symbol>() { Symbol.ZERO }));
                Expression rhs = expression.SkipWhile(symbol => !symbol.Equals(Symbol.POINT)).Skip(1).ToExpression().UnwrapOr(() => new Expression(new List<Symbol>() { Symbol.ZERO }));

                return new Calculation(
                    new List<Calculation>()
                    {
                            ParseInteger(lhs, history),
                            ParseInteger(rhs, history)
                    },
                    a => a[0] + (a[1] / Math.Pow(10, (rhs.Count)))
                );
            }

            return ParseInteger(expression, history);
        }

        private static Calculation ParseInteger(Expression expression, Calculation history)
        {
            double n = expression.Aggregate(
                0.0,
                (result, symbol) => result * 10.0 + symbol.ToInteger()
            );
            return new Calculation(
                new List<Calculation>(),
                x => n
            );
        }

        private static List<int> MarkScope (Expression expression)
        {
            List<int> scopeMarker = new List<int>(expression.Count);
            int scope = 0;
            for (int i = 0; i < expression.Count; i++)
            {
                if (expression[i].IsLeftParenthesisEquivalent())
                {
                    scope++;
                }

                if (expression[i].Equals(Symbol.RIGHT_PARENTHESIS))
                {
                    scope--;
                }

                scopeMarker.Insert(i, scope);
            }

            if (scope != 0)
            {
                throw new Error(ErrorCode.MISMATCHING_PARENTHESIS);
            }

            return scopeMarker;
        }

        private static List<int>  GetIndicesOfGlobalOperatorsOfTypes (Expression expression, List<Symbol> types)
        {
            List<int> indices = new List<int>();
            List<int> scopeMarker = MarkScope(expression);
            foreach (Symbol type in types)
            {
                indices = expression
                    .Select(
                        (symbol, index) => (type.Equals(symbol) && scopeMarker[index] == 0 ? index : -1)
                    )
                    .Where(isGlobalScope => isGlobalScope >= 0)
                    .ToList();
                if (indices.Any()) return indices;
            }

            return indices;
        }

        private static int GetIndexOfFirstGlobalOperatorOfTypes (Expression expression, List<Symbol> types, bool first)
        {
            List<int> globalOperationIndices = GetIndicesOfGlobalOperatorsOfTypes(expression, types);
            return first
                ? globalOperationIndices.Count > 0 ? globalOperationIndices.First() : -1
                : globalOperationIndices.Count > 0 ? globalOperationIndices.Last() : -1;
        }
    }
}