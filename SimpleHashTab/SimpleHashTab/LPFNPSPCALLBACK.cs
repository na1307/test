namespace SimpleHashTab;

[UnmanagedFunctionPointer(CallingConvention.StdCall)]
internal delegate uint LPFNPSPCALLBACK(IntPtr hWnd, uint uMsg, ref PROPSHEETPAGEW ppsp);
