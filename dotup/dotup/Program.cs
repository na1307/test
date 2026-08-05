using Dotup.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Dotup;

internal static class Program {
    private static async Task Main(string[] args) {
        IDisposable? singleInstance;
        const string dotupIsAlreadyRunning = "Dotup is already running.";

        if (OperatingSystem.IsWindows()) {
#pragma warning disable CA2000
            Mutex mutex = new(true, "Global\\DotupSingleInstance", out var createdNew);
#pragma warning restore CA2000

            if (!createdNew) {
                await Console.Error.WriteLineAsync(dotupIsAlreadyRunning);

                Environment.ExitCode = 1;

                return;
            }

            singleInstance = mutex;
        } else {
            // Native AOT on Linux has issues with named Mutexes (they are often process-local).
            // Use a file lock in the temp directory instead.
            var lockPath = Path.Combine(Path.GetTempPath(), $"dotup-{Environment.UserName}.lock");

            try {
                // FileShare.None prevents other processes from opening the file
#pragma warning disable CA2000
                FileStream fs = new(lockPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 1, FileOptions.DeleteOnClose);
#pragma warning restore CA2000
                singleInstance = fs;
            } catch (IOException) {
                await Console.Error.WriteLineAsync(dotupIsAlreadyRunning);

                Environment.ExitCode = 1;

                return;
            }
        }

        try {
            // On some Linux installations, the local share folder may not exist,
            // so Environment.SpecialFolder.LocalApplicationData may return your home directory.
            // To prevent this, create it in advance.
            if (OperatingSystem.IsLinux()) {
                var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

                Directory.CreateDirectory(Path.Combine(userProfile, ".local"));
                Directory.CreateDirectory(Path.Combine(userProfile, ".local", "share"));
            }

            await ConsoleApp.Create().ConfigureServices(static services => {
                services.AddSingleton<HttpClient>();
                services.AddSingleton<ITarExtractor, TarExtractor>();
                services.AddSingleton<IRegistryService, RegistryService>();
                services.AddSingleton<ISdkInstallationService, SdkInstallationService>();
            }).RunAsync(args);
        } finally {
            singleInstance.Dispose();
        }
    }
}
