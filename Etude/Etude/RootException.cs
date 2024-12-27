namespace Etude;

public sealed class RootException : Exception {
    public RootException() : this("This user is not root") { }
    public RootException(string message) : base(message) { }
    public RootException(string message, Exception inner) : base(message, inner) { }
}
