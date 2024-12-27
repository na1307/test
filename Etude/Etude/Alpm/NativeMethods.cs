using System.Runtime.InteropServices;

namespace Etude.Alpm;

internal static partial class NativeMethods {
    private const string libalpm = "alpm";

    [LibraryImport(libalpm, EntryPoint = "alpm_initialize", StringMarshalling = StringMarshalling.Utf8)]
    public static partial IntPtr Initialize(string root, string dbpath, out ErrNo err);

    [LibraryImport(libalpm, EntryPoint = "alpm_release")]
    public static partial int Release(IntPtr handle);

    [LibraryImport(libalpm, EntryPoint = "alpm_get_localdb")]
    public static partial IntPtr GetLocaldb(IntPtr handle);

    [LibraryImport(libalpm, EntryPoint = "alpm_get_syncdbs")]
    public static partial IntPtr GetSyncdbs(IntPtr handle);

    [LibraryImport(libalpm, EntryPoint = "alpm_register_syncdb", StringMarshalling = StringMarshalling.Utf8)]
    public static partial IntPtr RegisterSyncdb(IntPtr handle, string treename, int level);

    [LibraryImport(libalpm, EntryPoint = "alpm_db_set_usage")]
    public static partial int DbSetUsage(IntPtr db, DbUsage usage);

    [LibraryImport(libalpm, EntryPoint = "alpm_db_set_servers")]
    public static partial int DbSetServers(IntPtr db, IntPtr servers);

    [LibraryImport(libalpm, EntryPoint = "alpm_db_add_server", StringMarshalling = StringMarshalling.Utf8)]
    public static partial int DbAddServer(IntPtr db, string url);

    [LibraryImport(libalpm, EntryPoint = "alpm_db_get_pkg", StringMarshalling = StringMarshalling.Utf8)]
    public static partial IntPtr DbGetPkg(IntPtr db, string name);

    [LibraryImport(libalpm, EntryPoint = "alpm_db_update")]
    public static partial int DbUpdate(IntPtr handle, IntPtr dbs, [MarshalAs(UnmanagedType.I4)] bool force);

    [LibraryImport(libalpm, EntryPoint = "alpm_errno")]
    public static partial ErrNo ErrNo(IntPtr handle);

    [LibraryImport(libalpm, EntryPoint = "alpm_strerror")]
    public static partial IntPtr StrError(ErrNo err);
}
