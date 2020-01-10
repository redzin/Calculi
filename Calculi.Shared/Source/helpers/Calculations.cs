using Calculi.Shared.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculi.Shared
{
    public enum Symbol
    {
        EOF,
        ZERO,
        ONE,
        TWO,
        THREE,
        FOUR,
        FIVE,
        SIX,
        SEVEN,
        EIGHT,
        NINE,
        POINT,
        LEFT_PARENTHESIS,
        RIGHT_PARENTHESIS,
        ADD,
        SUBTRACT,
        MULTIPLY,
        DIVIDE,
        MODULO,
        EXP,
        POWER,
        SQR,
        SQRT,
        LOGARITHM,
        NATURAL_LOGARITHM,
        ANSWER,
        SINE,
        COSINE,
        TANGENT,
        SECANT,
        COSECANT,
        COTANGENT
    }
    public interface ICalculation : IEnumerable
    {
        public List<ICalculation> Children { get; }
        public Func<List<double>, double> Function { get; }
        public IExpression CollapseToExpression();
        public double ToDouble();
    }
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
    public interface IExpression : IList<Symbol> { }
    public class Expression : IExpression
    {
        private List<Symbol> symbols;
        public Expression()
        {
            symbols = new List<Symbol>();
        }
        public Expression(List<Symbol> symbols)
        {
            this.symbols = symbols;
        }
        public Symbol this[int index] { get => ((IList<Symbol>)symbols)[index]; set => ((IList<Symbol>)symbols)[index] = value; }
        public bool IsReadOnly => ((IList<Symbol>)symbols).IsReadOnly;
        public int Count => ((IList<Symbol>)symbols).Count;
        public void Add(Symbol value)
        {
            ((IList<Symbol>)symbols).Add(value);
        }
        public void Clear()
        {
            ((IList<Symbol>)symbols).Clear();
        }
        public bool Contains(Symbol value)
        {
            return ((IList<Symbol>)symbols).Contains(value);
        }
        public void CopyTo(Symbol[] array, int index)
        {
            ((IList<Symbol>)symbols).CopyTo(array, index);
        }
        public IEnumerator GetEnumerator()
        {
            return symbols.GetEnumerator();
        }
        public int IndexOf(Symbol value)
        {
            return ((IList<Symbol>)symbols).IndexOf(value);
        }
        public void Insert(int index, Symbol value)
        {
            ((IList<Symbol>)symbols).Insert(index, value);
        }
        public void Remove(Symbol value)
        {
            ((IList<Symbol>)symbols).Remove(value);
        }
        public void RemoveAt(int index)
        {
            ((IList<Symbol>)symbols).RemoveAt(index);
        }
        IEnumerator<Symbol> IEnumerable<Symbol>.GetEnumerator()
        {
            return ((IList<Symbol>)symbols).GetEnumerator();
        }
        bool ICollection<Symbol>.Remove(Symbol item)
        {
            return ((IList<Symbol>)symbols).Remove(item);
        }
    }
}
