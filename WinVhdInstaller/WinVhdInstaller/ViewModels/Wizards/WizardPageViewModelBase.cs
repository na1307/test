namespace WinVhdInstaller.ViewModels.Wizards;

public abstract class WizardPageViewModelBase : ViewModelBase {
    public abstract string Title { get; }
    public abstract string Description { get; }
    public abstract bool CanNavigateNext { get; }
    public abstract bool CanNavigatePrevious { get; }
}
