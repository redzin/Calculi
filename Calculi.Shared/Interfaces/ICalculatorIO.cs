using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Calculi.Shared
{
    interface ICalculatorIO
    {
        public ObservableCollection<CalculationResult> History { get; }
        public int Position { get; set; }
        public Expression Expression { get; set; }
        public void IncrementPosition();
        public void DecrementPosition();
        public int GetIndex();
        public void InsertSymbol(Symbol symbol);
        public void RemoveSymbol();
        public void ClearInput();
        public void MoveInputToHistory(ICalculation result);
    }
}
