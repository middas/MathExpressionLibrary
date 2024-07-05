using MathExpressionLibrary.Exceptions;
using MathExpressionLibrary.Expressions;
using MathExpressionLibrary.Functions;
using MathExpressionLibrary.Tokenization;

namespace MathExpressionLibrary
{
    public sealed class Parser
    {
        private Token? currentToken;
        private Token? lastToken;

        public Parser()
        {
            MathAndTrigFunctions.RegisterFunctions();
        }

        public IExpression ParseExpression(string expression)
        {
            Tokenizer tokenizer = new(expression);

            lastToken = currentToken;
            currentToken = tokenizer.GetToken();

            IExpression result = ParseCompare(tokenizer);

            if (currentToken.TokenOperator != TokenOperator.End)
            {
                throw new ExpressionException(currentToken.StartPointer, "The expression parsing terminated before reaching the end of the expression.");
            }

            return result;
        }

        private IExpression ParseCompare(Tokenizer tokenizer)
        {
            IExpression expression = ParseAddSub(tokenizer);

            while (currentToken!.TokenType == TokenType.Compare)
            {
                Token token = currentToken;
                lastToken = currentToken;
                currentToken = tokenizer.GetToken();
                IExpression rightExpression = ParseAddSub(tokenizer);
                expression = new BinaryExpression(expression, rightExpression, token);
            }

            return expression;
        }

        private IExpression ParseAddSub(Tokenizer tokenizer)
        {
            IExpression expression = ParseMulDiv(tokenizer);

            while (currentToken!.TokenType == TokenType.AddSub)
            {
                Token token = currentToken;
                lastToken = currentToken;
                currentToken = tokenizer.GetToken();
                IExpression rightExpression = ParseMulDiv(tokenizer);
                expression = new BinaryExpression(expression, rightExpression, token);
            }

            return expression;
        }

        private IExpression ParseMulDiv(Tokenizer tokenizer)
        {
            IExpression expression = ParsePower(tokenizer);

            while (currentToken!.TokenType == TokenType.MulDiv)
            {
                Token token = currentToken;
                lastToken = currentToken;
                currentToken = tokenizer.GetToken();
                IExpression rightExpression = ParsePower(tokenizer);
                expression = new BinaryExpression(expression, rightExpression, token);
            }

            return expression;
        }

        private IExpression ParsePower(Tokenizer tokenizer)
        {
            IExpression expression = ParseUnary(tokenizer);

            while (currentToken!.TokenType == TokenType.Power)
            {
                Token token = currentToken;
                lastToken = currentToken;
                currentToken = tokenizer.GetToken();
                IExpression rightExpression = ParseUnary(tokenizer);
                expression = new BinaryExpression(expression, rightExpression, token);
            }

            return expression;
        }

        private IExpression ParseUnary(Tokenizer tokenizer)
        {
            if (currentToken!.TokenOperator == TokenOperator.Add || currentToken.TokenOperator == TokenOperator.Sub)
            {
                Token token = currentToken;
                lastToken = currentToken;
                currentToken = tokenizer.GetToken();
                IExpression valueExpression = ParseAtomic(tokenizer);
                return new UnaryExpression(valueExpression, token);
            }

            IExpression result = ParseAtomic(tokenizer);

            if (currentToken.TokenOperator == TokenOperator.Factorial)
            {
                Token token = currentToken;
                lastToken = currentToken;
                currentToken = tokenizer.GetToken();
                return new UnaryExpression(result, token);
            }

            return result;
        }

        private IExpression ParseAtomic(Tokenizer tokenizer)
        {
            IExpression expression;
            switch (currentToken!.TokenType)
            {
                case TokenType.Group:
                    if (currentToken.TokenOperator != TokenOperator.Open)
                    {
                        throw new ExpressionException(currentToken.StartPointer, "A closing bracket was found without an opening bracket.");
                    }

                    lastToken = currentToken;
                    currentToken = tokenizer.GetToken();

                    if (currentToken.TokenOperator != TokenOperator.Close)
                    {
                        expression = ParseCompare(tokenizer);
                    }
                    else
                    {
                        return new AtomicExpression(currentToken, null);
                    }

                    if (currentToken.TokenOperator == TokenOperator.Comma)
                    {
                        return expression;
                    }

                    if (currentToken.TokenOperator != TokenOperator.Close)
                    {
                        throw new ExpressionException(currentToken.StartPointer, "A closing bracket was required but is missing.");
                    }
                    break;
                case TokenType.Identifier:
                    // check for function
                    if (currentToken.Value is not null && currentToken.Value is string identifier)
                    {
                        if (FunctionExpression.HasFunction(identifier))
                        {
                            List<IExpression> parameters = [];

                            if (tokenizer.Peek().TokenOperator == TokenOperator.Open)
                            {
                                lastToken = currentToken;
                                currentToken = tokenizer.GetToken();

                                while (currentToken.TokenOperator != TokenOperator.Close)
                                {
                                    lastToken = currentToken;
                                    currentToken = tokenizer.GetToken();

                                    if (currentToken.TokenOperator == TokenOperator.End)
                                    {
                                        throw new ExpressionException(currentToken.StartPointer, "Function is missing close paren.");
                                    }

                                    IExpression parameter = ParseCompare(tokenizer);
                                    parameters.Add(parameter);
                                }
                            }

                            expression = new FunctionExpression(identifier, currentToken, [.. parameters]);
                            break;
                        }
                    }
                    throw new ExpressionException(currentToken.StartPointer, $"An invalid identifier '{currentToken.Value ?? "NULL"}' was found.");
                case TokenType.Literal:
                    expression = new AtomicExpression(currentToken);
                    break;
                default:
                    throw new ExpressionException(currentToken.StartPointer, $"An unsupported type of {currentToken.Value} was encountered.");
            }

            lastToken = currentToken;
            currentToken = tokenizer.GetToken();

            return expression;
        }
    }
}
