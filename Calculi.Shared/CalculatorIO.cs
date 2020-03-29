using System;
using System.Collections.ObjectModel;
using Calculi.Shared.Converters;
using Calculi.Shared.Extensions;

namespace Calculi.Shared
{
    class CalculatorIO : ICalculatorIO
    {
        private ObservableCollection<CalculationResult> _history;
        public ObservableCollection<CalculationResult> History { 
            get {
                return _history;
            }
        }
        public Expression Expression { get; set; }
        public int Position { get; set; }
        public CalculatorIO()
        {
            _history = new ObservableCollection<CalculationResult>();
            Expression = new Expression();
        }
        public void IncrementPosition()
        {
            Position = Position >= Expression.Count ? Position : Position + 1;
        }
        public void DecrementPosition()
        {
            Position = Position <= 0 ? 0 : Position - 1;
        }
        public int GetIndex()
        {
            return Position;
        }
        public void InsertSymbol(Symbol symbol)
        {
            if (symbol.IsLeftParenthsisEquivalent())
            {
                if (ExpressionEvaluates(Expression))
                {
                    Expression.Insert(Position, symbol);
                    IncrementPosition();
                    Expression.Insert(Position, Symbol.RIGHT_PARENTHESIS);
                    return;
                }
                
                Expression.Insert(Position, symbol);
                IncrementPosition();

                if (!ExpressionEvaluates(Expression))
                {
                    Expression.Insert(Position, Symbol.RIGHT_PARENTHESIS);
                }

                return;
            }

            Expression.Insert(Position, symbol);
            IncrementPosition();
            
        }
        public void RemoveSymbol()
        {
            if (Expression.Count > 0 && Position > 0)
            {
                bool removedSymbolIsLeftParenthesisEquivalent = Expression[Position - 1].IsLeftParenthsisEquivalent();
                DecrementPosition();
                Expression.RemoveAt(Position);
                if (removedSymbolIsLeftParenthesisEquivalent && Position < Expression.Count && Expression[Position].Equals(Symbol.RIGHT_PARENTHESIS))
                {
                    if (!ExpressionEvaluates(Expression))
                    {
                        Expression.RemoveAt(Position);
                    }
                }
            }
        }
        private bool ExpressionEvaluates(IExpression expression)
        {
            try
            {
                IExpressionToICalculationConverter converter = new IExpressionToICalculationConverter(this);
                ICalculation calculation = converter.Convert(expression);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public void ClearInput()
        {
            Expression = new Expression();
            Position = 0;
        }
        public void MoveInputToHistory(ICalculation result)
        {
            History.Add(new CalculationResult(Expression, result));
            ClearInput();
        }
    }

}
