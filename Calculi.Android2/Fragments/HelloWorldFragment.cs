using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Calculi.Android2.Fragments
{
    public class HelloWorldFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_hello_world, container, false);
            return view;
        }

        public void setText(String text)
        {
            TextView view = (TextView)this.Activity.FindViewById(Resource.Id.helloWorldText);
            view.Text = text;
        }
    }
}