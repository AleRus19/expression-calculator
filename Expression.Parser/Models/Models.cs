using System.Text;

namespace Expression.Parser.Models;

public enum Associativity {
    Left,
    Right
}

public record OperatorPrecedenceRules(int Precedence, Associativity Associativity, Func<double, double, double> Func);

public class ParserResult {
    public List<string> Elements { get; set; } = [];

    public override string ToString() {
        var result = new StringBuilder();
        foreach (var elem in Elements) {
            result.Append(elem);
        }
        return result.ToString();
    }
}