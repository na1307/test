using System.Runtime.InteropServices;
using System.Text;

namespace WA.Cbs;

[ComImport]
[Guid("75207391-23f2-4396-85f0-8fdb879ed0ed")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface ICbsSession {
    void Initialize(
        CbsSessionOption sessionOptions,
        [MarshalAs(UnmanagedType.LPWStr)] string clientID,
        [MarshalAs(UnmanagedType.LPWStr)] string? bootDrive,
        [MarshalAs(UnmanagedType.LPWStr)] string? winDir);

    CbsRequiredAction Finalize();

    unsafe void* /* ICbsPackage* */ CreatePackage(
        uint options,
        CbsPackageType packageType,
        [MarshalAs(UnmanagedType.LPWStr)] string pkgPath,
        [MarshalAs(UnmanagedType.LPWStr)] string sandboxPath);

    unsafe void* /* ICbsPackage* */ OpenPackage(
        uint options,
        void* /* ICbsIdentity* */ packageIdentity,
        [MarshalAs(UnmanagedType.LPWStr)] string unkArgAboutLog);

    unsafe void* /* IEnumCbsIdentity* */ EnumeratePackages(uint options);

    unsafe void* /* ICbsIdentity* */ CreateCbsIdentity();

    [PreserveSig]
    int GetStatus(
        out uint currentPhase,
        out CbsSessionState lastSuccessfulSessionState,
        [MarshalAs(UnmanagedType.Bool)] out bool completed,
        out int status);

    unsafe void Resume(void* /* ICbsUIHandler* */ uiHandler);

    void GetSessionId([MarshalAs(UnmanagedType.LPWStr)] StringBuilder id);

    [return: MarshalAs(UnmanagedType.LPWStr)]
    StringBuilder GetProperty(CbsSessionProperty prop);

    void AddPhaseBreak();

    [Obsolete("Unknown function", true)]
    CbsRequiredAction FinalizeEx(uint unknown);

    void AddSource(uint options, [MarshalAs(UnmanagedType.LPWStr)] string basePath);

    unsafe void RegisterCbsUIHandler(void* /* ICbsUIHandler* */ uiHandler);

    [Obsolete("Unknown function", true)]
    [PreserveSig]
    unsafe int CreateWindowsUpdatePackage(
        uint unknown1,
        [MarshalAs(UnmanagedType.LPWStr)] string unknown2,
        Guid unknown3,
        uint unknown4,
        CbsPackageType unknown5,
        [MarshalAs(UnmanagedType.LPWStr)] string unknown6,
        [MarshalAs(UnmanagedType.LPWStr)] string unknown7,
        uint unknown8,
        out int unknown9,
        int unknown10,
        out void* /* ICbsPackage* */ unknown11);

    unsafe void* /* IEnumCbsCapability* */ EnumerateCapabilities(
        uint sourceFilter,
        [MarshalAs(UnmanagedType.LPWStr)] string @namespace,
        [MarshalAs(UnmanagedType.LPWStr)] string lang,
        [MarshalAs(UnmanagedType.LPWStr)] string arch,
        uint major,
        uint minor);

    void InitializeEx(
        uint sessionOptions,
        [MarshalAs(UnmanagedType.LPWStr)] string sourceName,
        [MarshalAs(UnmanagedType.LPWStr)] string bootDrive,
        [MarshalAs(UnmanagedType.LPWStr)] string winDir,
        [MarshalAs(UnmanagedType.LPWStr)] string externalDir);

    unsafe void* /* ICSIExternalTransformerExecutor* */ CreateExternalTransformerExecutor();

    unsafe void* /* IEnumCbsSession* */ ObserveSessions(uint options, void* /* ICbsSessionObserverListener* */ listener);

    unsafe void* /* IEnumCbsActivity* */ GetActivities(long options);

    void SetEnhancedOptions(CbsSessionEnhancedOption enhancedOptions);

    void SetProperty(int prop, [MarshalAs(UnmanagedType.LPWStr)] string value);

    void PerformOperation(uint reserved, CbsOperationType type);

    [Obsolete("Unknown function", true)]
    void SetClientToken(long unknown);

    int QuerySessionStatus([MarshalAs(UnmanagedType.LPWStr)] string sessionID);

    [PreserveSig]
    unsafe int Process(
        in uint servicingProcessorOptions,
        [MarshalAs(UnmanagedType.LPWStr)] string actionListPath,
        [MarshalAs(UnmanagedType.LPWStr)] string sandboxPath,
        [MarshalAs(UnmanagedType.LPWStr)] string clientID,
        void* /* ICbsUIHandler* */ uiHandler,
        out CbsRequiredAction requiredAction,
        [MarshalAs(UnmanagedType.LPWStr)] out StringBuilder sessionID);

    void WritePackageFileList(
        in uint servicingProcessorOptions,
        [MarshalAs(UnmanagedType.LPWStr)] string actionListPath,
        [MarshalAs(UnmanagedType.LPWStr)] string sandboxPath,
        [MarshalAs(UnmanagedType.LPWStr)] string fileListPath,
        [MarshalAs(UnmanagedType.LPWStr)] string clientID,
        [MarshalAs(UnmanagedType.LPWStr)] string packageFilePath);
}
