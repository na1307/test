using Dotup.Json.Registry;
using Dotup.Json.ReleasesIndex;

namespace Dotup.Services;

internal interface ISdkInstallationService {
    Task<bool> InstallSdkAsync(
        HttpClient client,
        string? channel,
        long totalSize,
        DotnetFile sdkFile,
        DotupRegistry registry,
        SemVersion sdkVersion,
        SemVersion runtimeVersion,
        CancellationToken token = default);

    Task UninstallSdkAsync(
        string channel,
        DotupRegistry registry,
        bool removeFromRegistry,
        CancellationToken token = default);
}
