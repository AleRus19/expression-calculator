using System.CommandLine;
using Expression.Parser;
var expressionOption = new Option<string>(
    name: "--e",
    description: "Expression"
);

var rootCommand = new RootCommand("Expression Calculator");

var evaluateCommand = new Command("eval", "Evaluate expression");
evaluateCommand.AddOption(expressionOption);
rootCommand.AddCommand(evaluateCommand);

evaluateCommand.SetHandler((expr) => {
    if (expr is null) {
        Console.WriteLine("Missing argument(s)");
        return;
    }
    var parser = new ExpressionParser();
    var res = parser.Evaluate(expr);
    Console.WriteLine(res);
}, expressionOption);

var rpnCommand = new Command("rpn", "Reverse Polish Notation");
rpnCommand.AddOption(expressionOption);
rootCommand.AddCommand(rpnCommand);

rpnCommand.SetHandler((expr) => {
    if (expr is null) {
        Console.WriteLine("Missing argument(s)");
        return;
    }
    var parser = new ExpressionParser();
    var res = parser.Parse(expr);
    Console.WriteLine(res);
}, expressionOption);

return await rootCommand.InvokeAsync(args);