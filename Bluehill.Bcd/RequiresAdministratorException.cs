namespace Bluehill.Bcd;

public class RequiresAdministratorException : BcdException {
    internal RequiresAdministratorException() : base("Requires administrator privileges.") { }
}
