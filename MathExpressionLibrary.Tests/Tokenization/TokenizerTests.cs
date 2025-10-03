using MathExpressionLibrary.Tokenization;

namespace MathExpressionLibrary.Tests.Tokenization
{
    [TestFixture]
    public class TokenizerTests
    {
        public static IEnumerable<TestCaseData> TokenizerTestData()
        {
            yield return new TestCaseData("100").Returns(new Token(0, 100D, TokenType.Literal, TokenOperator.Atomic));
            yield return new TestCaseData("+").Returns(new Token(TokenType.AddSub, TokenOperator.Add));
            yield return new TestCaseData("-").Returns(new Token(TokenType.AddSub, TokenOperator.Sub));
            yield return new TestCaseData("*").Returns(new Token(TokenType.MulDiv, TokenOperator.Mul));
            yield return new TestCaseData("/").Returns(new Token(TokenType.MulDiv, TokenOperator.Div));
            yield return new TestCaseData("^").Returns(new Token(TokenType.Power, TokenOperator.Power));
            yield return new TestCaseData("%").Returns(new Token(TokenType.MulDiv, TokenOperator.Mod));
            yield return new TestCaseData("!").Returns(new Token(TokenType.MulDiv, TokenOperator.Factorial));
            yield return new TestCaseData("(").Returns(new Token(TokenType.Group, TokenOperator.Open));
            yield return new TestCaseData("[").Returns(new Token(TokenType.Group, TokenOperator.Open));
            yield return new TestCaseData("{").Returns(new Token(TokenType.Group, TokenOperator.Open));
            yield return new TestCaseData(")").Returns(new Token(TokenType.Group, TokenOperator.Close));
            yield return new TestCaseData("]").Returns(new Token(TokenType.Group, TokenOperator.Close));
            yield return new TestCaseData("}").Returns(new Token(TokenType.Group, TokenOperator.Close));
            yield return new TestCaseData(",").Returns(new Token(0, ',', TokenType.Group, TokenOperator.Comma));
            yield return new TestCaseData(">").Returns(new Token(TokenType.Compare, TokenOperator.GT));
            yield return new TestCaseData("<").Returns(new Token(TokenType.Compare, TokenOperator.LT));
            yield return new TestCaseData("=").Returns(new Token(TokenType.Compare, TokenOperator.EQ));
            yield return new TestCaseData(">=").Returns(new Token(TokenType.Compare, TokenOperator.GE));
            yield return new TestCaseData("<=").Returns(new Token(TokenType.Compare, TokenOperator.LE));
            yield return new TestCaseData("==").Returns(new Token(TokenType.Compare, TokenOperator.EQ));
            yield return new TestCaseData("!=").Returns(new Token(TokenType.Compare, TokenOperator.NE));
            yield return new TestCaseData("<>").Returns(new Token(TokenType.Compare, TokenOperator.NE));
            yield return new TestCaseData("\"test\"").Returns(new Token(0, "test", TokenType.Literal, TokenOperator.Atomic));
            yield return new TestCaseData(" \n\t\r100").Returns(new Token(4, 100D, TokenType.Literal, TokenOperator.Atomic));
            yield return new TestCaseData(" \n\t\r+").Returns(new Token(4, null, TokenType.AddSub, TokenOperator.Add));
            yield return new TestCaseData(" \n\t\r-").Returns(new Token(4, null, TokenType.AddSub, TokenOperator.Sub));
            yield return new TestCaseData(" \n\t\r*").Returns(new Token(4, null, TokenType.MulDiv, TokenOperator.Mul));
            yield return new TestCaseData(" \n\t\r/").Returns(new Token(4, null, TokenType.MulDiv, TokenOperator.Div));
            yield return new TestCaseData(" \n\t\r^").Returns(new Token(4, null, TokenType.Power, TokenOperator.Power));
            yield return new TestCaseData(" \n\t\r%").Returns(new Token(4, null, TokenType.MulDiv, TokenOperator.Mod));
            yield return new TestCaseData(" \n\t\r!").Returns(new Token(4, null, TokenType.MulDiv, TokenOperator.Factorial));
            yield return new TestCaseData(" \n\t\r(").Returns(new Token(4, null, TokenType.Group, TokenOperator.Open));
            yield return new TestCaseData(" \n\t\r[").Returns(new Token(4, null, TokenType.Group, TokenOperator.Open));
            yield return new TestCaseData(" \n\t\r{").Returns(new Token(4, null, TokenType.Group, TokenOperator.Open));
            yield return new TestCaseData(" \n\t\r)").Returns(new Token(4, null, TokenType.Group, TokenOperator.Close));
            yield return new TestCaseData(" \n\t\r]").Returns(new Token(4, null, TokenType.Group, TokenOperator.Close));
            yield return new TestCaseData(" \n\t\r}").Returns(new Token(4, null, TokenType.Group, TokenOperator.Close));
            yield return new TestCaseData(" \n\t\r,").Returns(new Token(4, ',', TokenType.Group, TokenOperator.Comma));
            yield return new TestCaseData(" \n\t\r>").Returns(new Token(4, null, TokenType.Compare, TokenOperator.GT));
            yield return new TestCaseData(" \n\t\r<").Returns(new Token(4, null, TokenType.Compare, TokenOperator.LT));
            yield return new TestCaseData(" \n\t\r=").Returns(new Token(4, null, TokenType.Compare, TokenOperator.EQ));
            yield return new TestCaseData(" \n\t\r>=").Returns(new Token(4, null, TokenType.Compare, TokenOperator.GE));
            yield return new TestCaseData(" \n\t\r<=").Returns(new Token(4, null, TokenType.Compare, TokenOperator.LE));
            yield return new TestCaseData(" \n\t\r==").Returns(new Token(4, null, TokenType.Compare, TokenOperator.EQ));
            yield return new TestCaseData(" \n\t\r!=").Returns(new Token(4, null, TokenType.Compare, TokenOperator.NE));
            yield return new TestCaseData(" \n\t\r<>").Returns(new Token(4, null, TokenType.Compare, TokenOperator.NE));
            yield return new TestCaseData(" \n\t\r\"test\"").Returns(new Token(4, "test", TokenType.Literal, TokenOperator.Atomic));
        }

        [Test]
        public void MathExpressionTest()
        {
            Tokenizer tokenizer = new("10 + 10 * 10 - 10 / 10");
            List<Token> tokens = [];

            while (true)
            {
                Token token = tokenizer.GetToken();
                tokens.Add(token);
                if (token.TokenOperator == TokenOperator.End)
                {
                    break;
                }
            }

            List<Token> expected = [
                new Token(0, 10D, TokenType.Literal, TokenOperator.Atomic),
                new Token(3, null, TokenType.AddSub, TokenOperator.Add),
                new Token(5, 10D, TokenType.Literal, TokenOperator.Atomic),
                new Token(8, null, TokenType.MulDiv, TokenOperator.Mul),
                new Token(10, 10D, TokenType.Literal, TokenOperator.Atomic),
                new Token(13, null, TokenType.AddSub, TokenOperator.Sub),
                new Token(15, 10D, TokenType.Literal, TokenOperator.Atomic),
                new Token(18, null, TokenType.MulDiv, TokenOperator.Div),
                new Token(20, 10D, TokenType.Literal, TokenOperator.Atomic),
                new Token(22, null, TokenType.Group, TokenOperator.End)
                ];

            Assert.That(expected, Is.EquivalentTo(tokens));
        }

        [TestCaseSource(nameof(TokenizerTestData))]
        public Token SingleTokenTest(string expression)
        {
            Tokenizer tokenizer = new(expression);
            return tokenizer.GetToken();
        }
    }
}