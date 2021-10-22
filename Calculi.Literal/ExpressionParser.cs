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
        public static readonly Func<Expression, Calculation, Calculation> Parse =
            (expression, history) => ParseExpression(expression, history);

        private static readonly Func<Expression, Calculation, Calculation> ParseExpression =
            (expression, history) =>
            {
                if (expression.Count == 0)
                {
                    throw new UserMessageException(Error.EMPTY_EXPRESSION);
                }
                return ParseTerms(expression, history);
            };

        private static readonly Func<Expression, Calculation, Calculation> ParseTerms =
            (expression, history) =>
            {
                if (expression.Count == 0)
                {
                    throw new UserMessageException(Error.MISSING_TERM);
                }

                int symbolIndex = GetIndexOfFirstGlobalOperatorOfTypes(expression,
                    new List<Symbol>() {{Symbol.ADD}, {Symbol.SUBTRACT}}, true);
                Symbol operation = symbolIndex >= 0 ? expression[symbolIndex] : Symbol.EOF;
                switch (operation)
                {
                    case Symbol.ADD:
                        Expression leftChild = expression.Where((symbol, index) => index < symbolIndex).ToExpression();
                        Expression rightChild = expression.Where((symbol, index) => index > symbolIndex).ToExpression();
                        List<Calculation> list = new List<Calculation>()
                        {
                            ParseTerms(leftChild, history),
                            ParseTerms(rightChild, history)
                        };
                        return new Calculation(list, (a) => a[0] + a[1]);
                    case Symbol.SUBTRACT:
                        Expression lhs = expression.Where((symbol, index) => index < symbolIndex).ToExpression();
                        lhs = lhs.Count > 0 ? lhs : new Expression(new List<Symbol>() {Symbol.ZERO});
                        Expression rhs = expression.Where((symbol, index) => index > symbolIndex).ToExpression();
                        return new Calculation(
                            new List<Calculation>()
                            {
                                ParseTerms(lhs, history),
                                ParseTerms(rhs, history)
                            },
                            (a) => a[0] - a[1]
                        );
                }

                return ParseFactor(expression, history);
            };

        private static readonly Func<Expression, Calculation, Calculation> ParseFactor =
            (expression, history) =>
            {
                if (expression.Count == 0)
                {
                    throw new UserMessageException(Error.MISSING_FACTOR);
                }

                int symbolIndex = GetIndexOfFirstGlobalOperatorOfTypes(expression,
                    new List<Symbol>() {{Symbol.MULTIPLY}, {Symbol.MODULO}, {Symbol.POWER}, {Symbol.SQR}}, true);
                Symbol operation = symbolIndex >= 0 ? expression[symbolIndex] : Symbol.EOF;

                switch (operation)
                {
                    case Symbol.MULTIPLY:
                        return new Calculation(
                            new List<Calculation>()
                            {
                                ParseFactor(expression.Where((symbol, index) => index < symbolIndex).ToExpression(),
                                    history),
                                ParseFactor(expression.Where((symbol, index) => index > symbolIndex).ToExpression(),
                                    history)
                            },
                            a => a[0] * a[1]
                        );
                    case Symbol.MODULO:
                        return new Calculation(
                            new List<Calculation>()
                            {
                                ParseFactor(expression.Where((symbol, index) => index < symbolIndex).ToExpression(),
                                    history),
                                ParseFactor(expression.Where((symbol, index) => index > symbolIndex).ToExpression(),
                                    history)
                            },
                            a => a[0] % a[1]
                        );
                    case Symbol.POWER:
                        return new Calculation(
                            new List<Calculation>()
                            {
                                ParseFactor(expression.Where((symbol, index) => index < symbolIndex).ToExpression(),
                                    history),
                                ParseFactor(expression.Where((symbol, index) => index > symbolIndex).ToExpression(),
                                    history)
                            },
                            a => Math.Pow(a[0], a[1])
                        );
                    case Symbol.SQR:
                        return new Calculation(
                            new List<Calculation>()
                            {
                                ParseFactor(expression.Where((symbol, index) => index < symbolIndex).ToExpression(),
                                    history)
                            },
                            (a) => a[0] * a[0]
                        );
                }

                return ParseDivision(expression, history);
            };

        private static readonly Func<Expression, Calculation, Calculation> ParseDivision =
            (expression, history) =>
            {
                //if (expression.Count == 0)
                //{
                //    throw new UserMessageException(Error.MISSING_DIVISION);
                //}

                int symbolIndex =
                    GetIndexOfFirstGlobalOperatorOfTypes(expression, new List<Symbol>() {{Symbol.DIVIDE}}, false);
                Symbol operation = symbolIndex >= 0 ? expression[symbolIndex] : Symbol.EOF;

                switch (operation)
                {
                    case Symbol.DIVIDE:
                        return new Calculation(
                                new Try<List<Calculation>>( () =>
                                {
                                    List<Calculation> children = new List<Calculation>();
                                    new Try<Calculation>(() =>
                                        ParseDivision(expression.Where((symbol, index) => index < symbolIndex).ToExpression(),history))
                                            .Result
                                            .Match(
                                                left: (e) => throw new UserMessageException(Error.MISSING_NUMERATOR),
                                                right: (child) => children.Add(child)
                                            );
                                    new Try<Calculation>(() =>
                                        ParseDivision(expression.Where((symbol, index) => index > symbolIndex).ToExpression(), history))
                                            .Result
                                            .Match(
                                                left: (e) => throw new UserMessageException(Error.MISSING_DENOMINATOR),
                                                right: (child) => children.Add(child)
                                            );
                                    return children;

                                })
                                .Result.Match(
                                    left: (e) => throw new UserMessageException(Error.MISSING_FACTOR),
                                    right: (children) => children
                                ),
                            a =>
                            {
                                return new Try<double>(() =>
                                {
                                    return a[1] != 0 ? a[0] / a[1] : throw new UserMessageException(Error.DIVISION_BY_ZERO);
                                }).Result.Match(
                                    left: (e) => throw new UserMessageException(Error.UNKNOWN_ERROR),
                                    right: (result) => result
                                );
                            });
                }

                return ParseParenthesis(expression, history);
            };

        private static readonly Func<Expression, Calculation, Calculation> ParseParenthesis =
            (expression, history) =>
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
            };

        private static readonly Func<Expression, Calculation, Func<List<double>, double>, Calculation> ParseParenthesisHelper =
            (expression, history, function) =>
            {
                if (expression.Count < 3 || !expression.Last().Equals(Symbol.RIGHT_PARENTHESIS))
                {
                    throw new UserMessageException(Error.MISMATCHING_PARENTHESIS);
                }
                return new Calculation(
                    new List<Calculation>()
                    {
                        ParseExpression(
                            expression.Where((symbol, index) => index != 0 && index != expression.Count - 1)
                                .ToExpression(),
                            history)
                    },
                    function
                );
            };

        private static readonly Func<Expression, Calculation, Calculation> ParseConstant =
            (expression, history) =>
            {

                List<Symbol> constants = expression.ToList().FindAll((s) => s == Symbol.PI || s == Symbol.EULER_CONSTANT);

                if (constants.Count() > 0 && expression.Count > 1)
                    throw new UserMessageException(Error.REAL_NUMBER_MIXED_WITH_CONSTANT);

                if (constants.Count() > 1)
                    throw new UserMessageException(Error.MULTIPLE_CONSTANTS);

                switch (constants.FirstOrDefault())
                {
                    case Symbol.PI:
                        return Calculation.FromConstant(Math.PI);
                    case Symbol.EULER_CONSTANT:
                        return Calculation.FromConstant(Math.E);
                    default:
                        return ParsePoint(expression, history);
                }
            };

        private static readonly Func<Expression, Calculation, Calculation> ParsePoint =
            (expression, history) =>
            {
                if (expression.Contains(Symbol.POINT))
                {
                    Expression lhs = expression.TakeWhile(symbol => !symbol.Equals(Symbol.POINT)).ToExpression();
                    Expression rhs = expression.SkipWhile(symbol => !symbol.Equals(Symbol.POINT)).Skip(1)
                        .ToExpression();
                    if (lhs.Count == 0 && rhs.Count == 0)
                    {
                        throw new UserMessageException(Error.INVALID_POINT);
                    }

                    lhs = lhs.Count > 0 ? lhs : new Expression(new List<Symbol>() { Symbol.ZERO });
                    rhs = rhs.Count > 0 ? rhs : new Expression(new List<Symbol>() { Symbol.ZERO });
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
            };

        private static readonly Func<Expression, Calculation, Calculation> ParseInteger =
            (expression, history) =>
            {
                double n = expression.Aggregate(
                    0.0,
                    (result, symbol) => result * 10.0 + symbol.ToInteger()
                );
                return new Calculation(
                    new List<Calculation>(),
                    x => n
                );
            };

        private static readonly Func<Expression, List<int>> MarkScope =
            (expression) =>
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
                    throw new UserMessageException(Error.MISMATCHING_PARENTHESIS);
                }

                return scopeMarker;
            };

        private static readonly Func<Expression, List<Symbol>, List<int>>  GetIndicesOfGlobalOperatorsOfTypes =
            (expression, types) =>
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
            };

        private static readonly Func<Expression, List<Symbol>, bool, int> GetIndexOfFirstGlobalOperatorOfTypes =
            (expression, types, first) =>
            {
                List<int> globalOperationIndices = GetIndicesOfGlobalOperatorsOfTypes(expression, types);
                return first
                    ? globalOperationIndices.Count > 0 ? globalOperationIndices.First() : -1
                    : globalOperationIndices.Count > 0 ? globalOperationIndices.Last() : -1;
            };
    }
}