using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculi.Shared.Deprecated.Version1
{
    public class Calculation : ICalculation
    {
        public List<ICalculation> Children { get; private set; }
        public Func<List<double>, double> Function { get; private set; }
        public Calculation(List<ICalculation> children, Func<List<double>, double> Function)
        {
            this.Children = children;
            this.Function = Function;
        }
        public IEnumerator GetEnumerator()
        {
            foreach(ICalculation child in Children)
            {
                yield return child;
            }
        }
        public IExpression CollapseToExpression()
        {
            double doubleValue = this.ToDouble();
            return new Expression(doubleValue.ToString("0." + new string('#', 339)).ToList().Select(c => {
                switch (c.ToString())
                {
                    case "0":
                        return Symbol.ZERO;
                    case "1":
                        return Symbol.ONE;
                    case "2":
                        return Symbol.TWO;
                    case "3":
                        return Symbol.THREE;
                    case "4":
                        return Symbol.FOUR;
                    case "5":
                        return Symbol.FIVE;
                    case "6":
                        return Symbol.SIX;
                    case "7":
                        return Symbol.SEVEN;
                    case "8":
                        return Symbol.EIGHT;
                    case "9":
                        return Symbol.NINE;
                    default:
                        return Symbol.POINT;
                }
            }).ToList());
        }
        public double ToDouble()
        {
            return this.Function(this.Children.Select(c => c.ToDouble()).ToList());
        }
    }
}
