using MathExpressionLibrary.Expressions;
using static MathExpressionLibrary.Functions.Function;

namespace MathExpressionLibrary.Functions
{
    public sealed class MathAndTrigFunctions
    {
        private static readonly IList<IFunction> functions = new List<IFunction>()
        {
            Sum()
        };

        public static void RegisterFunctions()
        {
            foreach (var function in functions)
            {
                FunctionExpression.AddFunction(function);
            }
        }

        private static IFunction Sum()
        {
            return new Function("SUM", "Sums the parameters together", new FunctionDelegate((args) => args!.Aggregate(0D, (cur, val) => cur + ToDouble(val.Evaluate()))), 1);
        }

        private static double ToDouble(object? obj)
        {
            if (obj is null)
            {
                return 0;
            }

            if (obj is double d)
            {
                return d;
            }

            return 0;
        }
    }
}