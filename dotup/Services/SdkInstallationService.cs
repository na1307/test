using Dotup.Json.Registry;
using Dotup.Json.ReleasesIndex;
using System.Diagnostics;
using System.IO.Compression;
using System.Security.Cryptography;

namespace Dotup.Services;

internal sealed class SdkInstallationService(IRegistryService registryService, ITarExtractor tarExtractor)
    : ISdkInstallationService {
    public async Task<bool> InstallSdkAsync(
        HttpClient client,
        string? channel,
        long totalSize,
        DotnetFile sdkFile,
        DotupRegistry registry,
        SemVersion sdkVersion,
        SemVersion runtimeVersion,
        CancellationToken token = default)
        => await AnsiConsole.Progress()
            .Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new RemainingTimeColumn(), new SpinnerColumn())
            .StartAsync(async context => {
                const int bufferSize = 1024 * 1024;
                var downloadTask = context.AddTask("[green]Downloading .NET SDK[/]", maxValue: totalSize);
                var hashTask = context.AddTask("[blue]Calculating hash[/]", maxValue: totalSize);
                var extractTask = context.AddTask("[yellow]Extracting .NET SDK[/]");
                var fileExtension = GetFileExtension(sdkFile.Url);
                var randomFilePath = Path.Combine(DotupPath, Path.GetRandomFileName());
                var tempDirPath = Path.Combine(DotupPath, "temp");

                // Ensure random file is clean
                if (File.Exists(randomFilePath)) {
                    File.Delete(randomFilePath);
                }

                try {
                    await using (FileStream fs = new(randomFilePath, FileMode.CreateNew, FileAccess.Write, FileShare.None, bufferSize,
                                     true))
                    await using (var response = await client.GetStreamAsync(sdkFile.Url, token)) {
                        var stopwatch = Stopwatch.StartNew();
                        var buffer = new byte[bufferSize];
                        int bytesRead;
                        long totalBytesRead = 0;

                        while ((bytesRead = await response.ReadAsync(buffer, token)) > 0) {
                            // Write to file
                            await fs.WriteAsync(buffer.AsMemory(0, bytesRead), token);

                            totalBytesRead += bytesRead;
                            downloadTask.Value = totalBytesRead;

                            // Display download speed
                            var elapsed = stopwatch.Elapsed.TotalSeconds;

                            if (elapsed > 0) {
                                var speed = totalBytesRead / elapsed / 1024 / 1024; // MB/s
                                downloadTask.Description = $"[green]Downloading .NET SDK[/] ({speed:F1} MB/s)";
                            }
                        }
                    }

                    downloadTask.Value = totalSize;
                    byte[] computedHash;

                    await using (FileStream fs = new(randomFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, true))
                    await using (ReadProgressStream rps = new(fs, progress => hashTask.Value = progress)) {
                        computedHash = await SHA512.HashDataAsync(rps, token);
                    }

                    if (!computedHash.SequenceEqual(sdkFile.Sha512Hash)) {
                        AnsiConsole.MarkupLine("[red]Hash mismatch![/]");

                        return false;
                    }

                    AnsiConsole.MarkupLine("[green]Download completed successfully![/]");

                    if (Directory.Exists(tempDirPath)) {
                        Directory.Delete(tempDirPath, true);
                    }

                    Directory.CreateDirectory(tempDirPath);

                    await using (FileStream fs = new(randomFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize,
                                     true)) {
                        var fileSize = fs.Length;
                        extractTask.MaxValue = fileSize;

                        switch (fileExtension) {
                            case "tar.gz":
                                await using (GZipStream gzStream = new(fs, CompressionMode.Decompress))
                                await using (ReadProgressStream read = new(gzStream, progress => extractTask.Value = Math.Min(progress, fileSize))) {
                                    await tarExtractor.ExtractToDirectoryAsync(read, tempDirPath, true, token);
                                }

                                break;

                            default:
                                AnsiConsole.MarkupLine("[red]Something went wrong.[/]");
                                return false;
                        }

                        extractTask.Value = fileSize;
                    }

                    var sdkManifestsPath = Path.Combine(tempDirPath, SdkManifestsDirName);

                    var sdkManifests = Directory.Exists(sdkManifestsPath)
                        ? Directory.EnumerateDirectories(sdkManifestsPath).Select(d => SemVersion.Parse(Path.GetFileName(d))).ToArray()
                        : [];

                    var inputIsGreater = registry.CliVersion is null || SemVersionComparer.Default.Compare(sdkVersion, registry.CliVersion) > 0;
                    var instanceDir = Directory.CreateDirectory(InstancesPath);

                    // Recursive move using IFileSystem
                    foreach (var file in Directory.EnumerateFiles(tempDirPath, "*", SearchOption.AllDirectories)) {
                        var dest = Path.Combine(instanceDir.FullName, Path.GetRelativePath(tempDirPath, file));

                        Directory.CreateDirectory(Path.GetDirectoryName(dest)!);

                        if (inputIsGreater || !File.Exists(dest)) {
                            if (File.Exists(dest)) {
                                File.Delete(dest);
                            }

                            File.Move(file, dest);
                        }
                    }

                    await registryService.AddOrModifyRegistryAsync(channel ?? sdkVersion.ToString(), registry, sdkVersion, runtimeVersion,
                        sdkManifests, inputIsGreater, token);

                    AnsiConsole.MarkupLine("[green]Extraction completed successfully![/]");

                    return true;
                } finally {
                    if (Directory.Exists(tempDirPath)) {
                        Directory.Delete(tempDirPath, true);
                    }

                    if (File.Exists(randomFilePath)) {
                        File.Delete(randomFilePath);
                    }
                }
            });

    public async Task UninstallSdkAsync(string channel, DotupRegistry registry, bool removeFromRegistry, CancellationToken token = default) {
        var channels = registry.InstalledChannels;
        var uninstallChannel = channels[channel];
        var uninstallSdkVersion = uninstallChannel.SdkVersion;
        var uninstallRuntimeVersion = uninstallChannel.RuntimeVersion;
        var uninstallSdkManifestVersions = uninstallChannel.SdkManifests;
        var notUninstallSdk = channels.Values.Count(c => c.SdkVersion == uninstallSdkVersion) > 1;
        var notUninstallRuntime = channels.Values.Count(c => c.RuntimeVersion == uninstallRuntimeVersion) > 1;

        var notUninstallSdkManifests
            = uninstallSdkManifestVersions.ToDictionary(m => m, m => channels.Values.Count(c => c.SdkManifests.Contains(m)) > 1);

        try {
            if (notUninstallSdk) {
                if (removeFromRegistry) {
                    AnsiConsole.Markup(
                        $"[yellow]More than one channel is referencing the SDK {uninstallSdkVersion}. Just removing channel from registry...[/]");
                }

                return;
            }

            DeleteDirectoryIfExists(Path.Combine(InstancesPath, SdkDirName, uninstallSdkVersion.ToString()));

            foreach (var manifest in notUninstallSdkManifests.Where(m => !m.Value).Select(m => m.Key)) {
                DeleteDirectoryIfExists(Path.Combine(InstancesPath, SdkManifestsDirName, manifest.ToString()));
            }

            if (notUninstallRuntime) {
                return;
            }

            DeleteDirectoryIfExists(Path.Combine(InstancesPath, HostDirName, FxrDirName, uninstallRuntimeVersion.ToString()));

            var packsBasePath = Path.Combine(InstancesPath, PacksDirName);

            DeleteDirectoryIfExists(Path.Combine(packsBasePath, $"{NetCoreAppHostDirName}.{RuntimeIdentifier}", uninstallRuntimeVersion.ToString()));
            DeleteDirectoryIfExists(Path.Combine(packsBasePath, NetCoreAppRefDirName, uninstallRuntimeVersion.ToString()));
            DeleteDirectoryIfExists(Path.Combine(packsBasePath, AspNetCoreAppRefDirName, uninstallRuntimeVersion.ToString()));

            var sharedPath = Path.Combine(InstancesPath, SharedDirName);
            var netcoreappPath = Path.Combine(sharedPath, NetCoreAppDirName, uninstallRuntimeVersion.ToString());
            var aspnetcoreappPath = Path.Combine(sharedPath, AspNetCoreAppDirName, uninstallRuntimeVersion.ToString());
            var aspnetcoreallPath = Path.Combine(sharedPath, AspNetCoreAllDirName, uninstallRuntimeVersion.ToString());

            DeleteDirectoryIfExists(netcoreappPath);
            DeleteDirectoryIfExists(aspnetcoreappPath);
            DeleteDirectoryIfExists(aspnetcoreallPath);

            DeleteDirectoryIfExists(Path.Combine(InstancesPath, TemplatesDirName, uninstallRuntimeVersion.ToString()));
        } finally {
            if (removeFromRegistry) {
                await registryService.RemoveFromRegistryAsync(channel, registry, token);
            }
        }
    }

    private static string GetFileExtension(Uri url) {
        var extension = Path.GetExtension(url.LocalPath);

        return extension switch {
            ".gz" => "tar.gz",
            _ => extension.TrimStart('.')
        };
    }

    private void DeleteDirectoryIfExists(string path) {
        if (Directory.Exists(path)) {
            Directory.Delete(path, true);
        }
    }
}
