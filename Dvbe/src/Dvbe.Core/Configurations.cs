namespace Dvbe;

public abstract class Configurations {
    private protected Configurations() { }

    internal bool IsConfigured {
        get;
        set {
            if (field && !value) {
                throw new ArgumentException("Cannot reset value.", nameof(value));
            }

            field = value;
        }
    }

    protected void ThrowIfConfigured() {
        if (IsConfigured) {
            throw new InvalidOperationException("Already configured.");
        }
    }
}
