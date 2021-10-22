using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Calculi.Android2.Fragments;
using Calculi.Literal.Errors;
using Calculi.Literal;
using Calculi.Literal.Extensions;
using Calculi.Literal.Types;
using Calculi.Support;

namespace Calculi.Android2
{
    [Activity(
        Label = "@string/app_name",
        Theme = "@style/AppTheme",
        MainLauncher = true,
        WindowSoftInputMode = SoftInput.StateAlwaysHidden

    )]
    public class MainActivity : AppCompatActivity
    {
        private readonly List<Subscription<Calculator>> _subscriptions = new List<Subscription<Calculator>>();

        public readonly Observable<Calculator> Calculator = new Observable<Calculator>(new Calculator());

        private OutputPreviewFragment _outputPreviewFragment;
        private KeypadFragment _keypadFragment;
        private KeypadArithmeticFragment _keypadArithmeticFragment;
        private KeypadAdvancedFragment _keypadAdvancedFragment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.activity_main);

            SetFragmentViews();
            SetSymbolStringConverters();
            SetStateListeners();
            SetUIBindings();
            SetFocus();
        }

        private void SetFocus()
        {
            EditText outputView = (EditText)FindViewById(Resource.Id.outputText);
            outputView.ShowSoftInputOnFocus = false;
            InputMethodManager inputMethodManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
            outputView.RequestFocus();
        }

        private void SetStateListeners()
        {
            Calculator.Subscribe(calculator =>
            {
                _outputPreviewFragment.Expression.Next(calculator.Expression);
            });
        }

        private void SetUIBindings()
        {
            Action<Symbol> insertSymbol = symbol => { Calculator.Next(Calculator.Value.InsertSymbol(symbol)); };

            _keypadFragment.OnSymbolClick += symbol => insertSymbol(symbol);
            _keypadArithmeticFragment.OnSymbolClick += symbol => insertSymbol(symbol);
            _keypadAdvancedFragment.OnSymbolClick += symbol => insertSymbol(symbol);
            _keypadArithmeticFragment.OnClearClick += () => Calculator.Next(Calculator.Value.ClearExpression());
            _keypadArithmeticFragment.OnDeleteClick += () => Calculator.Next(Calculator.Value.RemoveSymbol());

            _keypadArithmeticFragment.OnEnterClick += () =>
            {
                Calculator.Value.MoveInputToHistory().Match(
                    left: e =>
                    {
                        if (e is UserMessageException)
                        {
                            // show user message in pop-up notification
                        }
                    },
                    right: calculator => { Calculator.Next(calculator); }
                );
            };
        }

        private void SetFragmentViews()
        {
            _outputPreviewFragment = (OutputPreviewFragment)SupportFragmentManager.FindFragmentById(Resource.Id.outputPreviewFragment);
            _keypadFragment = (KeypadFragment)SupportFragmentManager.FindFragmentById(Resource.Id.keypadFragment);
            _keypadArithmeticFragment = (KeypadArithmeticFragment)SupportFragmentManager.FindFragmentById(Resource.Id.keypadArithmeticFragment);
            _keypadAdvancedFragment = (KeypadAdvancedFragment)SupportFragmentManager.FindFragmentById(Resource.Id.keypadAdvancedFragment);
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
                {Symbol.COTANGENT, Resources.GetString(Resource.String.symbol_cotangent) },
                {Symbol.PI, Resources.GetString(Resource.String.symbol_pi) },
                {Symbol.EULER_CONSTANT, Resources.GetString(Resource.String.symbol_euler_constant) }
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

        protected override void OnDestroy()
        {
            _subscriptions.ForEach(sub => sub.Unsubscribe());
        }
    }
}