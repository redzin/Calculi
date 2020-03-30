using System;
using System.Collections.Generic;
using System.Linq;
using Calculi.Shared.Extensions;
using Calculi.Shared.Types;

namespace Calculi.Shared.Parsing
{
    static class ExpressionParser
    {
        public static readonly Func<Expression, Calculation, Calculation> Parse =
            (expression, history) => ParseExpression(expression, history);

        private static readonly Func<Expression, Calculation, Calculation> ParseExpression =
            (expression, history)=> ParseTerms(expression, history);

        private static readonly Func<Expression, Calculation, Calculation> ParseTerms =
            (expression, history) =>
            {
                if (expression.Count == 0)
                {
                    throw new Exception("Invalid expression");
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
                    throw new Exception("Invalid expression");
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
                if (expression.Count == 0)
                {
                    throw new Exception("Invalid expression");
                }

                int symbolIndex =
                    GetIndexOfFirstGlobalOperatorOfTypes(expression, new List<Symbol>() {{Symbol.DIVIDE}}, false);
                Symbol operation = symbolIndex >= 0 ? expression[symbolIndex] : Symbol.EOF;

                switch (operation)
                {
                    case Symbol.DIVIDE:
                        return new Calculation(
                            new List<Calculation>()
                            {
                                ParseDivision(expression.Where((symbol, index) => index < symbolIndex).ToExpression(),
                                    history),
                                ParseDivision(expression.Where((symbol, index) => index > symbolIndex).ToExpression(),
                                    history)
                            },
                            a => a[0] / a[1]
                        );
                }

                return ParseParenthsis(expression, history);
            };

        private static readonly Func<Expression, Calculation, Calculation> ParseParenthsis =
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
                        return ParseParenthesisHelper(expression, history, x => Math.Sin(x[0]));
                    case Symbol.COSINE:
                        return ParseParenthesisHelper(expression, history, x => Math.Cos(x[0]));
                    case Symbol.TANGENT:
                        return ParseParenthesisHelper(expression, history, x => Math.Tan(x[0]));
                    case Symbol.SECANT:
                        return ParseParenthesisHelper(expression, history, x => 1 / Math.Cos(x[0]));
                    case Symbol.COSECANT:
                        return ParseParenthesisHelper(expression, history, x => 1 / Math.Sin(x[0]));
                    case Symbol.COTANGENT:
                        return ParseParenthesisHelper(expression, history, x => 1 / Math.Tan(x[0]));
                    case Symbol.LEFT_PARENTHESIS:
                        return ParseParenthesisHelper(expression, history, x => x[0]);
                }

                return ParsePoint(expression, history);
            };

        private static readonly Func<Expression, Calculation, Func<List<double>, double>, Calculation> ParseParenthesisHelper =
            (expression, history, function) =>
            {
                if (expression.Count < 3 || !expression.Last().Equals(Symbol.RIGHT_PARENTHESIS))
                {
                    throw new Exception("Invalid expression");
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

        private static readonly Func<Expression, Calculation, Calculation> ParsePoint =
            (expression, history) =>
            {
                Symbol operation = expression.ToList()
                    .Find(symbol => symbol.Equals(Symbol.POINT) || symbol.Equals(Symbol.ANSWER));
                switch (operation)
                {
                    case Symbol.POINT:
                        Expression lhs = expression.TakeWhile(symbol => !symbol.Equals(Symbol.POINT)).ToExpression();
                        Expression rhs = expression.SkipWhile(symbol => !symbol.Equals(Symbol.POINT)).Skip(1)
                            .ToExpression();
                        if (lhs.Count == 0 && rhs.Count == 0)
                        {
                            throw new Exception("Invalid expression");
                        }

                        lhs = lhs.Count > 0 ? lhs : new Expression() {Symbol.ZERO};
                        rhs = rhs.Count > 0 ? rhs : new Expression() {Symbol.ZERO};
                        return new Calculation(
                            new List<Calculation>()
                            {
                                ParseInteger(lhs, history),
                                ParseInteger(rhs, history)
                            },
                            a => a[0] + (a[1] / Math.Pow(10, (rhs.Count)))
                        );
                    case Symbol.ANSWER:
                        return history;
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
                    throw new Exception("Non-matching brackets!");
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

        private static Func<Expression, List<Symbol>, bool, int> GetIndexOfFirstGlobalOperatorOfTypes =
            (expression, types, first) =>
            {
                List<int> globalOperationIndices = GetIndicesOfGlobalOperatorsOfTypes(expression, types);
                return first
                    ? globalOperationIndices.Count > 0 ? globalOperationIndices.First() : -1
                    : globalOperationIndices.Count > 0 ? globalOperationIndices.Last() : -1;
            };
    }
}