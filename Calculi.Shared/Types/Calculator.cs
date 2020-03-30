using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Calculi.Shared.Types
{
    class Calculator
    {
        public Expression Expression { get; set; } = new Expression();
        public int CursorPosition { get; set; } = 0;
        public ReadOnlyCollection<ExpressionCalculationPair> History { get; set; } = (new List<ExpressionCalculationPair>()).AsReadOnly();

    }
}