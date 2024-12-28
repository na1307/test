namespace Bluehill.Vhd;

/// <summary>
/// Operation failed.
/// </summary>
public sealed class VhdOperationFailedException : Exception {
    internal VhdOperationFailedException() : base("Operation failed.") { }
    internal VhdOperationFailedException(string message) : base(message) { }
    internal VhdOperationFailedException(string message, Exception innerException) : base(message, innerException) { }
}
