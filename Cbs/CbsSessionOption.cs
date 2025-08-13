namespace WA.Cbs;

internal enum CbsSessionOption : uint {
    None = 0,
    LoadPersisted = 0x80,
    DoScavenge = 0x400,
    CancelAllPendedTransactions = 0x800,
    EnableCompression = 0x2000,
    DisableCompression = 0x4000,
    DetectAndRepairStoreCorruption = 0xC000,
    ReportStackInfo = 0x100000,
    DoSynchronousCleanup = 0x400000,
    AnalyzeComponentStore = 0x8000000,
    CancelOnlySmartPendedTransactions = 0x80000000
}
