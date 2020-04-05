using System;
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Calculi.Literal.Types;

namespace Calculi.Android2.Fragments
{
    public class KeypadArithmeticFragment : Fragment
    {
        public Action<Symbol> OnSymbolClick = symbol => { };
        public Action OnEnterClick = () => { };
        public Action OnDeleteClick = () => { };
        public Action OnClearClick = () => { };
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_keypad_arithmetic, container, false);
            return view;
        }

        public override void OnStart()
        {
            base.OnStart();

            TextView buttonDivision = (TextView)Activity.FindViewById(Resource.Id.keypadDivision);
            TextView buttonMultiplication = (TextView)Activity.FindViewById(Resource.Id.keypadMultiplication);
            TextView buttonSubtraction = (TextView)Activity.FindViewById(Resource.Id.keypadSubtraction);
            TextView buttonAddition = (TextView)Activity.FindViewById(Resource.Id.keypadAddition);
            TextView buttonEnter = (TextView)Activity.FindViewById(Resource.Id.keypadEnter);
            TextView buttonDelete = (TextView)Activity.FindViewById(Resource.Id.keypadDelete);
            TextView buttonClear = (TextView)Activity.FindViewById(Resource.Id.keypadClear);

            buttonDelete.Click += (sender, e) => OnDeleteClick();
            buttonClear.Click += (sender, e) => OnClearClick();
            buttonEnter.Click += (sender, e) => OnEnterClick();
            buttonDivision.Click += (sender, e) => OnSymbolClick(Symbol.DIVIDE);
            buttonMultiplication.Click += (sender, e) => OnSymbolClick(Symbol.MULTIPLY);
            buttonSubtraction.Click += (sender, e) => OnSymbolClick(Symbol.SUBTRACT);
            buttonAddition.Click += (sender, e) => OnSymbolClick(Symbol.ADD);

        }
    }
}