using System.Collections;
using System.Collections.Generic;
using Calculi.Shared.Extensions;

namespace Calculi.Shared.Types
{
    class Expression : IList<Symbol>
    {
        private readonly List<Symbol> symbols;

        public Expression()
        {
            this.symbols = new List<Symbol>();
        }
        public Expression(List<Symbol> symbols)
        {
            this.symbols = symbols;
        }

        public Symbol this[int index] { get => ((IList<Symbol>)symbols)[index]; set => ((IList<Symbol>)symbols)[index] = value; }

        public int Count => ((IList<Symbol>)symbols).Count;

        public bool IsReadOnly => ((IList<Symbol>)symbols).IsReadOnly;

        public void Add(Symbol item)
        {
            ((IList<Symbol>)symbols).Add(item);
        }

        public void Clear()
        {
            ((IList<Symbol>)symbols).Clear();
        }

        public bool Contains(Symbol item)
        {
            return ((IList<Symbol>)symbols).Contains(item);
        }

        public void CopyTo(Symbol[] array, int arrayIndex)
        {
            ((IList<Symbol>)symbols).CopyTo(array, arrayIndex);
        }

        public IEnumerator<Symbol> GetEnumerator()
        {
            return ((IList<Symbol>)symbols).GetEnumerator();
        }

        public int IndexOf(Symbol item)
        {
            return ((IList<Symbol>)symbols).IndexOf(item);
        }

        public void Insert(int index, Symbol item)
        {
            ((IList<Symbol>)symbols).Insert(index, item);
        }

        public bool Remove(Symbol item)
        {
            return ((IList<Symbol>)symbols).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<Symbol>)symbols).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<Symbol>)symbols).GetEnumerator();
        }

        public override string ToString()
        {
            return ExpressionExtensions.ToString(this);
        }
    }
}