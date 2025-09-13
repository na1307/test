namespace SimpleHashTab;

[UnmanagedFunctionPointer(CallingConvention.StdCall)]
internal delegate IntPtr DLGPROC(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);
