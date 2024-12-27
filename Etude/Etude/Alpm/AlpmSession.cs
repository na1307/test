using System.Runtime.InteropServices;

namespace Etude.Alpm;

public sealed class AlpmSession : IDisposable {
    private readonly IntPtr handle;
    private readonly PacmanConfig config;

    public AlpmSession(PacmanConfig config) {
        this.config = config;
        var returned = NativeMethods.Initialize(config.RootDir, config.DbPath, out var err);

        if (err != ErrNo.Ok) {
            throw new InvalidOperationException(
                $"Could not initialize alpm session: {Marshal.PtrToStringUTF8(NativeMethods.StrError(err))}");
        }

        handle = returned;
    }

    public void UpdateDb() {
        foreach (var repoServer in config.RepoServers) {
            var db = NativeMethods.RegisterSyncdb(handle, repoServer.Key, 0);

            if (db == IntPtr.Zero) {
                throw new(Marshal.PtrToStringUTF8(NativeMethods.StrError(NativeMethods.ErrNo(handle))));
            }

            if (NativeMethods.DbSetUsage(db, DbUsage.All) != 0) {
                throw new(Marshal.PtrToStringUTF8(NativeMethods.StrError(NativeMethods.ErrNo(handle))));
            }

            if (repoServer.Value.Any(server => NativeMethods.DbAddServer(db, server.ToString()) != 0)) {
                throw new(Marshal.PtrToStringUTF8(NativeMethods.StrError(NativeMethods.ErrNo(handle))));
            }
        }

        var dbs = NativeMethods.GetSyncdbs(handle);

        switch (NativeMethods.DbUpdate(handle, dbs, false)) {
            case 0:
                Console.WriteLine("Update successful");
                break;

            case 1:
                Console.WriteLine("Already up to date");
                break;

            case -1:
                throw new(Marshal.PtrToStringUTF8(NativeMethods.StrError(NativeMethods.ErrNo(handle))));
        }
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing) {
        ReleaseUnmanagedResources();

        if (disposing) {
            // 관리되는 리소스 없음
        }
    }

    private void ReleaseUnmanagedResources() => NativeMethods.Release(handle);

    ~AlpmSession() => Dispose(false);
}
