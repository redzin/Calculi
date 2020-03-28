using System;
using System.Collections;
using System.Collections.Generic;

namespace Calculi.Shared
{
    public interface ICalculation : IEnumerable
    {
        public List<ICalculation> Children { get; }
        public Func<List<double>, double> Function { get; }
        public Func<Expression> CollapseToExpression { get; }
        public Func<double> ToDouble { get; }
    }
}
