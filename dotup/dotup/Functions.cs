using Dotup.Json.ReleasesIndex;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization.Metadata;

namespace Dotup;

internal static class Functions {
    public static int ExitError(string[] messages, int exitCode = 1) {
        foreach (var message in messages) {
            AnsiConsole.MarkupLine($"[red]{message}[/]");
        }

        return exitCode;
    }

    public static int DotnetSdkNotInstalled() => ExitError(["Error: .NET SDK has not been installed", "Please install .NET SDK first."]);

    public static async Task<T?> GetCachedJsonAsync<T>(
        this HttpClient client,
        Uri url,
        JsonTypeInfo<T> jsonTypeInfo,
        CancellationToken cancellationToken = default) {
        if (!Directory.Exists(CachePath)) {
            Directory.CreateDirectory(CachePath);
        }

        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(url.ToString())));
        var cacheFilePath = Path.Combine(CachePath, hash);
        var lastModifiedFilePath = Path.Combine(CachePath, $"{hash}.lastmodified");
        using HttpRequestMessage request = new(HttpMethod.Get, url);

        if (File.Exists(cacheFilePath) && File.Exists(lastModifiedFilePath)) {
            var lastModified = await File.ReadAllTextAsync(lastModifiedFilePath, cancellationToken);

            request.Headers.IfModifiedSince = DateTimeOffset.Parse(lastModified, CultureInfo.InvariantCulture);
        }

        using var response = await client.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotModified) {
            await using FileStream fs = new(cacheFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);

            return await JsonSerializer.DeserializeAsync(fs, jsonTypeInfo, cancellationToken);
        }

        response.EnsureSuccessStatusCode();

        await using (FileStream fs = new(cacheFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true)) {
            await response.Content.CopyToAsync(fs, cancellationToken);
        }

        if (response.Content.Headers.LastModified is { } newLastModified) {
            await File.WriteAllTextAsync(lastModifiedFilePath, newLastModified.ToString("R"), cancellationToken);
        }

        await using (FileStream fs = new(cacheFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true)) {
            return await JsonSerializer.DeserializeAsync(fs, jsonTypeInfo, cancellationToken);
        }
    }

    public static DotnetSdk? ResolveSdkVersionPattern(string channel, DotnetChannel[] channels, DotnetSdk[] availableSdks) {
        // If the version is already exact, return it as-is
        if (SemVersion.TryParse(channel, out var sv)) {
            var sdkFromVersion = availableSdks.SingleOrDefault(s => s.Version == sv);

            if (sdkFromVersion is not null) {
                return sdkFromVersion;
            }

            var sdkFromDisplayVersion = availableSdks.SingleOrDefault(s => s.DisplayVersion == sv);

            if (sdkFromDisplayVersion is not null) {
                return sdkFromDisplayVersion;
            }

            var sdkFromRuntimeVersion = availableSdks.FirstOrDefault(s => s.RuntimeVersion is not null && s.RuntimeVersion == sv);

            if (sdkFromRuntimeVersion is not null) {
                return sdkFromRuntimeVersion;
            }
        }

        var latestPreviewChannel = channels.FirstOrDefault(c => c.SupportPhase == SupportPhase.Preview);

        // ‘preview’ pattern when preview is avaliable
        if (channel.Equals("preview", StringComparison.OrdinalIgnoreCase) && latestPreviewChannel is not null) {
            return availableSdks.SingleOrDefault(s => latestPreviewChannel.LatestSdk == s.Version);
        }

        // ‘latest’ pattern and 'preview' pattern when preview is not avaliable
        if (channel.Equals("latest", StringComparison.OrdinalIgnoreCase)
            || (channel.Equals("preview", StringComparison.OrdinalIgnoreCase) && latestPreviewChannel is null)) {
            var latestStableChannel = channels.First(c => c.SupportPhase == SupportPhase.Active);

            return availableSdks.SingleOrDefault(s => latestStableChannel.LatestSdk == s.Version);
        }

        // ‘lts’ pattern (8.0, 10.0 are LTS)
        if (channel.Equals("lts", StringComparison.OrdinalIgnoreCase)) {
            var latestLtsChannel = channels.First(c => c is { SupportPhase: SupportPhase.Active, ReleaseType: ReleaseType.Lts });

            return availableSdks.SingleOrDefault(s => latestLtsChannel.LatestSdk == s.Version);
        }

        // Single number (e.g., "10" -> latest 10.x.x)
        if (int.TryParse(channel, out var majorOnly)) {
            return availableSdks.FirstOrDefault(s => s.Version.Major == majorOnly);
        }

        // Handle 10.0.x pattern
        if (channel.EndsWith(".x")) {
            var prefix = channel[..^2]; // Remove ".x" suffix
            var parts = prefix.Split('.');

            if (parts.Length == 2 && int.TryParse(parts[0], out var major) && int.TryParse(parts[1], out var minor)) {
                return availableSdks.FirstOrDefault(s => {
                    var v = s.Version;

                    return v.Major == major && v.Minor == minor;
                }); // Already sorted in descending order
            }
        }

        // Handle 10.0.1xx pattern
        if (channel.EndsWith("xx")) {
            var prefix = channel[..^2]; // Remove "xx" suffix
            var parts = prefix.Split('.');

            if (parts.Length == 3 &&
                int.TryParse(parts[0], out var major) &&
                int.TryParse(parts[1], out var minor) &&
                int.TryParse(parts[2], out var patchPrefix)) {
                // e.g., 10.0.1xx -> search within 100–199 range
                var minPatch = patchPrefix * 100;
                var maxPatch = minPatch + 99;

                return availableSdks.FirstOrDefault(s => {
                    var v = s.Version;

                    return v.Major == major && v.Minor == minor && v.Patch >= minPatch && v.Patch <= maxPatch;
                });
            }
        }

        // No matches
        return null;
    }

    public static bool IsAppropriateFile(DotnetFile file) {
        var fileExtension = Path.GetExtension(file.Url.LocalPath);

#pragma warning disable IDE0066
        switch (OSName) {
#pragma warning restore IDE0066
            case "linux" when fileExtension == ".gz":
                return true;

            default:
                return false;
        }
    }
}
