using System;
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Calculi.Shared.Types;

namespace Calculi.Android2.Fragments
{
    public class KeypadFragment : Fragment
    {
        public Action<Symbol> OnClick = symbol => { };
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_keypad, container, false);
            return view;
        }

        public override void OnStart()
        {
            base.OnStart();

            TextView buttonZero = (TextView)Activity.FindViewById(Resource.Id.keypadZero);
            TextView buttonOne = (TextView)Activity.FindViewById(Resource.Id.keypadOne);
            TextView buttonTwo = (TextView)Activity.FindViewById(Resource.Id.keypadTwo);
            TextView buttonThree = (TextView)Activity.FindViewById(Resource.Id.keypadThree);
            TextView buttonFour = (TextView)Activity.FindViewById(Resource.Id.keypadFour);
            TextView buttonFive = (TextView)Activity.FindViewById(Resource.Id.keypadFive);
            TextView buttonSix = (TextView)Activity.FindViewById(Resource.Id.keypadSix);
            TextView buttonSeven = (TextView)Activity.FindViewById(Resource.Id.keypadSeven);
            TextView buttonEight = (TextView)Activity.FindViewById(Resource.Id.keypadEight);
            TextView buttonNine = (TextView)Activity.FindViewById(Resource.Id.keypadNine);
            TextView buttonPoint = (TextView)Activity.FindViewById(Resource.Id.keypadPoint);

            buttonZero.Click += (sender, e) => OnClick(Symbol.ZERO);
            buttonOne.Click += (sender, e) => OnClick(Symbol.ONE);
            buttonTwo.Click += (sender, e) => OnClick(Symbol.TWO);
            buttonThree.Click += (sender, e) => OnClick(Symbol.THREE);
            buttonFour.Click += (sender, e) => OnClick(Symbol.FOUR);
            buttonFive.Click += (sender, e) => OnClick(Symbol.FIVE);
            buttonSix.Click += (sender, e) => OnClick(Symbol.SIX);
            buttonSeven.Click += (sender, e) => OnClick(Symbol.SEVEN);
            buttonEight.Click += (sender, e) => OnClick(Symbol.EIGHT);
            buttonNine.Click += (sender, e) => OnClick(Symbol.NINE);
            buttonPoint.Click += (sender, e) => OnClick(Symbol.POINT);

        }
    }
}