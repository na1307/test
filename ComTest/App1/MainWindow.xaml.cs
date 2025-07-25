// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace App1;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow {
    private readonly string FN = "Unknown";

    public MainWindow(string? fn) {
        if (fn is not null) {
            FN = fn;
        }

        InitializeComponent();
    }
}
