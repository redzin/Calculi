using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculi.Shared.Deprecated.Version1.Converters
{
    internal class ICalculationToDoubleConverter : IConverter<ICalculation, double>
    {
        public double Convert(ICalculation calculation)
        {
            return calculation.Function(calculation.Children.Select(c => Convert(c)).ToList());
        }
    }
}
