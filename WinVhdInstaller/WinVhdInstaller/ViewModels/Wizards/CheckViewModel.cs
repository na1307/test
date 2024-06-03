using WinVhdInstaller.Models;

namespace WinVhdInstaller.ViewModels.Wizards;

public sealed class CheckViewModel : WizardPageViewModelBase {
    public override string Title => "Check";
    public override string Description => "Check page";
    public override bool CanNavigateNext => true;
    public override bool CanNavigatePrevious => true;
    public string InstallWimPath => InstallSettings.Instance.InstallWimPath;
    public int ImageIndex => InstallSettings.Instance.ImageIndex;
    public string VhdPath => InstallSettings.Instance.VhdPath;
    public int VhdSize => InstallSettings.Instance.VhdSize;
    public string FixedVhd => InstallSettings.Instance.FixedVhd ? "Fixed" : "Expandable";
}
