using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Calculi.Shared.Extensions;

namespace Calculi.Shared.Types
{
    class Expression : IEnumerable<Symbol>
    {
        private readonly ReadOnlyCollection<Symbol> _symbols;

        public Expression()
        {
            _symbols = (new List<Symbol>()).AsReadOnly();
        }
        public Expression(List<Symbol> symbols)
        {
            this._symbols = symbols.AsReadOnly();
        }

        public Symbol this[int index] { get => ((IList<Symbol>)_symbols)[index]; set => ((IList<Symbol>)_symbols)[index] = value; }

        public int Count => ((IList<Symbol>)_symbols).Count;

        public bool IsReadOnly => ((IList<Symbol>)_symbols).IsReadOnly;

        public bool Contains(Symbol item)
        {
            return ((IList<Symbol>)_symbols).Contains(item);
        }

        public IEnumerator<Symbol> GetEnumerator()
        {
            return ((IList<Symbol>)_symbols).GetEnumerator();
        }

        public int IndexOf(Symbol item)
        {
            return ((IList<Symbol>)_symbols).IndexOf(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<Symbol>)_symbols).GetEnumerator();
        }

        public override string ToString()
        {
            return ExpressionExtensions.ToString(this);
        }
    }
}