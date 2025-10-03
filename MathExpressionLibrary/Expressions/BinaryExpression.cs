using MathExpressionLibrary.Tokenization;

namespace MathExpressionLibrary.Expressions
{
    public sealed class BinaryExpression : IExpression
    {
        public BinaryExpression(IExpression leftExpression, IExpression rightExpression, Token token)
        {
            LeftExpression = leftExpression;
            RightExpression = rightExpression;
            Token = token;
        }

        public IExpression LeftExpression { get; private set; }

        public IExpression RightExpression { get; private set; }

        public Token Token { get; }

        public int CompareTo(IExpression? other)
        {
            return Expression.Compare(this, other);
        }

        public object? Evaluate()
        {
            object? leftResult = LeftExpression.Evaluate();
            object? rightResult = RightExpression.Evaluate();

            return Token.TokenOperator switch
            {
                TokenOperator.Add => ToDouble(leftResult) + ToDouble(rightResult),
                TokenOperator.Div => ToDouble(leftResult) / ToDouble(rightResult),
                TokenOperator.EQ => leftResult is not null ? leftResult.Equals(rightResult) : rightResult is not null ? rightResult.Equals(leftResult) : true,
                TokenOperator.GE => ToDouble(leftResult) >= ToDouble(rightResult),
                TokenOperator.GT => ToDouble(leftResult) > ToDouble(rightResult),
                TokenOperator.LE => ToDouble(leftResult) <= ToDouble(rightResult),
                TokenOperator.LT => ToDouble(leftResult) < ToDouble(rightResult),
                TokenOperator.Mod => ToDouble(leftResult) % ToDouble(rightResult),
                TokenOperator.Mul => ToDouble(leftResult) * ToDouble(rightResult),
                TokenOperator.NE => leftResult is not null ? !leftResult.Equals(rightResult) : rightResult is not null ? !rightResult.Equals(leftResult) : false,
                TokenOperator.Power => Math.Pow(ToDouble(leftResult), ToDouble(rightResult)),
                TokenOperator.Sub => ToDouble(leftResult) - ToDouble(rightResult),
                _ => throw new NotSupportedException()
            };
        }

        public IExpression Optimize()
        {
            IExpression left = LeftExpression.Optimize();
            IExpression right = RightExpression.Optimize();

            LeftExpression = left;
            RightExpression = right;
            if (left is AtomicExpression && right is AtomicExpression)
            {
                return new AtomicExpression(Token, Evaluate());
            }

            return this;
        }

        public override string ToString()
        {
            return $"{{Binary {LeftExpression.ToString()} {Token.TokenOperator} {RightExpression.ToString()}}}";
        }

        private double ToDouble(object? value)
        {
            if (value is double d)
            {
                return d;
            }

            return Convert.ToDouble(value);
        }
    }
}