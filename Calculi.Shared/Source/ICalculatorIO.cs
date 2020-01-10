using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Calculi.Shared
{
    interface ICalculatorIO
    {
        public int position { get; set; }
        public Expression currentExpression { get; set; }
        public List<HistoryEntry> GetHistory();
        public HistoryEntry GetHistoryEntry(int i);
        public void BindToHistoryChange(Action<object, NotifyCollectionChangedEventArgs> callback);
        public void IncrementIndex();
        public void DecrementIndex();
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
