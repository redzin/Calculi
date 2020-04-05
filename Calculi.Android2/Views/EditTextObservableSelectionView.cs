using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Calculi.Literal;
using Calculi.Literal.Extensions;
using Calculi.Literal.Types;
using Calculi.Support;
using Java.Interop;

namespace Calculi.Android2.Views
{
    public class EditTextObservableSelectionView : EditText
    {
        private Context Context;
        private Observable<Calculator> _calculator;

        public EditTextObservableSelectionView(Context context) : base(context)
        {
            Construct(context);
        }

        public EditTextObservableSelectionView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Construct(context);
        }

        public EditTextObservableSelectionView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Construct(context);
        }

        public EditTextObservableSelectionView(Context context, IAttributeSet attrs, int defStyle, int defStyleRes) : base(context, attrs, defStyle, defStyleRes)
        {
            Construct(context);
        }

        protected EditTextObservableSelectionView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        private void Construct(Context context)
        {
            Context = context;
            _calculator = ((MainActivity)this.Context).Calculator;
        }

        public string Text
        {
            get => base.Text;
            set
            {
                if (base.Text != value) // check needed to break cyclic behavior
                {
                    int cursorPositionStart = _calculator.Value.CursorPositionStart;
                    int cursorPositionEnd = _calculator.Value.CursorPositionEnd;
                    base.Text = value;
                    SetSelection(cursorPositionStart, cursorPositionEnd);
                }
            }
        }

        protected override void OnSelectionChanged(int start, int end)
        {
            if (Context == null)
            {
                return;
            }

            int realStart = TranslateInputPositionToLegalInputPosition(start, _calculator.Value.Expression);
            int realEnd = TranslateInputPositionToLegalInputPosition(end, _calculator.Value.Expression);

            base.OnSelectionChanged(realStart, realEnd);
            base.SetSelection(realStart, realEnd);

            if (_calculator.Value.CursorPositionStart == realStart && _calculator.Value.CursorPositionEnd == realEnd)
            {
                return;
            }

            _calculator.Next(Calculator.Mutate(
                _calculator.Value,
                cursorPositionStart: TranslateInputPositionToCalculatorPosition(realStart, _calculator.Value.Expression),
                cursorPositionEnd: TranslateInputPositionToCalculatorPosition(realEnd, _calculator.Value.Expression)
            ));
        }

        public void SetSelection(int calculatorPositionStart, int calculatorPositionEnd)
        {
            Expression expression = _calculator.Value.Expression;
            int start = expression.Select(symbol => Converters.SymbolToString(symbol).Length)
                .TakeWhile((v, i) => i < calculatorPositionStart).Sum();
            int end = expression.Select(symbol => Converters.SymbolToString(symbol).Length)
                .TakeWhile((v, i) => i < calculatorPositionEnd).Sum();

            if (start == SelectionStart && end == SelectionEnd)
            {
                return;
            }

            base.SetSelection(start, end);
        }

        private int TranslateInputPositionToCalculatorPosition(int rawInputPosition, Expression expression)
        {
            List<Symbol> symbolsInView = expression.ToList();
            Dictionary<int, int> map = symbolsInView.Aggregate(new List<int>() { 0 }, (lengths, symbol) =>
            {
                int length = Converters.SymbolToString(symbol).Length;
                int lengthToAdd = lengths.LastOrDefault() + length;
                lengths.AddRange(Enumerable.Repeat(lengthToAdd, length).Select((v, i) =>
                {
                    if (i < (double)Math.Round((length - 1) / 2.0))
                    {
                        return lengths.LastOrDefault();
                    }

                    return v;
                }));
                return lengths;
            }).ToHashSet().ToList().Select((i, v) => (i, v)).ToDictionary(ivPair => ivPair.i, ivPair => ivPair.v);

            return map[rawInputPosition];
        }

        private int TranslateInputPositionToLegalInputPosition(int rawInputPosition, Expression expression)
        {
            List<Symbol> symbolsInView = expression.ToList();
            List<int> cumulativeLength = symbolsInView.Aggregate(new List<int>(){ 0 }, (lengths, symbol) =>
            {
                int length = Converters.SymbolToString(symbol).Length;
                int lengthToAdd = lengths.LastOrDefault() + length;
                lengths.AddRange(Enumerable.Repeat(lengthToAdd, length).Select((v, i) =>
                {
                    if (i < (double)Math.Round((length - 1) / 2.0))
                    {
                        return lengths.LastOrDefault();
                    }

                    return v;
                }));
                return lengths;
            });

            return cumulativeLength[rawInputPosition];
        }
    }
}