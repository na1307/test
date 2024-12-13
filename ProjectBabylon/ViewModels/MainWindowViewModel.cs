using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.Input;

namespace ProjectBabylon.ViewModels;

public sealed partial class MainWindowViewModel : ViewModelBase {
    [RelayCommand]
    private void Esc() => ((IClassicDesktopStyleApplicationLifetime)App.Current.ApplicationLifetime!).MainWindow!.Close();
}
