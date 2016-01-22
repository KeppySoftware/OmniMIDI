Imports System.IO
Imports System.Drawing
Imports System.Net
Imports Microsoft.Win32
Imports System.Drawing.Text
Imports System.Runtime.InteropServices
Imports System.Diagnostics

Public Class DebugWin

    Private Sub DebugWin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DebugRefresh.Start()
    End Sub

    Private Sub DebugRefresh_Tick(sender As Object, e As EventArgs) Handles DebugRefresh.Tick
        Try
            Dim UserString As String
            UserString = "Software\Keppy's Driver"
            Dim keppykeydebug = My.Computer.Registry.CurrentUser.OpenSubKey(UserString, True)
            Dim keppykey = My.Computer.Registry.CurrentUser.OpenSubKey(UserString + "\Settings", True)

            voices1.Text = keppykeydebug.GetValue("currentvoices0") & "/" & keppykey.GetValue("polyphony")
            cpu1.Text = keppykeydebug.GetValue("currentcpuusage0") & "%"
            int1.Text = "Decoded datas size:" & vbCrLf & keppykeydebug.GetValue("int") & " frames"

            ErrorHoryShet.Visible = False
        Catch ex As Exception
            ErrorHoryShet.Visible = True
            MessageBox.Show(ex.ToString & vbCrLf & vbCrLf & "Press OK to stop the debug mode.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            DebugRefresh.Stop()
        End Try
    End Sub

End Class