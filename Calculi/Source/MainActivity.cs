using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views.InputMethods;
using System.Collections.Generic;

using Calculi.Shared;
using Calculi.Shared.Converters;
using Calculi.Shared.Utilities;

using Android.Support.V7.Widget;
using Android.Views;

using PopupMenu = Android.Support.V7.Widget.PopupMenu;

using System.Linq;
using System;

/* TODO
 * Fix ANS button
 * Rework how large numbers work; introduce an "exponent" operator E, e.g. "1.2E16"
 * Write/fix tests
 */

/*
TODO:
    * Revamp design and add "sneak peak" calculation area
    * Swipe for constants/functions
    * Dark mode / other themes / transparent?
    * Landscape mode?
    * Make homescreen logo
    * Upload to google app store
*/

namespace Calculi
{
    [Activity(
        Label = "@string/app_name",
        Theme = "@style/AppTheme",
        MainLauncher = true,
        WindowSoftInputMode = SoftInput.StateAlwaysHidden
    )]

    
    public class MainActivity : AppCompatActivity
    {
        ICalculatorIO calculatorIO;
        IConverter<Symbol, string> symbolToStringConverter;
        IConverter<string, Symbol> stringToSymbolConverter;
        IConverter<string, IExpression> stringToExpressionConverter;
        IConverter<IExpression, string> expressionToStringConverter;
        IConverter<IExpression, ICalculation> expressionToCalculationConverter;
        IConverter<ICalculation, double> calculationToDoubleConverter;
        IConverter<ICalculation, IExpression> calculationToExpressionConverter;

        RecyclerView historyRecyclerView;
        RecyclerView.LayoutManager historyLayoutManager;
        CalculationHistoryAdapter historyAdapter;
        
        EditText input;
        FrameLayout inputFrameLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            InitiateCalculatorIO();
            InitiateHistoryDropdownFunctionality();
            BindUIButtons();
            UpdateInputView(calculatorIO);

        }
        public int GetIndexForEditView()
        {
            return calculatorIO.currentExpression
                .Take(calculatorIO.position)
                .Select(s => symbolToStringConverter.Convert(s).Length)
                .Sum();
        }
        public void SetIndexFromViewPosition(int viewIndex)
        {
            int i = 0;
            while (calculatorIO.currentExpression.Take(i).Select(s => symbolToStringConverter.Convert(s).Length).Sum() < viewIndex)
            {
                ++i;
            }
            calculatorIO.position = i;
        }
        private void SetSelectionFromTouchEvent(Android.Views.View.TouchEventArgs e)
        {
            float x = e.Event.RawX;
            float y = e.Event.RawY;
            Android.Text.Layout layout = input.Layout;
            int offset = layout.GetOffsetForHorizontal(0, x);
            if (offset >= 0)
            {
                SetIndexFromViewPosition(offset);
                UpdateInputView(calculatorIO);
            }
        }
        private void UpdateInputView(ICalculatorIO io)
        {
            input.Text = this.expressionToStringConverter.Convert(io.currentExpression);
            input.RequestFocus();
            input.SetSelection(GetIndexForEditView());
            historyLayoutManager.ScrollToPosition(io.GetHistory().Count-1);
        }
        private void BindUIButtons()
        {

            input = FindViewById<EditText>(Resource.Id.Input);
            inputFrameLayout = FindViewById<FrameLayout>(Resource.Id.InputFrameLayout);

            Button buttonEnter = FindViewById<Button>(Resource.Id.buttonEnter);
            Button buttonZero = FindViewById<Button>(Resource.Id.button0);
            Button buttonOne = FindViewById<Button>(Resource.Id.button1);
            Button buttonTwo = FindViewById<Button>(Resource.Id.button2);
            Button buttonThree = FindViewById<Button>(Resource.Id.button3);
            Button buttonFour = FindViewById<Button>(Resource.Id.button4);
            Button buttonFive = FindViewById<Button>(Resource.Id.button5);
            Button buttonSix = FindViewById<Button>(Resource.Id.button6);
            Button buttonSeven = FindViewById<Button>(Resource.Id.button7);
            Button buttonEight = FindViewById<Button>(Resource.Id.button8);
            Button buttonNine = FindViewById<Button>(Resource.Id.button9);
            Button buttonPoint = FindViewById<Button>(Resource.Id.buttonPoint);
            Button buttonAdd = FindViewById<Button>(Resource.Id.buttonAdd);
            Button buttonSubtract = FindViewById<Button>(Resource.Id.buttonSubtract);
            Button buttonMultiply = FindViewById<Button>(Resource.Id.buttonMultiply);
            Button buttonDivide = FindViewById<Button>(Resource.Id.buttonDivide);
            Button buttonSqr = FindViewById<Button>(Resource.Id.buttonSquare);
            Button buttonMod = FindViewById<Button>(Resource.Id.buttonModulo);
            Button buttonSqrt = FindViewById<Button>(Resource.Id.buttonSquareRoot);
            Button buttonLogarithm = FindViewById<Button>(Resource.Id.buttonLog);
            Button buttonNaturalLogarithm = FindViewById<Button>(Resource.Id.buttonLn);
            Button buttonExp = FindViewById<Button>(Resource.Id.buttonExponential);
            Button buttonPower = FindViewById<Button>(Resource.Id.buttonPower);
            Button buttonSine = FindViewById<Button>(Resource.Id.buttonSine);
            Button buttonCosine = FindViewById<Button>(Resource.Id.buttonCosine);
            Button buttonTangent = FindViewById<Button>(Resource.Id.buttonTangent);
            Button buttonAns = FindViewById<Button>(Resource.Id.buttonAnswer);
            Button buttonLeftParenthesis = FindViewById<Button>(Resource.Id.buttonLeftParenthesis);
            Button buttonRightParenthesis = FindViewById<Button>(Resource.Id.buttonRightParenthesis);
            Button buttonLeftArrow = FindViewById<Button>(Resource.Id.buttonLeftArrow);
            Button buttonRightArrow = FindViewById<Button>(Resource.Id.buttonRightArrow);
            Button buttonDelete = FindViewById<Button>(Resource.Id.buttonDelete);
            Button buttonClear = FindViewById<Button>(Resource.Id.buttonClear);


            input.Touch += (sender, e) =>
            {
                SetSelectionFromTouchEvent(e);
            };

            inputFrameLayout.Touch += (sender, e) =>
            {
                SetSelectionFromTouchEvent(e);
            };

            buttonZero.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.ZERO);
                UpdateInputView(calculatorIO);
            };

            buttonOne.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.ONE);
                UpdateInputView(calculatorIO);
            };

            buttonTwo.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.TWO);
                UpdateInputView(calculatorIO);
            };

            buttonThree.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.THREE);
                UpdateInputView(calculatorIO);
            };

            buttonFour.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.FOUR);
                UpdateInputView(calculatorIO);
            };

            buttonFive.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.FIVE);
                UpdateInputView(calculatorIO);
            };

            buttonSix.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.SIX);
                UpdateInputView(calculatorIO);
            };

            buttonSeven.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.SEVEN);
                UpdateInputView(calculatorIO);
            };

            buttonEight.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.EIGHT);
                UpdateInputView(calculatorIO);
            };

            buttonNine.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.NINE);
                UpdateInputView(calculatorIO);
            };

            buttonAdd.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.ADD);
                UpdateInputView(calculatorIO);
            };

            buttonSubtract.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.SUBTRACT);
                UpdateInputView(calculatorIO);
            };

            buttonMultiply.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.MULTIPLY);
                UpdateInputView(calculatorIO);
            };

            buttonDivide.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.DIVIDE);
                UpdateInputView(calculatorIO);
            };

            buttonPoint.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.POINT);
                UpdateInputView(calculatorIO);
            };

            buttonSqrt.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.SQRT);
                calculatorIO.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
                calculatorIO.DecrementIndex();
                UpdateInputView(calculatorIO);
            };

            buttonSqr.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.SQR);
                UpdateInputView(calculatorIO);
            };

            buttonLogarithm.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.LOGARITHM);
                calculatorIO.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
                calculatorIO.DecrementIndex();
                UpdateInputView(calculatorIO);
            };

            buttonNaturalLogarithm.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.NATURAL_LOGARITHM);
                calculatorIO.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
                calculatorIO.DecrementIndex();
                UpdateInputView(calculatorIO);
            };

            buttonExp.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.EXP);
                calculatorIO.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
                calculatorIO.DecrementIndex();
                UpdateInputView(calculatorIO);
            };

            buttonPower.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.POWER);
                UpdateInputView(calculatorIO);
            };

            buttonSine.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.SINE);
                calculatorIO.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
                calculatorIO.DecrementIndex();
                UpdateInputView(calculatorIO);
            };

            buttonCosine.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.COSINE);
                calculatorIO.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
                calculatorIO.DecrementIndex();
                UpdateInputView(calculatorIO);
            };

            buttonTangent.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.TANGENT);
                calculatorIO.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
                calculatorIO.DecrementIndex();
                UpdateInputView(calculatorIO);
            };

            buttonMod.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.MODULO);
                UpdateInputView(calculatorIO);
            };

            buttonAns.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.ANSWER);
                UpdateInputView(calculatorIO);
            };

            buttonLeftParenthesis.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.LEFT_PARENTHESIS);
                UpdateInputView(calculatorIO);
            };

            buttonRightParenthesis.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.RIGHT_PARENTHESIS);
                UpdateInputView(calculatorIO);
            };

            buttonLeftArrow.Click += (sender, e) =>
            {
                calculatorIO.DecrementIndex();
                UpdateInputView(calculatorIO);
            };

            buttonRightArrow.Click += (sender, e) =>
            {
                calculatorIO.IncrementIndex();
                UpdateInputView(calculatorIO);
            };

            buttonDelete.Click += (sender, e) =>
            {
                calculatorIO.RemoveSymbol();
                UpdateInputView(calculatorIO);
            };

            buttonClear.Click += (sender, e) =>
            {
                calculatorIO.ClearInput();
                UpdateInputView(calculatorIO);
            };

            buttonEnter.Click += (sender, e) =>
            {
                historyAdapter.NotifyItemInserted(0);
                calculatorIO.MoveInputToHistory();
                UpdateInputView(calculatorIO);
            };

        }
        private void InitiateCalculatorIO()
        {
            calculatorIO = new Shared.CalculatorIO();
            this.symbolToStringConverter = ConverterFactory.GetSymbolToStringConverter(this.Resources);
            this.stringToSymbolConverter = ConverterFactory.GetStringToSymbolConverter(this.Resources);
            this.stringToExpressionConverter = ConverterFactory.GetStringToExpressionConverter(this.stringToSymbolConverter);
            this.expressionToStringConverter = ConverterFactory.GetExpressionToStringConverter(symbolToStringConverter);
            this.expressionToCalculationConverter = ConverterFactory.GetExpressionToICalculationConverter(calculatorIO);
            this.calculationToDoubleConverter = ConverterFactory.GetICalculationToDoubleConverter();
            this.calculationToExpressionConverter = ConverterFactory.GetICalculationToExpressionConverter();
        }
        private void InitiateHistoryDropdownFunctionality()
        {
            historyRecyclerView = FindViewById<RecyclerView>(Resource.Id.HistoryRecyclerView);
            historyLayoutManager = new LinearLayoutManager(this)
            {
                StackFromEnd = true,
                ReverseLayout = false
            };
            historyAdapter = new CalculationHistoryAdapter(
                    calculatorIO,
                    calculationToDoubleConverter,
                    expressionToCalculationConverter,
                    expressionToStringConverter
                );

            historyRecyclerView.SetAdapter(historyAdapter);
            historyRecyclerView.SetLayoutManager(historyLayoutManager);

            historyAdapter.ItemClick += (sender, position) =>
            {

                View historyEntry = historyLayoutManager.GetChildAt(position);
                PopupMenu popupmenu = new PopupMenu(this, historyEntry);
                popupmenu.Inflate(Resource.Layout.history_click);
                popupmenu.Gravity = (int)GravityFlags.Right;

                popupmenu.MenuItemClick += (sender2, e) =>
                {
                    switch (e.Item.ItemId)
                    {
                        case Resource.Id.historyGetResult:
                            try
                            {
                                double result = calculationToDoubleConverter.Convert(expressionToCalculationConverter.Convert(calculatorIO.GetHistory(position)));
                                stringToExpressionConverter.Convert(result.ToString()).ToList().ForEach(s => calculatorIO.InsertSymbol(s));
                            }
                            catch (Exception excptn)
                            {

                            }
                            UpdateInputView(calculatorIO);
                            break;
                        case Resource.Id.historyGetExpression:
                            calculatorIO.GetHistory(position).ToList().ForEach(s => calculatorIO.InsertSymbol(s));
                            UpdateInputView(calculatorIO);
                            break;
                        default:
                            break;
                    }
                };

                popupmenu.Show();
            };
        }


        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        //{
        //    Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        //    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}
    }

}