namespace Dotup.Services;

internal interface ITarExtractor {
    Task ExtractToDirectoryAsync(Stream source, string destinationDirectoryName, bool overwriteFiles, CancellationToken cancellationToken = default);
}
