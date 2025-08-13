namespace WA.Cbs;

internal enum CbsSessionProperty {
    RebootRequired = 1,
    ErrorDetail = 2,
    Serviceable = 3,
    CompressionEnabled = 4,
    Report = 5,
    CorruptionFlag = 6,

    VolatileSize = 8,
    NonVolatileSize = 9,
    SharedWithWindowsSize = 10,
    AccordingToExplorer = 11,
    LastScavengeDatetime = 12,
    SupersededPackageCount = 13,

    SessionCompletionDatatime = 15,
    PackageMinSize = 16,
    // ?
    TotalPackageMinSize = 18,

    RepairNeeded = 22,
    ReOffer = 23
}
