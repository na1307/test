namespace WA.Cbs;

internal enum CbsSessionState {
    Unknown = 0,
    Ready = 16,
    Queued = 32,
    Started = 48,
    Panned = 64,
    Resolved = 80,
    Staged = 96,
    ExecutionDelayed = 101,
    Installed = 112,
    ShutdownStart = 144,
    ShutdownFinish = 160,
    Startup = 176,
    StartupFinish = 192,
    Complete = 208,
    Interrupted = 224,
    Corrupted = 240,
    MarkedForRetry = 256
}
