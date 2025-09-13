namespace SimpleHashTab;

partial class HashControl {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
        dataGridView1 = new DataGridView();
        Algorithm = new DataGridViewTextBoxColumn();
        HashString = new DataGridViewTextBoxColumn();
        progressBar1 = new ProgressBar();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        SuspendLayout();
        // 
        // dataGridView1
        // 
        dataGridView1.AllowUserToAddRows = false;
        dataGridView1.AllowUserToDeleteRows = false;
        dataGridView1.AllowUserToResizeColumns = false;
        dataGridView1.AllowUserToResizeRows = false;
        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Algorithm, HashString });
        dataGridView1.Location = new Point(3, 3);
        dataGridView1.MultiSelect = false;
        dataGridView1.Name = "dataGridView1";
        dataGridView1.ReadOnly = true;
        dataGridView1.RowHeadersVisible = false;
        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dataGridView1.Size = new Size(434, 200);
        dataGridView1.TabIndex = 0;
        // 
        // Algorithm
        // 
        Algorithm.DataPropertyName = "Algorithm";
        Algorithm.HeaderText = "알고리즘";
        Algorithm.Name = "Algorithm";
        Algorithm.ReadOnly = true;
        // 
        // HashString
        // 
        HashString.DataPropertyName = "HashString";
        HashString.HeaderText = "해시 값";
        HashString.Name = "HashString";
        HashString.ReadOnly = true;
        HashString.Width = 300;
        // 
        // progressBar1
        // 
        progressBar1.Location = new Point(3, 209);
        progressBar1.Name = "progressBar1";
        progressBar1.Size = new Size(434, 23);
        progressBar1.TabIndex = 1;
        // 
        // HashControl
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(249, 249, 249);
        Controls.Add(progressBar1);
        Controls.Add(dataGridView1);
        Name = "HashControl";
        Size = new Size(440, 500);
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private DataGridView dataGridView1;
    private DataGridViewTextBoxColumn Algorithm;
    private DataGridViewTextBoxColumn HashString;
    private ProgressBar progressBar1;
}
