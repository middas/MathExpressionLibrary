using MathExpressionLibrary.Expressions;

namespace MathExpressionLibrary.Functions
{
    public sealed class Function : IFunction
    {
        private readonly FunctionDelegate functionDelegate;

        public Function(string name, string description, string category, FunctionDelegate functionDelegate) : this(name, description, category, functionDelegate, 0, null)
        {
        }

        public Function(string name, string description, string category, FunctionDelegate functionDelegate, int minimumParameterCount) : this(name, description, category, functionDelegate, minimumParameterCount, null)
        {
        }

        public Function(string name, string description, string category, FunctionDelegate functionDelegate, int minimumParameterCount, int? maxmimumParameterCount)
        {
            Name = name;
            Description = description;
            Category = category;
            this.functionDelegate = functionDelegate;
            MinimumParameterCount = minimumParameterCount;
            MaxmimumParameterCount = maxmimumParameterCount;
        }

        public delegate object? FunctionDelegate(IList<IExpression>? arguments);

        public string Description { get; }

        public int? MaxmimumParameterCount { get; set; }

        public int MinimumParameterCount { get; set; }

        public string Name { get; }
        public string Category { get; }

        public object? Evaluate(IExpression[]? parameters)
        {
            return functionDelegate(parameters);
        }
    }
}