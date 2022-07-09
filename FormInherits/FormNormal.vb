Public Class FormNormal : Inherits FormAbstract
    Public Sub New()
        InitializeComponent()
    End Sub


    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'FormNormal
        '
        Me.ClientSize = New System.Drawing.Size(484, 161)
        Me.Name = "FormNormal"
        Me.Text = "보통"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
End Class