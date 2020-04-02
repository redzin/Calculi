using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Calculi.Shared.Types
{
    class Calculator
    {
        public Expression Expression { get; }
        public int CursorPosition { get; }
        public ReadOnlyCollection<ExpressionCalculationPair> History { get; }

        public Calculator(Expression expression, int cursorPosition, ReadOnlyCollection<ExpressionCalculationPair> history)
        {
            Expression = expression;
            CursorPosition = cursorPosition;
            History = history;
        }

        public static Calculator Mutate(
            Calculator source,
            Expression expression = null,
            int? cursorPosition = null,
            ReadOnlyCollection<ExpressionCalculationPair> history = null)
        {
            return new Calculator(
                expression ?? source.Expression,
                cursorPosition ?? source.CursorPosition,
                history ?? source.History
            );
        }
    }
}