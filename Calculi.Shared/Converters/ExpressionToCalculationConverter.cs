using Calculi.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculi.Shared.Converters
{
    internal class IExpressionToICalculationConverter : IConverter<IExpression, ICalculation>
    {
        private ICalculatorIO calculatorIO;

        public IExpressionToICalculationConverter(ICalculatorIO calculatorIO)
        {
            this.calculatorIO = calculatorIO;
        }
        public ICalculation Convert(IExpression expression)
        {
            return ParseExpression(expression, calculatorIO.History.ToList());
        }

        private List<int> MarkScope(IExpression expression)
        {
            List<int> scope_marker = new List<int>(expression.Count);
            int scope = 0;
            for (int i = 0; i < expression.Count; i++)
            {
                if (expression[i].IsLeftParenthsisEquivalent())
                {
                    scope++;
                }
                if (expression[i].Equals(Symbol.RIGHT_PARENTHESIS))
                {
                    scope--;
                }
                scope_marker.Insert(i, scope);
            }
            if (scope != 0)
            {
                throw new Exception("Non-matching brackets!");
            }
            return scope_marker;
        }
        private List<int> GetIndicesOfGlobalOperatorsOfTypes(IExpression expression, List<Symbol> types)
        {
            List<int> indices = new List<int>();
            List<int> scope_marker = MarkScope(expression);
            foreach (Symbol type in types)
            {
                indices = expression
                .Select(
                        (symbol, index) => (type.Equals(symbol) && scope_marker[index] == 0 ? index : -1)
                    )
                .Where(is_global_scope => is_global_scope >= 0)
                .ToList();
                if (indices.Count() > 0) return indices;
            }
            return indices;
        }
        private int GetIndexOfFirstGlobalOperatorOfTypes(IExpression expression, List<Symbol> types, bool first)
        {
            List<int> global_operation_indices = GetIndicesOfGlobalOperatorsOfTypes(expression, types);
            switch (first)
            {
                case false:
                    return global_operation_indices.Count > 0 ? global_operation_indices.Last() : -1;
                case true:
                default:
                    return global_operation_indices.Count > 0 ? global_operation_indices.First() : -1;

            }
        }
        private ICalculation ParseExpression(IExpression expression, List<CalculationResult> history)
        {
            return ParseTerms(expression, history);
        }
        private ICalculation ParseTerms(IExpression expression, List<CalculationResult> history)
        {
            if (expression.Count == 0)
            {
                throw new Exception("Invalid expression");
            }
            int symbolIndex = GetIndexOfFirstGlobalOperatorOfTypes(expression, new List<Symbol>() { { Symbol.ADD }, { Symbol.SUBTRACT } }, true);
            Symbol operation = symbolIndex >= 0 ? expression[symbolIndex] : Symbol.EOF;
            switch (operation)
            {
                case Symbol.ADD:
                    IExpression leftChild = expression.Where((symbol, index) => index < symbolIndex).ToExpression();
                    IExpression rightChild = expression.Where((symbol, index) => index > symbolIndex).ToExpression();
                    List<ICalculation> list = new List<ICalculation>() {
                                ParseTerms(leftChild, history),
                                ParseTerms(rightChild, history)
                            };
                    return new Calculation(list, (a) => a[0] + a[1]);
                case Symbol.SUBTRACT:
                    IExpression lhs = expression.Where((symbol, index) => index < symbolIndex).ToExpression();
                    lhs = lhs.Count > 0 ? lhs : new Expression(new List<Symbol>(){ Symbol.ZERO }) ;
                    IExpression rhs = expression.Where((symbol, index) => index > symbolIndex).ToExpression();
                    return new Calculation(
                            new List<ICalculation>() {
                                ParseTerms(lhs, history),
                                ParseTerms(rhs, history)
                            },
                            (a) => a[0] - a[1]
                        );
                default:
                    break;
            }
            return ParseFactor(expression, history);
        }
        private ICalculation ParseFactor(IExpression expression, List<CalculationResult> history)
        {
            if (expression.Count == 0)
            {
                throw new Exception("Invalid expression");
            }
            int symbolIndex = GetIndexOfFirstGlobalOperatorOfTypes(expression, new List<Symbol>() { { Symbol.MULTIPLY }, { Symbol.MODULO }, { Symbol.POWER }, { Symbol.SQR } }, true);
            Symbol operation = symbolIndex >= 0 ? expression[symbolIndex] : Symbol.EOF;

            switch (operation)
            {
                case Symbol.MULTIPLY:
                    return new Calculation(
                            new List<ICalculation>() {
                                ParseFactor(expression.Where((symbol, index) => index < symbolIndex).ToExpression(), history),
                                ParseFactor(expression.Where((symbol, index) => index > symbolIndex).ToExpression(), history)
                            },
                            a => a[0] * a[1]
                        );
                case Symbol.MODULO:
                    return new Calculation(
                            new List<ICalculation>() {
                                ParseFactor(expression.Where((symbol, index) => index < symbolIndex).ToExpression(), history),
                                ParseFactor(expression.Where((symbol, index) => index > symbolIndex).ToExpression(), history)
                            },
                            a => a[0] % a[1]
                        );
                case Symbol.POWER:
                    return new Calculation(
                            new List<ICalculation>() {
                                ParseFactor(expression.Where((symbol, index) => index < symbolIndex).ToExpression(), history),
                                ParseFactor(expression.Where((symbol, index) => index > symbolIndex).ToExpression(), history)
                            },
                            a => Math.Pow(a[0], a[1])
                        );
                case Symbol.SQR:
                    return new Calculation(
                            new List<ICalculation>(){ ParseFactor(expression.Where((symbol, index) => index < symbolIndex).ToExpression(), history) },
                            (a) => a[0] * a[0]
                        );
            }
            return ParseDivision(expression, history);
        }
        private ICalculation ParseDivision(IExpression expression, List<CalculationResult> history)
        {
            if (expression.Count == 0)
            {
                throw new Exception("Invalid expression");
            }
            int symbolIndex = GetIndexOfFirstGlobalOperatorOfTypes(expression, new List<Symbol>() { { Symbol.DIVIDE } }, false);
            Symbol operation = symbolIndex >= 0 ? expression[symbolIndex] : Symbol.EOF;

            switch (operation)
            {
                case Symbol.DIVIDE:
                    return new Calculation(
                            new List<ICalculation>(){
                                ParseDivision(expression.Where((symbol, index) => index < symbolIndex).ToExpression(), history),
                                ParseDivision(expression.Where((symbol, index) => index > symbolIndex).ToExpression(), history)
                            },
                            a => a[0] / a[1]
                        );
                default:
                    break;
            }
            return ParseParenthsis(expression, history);
        }
        private ICalculation ParseParenthsis(IExpression expression, List<CalculationResult> history)
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
                default:
                    break;
            }
            return ParsePoint(expression, history);
        }
        private ICalculation ParseParenthesisHelper(IExpression expression, List<CalculationResult> history, Func<List<double>, double> function)
        {

            if (expression.Count < 3
                || !expression.Last().Equals(Symbol.RIGHT_PARENTHESIS)
            )
            {
                throw new Exception("Invalid expression");
            }
            return new Calculation(
                new List<ICalculation>() { ParseExpression(expression.Where((symbol, index) => index != 0 && index != expression.Count - 1).ToExpression(), history) },
                function
            );
        }
        private ICalculation ParsePoint(IExpression expression, List<CalculationResult> history)
        {
            Symbol operation = expression.ToList().Find(symbol => symbol.Equals(Symbol.POINT) || symbol.Equals(Symbol.ANSWER));
            switch (operation)
            {
                case Symbol.POINT:
                    IExpression lhs = expression.TakeWhile(symbol => !symbol.Equals(Symbol.POINT)).ToExpression();
                    IExpression rhs = expression.SkipWhile(symbol => !symbol.Equals(Symbol.POINT)).Skip(1).ToExpression();
                    if (lhs.Count == 0 && rhs.Count == 0)
                    {
                        throw new Exception("Invalid expression");
                    }
                    lhs = lhs.Count > 0 ? lhs : new Expression() { Symbol.ZERO };
                    rhs = rhs.Count > 0 ? rhs : new Expression() { Symbol.ZERO };
                    return new Calculation(
                        new List<ICalculation>()
                        {
                            ParseInteger(lhs, history),
                            ParseInteger(rhs, history)
                        },
                        a => a[0] + (a[1] / Math.Pow(10, (rhs.Count)))
                    );
                case Symbol.ANSWER:
                    return history.Last().Calculation;
                default:
                    break;
            }
            return ParseInteger(expression, history);
        }
        private ICalculation ParseInteger(IExpression expression, List<CalculationResult> history)
        {
            double n = expression.Aggregate(
                (double)0.0,
                (result, symbol) => result * (double)10.0 + symbol.ToInt()
            );
            return new Calculation(
                new List<ICalculation>(),
                x => n
            );
        }

    }

}
