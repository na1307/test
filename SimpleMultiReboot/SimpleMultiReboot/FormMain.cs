namespace SimpleMultiReboot;

internal sealed partial class FormMain : Form {
    private readonly BcdController bcdController;

    public FormMain() {
        InitializeComponent();
        bcdController = new();
        firmwareRadioButton.Enabled = BcdController.IsUefi;
        entryDataGridView.DataSource = bcdController.Entries;
    }

    private void okButton_Click(object sender, EventArgs e) {
        var bound = (BootEntry)entryDataGridView.SelectedRows[0].DataBoundItem!;

        if (entryRadioButton.Checked) {
            bcdController.SetBootSequence(bound);
        } else if (firmwareRadioButton.Checked) {
            Program.SetFirmwareVariable();
        }

        if (rebootNowCheckBox.Checked) {
            Program.RebootComputer();
        }

        DialogResult = DialogResult.OK;

        Close();
    }

    private void cancelButton_Click(object sender, EventArgs e) {
        DialogResult = DialogResult.Cancel;

        Close();
    }

    private void entryRadioButton_CheckedChanged(object sender, EventArgs e) => entryDataGridView.Enabled = true;

    private void firmwareRadioButton_CheckedChanged(object sender, EventArgs e) => entryDataGridView.Enabled = false;
}
