using MathExpressionLibrary.Tokenization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressionLibrary.Expressions
{
    public sealed class UnaryExpression : IExpression
    {
        public UnaryExpression(IExpression expression, Token token)
        {
            Expression = expression;
            Token = token;
        }

        public IExpression Expression { get; }

        public Token Token { get; }

        public int CompareTo(IExpression? other)
        {
            return Expressions.Expression.Compare(this, other);
        }

        public object? Evaluate()
        {
            return Token.TokenOperator switch
            {
                TokenOperator.Add => +EvaluateAsDouble(),
                TokenOperator.Sub => -EvaluateAsDouble(),
                TokenOperator.Factorial => Enumerable.Range(1, Math.Max(1, Convert.ToInt32(Expression.Evaluate() ?? 0))).Aggregate(1L, (x, y) => x * y),
                _ => throw new NotSupportedException()
            };
        }

        public IExpression Optimize()
        {
            return new AtomicExpression(Token, Evaluate());
        }

        public override string ToString()
        {
            return $"{{Unary {Token.TokenOperator} {Expression.ToString()}}}";
        }

        private double EvaluateAsDouble()
        {
            object? value = Expression.Evaluate();

            if (value is double d)
            {
                return d;
            }

            return 0;
        }
    }
}