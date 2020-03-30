using System;
using Android.OS;
using Android.Support.V4.App;
using Activity = Android.Support.V7.App.AppCompatActivity;
using Android.Views;
using Android.Widget;

namespace Calculi.Android2.Fragments
{
    public class OutputPreviewFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_output_preview, container, false);
            return view;
        }
        public void SetOutputText(String text)
        {
            TextView view = (TextView)Activity.FindViewById(Resource.Id.outputText);
            view.Text = text;
        }
        public void SetPreviewText(String text)
        {
            TextView view = (TextView)Activity.FindViewById(Resource.Id.previewText);
            view.Text = text;
        }
    }
}