// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "Roslynator", "RCS1135:Declare enum member with zero value (when enum has FlagsAttribute)", Scope = "type",
    Target = "~T:Etude.Alpm.DbUsage"
)]

[assembly: SuppressMessage(
    "Critical Code Smell", "S2346:Flags enumerations zero-value members should be named \"None\"",
    Scope = "type", Target = "~T:Etude.Alpm.PkgValidation"
)]
