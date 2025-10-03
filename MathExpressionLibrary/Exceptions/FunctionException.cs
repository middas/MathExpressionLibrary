namespace MathExpressionLibrary.Exceptions
{
    [Serializable]
    public sealed class FunctionException : Exception
    {
        public FunctionException(string message) : base(message)
        {
        }
    }
}