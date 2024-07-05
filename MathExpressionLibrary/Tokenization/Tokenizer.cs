using MathExpressionLibrary.Exceptions;
using System.Globalization;

namespace MathExpressionLibrary.Tokenization
{
    public sealed class Tokenizer(string expression)
    {
        private static Dictionary<object, Token> operatorTokens = new()
        {
            {'+', new Token(TokenType.AddSub, TokenOperator.Add) },
            {'-', new Token(TokenType.AddSub, TokenOperator.Sub) },
            {'*', new Token(TokenType.MulDiv, TokenOperator.Mul) },
            {'/', new Token(TokenType.MulDiv, TokenOperator.Div) },
            {'%', new Token(TokenType.MulDiv, TokenOperator.Mod) },
            {'!', new Token(TokenType.MulDiv, TokenOperator.Factorial) },
            {'^', new Token(TokenType.Power, TokenOperator.Power) },
            {'=', new Token(TokenType.Compare, TokenOperator.EQ) },
            {"==", new Token(TokenType.Compare, TokenOperator.EQ) },
            {'>', new Token(TokenType.Compare, TokenOperator.GT) },
            {'<', new Token(TokenType.Compare, TokenOperator.LT) },
            {">=", new Token(TokenType.Compare, TokenOperator.GE) },
            {"<=", new Token(TokenType.Compare, TokenOperator.LE) },
            {"!=", new Token(TokenType.Compare, TokenOperator.NE) },
            {"<>", new Token(TokenType.Compare, TokenOperator.NE) },
            {'(', new Token(TokenType.Group, TokenOperator.Open) },
            {'[', new Token(TokenType.Group, TokenOperator.Open) },
            {'{', new Token(TokenType.Group, TokenOperator.Open) },
            {')', new Token(TokenType.Group, TokenOperator.Close) },
            {']', new Token(TokenType.Group, TokenOperator.Close) },
            {'}', new Token(TokenType.Group, TokenOperator.Close) },
        };

        private readonly char decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
        private readonly string expression = expression;
        private readonly int length = expression.Length;
        private readonly char listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator[0];

        private Token? nextToken = null;

        public int Position { get; private set; }

        public Token GetToken()
        {
            if (nextToken is not null)
            {
                Token temp = (Token)nextToken.Clone();
                nextToken = null;
                Position = temp.StartPointer + 1;
                return temp;
            }

            while (Position < length && char.IsWhiteSpace(expression[Position]))
            {
                Position++;
            }

            if (Position >= length)
            {
                return new Token(Position, null, TokenType.Group, TokenOperator.End);
            }

            char ch = expression[Position];
            bool isNumber = char.IsDigit(ch) || ch == decimalSeparator;
            bool isLetter = char.IsLetter(ch);

            if (!isNumber && !isLetter)
            {
                if (ch == listSeparator)
                {
                    Token t1 = new(Position, ch, TokenType.Group, TokenOperator.Comma);
                    Position++;
                    return t1;
                }

                if (operatorTokens.TryGetValue(ch, out Token? token))
                {
                    token.StartPointer = Position;

                    if (Position + 1 < length && operatorTokens.TryGetValue(expression.Substring(Position, 2), out Token? t1))
                    {
                        token = t1;
                        token.StartPointer = Position;
                        Position++;
                    }

                    Position++;
                    return token;
                }

                if (Position + 1 < length && operatorTokens.TryGetValue(expression.Substring(Position, 2), out Token? t2))
                {
                    t2.StartPointer = Position;
                    Position += 2;
                    return t2;
                }
            }

            int i = 0;
            if (isNumber)
            {
                double value = 0;
                double divisor = -1;

                for (; Position + i < length; i++)
                {
                    ch = expression[Position + i];

                    if (char.IsDigit(ch))
                    {
                        value = value * 10 + (ch - '0');
                        if (divisor > -1)
                        {
                            divisor *= 10;
                        }
                        continue;
                    }

                    if (ch == decimalSeparator)
                    {
                        if (divisor > -1)
                        {
                            throw new ExpressionException(Position, "An invalid number was detected with multiple decimal separators.");
                        }

                        divisor = 1;
                        continue;
                    }

                    break;
                }

                if (divisor > -1)
                {
                    value /= divisor;
                }

                Token token = new(Position, value, TokenType.Literal, TokenOperator.Atomic);
                Position += i;
                return token;
            }

            if (ch == '"')
            {
                i = 1;
                for (; Position + i < length; i++)
                {
                    ch = expression[Position + i];
                    if (ch == '"')
                    {
                        break;
                    }
                }

                if (ch != '"')
                {
                    throw new ExpressionException(Position, "The end of the expression was reached before a matching close quote was found.");
                }

                Token token = new(Position, expression.Substring(Position + 1, i - 1), TokenType.Literal, TokenOperator.Atomic);
                Position += i;
                return token;
            }

            for (; i < length; i++)
            {
                if (!char.IsLetterOrDigit(expression[Position + i]) && expression[Position + i] != '_')
                {
                    break;
                }
            }

            Token t = new(Position, expression.Substring(Position, i), TokenType.Identifier, TokenOperator.Atomic);
            Position += i;
            return t;
        }

        public Token Peek()
        {
            int currentPosition = Position;
            nextToken = GetToken();
            Position = currentPosition;
            return nextToken;
        }
    }
}