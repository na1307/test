using Microsoft.Dism;
using System.Windows.Threading;

namespace WinVhdInstaller;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public sealed partial class App {
    protected override void OnStartup(StartupEventArgs e) {
        base.OnStartup(e);
        DismApi.Initialize(DismLogLevel.LogErrors);
    }

    protected override void OnExit(ExitEventArgs e) {
        base.OnExit(e);
        DismApi.Shutdown();
    }

    private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) => ErrMsg(e.Exception.ToString());
}
