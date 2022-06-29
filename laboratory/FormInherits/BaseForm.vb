Public MustInherit Class BaseForm
    Private Sub BaseForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = "BaseForm (재정의 안됨)"
    End Sub

    Protected MustOverride Sub BaseButton_Click(sender As Object, e As EventArgs) Handles BaseButton.Click
End Class