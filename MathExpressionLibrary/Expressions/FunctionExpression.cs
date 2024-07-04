using MathExpressionLibrary.Exceptions;
using MathExpressionLibrary.Functions;
using MathExpressionLibrary.Tokenization;

namespace MathExpressionLibrary.Expressions
{
    public sealed class FunctionExpression : IExpression
    {
        private static Dictionary<string, IFunction> functions = [];

        private readonly IFunction assignedFunction;

        public FunctionExpression(string functionName, Token token, params IExpression[]? parameters)
        {
            FunctionName = functionName;
            Token = token;
            Parameters = parameters;

            if (functions.TryGetValue(functionName.ToUpperInvariant(), out IFunction? function))
            {
                int parameterLength = parameters?.Length ?? 0;
                if (parameterLength < function.MinimumParamterCount)
                {
                    throw new FunctionException($"The function was passed {parameterLength} parameters, but requires a minimum of {function.MinimumParamterCount} parameters.");
                }

                if (function.MaxmimumParameterCount is not null && parameterLength > function.MaxmimumParameterCount)
                {
                    throw new FunctionException($"The function was passed {parameterLength} parameters, but has a maximum of {function.MaxmimumParameterCount} parameters.");
                }

                assignedFunction = function;
            }
            else
            {
                throw new FunctionException($"{functionName} does not exist as a function.");
            }
        }

        public string FunctionName { get; }

        public IExpression[]? Parameters { get; private set; }

        public Token Token { get; }

        public static void AddFunction(string functionName, IFunction function)
        {
            if (HasFunction(functionName))
            {
                throw new FunctionException($"The function {functionName} has already been added.");
            }

            functions.Add(functionName.ToUpperInvariant(), function);
        }

        public static bool HasFunction(string functionName)
        {
            return functions.ContainsKey(functionName.ToUpperInvariant());
        }

        public static bool RemoveFunction(string functionName)
        {
            return functions.Remove(functionName.ToUpperInvariant());
        }

        public int CompareTo(IExpression? other)
        {
            return Expression.Compare(this, other);
        }

        public object? Evaluate()
        {
            return assignedFunction.Evaluate(Parameters);
        }

        public IExpression Optimize()
        {
            Parameters = Parameters?.Select(p => p.Optimize()).ToArray();

            if (Parameters is null || Parameters.All(p => p is AtomicExpression))
            {
                return new AtomicExpression(Token, assignedFunction.Evaluate(Parameters));
            }

            return this;
        }
    }
}