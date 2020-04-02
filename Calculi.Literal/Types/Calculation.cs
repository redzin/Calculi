using System;
using System.Collections.Generic;
using Calculi.Shared.Extensions;

namespace Calculi.Shared.Types
{
    public class Calculation
    {
        public List<Calculation> Children { get; }
        public Func<List<double>, double> Function { get; }

        public Calculation(List<Calculation> children, Func<List<double>, double> function)
        {
            Children = children;
            Function = function;
        }

        public override string ToString()
        {
            return CalculationExtensions.ToString(this);
        }
    }
}
