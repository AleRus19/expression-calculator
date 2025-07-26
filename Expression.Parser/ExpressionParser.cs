using System.Text;

namespace Expression.Parser;

public enum Associativity {
    Left,
    Right
}

public record OperatorPrecedenceRules(int Precedence, Associativity Associativity);

public class ExpressionParser {
    private readonly Stack<char> _stack;
    private readonly Stack<double> _evalStack;


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
        _stack = new Stack<char>();
        _evalStack = new Stack<double>();
    }

    public string ToPostfixNotation(string expr) {
        var trimmedExpr = expr.Trim();
        trimmedExpr = trimmedExpr.Replace(" ", string.Empty);

        var postfixNotation = new StringBuilder();
        foreach (var ch in trimmedExpr) {
            if (_rules.TryGetValue(ch, out var op)) {
                while (_stack.TryPeek(out var peek)) {
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
                        postfixNotation.Append(_stack.Pop());
                    } else {
                        break;
                    }
                }
                _stack.Push(ch);
            } else {
                if (ch == '(') {
                    _stack.Push(ch);
                } else if (ch == ')') {
                    while (_stack.TryPeek(out var peek) && peek != '(') {
                        postfixNotation.Append(_stack.Pop());
                    }

                    _stack.Pop();
                } else {
                    postfixNotation.Append(ch);
                }
            }
        }

        while (_stack.Count() > 0) {
            postfixNotation.Append(_stack.Pop());
        }

        return postfixNotation.ToString();
    }


    public double Evaluate(string expr) {
        _stack.Clear();

        var postfixNotation = ToPostfixNotation(expr);
        foreach (var ch in postfixNotation) {
            if (!_rules.TryGetValue(ch, out var op)) {
                double.TryParse(ch.ToString(), out var number);

                _evalStack.Push(number);
            } else {
                var a = _evalStack.Pop();
                var b = _evalStack.Pop();

                var res = ch switch {
                    '+' => b + a,
                    '-' => b - a,
                    '*' => b * a,
                    '/' => b / a,
                    '^' => Math.Pow(b, a),
                    _ => throw new NotSupportedException()
                };

                _evalStack.Push(res);
            }
        }

        return _evalStack.Pop();
    }
}
