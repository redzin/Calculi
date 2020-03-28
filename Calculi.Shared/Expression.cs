using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Calculi.Shared
{
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        bool ICollection<Symbol>.Remove(Symbol item)
        {
            return ((IList<Symbol>)symbols).Remove(item);
        }
    }
}
