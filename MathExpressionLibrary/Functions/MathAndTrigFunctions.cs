using MathExpressionLibrary.Exceptions;
using MathExpressionLibrary.Expressions;
using static MathExpressionLibrary.Functions.Function;

namespace MathExpressionLibrary.Functions
{
    public sealed class MathAndTrigFunctions
    {
        private const string category = "Math & Trigonometry";

        private static readonly IList<IFunction> functions = new List<IFunction>()
        {
            Abs(),
            Acos(),
            Acosh(),
            Acot(),
            Acoth(),
            Asin(),
            Asinh(),
            Atan(),
            Atan2(),
            Atanh(),
            Base(),
            Ceiling(),
            Combin(),
            Combina(),
            E(),
            PI(),
            Sum(),
        };

        public static void RegisterFunctions()
        {
            foreach (var function in functions)
            {
                FunctionExpression.AddFunction(function);
            }
        }

        private static IFunction Abs() => new Function("ABS", "The absolute value of a number", category, new FunctionDelegate((args) => Math.Abs(ToDouble(args!.First().Evaluate()))));

        private static IFunction Acos() => new Function("ACOS", "The arcosine of a number", category, new FunctionDelegate((args) => Math.Acos(ToDouble(args!.First().Evaluate()))), 1, 1);

        private static IFunction Acosh() => new Function("ACOSH", "The inverse hyperbolic cosine", category, new FunctionDelegate((args) => Math.Acosh(ToDouble(args!.First().Evaluate()))), 1, 1);

        private static IFunction Acot() => new Function("ACOT", "The arccotangent of a number", category, new FunctionDelegate((args) => Math.Atan2(1, ToDouble(args!.First().Evaluate()))), 1, 1);

        private static IFunction Acoth() => new Function("ACOTH", "The hyperbolic arccotangent of a number", category, new FunctionDelegate((args) =>
        {
            double x = ToDouble(args!.First().Evaluate());
            if (double.IsInfinity(x))
                return 0.0;

            if (Math.Abs(x) <= 1.0)
                return double.NaN;

            return Math.Log((x + 1.0) / (x - 1.0)) / 2.0;
        }), 1, 1);

        private static IFunction Asin() => new Function("ASIN", "The arcsine of a number", category, new FunctionDelegate((args) => Math.Asin(ToDouble(args!.First().Evaluate()))), 1, 1);

        private static IFunction Asinh() => new Function("ASINH", "The hyperbolic cosine", category, new FunctionDelegate((args) => Math.Asinh(ToDouble(args!.First().Evaluate()))), 1, 1);

        private static IFunction Atan() => new Function("ATAN", "The arctangent of a number", category, new FunctionDelegate((args) => Math.Atan(ToDouble(args!.First().Evaluate()))), 1, 1);

        private static IFunction Atan2() => new Function("ATAN2'", "The arctangent for the x and y coordinate", category, new FunctionDelegate((args) => Math.Atan2(ToDouble(args!.ElementAt(0).Evaluate()), ToDouble(args!.ElementAt(1).Evaluate()))), 2, 2);

        private static IFunction Atanh() => new Function("ATANH", "The hyperbolic arctangent of a number", category, new FunctionDelegate((args) => Math.Atanh(ToDouble(args!.First().Evaluate()))), 1, 1);

        private static IFunction Base() => new Function("BASE", "Converts the number into the text representation of the given radix", category, new FunctionDelegate((args) =>
                                                                                        {
                                                                                            double doubleNumber = ToDouble(args!.ElementAt(0).Evaluate());
                                                                                            int radix = ToInteger(args!.ElementAt(1).Evaluate());
                                                                                            const int BitsInLong = 64;
                                                                                            const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                                                                                            if (radix < 2 || radix > Digits.Length)
                                                                                                throw new ArgumentException("The radix must be >= 2 and <= " + Digits.Length.ToString());

                                                                                            if (doubleNumber == 0)
                                                                                                return "0";

                                                                                            int index = BitsInLong - 1;
                                                                                            long currentNumber = (long)Math.Abs(doubleNumber);
                                                                                            char[] charArray = new char[BitsInLong];

                                                                                            while (currentNumber != 0)
                                                                                            {
                                                                                                int remainder = (int)(currentNumber % radix);
                                                                                                charArray[index--] = Digits[remainder];
                                                                                                currentNumber = currentNumber / radix;
                                                                                            }

                                                                                            string result = new(charArray, index + 1, BitsInLong - index - 1);
                                                                                            if (doubleNumber < 0)
                                                                                            {
                                                                                                result = "-" + result;
                                                                                            }

                                                                                            return result;
                                                                                        }), 2, 2);

        private static IFunction Ceiling() => new Function("CEILING", "Rounds a number to the nearest integer or nearest multiple of significance", category, new FunctionDelegate((args) => Math.Ceiling(ToDouble(args!.First().Evaluate()))), 1, 1);

        private static IFunction Combin() => new Function("COMBIN", "Returns the number of combinations for the given number of items", category, new FunctionDelegate((args) =>
                                                                                                        {
                                                                                                            int n = ToInteger(args!.ElementAt(0).Evaluate());
                                                                                                            int k = ToInteger(args!.ElementAt(1).Evaluate());

                                                                                                            if (n < 0)
                                                                                                            {
                                                                                                                throw new FunctionException($"Number {n} cannot be negative.");
                                                                                                            }

                                                                                                            if (k < 0)
                                                                                                            {
                                                                                                                throw new FunctionException($"Number {k} cannot be negative.");
                                                                                                            }

                                                                                                            long factorialN = Enumerable.Range(1, n).Aggregate(1, (cur, i) => cur * i);
                                                                                                            long divisor = Enumerable.Range(1, n - k).Aggregate(1, (cur, i) => cur * i);

                                                                                                            return factorialN / (k * divisor);
                                                                                                        }), 2, 2);

        private static IFunction Combina() => new Function("COMBINA", "Returns the number of combinations with repititions for ht given number of items", category, new FunctionDelegate((args) =>
        {
            int n = ToInteger(args!.ElementAt(0).Evaluate());
            int k = ToInteger(args!.ElementAt(1).Evaluate());

            if (n < 0)
            {
                throw new FunctionException($"Number {n} cannot be negative.");
            }

            if (k < 0)
            {
                throw new FunctionException($"Number {k} cannot be negative.");
            }

            n = n + k - 1;

            long factorial = Enumerable.Range(1, n).Aggregate(1, (cur, i) => cur * i);
            long divisor = Enumerable.Range(1, n - k).Aggregate(1, (cur, i) => cur * i);

            return (factorial / (k * divisor)) / 2;
        }), 2, 2);

        private static IFunction E() => new Function("E", "The value of E", category, new FunctionDelegate((args) => Math.E));

        private static IFunction PI() => new Function("PI", "The value of PI", category, new FunctionDelegate((args) => Math.PI));

        private static IFunction Sum() => new Function("SUM", "The sum of the values", category, new FunctionDelegate((args) => args!.Aggregate(0D, (cur, val) => cur + ToDouble(val.Evaluate()))), 1);

        private static double ToDouble(object? obj)
        {
            if (obj is double d)
            {
                return d;
            }

            return 0;
        }

        private static int ToInteger(object? v)
        {
            if (v is int i)
            {
                return i;
            }

            if (v is double d)
            {
                return (int)d;
            }

            return 0;
        }
    }
}