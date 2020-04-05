using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Calculi.Literal.Types
{
    public class Calculator
    {
        public Expression Expression { get; } = new Expression();
        public int CursorPositionStart { get; } = 0;
        public int CursorPositionEnd { get; } = 0;
        public ReadOnlyCollection<ExpressionCalculationPair> History { get; } = (new List<ExpressionCalculationPair>()).AsReadOnly();

        public Calculator()
        {

        }

        public Calculator(Expression expression, int cursorPositionStart, int cursorPositionEnd, ReadOnlyCollection<ExpressionCalculationPair> history)
        {
            Expression = expression;
            CursorPositionStart = cursorPositionStart;
            CursorPositionEnd = cursorPositionEnd;
            History = history;
        }

        public static Calculator Mutate(
            Calculator source,
            Expression expression = null,
            int? cursorPositionStart = null,
            int? cursorPositionEnd = null,
            ReadOnlyCollection<ExpressionCalculationPair> history = null)
        {
            return new Calculator(
                expression ?? source.Expression,
                cursorPositionStart ?? source.CursorPositionStart,
                cursorPositionEnd ?? source.CursorPositionEnd,
                history ?? source.History
            );
        }
    }
}