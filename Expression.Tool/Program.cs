using Expression.Parser;

var parser = new ExpressionParser();

var expr = "2+2";

var postfixNotation = parser.Parse(expr);
Console.WriteLine(postfixNotation);

var res = parser.Evaluate(expr);

Console.WriteLine(res);
