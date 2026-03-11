using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Running;
using CliFx;
using Spectre.Console.Cli;
using System.CommandLine;

namespace ConsoleBenchmark;

[SimpleJob(RunStrategy.ColdStart, warmupCount: 0)]
[MemoryDiagnoser]
public class Benchmarks {
    private static readonly int Number = Random.Shared.Next(1, 101);
    private static readonly string[] Lee = ["Lee", Number.ToString()];
    private static readonly string[] LeeVerbose = ["Lee", Number.ToString(), "--verbose"];
    private static readonly string[] VerboseLee = ["--verbose", "Lee", Number.ToString()];

    [Benchmark]
    public void SystemCommandLine() {
        TestCommandSystemCommandLine cmd = new();

        cmd.Parse(Lee).Invoke();
        cmd.Parse(LeeVerbose).Invoke();
        cmd.Parse(VerboseLee).Invoke();
    }

    [Benchmark]
    public void SpectreConsoleCli() {
        CommandApp<TestCommandSpectreConsoleCli> cmdapp = new();

        cmdapp.Run(Lee);
        cmdapp.Run(LeeVerbose);
        cmdapp.Run(VerboseLee);
    }

    [Benchmark]
    public async Task CliFx() {
        CliApplicationBuilder builder = new();
        var app = builder.SetExecutableName("test").AddCommand<TestCommandCliFx>().Build();

        await app.RunAsync(Lee);
        await app.RunAsync(LeeVerbose);
        ////await app.RunAsync(VerboseLee); // Not working
    }

    private static void Main() => BenchmarkRunner.Run<Benchmarks>();
}
