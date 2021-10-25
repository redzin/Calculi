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
            if (calculator.CursorPositionStart != calculator.CursorPositionEnd)
            {
                calculator = calculator.RemoveSymbol();
            }

            List<Symbol> symbols = calculator.Expression.ToList();
            symbols.Insert(calculator.CursorPositionStart, symbol);

            Calculator newCalc = Calculator.Mutate(calculator, expression: new Expression(symbols)).IncrementPosition();

            if (Symbols.LeftParenthesisEquivalents.Contains(symbol))
            {
                return newCalc.Expression.ParseToDouble().Result.Match(
                    left: d => {
                        return newCalc;
                    },
                    right: e => {
                        return newCalc.InsertSymbol(Symbol.RIGHT_PARENTHESIS).DecrementPosition();
                    }
                );
            }

            return newCalc;
        }

        public static Calculator RemoveSymbol(this Calculator calculator)
        {
            if (calculator.CursorPositionStart > calculator.Expression.Count)
                return Calculator.Mutate(calculator, cursorPositionStart: calculator.CursorPositionStart, cursorPositionEnd: calculator.CursorPositionStart);
            if (calculator.CursorPositionEnd == 0)
                return calculator;
 
            List<Symbol> symbols = calculator.Expression.ToList();

            if (calculator.CursorPositionStart != calculator.CursorPositionEnd)
            {
                symbols.RemoveRange(calculator.CursorPositionStart, calculator.CursorPositionEnd - calculator.CursorPositionStart);
                return Calculator.Mutate(calculator, expression: new Expression(symbols), cursorPositionStart: calculator.CursorPositionStart, cursorPositionEnd: calculator.CursorPositionStart);
            }
            else
            {
                Symbol removedSymbol = calculator.Expression[calculator.CursorPositionStart - 1];
                symbols.RemoveAt(calculator.CursorPositionStart - 1);

                Calculator newCalc = Calculator.Mutate(calculator, expression: new Expression(symbols), calculator.CursorPositionStart - 1, calculator.CursorPositionEnd - 1);

                if (Symbols.LeftParenthesisEquivalents.Contains(removedSymbol) &&
                    newCalc.Expression.Count > 0 &&
                    newCalc.CursorPositionEnd < newCalc.Expression.Count &&
                    newCalc.Expression[newCalc.CursorPositionStart].Equals(Symbol.RIGHT_PARENTHESIS)
                )
                {
                    return newCalc.Expression.ParseToDouble().Result.Match(
                        left: _ => newCalc,
                        right: e => newCalc.IncrementPosition().RemoveSymbol()
                    );
                }

                return newCalc;
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

        public static Try<Calculator> MoveInputToHistory(this Calculator calculator)
        {
            return Try.Invoke(() =>
            {
                ExpressionCalculationPair entry = new ExpressionCalculationPair(
                    calculator.Expression,
                    calculator.Expression.ParseToCalculation(calculator.History.Count > 0 ? calculator.History.Last().Calculation : null).Unwrap()
                );
                List<ExpressionCalculationPair> newHistory = new List<ExpressionCalculationPair>(calculator.History)
                {
                    entry
                };

                Expression newExpression = entry.Calculation.ToDouble().ToExpression();

                return Calculator.Mutate(calculator, newExpression, newExpression.Count, newExpression.Count, history: newHistory.AsReadOnly());
            });

        }
        
    }
}
