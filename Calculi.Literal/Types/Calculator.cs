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
            Expression newExpr = expression ?? source.Expression;
            int newCursorPositionStart = cursorPositionStart ?? source.CursorPositionStart;
            int newCursorPositionEnd = cursorPositionEnd ?? source.CursorPositionEnd;

            newCursorPositionEnd = newCursorPositionEnd > newExpr.Count ? newExpr.Count : newCursorPositionEnd;
            newCursorPositionStart = newCursorPositionStart < 0 ? 0 : newCursorPositionStart;

            newCursorPositionStart = newCursorPositionStart > newCursorPositionEnd ? newCursorPositionEnd : newCursorPositionStart;
            newCursorPositionEnd = newCursorPositionEnd < newCursorPositionStart ? newCursorPositionStart : newCursorPositionEnd;

            return new Calculator(
                newExpr,
                newCursorPositionStart,
                newCursorPositionEnd,
                history ?? source.History
            );
        }
    }
}