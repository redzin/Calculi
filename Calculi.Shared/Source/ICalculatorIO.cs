using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Calculi.Shared
{
    interface ICalculatorIO
    {
        public int position { get; set; }
        public Expression currentExpression { get; set; }
        public List<IExpression> GetHistory();
        public IExpression GetHistory(int i);
        public void BindToHistoryChange(Action<object, NotifyCollectionChangedEventArgs> callback);
        public void IncrementIndex();
        public void DecrementIndex();
        public int GetIndex();
        public void InsertSymbol(Symbol symbol);
        public void RemoveSymbol();
        public void ClearInput();
        public void MoveInputToHistory();
    }
}
