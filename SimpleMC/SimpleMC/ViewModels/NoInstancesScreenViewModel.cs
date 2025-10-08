using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

namespace SimpleMC.ViewModels;

internal sealed partial class NoInstancesScreenViewModel : ViewModelBase {
    [RelayCommand]
    private void AddInstance() => Ioc.Default.GetRequiredService<MainWindowViewModel>().CurrentScreen = new AddInstanceScreenViewModel();
}
