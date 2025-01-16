using Spectre.Console.Cli;

namespace Ndnm;

internal sealed class TypeResolver(IServiceProvider provider) : ITypeResolver {
    public object? Resolve(Type? type) => provider.GetService(type!);
}
