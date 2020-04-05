using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Calculi.Literal.Errors;
using Calculi.Literal.Types;
using Calculi.Support;

namespace Calculi.Literal.Extensions
{
    static class CalculatorExtensions
    {
        public static Calculator IncrementPosition(this Calculator calculator)
        {
            if (calculator.CursorPositionStart >= calculator.Expression.Count)
            {
                return calculator;
            }

            return Calculator.Mutate(calculator, cursorPositionStart: calculator.CursorPositionStart + 1, cursorPositionEnd: calculator.CursorPositionStart + 1);
        }

        public static Calculator DecrementPosition(this Calculator calculator)
        {
            if (calculator.CursorPositionStart == 0)
            {
                return calculator;
            }

            return Calculator.Mutate(calculator, cursorPositionStart: calculator.CursorPositionStart - 1, cursorPositionEnd: calculator.CursorPositionEnd - 1);
        }

        public static Calculator InsertSymbol(this Calculator calculator, Symbol symbol)
        {
            List<Symbol> symbols = calculator.Expression.ToList();

            if (calculator.CursorPositionStart != calculator.CursorPositionEnd)
            {
                calculator = calculator.RemoveSymbol();
            }

            symbols.Insert(calculator.CursorPositionStart, symbol);
            Calculator newCalc = Calculator.Mutate(calculator, expression: new Expression(symbols)).IncrementPosition();

            if (Symbols.LeftParenthesisEquivalents.Contains(symbol))
            {
                return new Try<double>(() => newCalc.Expression.ParseToDouble()).Result.Match(
                    left: (exception => newCalc.InsertSymbol(Symbol.RIGHT_PARENTHESIS).DecrementPosition()),
                    right: (d => newCalc)
                );
            }

            return newCalc;
        }

        public static Calculator RemoveSymbol(this Calculator calculator)
        {
            if (calculator.CursorPositionStart > calculator.Expression.Count || calculator.CursorPositionEnd == 0)
            {
                return calculator;
            }

            List<Symbol> symbols = calculator.Expression.ToList();

            if (calculator.CursorPositionStart != calculator.CursorPositionEnd)
            {
                symbols.RemoveRange(calculator.CursorPositionStart, calculator.CursorPositionEnd - calculator.CursorPositionStart);
                return Calculator.Mutate(calculator, expression: new Expression(symbols)).DecrementPosition();
            }
            else
            {
                Symbol removedSymbol = calculator.Expression[calculator.CursorPositionStart - 1];
                symbols.RemoveAt(calculator.CursorPositionStart - 1);

                Calculator newCalc = Calculator.Mutate(calculator, expression: new Expression(symbols));

                if (Symbols.LeftParenthesisEquivalents.Contains(removedSymbol) && newCalc.Expression[calculator.CursorPositionStart - 1].Equals(Symbol.RIGHT_PARENTHESIS))
                {

                    Func<double> a = () =>
                    {
                        double d = newCalc.Expression.ParseToDouble();
                        return d;
                    };

                    return new Try<double>(a).Result.Match(
                        left: (exception) => newCalc.RemoveSymbol().DecrementPosition(),
                        right: (d => newCalc.DecrementPosition())
                    );
                }

                return newCalc.DecrementPosition();
            }
        }

        public static Calculator ClearExpression(this Calculator calculator)
        {
            return Calculator.Mutate(calculator, expression: new Expression(), cursorPositionStart: 0, cursorPositionEnd: 0);
        }

        public static Calculator ClearHistory(this Calculator calculator)
        {
            return Calculator.Mutate(calculator, history: (new List<ExpressionCalculationPair>()).AsReadOnly());
        }

        public static Either<Exception, Calculator> MoveInputToHistory(this Calculator calculator)
        {

            return new Try<Calculator>(() =>
            {
                    ExpressionCalculationPair entry = new ExpressionCalculationPair(
                        calculator.Expression,
                        calculator.Expression.ParseToCalculation(calculator.History.Count > 0 ? calculator.History.Last().Calculation : null)
                    );
                    List<ExpressionCalculationPair> newHistory = new List<ExpressionCalculationPair>(calculator.History)
                    {
                        entry
                    };
                    return Calculator.Mutate(calculator, history: newHistory.AsReadOnly()).ClearExpression();
                
            }).Result;

        }
        
    }
}
