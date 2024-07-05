using MathExpressionLibrary.Expressions;

namespace MathExpressionLibrary.Functions
{
    /// <summary>
    /// Handles function expression evaluations
    /// </summary>
    public interface IFunction
    {
        /// <summary>
        /// The function description
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// The maximum number of parameters
        /// Null if there is no maximum
        /// </summary>
        public int? MaxmimumParameterCount { get; set; }

        /// <summary>
        /// The minimum number of parameters required
        /// </summary>
        public int MinimumParameterCount { get; set; }

        /// <summary>
        /// The function name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Evaluates the function
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>The evaluated result</returns>
        object? Evaluate(IExpression[]? parameters);
    }
}