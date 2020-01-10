using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Calculi.Shared
{
    class CalculatorIO : ICalculatorIO
    {
        private ObservableCollection<HistoryEntry> history;
        public Expression currentExpression { get; set; }
        public int position { get; set; }
        public CalculatorIO()
        {
            history = new ObservableCollection<HistoryEntry>();
            currentExpression = new Expression();
        }
        public List<HistoryEntry> GetHistory()
        {
            return history.ToList();
        }
        public HistoryEntry GetHistoryEntry(int i)
        {
            return history[i];
        }
        public void BindToHistoryChange(Action<object, NotifyCollectionChangedEventArgs> callback)
        {
            this.history.CollectionChanged += (sender, args) => callback(sender, args);
        }
        public void IncrementIndex()
        {
            position = position >= currentExpression.Count ? position : position + 1;
        }
        public void DecrementIndex()
        {
            position = position <= 0 ? 0 : position - 1;
        }
        public int GetIndex()
        {
            return position;
        }
        public void InsertSymbol(Symbol symbol)
        {
            currentExpression.Insert(position, symbol);
            IncrementIndex();
        }
        public void RemoveSymbol()
        {
            if (currentExpression.Count > 0 && position > 0)
            {
                DecrementIndex();
                currentExpression.RemoveAt(position);
            }
        }
        public void ClearInput()
        {
            currentExpression = new Expression();
            position = 0;
        }
        public void MoveInputToHistory(ICalculation result)
        {
            history.Add(new HistoryEntry(currentExpression, result));
            ClearInput();
        }
    }

}
