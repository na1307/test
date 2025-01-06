using Spectre.Console.Cli;

namespace Dnvm;

internal sealed class TypeResolver(IServiceProvider provider) : ITypeResolver {
    public object? Resolve(Type? type) => provider.GetService(type!);
}
