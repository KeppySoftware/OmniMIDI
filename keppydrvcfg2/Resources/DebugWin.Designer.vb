<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DebugWin
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
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

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla nell'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.cpu1 = New System.Windows.Forms.Label()
        Me.voices1 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.DebugRefresh = New System.Windows.Forms.Timer(Me.components)
        Me.ErrorHoryShet = New System.Windows.Forms.Label()
        Me.int1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'cpu1
        '
        Me.cpu1.AutoSize = True
        Me.cpu1.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cpu1.ForeColor = System.Drawing.Color.Black
        Me.cpu1.Location = New System.Drawing.Point(216, 30)
        Me.cpu1.Name = "cpu1"
        Me.cpu1.Size = New System.Drawing.Size(47, 18)
        Me.cpu1.TabIndex = 13
        Me.cpu1.Text = "100%"
        '
        'voices1
        '
        Me.voices1.AutoSize = True
        Me.voices1.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.voices1.ForeColor = System.Drawing.Color.Black
        Me.voices1.Location = New System.Drawing.Point(186, 9)
        Me.voices1.Name = "voices1"
        Me.voices1.Size = New System.Drawing.Size(110, 18)
        Me.voices1.TabIndex = 11
        Me.voices1.Text = "100000/100000"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.Black
        Me.Label12.Location = New System.Drawing.Point(126, 30)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(84, 18)
        Me.Label12.TabIndex = 10
        Me.Label12.Text = "CPU usage:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(126, 9)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(54, 18)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Voices:"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(10, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(110, 39)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Debug"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Label1.UseCompatibleTextRendering = True
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.Red
        Me.Label14.Location = New System.Drawing.Point(20, 53)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(140, 97)
        Me.Label14.TabIndex = 5
        Me.Label14.Text = "WARNING!" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Only one program per session is supported!" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Using multiple programs at " & _
    "once will probably" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "mix up their values."
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DebugRefresh
        '
        Me.DebugRefresh.Enabled = True
        Me.DebugRefresh.Interval = 1
        '
        'ErrorHoryShet
        '
        Me.ErrorHoryShet.ForeColor = System.Drawing.Color.Red
        Me.ErrorHoryShet.Location = New System.Drawing.Point(171, 53)
        Me.ErrorHoryShet.Name = "ErrorHoryShet"
        Me.ErrorHoryShet.Size = New System.Drawing.Size(130, 55)
        Me.ErrorHoryShet.TabIndex = 6
        Me.ErrorHoryShet.Text = "ERROR WHILE READING" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "REGISTRY!"
        Me.ErrorHoryShet.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ErrorHoryShet.Visible = False
        '
        'int1
        '
        Me.int1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.int1.ForeColor = System.Drawing.Color.Black
        Me.int1.Location = New System.Drawing.Point(166, 108)
        Me.int1.Name = "int1"
        Me.int1.Size = New System.Drawing.Size(130, 42)
        Me.int1.TabIndex = 17
        Me.int1.Text = "INTA"
        Me.int1.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'DebugWin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(307, 164)
        Me.Controls.Add(Me.cpu1)
        Me.Controls.Add(Me.int1)
        Me.Controls.Add(Me.voices1)
        Me.Controls.Add(Me.ErrorHoryShet)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "DebugWin"
        Me.ShowIcon = False
        Me.Text = "Debug window"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cpu1 As System.Windows.Forms.Label
    Friend WithEvents voices1 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents DebugRefresh As System.Windows.Forms.Timer
    Friend WithEvents ErrorHoryShet As System.Windows.Forms.Label
    Friend WithEvents int1 As System.Windows.Forms.Label
End Class
