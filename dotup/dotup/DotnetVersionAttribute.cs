using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace Dotup;

[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class DotnetVersionAttribute(bool required) : ValidationAttribute {
    private static readonly Regex VersionRegex = new(@"^([0-9]+(\.[0-9]+\.([0-9]{1,3}|[0-9]xx|x))?|lts|latest|preview)$");

    public bool Required => required;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        var version = (string?)value;

        if (version is not null) {
            if (!VersionRegex.IsMatch(version)) {
                return new("""
                           Invalid version format. Please use the following format:
                             - 10.0.100 (exact version)
                             - 10.0.0 (exact release version, the latest SDK for the release)
                             - 10.0.1xx (latest patch of major.minor.feature band)
                             - 10.0.x (latest feature band and patch of major.minor)
                             - 10 (latest version of major version)
                             - lts (latest long-term support version)
                             - latest (latest stable version)
                             - preview (latest preview version)
                           """);
            }
        } else if (required) {
            return new("Version is required.");
        }

        return ValidationResult.Success;
    }
}
