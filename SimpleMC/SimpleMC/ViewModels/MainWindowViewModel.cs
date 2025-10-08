using FluentAvalonia.UI.Controls;
using SimpleMC.Models;

namespace SimpleMC.ViewModels;

internal sealed partial class MainWindowViewModel : ViewModelBase {
    [ObservableProperty]
    public partial ViewModelBase CurrentScreen { get; set; } = null!;

    public void NV(object? sender, NavigationViewSelectionChangedEventArgs e) {
        if (e.IsSettingsSelected) {
            CurrentScreen = new SettingsScreenViewModel();
        } else if (e.SelectedItem is NavigationViewItem { Tag: "Instances" }) {
            CurrentScreen = new InstancesMiddleScreenViewModel(Instance.GetAllInstances());
        } else {
            CurrentScreen = (ViewModelBase)Activator.CreateInstance(
                Type.GetType($"SimpleMC.ViewModels.{((NavigationViewItem)e.SelectedItem).Tag}ScreenViewModel")!)!;
        }
    }
}
