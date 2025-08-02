namespace System.CommandLine;

internal sealed class TestCommandSystemCommandLine : RootCommand {
    private readonly Argument<string> name = new("name");
    private readonly Argument<int> number = new("number");
    private readonly Option<bool> verbose = new("--verbose");

    public TestCommandSystemCommandLine() {
        Arguments.Add(name);
        Arguments.Add(number);
        Options.Add(verbose);
        SetAction(Greet);
    }

    private void Greet(ParseResult obj) {
        var v = obj.GetValue(verbose);
        var n = obj.GetValue(name);
        var n2 = obj.GetValue(number);

        Console.WriteLine(v ? $"Hello, {n} at {DateTime.Now:yyyy-MM-dd HH:mm:ss}!" : $"Hello, {n}!");
        Console.WriteLine($"The number is {n2}");
    }
}
