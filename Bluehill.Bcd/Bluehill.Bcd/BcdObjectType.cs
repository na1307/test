namespace Bluehill.Bcd;

/// <summary>
/// BcdObjectType
/// </summary>
public enum BcdObjectType {
    /// <summary>
    /// fwbootmgr
    /// </summary>
    FirmwareBootManager = 0x10100001,

    /// <summary>
    /// bootmgr
    /// </summary>
    BootManager = 0x10100002,

    /// <summary>
    /// OSLoader
    /// </summary>
    BootLoader = 0x10200003,

    /// <summary>
    /// Memory Diagnostic
    /// </summary>
    MemoryTester = 0x10200005,

    /// <summary>
    /// NTLDR
    /// </summary>
    Ntldr = 0x10300006,

    /// <summary>
    /// Device
    /// </summary>
    Device = 0x30000000,
}
