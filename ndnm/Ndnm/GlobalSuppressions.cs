// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Performance", "CA1819:속성은 배열을 반환하지 않아야 합니다.", Justification = "JSON", Scope = "member",
    Target = "~P:Ndnm.DotNetChannel.Releases"
)]
[assembly: SuppressMessage(
    "Performance", "CA1819:속성은 배열을 반환하지 않아야 합니다.", Justification = "JSON", Scope = "member",
    Target = "~P:Ndnm.DotNetRelease.Sdks"
)]
[assembly: SuppressMessage(
    "Performance", "CA1819:속성은 배열을 반환하지 않아야 합니다.", Justification = "JSON", Scope = "member",
    Target = "~P:Ndnm.DotNetSdkRelease.Files"
)]
