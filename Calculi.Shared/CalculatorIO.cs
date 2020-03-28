using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Calculi.Shared
{
    class CalculatorIO : ICalculatorIO
    {
        private ObservableCollection<HistoryEntry> _history;
        public ObservableCollection<HistoryEntry> history { 
            get {
                return _history;
            }
        }
        public Expression expression { get; set; }
        public int position { get; set; }
        public CalculatorIO()
        {
            _history = new ObservableCollection<HistoryEntry>();
            expression = new Expression();
        }
        public void IncrementPosition()
        {
            position = position >= expression.Count ? position : position + 1;
        }
        public void DecrementPosition()
        {
            position = position <= 0 ? 0 : position - 1;
        }
        public int GetIndex()
        {
            return position;
        }
        public void InsertSymbol(Symbol symbol)
        {
            expression.Insert(position, symbol);
            IncrementPosition();
        }
        public void RemoveSymbol()
        {
            if (expression.Count > 0 && position > 0)
            {
                DecrementPosition();
                expression.RemoveAt(position);
            }
        }
        public void ClearInput()
        {
            expression = new Expression();
            position = 0;
        }
        public void MoveInputToHistory(ICalculation result)
        {
            history.Add(new HistoryEntry(expression, result));
            ClearInput();
        }
    }

}
