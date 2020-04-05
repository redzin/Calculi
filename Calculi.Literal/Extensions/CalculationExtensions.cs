using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Calculi.Literal.Types;

namespace Calculi.Literal.Extensions
{
    static class CalculationExtensions
    {
        public static double ToDouble(this Calculation calculation)
        {
            return calculation.Function(calculation.Children.Select(child => child.ToDouble()).ToList());
        }

        public static string ToString(this Calculation calculation)
        {
            return calculation.ToDouble().ToString(CultureInfo.InvariantCulture);
        }
    }
}
