namespace Etude.Alpm;

[Flags]
public enum PkgValidation {
    Unknown = 0,
    None = 1 << 0,
    Md5Sum = 1 << 1,
    Sha256Sum = 1 << 2,
    Signature = 1 << 3
}
