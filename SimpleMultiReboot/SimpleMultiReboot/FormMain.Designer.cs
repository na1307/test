namespace SimpleMultiReboot;

partial class FormMain {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
        if (disposing) {
            if (components != null) {
                components.Dispose();
            }

            bcdController.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
        entryDataGridView = new DataGridView();
        descriptionColumn = new DataGridViewTextBoxColumn();
        typeColumn = new DataGridViewTextBoxColumn();
        identifierColumn = new DataGridViewTextBoxColumn();
        okButton = new Button();
        cancelButton = new Button();
        entryRadioButton = new RadioButton();
        firmwareRadioButton = new RadioButton();
        rebootNowCheckBox = new CheckBox();
        ((System.ComponentModel.ISupportInitialize)entryDataGridView).BeginInit();
        SuspendLayout();
        // 
        // entryDataGridView
        // 
        entryDataGridView.AllowUserToAddRows = false;
        entryDataGridView.AllowUserToDeleteRows = false;
        entryDataGridView.AllowUserToResizeRows = false;
        entryDataGridView.AutoGenerateColumns = false;
        entryDataGridView.Columns.AddRange(new DataGridViewColumn[] { descriptionColumn, typeColumn, identifierColumn });
        entryDataGridView.Location = new Point(12, 37);
        entryDataGridView.MultiSelect = false;
        entryDataGridView.Name = "entryDataGridView";
        entryDataGridView.ReadOnly = true;
        entryDataGridView.RowHeadersVisible = false;
        entryDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        entryDataGridView.Size = new Size(760, 319);
        entryDataGridView.TabIndex = 3;
        // 
        // descriptionColumn
        // 
        descriptionColumn.DataPropertyName = "Description";
        descriptionColumn.HeaderText = "Description";
        descriptionColumn.Name = "descriptionColumn";
        descriptionColumn.ReadOnly = true;
        descriptionColumn.Width = 250;
        // 
        // typeColumn
        // 
        typeColumn.DataPropertyName = "TypeString";
        typeColumn.HeaderText = "Type";
        typeColumn.Name = "typeColumn";
        typeColumn.ReadOnly = true;
        typeColumn.Width = 200;
        // 
        // identifierColumn
        // 
        identifierColumn.DataPropertyName = "Identifier";
        identifierColumn.HeaderText = "Identifier";
        identifierColumn.Name = "identifierColumn";
        identifierColumn.ReadOnly = true;
        identifierColumn.Width = 300;
        // 
        // okButton
        // 
        okButton.Location = new Point(616, 406);
        okButton.Name = "okButton";
        okButton.Size = new Size(75, 23);
        okButton.TabIndex = 0;
        okButton.Text = "OK";
        okButton.UseVisualStyleBackColor = true;
        okButton.Click += okButton_Click;
        // 
        // cancelButton
        // 
        cancelButton.Location = new Point(697, 406);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(75, 23);
        cancelButton.TabIndex = 1;
        cancelButton.Text = "Cancel";
        cancelButton.UseVisualStyleBackColor = true;
        cancelButton.Click += cancelButton_Click;
        // 
        // entryRadioButton
        // 
        entryRadioButton.AutoSize = true;
        entryRadioButton.Checked = true;
        entryRadioButton.Location = new Point(12, 12);
        entryRadioButton.Name = "entryRadioButton";
        entryRadioButton.Size = new Size(229, 19);
        entryRadioButton.TabIndex = 2;
        entryRadioButton.TabStop = true;
        entryRadioButton.Text = "Reboot to a BCD Entry or a UEFI Device";
        entryRadioButton.UseVisualStyleBackColor = true;
        entryRadioButton.CheckedChanged += entryRadioButton_CheckedChanged;
        // 
        // firmwareRadioButton
        // 
        firmwareRadioButton.AutoSize = true;
        firmwareRadioButton.Location = new Point(12, 362);
        firmwareRadioButton.Name = "firmwareRadioButton";
        firmwareRadioButton.Size = new Size(194, 19);
        firmwareRadioButton.TabIndex = 4;
        firmwareRadioButton.TabStop = true;
        firmwareRadioButton.Text = "Reboot to the Firmware Settings";
        firmwareRadioButton.UseVisualStyleBackColor = true;
        firmwareRadioButton.CheckedChanged += firmwareRadioButton_CheckedChanged;
        // 
        // rebootNowCheckBox
        // 
        rebootNowCheckBox.AutoSize = true;
        rebootNowCheckBox.Location = new Point(680, 381);
        rebootNowCheckBox.Name = "rebootNowCheckBox";
        rebootNowCheckBox.Size = new Size(92, 19);
        rebootNowCheckBox.TabIndex = 5;
        rebootNowCheckBox.Text = "Reboot Now";
        rebootNowCheckBox.UseVisualStyleBackColor = true;
        // 
        // FormMain
        // 
        AcceptButton = okButton;
        CancelButton = cancelButton;
        ClientSize = new Size(784, 441);
        Controls.Add(rebootNowCheckBox);
        Controls.Add(firmwareRadioButton);
        Controls.Add(entryDataGridView);
        Controls.Add(entryRadioButton);
        Controls.Add(cancelButton);
        Controls.Add(okButton);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MaximumSize = new Size(800, 480);
        MinimumSize = new Size(800, 480);
        Name = "FormMain";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "SimpleMultiReboot";
        ((System.ComponentModel.ISupportInitialize)entryDataGridView).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button okButton;
    private Button cancelButton;
    private RadioButton entryRadioButton;
    private DataGridView entryDataGridView;
    private RadioButton firmwareRadioButton;
    private CheckBox rebootNowCheckBox;
    private DataGridViewTextBoxColumn descriptionColumn;
    private DataGridViewTextBoxColumn typeColumn;
    private DataGridViewTextBoxColumn identifierColumn;
}
