using FluentAvalonia.UI.Controls;
using SimpleMC.ViewModels;

namespace SimpleMC.Views;

public sealed partial class MainWindow : Window {
    public MainWindow() => InitializeComponent();

    private void NavigationView_OnSelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
        => ((MainWindowViewModel)DataContext!).NV(sender, e);
}
