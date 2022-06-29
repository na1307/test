Public Class ActForm2 : Inherits BaseForm
    Friend WithEvents Label2 As Label

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub ActForm1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Text = "Act2"
        Label1.Text = "실제 폼 2"
        BaseButton.Name = "Inherited2Button"
        BaseButton.Text = "Inherited2Button"
    End Sub

    Protected Overrides Sub BaseButton_Click(sender As Object, e As EventArgs) Handles BaseButton.Click
        ActForm1Instance.Show()
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
        Me.Label2.Text = "이건 두번째 라벨 이게 모야"
        '
        'ActForm2
        '
        Me.ClientSize = New System.Drawing.Size(484, 161)
        Me.Controls.Add(Me.Label2)
        Me.Name = "ActForm2"
        Me.Controls.SetChildIndex(Me.Label2, 0)
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub
End Class