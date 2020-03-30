using System;
using System.Collections.Generic;
using System.Text;
using Calculi.Shared.Types;

namespace Calculi.Shared
{
    class Events
    {
        public static Action<int> OnCursorPositionIncremented = symbol => { };
        public static Action<int> OnCursorPositionDecremented = symbol => { };
        public static Action<Symbol> OnSymbolInserted = symbol => { };
        public static Action<Symbol> OnSymbolRemoved = symbol => { };
        public static Action<ExpressionCalculationPair> OnHistoryEntryAdded = symbol => { };
        public static Action OnExpressionCleared = () => { };
    }
}
