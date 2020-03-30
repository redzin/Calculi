using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calculi.Shared.Types;

namespace Calculi.Shared.Extensions
{
    static class CalculatorExtensions
    {
        public static void IncrementPosition(this Calculator calculator)
        {
            if (calculator.CursorPosition >= calculator.Expression.Count)
            {
                return;
            }

            calculator.CursorPosition++;
            Events.OnCursorPositionIncremented(calculator.CursorPosition);
        }

        public static void DecrementPosition(this Calculator calculator)
        {
            if (calculator.CursorPosition == 0)
            {
                return;
            }

            calculator.CursorPosition--;
            Events.OnCursorPositionDecremented(calculator.CursorPosition);
        }

        public static void InsertSymbol(this Calculator calculator, Symbol symbol)
        {
            calculator.Expression.Insert(calculator.CursorPosition, symbol);
            calculator.IncrementPosition();
            Events.OnSymbolInserted(symbol);
        }

        public static void RemoveSymbol(this Calculator calculator)
        {
            if (calculator.CursorPosition > 0)
            {
                return;
            }

            Symbol removed = calculator.Expression[calculator.CursorPosition];
            calculator.Expression.RemoveAt(calculator.CursorPosition);
            Events.OnSymbolRemoved(removed);
        }

        public static void ClearExpression(this Calculator calculator)
        {
            calculator.Expression = new Expression();
            calculator.CursorPosition = 0;
            Events.OnExpressionCleared();
        }

        public static void MoveInputToHistory(this Calculator calculator)
        {
            ExpressionCalculationPair entry = new ExpressionCalculationPair(
                calculator.Expression,
                calculator.Expression.ParseToCalculation(calculator.History.Count > 0 ? calculator.History.Last().Calculation : null)
            );
            List<ExpressionCalculationPair> newHistory = new List<ExpressionCalculationPair>(calculator.History)
            {
                entry
            };
            calculator.History = newHistory.AsReadOnly();
            calculator.ClearExpression();
            Events.OnHistoryEntryAdded(entry);
        }
        
    }
}
