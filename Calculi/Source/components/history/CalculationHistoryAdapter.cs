using System;
using Calculi.Shared;
using Android.Views;
using System.Linq;
using Android.Support.V7.Widget;

namespace Calculi
{
    public class CalculationHistoryAdapter : RecyclerView.Adapter
    {
        IConverter<ICalculation, double> calculationToDoubleConverter;
        IConverter<IExpression, ICalculation> expressionToICalculationConverter;
        IConverter<IExpression, string> expressionToStringConverter;
        public event EventHandler<int> ItemClick;
        private ICalculatorIO calculator;
        public override int ItemCount
        {
            get { return calculator.History.Count; }
        }
        void OnClick(int position)
        {
                ItemClick(this, position);
        }
        internal CalculationHistoryAdapter(
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
            calculator.History.CollectionChanged += ((sender, e) => {
                this.NotifyItemRangeRemoved(0, 1);
                this.NotifyItemInserted(calculator.History.Count-1);
            });
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            CalculationHistoryViewHolder vh = holder as CalculationHistoryViewHolder;
            CalculationResult historyEntry = calculator.History[position];
            try
            {
                vh.calculationResult.Text = calculationToDoubleConverter.Convert(historyEntry.Calculation).ToString();
                vh.calculationExpression.Text = expressionToStringConverter.Convert(historyEntry.Expression);
            }
            catch (Exception e)
            {
                vh.calculationResult.Text = "Error";
                vh.calculationExpression.Text = "Error";
            }
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