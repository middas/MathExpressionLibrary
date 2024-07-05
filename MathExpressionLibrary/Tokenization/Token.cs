namespace MathExpressionLibrary.Tokenization
{
    public enum TokenOperator
    {
        GT,
        LT,
        GE,
        LE,
        EQ,
        NE,
        Add,
        Sub,
        Mul,
        Div,
        Mod,
        Factorial,
        Power,
        Open,
        Close,
        Comma,
        Atomic,
        End
    }

    public enum TokenType
    {
        Compare,
        AddSub,
        MulDiv,
        Power,
        Group,
        Literal,
        Identifier
    }

    public sealed class Token : ICloneable
    {
        public Token(TokenType tokenType, TokenOperator tokenOperator) : this(0, null, tokenType, tokenOperator)
        {
        }

        public Token(int ptr, object? value, TokenType tokenType, TokenOperator tokenOperator)
        {
            StartPointer = ptr;
            Value = value;
            TokenType = tokenType;
            TokenOperator = tokenOperator;
        }

        public int StartPointer { get; internal set; }

        public TokenOperator TokenOperator { get; }

        public TokenType TokenType { get; }

        public object? Value { get; }

        public object Clone()
        {
            return new Token(StartPointer, Value, TokenType, TokenOperator);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Token t)
            {
                bool result = t.TokenType == TokenType && t.TokenOperator == TokenOperator && t.StartPointer == StartPointer;
                if (result && t.Value is not null)
                {
                    result = t.Value.Equals(Value);
                }
                else if (result && Value is not null)
                {
                    result = Value.Equals(t.Value);
                }

                return result;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StartPointer, TokenOperator, TokenType, Value);
        }

        public override string ToString()
        {
            return $"Ptr: {StartPointer}, Type: {TokenType}, Opr: {TokenOperator}, Value: {(Value is null ? "NULL" : Value)}";
        }
    }
}