﻿using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Views.InputMethods;

using System;
using System.Collections.Generic;

using Calculi.Shared;
using Calculi.Shared.Converters;
using Calculi.Shared.Utilities;

using Android.Support.V7.Widget;
using Android.Views;

using PopupMenu = Android.Support.V7.Widget.PopupMenu;

using System.Linq;

/* TODO
 * 
 * Refactor into more readable code, more files?
 * Refactor on-click bindings to methods
 * Refactor computation to happen only ever through one main-activity method, implement exception handling
 * Implement config for calculator class
 * Refactor layout file names
 */


/*
TODO:
    * Add "sneak peak" calculation area
    * Format ui history elements properly and make them clickable
    * Swipe for constants/functions
    * Upload to google app store
    * Dark mode / other themes
    * Landscape mode?
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
        CalculatorIO calculatorIO;
        IConverter<Symbol, string> symbolToStringConverter;
        IConverter<string, Symbol> stringToSymbolConverter;
        IConverter<IExpression, string> expressionToStringConverter;
        IConverter<IExpression, ICalculation> expressionToCalculationConverter;
        IConverter<ICalculation, double> calculationToDoubleConverter;
        IConverter<ICalculation, IExpression> calculationToExpressionConverter;

        RecyclerView historyRecyclerView;
        RecyclerView.LayoutManager historyLayoutManager;
        CalculatorIOHistoryAdapter historyAdapter;
        
        
        EditText input;
        FrameLayout inputFrameLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            calculatorIO = new Shared.CalculatorIO();
            this.symbolToStringConverter = Factory.GetSymbolToStringConverter(this.Resources);
            this.stringToSymbolConverter = Factory.GetStringToSymbolConverter(this.Resources);
            this.expressionToStringConverter = Factory.GetExpressionToStringConverter(symbolToStringConverter);
            this.expressionToCalculationConverter = Factory.GetExpressionToICalculationConverter(calculatorIO);
            this.calculationToDoubleConverter = Factory.GetCalculationToDoubleConverter();
            this.calculationToExpressionConverter = Factory.GetCalculationToExpressionConverter();

            historyRecyclerView = FindViewById<RecyclerView>(Resource.Id.HistoryRecyclerView);
            historyLayoutManager = new LinearLayoutManager(this)
            {
                StackFromEnd = true,
                ReverseLayout = false
            };
            historyAdapter = new CalculatorIOHistoryAdapter(
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
                            double result = calculationToDoubleConverter.Convert(expressionToCalculationConverter.Convert(calculatorIO.GetHistory(position)));
                            result.ToString().ToList().Select(s => stringToSymbolConverter.Convert(s.ToString())).ToList().ForEach(s => calculatorIO.InsertSymbol(s));
                            UpdateInputView(calculatorIO);
                            break;
                        case Resource.Id.historyGetExpression:
                            calculatorIO
                                .GetHistory(position).ToList()
                                .ForEach(s => calculatorIO.InsertSymbol(s));
                            UpdateInputView(calculatorIO);
                            break;
                        default:
                            break;
                    }
                };

                popupmenu.Show();
            };




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
                calculatorIO.AddInputToHistory();
                calculatorIO.ClearInput();
                UpdateInputView(calculatorIO);
            };

            UpdateInputView(calculatorIO);

        }
        public int GetIndexForEditView()
        {
            return calculatorIO.currentExpression
                .Take(calculatorIO.index)
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
            calculatorIO.index = i;
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

        private void UpdateInputView(CalculatorIO io)
        {
            input.Text = this.expressionToStringConverter.Convert(io.currentExpression);
            input.RequestFocus();
            input.SetSelection(GetIndexForEditView());
            historyLayoutManager.ScrollToPosition(io.GetHistory().Count-1);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class CalculatorIOHistoryAdapter : RecyclerView.Adapter
    {
        IConverter<ICalculation, double> calculationToDoubleConverter;
        IConverter<IExpression, ICalculation> expressionToICalculationConverter;
        IConverter<IExpression, string> expressionToStringConverter;
        public event EventHandler<int> ItemClick;
        private CalculatorIO calculator;
        public override int ItemCount
        {
            get { return calculator.GetHistory().Count; }
        }
        void OnClick(int position)
        {
                ItemClick(this, position);
        }
        internal CalculatorIOHistoryAdapter(
                CalculatorIO calculator,
                IConverter<ICalculation, double> calculationToDoubleConverter,
                IConverter<IExpression, ICalculation> expressionToICalculationConverter,
                IConverter<IExpression, string> expressionToStringConverter
            )
        {
            this.calculationToDoubleConverter = calculationToDoubleConverter;
            this.expressionToICalculationConverter = expressionToICalculationConverter;
            this.expressionToStringConverter = expressionToStringConverter;

            this.calculator = calculator;
            calculator.BindToHistoryChange((sender, e) => {
                this.NotifyItemRangeRemoved(0, 1);
                this.NotifyItemInserted(calculator.GetHistory().Count-1);
            });
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            CalculationHistoryViewHolder vh = holder as CalculationHistoryViewHolder;
            IExpression expr = calculator.GetHistory(position);
            ICalculation calc = expressionToICalculationConverter.Convert(expr);
            double result = calculationToDoubleConverter.Convert(calc);
            vh.calculationResult.Text = result.ToString();
            vh.calculationExpression.Text = expressionToStringConverter.Convert(calculator.GetHistory(position));
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the CardView for the photo:
            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Layout.history_entry, parent, false);

            // Create a ViewHolder to hold view references inside the CardView:
            CalculationHistoryViewHolder vh = new CalculationHistoryViewHolder(itemView, OnClick);
            return vh;
        }
    }
    public class CalculationHistoryViewHolder : RecyclerView.ViewHolder
    {
        public TextView calculationResult { get; private set; }
        public TextView calculationExpression { get; private set; }

        public CalculationHistoryViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            // Locate and cache view references:
            calculationResult = itemView.FindViewById<TextView>(Resource.Id.calculationResultTextView);
            calculationExpression = itemView.FindViewById<TextView>(Resource.Id.calculationExpressionTextView);
            itemView.Click += (sender, e) => listener.Invoke(base.LayoutPosition);
        }
    }

}