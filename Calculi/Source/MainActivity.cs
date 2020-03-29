using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;

using Calculi.Shared;

using Android.Support.V7.Widget;
using Android.Views;

using PopupMenu = Android.Support.V7.Widget.PopupMenu;

using System.Linq;
using System;

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

        TextView inputPreview;
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
            return calculatorIO.Expression
                .Take(calculatorIO.Position)
                .Select(s => symbolToStringConverter.Convert(s).Length)
                .Sum();
        }
        public void SetIndexFromViewPosition(int viewIndex)
        {
            int i = 0;
            while (calculatorIO.Expression.Take(i).Select(s => symbolToStringConverter.Convert(s).Length).Sum() < viewIndex)
            {
                ++i;
            }
            calculatorIO.Position = i;
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
            input.Text = this.expressionToStringConverter.Convert(io.Expression);
            input.RequestFocus();
            input.SetSelection(GetIndexForEditView());
            historyLayoutManager.ScrollToPosition(io.History.Count-1);
            UpdateInputPreviewView(io);
        }
        private void UpdateInputPreviewView(ICalculatorIO io)
        {
            if (io.Expression.Count == 0) {
                inputPreview.Text = "";
            } else
            {
                CalculationResult result = ExpressionToCalculation(io.Expression);
                inputPreview.Text = result.Calculation != null ? result.Calculation.ToDouble().ToString() : inputPreview.Text;
            }
        }
        private void BindUIButtons()
        {
            inputPreview = FindViewById<TextView>(Resource.Id.InputPreview);
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
                if (calculatorIO.Expression.Count == 0)
                {
                    calculatorIO.InsertSymbol(Symbol.ANSWER);
                }
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
                UpdateInputView(calculatorIO);
            };

            buttonNaturalLogarithm.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.NATURAL_LOGARITHM);
                UpdateInputView(calculatorIO);
            };

            buttonExp.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.EXP);
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
                UpdateInputView(calculatorIO);
            };

            buttonCosine.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.COSINE);
                UpdateInputView(calculatorIO);
            };

            buttonTangent.Click += (sender, e) =>
            {
                calculatorIO.InsertSymbol(Symbol.TANGENT);
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
                calculatorIO.DecrementPosition();
                UpdateInputView(calculatorIO);
            };

            buttonRightArrow.Click += (sender, e) =>
            {
                calculatorIO.IncrementPosition();
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
                ICalculation calculation = ExpressionToCalculation(calculatorIO.Expression).Calculation;
                if (calculation != null)
                {
                    historyAdapter.NotifyItemInserted(0);
                    calculatorIO.MoveInputToHistory(calculation);
                    UpdateInputView(calculatorIO);
                }
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
        }
        private CalculationResult ExpressionToCalculation (IExpression expression)
        {
            try
            {
                return new CalculationResult(expression, this.expressionToCalculationConverter.Convert(expression));
            }
            catch(Exception e)
            {
                return new CalculationResult(expression, null);
            }
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
                View historyEntry = historyLayoutManager.FindViewByPosition(position);
                PopupMenu popupmenu = new PopupMenu(this, historyEntry);

                popupmenu.MenuItemClick += (sender2, e) =>
                {
                    switch (e.Item.ItemId)
                    {
                        case Resource.Id.historyGetResult:
                            try
                            {
                                calculatorIO.History[position].Calculation.CollapseToExpression().ToList().ForEach(s => calculatorIO.InsertSymbol(s));
                            }
                            catch (Exception excptn)
                            {

                            }
                            UpdateInputView(calculatorIO);
                            break;
                        case Resource.Id.historyGetExpression:
                            calculatorIO.History[position].Expression.ToList().ForEach(s => calculatorIO.InsertSymbol(s));
                            UpdateInputView(calculatorIO);
                            break;
                        default:
                            break;
                    }
                };

                popupmenu.Inflate(Resource.Layout.history_click);
                popupmenu.Gravity = (int)GravityFlags.Right;
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