using System.Text;

namespace Expression.Parser;

public enum Associativity {
    Left,
    Right
}

public record OperatorPrecedenceRules(int Precedence, Associativity Associativity);

public class ExpressionParser {
    private Stack<char> _operatorStack;

    private Dictionary<char, OperatorPrecedenceRules> _rules =
        new()
        {
            { '^', new OperatorPrecedenceRules(4, Associativity.Right) },
            { '/', new OperatorPrecedenceRules(3, Associativity.Left) },
            { '*', new OperatorPrecedenceRules(3, Associativity.Left) },
            { '+', new OperatorPrecedenceRules(2, Associativity.Left) },
            { '-', new OperatorPrecedenceRules(2, Associativity.Left) }
        };

    public ExpressionParser() {
        _operatorStack = new Stack<char>();
    }

    public string ToPostfixNotation(string expr) {
        var trimmedExpr = expr.Trim();
        trimmedExpr = trimmedExpr.Replace(" ", string.Empty);

        var postfixNotation = new StringBuilder();
        foreach (var ch in trimmedExpr) {
            if (_rules.TryGetValue(ch, out var op)) {
                while (_operatorStack.TryPeek(out var peek)) {
                    if (peek == '(') {
                        break;
                    }
                    if (
                        op.Precedence < _rules[peek].Precedence
                        || (
                            op.Precedence == _rules[peek].Precedence
                            && _rules[peek].Associativity == Associativity.Left
                        )
                    ) {
                        postfixNotation.Append(_operatorStack.Pop());
                    } else {
                        break;
                    }
                }
                _operatorStack.Push(ch);
            } else {
                if (ch == '(') {
                    _operatorStack.Push(ch);
                } else if (ch == ')') {
                    while (_operatorStack.TryPeek(out var peek) && peek != '(') {
                        postfixNotation.Append(_operatorStack.Pop());
                    }

                    _operatorStack.Pop();
                } else {
                    postfixNotation.Append(ch);
                }
            }
        }

        while (_operatorStack.Count() > 0) {
            postfixNotation.Append(_operatorStack.Pop());
        }

        return postfixNotation.ToString();
    }
}
