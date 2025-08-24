using Expression.Parser.Models;

namespace Expression.Parser;

public class ExpressionParser {
    private readonly Stack<char> _stack;

    private readonly Stack<double> _evalStack;

    private readonly Dictionary<string, OperatorPrecedenceRules> _rules =
        new()
        {
            { "^", new OperatorPrecedenceRules(4, Associativity.Right, Math.Pow) },
            { "/", new OperatorPrecedenceRules(3, Associativity.Left, (b, a) => b / a) },
            { "*", new OperatorPrecedenceRules(3, Associativity.Left, (b, a) => b * a) },
            { "+", new OperatorPrecedenceRules(2, Associativity.Left, (b, a) => b + a) },
            { "-", new OperatorPrecedenceRules(2, Associativity.Left, (b, a) => b - a) }
        };

    public ExpressionParser() {
        _stack = new Stack<char>();
        _evalStack = new Stack<double>();
    }

    // Shunting Yard Algorithm
    public ParserResult Parse(string expr) {
        var result = new ParserResult();

        var trimmedExpr = expr.Trim();
        trimmedExpr = trimmedExpr.Replace(" ", string.Empty);

        var element = "";
        foreach (var ch in trimmedExpr) {
            if (_rules.TryGetValue(ch.ToString(), out var op)) {
                if (!string.IsNullOrEmpty(element)) {
                    result.Elements.Add(element);
                }
                element = "";
                while (_stack.TryPeek(out var peek)) {
                    if (peek == '(') {
                        break;
                    }
                    if (
                        op.Precedence < _rules[peek.ToString()].Precedence
                        || (
                            op.Precedence == _rules[peek.ToString()].Precedence
                            && _rules[peek.ToString()].Associativity == Associativity.Left
                        )
                    ) {
                        result.Elements.Add(_stack.Pop().ToString());
                    } else {
                        break;
                    }
                }
                _stack.Push(ch);
            } else {
                if (ch == '(') {
                    _stack.Push(ch);
                } else if (ch == ')') {
                    if (!string.IsNullOrEmpty(element)) {
                        result.Elements.Add(element);
                    }
                    element = "";
                    while (_stack.TryPeek(out var peek) && peek != '(') {
                        result.Elements.Add(_stack.Pop().ToString());
                    }

                    _stack.Pop();
                } else {
                    element += ch;
                }
            }
        }
        if (!string.IsNullOrEmpty(element)) {
            result.Elements.Add(element);
        }

        while (_stack.Count() > 0) {
            result.Elements.Add(_stack.Pop().ToString());
        }

        return result;
    }


    public double Evaluate(string expr, int round = 2) {
        _stack.Clear();

        var postfixNotation = Parse(expr);
        foreach (var elem in postfixNotation.Elements) {
            if (!_rules.TryGetValue(elem, out var op)) {
                _ = double.TryParse(elem, out var number);

                _evalStack.Push(number);
            } else {
                var a = _evalStack.Pop();
                var b = _evalStack.Pop();

                var res = _rules[elem].Func(b, a);
                _evalStack.Push(res);
            }
        }

        return Math.Round(_evalStack.Pop(), round);
    }
}
