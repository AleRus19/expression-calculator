using System.Text;

namespace Expression.Parser;

public enum Associativity
{
    Left,
    Right
}

public record OperatorPrecedenceRules(int Precedence, Associativity Associativity);

public class ExpressionParser
{
    private Stack<string> _operatorStack;

    private Dictionary<char, OperatorPrecedenceRules> _rules =
        new()
        {
            { '^', new OperatorPrecedenceRules(4, Associativity.Left) },
            { '/', new OperatorPrecedenceRules(3, Associativity.Left) },
            { '*', new OperatorPrecedenceRules(3, Associativity.Left) },
            { '+', new OperatorPrecedenceRules(2, Associativity.Left) },
            { '-', new OperatorPrecedenceRules(2, Associativity.Left) }
        };

    public ExpressionParser()
    {
        _operatorStack = new Stack<string>();
    }

    public string ToPostfixNotation(string expr)
    {
        var trimmedExpr = expr.Trim();
        trimmedExpr = trimmedExpr.Replace(" ", string.Empty);

        var postfixNotation = new StringBuilder();
        foreach (var ch in trimmedExpr)
        {
            if (_rules.TryGetValue(ch, out var op))
            {
                // Handle operator in stack
            }
            else
            {
                postfixNotation.Append(ch);
            }
        }

        return postfixNotation.ToString();
    }
}
