using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Calculi.Android2.Fragments;
using Calculi.Shared;
using Calculi.Shared.Deprecated.Version1.Converters;
using Calculi.Shared.Extensions;
using Calculi.Shared.Types;

namespace Calculi.Android2
{
    [Activity(
        Label = "@string/app_name",
        Theme = "@style/AppTheme",
        MainLauncher = true
    )]
    public class MainActivity : AppCompatActivity
    {
        private OutputPreviewFragment outputPreviewFragment;
        private KeypadFragment keypadFragment;
        private KeypadArithmeticFragment keypadArithmeticFragment;
        private Calculator calculator = new Calculator { Expression = { }, History = { }, CursorPosition = 0 };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            SetSymbolStringConverters();
            outputPreviewFragment = (OutputPreviewFragment)SupportFragmentManager.FindFragmentById(Resource.Id.outputPreviewFragment);
            keypadFragment = (KeypadFragment)SupportFragmentManager.FindFragmentById(Resource.Id.keypadFragment);
            keypadArithmeticFragment = (KeypadArithmeticFragment)SupportFragmentManager.FindFragmentById(Resource.Id.keypadArithmeticFragment);

            Events.OnCursorPositionIncremented = i => { };
            Events.OnCursorPositionDecremented = i => { };
            Events.OnSymbolInserted += symbol => { };
            Events.OnSymbolRemoved += symbol => { };
            Events.OnExpressionCleared += () => { UpdateOutputPreviewText(new ExpressionCalculationPair(null, null)); };
            Events.OnHistoryEntryAdded += pair => UpdateOutputPreviewText(pair);

            keypadFragment.OnClick += symbol => calculator.InsertSymbol(symbol);
            keypadArithmeticFragment.OnSymbolClick += symbol => calculator.InsertSymbol(symbol);
            keypadArithmeticFragment.OnEnterClick += () => calculator.MoveInputToHistory();

        }

        private void UpdateOutputPreviewText(ExpressionCalculationPair pair)
        {
            outputPreviewFragment.SetOutputText(pair.Expression == null ? "" : pair.Expression.ToString());
            outputPreviewFragment.SetPreviewText(pair.Calculation == null ? "" : pair.Calculation.ToString());
        }

        private void SetSymbolStringConverters()
        {
            Dictionary<Symbol, string> symbolToStringDictionary = new Dictionary<Symbol, string>() {
                {Symbol.ZERO, Resources.GetString(Resource.String.symbol_0) },
                {Symbol.ONE, Resources.GetString(Resource.String.symbol_1) },
                {Symbol.TWO, Resources.GetString(Resource.String.symbol_2) },
                {Symbol.THREE, Resources.GetString(Resource.String.symbol_3) },
                {Symbol.FOUR, Resources.GetString(Resource.String.symbol_4) },
                {Symbol.FIVE, Resources.GetString(Resource.String.symbol_5) },
                {Symbol.SIX, Resources.GetString(Resource.String.symbol_6) },
                {Symbol.SEVEN, Resources.GetString(Resource.String.symbol_7) },
                {Symbol.EIGHT, Resources.GetString(Resource.String.symbol_8) },
                {Symbol.NINE, Resources.GetString(Resource.String.symbol_9) },
                {Symbol.POINT, Resources.GetString(Resource.String.symbol_point) },
                {Symbol.LEFT_PARENTHESIS, Resources.GetString(Resource.String.symbol_left_parenthesis) },
                {Symbol.RIGHT_PARENTHESIS, Resources.GetString(Resource.String.symbol_right_parenthesis) },
                {Symbol.ADD, Resources.GetString(Resource.String.symbol_add) },
                {Symbol.SUBTRACT, Resources.GetString(Resource.String.symbol_subtract) },
                {Symbol.MULTIPLY, Resources.GetString(Resource.String.symbol_multiply) },
                {Symbol.DIVIDE, Resources.GetString(Resource.String.symbol_divide) },
                {Symbol.MODULO, Resources.GetString(Resource.String.symbol_modulo) },
                {Symbol.EXP, Resources.GetString(Resource.String.symbol_exponential) },
                {Symbol.POWER, Resources.GetString(Resource.String.symbol_power) },
                {Symbol.SQR, Resources.GetString(Resource.String.symbol_sqr) },
                {Symbol.SQRT, Resources.GetString(Resource.String.symbol_sqrt) },
                {Symbol.LOGARITHM, Resources.GetString(Resource.String.symbol_logarithm) },
                {Symbol.NATURAL_LOGARITHM, Resources.GetString(Resource.String.symbol_natural_logarithm) },
                {Symbol.ANSWER, Resources.GetString(Resource.String.symbol_answer) },
                {Symbol.SINE, Resources.GetString(Resource.String.symbol_sine) },
                {Symbol.COSINE, Resources.GetString(Resource.String.symbol_cosine) },
                {Symbol.TANGENT, Resources.GetString(Resource.String.symbol_tangent) },
                {Symbol.SECANT, Resources.GetString(Resource.String.symbol_secant) },
                {Symbol.COSECANT, Resources.GetString(Resource.String.symbol_cosecant) },
                {Symbol.COTANGENT, Resources.GetString(Resource.String.symbol_cotangent) }
            };
            Converters.SymbolToString = symbol => symbolToStringDictionary[symbol];

            Dictionary<string, Symbol> stringToSymbolDictionary = new Dictionary<string, Symbol>() {
                {Resources.GetString(Resource.String.symbol_0), Symbol.ZERO },
                {Resources.GetString(Resource.String.symbol_1), Symbol.ONE },
                {Resources.GetString(Resource.String.symbol_2), Symbol.TWO },
                {Resources.GetString(Resource.String.symbol_3), Symbol.THREE},
                {Resources.GetString(Resource.String.symbol_4), Symbol.FOUR },
                {Resources.GetString(Resource.String.symbol_5), Symbol.FIVE },
                {Resources.GetString(Resource.String.symbol_6), Symbol.SIX },
                {Resources.GetString(Resource.String.symbol_7), Symbol.SEVEN },
                {Resources.GetString(Resource.String.symbol_8), Symbol.EIGHT },
                {Resources.GetString(Resource.String.symbol_9), Symbol.NINE },
                {Resources.GetString(Resource.String.symbol_point), Symbol.POINT },
                {Resources.GetString(Resource.String.symbol_left_parenthesis), Symbol.LEFT_PARENTHESIS },
                {Resources.GetString(Resource.String.symbol_right_parenthesis), Symbol.RIGHT_PARENTHESIS },
                {Resources.GetString(Resource.String.symbol_add), Symbol.ADD },
                {Resources.GetString(Resource.String.symbol_subtract), Symbol.SUBTRACT },
                {Resources.GetString(Resource.String.symbol_multiply), Symbol.MULTIPLY },
                {Resources.GetString(Resource.String.symbol_divide), Symbol.DIVIDE },
                {Resources.GetString(Resource.String.symbol_modulo), Symbol.MODULO },
                {Resources.GetString(Resource.String.symbol_exponential), Symbol.EXP },
                {Resources.GetString(Resource.String.symbol_power), Symbol.POWER },
                {Resources.GetString(Resource.String.symbol_sqr), Symbol.SQR },
                {Resources.GetString(Resource.String.symbol_sqrt), Symbol.SQRT },
                {Resources.GetString(Resource.String.symbol_logarithm), Symbol.LOGARITHM },
                {Resources.GetString(Resource.String.symbol_natural_logarithm), Symbol.NATURAL_LOGARITHM },
                {Resources.GetString(Resource.String.symbol_answer), Symbol.ANSWER },
                {Resources.GetString(Resource.String.symbol_sine), Symbol.SINE },
                {Resources.GetString(Resource.String.symbol_cosine), Symbol.COSINE },
                {Resources.GetString(Resource.String.symbol_tangent), Symbol.TANGENT },
                {Resources.GetString(Resource.String.symbol_secant), Symbol.SECANT },
                {Resources.GetString(Resource.String.symbol_cosecant), Symbol.COSECANT },
                {Resources.GetString(Resource.String.symbol_cotangent), Symbol.COTANGENT }
            };
            Converters.StringToSymbol = sourceString => stringToSymbolDictionary[sourceString];
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}