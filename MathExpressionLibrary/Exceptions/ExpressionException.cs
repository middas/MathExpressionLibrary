namespace MathExpressionLibrary.Exceptions
{
    [Serializable]
    public class ExpressionException : Exception
    {
        public ExpressionException() : this(null, null)
        {
        }

        public ExpressionException(int? position) : this(position, null)
        {
        }

        public ExpressionException(string? message) : this(null, message)
        {
        }

        public ExpressionException(int? position, string? message) : base(message)
        {
            Position = position;
        }

        public int? Position { get; }
    }
}