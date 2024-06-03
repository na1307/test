namespace WinVhdInstaller.ViewModels.Wizards;

public sealed class ProgressModel : WizardPageViewModelBase {
    public override string Title => "Installing";
    public override string Description => "Install progressing";
    public override bool CanNavigateNext => false;
    public override bool CanNavigatePrevious => false;
}
