namespace Bluehill.Vhd;

internal static class NativeMethods {
    [DllImport("virtdisk.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
    public static extern uint CreateVirtualDisk(
        in VirtualStorageType virtualStorageType,
        [MarshalAs(UnmanagedType.LPWStr)] string path,
        VirtualDiskAccessMask virtualDiskAccessMask,
        IntPtr securityDescriptor,
        CreateVirtualDiskOptions flags,
        uint providerSpecificFlags,
        in CreateVirtualDiskParameters parameters,
        IntPtr overlapped,
        out SafeVirtualDiskHandle handle);

    [DllImport("virtdisk.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
    public static extern uint OpenVirtualDisk(
        in VirtualStorageType virtualStorageType,
        [MarshalAs(UnmanagedType.LPWStr)] string path,
        VirtualDiskAccessMask virtualDiskAccessMask,
        OpenVirtualDiskOptions flags,
        IntPtr parameters,
        out SafeVirtualDiskHandle handle);

    [DllImport("virtdisk.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
    public static extern uint AttachVirtualDisk(
        SafeVirtualDiskHandle virtualDiskHandle,
        IntPtr securityDescriptor,
        AttachVirtualDiskOptions flag,
        uint providerSpecificFlags,
        IntPtr parameters,
        IntPtr overlapped);

    [DllImport("virtdisk.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
    public static extern uint DetachVirtualDisk(
        SafeVirtualDiskHandle virtualDiskHandle,
        DetachVirtualDiskOptions flags,
        uint providerSpecificFlags);

    [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CloseHandle(IntPtr handle);
}
