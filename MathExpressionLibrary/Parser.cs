﻿using MathExpressionLibrary.Expressions;
using MathExpressionLibrary.Tokenization;

namespace MathExpressionLibrary
{
    public sealed class Parser
    {
        private Token? lastToken;

        public IExpression ParseExpression(string expression)
        {
            Tokenizer tokenizer = new(expression);

            lastToken = tokenizer.GetToken();

            IExpression result = ParseCompare(tokenizer);

            if (lastToken.TokenOperator != TokenOperator.End)
            {
                throw new NotSupportedException();
            }

            return result.Optimize();
        }

        private IExpression ParseCompare(Tokenizer tokenizer)
        {
            IExpression expression = ParseAddSub(tokenizer);

            while (lastToken!.TokenType == TokenType.Compare)
            {
                Token token = lastToken;
                lastToken = tokenizer.GetToken();
                IExpression rightExpression = ParseAddSub(tokenizer);
                expression = new BinaryExpression(expression, rightExpression, token);
            }

            return expression;
        }

        private IExpression ParseAddSub(Tokenizer tokenizer)
        {
            IExpression expression = ParseMulDiv(tokenizer);

            while (lastToken!.TokenType == TokenType.AddSub)
            {
                Token token = lastToken;
                lastToken = tokenizer.GetToken();
                IExpression rightExpression = ParseMulDiv(tokenizer);
                expression = new BinaryExpression(expression, rightExpression, token);
            }

            return expression;
        }

        private IExpression ParseMulDiv(Tokenizer tokenizer)
        {
            IExpression expression = ParsePower(tokenizer);

            while (lastToken!.TokenType == TokenType.MulDiv)
            {
                Token token = lastToken;
                lastToken = tokenizer.GetToken();
                IExpression rightExpression = ParsePower(tokenizer);
                expression = new BinaryExpression(expression, rightExpression, token);
            }

            return expression;
        }

        private IExpression ParsePower(Tokenizer tokenizer)
        {
            IExpression expression = ParseUnary(tokenizer);

            while (lastToken!.TokenType == TokenType.Power)
            {
                Token token = lastToken;
                lastToken = tokenizer.GetToken();
                IExpression rightExpression = ParseUnary(tokenizer);
                expression = new BinaryExpression(expression, rightExpression, token);
            }

            return expression;
        }

        private IExpression ParseUnary(Tokenizer tokenizer)
        {
            if (lastToken!.TokenOperator == TokenOperator.Add || lastToken.TokenOperator == TokenOperator.Sub)
            {
                Token token = lastToken;
                lastToken = tokenizer.GetToken();
                IExpression valueExpression = ParseAtomic(tokenizer);
                return new UnaryExpression(valueExpression, token);
            }

            IExpression result = ParseAtomic(tokenizer);

            if (lastToken.TokenOperator == TokenOperator.Factorial)
            {
                Token token = lastToken;
                lastToken = tokenizer.GetToken();
                return new UnaryExpression(result, token);
            }

            return result;
        }

        private IExpression ParseAtomic(Tokenizer tokenizer)
        {
            IExpression expression;
            switch (lastToken!.TokenType)
            {
                case TokenType.Group:
                    if (lastToken.TokenOperator != TokenOperator.Open)
                    {
                        // throw
                    }

                    lastToken = tokenizer.GetToken();

                    if (lastToken.TokenOperator != TokenOperator.Close)
                    {
                        expression = ParseCompare(tokenizer);
                    }
                    else
                    {
                        return new AtomicExpression(lastToken, null);
                    }

                    if (lastToken.TokenOperator != TokenOperator.Close)
                    {
                        // throw
                    }
                    break;
                case TokenType.Identifier:
                    throw new NotImplementedException();
                    break;
                case TokenType.Literal:
                    expression = new AtomicExpression(lastToken);
                    break;
                default:
                    throw new NotSupportedException();
                    break;
            }

            lastToken = tokenizer.GetToken();

            return expression;
        }
    }
}
