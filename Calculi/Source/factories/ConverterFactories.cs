using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Calculi.Shared;
using Calculi.Shared.Converters;

namespace Calculi
{
    internal static class ConverterFactories
    {

        internal static IConverter<IExpression, ICalculation> GetExpressionToICalculationConverter(ICalculatorIO calculatorIO)
        {
            return new IExpressionToICalculationConverter(calculatorIO);
        }
        internal static IConverter<IExpression, string> GetExpressionToStringConverter(IConverter<Symbol, string> symbolToStringConverter)
        {
            return new IExpressionToStringConverter(symbolToStringConverter);
        }
        internal static IConverter<ICalculation, IExpression> GetCalculationToExpressionConverter()
        {
            return new ICalculationToIExpressionConverter();
        }
        internal static IConverter<ICalculation, double> GetCalculationToDoubleConverter()
        {
            return new ICalculationToDoubleConverter();
        }
        internal static IConverter<Symbol, string> GetSymbolToStringConverter(Android.Content.Res.Resources res)
        {
            Dictionary<Symbol, string> dictionary = new Dictionary<Symbol, string>() {
                {Symbol.ZERO, res.GetString(Resource.String.symbol_0) },
                {Symbol.ONE, res.GetString(Resource.String.symbol_1) },
                {Symbol.TWO, res.GetString(Resource.String.symbol_2) },
                {Symbol.THREE, res.GetString(Resource.String.symbol_3) },
                {Symbol.FOUR, res.GetString(Resource.String.symbol_4) },
                {Symbol.FIVE, res.GetString(Resource.String.symbol_5) },
                {Symbol.SIX, res.GetString(Resource.String.symbol_6) },
                {Symbol.SEVEN, res.GetString(Resource.String.symbol_7) },
                {Symbol.EIGHT, res.GetString(Resource.String.symbol_8) },
                {Symbol.NINE, res.GetString(Resource.String.symbol_9) },
                {Symbol.POINT, res.GetString(Resource.String.symbol_point) },
                {Symbol.LEFT_PARENTHESIS, res.GetString(Resource.String.symbol_left_parenthesis) },
                {Symbol.RIGHT_PARENTHESIS, res.GetString(Resource.String.symbol_right_parenthesis) },
                {Symbol.ADD, res.GetString(Resource.String.symbol_add) },
                {Symbol.SUBTRACT, res.GetString(Resource.String.symbol_subtract) },
                {Symbol.MULTIPLY, res.GetString(Resource.String.symbol_multiply) },
                {Symbol.DIVIDE, res.GetString(Resource.String.symbol_divide) },
                {Symbol.MODULO, res.GetString(Resource.String.symbol_modulo) },
                {Symbol.EXP, res.GetString(Resource.String.symbol_exponential) },
                {Symbol.POWER, res.GetString(Resource.String.symbol_power) },
                {Symbol.SQR, res.GetString(Resource.String.symbol_sqr) },
                {Symbol.SQRT, res.GetString(Resource.String.symbol_sqrt) },
                {Symbol.LOGARITHM, res.GetString(Resource.String.symbol_logarithm) },
                {Symbol.NATURAL_LOGARITHM, res.GetString(Resource.String.symbol_natural_logarithm) },
                {Symbol.ANSWER, res.GetString(Resource.String.symbol_answer) },
                {Symbol.SINE, res.GetString(Resource.String.symbol_sine) },
                {Symbol.COSINE, res.GetString(Resource.String.symbol_cosine) },
                {Symbol.TANGENT, res.GetString(Resource.String.symbol_tangent) },
                {Symbol.SECANT, res.GetString(Resource.String.symbol_secant) },
                {Symbol.COSECANT, res.GetString(Resource.String.symbol_cosecant) },
                {Symbol.COTANGENT, res.GetString(Resource.String.symbol_cotangent) }
            };
            return new SymbolToStringConverter(dictionary);
        }
        internal static IConverter<string, Symbol> GetStringToSymbolConverter(Android.Content.Res.Resources res)
        {
            Dictionary<string, Symbol> dictionary = new Dictionary<string, Symbol>() {
                {res.GetString(Resource.String.symbol_0), Symbol.ZERO },
                {res.GetString(Resource.String.symbol_1), Symbol.ONE },
                {res.GetString(Resource.String.symbol_2), Symbol.TWO },
                {res.GetString(Resource.String.symbol_3), Symbol.THREE},
                {res.GetString(Resource.String.symbol_4), Symbol.FOUR },
                {res.GetString(Resource.String.symbol_5), Symbol.FIVE },
                {res.GetString(Resource.String.symbol_6), Symbol.SIX },
                {res.GetString(Resource.String.symbol_7), Symbol.SEVEN },
                {res.GetString(Resource.String.symbol_8), Symbol.EIGHT },
                {res.GetString(Resource.String.symbol_9), Symbol.NINE },
                {res.GetString(Resource.String.symbol_point), Symbol.POINT },
                {res.GetString(Resource.String.symbol_left_parenthesis), Symbol.LEFT_PARENTHESIS },
                {res.GetString(Resource.String.symbol_right_parenthesis), Symbol.RIGHT_PARENTHESIS },
                {res.GetString(Resource.String.symbol_add), Symbol.ADD },
                {res.GetString(Resource.String.symbol_subtract), Symbol.SUBTRACT },
                {res.GetString(Resource.String.symbol_multiply), Symbol.MULTIPLY },
                {res.GetString(Resource.String.symbol_divide), Symbol.DIVIDE },
                {res.GetString(Resource.String.symbol_modulo), Symbol.MODULO },
                {res.GetString(Resource.String.symbol_exponential), Symbol.EXP },
                {res.GetString(Resource.String.symbol_power), Symbol.POWER },
                {res.GetString(Resource.String.symbol_sqr), Symbol.SQR },
                {res.GetString(Resource.String.symbol_sqrt), Symbol.SQRT },
                {res.GetString(Resource.String.symbol_logarithm), Symbol.LOGARITHM },
                {res.GetString(Resource.String.symbol_natural_logarithm), Symbol.NATURAL_LOGARITHM },
                {res.GetString(Resource.String.symbol_answer), Symbol.ANSWER },
                {res.GetString(Resource.String.symbol_sine), Symbol.SINE },
                {res.GetString(Resource.String.symbol_cosine), Symbol.COSINE },
                {res.GetString(Resource.String.symbol_tangent), Symbol.TANGENT },
                {res.GetString(Resource.String.symbol_secant), Symbol.SECANT },
                {res.GetString(Resource.String.symbol_cosecant), Symbol.COSECANT },
                {res.GetString(Resource.String.symbol_cotangent), Symbol.COTANGENT }
            };
            return new StringToSymbolConverter(dictionary);
        }
    }
}