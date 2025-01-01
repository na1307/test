namespace Bluehill.Bcd;

/// <summary>
/// Requires administrator privileges exception.
/// </summary>
public class RequiresAdministratorException : BcdException {
    internal RequiresAdministratorException() : base("Requires administrator privileges.") { }
}
