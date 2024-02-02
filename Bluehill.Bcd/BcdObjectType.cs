namespace Bluehill.Bcd;

public enum BcdObjectType {
    FirmwareBootManager = 0x10100001,
    BootManager = 0x10100002,
    BootLoader = 0x10200003,
    MemoryTester = 0x10200005,
    Ntldr = 0x10300006,
    Device = 0x30000000,
}
