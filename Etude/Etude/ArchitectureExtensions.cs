using System.Runtime.InteropServices;

namespace Etude;

public static class ArchitectureExtensions {
    public static string ToAlpmArchitecture(this Architecture arch) => arch switch {
        Architecture.X64 => "x86_64",
        Architecture.Arm64 => "aarch64",
        _ => "Unknown",
    };

    public static Architecture FromAlpmArchitecture(string alpmArch) => alpmArch switch {
        "x86_64" => Architecture.X64,
        "aarch64" => Architecture.Arm64,
        "auto" => RuntimeInformation.OSArchitecture,
        _ => throw new ArgumentException("Unknown architecture", nameof(alpmArch))
    };
}
