using MathExpressionLibrary.Exceptions;

namespace MathExpressionLibrary.Expressions
{
    public sealed class VariableExpression : IExpression
    {
        private readonly string name;
        private readonly Func<string, object?> varLookup;

        public VariableExpression(string name, Func<string, object?> varLookup)
        {
            this.name = name;
            this.varLookup = varLookup;
        }

        public int CompareTo(IExpression? other)
        {
            return Expression.Compare(this, other);
        }

        public object? Evaluate()
        {
            try
            {
                return varLookup(name);
            }
            catch (KeyNotFoundException)
            {
                throw new ExpressionException($"Variable {name} does not exist.");
            }
        }

        public IExpression Optimize()
        {
            return this;
        }
    }
}