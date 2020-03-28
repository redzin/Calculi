using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Calculi.Android2
{
    public class HistoryFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_history, container, false);
            return view;
        }

        public void setText(String text)
        {
            TextView view = (TextView) this.Activity.FindViewById(Resource.Id.historyText);
            view.Text = text;
        }
    }
}