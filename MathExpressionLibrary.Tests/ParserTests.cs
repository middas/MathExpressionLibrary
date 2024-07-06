using MathExpressionLibrary.Expressions;

namespace MathExpressionLibrary.Tests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void ParseExpression_Simple()
        {
            var parser = new Parser();
            IExpression expression = parser.ParseExpression("1+1");

            Assert.That(expression, Is.Not.Null);
            Assert.That(expression.Evaluate(), Is.EqualTo(2));
        }

        [TestCaseSource(nameof(MathTests))]
        [DefaultFloatingPointTolerance(0.0001)]
        public object ParseExpression_MathTests(string expression)
        {
            var parser = new Parser();
            IExpression result = parser.ParseExpression(expression);

            Assert.That(result, Is.Not.Null);

            return result.Evaluate()!;
        }

        private static IEnumerable<TestCaseData> MathTests()
        {
            yield return new TestCaseData("9-((2^3-3)*8)/6").Returns(2.33333333D);
            yield return new TestCaseData("7-(1+6)").Returns(0D);
            yield return new TestCaseData("(2^2+1)*(2^2+1)").Returns(25D);
            yield return new TestCaseData("2^3/(3-1)").Returns(4D);
            yield return new TestCaseData("6^2/3+8").Returns(20D);
            yield return new TestCaseData("7+3*3^2+4").Returns(38D);
            yield return new TestCaseData("(7*6)+(4*3)").Returns(54D);
            yield return new TestCaseData("(3+7)*7").Returns(70D);
            yield return new TestCaseData("2^6-(2^3-5)").Returns(61D);
            yield return new TestCaseData("5*2+2").Returns(12D);
            yield return new TestCaseData("7*(5/1)").Returns(35D);
            yield return new TestCaseData("(1+15)/2^3").Returns(2D);
            yield return new TestCaseData("4+1-1").Returns(4D);
            yield return new TestCaseData("41-4*3^2").Returns(5D);
            yield return new TestCaseData("6/3-1").Returns(1D);
            yield return new TestCaseData("4-4+7").Returns(7D);
            yield return new TestCaseData("2^2*4/2").Returns(8D);
            yield return new TestCaseData("24/2^2-2+2").Returns(6D);
            yield return new TestCaseData("(3+9)/4").Returns(3D);
            yield return new TestCaseData("2^4-(2^2-1)^2").Returns(7D);
            yield return new TestCaseData("9+8-4").Returns(13D);
            yield return new TestCaseData("5!/2^3").Returns(15D);
            yield return new TestCaseData("(-4)-5*(3-(3^2+5))").Returns(51D);
            yield return new TestCaseData("(-8)-7*(2-(2^3+(-7)))").Returns(-15D);
            yield return new TestCaseData("9--6*(-2-(-2^3+6))").Returns(9D);
            yield return new TestCaseData("(-4-(-8/-4)^2)*-6+-6").Returns(42D);
            yield return new TestCaseData("(2+(4/2))*2^3-8").Returns(24D);
            yield return new TestCaseData("10-11*(5-(5^2+11))").Returns(351D);
            yield return new TestCaseData("(-11-(-18/9)^2)*-8+-8").Returns(112D);
            yield return new TestCaseData("((96/-4)^2-11)*-10+-10").Returns(-5660D);
            yield return new TestCaseData("6-4*(4-(4^2+4))").Returns(70D);
            yield return new TestCaseData("((3^3+2)*3)-7+2").Returns(82D);
            yield return new TestCaseData("((-72/-2)^3--7)*-5+-5").Returns(-233320D);
            yield return new TestCaseData("(-2+(-4/-2))*-2^3--4").Returns(4D);
            yield return new TestCaseData("+1.23456 + -6.54321").Returns(-5.30865D);
            yield return new TestCaseData("1+1=3-1").Returns(true);
            yield return new TestCaseData("1+1>=3-1").Returns(true);
            yield return new TestCaseData("1+1<=3-1").Returns(true);
            yield return new TestCaseData("1+1>=3-2").Returns(true);
            yield return new TestCaseData("1+1<=3+2").Returns(true);
            yield return new TestCaseData("1+1>3-2").Returns(true);
            yield return new TestCaseData("1+1<3+2").Returns(true);
            yield return new TestCaseData("(-4-(-8/-4)^2)*-6+-6>0").Returns(true);
            yield return new TestCaseData("sum(1)").Returns(1);
            yield return new TestCaseData("sum(1,2,3)").Returns(6);
            yield return new TestCaseData("sum(1,2,3,4,5,6,7,8,9)").Returns(45);
            yield return new TestCaseData("pi").Returns(Math.PI);
            yield return new TestCaseData("e").Returns(Math.E);
            yield return new TestCaseData("combin(8,2)").Returns(28);
            yield return new TestCaseData("combina(4, 3)").Returns(20);
            yield return new TestCaseData("combina(10, 3)").Returns(220);
            yield return new TestCaseData("combina(5, 3)").Returns(35);
            yield return new TestCaseData("factdouble(6)").Returns(48);
            yield return new TestCaseData("factdouble(7)").Returns(105);
            yield return new TestCaseData("lcm(5,2)").Returns(10);
            yield return new TestCaseData("lcm(24,36)").Returns(72);
        }
    }
}
