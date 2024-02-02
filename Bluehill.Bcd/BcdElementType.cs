﻿namespace Bluehill.Bcd;

public enum BcdElementType {
    // BcdLibraryElementTypes
    BcdLibraryApplicationDevice = 0x11000001,
    BcdLibraryApplicationPath = 0x12000002,
    BcdLibraryDescription = 0x12000004,
    BcdLibraryPreferredLocale = 0x12000005,
    BcdLibraryInheritedObjects = 0x14000006,
    BcdLibraryTruncatePhysicalMemory = 0x15000007,
    BcdLibraryRecoverySequence = 0x14000008,
    BcdLibraryAutoRecoveryEnabled = 0x16000009,
    BcdLibraryBadMemoryList = 0x1700000a,
    BcdLibraryAllowBadMemoryAccess = 0x1600000b,
    BcdLibraryFirstMegabytePolicy = 0x1500000c,
    BcdLibraryRelocatePhysicalMemory = 0x1500000D,
    BcdLibraryAvoidLowPhysicalMemory = 0x1500000E,
    BcdLibraryDebuggerEnabled = 0x16000010,
    BcdLibraryDebuggerType = 0x15000011,
    BcdLibrarySerialDebuggerPortAddress = 0x15000012,
    BcdLibrarySerialDebuggerPort = 0x15000013,
    BcdLibrarySerialDebuggerBaudRate = 0x15000014,
    BcdLibrary1394DebuggerChannel = 0x15000015,
    BcdLibraryUsbDebuggerTargetName = 0x12000016,
    BcdLibraryDebuggerIgnoreUsermodeExceptions = 0x16000017,
    BcdLibraryDebuggerStartPolicy = 0x15000018,
    BcdLibraryDebuggerBusParameters = 0x12000019,
    BcdLibraryDebuggerNetHostIP = 0x1500001A,
    BcdLibraryDebuggerNetPort = 0x1500001B,
    BcdLibraryDebuggerNetDhcp = 0x1600001C,
    BcdLibraryDebuggerNetKey = 0x1200001D,
    BcdLibraryEmsEnabled = 0x16000020,
    BcdLibraryEmsPort = 0x15000022,
    BcdLibraryEmsBaudRate = 0x15000023,
    BcdLibraryLoadOptionsString = 0x12000030,
    BcdLibraryDisplayAdvancedOptions = 0x16000040,
    BcdLibraryDisplayOptionsEdit = 0x16000041,
    BcdLibraryBsdLogDevice = 0x11000043,
    BcdLibraryBsdLogPath = 0x12000044,
    BcdLibraryGraphicsModeDisabled = 0x16000046,
    BcdLibraryConfigAccessPolicy = 0x15000047,
    BcdLibraryDisableIntegrityChecks = 0x16000048,
    BcdLibraryAllowPrereleaseSignatures = 0x16000049,
    BcdLibraryFontPath = 0x1200004A,
    BcdLibrarySiPolicy = 0x1500004B,
    BcdLibraryFveBandId = 0x1500004C,
    BcdLibraryConsoleExtendedInput = 0x16000050,
    BcdLibraryGraphicsResolution = 0x15000052,
    BcdLibraryRestartOnFailure = 0x16000053,
    BcdLibraryGraphicsForceHighestMode = 0x16000054,
    BcdLibraryIsolatedExecutionContext = 0x16000060,
    BcdLibraryBootUxDisable = 0x1600006C,
    BcdLibraryBootShutdownDisabled = 0x16000074,
    BcdLibraryAllowedInMemorySettings = 0x17000077,
    BcdLibraryForceFipsCrypto = 0x16000079,

    // BcdBootMgrElementTypes
    BcdBootMgrDisplayOrder = 0x24000001,
    BcdBootMgrBootSequence = 0x24000002,
    BcdBootMgrDefaultObject = 0x23000003,
    BcdBootMgrTimeout = 0x25000004,
    BcdBootMgrAttemptResume = 0x26000005,
    BcdBootMgrResumeObject = 0x23000006,
    BcdBootMgrToolsDisplayOrder = 0x24000010,
    BcdBootMgrDisplayBootMenu = 0x26000020,
    BcdBootMgrNoErrorDisplay = 0x26000021,
    BcdBootMgrBcdDevice = 0x21000022,
    BcdBootMgrBcdFilePath = 0x22000023,
    BcdBootMgrProcessCustomActionsFirst = 0x26000028,
    BcdBootMgrCustomActionsList = 0x27000030,
    BcdBootMgrPersistBootSequence = 0x26000031,

    // BcdOSLoaderElementTypes
    BcdOSLoaderOSDevice = 0x21000001,
    BcdOSLoaderSystemRoot = 0x22000002,
    BcdOSLoaderAssociatedResumeObject = 0x23000003,
    BcdOSLoaderDetectKernelAndHal = 0x26000010,
    BcdOSLoaderKernelPath = 0x22000011,
    BcdOSLoaderHalPath = 0x22000012,
    BcdOSLoaderDbgTransportPath = 0x22000013,
    BcdOSLoaderNxPolicy = 0x25000020,
    BcdOSLoaderPAEPolicy = 0x25000021,
    BcdOSLoaderWinPEMode = 0x26000022,
    BcdOSLoaderDisableCrashAutoReboot = 0x26000024,
    BcdOSLoaderUseLastGoodSettings = 0x26000025,
    BcdOSLoaderAllowPrereleaseSignatures = 0x26000027,
    BcdOSLoaderNoLowMemory = 0x26000030,
    BcdOSLoaderRemoveMemory = 0x25000031,
    BcdOSLoaderIncreaseUserVa = 0x25000032,
    BcdOSLoaderUseVgaDriver = 0x26000040,
    BcdOSLoaderDisableBootDisplay = 0x26000041,
    BcdOSLoaderDisableVesaBios = 0x26000042,
    BcdOSLoaderDisableVgaMode = 0x26000043,
    BcdOSLoaderClusterModeAddressing = 0x25000050,
    BcdOSLoaderUsePhysicalDestination = 0x26000051,
    BcdOSLoaderRestrictApicCluster = 0x25000052,
    BcdOSLoaderUseLegacyApicMode = 0x26000054,
    BcdOSLoaderX2ApicPolicy = 0x25000055,
    BcdOSLoaderUseBootProcessorOnly = 0x26000060,
    BcdOSLoaderNumberOfProcessors = 0x25000061,
    BcdOSLoaderForceMaximumProcessors = 0x26000062,
    BcdOSLoaderProcessorConfigurationFlags = 0x25000063,
    BcdOSLoaderMaximizeGroupsCreated = 0x26000064,
    BcdOSLoaderForceGroupAwareness = 0x26000065,
    BcdOSLoaderGroupSize = 0x25000066,
    BcdOSLoaderUseFirmwarePciSettings = 0x26000070,
    BcdOSLoaderMsiPolicy = 0x25000071,
    BcdOSLoaderSafeBoot = 0x25000080,
    BcdOSLoaderSafeBootAlternateShell = 0x26000081,
    BcdOSLoaderBootLogInitialization = 0x26000090,
    BcdOSLoaderVerboseObjectLoadMode = 0x26000091,
    BcdOSLoaderKernelDebuggerEnabled = 0x260000a0,
    BcdOSLoaderDebuggerHalBreakpoint = 0x260000a1,
    BcdOSLoaderUsePlatformClock = 0x260000A2,
    BcdOSLoaderForceLegacyPlatform = 0x260000A3,
    BcdOSLoaderTscSyncPolicy = 0x250000A6,
    BcdOSLoaderEmsEnabled = 0x260000b0,
    BcdOSLoaderDriverLoadFailurePolicy = 0x250000c1,
    BcdOSLoaderBootMenuPolicy = 0x250000C2,
    BcdOSLoaderAdvancedOptionsOneTime = 0x260000C3,
    BcdOSLoaderBootStatusPolicy = 0x250000E0,
    BcdOSLoaderDisableElamDrivers = 0x260000E1,
    BcdOSLoaderHypervisorLaunchType = 0x250000F0,
    BcdOSLoaderHypervisorDebuggerEnabled = 0x260000F2,
    BcdOSLoaderHypervisorDebuggerType = 0x250000F3,
    BcdOSLoaderHypervisorDebuggerPortNumber = 0x250000F4,
    BcdOSLoaderHypervisorDebuggerBaudrate = 0x250000F5,
    BcdOSLoaderHypervisorDebugger1394Channel = 0x250000F6,
    BcdOSLoaderBootUxPolicy = 0x250000F7,
    BcdOSLoaderHypervisorDebuggerBusParams = 0x220000F9,
    BcdOSLoaderHypervisorNumProc = 0x250000FA,
    BcdOSLoaderHypervisorRootProcPerNode = 0x250000FB,
    BcdOSLoaderHypervisorUseLargeVTlb = 0x260000FC,
    BcdOSLoaderHypervisorDebuggerNetHostIp = 0x250000FD,
    BcdOSLoaderHypervisorDebuggerNetHostPort = 0x250000FE,
    BcdOSLoaderTpmBootEntropyPolicy = 0x25000100,
    BcdOSLoaderHypervisorDebuggerNetKey = 0x22000110,
    BcdOSLoaderHypervisorDebuggerNetDhcp = 0x26000114,
    BcdOSLoaderHypervisorIommuPolicy = 0x25000115,
    BcdOSLoaderXSaveDisable = 0x2500012b,

    // BcdMemDiagElementTypes
    BcdMemDiagPassCount = 0x25000001,
    BcdMemDiagFailureCount = 0x25000003,

    // BcdDeviceObjectElementTypes
    BcdDeviceRamdiskImageOffset = 0x35000001,
    BcdDeviceTftpClientPort = 0x35000002,
    BcdDeviceSdiDevice = 0x31000003,
    BcdDeviceSdiPath = 0x32000004,
    BcdDeviceRamdiskImageLength = 0x35000005,
    BcdDeviceRamdiskExportAsCd = 0x36000006,
    BcdDeviceRamdiskTftpBlockSize = 0x36000007,
    BcdDeviceRamdiskTftpWindowSize = 0x36000008,
    BcdDeviceRamdiskMulticastEnabled = 0x36000009,
    BcdDeviceRamdiskMulticastTftpFallback = 0x3600000A,
    BcdDeviceRamdiskTftpVarWindow = 0x3600000B,
}
