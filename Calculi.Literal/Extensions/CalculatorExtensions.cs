using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Calculi.Literal.Errors;
using Calculi.Shared.Types;
using Calculi.Support;

namespace Calculi.Shared.Extensions
{
    static class CalculatorExtensions
    {
        public static Calculator IncrementPosition(this Calculator calculator)
        {
            if (calculator.CursorPosition >= calculator.Expression.Count)
            {
                return calculator;
            }

            return Calculator.Mutate(calculator, cursorPosition: calculator.CursorPosition + 1);
        }

        public static Calculator DecrementPosition(this Calculator calculator)
        {
            if (calculator.CursorPosition == 0)
            {
                return calculator;
            }

            return Calculator.Mutate(calculator, cursorPosition: calculator.CursorPosition - 1);
        }

        public static Either<UserMessageException, Calculator> InsertSymbol(this Calculator calculator, Symbol symbol)
        {
            List<Symbol> symbols = calculator.Expression.ToList();
            symbols.Insert(calculator.CursorPosition, symbol);
            Expression newExpression = new Expression(symbols);

            return new Try<Calculation>(() => newExpression.ParseToCalculation(calculator.History.Count > 0 ? calculator.History.Last().Calculation : null))
                .Result.Select<UserMessageException, Calculator>(
                    left: e => e is UserMessageException ? (UserMessageException)e : new UserMessageException(Error.COULD_NOT_INSERT),
                    right: calculation => Calculator.Mutate(calculator, expression: newExpression).IncrementPosition()
                );
        }

        public static Calculator RemoveSymbol(this Calculator calculator)
        {
            if (calculator.CursorPosition > calculator.Expression.Count || calculator.CursorPosition == 0)
            {
                return calculator;
            }

            List<Symbol> symbols = calculator.Expression.ToList();
            symbols.RemoveAt(calculator.CursorPosition - 1);
            return Calculator.Mutate(calculator, expression: new Expression(symbols)).DecrementPosition();
        }

        public static Calculator ClearExpression(this Calculator calculator)
        {
            return Calculator.Mutate(calculator, expression: new Expression(), cursorPosition: 0);
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
