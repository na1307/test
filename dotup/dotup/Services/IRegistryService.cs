using Dotup.Json.Registry;

namespace Dotup.Services;

internal interface IRegistryService {
    Task<DotupRegistry?> GetRegistryAsync(CancellationToken token = default);

    Task<bool> IsChannelAlreadyInstalledAsync(string channel, CancellationToken token = default);

    Task AddOrModifyRegistryAsync(
        string channel,
        DotupRegistry registry,
        SemVersion sdkVersion,
        SemVersion runtimeVersion,
        SemVersion[] sdkManifests,
        bool inputIsGreater,
        CancellationToken token = default);

    Task RemoveFromRegistryAsync(string channel, DotupRegistry registry, CancellationToken token = default);
}
