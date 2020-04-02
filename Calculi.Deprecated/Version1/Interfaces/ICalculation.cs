using System;
using System.Collections;
using System.Collections.Generic;

namespace Calculi.Shared.Deprecated.Version1
{
    public interface ICalculation : IEnumerable
    {
        public List<ICalculation> Children { get; }
        public Func<List<double>, double> Function { get; }
        public IExpression CollapseToExpression();
        public double ToDouble();
    }
}
