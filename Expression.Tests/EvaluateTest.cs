using Expression.Parser;

namespace Expression.Tests;

public class EvaluateTests {
    public ExpressionParser expressionParser;

    [SetUp]
    public void Setup() {
        expressionParser = new ExpressionParser();
    }

    [Test]
    public void Test1() {
        var res = expressionParser.Evaluate("1+1");


        Assert.That(res, Is.EqualTo(2));
    }

    [Test]
    public void Test2() {
        var res = expressionParser.Evaluate("(3+4)*5");

        Assert.That(res, Is.EqualTo((3 + 4) * 5));
    }

    [Test]
    public void Test3() {
        var res = expressionParser.Evaluate("(1 * 2) - (3 * 4)");

        Assert.That(res, Is.EqualTo((1 * 2) - (3 * 4)));
    }


    [Test]
    public void Test4() {
        var res = expressionParser.Evaluate("(3 + (4 * 2)) / ((( 1 - 5 ) ^ 2) ^ 3)", 5);

        Assert.That(res, Is.EqualTo(Math.Round((3 + (4 * 2)) / Math.Pow(Math.Pow(1 - 5, 2), 3), 5)));
    }

    [Test]
    public void Test5() {
        var res = expressionParser.Evaluate("5 + (((2 / (3- 8)) ^ 5) ^ 2)", 3);

        Assert.That(res, Is.EqualTo(Math.Round(5 + Math.Pow(Math.Pow(2.0 / (3.0 - 8.0), 5), 3))));
    }

    [Test]
    public void Test6() {
        var res = expressionParser.Evaluate("11 + 1");

        Assert.That(res, Is.EqualTo(11 + 1));
    }
}