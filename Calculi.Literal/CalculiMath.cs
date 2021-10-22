using System;
using System.Collections.Generic;
using System.Text;

namespace Calculi.Literal
{
    public static class CalculiMath
    {
        private static double trigonometricEpsilon = 0.5 * Math.Pow(10, -13);

        public static double Sin(double d)
        {
            d = d % (2 * Math.PI);

            if (Math.Abs(d) < trigonometricEpsilon || Math.Abs(d - Math.PI) < trigonometricEpsilon || Math.Abs(d + Math.PI) < trigonometricEpsilon)
                return 0.0;
            else
                return Math.Sin(d);
        }

        public static double Cos(double d)
        {
            d = d % (2 * Math.PI);

            double multipleOfPi = d / Math.PI;

            if (Math.Abs(multipleOfPi - 0.5) < trigonometricEpsilon || Math.Abs(multipleOfPi + 0.5) < trigonometricEpsilon || Math.Abs(multipleOfPi - 1.5) < trigonometricEpsilon || Math.Abs(multipleOfPi + 1.5) < trigonometricEpsilon)
                return 0.0;
            else
                return Math.Cos(d);
        }

        public static double Tan(double d)
        {
            d = d % (2 * Math.PI);

            if (Math.Abs(d) < trigonometricEpsilon || Math.Abs(d - Math.PI) < trigonometricEpsilon || Math.Abs(d + Math.PI) < trigonometricEpsilon)
                return 0.0;
            else
                return Math.Tan(d);
        }

    }
}
