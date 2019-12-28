﻿
using System;

using Calculi.Shared;
using Calculi.Shared.Converters;

using Android.Support.V7.Widget;
using Android.Views;

using System.Linq;

/* TODO
 * 
 * Refactor into more readable code, more files?
 * Refactor on-click bindings to methods
 * Refactor computation to happen only ever through one main-activity method, implement exception handling
 * Implement config for calculator class
 * Refactor layout file names
 */


/*
TODO:
    * Add "sneak peak" calculation area
    * Format ui history elements properly and make them clickable
    * Swipe for constants/functions
    * Upload to google app store
    * Dark mode / other themes
    * Landscape mode?
*/

namespace Calculi
{
    public class ICalculatorIOHistoryAdapter : RecyclerView.Adapter
    {
        IConverter<ICalculation, double> calculationToDoubleConverter;
        IConverter<IExpression, ICalculation> expressionToICalculationConverter;
        IConverter<IExpression, string> expressionToStringConverter;
        public event EventHandler<int> ItemClick;
        private ICalculatorIO calculator;
        public override int ItemCount
        {
            get { return calculator.GetHistory().Count; }
        }
        void OnClick(int position)
        {
                ItemClick(this, position);
        }
        internal ICalculatorIOHistoryAdapter(
                ICalculatorIO calculator,
                IConverter<ICalculation, double> calculationToDoubleConverter,
                IConverter<IExpression, ICalculation> expressionToICalculationConverter,
                IConverter<IExpression, string> expressionToStringConverter
            )
        {
            this.calculationToDoubleConverter = calculationToDoubleConverter;
            this.expressionToICalculationConverter = expressionToICalculationConverter;
            this.expressionToStringConverter = expressionToStringConverter;

            this.calculator = calculator;
            calculator.BindToHistoryChange((sender, e) => {
                this.NotifyItemRangeRemoved(0, 1);
                this.NotifyItemInserted(calculator.GetHistory().Count-1);
            });
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            CalculationHistoryViewHolder vh = holder as CalculationHistoryViewHolder;
            IExpression expr = calculator.GetHistory(position);
            ICalculation calc = expressionToICalculationConverter.Convert(expr);
            double result = calculationToDoubleConverter.Convert(calc);
            vh.calculationResult.Text = result.ToString();
            vh.calculationExpression.Text = expressionToStringConverter.Convert(calculator.GetHistory(position));
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the CardView for the photo:
            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Layout.history_entry, parent, false);

            // Create a ViewHolder to hold view references inside the CardView:
            CalculationHistoryViewHolder vh = new CalculationHistoryViewHolder(itemView, OnClick);
            return vh;
        }
    }

}