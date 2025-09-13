namespace SimpleHashTab;

[UnmanagedFunctionPointer(CallingConvention.StdCall)]
[return: MarshalAs(UnmanagedType.Bool)]
internal delegate bool LPFNADDPROPSHEETPAGE(IntPtr page, IntPtr lparam);
