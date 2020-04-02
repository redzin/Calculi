using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Calculi.Shared.Types;

namespace Calculi.Android2.Fragments
{
    public class KeypadAdvancedFragment : Fragment
    {
        public Action<Symbol> OnSymbolClick = symbol => { };
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.fragment_keypad_advanced, container, false);
            return view;
        }

        public override void OnStart()
        {
            base.OnStart();

            TextView buttonLeftParenthesis = (TextView)Activity.FindViewById(Resource.Id.keypadLeftParenthesis);
            TextView buttonRightParenthesis = (TextView)Activity.FindViewById(Resource.Id.keypadRightParenthesis);
            TextView buttonLn = (TextView)Activity.FindViewById(Resource.Id.keypadLn);
            TextView buttonLog = (TextView)Activity.FindViewById(Resource.Id.keypadLog);
            TextView buttonExp = (TextView)Activity.FindViewById(Resource.Id.keypadExp);
            TextView buttonPow = (TextView)Activity.FindViewById(Resource.Id.keypadPower);
            TextView buttonSqr = (TextView)Activity.FindViewById(Resource.Id.keypadSquare);
            TextView buttonSqrt = (TextView)Activity.FindViewById(Resource.Id.keypadSquareRoot);
            TextView buttonSine = (TextView)Activity.FindViewById(Resource.Id.keypadSine);
            TextView buttonCosine = (TextView)Activity.FindViewById(Resource.Id.keypadCosine);
            TextView buttonTan = (TextView)Activity.FindViewById(Resource.Id.keypadTangent);
            TextView buttonModulo = (TextView)Activity.FindViewById(Resource.Id.keypadModulo);

            buttonLeftParenthesis.Click += (sender, e) => OnSymbolClick(Symbol.LEFT_PARENTHESIS);
            buttonRightParenthesis.Click += (sender, e) => OnSymbolClick(Symbol.RIGHT_PARENTHESIS);
            buttonLn.Click += (sender, e) => OnSymbolClick(Symbol.NATURAL_LOGARITHM);
            buttonLog.Click += (sender, e) => OnSymbolClick(Symbol.LOGARITHM);
            buttonExp.Click += (sender, e) => OnSymbolClick(Symbol.EXP);
            buttonPow.Click += (sender, e) => OnSymbolClick(Symbol.POWER);
            buttonSqr.Click += (sender, e) => OnSymbolClick(Symbol.SQR);
            buttonModulo.Click += (sender, e) => OnSymbolClick(Symbol.MODULO);
            buttonSqrt.Click += (sender, e) => OnSymbolClick(Symbol.SQRT);
            buttonSine.Click += (sender, e) => OnSymbolClick(Symbol.SINE);
            buttonCosine.Click += (sender, e) => OnSymbolClick(Symbol.COSINE);
            buttonTan.Click += (sender, e) => OnSymbolClick(Symbol.TANGENT);
        }
    }
}