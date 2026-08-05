using System.Formats.Tar;

namespace Dotup.Services;

internal sealed class TarExtractor : ITarExtractor {
    public Task ExtractToDirectoryAsync(
        Stream source,
        string destinationDirectoryName,
        bool overwriteFiles,
        CancellationToken cancellationToken = default)
        => TarFile.ExtractToDirectoryAsync(source, destinationDirectoryName, overwriteFiles, cancellationToken);
}
