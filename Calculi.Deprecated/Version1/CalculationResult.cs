namespace Calculi.Shared.Deprecated.Version1
{
    public class CalculationResult
    {
        public IExpression Expression { get; set; }
        public ICalculation Calculation { get; set; }
        public CalculationResult(IExpression expression, ICalculation result)
        {
            this.Expression = expression;
            this.Calculation = result;
        }
    }
}
