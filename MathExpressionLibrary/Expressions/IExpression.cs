namespace MathExpressionLibrary.Expressions
{
    public interface IExpression : IComparable<IExpression>
    {
        object? Evaluate();

        IExpression Optimize();
    }
}