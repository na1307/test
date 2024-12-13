using Avalonia.Controls;

namespace ProjectBabylon.Views;

public sealed partial class MainWindow : Window {
    public MainWindow() => InitializeComponent();

    private void Window_OnClosing(object? sender, WindowClosingEventArgs e) => e.Cancel = true;
}
