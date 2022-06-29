Public Class ActForm1 : Inherits BaseForm
    Friend WithEvents Label2 As Label

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub ActForm1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Text = "Act1"
        Label1.Text = "실제 폼 1"
        BaseButton.Name = "Inherited1Button"
        BaseButton.Text = "Inherited1Button"
    End Sub

    Protected Overrides Sub BaseButton_Click(sender As Object, e As EventArgs) Handles BaseButton.Click
        ActForm2Instance.Show()
        Hide()
    End Sub

    Private Sub InitializeComponent()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(184, 67)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(153, 12)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "이건 두번째 라벨 와 신난다"
        '
        'ActForm1
        '
        Me.ClientSize = New System.Drawing.Size(484, 161)
        Me.Controls.Add(Me.Label2)
        Me.Name = "ActForm1"
        Me.Controls.SetChildIndex(Me.Label2, 0)
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub
End Class