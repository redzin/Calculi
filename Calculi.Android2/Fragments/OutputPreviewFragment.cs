using System;
using System.Collections.Generic;
using Android.OS;
using Android.Support.V4.App;
using Activity = Android.Support.V7.App.AppCompatActivity;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Calculi.Android2.Views;
using Calculi.Literal.Extensions;
using Calculi.Literal.Types;
using Calculi.Support;

namespace Calculi.Android2.Fragments
{
    public class OutputPreviewFragment : Fragment
    {
        public Action<int, int> OnCursorChange = (start, end) => { };
        public Observable<Expression> Expression = new Observable<Expression>(new Expression());
        private EditTextObservableSelectionView OutputView { get; set; }
        private TextView PreviewView { get; set; }
        private readonly List<Subscription<Expression>> _subscriptions = new List<Subscription<Expression>>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_output_preview, container, false);
            return view;
        }

        public override void OnStart()
        {
            base.OnStart();
            OutputView = (EditTextObservableSelectionView)Activity.FindViewById(Resource.Id.outputText);
            PreviewView = (TextView)Activity.FindViewById(Resource.Id.previewText);
            OutputView.ShowSoftInputOnFocus = false;
            OutputView.RequestFocus();
            Expression.Subscribe(expression =>
            {
                OutputView.Text = expression.ToString();
                expression.ParseToString().Match(
                    left: e => PreviewView.Text = "",   
                    right: (s) => PreviewView.Text = s
                );
            });
        }

        public override void OnDestroy()
        {
            _subscriptions.ForEach(sub => sub.Unsubscribe());
        }
    }
}