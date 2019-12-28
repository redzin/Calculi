using Android.Widget;

using System;

using Android.Support.V7.Widget;
using Android.Views;

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