Public Class Informations

    Private Sub Informations_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim Driver As FileVersionInfo = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\keppydrv\keppydrv.dll")
        Label8.Text = "Keppy's Driver by Keppy Studios" & vbCrLf & "Version " & Driver.FileVersion.ToString
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        MsgBox(My.Resources.Credits.ToString, 64, "Specific credits")
        MsgBox(My.Resources.Credits2.ToString, 64, "Specific credits")
    End Sub

End Class