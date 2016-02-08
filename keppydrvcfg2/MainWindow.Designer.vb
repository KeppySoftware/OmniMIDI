<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainWindow
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainWindow))
        Me.PortAOpenDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.PortBOpenDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.Information = New System.Windows.Forms.ToolTip(Me.components)
        Me.PolyphonyLimit = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.MaxCPU = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Frequency = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.RealTimeSet = New System.Windows.Forms.CheckBox()
        Me.NoDX8FX = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TracksLimit = New System.Windows.Forms.NumericUpDown()
        Me.BufferText = New System.Windows.Forms.Label()
        Me.bufsize = New System.Windows.Forms.NumericUpDown()
        Me.RealTimeSet2 = New System.Windows.Forms.CheckBox()
        Me.MainMenu = New System.Windows.Forms.MenuStrip()
        Me.AppToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CheckForUpdatesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenDebugWindowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.InformationsAboutThisProgramToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ReportABugToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DownloadTheSourceCodeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VisitKeppyStudiosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CanIChangeTheSoundfontInRealtimeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.BlackListFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.ClockTimer = New System.Windows.Forms.Timer(Me.components)
        Me.FirstRunTimer = New System.Windows.Forms.Timer(Me.components)
        Me.FXTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ExtListPortADialog = New System.Windows.Forms.OpenFileDialog()
        Me.ExtListPortBDialog = New System.Windows.Forms.OpenFileDialog()
        Me.PortCOpenDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.PortDOpenDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.ExtListPortCDialog = New System.Windows.Forms.OpenFileDialog()
        Me.ExtListPortDDialog = New System.Windows.Forms.OpenFileDialog()
        Me.Settings = New System.Windows.Forms.TabPage()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.CurrentVolumeHUE2 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.VolumeBar = New System.Windows.Forms.TrackBar()
        Me.CurrentVolumeHUE = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.VoiceWarning = New System.Windows.Forms.PictureBox()
        Me.DisableFX = New System.Windows.Forms.CheckBox()
        Me.Preload = New System.Windows.Forms.CheckBox()
        Me.NoteOff = New System.Windows.Forms.CheckBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.ClockSys = New System.Windows.Forms.Label()
        Me.Versionlabel = New System.Windows.Forms.Label()
        Me.LatestVersionDriver = New System.Windows.Forms.Label()
        Me.UpdateDownload = New System.Windows.Forms.Button()
        Me.ThisVersionDriver = New System.Windows.Forms.Label()
        Me.BlackMIDIPreset = New System.Windows.Forms.Button()
        Me.RealTimeSetText = New System.Windows.Forms.Label()
        Me.Apply = New System.Windows.Forms.Button()
        Me.Reset = New System.Windows.Forms.Button()
        Me.BlacklistSys = New System.Windows.Forms.TabPage()
        Me.ClearBlacklist = New System.Windows.Forms.Button()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.ManualBlackListLabel = New System.Windows.Forms.Label()
        Me.ManualBlackList = New System.Windows.Forms.TextBox()
        Me.BlackListDef = New System.Windows.Forms.Label()
        Me.BlackListAdvancedMode = New System.Windows.Forms.CheckBox()
        Me.SystemList = New System.Windows.Forms.Button()
        Me.RemoveBlackList = New System.Windows.Forms.Button()
        Me.AddBlackList = New System.Windows.Forms.Button()
        Me.UserProgramsBlackList = New System.Windows.Forms.ListBox()
        Me.Port4 = New System.Windows.Forms.TabPage()
        Me.SFZ4 = New System.Windows.Forms.Button()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.DisableCheckPortD = New System.Windows.Forms.CheckBox()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Button11 = New System.Windows.Forms.Button()
        Me.Button12 = New System.Windows.Forms.Button()
        Me.Button13 = New System.Windows.Forms.Button()
        Me.PortDBox = New System.Windows.Forms.ListBox()
        Me.Port3 = New System.Windows.Forms.TabPage()
        Me.SFZ3 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.DisableCheckPortC = New System.Windows.Forms.CheckBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.PortCBox = New System.Windows.Forms.ListBox()
        Me.Port2 = New System.Windows.Forms.TabPage()
        Me.SFZ2 = New System.Windows.Forms.Button()
        Me.ExtListPortB = New System.Windows.Forms.Button()
        Me.DisableCheckPortB = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ClearPortB = New System.Windows.Forms.Button()
        Me.MoveDownPortB = New System.Windows.Forms.Button()
        Me.MoveUpPortB = New System.Windows.Forms.Button()
        Me.RemoveSFPortB = New System.Windows.Forms.Button()
        Me.ImportSFPortB = New System.Windows.Forms.Button()
        Me.PortBBox = New System.Windows.Forms.ListBox()
        Me.Port1 = New System.Windows.Forms.TabPage()
        Me.SFZ1 = New System.Windows.Forms.Button()
        Me.ExtListPortA = New System.Windows.Forms.Button()
        Me.DisableCheckPortA = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ClearPortA = New System.Windows.Forms.Button()
        Me.MoveDownPortA = New System.Windows.Forms.Button()
        Me.MoveUpPortA = New System.Windows.Forms.Button()
        Me.RemoveSFPortA = New System.Windows.Forms.Button()
        Me.ImportSFPortA = New System.Windows.Forms.Button()
        Me.PortABox = New System.Windows.Forms.ListBox()
        Me.Tabs1 = New System.Windows.Forms.TabControl()
        Me.AdvSettings = New System.Windows.Forms.TabPage()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.BlackMIDIPreset2 = New System.Windows.Forms.Button()
        Me.RealTimeSetText2 = New System.Windows.Forms.Label()
        Me.AdvancedApply = New System.Windows.Forms.Button()
        Me.AdvancedReset = New System.Windows.Forms.Button()
        Me.AdvPanel = New System.Windows.Forms.Panel()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.SincInter = New System.Windows.Forms.CheckBox()
        Me.BufferWarning = New System.Windows.Forms.PictureBox()
        Me.SysResetIgnore = New System.Windows.Forms.CheckBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.EchoFXNum = New System.Windows.Forms.NumericUpDown()
        Me.EchoFX = New System.Windows.Forms.CheckBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.SittingFXNum = New System.Windows.Forms.NumericUpDown()
        Me.DistortionFXNum = New System.Windows.Forms.NumericUpDown()
        Me.GargleFXNum = New System.Windows.Forms.NumericUpDown()
        Me.SittingFX = New System.Windows.Forms.CheckBox()
        Me.DistortionFX = New System.Windows.Forms.CheckBox()
        Me.CompressorFXNum = New System.Windows.Forms.NumericUpDown()
        Me.FlangerFXNum = New System.Windows.Forms.NumericUpDown()
        Me.ChorusFXNum = New System.Windows.Forms.NumericUpDown()
        Me.ReverbFXNum = New System.Windows.Forms.NumericUpDown()
        Me.CompressorFX = New System.Windows.Forms.CheckBox()
        Me.FlangerFX = New System.Windows.Forms.CheckBox()
        Me.ChorusFX = New System.Windows.Forms.CheckBox()
        Me.GargleFX = New System.Windows.Forms.CheckBox()
        Me.ReverbFX = New System.Windows.Forms.CheckBox()
        CType(Me.PolyphonyLimit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TracksLimit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.bufsize, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MainMenu.SuspendLayout()
        Me.Settings.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.VolumeBar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        CType(Me.VoiceWarning, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox4.SuspendLayout()
        Me.BlacklistSys.SuspendLayout()
        Me.Port4.SuspendLayout()
        Me.Port3.SuspendLayout()
        Me.Port2.SuspendLayout()
        Me.Port1.SuspendLayout()
        Me.Tabs1.SuspendLayout()
        Me.AdvSettings.SuspendLayout()
        Me.AdvPanel.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        CType(Me.BufferWarning, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        CType(Me.EchoFXNum, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SittingFXNum, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DistortionFXNum, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GargleFXNum, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CompressorFXNum, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.FlangerFXNum, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ChorusFXNum, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ReverbFXNum, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PortAOpenDialog1
        '
        Me.PortAOpenDialog1.FileName = "OpenFileDialog1"
        Me.PortAOpenDialog1.Multiselect = True
        '
        'PortBOpenDialog1
        '
        Me.PortBOpenDialog1.FileName = "OpenFileDialog2"
        Me.PortBOpenDialog1.Multiselect = True
        '
        'Information
        '
        Me.Information.IsBalloon = True
        Me.Information.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.Information.ToolTipTitle = "What's this?"
        '
        'PolyphonyLimit
        '
        resources.ApplyResources(Me.PolyphonyLimit, "PolyphonyLimit")
        Me.PolyphonyLimit.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.PolyphonyLimit.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.PolyphonyLimit.Name = "PolyphonyLimit"
        Me.Information.SetToolTip(Me.PolyphonyLimit, resources.GetString("PolyphonyLimit.ToolTip"))
        Me.PolyphonyLimit.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        Me.Information.SetToolTip(Me.Label3, resources.GetString("Label3.ToolTip"))
        '
        'MaxCPU
        '
        Me.MaxCPU.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.MaxCPU.FormattingEnabled = True
        Me.MaxCPU.Items.AddRange(New Object() {resources.GetString("MaxCPU.Items"), resources.GetString("MaxCPU.Items1"), resources.GetString("MaxCPU.Items2"), resources.GetString("MaxCPU.Items3"), resources.GetString("MaxCPU.Items4"), resources.GetString("MaxCPU.Items5"), resources.GetString("MaxCPU.Items6"), resources.GetString("MaxCPU.Items7"), resources.GetString("MaxCPU.Items8"), resources.GetString("MaxCPU.Items9"), resources.GetString("MaxCPU.Items10"), resources.GetString("MaxCPU.Items11"), resources.GetString("MaxCPU.Items12"), resources.GetString("MaxCPU.Items13"), resources.GetString("MaxCPU.Items14"), resources.GetString("MaxCPU.Items15"), resources.GetString("MaxCPU.Items16"), resources.GetString("MaxCPU.Items17"), resources.GetString("MaxCPU.Items18"), resources.GetString("MaxCPU.Items19"), resources.GetString("MaxCPU.Items20")})
        resources.ApplyResources(Me.MaxCPU, "MaxCPU")
        Me.MaxCPU.Name = "MaxCPU"
        Me.Information.SetToolTip(Me.MaxCPU, resources.GetString("MaxCPU.ToolTip"))
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        Me.Information.SetToolTip(Me.Label5, resources.GetString("Label5.ToolTip"))
        '
        'Frequency
        '
        Me.Frequency.FormattingEnabled = True
        Me.Frequency.Items.AddRange(New Object() {resources.GetString("Frequency.Items"), resources.GetString("Frequency.Items1"), resources.GetString("Frequency.Items2"), resources.GetString("Frequency.Items3"), resources.GetString("Frequency.Items4"), resources.GetString("Frequency.Items5"), resources.GetString("Frequency.Items6"), resources.GetString("Frequency.Items7"), resources.GetString("Frequency.Items8"), resources.GetString("Frequency.Items9"), resources.GetString("Frequency.Items10"), resources.GetString("Frequency.Items11"), resources.GetString("Frequency.Items12"), resources.GetString("Frequency.Items13"), resources.GetString("Frequency.Items14"), resources.GetString("Frequency.Items15"), resources.GetString("Frequency.Items16"), resources.GetString("Frequency.Items17"), resources.GetString("Frequency.Items18"), resources.GetString("Frequency.Items19")})
        resources.ApplyResources(Me.Frequency, "Frequency")
        Me.Frequency.Name = "Frequency"
        Me.Information.SetToolTip(Me.Frequency, resources.GetString("Frequency.ToolTip"))
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        Me.Information.SetToolTip(Me.Label6, resources.GetString("Label6.ToolTip"))
        '
        'RealTimeSet
        '
        resources.ApplyResources(Me.RealTimeSet, "RealTimeSet")
        Me.RealTimeSet.Name = "RealTimeSet"
        Me.Information.SetToolTip(Me.RealTimeSet, resources.GetString("RealTimeSet.ToolTip"))
        Me.RealTimeSet.UseVisualStyleBackColor = True
        '
        'NoDX8FX
        '
        resources.ApplyResources(Me.NoDX8FX, "NoDX8FX")
        Me.NoDX8FX.Name = "NoDX8FX"
        Me.Information.SetToolTip(Me.NoDX8FX, resources.GetString("NoDX8FX.ToolTip"))
        Me.NoDX8FX.UseVisualStyleBackColor = True
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        Me.Information.SetToolTip(Me.Label4, resources.GetString("Label4.ToolTip"))
        '
        'TracksLimit
        '
        resources.ApplyResources(Me.TracksLimit, "TracksLimit")
        Me.TracksLimit.Maximum = New Decimal(New Integer() {128, 0, 0, 0})
        Me.TracksLimit.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.TracksLimit.Name = "TracksLimit"
        Me.Information.SetToolTip(Me.TracksLimit, resources.GetString("TracksLimit.ToolTip"))
        Me.TracksLimit.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'BufferText
        '
        resources.ApplyResources(Me.BufferText, "BufferText")
        Me.BufferText.Name = "BufferText"
        Me.Information.SetToolTip(Me.BufferText, resources.GetString("BufferText.ToolTip"))
        '
        'bufsize
        '
        resources.ApplyResources(Me.bufsize, "bufsize")
        Me.bufsize.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.bufsize.Name = "bufsize"
        Me.Information.SetToolTip(Me.bufsize, resources.GetString("bufsize.ToolTip"))
        Me.bufsize.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'RealTimeSet2
        '
        resources.ApplyResources(Me.RealTimeSet2, "RealTimeSet2")
        Me.RealTimeSet2.Name = "RealTimeSet2"
        Me.Information.SetToolTip(Me.RealTimeSet2, resources.GetString("RealTimeSet2.ToolTip"))
        Me.RealTimeSet2.UseVisualStyleBackColor = True
        '
        'MainMenu
        '
        resources.ApplyResources(Me.MainMenu, "MainMenu")
        Me.MainMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
        Me.MainMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AppToolStripMenuItem, Me.ToolStripMenuItem1, Me.CanIChangeTheSoundfontInRealtimeToolStripMenuItem, Me.ToolStripMenuItem2})
        Me.MainMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow
        Me.MainMenu.Name = "MainMenu"
        Me.MainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        '
        'AppToolStripMenuItem
        '
        Me.AppToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CheckForUpdatesToolStripMenuItem, Me.OpenDebugWindowToolStripMenuItem, Me.ToolStripSeparator1, Me.ExitToolStripMenuItem})
        Me.AppToolStripMenuItem.Name = "AppToolStripMenuItem"
        resources.ApplyResources(Me.AppToolStripMenuItem, "AppToolStripMenuItem")
        '
        'CheckForUpdatesToolStripMenuItem
        '
        Me.CheckForUpdatesToolStripMenuItem.Name = "CheckForUpdatesToolStripMenuItem"
        resources.ApplyResources(Me.CheckForUpdatesToolStripMenuItem, "CheckForUpdatesToolStripMenuItem")
        '
        'OpenDebugWindowToolStripMenuItem
        '
        Me.OpenDebugWindowToolStripMenuItem.Name = "OpenDebugWindowToolStripMenuItem"
        resources.ApplyResources(Me.OpenDebugWindowToolStripMenuItem, "OpenDebugWindowToolStripMenuItem")
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        resources.ApplyResources(Me.ToolStripSeparator1, "ToolStripSeparator1")
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        resources.ApplyResources(Me.ExitToolStripMenuItem, "ExitToolStripMenuItem")
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.InformationsAboutThisProgramToolStripMenuItem, Me.ToolStripSeparator2, Me.ReportABugToolStripMenuItem, Me.DownloadTheSourceCodeToolStripMenuItem, Me.VisitKeppyStudiosToolStripMenuItem})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        resources.ApplyResources(Me.ToolStripMenuItem1, "ToolStripMenuItem1")
        '
        'InformationsAboutThisProgramToolStripMenuItem
        '
        Me.InformationsAboutThisProgramToolStripMenuItem.Name = "InformationsAboutThisProgramToolStripMenuItem"
        resources.ApplyResources(Me.InformationsAboutThisProgramToolStripMenuItem, "InformationsAboutThisProgramToolStripMenuItem")
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        resources.ApplyResources(Me.ToolStripSeparator2, "ToolStripSeparator2")
        '
        'ReportABugToolStripMenuItem
        '
        Me.ReportABugToolStripMenuItem.Name = "ReportABugToolStripMenuItem"
        resources.ApplyResources(Me.ReportABugToolStripMenuItem, "ReportABugToolStripMenuItem")
        '
        'DownloadTheSourceCodeToolStripMenuItem
        '
        Me.DownloadTheSourceCodeToolStripMenuItem.Name = "DownloadTheSourceCodeToolStripMenuItem"
        resources.ApplyResources(Me.DownloadTheSourceCodeToolStripMenuItem, "DownloadTheSourceCodeToolStripMenuItem")
        '
        'VisitKeppyStudiosToolStripMenuItem
        '
        Me.VisitKeppyStudiosToolStripMenuItem.Name = "VisitKeppyStudiosToolStripMenuItem"
        resources.ApplyResources(Me.VisitKeppyStudiosToolStripMenuItem, "VisitKeppyStudiosToolStripMenuItem")
        '
        'CanIChangeTheSoundfontInRealtimeToolStripMenuItem
        '
        Me.CanIChangeTheSoundfontInRealtimeToolStripMenuItem.Name = "CanIChangeTheSoundfontInRealtimeToolStripMenuItem"
        resources.ApplyResources(Me.CanIChangeTheSoundfontInRealtimeToolStripMenuItem, "CanIChangeTheSoundfontInRealtimeToolStripMenuItem")
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        resources.ApplyResources(Me.ToolStripMenuItem2, "ToolStripMenuItem2")
        '
        'Label22
        '
        resources.ApplyResources(Me.Label22, "Label22")
        Me.Label22.Name = "Label22"
        '
        'BlackListFileDialog
        '
        Me.BlackListFileDialog.FileName = "Add an executable..."
        Me.BlackListFileDialog.Multiselect = True
        '
        'ClockTimer
        '
        Me.ClockTimer.Interval = 1000
        '
        'FirstRunTimer
        '
        Me.FirstRunTimer.Interval = 1250
        '
        'FXTimer
        '
        Me.FXTimer.Interval = 10
        '
        'ExtListPortADialog
        '
        resources.ApplyResources(Me.ExtListPortADialog, "ExtListPortADialog")
        Me.ExtListPortADialog.Multiselect = True
        Me.ExtListPortADialog.RestoreDirectory = True
        '
        'ExtListPortBDialog
        '
        resources.ApplyResources(Me.ExtListPortBDialog, "ExtListPortBDialog")
        Me.ExtListPortBDialog.Multiselect = True
        Me.ExtListPortBDialog.RestoreDirectory = True
        '
        'PortCOpenDialog1
        '
        Me.PortCOpenDialog1.FileName = "OpenFileDialog2"
        Me.PortCOpenDialog1.Multiselect = True
        '
        'PortDOpenDialog1
        '
        Me.PortDOpenDialog1.FileName = "OpenFileDialog2"
        Me.PortDOpenDialog1.Multiselect = True
        '
        'ExtListPortCDialog
        '
        resources.ApplyResources(Me.ExtListPortCDialog, "ExtListPortCDialog")
        Me.ExtListPortCDialog.Multiselect = True
        Me.ExtListPortCDialog.RestoreDirectory = True
        '
        'ExtListPortDDialog
        '
        resources.ApplyResources(Me.ExtListPortDDialog, "ExtListPortDDialog")
        Me.ExtListPortDDialog.Multiselect = True
        Me.ExtListPortDDialog.RestoreDirectory = True
        '
        'Settings
        '
        Me.Settings.BackColor = System.Drawing.SystemColors.Control
        Me.Settings.Controls.Add(Me.RealTimeSet)
        Me.Settings.Controls.Add(Me.Panel2)
        Me.Settings.Controls.Add(Me.BlackMIDIPreset)
        Me.Settings.Controls.Add(Me.RealTimeSetText)
        Me.Settings.Controls.Add(Me.Apply)
        Me.Settings.Controls.Add(Me.Reset)
        resources.ApplyResources(Me.Settings, "Settings")
        Me.Settings.Name = "Settings"
        '
        'Panel2
        '
        resources.ApplyResources(Me.Panel2, "Panel2")
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel2.Controls.Add(Me.GroupBox1)
        Me.Panel2.Controls.Add(Me.GroupBox2)
        Me.Panel2.Controls.Add(Me.GroupBox4)
        Me.Panel2.Name = "Panel2"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CurrentVolumeHUE2)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.VolumeBar)
        Me.GroupBox1.Controls.Add(Me.CurrentVolumeHUE)
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'CurrentVolumeHUE2
        '
        resources.ApplyResources(Me.CurrentVolumeHUE2, "CurrentVolumeHUE2")
        Me.CurrentVolumeHUE2.Name = "CurrentVolumeHUE2"
        '
        'Label10
        '
        resources.ApplyResources(Me.Label10, "Label10")
        Me.Label10.Name = "Label10"
        '
        'VolumeBar
        '
        resources.ApplyResources(Me.VolumeBar, "VolumeBar")
        Me.VolumeBar.BackColor = System.Drawing.SystemColors.Control
        Me.VolumeBar.Maximum = 10000
        Me.VolumeBar.Name = "VolumeBar"
        Me.VolumeBar.TabStop = False
        Me.VolumeBar.TickFrequency = 100
        Me.VolumeBar.TickStyle = System.Windows.Forms.TickStyle.None
        '
        'CurrentVolumeHUE
        '
        resources.ApplyResources(Me.CurrentVolumeHUE, "CurrentVolumeHUE")
        Me.CurrentVolumeHUE.Name = "CurrentVolumeHUE"
        Me.CurrentVolumeHUE.UseCompatibleTextRendering = True
        Me.CurrentVolumeHUE.UseMnemonic = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.VoiceWarning)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.Frequency)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.MaxCPU)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.PolyphonyLimit)
        Me.GroupBox2.Controls.Add(Me.DisableFX)
        Me.GroupBox2.Controls.Add(Me.Preload)
        Me.GroupBox2.Controls.Add(Me.NoteOff)
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = False
        '
        'VoiceWarning
        '
        Me.VoiceWarning.Cursor = System.Windows.Forms.Cursors.Hand
        Me.VoiceWarning.Image = Global.keppydrvcfg.My.Resources.Resources.Warning_Box_Yellow
        resources.ApplyResources(Me.VoiceWarning, "VoiceWarning")
        Me.VoiceWarning.Name = "VoiceWarning"
        Me.VoiceWarning.TabStop = False
        '
        'DisableFX
        '
        resources.ApplyResources(Me.DisableFX, "DisableFX")
        Me.DisableFX.Name = "DisableFX"
        Me.DisableFX.UseVisualStyleBackColor = True
        '
        'Preload
        '
        resources.ApplyResources(Me.Preload, "Preload")
        Me.Preload.Name = "Preload"
        Me.Preload.UseVisualStyleBackColor = True
        '
        'NoteOff
        '
        resources.ApplyResources(Me.NoteOff, "NoteOff")
        Me.NoteOff.Name = "NoteOff"
        Me.NoteOff.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.ClockSys)
        Me.GroupBox4.Controls.Add(Me.Versionlabel)
        Me.GroupBox4.Controls.Add(Me.LatestVersionDriver)
        Me.GroupBox4.Controls.Add(Me.UpdateDownload)
        Me.GroupBox4.Controls.Add(Me.ThisVersionDriver)
        resources.ApplyResources(Me.GroupBox4, "GroupBox4")
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.TabStop = False
        '
        'ClockSys
        '
        resources.ApplyResources(Me.ClockSys, "ClockSys")
        Me.ClockSys.Name = "ClockSys"
        Me.ClockSys.UseCompatibleTextRendering = True
        '
        'Versionlabel
        '
        resources.ApplyResources(Me.Versionlabel, "Versionlabel")
        Me.Versionlabel.Name = "Versionlabel"
        '
        'LatestVersionDriver
        '
        resources.ApplyResources(Me.LatestVersionDriver, "LatestVersionDriver")
        Me.LatestVersionDriver.Name = "LatestVersionDriver"
        '
        'UpdateDownload
        '
        resources.ApplyResources(Me.UpdateDownload, "UpdateDownload")
        Me.UpdateDownload.Name = "UpdateDownload"
        Me.UpdateDownload.UseVisualStyleBackColor = True
        '
        'ThisVersionDriver
        '
        resources.ApplyResources(Me.ThisVersionDriver, "ThisVersionDriver")
        Me.ThisVersionDriver.Name = "ThisVersionDriver"
        '
        'BlackMIDIPreset
        '
        resources.ApplyResources(Me.BlackMIDIPreset, "BlackMIDIPreset")
        Me.BlackMIDIPreset.Name = "BlackMIDIPreset"
        Me.BlackMIDIPreset.UseVisualStyleBackColor = True
        '
        'RealTimeSetText
        '
        resources.ApplyResources(Me.RealTimeSetText, "RealTimeSetText")
        Me.RealTimeSetText.Name = "RealTimeSetText"
        '
        'Apply
        '
        resources.ApplyResources(Me.Apply, "Apply")
        Me.Apply.Name = "Apply"
        Me.Apply.UseVisualStyleBackColor = True
        '
        'Reset
        '
        resources.ApplyResources(Me.Reset, "Reset")
        Me.Reset.Name = "Reset"
        Me.Reset.UseVisualStyleBackColor = True
        '
        'BlacklistSys
        '
        Me.BlacklistSys.BackColor = System.Drawing.SystemColors.Control
        Me.BlacklistSys.Controls.Add(Me.ClearBlacklist)
        Me.BlacklistSys.Controls.Add(Me.Label13)
        Me.BlacklistSys.Controls.Add(Me.ManualBlackListLabel)
        Me.BlacklistSys.Controls.Add(Me.ManualBlackList)
        Me.BlacklistSys.Controls.Add(Me.BlackListDef)
        Me.BlacklistSys.Controls.Add(Me.BlackListAdvancedMode)
        Me.BlacklistSys.Controls.Add(Me.SystemList)
        Me.BlacklistSys.Controls.Add(Me.RemoveBlackList)
        Me.BlacklistSys.Controls.Add(Me.AddBlackList)
        Me.BlacklistSys.Controls.Add(Me.UserProgramsBlackList)
        resources.ApplyResources(Me.BlacklistSys, "BlacklistSys")
        Me.BlacklistSys.Name = "BlacklistSys"
        '
        'ClearBlacklist
        '
        resources.ApplyResources(Me.ClearBlacklist, "ClearBlacklist")
        Me.ClearBlacklist.Name = "ClearBlacklist"
        Me.ClearBlacklist.UseVisualStyleBackColor = True
        '
        'Label13
        '
        resources.ApplyResources(Me.Label13, "Label13")
        Me.Label13.Name = "Label13"
        '
        'ManualBlackListLabel
        '
        resources.ApplyResources(Me.ManualBlackListLabel, "ManualBlackListLabel")
        Me.ManualBlackListLabel.Name = "ManualBlackListLabel"
        '
        'ManualBlackList
        '
        resources.ApplyResources(Me.ManualBlackList, "ManualBlackList")
        Me.ManualBlackList.Name = "ManualBlackList"
        '
        'BlackListDef
        '
        resources.ApplyResources(Me.BlackListDef, "BlackListDef")
        Me.BlackListDef.Name = "BlackListDef"
        '
        'BlackListAdvancedMode
        '
        resources.ApplyResources(Me.BlackListAdvancedMode, "BlackListAdvancedMode")
        Me.BlackListAdvancedMode.Name = "BlackListAdvancedMode"
        Me.BlackListAdvancedMode.UseVisualStyleBackColor = True
        '
        'SystemList
        '
        resources.ApplyResources(Me.SystemList, "SystemList")
        Me.SystemList.Name = "SystemList"
        Me.SystemList.UseVisualStyleBackColor = True
        '
        'RemoveBlackList
        '
        resources.ApplyResources(Me.RemoveBlackList, "RemoveBlackList")
        Me.RemoveBlackList.Name = "RemoveBlackList"
        Me.RemoveBlackList.UseVisualStyleBackColor = True
        '
        'AddBlackList
        '
        resources.ApplyResources(Me.AddBlackList, "AddBlackList")
        Me.AddBlackList.Name = "AddBlackList"
        Me.AddBlackList.UseVisualStyleBackColor = True
        '
        'UserProgramsBlackList
        '
        Me.UserProgramsBlackList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.UserProgramsBlackList, "UserProgramsBlackList")
        Me.UserProgramsBlackList.FormattingEnabled = True
        Me.UserProgramsBlackList.Name = "UserProgramsBlackList"
        Me.UserProgramsBlackList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        '
        'Port4
        '
        Me.Port4.BackColor = System.Drawing.SystemColors.Control
        Me.Port4.Controls.Add(Me.SFZ4)
        Me.Port4.Controls.Add(Me.Button8)
        Me.Port4.Controls.Add(Me.DisableCheckPortD)
        Me.Port4.Controls.Add(Me.Label27)
        Me.Port4.Controls.Add(Me.Button9)
        Me.Port4.Controls.Add(Me.Button10)
        Me.Port4.Controls.Add(Me.Button11)
        Me.Port4.Controls.Add(Me.Button12)
        Me.Port4.Controls.Add(Me.Button13)
        Me.Port4.Controls.Add(Me.PortDBox)
        resources.ApplyResources(Me.Port4, "Port4")
        Me.Port4.Name = "Port4"
        '
        'SFZ4
        '
        resources.ApplyResources(Me.SFZ4, "SFZ4")
        Me.SFZ4.Name = "SFZ4"
        Me.SFZ4.UseVisualStyleBackColor = True
        '
        'Button8
        '
        resources.ApplyResources(Me.Button8, "Button8")
        Me.Button8.Name = "Button8"
        Me.Button8.UseVisualStyleBackColor = True
        '
        'DisableCheckPortD
        '
        resources.ApplyResources(Me.DisableCheckPortD, "DisableCheckPortD")
        Me.DisableCheckPortD.Name = "DisableCheckPortD"
        Me.DisableCheckPortD.UseVisualStyleBackColor = True
        '
        'Label27
        '
        resources.ApplyResources(Me.Label27, "Label27")
        Me.Label27.Name = "Label27"
        '
        'Button9
        '
        resources.ApplyResources(Me.Button9, "Button9")
        Me.Button9.Name = "Button9"
        Me.Button9.UseVisualStyleBackColor = True
        '
        'Button10
        '
        resources.ApplyResources(Me.Button10, "Button10")
        Me.Button10.Name = "Button10"
        Me.Button10.UseVisualStyleBackColor = True
        '
        'Button11
        '
        resources.ApplyResources(Me.Button11, "Button11")
        Me.Button11.Name = "Button11"
        Me.Button11.UseVisualStyleBackColor = True
        '
        'Button12
        '
        resources.ApplyResources(Me.Button12, "Button12")
        Me.Button12.Name = "Button12"
        Me.Button12.UseVisualStyleBackColor = True
        '
        'Button13
        '
        resources.ApplyResources(Me.Button13, "Button13")
        Me.Button13.Name = "Button13"
        Me.Button13.UseVisualStyleBackColor = True
        '
        'PortDBox
        '
        Me.PortDBox.FormattingEnabled = True
        resources.ApplyResources(Me.PortDBox, "PortDBox")
        Me.PortDBox.Name = "PortDBox"
        Me.PortDBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        '
        'Port3
        '
        Me.Port3.BackColor = System.Drawing.SystemColors.Control
        Me.Port3.Controls.Add(Me.SFZ3)
        Me.Port3.Controls.Add(Me.Button1)
        Me.Port3.Controls.Add(Me.DisableCheckPortC)
        Me.Port3.Controls.Add(Me.Label25)
        Me.Port3.Controls.Add(Me.Button2)
        Me.Port3.Controls.Add(Me.Button4)
        Me.Port3.Controls.Add(Me.Button5)
        Me.Port3.Controls.Add(Me.Button6)
        Me.Port3.Controls.Add(Me.Button7)
        Me.Port3.Controls.Add(Me.PortCBox)
        resources.ApplyResources(Me.Port3, "Port3")
        Me.Port3.Name = "Port3"
        '
        'SFZ3
        '
        resources.ApplyResources(Me.SFZ3, "SFZ3")
        Me.SFZ3.Name = "SFZ3"
        Me.SFZ3.UseVisualStyleBackColor = True
        '
        'Button1
        '
        resources.ApplyResources(Me.Button1, "Button1")
        Me.Button1.Name = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'DisableCheckPortC
        '
        resources.ApplyResources(Me.DisableCheckPortC, "DisableCheckPortC")
        Me.DisableCheckPortC.Name = "DisableCheckPortC"
        Me.DisableCheckPortC.UseVisualStyleBackColor = True
        '
        'Label25
        '
        resources.ApplyResources(Me.Label25, "Label25")
        Me.Label25.Name = "Label25"
        '
        'Button2
        '
        resources.ApplyResources(Me.Button2, "Button2")
        Me.Button2.Name = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button4
        '
        resources.ApplyResources(Me.Button4, "Button4")
        Me.Button4.Name = "Button4"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button5
        '
        resources.ApplyResources(Me.Button5, "Button5")
        Me.Button5.Name = "Button5"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button6
        '
        resources.ApplyResources(Me.Button6, "Button6")
        Me.Button6.Name = "Button6"
        Me.Button6.UseVisualStyleBackColor = True
        '
        'Button7
        '
        resources.ApplyResources(Me.Button7, "Button7")
        Me.Button7.Name = "Button7"
        Me.Button7.UseVisualStyleBackColor = True
        '
        'PortCBox
        '
        Me.PortCBox.FormattingEnabled = True
        resources.ApplyResources(Me.PortCBox, "PortCBox")
        Me.PortCBox.Name = "PortCBox"
        Me.PortCBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        '
        'Port2
        '
        Me.Port2.BackColor = System.Drawing.SystemColors.Control
        Me.Port2.Controls.Add(Me.SFZ2)
        Me.Port2.Controls.Add(Me.ExtListPortB)
        Me.Port2.Controls.Add(Me.DisableCheckPortB)
        Me.Port2.Controls.Add(Me.Label2)
        Me.Port2.Controls.Add(Me.ClearPortB)
        Me.Port2.Controls.Add(Me.MoveDownPortB)
        Me.Port2.Controls.Add(Me.MoveUpPortB)
        Me.Port2.Controls.Add(Me.RemoveSFPortB)
        Me.Port2.Controls.Add(Me.ImportSFPortB)
        Me.Port2.Controls.Add(Me.PortBBox)
        resources.ApplyResources(Me.Port2, "Port2")
        Me.Port2.Name = "Port2"
        '
        'SFZ2
        '
        resources.ApplyResources(Me.SFZ2, "SFZ2")
        Me.SFZ2.Name = "SFZ2"
        Me.SFZ2.UseVisualStyleBackColor = True
        '
        'ExtListPortB
        '
        resources.ApplyResources(Me.ExtListPortB, "ExtListPortB")
        Me.ExtListPortB.Name = "ExtListPortB"
        Me.ExtListPortB.UseVisualStyleBackColor = True
        '
        'DisableCheckPortB
        '
        resources.ApplyResources(Me.DisableCheckPortB, "DisableCheckPortB")
        Me.DisableCheckPortB.Name = "DisableCheckPortB"
        Me.DisableCheckPortB.UseVisualStyleBackColor = True
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'ClearPortB
        '
        resources.ApplyResources(Me.ClearPortB, "ClearPortB")
        Me.ClearPortB.Name = "ClearPortB"
        Me.ClearPortB.UseVisualStyleBackColor = True
        '
        'MoveDownPortB
        '
        resources.ApplyResources(Me.MoveDownPortB, "MoveDownPortB")
        Me.MoveDownPortB.Name = "MoveDownPortB"
        Me.MoveDownPortB.UseVisualStyleBackColor = True
        '
        'MoveUpPortB
        '
        resources.ApplyResources(Me.MoveUpPortB, "MoveUpPortB")
        Me.MoveUpPortB.Name = "MoveUpPortB"
        Me.MoveUpPortB.UseVisualStyleBackColor = True
        '
        'RemoveSFPortB
        '
        resources.ApplyResources(Me.RemoveSFPortB, "RemoveSFPortB")
        Me.RemoveSFPortB.Name = "RemoveSFPortB"
        Me.RemoveSFPortB.UseVisualStyleBackColor = True
        '
        'ImportSFPortB
        '
        resources.ApplyResources(Me.ImportSFPortB, "ImportSFPortB")
        Me.ImportSFPortB.Name = "ImportSFPortB"
        Me.ImportSFPortB.UseVisualStyleBackColor = True
        '
        'PortBBox
        '
        Me.PortBBox.FormattingEnabled = True
        resources.ApplyResources(Me.PortBBox, "PortBBox")
        Me.PortBBox.Name = "PortBBox"
        Me.PortBBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        '
        'Port1
        '
        Me.Port1.BackColor = System.Drawing.SystemColors.Control
        Me.Port1.Controls.Add(Me.SFZ1)
        Me.Port1.Controls.Add(Me.ExtListPortA)
        Me.Port1.Controls.Add(Me.DisableCheckPortA)
        Me.Port1.Controls.Add(Me.Label1)
        Me.Port1.Controls.Add(Me.ClearPortA)
        Me.Port1.Controls.Add(Me.MoveDownPortA)
        Me.Port1.Controls.Add(Me.MoveUpPortA)
        Me.Port1.Controls.Add(Me.RemoveSFPortA)
        Me.Port1.Controls.Add(Me.ImportSFPortA)
        Me.Port1.Controls.Add(Me.PortABox)
        resources.ApplyResources(Me.Port1, "Port1")
        Me.Port1.Name = "Port1"
        '
        'SFZ1
        '
        resources.ApplyResources(Me.SFZ1, "SFZ1")
        Me.SFZ1.Name = "SFZ1"
        Me.SFZ1.UseVisualStyleBackColor = True
        '
        'ExtListPortA
        '
        resources.ApplyResources(Me.ExtListPortA, "ExtListPortA")
        Me.ExtListPortA.Name = "ExtListPortA"
        Me.ExtListPortA.UseVisualStyleBackColor = True
        '
        'DisableCheckPortA
        '
        resources.ApplyResources(Me.DisableCheckPortA, "DisableCheckPortA")
        Me.DisableCheckPortA.Name = "DisableCheckPortA"
        Me.DisableCheckPortA.UseVisualStyleBackColor = True
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'ClearPortA
        '
        resources.ApplyResources(Me.ClearPortA, "ClearPortA")
        Me.ClearPortA.Name = "ClearPortA"
        Me.ClearPortA.UseVisualStyleBackColor = True
        '
        'MoveDownPortA
        '
        resources.ApplyResources(Me.MoveDownPortA, "MoveDownPortA")
        Me.MoveDownPortA.Name = "MoveDownPortA"
        Me.MoveDownPortA.UseVisualStyleBackColor = True
        '
        'MoveUpPortA
        '
        resources.ApplyResources(Me.MoveUpPortA, "MoveUpPortA")
        Me.MoveUpPortA.Name = "MoveUpPortA"
        Me.MoveUpPortA.UseVisualStyleBackColor = True
        '
        'RemoveSFPortA
        '
        resources.ApplyResources(Me.RemoveSFPortA, "RemoveSFPortA")
        Me.RemoveSFPortA.Name = "RemoveSFPortA"
        Me.RemoveSFPortA.UseVisualStyleBackColor = True
        '
        'ImportSFPortA
        '
        resources.ApplyResources(Me.ImportSFPortA, "ImportSFPortA")
        Me.ImportSFPortA.Name = "ImportSFPortA"
        Me.ImportSFPortA.UseVisualStyleBackColor = True
        '
        'PortABox
        '
        Me.PortABox.FormattingEnabled = True
        resources.ApplyResources(Me.PortABox, "PortABox")
        Me.PortABox.Name = "PortABox"
        Me.PortABox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        '
        'Tabs1
        '
        resources.ApplyResources(Me.Tabs1, "Tabs1")
        Me.Tabs1.Controls.Add(Me.Port1)
        Me.Tabs1.Controls.Add(Me.Port2)
        Me.Tabs1.Controls.Add(Me.Port3)
        Me.Tabs1.Controls.Add(Me.Port4)
        Me.Tabs1.Controls.Add(Me.BlacklistSys)
        Me.Tabs1.Controls.Add(Me.Settings)
        Me.Tabs1.Controls.Add(Me.AdvSettings)
        Me.Tabs1.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.Tabs1.Name = "Tabs1"
        Me.Tabs1.SelectedIndex = 0
        '
        'AdvSettings
        '
        Me.AdvSettings.BackColor = System.Drawing.SystemColors.Control
        Me.AdvSettings.Controls.Add(Me.RealTimeSet2)
        Me.AdvSettings.Controls.Add(Me.Label7)
        Me.AdvSettings.Controls.Add(Me.BlackMIDIPreset2)
        Me.AdvSettings.Controls.Add(Me.RealTimeSetText2)
        Me.AdvSettings.Controls.Add(Me.AdvancedApply)
        Me.AdvSettings.Controls.Add(Me.AdvancedReset)
        Me.AdvSettings.Controls.Add(Me.AdvPanel)
        resources.ApplyResources(Me.AdvSettings, "AdvSettings")
        Me.AdvSettings.Name = "AdvSettings"
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        '
        'BlackMIDIPreset2
        '
        resources.ApplyResources(Me.BlackMIDIPreset2, "BlackMIDIPreset2")
        Me.BlackMIDIPreset2.Name = "BlackMIDIPreset2"
        Me.BlackMIDIPreset2.UseVisualStyleBackColor = True
        '
        'RealTimeSetText2
        '
        resources.ApplyResources(Me.RealTimeSetText2, "RealTimeSetText2")
        Me.RealTimeSetText2.Name = "RealTimeSetText2"
        '
        'AdvancedApply
        '
        resources.ApplyResources(Me.AdvancedApply, "AdvancedApply")
        Me.AdvancedApply.Name = "AdvancedApply"
        Me.AdvancedApply.UseVisualStyleBackColor = True
        '
        'AdvancedReset
        '
        resources.ApplyResources(Me.AdvancedReset, "AdvancedReset")
        Me.AdvancedReset.Name = "AdvancedReset"
        Me.AdvancedReset.UseVisualStyleBackColor = True
        '
        'AdvPanel
        '
        Me.AdvPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.AdvPanel.Controls.Add(Me.GroupBox5)
        Me.AdvPanel.Controls.Add(Me.GroupBox3)
        resources.ApplyResources(Me.AdvPanel, "AdvPanel")
        Me.AdvPanel.Name = "AdvPanel"
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.SincInter)
        Me.GroupBox5.Controls.Add(Me.TracksLimit)
        Me.GroupBox5.Controls.Add(Me.NoDX8FX)
        Me.GroupBox5.Controls.Add(Me.Label4)
        Me.GroupBox5.Controls.Add(Me.BufferText)
        Me.GroupBox5.Controls.Add(Me.BufferWarning)
        Me.GroupBox5.Controls.Add(Me.SysResetIgnore)
        Me.GroupBox5.Controls.Add(Me.bufsize)
        resources.ApplyResources(Me.GroupBox5, "GroupBox5")
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.TabStop = False
        '
        'SincInter
        '
        resources.ApplyResources(Me.SincInter, "SincInter")
        Me.SincInter.Name = "SincInter"
        Me.SincInter.UseVisualStyleBackColor = True
        '
        'BufferWarning
        '
        Me.BufferWarning.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BufferWarning.Image = Global.keppydrvcfg.My.Resources.Resources.Warning_Box_Yellow
        resources.ApplyResources(Me.BufferWarning, "BufferWarning")
        Me.BufferWarning.Name = "BufferWarning"
        Me.BufferWarning.TabStop = False
        '
        'SysResetIgnore
        '
        resources.ApplyResources(Me.SysResetIgnore, "SysResetIgnore")
        Me.SysResetIgnore.Name = "SysResetIgnore"
        Me.SysResetIgnore.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.EchoFXNum)
        Me.GroupBox3.Controls.Add(Me.EchoFX)
        Me.GroupBox3.Controls.Add(Me.Label12)
        Me.GroupBox3.Controls.Add(Me.SittingFXNum)
        Me.GroupBox3.Controls.Add(Me.DistortionFXNum)
        Me.GroupBox3.Controls.Add(Me.GargleFXNum)
        Me.GroupBox3.Controls.Add(Me.SittingFX)
        Me.GroupBox3.Controls.Add(Me.DistortionFX)
        Me.GroupBox3.Controls.Add(Me.CompressorFXNum)
        Me.GroupBox3.Controls.Add(Me.FlangerFXNum)
        Me.GroupBox3.Controls.Add(Me.ChorusFXNum)
        Me.GroupBox3.Controls.Add(Me.ReverbFXNum)
        Me.GroupBox3.Controls.Add(Me.CompressorFX)
        Me.GroupBox3.Controls.Add(Me.FlangerFX)
        Me.GroupBox3.Controls.Add(Me.ChorusFX)
        Me.GroupBox3.Controls.Add(Me.GargleFX)
        Me.GroupBox3.Controls.Add(Me.ReverbFX)
        resources.ApplyResources(Me.GroupBox3, "GroupBox3")
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.TabStop = False
        '
        'EchoFXNum
        '
        resources.ApplyResources(Me.EchoFXNum, "EchoFXNum")
        Me.EchoFXNum.Maximum = New Decimal(New Integer() {8, 0, 0, 0})
        Me.EchoFXNum.Name = "EchoFXNum"
        Me.EchoFXNum.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'EchoFX
        '
        resources.ApplyResources(Me.EchoFX, "EchoFX")
        Me.EchoFX.Name = "EchoFX"
        Me.EchoFX.UseVisualStyleBackColor = True
        '
        'Label12
        '
        resources.ApplyResources(Me.Label12, "Label12")
        Me.Label12.Name = "Label12"
        '
        'SittingFXNum
        '
        resources.ApplyResources(Me.SittingFXNum, "SittingFXNum")
        Me.SittingFXNum.Maximum = New Decimal(New Integer() {8, 0, 0, 0})
        Me.SittingFXNum.Name = "SittingFXNum"
        Me.SittingFXNum.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'DistortionFXNum
        '
        resources.ApplyResources(Me.DistortionFXNum, "DistortionFXNum")
        Me.DistortionFXNum.Maximum = New Decimal(New Integer() {8, 0, 0, 0})
        Me.DistortionFXNum.Name = "DistortionFXNum"
        Me.DistortionFXNum.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'GargleFXNum
        '
        resources.ApplyResources(Me.GargleFXNum, "GargleFXNum")
        Me.GargleFXNum.Maximum = New Decimal(New Integer() {8, 0, 0, 0})
        Me.GargleFXNum.Name = "GargleFXNum"
        Me.GargleFXNum.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'SittingFX
        '
        resources.ApplyResources(Me.SittingFX, "SittingFX")
        Me.SittingFX.Name = "SittingFX"
        Me.SittingFX.UseVisualStyleBackColor = True
        '
        'DistortionFX
        '
        resources.ApplyResources(Me.DistortionFX, "DistortionFX")
        Me.DistortionFX.Name = "DistortionFX"
        Me.DistortionFX.UseVisualStyleBackColor = True
        '
        'CompressorFXNum
        '
        resources.ApplyResources(Me.CompressorFXNum, "CompressorFXNum")
        Me.CompressorFXNum.Maximum = New Decimal(New Integer() {8, 0, 0, 0})
        Me.CompressorFXNum.Name = "CompressorFXNum"
        Me.CompressorFXNum.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'FlangerFXNum
        '
        resources.ApplyResources(Me.FlangerFXNum, "FlangerFXNum")
        Me.FlangerFXNum.Maximum = New Decimal(New Integer() {8, 0, 0, 0})
        Me.FlangerFXNum.Name = "FlangerFXNum"
        Me.FlangerFXNum.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'ChorusFXNum
        '
        resources.ApplyResources(Me.ChorusFXNum, "ChorusFXNum")
        Me.ChorusFXNum.Maximum = New Decimal(New Integer() {8, 0, 0, 0})
        Me.ChorusFXNum.Name = "ChorusFXNum"
        Me.ChorusFXNum.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'ReverbFXNum
        '
        resources.ApplyResources(Me.ReverbFXNum, "ReverbFXNum")
        Me.ReverbFXNum.Maximum = New Decimal(New Integer() {8, 0, 0, 0})
        Me.ReverbFXNum.Name = "ReverbFXNum"
        Me.ReverbFXNum.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'CompressorFX
        '
        resources.ApplyResources(Me.CompressorFX, "CompressorFX")
        Me.CompressorFX.Name = "CompressorFX"
        Me.CompressorFX.UseVisualStyleBackColor = True
        '
        'FlangerFX
        '
        resources.ApplyResources(Me.FlangerFX, "FlangerFX")
        Me.FlangerFX.Name = "FlangerFX"
        Me.FlangerFX.UseVisualStyleBackColor = True
        '
        'ChorusFX
        '
        resources.ApplyResources(Me.ChorusFX, "ChorusFX")
        Me.ChorusFX.Name = "ChorusFX"
        Me.ChorusFX.UseVisualStyleBackColor = True
        '
        'GargleFX
        '
        resources.ApplyResources(Me.GargleFX, "GargleFX")
        Me.GargleFX.Name = "GargleFX"
        Me.GargleFX.UseVisualStyleBackColor = True
        '
        'ReverbFX
        '
        resources.ApplyResources(Me.ReverbFX, "ReverbFX")
        Me.ReverbFX.Name = "ReverbFX"
        Me.ReverbFX.UseVisualStyleBackColor = True
        '
        'MainWindow
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.Controls.Add(Me.Tabs1)
        Me.Controls.Add(Me.MainMenu)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MainMenuStrip = Me.MainMenu
        Me.MaximizeBox = False
        Me.Name = "MainWindow"
        CType(Me.PolyphonyLimit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TracksLimit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.bufsize, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MainMenu.ResumeLayout(False)
        Me.MainMenu.PerformLayout()
        Me.Settings.ResumeLayout(False)
        Me.Settings.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.VolumeBar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.VoiceWarning, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.BlacklistSys.ResumeLayout(False)
        Me.BlacklistSys.PerformLayout()
        Me.Port4.ResumeLayout(False)
        Me.Port4.PerformLayout()
        Me.Port3.ResumeLayout(False)
        Me.Port3.PerformLayout()
        Me.Port2.ResumeLayout(False)
        Me.Port2.PerformLayout()
        Me.Port1.ResumeLayout(False)
        Me.Port1.PerformLayout()
        Me.Tabs1.ResumeLayout(False)
        Me.AdvSettings.ResumeLayout(False)
        Me.AdvSettings.PerformLayout()
        Me.AdvPanel.ResumeLayout(False)
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        CType(Me.BufferWarning, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.EchoFXNum, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SittingFXNum, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DistortionFXNum, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GargleFXNum, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CompressorFXNum, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.FlangerFXNum, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ChorusFXNum, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ReverbFXNum, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PortAOpenDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents PortBOpenDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Information As System.Windows.Forms.ToolTip
    Friend WithEvents BlackListFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ClockTimer As System.Windows.Forms.Timer
    Friend WithEvents MainMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents AppToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CheckForUpdatesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents InformationsAboutThisProgramToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DownloadTheSourceCodeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents VisitKeppyStudiosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ReportABugToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CanIChangeTheSoundfontInRealtimeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents FirstRunTimer As System.Windows.Forms.Timer
    Friend WithEvents FXTimer As System.Windows.Forms.Timer
    Friend WithEvents ExtListPortADialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ExtListPortBDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents PortCOpenDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents PortDOpenDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ExtListPortCDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ExtListPortDDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Settings As System.Windows.Forms.TabPage
    Friend WithEvents RealTimeSet As System.Windows.Forms.CheckBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents VolumeBar As System.Windows.Forms.TrackBar
    Friend WithEvents CurrentVolumeHUE As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents VoiceWarning As System.Windows.Forms.PictureBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Frequency As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents MaxCPU As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents PolyphonyLimit As System.Windows.Forms.NumericUpDown
    Friend WithEvents DisableFX As System.Windows.Forms.CheckBox
    Friend WithEvents Preload As System.Windows.Forms.CheckBox
    Friend WithEvents NoteOff As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents ClockSys As System.Windows.Forms.Label
    Friend WithEvents Versionlabel As System.Windows.Forms.Label
    Friend WithEvents LatestVersionDriver As System.Windows.Forms.Label
    Friend WithEvents UpdateDownload As System.Windows.Forms.Button
    Friend WithEvents ThisVersionDriver As System.Windows.Forms.Label
    Friend WithEvents BlackMIDIPreset As System.Windows.Forms.Button
    Friend WithEvents RealTimeSetText As System.Windows.Forms.Label
    Friend WithEvents Apply As System.Windows.Forms.Button
    Friend WithEvents Reset As System.Windows.Forms.Button
    Friend WithEvents BlacklistSys As System.Windows.Forms.TabPage
    Friend WithEvents ClearBlacklist As System.Windows.Forms.Button
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents ManualBlackListLabel As System.Windows.Forms.Label
    Friend WithEvents ManualBlackList As System.Windows.Forms.TextBox
    Friend WithEvents BlackListDef As System.Windows.Forms.Label
    Friend WithEvents BlackListAdvancedMode As System.Windows.Forms.CheckBox
    Friend WithEvents SystemList As System.Windows.Forms.Button
    Friend WithEvents RemoveBlackList As System.Windows.Forms.Button
    Friend WithEvents AddBlackList As System.Windows.Forms.Button
    Friend WithEvents UserProgramsBlackList As System.Windows.Forms.ListBox
    Friend WithEvents Port4 As System.Windows.Forms.TabPage
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents DisableCheckPortD As System.Windows.Forms.CheckBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents Button10 As System.Windows.Forms.Button
    Friend WithEvents Button11 As System.Windows.Forms.Button
    Friend WithEvents Button12 As System.Windows.Forms.Button
    Friend WithEvents Button13 As System.Windows.Forms.Button
    Friend WithEvents PortDBox As System.Windows.Forms.ListBox
    Friend WithEvents Port3 As System.Windows.Forms.TabPage
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents DisableCheckPortC As System.Windows.Forms.CheckBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button6 As System.Windows.Forms.Button
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents PortCBox As System.Windows.Forms.ListBox
    Friend WithEvents Port2 As System.Windows.Forms.TabPage
    Friend WithEvents ExtListPortB As System.Windows.Forms.Button
    Friend WithEvents DisableCheckPortB As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ClearPortB As System.Windows.Forms.Button
    Friend WithEvents MoveDownPortB As System.Windows.Forms.Button
    Friend WithEvents MoveUpPortB As System.Windows.Forms.Button
    Friend WithEvents RemoveSFPortB As System.Windows.Forms.Button
    Friend WithEvents ImportSFPortB As System.Windows.Forms.Button
    Friend WithEvents PortBBox As System.Windows.Forms.ListBox
    Friend WithEvents Port1 As System.Windows.Forms.TabPage
    Friend WithEvents ExtListPortA As System.Windows.Forms.Button
    Friend WithEvents DisableCheckPortA As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ClearPortA As System.Windows.Forms.Button
    Friend WithEvents MoveDownPortA As System.Windows.Forms.Button
    Friend WithEvents MoveUpPortA As System.Windows.Forms.Button
    Friend WithEvents RemoveSFPortA As System.Windows.Forms.Button
    Friend WithEvents ImportSFPortA As System.Windows.Forms.Button
    Friend WithEvents PortABox As System.Windows.Forms.ListBox
    Friend WithEvents Tabs1 As System.Windows.Forms.TabControl
    Friend WithEvents OpenDebugWindowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CurrentVolumeHUE2 As System.Windows.Forms.Label
    Friend WithEvents SFZ4 As System.Windows.Forms.Button
    Friend WithEvents SFZ3 As System.Windows.Forms.Button
    Friend WithEvents SFZ2 As System.Windows.Forms.Button
    Friend WithEvents SFZ1 As System.Windows.Forms.Button
    Friend WithEvents AdvSettings As System.Windows.Forms.TabPage
    Friend WithEvents RealTimeSet2 As System.Windows.Forms.CheckBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents BlackMIDIPreset2 As System.Windows.Forms.Button
    Friend WithEvents RealTimeSetText2 As System.Windows.Forms.Label
    Friend WithEvents AdvancedApply As System.Windows.Forms.Button
    Friend WithEvents AdvancedReset As System.Windows.Forms.Button
    Friend WithEvents AdvPanel As System.Windows.Forms.Panel
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents EchoFXNum As System.Windows.Forms.NumericUpDown
    Friend WithEvents EchoFX As System.Windows.Forms.CheckBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents SittingFXNum As System.Windows.Forms.NumericUpDown
    Friend WithEvents DistortionFXNum As System.Windows.Forms.NumericUpDown
    Friend WithEvents GargleFXNum As System.Windows.Forms.NumericUpDown
    Friend WithEvents SittingFX As System.Windows.Forms.CheckBox
    Friend WithEvents DistortionFX As System.Windows.Forms.CheckBox
    Friend WithEvents CompressorFXNum As System.Windows.Forms.NumericUpDown
    Friend WithEvents FlangerFXNum As System.Windows.Forms.NumericUpDown
    Friend WithEvents ChorusFXNum As System.Windows.Forms.NumericUpDown
    Friend WithEvents ReverbFXNum As System.Windows.Forms.NumericUpDown
    Friend WithEvents CompressorFX As System.Windows.Forms.CheckBox
    Friend WithEvents FlangerFX As System.Windows.Forms.CheckBox
    Friend WithEvents ChorusFX As System.Windows.Forms.CheckBox
    Friend WithEvents GargleFX As System.Windows.Forms.CheckBox
    Friend WithEvents ReverbFX As System.Windows.Forms.CheckBox
    Friend WithEvents NoDX8FX As System.Windows.Forms.CheckBox
    Friend WithEvents SincInter As System.Windows.Forms.CheckBox
    Friend WithEvents BufferWarning As System.Windows.Forms.PictureBox
    Friend WithEvents bufsize As System.Windows.Forms.NumericUpDown
    Friend WithEvents SysResetIgnore As System.Windows.Forms.CheckBox
    Friend WithEvents BufferText As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TracksLimit As System.Windows.Forms.NumericUpDown
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox

End Class
