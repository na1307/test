namespace BookLibrary;

public class ProgramException : Exception {
    public ProgramException(string message) : base(message) { }
    public ProgramException(string message, Exception innerException) : base(message, innerException) { }
}