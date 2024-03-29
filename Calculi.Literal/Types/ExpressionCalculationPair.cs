﻿
namespace Calculi.Literal.Types
{
    public class ExpressionCalculationPair
    {
        public Expression Expression { get; }
        public Calculation Calculation { get; }

        public ExpressionCalculationPair(Expression expression, Calculation calculation)
        {
            Expression = expression;
            Calculation = calculation;
        }
    }
}