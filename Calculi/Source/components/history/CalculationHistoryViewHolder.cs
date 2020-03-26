using Android.Widget;

using System;

using Android.Support.V7.Widget;
using Android.Views;

namespace Calculi
{
    public class CalculationHistoryViewHolder : RecyclerView.ViewHolder
    {
        public TextView calculationResult { get; private set; }
        public TextView calculationExpression { get; private set; }

        public CalculationHistoryViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            // Locate and cache view references:
            calculationResult = itemView.FindViewById<TextView>(Resource.Id.calculationResultTextView);
            calculationExpression = itemView.FindViewById<TextView>(Resource.Id.calculationExpressionTextView);
            itemView.Click += (sender, e) => listener.Invoke(base.LayoutPosition);
        }
    }

}