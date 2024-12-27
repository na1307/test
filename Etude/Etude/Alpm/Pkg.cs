using System.Runtime.InteropServices;

namespace Etude.Alpm;

[StructLayout(LayoutKind.Sequential)]
public struct Pkg {
    public nuint name_hash;
    public string filename;
    public string @base;
    public string name;
    public string version;
    public string desc;
    public string url;
    public string packager;
    public string md5sum;
    public string sha256sum;
    public string base64_sig;
    public string arch;

    public long builddate;
    public long installdate;

    public int size;
    public int isize;
    public int download_size;

    public IntPtr handle;

    public IntPtr licenses;
    public IntPtr replaces;
    public IntPtr groups;
    public IntPtr backup;
    public IntPtr depends;
    public IntPtr optdepends;
    public IntPtr checkdepends;
    public IntPtr makedepends;
    public IntPtr conflicts;
    public IntPtr provides;
    public IntPtr removes;
    public IntPtr oldpkg;

    public FileList files;

    public PkgOriginData origin_data;

    public PkgFrom origin;
    public PkgReason reason;
    public int scriptlet;

    public IntPtr xdata;

    public int infolevel;
    public int validation;
}
