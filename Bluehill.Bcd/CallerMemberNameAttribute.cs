#if !NET45_OR_GREATER && !NETSTANDARD1_0_OR_GREATER && !NETCOREAPP1_0_OR_GREATER
namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
internal sealed class CallerMemberNameAttribute : Attribute;
#endif
