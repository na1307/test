using System.Diagnostics;

await run("gpg-agent");
await run("keyboxd");

async Task run(string processName) {
    var attempt = 0;
    int code;

    do {
        attempt++;
        Console.Write($"Starting {processName} (Attempt {attempt})...");

        using Process gpgconf = new() {
            StartInfo = new() {
                FileName = "gpgconf.exe",
                Arguments = $"--launch {processName}",
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        gpgconf.Start();
        await gpgconf.WaitForExitAsync();
        code = gpgconf.ExitCode;
        Console.WriteLine();
    } while (code != 0);
}
