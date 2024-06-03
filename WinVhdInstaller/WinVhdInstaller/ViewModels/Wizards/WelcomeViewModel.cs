namespace WinVhdInstaller.ViewModels.Wizards;

public sealed class WelcomeViewModel : WizardPageViewModelBase {
    public override string Title => "Welcome Page";
    public override string Description => "Welcome to the WinVhdInstaller Wizard.";
    public override bool CanNavigateNext => true;
    public override bool CanNavigatePrevious => false;
}
