using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Calculi.Shared
{
    class CalculatorIO
    {
        private ObservableCollection<IExpression> history;
        public Expression currentExpression { get; private set; }
        public int index { get; set; }

        public CalculatorIO()
        {
            history = new ObservableCollection<IExpression>();
            currentExpression = new Expression();
        }
        public List<IExpression> GetHistory()
        {
            return history.ToList();
        }
        public IExpression GetHistory(int i)
        {
            return history[i];
        }
        public void BindToHistoryChange(Action<object, System.Collections.Specialized.NotifyCollectionChangedEventArgs> callback)
        {
            this.history.CollectionChanged += (sender, args) => callback(sender, args);
        }

        public void IncrementIndex()
        {
            index = index >= currentExpression.Count ? index : index + 1;
        }
        public void DecrementIndex()
        {
            index = index <= 0 ? 0 : index - 1;
        }

        public int GetIndex()
        {
            return index;
        }
        public void InsertSymbol(Symbol symbol)
        {
            currentExpression.Insert(index, symbol);
            IncrementIndex();
        }
        public void RemoveSymbol()
        {
            if (currentExpression.Count > 0 && index > 0)
            {
                DecrementIndex();
                currentExpression.RemoveAt(index);
            }
        }
        public void ClearInput()
        {
            currentExpression = new Expression();
            index = 0;
        }
        public void AddInputToHistory()
        {
            history.Add(currentExpression);
        }
    }

}