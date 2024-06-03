using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using WinVhdInstaller.ViewModels.Wizards;

namespace WinVhdInstaller.ViewModels;

public sealed partial class MainViewModel : ViewModelBase, IRecipient<PropertyChangedMessage<string>>, IRecipient<PropertyChangedMessage<bool?>> {
    private readonly WizardPageViewModelBase[] pages;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(NavigateNextCommand))]
    [NotifyCanExecuteChangedFor(nameof(NavigatePreviousCommand))]
    private WizardPageViewModelBase currentPage;

    public MainViewModel() {
        IsActive = true;
        pages = [new WelcomeViewModel(), new InstallWimViewModel(), new VhdPageViewModel(), new CheckViewModel(), new ProgressModel()];
        currentPage = pages[0];
    }

    private bool CanNavigateNext => CurrentPage.CanNavigateNext;
    private bool CanNavigatePrevious => CurrentPage.CanNavigatePrevious;

    public void Receive(PropertyChangedMessage<string> message) => NavigateNextCommand.NotifyCanExecuteChanged();
    public void Receive(PropertyChangedMessage<bool?> message) => NavigateNextCommand.NotifyCanExecuteChanged();

    [RelayCommand(CanExecute = nameof(CanNavigateNext))]
    private void NavigateNext() => CurrentPage = pages[Array.IndexOf(pages, CurrentPage) + 1];

    [RelayCommand(CanExecute = nameof(CanNavigatePrevious))]
    private void NavigatePrevious() => CurrentPage = pages[Array.IndexOf(pages, CurrentPage) - 1];
}
