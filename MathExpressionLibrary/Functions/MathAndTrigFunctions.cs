using MathExpressionLibrary.Exceptions;
using MathExpressionLibrary.Expressions;
using System.Collections.Generic;
using System.Runtime;
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
            Cos(),
            Cosh(),
            Cot(),
            Coth(),
            Csc(),
            Csch(),
            Dec(),
            Degrees(),
            E(),
            Even(),
            Exp(),
            Fact(),
            FactDouble(),
            Floor(),
            GCD(),
            Int(),
            LCM(),
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

        private static IFunction Combina() => new Function("COMBINA", "Returns the number of combinations with repititions for the given number of items", category, new FunctionDelegate((args) =>
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

        private static IFunction Cos() => new Function("COS", "The cosine value of a number", category, new FunctionDelegate((args) => Math.Cos(ToDouble(args!.First().Evaluate()))), 1, 1);

        private static IFunction Cosh() => new Function("COSH", "The hyperbolic cosine of a number", category, new FunctionDelegate((args) => Math.Cosh(ToDouble(args!.First().Evaluate()))), 1, 1);

        private static IFunction Cot() => new Function("COT", "Returns the cotangent of an angle", category, new FunctionDelegate((args) =>
        {
            double a = ToDouble(args!.First().Evaluate());
            var sin = Math.Sin(a);
            if (sin == 0.0)
                return double.NaN;

            return Math.Cos(a) / sin;
        }), 1, 1);

        private static IFunction Coth() => new Function("COTH", "Returns the hyperbolic cotangent of a number", category, new FunctionDelegate((args) =>
        {
            double x = ToDouble(args!.First().Evaluate());
            if (x == 0.0)
                return double.NaN;

            return 1.0 / Math.Tanh(x);
        }), 1, 1);

        private static IFunction Csc() => new Function("CSC", "Returns the cosecant of an angle", category, new FunctionDelegate((args) =>
        {
            double a = ToDouble(args!.First().Evaluate());
            var sin = Math.Sin(a);
            if (sin == 0.0)
                return double.NaN;

            return 1.0 / sin;
        }), 1, 1);

        private static IFunction Csch() => new Function("CSCH", "Returns the hyperbolic cosecant of an angle", category, new FunctionDelegate((args) =>
        {
            double x = ToDouble(args!.First().Evaluate());
            var sin = Math.Sinh(x);
            if (sin == 0.0)
                return double.NaN;

            return 1.0 / sin;
        }), 1, 1);

        private static IFunction Dec() => new Function("DECIMAL", "Converts the number and given radix to a decimal number", category, new FunctionDelegate((args) =>
                                                                                                                                                                        {
                                                                                                                                                                            const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                                                                                                                                                                            int radix = ToInteger(args!.ElementAt(1).Evaluate());
                                                                                                                                                                            string? number = args!.ElementAt(0).Evaluate()?.ToString();

                                                                                                                                                                            if (radix < 2 || radix > Digits.Length)
                                                                                                                                                                                throw new FunctionException("The radix must be >= 2 and <= " +
                                                                                                                                                                                    Digits.Length.ToString());

                                                                                                                                                                            if (string.IsNullOrEmpty(number))
                                                                                                                                                                                return 0;

                                                                                                                                                                            // Make sure the arbitrary numeral system number is in upper case
                                                                                                                                                                            number = number.ToUpperInvariant();

                                                                                                                                                                            long result = 0;
                                                                                                                                                                            long multiplier = 1;
                                                                                                                                                                            for (int i = number.Length - 1; i >= 0; i--)
                                                                                                                                                                            {
                                                                                                                                                                                char c = number[i];
                                                                                                                                                                                if (i == 0 && c == '-')
                                                                                                                                                                                {
                                                                                                                                                                                    // This is the negative sign symbol
                                                                                                                                                                                    result = -result;
                                                                                                                                                                                    break;
                                                                                                                                                                                }

                                                                                                                                                                                int digit = Digits.IndexOf(c);
                                                                                                                                                                                if (digit == -1)
                                                                                                                                                                                    throw new FunctionException(
                                                                                                                                                                                        "Invalid character in the arbitrary numeral system number");

                                                                                                                                                                                result += digit * multiplier;
                                                                                                                                                                                multiplier *= radix;
                                                                                                                                                                            }

                                                                                                                                                                            return result;
                                                                                                                                                                        }), 2, 2);

        private static IFunction Degrees() => new Function("DEGREES", "Converts radians to degrees", category, new FunctionDelegate((args) => ToDouble(args!.First().Evaluate()) * Math.PI / 180.0), 1, 1);

        private static IFunction E() => new Function("E", "The value of E", category, new FunctionDelegate((args) => Math.E));

        private static IFunction Even() => new Function("EVEN", "Rounds a number to the nearest even number", category, new FunctionDelegate((args) => Math.Round(ToDouble(args!.First().Evaluate()), MidpointRounding.ToEven)), 1, 1);

        private static IFunction Exp() => new Function("EXP", "Raises a number to the given exponent", category, new FunctionDelegate((args) => Math.Pow(ToDouble(args!.First().Evaluate()), ToDouble(args!.ElementAt(1).Evaluate()))), 2, 2);

        private static IFunction Fact() => new Function("FACT", "Returns the factorial of a number", category, new FunctionDelegate((args) => Enumerable.Range(1, Math.Max(1, ToInteger(args!.First().Evaluate()))).Aggregate(1, (cur, i) => cur * i)), 1, 1);

        private static IFunction FactDouble() => new Function("FACTDOUBLE", "Returns the double factorial of a number", category, new FunctionDelegate((args) =>
        {
            IEnumerable<int> numbers;
            int x = ToInteger(args!.First().Evaluate());

            if (x % 2 == 0)
            {
                numbers = Enumerable.Range(1, x).Where(i => i % 2 == 0);
            }
            else
            {
                numbers = Enumerable.Range(1, x).Where(i => i % 2 != 0);
            }

            return numbers.Aggregate(1, (cur, i) => cur * i);
        }), 1, 1);

        private static IFunction Floor() => new Function("FLOOR", "Rounds a number down towards zero", category, new FunctionDelegate((args) => Math.Floor(ToDouble(args!.First().Evaluate()))), 1, 1);

        private static IFunction GCD() => new Function("GCD", "Returns the greatest common divisor", category, new FunctionDelegate((args) =>
        {
            uint localGcd(uint x, uint y)
            {
                while (x != 0 && y != 0)
                {
                    if (x > y) x %= y;
                    else y %= x;
                }

                return x == 0 ? y : x;
            };

            uint localGcd1(params uint[] numbers)
            {
                if (numbers == null || numbers.Length == 0) return 0;

                var result = numbers[0];

                for (int i = 1; i < numbers.Length; i++)
                {
                    result = localGcd(result, numbers[i]);
                }

                return result;
            }

            return localGcd1(args!.Select(x => (uint)ToInteger(x.Evaluate())).ToArray());
        }), 2);

        private static IFunction Int() => new Function("INT", "Rounds a number down to the nearest integer", category, new FunctionDelegate((args) => Math.Round(ToDouble(args!.First().Evaluate()), MidpointRounding.AwayFromZero)));

        private static IFunction LCM() => new Function("LCM", "The lowest common multiple", category, new FunctionDelegate((args) =>
                                                                                                                                                                                                                                                        {
                                                                                                                                                                                                                                                            int gcf(int a, int b)
                                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                                while (b != 0)
                                                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                                                    int temp = b;
                                                                                                                                                                                                                                                                    b = a % b;
                                                                                                                                                                                                                                                                    a = temp;
                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                return a;
                                                                                                                                                                                                                                                            }

                                                                                                                                                                                                                                                            int lcm(int a, int b)
                                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                                return (a / gcf(a, b)) * b;
                                                                                                                                                                                                                                                            }

                                                                                                                                                                                                                                                            int lcm1(params int[] numbers)
                                                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                                                if (numbers == null || numbers.Length == 0) return 0;

                                                                                                                                                                                                                                                                var result = numbers[0];

                                                                                                                                                                                                                                                                for (int i = 1; i < numbers.Length; i++)
                                                                                                                                                                                                                                                                {
                                                                                                                                                                                                                                                                    result = lcm(result, numbers[i]);
                                                                                                                                                                                                                                                                }

                                                                                                                                                                                                                                                                return result;
                                                                                                                                                                                                                                                            }

                                                                                                                                                                                                                                                            return lcm1(args!.Select(x => ToInteger(x.Evaluate())).ToArray());
                                                                                                                                                                                                                                                        }), 2);

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