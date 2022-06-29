<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BaseForm
    Inherits System.Windows.Forms.Form

    'Form은 Dispose를 재정의하여 구성 요소 목록을 정리합니다.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows Form 디자이너에 필요합니다.
    Private components As System.ComponentModel.IContainer

    '참고: 다음 프로시저는 Windows Form 디자이너에 필요합니다.
    '수정하려면 Windows Form 디자이너를 사용하십시오.  
    '코드 편집기에서는 수정하지 마세요.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.BaseButton = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'BaseButton
        '
        Me.BaseButton.Location = New System.Drawing.Point(12, 126)
        Me.BaseButton.Name = "BaseButton"
        Me.BaseButton.Size = New System.Drawing.Size(460, 23)
        Me.BaseButton.TabIndex = 0
        Me.BaseButton.Text = "BaseButton"
        Me.BaseButton.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(10, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(462, 23)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Label1"
        '
        'BaseForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(484, 161)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.BaseButton)
        Me.Name = "BaseForm"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents BaseButton As Button
    Friend WithEvents Label1 As Label
End Class
