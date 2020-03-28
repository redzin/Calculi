using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Calculi.Shared
{
    interface ICalculatorIO
    {
        public int position { get; set; }
        public Expression currentExpression { get; set; }
        public Action<List<HistoryEntry>> GetHistory { get; }
        public Func<HistoryEntry, int> GetHistoryEntry { get; }
        public Action<Action<object, NotifyCollectionChangedEventArgs>> BindCallbackToHistoryChange { get; }
        public Action IncrementIndex { get; }
        public Action DecrementIndex { get; }
        public Func<int> GetIndex { get; }
        public Action<Symbol> InsertSymbol { get; }
        public Action RemoveSymbol { get; }
        public Action ClearInput { get; }
        public Action<ICalculation> MoveInputToHistory { get; }
    }
}
