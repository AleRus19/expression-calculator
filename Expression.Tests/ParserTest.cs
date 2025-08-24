using Expression.Parser;

namespace Expression.Tests;

public class Tests {
    public ExpressionParser expressionParser;

    [SetUp]
    public void Setup() {
        expressionParser = new ExpressionParser();
    }

    [Test]
    public void Test1() {
        var rpn = expressionParser.Parse("1+1");

        Assert.That(rpn.ToString(), Is.EqualTo("11+"));
    }

    [Test]
    public void Test12() {
        var rpn = expressionParser.Parse("11+1");

        Assert.That(rpn.ToString(), Is.EqualTo("111+"));
    }

    [Test]
    public void Test2() {
        var rpn = expressionParser.Parse("(3+4)*5");

        Assert.That(rpn.ToString(), Is.EqualTo("34+5*"));
    }


    [Test]
    public void Test3() {
        var rpn = expressionParser.Parse("(1 * 2) - (3 * 4)");

        Assert.That(rpn.ToString(), Is.EqualTo("12*34*-"));
    }


    [Test]
    public void Test4() {
        var rpn = expressionParser.Parse("3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3");

        Assert.That(rpn.ToString(), Is.EqualTo("342*15-23^^/+"));
    }

    [Test]
    public void Test5() {
        var rpn = expressionParser.Parse("5 + 2 / (3- 8) ^ 5 ^ 2 ");

        Assert.That(rpn.ToString(), Is.EqualTo("5238-52^^/+"));
    }
}