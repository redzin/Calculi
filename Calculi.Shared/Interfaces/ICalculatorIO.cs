using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Calculi.Shared
{
    interface ICalculatorIO
    {
        public ObservableCollection<HistoryEntry> history { get; }
        public int position { get; set; }
        public Expression expression { get; set; }
        public void IncrementPosition();
        public void DecrementPosition();
        public int GetIndex();
        public void InsertSymbol(Symbol symbol);
        public void RemoveSymbol();
        public void ClearInput();
        public void MoveInputToHistory(ICalculation result);
    }

    public class HistoryEntry
    {
        public IExpression Expression { get; set; }
        public ICalculation Calculation { get; set; }
        public HistoryEntry(IExpression expression, ICalculation result)
        {
            this.Expression = expression;
            this.Calculation = result;
        }
    }
}
