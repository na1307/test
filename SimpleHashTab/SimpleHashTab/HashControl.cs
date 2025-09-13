using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SimpleHashTab;

[RequiresUnreferencedCode("DataGridView")]
internal sealed partial class HashControl : UserControl {
    private readonly string filePath;

    public HashControl(string filePath) {
        this.filePath = filePath;

        InitializeComponent();
    }

    protected override async void OnVisibleChanged(EventArgs e) {
        base.OnVisibleChanged(e);

        if (Visible) {
            await using (var tempFs = File.OpenRead(filePath)) {
                progressBar1.Maximum = (int)(tempFs.Length * Hashed.AlgorithmCount / 1000);
            }

            var crc32 = await Hashed.HashCrc32(filePath, p => progressBar1.Increment((int)(p / 1000)));
            var md5 = await Hashed.HashMD5(filePath, p => progressBar1.Increment((int)(p / 1000)));
            var sha1 = await Hashed.HashSha1(filePath, p => progressBar1.Increment((int)(p / 1000)));
            var sha256 = await Hashed.HashSha256(filePath, p => progressBar1.Increment((int)(p / 1000)));
            var sha512 = await Hashed.HashSha512(filePath, p => progressBar1.Increment((int)(p / 1000)));

            dataGridView1.DataSource = (Hashed[])[crc32, md5, sha1, sha256, sha512];
        }
    }

    protected override void WndProc(ref Message m) {
        if (m.Msg == WindowsMessages.WM_DROPFILES) {
            var countOfFiles = HashPropSheet.DragQueryFileW(m.WParam, 0xFFFFFFFF, null, 0);

            if (countOfFiles != 1) {
                MessageBox.Show("Please drop only one file.", null, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            StringBuilder sb = new(260);
            var result = HashPropSheet.DragQueryFileW(m.WParam, 0, sb, 260);

            if (result == 0) {
                MessageBox.Show($"DragQueryFileW Failed: {result}");

                return;
            }
        }

        base.WndProc(ref m);
    }
}
