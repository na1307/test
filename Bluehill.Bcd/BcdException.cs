namespace Bluehill.Bcd;

/// <summary>
/// BCD WMI Exception.
/// </summary>
public class BcdException : Exception {
    internal BcdException() : base("An error occurred during a BCD WMI operation.") { }
    internal BcdException(Exception innerException) : base("An error occurred during a BCD WMI operation.", innerException) { }
    internal BcdException(string message) : base(message) { }
    internal BcdException(string message, Exception innerException) : base(message, innerException) { }
}
