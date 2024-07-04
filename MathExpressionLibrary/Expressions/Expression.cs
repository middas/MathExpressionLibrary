namespace MathExpressionLibrary.Expressions
{
    public static class Expression
    {
        public static int Compare(IExpression? left, IExpression? right)
        {
            var e1 = left?.Evaluate() as IComparable;
            var e2 = right?.Evaluate() as IComparable;

            if (e1 is null)
            {
                return 1;
            }

            if (e2 is null)
            {
                return -1;
            }

            if (e1 is null && e2 is null)
            {
                return 0;
            }

            if (e1.GetType() != e2.GetType())
            {
                e2 = Convert.ChangeType(e2, e1.GetType()) as IComparable;
            }

            return e1.CompareTo(e2);
        }
    }
}