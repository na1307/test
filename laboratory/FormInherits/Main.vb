Public Module Main
    Public WithEvents ActForm1Instance As New ActForm1
    Public WithEvents ActForm2Instance As New ActForm2

    <STAThread>
    Public Sub Main()
        Application.Run(ActForm1Instance)
    End Sub

    Public Sub ActForm2_Closed(sender As Object, e As EventArgs) Handles ActForm2Instance.FormClosed
        ActForm1Instance.Close()
    End Sub
End Module