using System.IO.MemoryMappedFiles;
using System.Text;

namespace WindowsService;

public class Worker(ILogger<Worker> logger) : BackgroundService {
    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        using var mmf = MemoryMappedFile.CreateNew("Global\\TestServiceMMF", 1024);
        using EventWaitHandle ewh = new(false, EventResetMode.ManualReset, "Global\\TestServiceEWH");

        try {
            while (!stoppingToken.IsCancellationRequested) {
                if (!ewh.WaitOne(TimeSpan.FromSeconds(30))) {
                    Environment.Exit(0);
                }

                string fulltext;

                using (StreamReader sr = new(mmf.CreateViewStream(), Encoding.UTF8)) {
                    fulltext = await sr.ReadToEndAsync(stoppingToken);
                }

                if (!string.IsNullOrWhiteSpace(fulltext)) {
                    logger.LogInformation("{Text}", fulltext);
                }

                ewh.Reset();
            }
        } catch (OperationCanceledException) {
            // When the stopping token is canceled, for example, a call made from services.msc,
            // we shouldn't exit with a non-zero exit code. In other words, this is expected...
        } catch (Exception ex) {
            logger.LogError(ex, "{Message}", ex.Message);

            // Terminates this process and returns an exit code to the operating system.
            // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
            // performs one of two scenarios:
            // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
            // 2. When set to "StopHost": will cleanly stop the host, and log errors.
            //
            // In order for the Windows Service Management system to leverage configured
            // recovery options, we need to terminate the process with a non-zero exit code.
            Environment.Exit(1);
        }
    }
}
