using System.Linq;
using System.Text;
using Calculi.Shared.Deprecated.Version1.Extensions;

namespace Calculi.Shared.Deprecated.Version1.Converters
{
    internal class StringToIExpressionConverter : IConverter<string, IExpression>
    {
        private IConverter<string, Symbol> stringToSymbolConverter;
        public StringToIExpressionConverter(IConverter<string, Symbol> stringToSymbolConverter)
        {
            this.stringToSymbolConverter = stringToSymbolConverter;
        }
        public IExpression Convert(string source_string)
        {
            IExpression expr = new Expression();
            string extraZeroes = "";
            string baseValue = source_string.ToList().TakeWhile(c => c != 'E').ToList().Aggregate("", (result, c) => result + c);
            if (source_string.ToList().Exists(c => c == 'E'))
            {
                double exponent = System.Convert.ToDouble(source_string.SkipWhile(c => c != 'E').Skip(2).ToList().Aggregate("", (result, c) => result + c));
                double numDecimals = baseValue.SkipWhile(c => c != '.').Skip(1).Count();
                double numZeroes = exponent - numDecimals;
                for (int i = 0; i < numZeroes; i++)
                {
                    extraZeroes += "0";
                }
                baseValue = baseValue.Split(".").Aggregate("", (result, c) => result + c);
            }
            return (baseValue + extraZeroes).ToList().Select(s => stringToSymbolConverter.Convert(s.ToString())).ToExpression();
        }
    }
}
