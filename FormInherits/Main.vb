Public Module Main
    Private WithEvents Selector As New Form
    Private WithEvents Button1 As New Button
    Private WithEvents Button2 As New Button
    Private WithEvents Normal As New FormNormal
    Private WithEvents [Single] As FormSingle = FormSingle.GetInstance

    <STAThread>
    Public Sub Main()
        With Button1
            .Name = "Button1"
            .Text = "Normal"
            .Size = New Size(260, 25)
            .Location = New Point(12, 12)
            .UseVisualStyleBackColor = True
        End With

        With Button2
            .Name = "Button2"
            .Text = "Single"
            .Size = New Size(260, 25)
            .Location = New Point(12, 224)
            .UseVisualStyleBackColor = True
        End With

        With Selector
            .Name = "FormSelector"
            .Text = "폼 선택"
            .AutoScaleMode = AutoScaleMode.None
            .Font = New Font("맑은 고딕", 9.0!, FontStyle.Regular, GraphicsUnit.Point, 129)
            .Controls.Add(Button1)
            .Controls.Add(Button2)
        End With

        Application.Run(Selector)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Normal.ShowDialog()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        [Single].ShowDialog()
    End Sub
End Module