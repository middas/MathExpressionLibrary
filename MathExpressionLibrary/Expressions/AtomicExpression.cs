using MathExpressionLibrary.Tokenization;

namespace MathExpressionLibrary.Expressions
{
    public sealed class AtomicExpression : IExpression
    {
        private readonly object? value;

        public AtomicExpression(Token token) : this(token, token.Value)
        {
        }

        public AtomicExpression(Token token, object? value)
        {
            Token = token;
            this.value = value;
        }

        public Token Token { get; }

        public int CompareTo(IExpression? other)
        {
            return Expression.Compare(this, other);
        }

        public object? Evaluate()
        {
            return value;
        }

        public IExpression Optimize()
        {
            return this;
        }

        public override string ToString()
        {
            return $"{{Atomic {value ?? "NULL"}}}";
        }
    }
}