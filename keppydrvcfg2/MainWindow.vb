Imports System.IO
Imports System.Drawing
Imports System.Net
Imports Microsoft.Win32
Imports System.Drawing.Text
Imports System.Runtime.InteropServices
Imports System.Diagnostics

Module VolumeCustomFont

    Private _pfc As PrivateFontCollection = Nothing

    Public ReadOnly Property GetInstance(ByVal Size As Single, ByVal style As FontStyle) As Font
        Get
            If _pfc Is Nothing Then LoadFont()
            Return New Font(_pfc.Families(0), Size, style)
        End Get
    End Property

    Private Sub LoadFont()
        Try
            _pfc = New PrivateFontCollection
            Dim fontMemPointer As IntPtr = Marshal.AllocCoTaskMem(My.Resources.kepsdigital.Length)
            Marshal.Copy(My.Resources.kepsdigital, 0, fontMemPointer, My.Resources.kepsdigital.Length)
            _pfc.AddMemoryFont(fontMemPointer, My.Resources.kepsdigital.Length)
            Marshal.FreeCoTaskMem(fontMemPointer)
        Catch ex As Exception
            MsgBox("Fatal error while loading custom font. Press OK to quit.")
            Application.Exit()
        End Try
    End Sub
End Module

Public Class MainWindow

    Public Declare Function GetAsyncKeyState Lib "user32.dll" (ByVal vKey As Int32) As UShort 'Used for the ENTER button, to manually add names to the blacklist
    Private f_ScrollH As UInteger
    Private f_ScrollW As UInteger

    Function CheckForAlphaCharacters(ByVal StringToCheck As String)
        For i = 0 To StringToCheck.Length - 1
            If Char.IsLetter(StringToCheck.Chars(i)) Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ClockTimer.Start()
        Dim Driver As FileVersionInfo = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\keppydrv\keppydrv.dll")

        Dim UserString As String
        UserString = "Software\Keppy's Driver"
        Dim keppykey = My.Computer.Registry.CurrentUser.OpenSubKey(UserString + "\Settings", True)
        Dim keppykeyfx = My.Computer.Registry.CurrentUser.OpenSubKey(UserString + "\Effects", True)
        Dim IsDebugActivated As Integer
        
        Dim strStartupArguments() As String, intCount As Integer
        strStartupArguments = System.Environment.GetCommandLineArgs
        For intCount = 0 To UBound(strStartupArguments)
            Select Case strStartupArguments(intCount).ToLower
                Case "-debug"
                    IsDebugActivated = 1
                    Win32.AllocConsole()
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.ForegroundColor = ConsoleColor.Magenta
                    Console.Title = "Keppy's Driver (Configurator) - Debug Window"
                    Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Keppy's Driver (Configurator)")
                    Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Version number: " + Driver.FileVersion.ToString)
                    Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Debug started.")
                    Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "-----------------------------------------------------")
                    Select Case MsgBox("The configurator has been started in Debug mode. Continue?", MsgBoxStyle.YesNo, "Debug Mode enabled")
                        Case MsgBoxResult.Yes
                            'NULL
                        Case MsgBoxResult.No
                            Application.Exit()
                    End Select
            End Select
        Next intCount

        CurrentVolumeHUE.Font = VolumeCustomFont.GetInstance(48, FontStyle.Italic) 'The fancy font for the volume label!

        Dim osVer As Version = Environment.OSVersion.Version 'Fancy stuff to see what O.S. you're using
        Dim osName As String = Environment.ProcessorCount.ToString
        If osVer.Major = 10 Then
            Versionlabel.Text = "Your current O.S. is: Windows 10 or Windows Server 2016"
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Current O.S. version: Windows 10/Windows Server 2016")
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Current O.S. build number: " + osVer.ToString)
        ElseIf osVer.Major = 6 And osVer.Minor = 3 Then
            Versionlabel.Text = "Your current O.S. is: Windows 8.1 or Windows Server 2012 R2"
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Current O.S. version: Windows 8.1/Windows Server 2012 R2")
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Current O.S. build number: " + osVer.ToString)
        ElseIf osVer.Major = 6 And osVer.Minor = 2 Then
            Versionlabel.Text = "Your current O.S. is: Windows 8 or Windows Server 2012"
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Current O.S. version: Windows 8.0/Windows Server 2012")
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Current O.S. build number: " + osVer.ToString)
        ElseIf osVer.Major = 6 And osVer.Minor = 1 Then
            Versionlabel.Text = "Your current O.S. is: Windows 7 or Windows Server 2008 R2"
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Current O.S. version: Windows 7/Windows Server 2008 R2")
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Current O.S. build number: " + osVer.ToString)
        ElseIf osVer.Major = 6 And osVer.Minor = 0 Then
            Versionlabel.Text = "Your current O.S. is: Windows Vista or Windows Server 2008"
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Current O.S. version: Windows Vista/Windows Server 2008")
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Current O.S. build number: " + osVer.ToString)
        ElseIf osVer.Major = 5 And osVer.Minor = 2 Then
            Versionlabel.Text = "Your current O.S. is: Windows XP (32-bit)"
            FloatingDisabled.Enabled = False
            FloatingDisabled.Checked = True
            keppykey.SetValue("nofloat", "1")
            Versionlabel.Text = "Your current O.S. is: Windows XP (64-bit) or Windows Server 2003"
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Current O.S. version: Windows XP x64/Windows Server 2003")
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Current O.S. build number: " + osVer.ToString)
        ElseIf osVer.Major = 5 And osVer.Minor = 1 Then
            Versionlabel.Text = "Your current O.S. is: Windows XP (32-bit)"
            FloatingDisabled.Enabled = False
            FloatingDisabled.Checked = True
            keppykey.SetValue("nofloat", "1")
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Current O.S. version: Windows XP (Unsupported)")
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Current O.S. build number: " + osVer.ToString)
        ElseIf osVer.Major = 5 And osVer.Minor = 0 Then
            Versionlabel.Text = "Your current O.S. is: Windows 2000"
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Current O.S. version: Windows 2000 (Unsupported)")
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Current O.S. build number: " + osVer.ToString)
        Else
            Versionlabel.Text = "Your current O.S. is: Unknown"
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Current O.S. version: Unknown")
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Current O.S. build number: " + osVer.ToString)
        End If

        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Number of physical cores: " + osName)
        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "-----------------------------------------------------")
        Console.ForegroundColor = ConsoleColor.Green

        If (Not System.IO.Directory.Exists(Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\")) Then
            System.IO.Directory.CreateDirectory(Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\")
            Try 'Checks if there are updates for the driver
                Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create("https://raw.githubusercontent.com/KaleidonKep99/Keppy-s-MIDI-Driver/master/output/keppydriverupdate.txt")
                Dim response As System.Net.HttpWebResponse = request.GetResponse()
                Dim sr As System.IO.StreamReader = New System.IO.StreamReader(response.GetResponseStream())
                Dim newestversion As String = sr.ReadToEnd()
                ThisVersionDriver.Text = "The current version of the driver, installed on your system, is: " + Driver.FileVersion.ToString
                LatestVersionDriver.Text = "The latest version online, in the GitHub repository, is: " + newestversion.ToString
                If newestversion > Driver.FileVersion Then
                    UpdateDownload.Text = "Download update"
                Else

                End If
            Catch ex As Exception
                ThisVersionDriver.Text = "The current version of the driver, installed on your system, is: " + Driver.FileVersion.ToString
                LatestVersionDriver.Text = "Can not check for updates. You're offline, or maybe the website is temporarily down."
            End Try

            Try 'Checks if the list for Port A exists
                Dim PortASFList As String = (Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidi.sflist")
                Dim reader As StreamReader = New StreamReader(New FileStream(PortASFList, FileMode.Open))
                Do While Not reader.EndOfStream
                    PortABox.Items.Add(reader.ReadLine())
                Loop
                reader.Close()
            Catch ex As Exception
                Dim PortASFList As String = (Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidi.sflist")
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Can not read " + "''" + PortASFList + "''!")
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Trying to create file...")
                Try
                    File.Create(Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidi.sflist").Dispose()
                Catch exc As Exception
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Can not create file!")
                    Console.ForegroundColor = ConsoleColor.Green
                End Try
            End Try

            Try 'Does the same for Port B
                Dim PortBSFList As String = (Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidib.sflist")
                Dim reader2 As StreamReader = New StreamReader(New FileStream(PortBSFList, FileMode.Open))
                Do While Not reader2.EndOfStream
                    PortBBox.Items.Add(reader2.ReadLine())
                Loop
                reader2.Close()
            Catch ex As Exception
                Dim PortBSFList As String = (Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidib.sflist")
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Can not read " + "''" + PortBSFList + "''!")
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Trying to create file...")
                Try
                    File.Create(Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidib.sflist").Dispose()
                Catch exc As Exception
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Can not create file!")
                    Console.ForegroundColor = ConsoleColor.Green
                End Try
            End Try

            Try 'Does the same for Port C
                Dim PortCSFList As String = (Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidic.sflist")
                Dim reader2 As StreamReader = New StreamReader(New FileStream(PortCSFList, FileMode.Open))
                Do While Not reader2.EndOfStream
                    PortCBox.Items.Add(reader2.ReadLine())
                Loop
                reader2.Close()
            Catch ex As Exception
                Dim PortCSFList As String = (Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidic.sflist")
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Can not read " + "''" + PortCSFList + "''!")
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Trying to create file...")
                Try
                    File.Create(Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidic.sflist").Dispose()
                Catch exc As Exception
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Can not create file!")
                    Console.ForegroundColor = ConsoleColor.Green
                End Try
            End Try

            Try 'Does the same for Port D
                Dim PortDSFList As String = (Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidid.sflist")
                Dim reader2 As StreamReader = New StreamReader(New FileStream(PortDSFList, FileMode.Open))
                Do While Not reader2.EndOfStream
                    PortDBox.Items.Add(reader2.ReadLine())
                Loop
                reader2.Close()
            Catch ex As Exception
                Dim PortDSFList As String = (Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidid.sflist")
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Can not read " + "''" + PortDSFList + "''!")
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Trying to create file...")
                Try
                    File.Create(Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidid.sflist").Dispose()
                Catch exc As Exception
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Can not create file!")
                    Console.ForegroundColor = ConsoleColor.Green
                End Try
            End Try
        Else
            Try 'Checks if there are updates for the driver
                Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create("https://raw.githubusercontent.com/KaleidonKep99/Keppy-s-MIDI-Driver/master/output/keppydriverupdate.txt")
                Dim response As System.Net.HttpWebResponse = request.GetResponse()
                Dim sr As System.IO.StreamReader = New System.IO.StreamReader(response.GetResponseStream())
                Dim newestversion As String = sr.ReadToEnd()
                ThisVersionDriver.Text = "The current version of the driver, installed on your system, is: " + Driver.FileVersion.ToString
                LatestVersionDriver.Text = "The latest version online, in the GitHub repository, is: " + newestversion.ToString
                If newestversion > Driver.FileVersion Then
                    UpdateDownload.Text = "Download update"
                Else

                End If
            Catch ex As Exception
                ThisVersionDriver.Text = "The current version of the driver, installed on your system, is: " + Driver.FileVersion.ToString
                LatestVersionDriver.Text = "Can not check for updates. You're offline, or maybe the website is temporarily down."
            End Try

            Try 'Checks if the list for Port A exists
                Dim PortASFList As String = (Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidi.sflist")
                Dim reader As StreamReader = New StreamReader(New FileStream(PortASFList, FileMode.Open))
                Do While Not reader.EndOfStream
                    PortABox.Items.Add(reader.ReadLine())
                Loop
                reader.Close()
            Catch ex As Exception
                Dim PortASFList As String = (Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidi.sflist")
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Can not read " + "''" + PortASFList + "''!")
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Trying to create file...")
                Try
                    File.Create(Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidi.sflist").Dispose()
                Catch exc As Exception
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Can not create file!")
                    Console.ForegroundColor = ConsoleColor.Green
                End Try
            End Try

            Try 'Does the same for Port B
                Dim PortBSFList As String = (Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidib.sflist")
                Dim reader2 As StreamReader = New StreamReader(New FileStream(PortBSFList, FileMode.Open))
                Do While Not reader2.EndOfStream
                    PortBBox.Items.Add(reader2.ReadLine())
                Loop
                reader2.Close()
            Catch ex As Exception
                Dim PortBSFList As String = (Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidib.sflist")
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Can not read " + "''" + PortBSFList + "''!")
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Trying to create file...")
                Try
                    File.Create(Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidib.sflist").Dispose()
                Catch exc As Exception
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Can not create file!")
                    Console.ForegroundColor = ConsoleColor.Green
                End Try
            End Try

            Try 'Does the same for Port C
                Dim PortCSFList As String = (Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidic.sflist")
                Dim reader2 As StreamReader = New StreamReader(New FileStream(PortCSFList, FileMode.Open))
                Do While Not reader2.EndOfStream
                    PortCBox.Items.Add(reader2.ReadLine())
                Loop
                reader2.Close()
            Catch ex As Exception
                Dim PortCSFList As String = (Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidic.sflist")
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Can not read " + "''" + PortCSFList + "''!")
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Trying to create file...")
                Try
                    File.Create(Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidic.sflist").Dispose()
                Catch exc As Exception
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Can not create file!")
                    Console.ForegroundColor = ConsoleColor.Green
                End Try
            End Try

            Try 'Does the same for Port D
                Dim PortDSFList As String = (Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidid.sflist")
                Dim reader2 As StreamReader = New StreamReader(New FileStream(PortDSFList, FileMode.Open))
                Do While Not reader2.EndOfStream
                    PortDBox.Items.Add(reader2.ReadLine())
                Loop
                reader2.Close()
            Catch ex As Exception
                Dim PortDSFList As String = (Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidid.sflist")
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Can not read " + "''" + PortDSFList + "''!")
                Console.ForegroundColor = ConsoleColor.Green
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Trying to create file...")
                Try
                    File.Create(Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidid.sflist").Dispose()
                Catch exc As Exception
                    Console.ForegroundColor = ConsoleColor.Red
                    Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Can not create file!")
                    Console.ForegroundColor = ConsoleColor.Green
                End Try
            End Try
        End If

        Try 'Again, the same as for both Port A, Port B, Port C and Port D but for the blacklist system
            Dim BlackList As String = (Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\blacklist\keppymididrv.blacklist")
            Dim reader3 As StreamReader = New StreamReader(New FileStream(Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\blacklist\keppymididrv.blacklist", FileMode.Open))
            Do While Not reader3.EndOfStream
                UserProgramsBlackList.Items.Add(reader3.ReadLine())
            Loop
            reader3.Close()
        Catch ex As Exception
            Try
                File.Create(Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\blacklist\keppymididrv.blacklist").Dispose()
            Catch exc As Exception
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Error while creating file: " + Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\blacklist\keppymididrv.blacklist")
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Is the folder " + Environment.GetEnvironmentVariable("LocalAppData") + " accessible by the user/locked by SYSTEM?")
                Console.ForegroundColor = ConsoleColor.Green
            End Try
        End Try

        Try
            MaxCPU.Text = keppykey.GetValue("cpu")
            PolyphonyLimit.Value = keppykey.GetValue("polyphony")
            bufsize.Value = keppykey.GetValue("buflen")
            TracksLimit.Value = keppykey.GetValue("tracks")
            VolumeBar.Value = keppykey.GetValue("volume")
            CurrentVolumeHUE2.Text = "Int: " & keppykey.GetValue("volume")
            If keppykey.GetValue("cpu") = "0" Then
                MaxCPU.Text = "Disabled"
            Else
                MaxCPU.Text = keppykey.GetValue("cpu")
            End If
            Frequency.Text = keppykey.GetValue("frequency")
            Dim lnumber As Double
            Dim lResult As Long
            lnumber = keppykey.GetValue("volume") / 100
            lResult = Int(lnumber)
            If keppykey.GetValue("preload") = 1 Then
                Preload.Checked = True
            Else
                Preload.Checked = False
            End If
            If keppykey.GetValue("sysresetignore") = 1 Then
                SysResetIgnore.Checked = True
            Else
                SysResetIgnore.Checked = False
            End If
            If keppykey.GetValue("nofx") = 1 Then
                DisableFX.Checked = True
            Else
                DisableFX.Checked = False
            End If
            If keppykey.GetValue("nodx8") = 1 Then
                NoDX8FX.Checked = True
            Else
                NoDX8FX.Checked = False
            End If
            If keppykey.GetValue("softwaremode") = 1 Then
                SoftwareRendering.Checked = True
            Else
                SoftwareRendering.Checked = False
            End If
            If keppykey.GetValue("nofloat") = 0 Then
                FloatingDisabled.Checked = True
            Else
                FloatingDisabled.Checked = False
            End If
            If keppykey.GetValue("noteoff") = 1 Then
                NoteOff.Checked = True
            Else
                NoteOff.Checked = False
            End If
            If keppykey.GetValue("realtimeset") = 1 Then
                RealTimeSet.Checked = True
            Else
                RealTimeSet.Checked = False
            End If
            If keppykey.GetValue("sinc") = 1 Then
                SincInter.Checked = True
            Else
                SincInter.Checked = False
            End If
            If keppykeyfx.GetValue("reverbfx") = 1 Then
                ReverbFX.Checked = True
            Else
                ReverbFX.Checked = False
            End If
            If keppykeyfx.GetValue("chorusfx") = 1 Then
                ChorusFX.Checked = True
            Else
                ChorusFX.Checked = False
            End If
            If keppykeyfx.GetValue("echofx") = 1 Then
                EchoFX.Checked = True
            Else
                EchoFX.Checked = False
            End If
            If keppykeyfx.GetValue("flangerfx") = 1 Then
                FlangerFX.Checked = True
            Else
                FlangerFX.Checked = False
            End If
            If keppykeyfx.GetValue("garglefx") = 1 Then
                GargleFX.Checked = True
            Else
                GargleFX.Checked = False
            End If
            If keppykeyfx.GetValue("sittingfx") = 1 Then
                SittingFX.Checked = True
            Else
                SittingFX.Checked = False
            End If
            If keppykeyfx.GetValue("compressorfx") = 1 Then
                CompressorFX.Checked = True
            Else
                CompressorFX.Checked = False
            End If
            If keppykeyfx.GetValue("distortionfx") = 1 Then
                DistortionFX.Checked = True
            Else
                DistortionFX.Checked = False
            End If
            ReverbFXNum.Value = keppykeyfx.GetValue("reverbfxnum")
            ChorusFXNum.Value = keppykeyfx.GetValue("chorusfxnum")
            EchoFXNum.Value = keppykeyfx.GetValue("echofxnum")
            FlangerFXNum.Value = keppykeyfx.GetValue("flangerfxnum")
            GargleFXNum.Value = keppykeyfx.GetValue("garglefxnum")
            SittingFXNum.Value = keppykeyfx.GetValue("sittingfxnum")
            DistortionFXNum.Value = keppykeyfx.GetValue("distortionfxnum")
            CompressorFXNum.Value = keppykeyfx.GetValue("compressorfxnum")
            Dim VolumeValue As Integer
            Dim x As Double = VolumeBar.Value.ToString / 100
            VolumeValue = Convert.ToInt32(x)
            CurrentVolumeHUE.Text = VolumeValue.ToString
            If osVer.Major >= 6 Then
                SoftwareRendering.Checked = True
                SoftwareRendering.Enabled = False
                SoftwareRendering.Text = "Only software rendering available"
                keppykey.SetValue("softwaremode", "1", RegistryValueKind.DWord)
            End If
            bufsize_ValueChanged()
        Catch ex As Exception
            If IsDebugActivated = 1 Then
                Console.Clear()
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Can not read from the registry!")
                Console.ForegroundColor = ConsoleColor.DarkCyan
                Console.WriteLine("")
                Console.WriteLine("FATAL ERROR DURING THE EXECUTION OF THIS PROGRAM.")
                Console.WriteLine("")
                Console.WriteLine("Error: Unable to read from registry")
                Console.ForegroundColor = ConsoleColor.Green
            Else
                MsgBox(ex.ToString & vbCrLf & vbCrLf & "Press OK to quit", MsgBoxStyle.Critical, "Error")
                Application.Exit()
            End If
        End Try
        Try
            If keppykey.GetValue("firstrun") = 1 Then
                FirstRunTimer.Start()
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString & vbCrLf & vbCrLf & "Press OK to quit", MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub ApplyPortA_Click()
        Try
            Dim BASSMIDIListA As String = Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidi.sflist"
            Dim Filenum As Integer = FreeFile()
            FileOpen(Filenum, BASSMIDIListA, OpenMode.Output)
            FileClose()

            Using SW As New IO.StreamWriter(BASSMIDIListA, True)
                For Each itm As String In Me.PortABox.Items
                    SW.WriteLine(itm)
                Next
                For Each itemdebug As String In Me.PortABox.Items
                    Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Saving item to port A list: " + itemdebug.ToString)
                Next
            End Using
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Soundfonts saved.")
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Dim BlackListText As String = Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\blacklist\keppymididrv.blacklist"
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Error while saving blacklist file: " + BlackListText)
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Is the file accessible by the user/locked by SYSTEM?")
            Console.ForegroundColor = ConsoleColor.Green
            MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub ApplyPortB_Click()
        Try
            Dim BASSMIDIListB As String = Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidib.sflist"
            Dim Filenum As Integer = FreeFile()
            FileOpen(Filenum, BASSMIDIListB, OpenMode.Output)
            FileClose()

            Using SW As New IO.StreamWriter(BASSMIDIListB, True)
                For Each itm As String In Me.PortBBox.Items
                    SW.WriteLine(itm)
                Next
                For Each itemdebug As String In Me.PortBBox.Items
                    Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Saving item to port B list: " + itemdebug.ToString)
                Next
            End Using
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Dim BlackListText As String = Environment.GetEnvironmentVariable("WINDIR") + "\keppymidi.sflist"
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Error while saving blacklist file: " + BlackListText)
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Is the file accessible by the user/locked by SYSTEM?")
            Console.ForegroundColor = ConsoleColor.Green
            MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub ApplyPortC_Click()
        Try
            Dim BASSMIDIListB As String = Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidic.sflist"
            Dim Filenum As Integer = FreeFile()
            FileOpen(Filenum, BASSMIDIListB, OpenMode.Output)
            FileClose()

            Using SW As New IO.StreamWriter(BASSMIDIListB, True)
                For Each itm As String In Me.PortCBox.Items
                    SW.WriteLine(itm)
                Next
                For Each itemdebug As String In Me.PortCBox.Items
                    Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Saving item to port C list: " + itemdebug.ToString)
                Next
            End Using
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Dim BlackListText As String = Environment.GetEnvironmentVariable("WINDIR") + "\keppymidi.sflist"
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Error while saving blacklist file: " + BlackListText)
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Is the file accessible by the user/locked by SYSTEM?")
            Console.ForegroundColor = ConsoleColor.Green
            MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub ApplyPortD_Click()
        Try
            Dim BASSMIDIListB As String = Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\lists\keppymidid.sflist"
            Dim Filenum As Integer = FreeFile()
            FileOpen(Filenum, BASSMIDIListB, OpenMode.Output)
            FileClose()

            Using SW As New IO.StreamWriter(BASSMIDIListB, True)
                For Each itm As String In Me.PortDBox.Items
                    SW.WriteLine(itm)
                Next
                For Each itemdebug As String In Me.PortDBox.Items
                    Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Saving item to port D list: " + itemdebug.ToString)
                Next
            End Using
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Dim BlackListText As String = Environment.GetEnvironmentVariable("WINDIR") + "\keppymidi.sflist"
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Error while saving blacklist file: " + BlackListText)
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Is the file accessible by the user/locked by SYSTEM?")
            Console.ForegroundColor = ConsoleColor.Green
            MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub ImportSFPortA_Click(sender As Object, e As EventArgs) Handles ImportSFPortA.Click
        Try
            Dim strlist As New List(Of String)
            If DisableCheckPortA.Checked = True Then
                PortAOpenDialog1.Filter = "All files (*.*)|*.*"
            Else
                PortAOpenDialog1.Filter = "SoundFont/SFZ Files (*.sf2, *.sfpack, *.sfz)|*.sf1;*.sf2;*.sf2pack;*.sfz"
            End If
            PortAOpenDialog1.FileName = ""
            If PortAOpenDialog1.ShowDialog = DialogResult.OK Then
                For Each itm In PortAOpenDialog1.FileNames
                    Dim infoReader As System.IO.FileInfo
                    infoReader = My.Computer.FileSystem.GetFileInfo(itm)
                    Dim fileName As String = itm
                    Dim fileName2 As String
                    Dim extension As String
                    extension = Path.GetExtension(fileName)
                    fileName2 = Path.GetFileName(fileName)
                    If infoReader.Length > 2147483647 Then
                        Dim Size As Integer = infoReader.Length / 1048576
                        MsgBox(fileName2.ToString + "'s size goes over the 32-bit memory barrier. (" + Size.ToString + "MB, while the limit is 2047MB)" & vbCrLf & vbCrLf & "Do not open 32-bit programs while using it.", 48, "Warning")
                        If extension.ToString = ".sfz" Then
                            Dim bank As String
                            Dim preset As String
                            bank = InputBox("Enter the number of the bank. (From 0 to 127)" & vbCrLf & itm, "Select a bank")
                            preset = InputBox("Enter the number of the preset. (From 0 to 127)" & vbCrLf & itm, "Select a preset")
                            If CheckForAlphaCharacters(bank) And CheckForAlphaCharacters(preset) Then
                                PortABox.Items.Add("p0,0=0,0|" + itm)
                            ElseIf bank.Length = 0 And preset.Length = 0 Then
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Empty values for both bank and preset.")
                                PortABox.Items.Add("p0,0=0,0|" + itm)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,0=0,0|" + itm)
                                ApplyPortA_Click()
                            Else
                                If bank > 128 Then
                                    If preset > 127 Then
                                        PortABox.Items.Add("p127,127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127,127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortABox.Items.Add("p127,0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127,0=0,0|" + itm)
                                    Else
                                        PortABox.Items.Add("p127," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortA_Click()
                                ElseIf bank < 0 Then
                                    If preset > 127 Then
                                        PortABox.Items.Add("p0,127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortABox.Items.Add("p0,0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,0=0,0|" + itm)
                                    Else
                                        PortABox.Items.Add("p0," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortA_Click()
                                Else
                                    If preset > 127 Then
                                        PortABox.Items.Add("p" + bank + ",127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + ",127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortABox.Items.Add("p" + bank + ",0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + ",0=0,0|" + itm)
                                    Else
                                        PortABox.Items.Add("p" + bank + "," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + "," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortA_Click()
                                End If
                            End If
                        ElseIf extension.ToString = ".sf2" Or extension.ToString = ".sfpack" Or extension.ToString = ".SF2" Or extension.ToString = ".SFPACK" Then
                            PortABox.Items.Add(itm)
                            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                            ApplyPortA_Click()
                        Else
                            If DisableCheckPortA.Checked = True Then
                                PortABox.Items.Add(itm)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                            Else
                                Console.ForegroundColor = ConsoleColor.Red
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Invalid format: " + extension.ToString)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                                Console.ForegroundColor = ConsoleColor.Green
                            End If
                            ApplyPortA_Click()
                        End If
                    Else
                        If extension.ToString = ".sfz" Then
                            Dim bank As String
                            Dim preset As String
                            bank = InputBox("Enter the number of the bank. (From 0 to 127)" & vbCrLf & itm, "Select a bank")
                            preset = InputBox("Enter the number of the preset. (From 0 to 127)" & vbCrLf & itm, "Select a preset")
                            If CheckForAlphaCharacters(bank) And CheckForAlphaCharacters(preset) Then
                                PortABox.Items.Add("p0,0=0,0|" + itm)
                                ApplyPortA_Click()
                            ElseIf bank.Length = 0 And preset.Length = 0 Then
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Empty values for both bank and preset.")
                                PortABox.Items.Add("p0,0=0,0|" + itm)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,0=0,0|" + itm)
                                ApplyPortA_Click()
                            Else
                                If bank > 128 Then
                                    If preset > 127 Then
                                        PortABox.Items.Add("p127,127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127,127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortABox.Items.Add("p127,0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127,0=0,0|" + itm)
                                    Else
                                        PortABox.Items.Add("p127," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortA_Click()
                                ElseIf bank < 0 Then
                                    If preset > 127 Then
                                        PortABox.Items.Add("p0,127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortABox.Items.Add("p0,0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,0=0,0|" + itm)
                                    Else
                                        PortABox.Items.Add("p0," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortA_Click()
                                Else
                                    If preset > 127 Then
                                        PortABox.Items.Add("p" + bank + ",127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + ",127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortABox.Items.Add("p" + bank + ",0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + ",0=0,0|" + itm)
                                    Else
                                        PortABox.Items.Add("p" + bank + "," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + "," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortA_Click()
                                End If
                            End If
                        ElseIf extension.ToString = ".sf2" Or extension.ToString = ".sfpack" Or extension.ToString = ".SF2" Or extension.ToString = ".SFPACK" Then
                            PortABox.Items.Add(itm)
                            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                            ApplyPortA_Click()
                        Else
                            If DisableCheckPortA.Checked = True Then
                                PortABox.Items.Add(itm)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                                ApplyPortA_Click()
                            Else
                                Console.ForegroundColor = ConsoleColor.Red
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Invalid format: " + extension.ToString)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                                Console.ForegroundColor = ConsoleColor.Green
                            End If
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub ImportSFPortB_Click(sender As Object, e As EventArgs) Handles ImportSFPortB.Click
        Try
            Dim strlist As New List(Of String)
            If DisableCheckPortB.Checked = True Then
                PortBOpenDialog1.Filter = "All files (*.*)|*.*"
            Else
                PortBOpenDialog1.Filter = "SoundFont/SFZ Files (*.sf2, *.sfpack, *.sfz)|*.sf1;*.sf2;*.sf2pack;*.sfz"
            End If
            PortBOpenDialog1.FileName = ""
            If PortBOpenDialog1.ShowDialog = DialogResult.OK Then
                For Each itm In PortBOpenDialog1.FileNames
                    Dim infoReader As System.IO.FileInfo
                    infoReader = My.Computer.FileSystem.GetFileInfo(itm)
                    Dim fileName As String = itm
                    Dim fileName2 As String
                    Dim extension As String
                    extension = Path.GetExtension(fileName)
                    fileName2 = Path.GetFileName(fileName)
                    If infoReader.Length > 2147483647 Then
                        Dim Size As Integer = infoReader.Length / 1048576
                        MsgBox(fileName2.ToString + "'s size goes over the 32-bit memory barrier. (" + Size.ToString + "MB, while the limit is 2047MB)" & vbCrLf & vbCrLf & "Do not open 32-bit programs while using it.", 48, "Warning")
                        If extension.ToString = ".sfz" Then
                            Dim bank As String
                            Dim preset As String
                            bank = InputBox("Enter the number of the bank. (From 0 to 127)" & vbCrLf & itm, "Select a bank")
                            preset = InputBox("Enter the number of the preset. (From 0 to 127)" & vbCrLf & itm, "Select a preset")
                            If CheckForAlphaCharacters(bank) And CheckForAlphaCharacters(preset) Then
                                PortBBox.Items.Add("p0,0=0,0|" + itm)
                            ElseIf bank.Length = 0 And preset.Length = 0 Then
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Empty values for both bank and preset.")
                                PortBBox.Items.Add("p0,0=0,0|" + itm)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,0=0,0|" + itm)
                                ApplyPortB_Click()
                            Else
                                If bank > 128 Then
                                    If preset > 127 Then
                                        PortBBox.Items.Add("p127,127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127,127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortBBox.Items.Add("p127,0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127,0=0,0|" + itm)
                                    Else
                                        PortBBox.Items.Add("p127," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortB_Click()
                                ElseIf bank < 0 Then
                                    If preset > 127 Then
                                        PortBBox.Items.Add("p0,127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortBBox.Items.Add("p0,0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,0=0,0|" + itm)
                                    Else
                                        PortBBox.Items.Add("p0," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortB_Click()
                                Else
                                    If preset > 127 Then
                                        PortBBox.Items.Add("p" + bank + ",127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + ",127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortBBox.Items.Add("p" + bank + ",0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + ",0=0,0|" + itm)
                                    Else
                                        PortBBox.Items.Add("p" + bank + "," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + "," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortB_Click()
                                End If
                            End If
                        ElseIf extension.ToString = ".sf2" Or extension.ToString = ".sfpack" Or extension.ToString = ".SF2" Or extension.ToString = ".SFPACK" Then
                            PortBBox.Items.Add(itm)
                            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                            ApplyPortB_Click()
                        Else
                            If DisableCheckPortB.Checked = True Then
                                PortBBox.Items.Add(itm)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                            Else
                                Console.ForegroundColor = ConsoleColor.Red
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Invalid format: " + extension.ToString)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                                Console.ForegroundColor = ConsoleColor.Green
                            End If
                            ApplyPortB_Click()
                        End If
                    Else
                        If extension.ToString = ".sfz" Then
                            Dim bank As String
                            Dim preset As String
                            bank = InputBox("Enter the number of the bank. (From 0 to 127)" & vbCrLf & itm, "Select a bank")
                            preset = InputBox("Enter the number of the preset. (From 0 to 127)" & vbCrLf & itm, "Select a preset")
                            If CheckForAlphaCharacters(bank) And CheckForAlphaCharacters(preset) Then
                                PortBBox.Items.Add("p0,0=0,0|" + itm)
                                ApplyPortA_Click()
                            ElseIf bank.Length = 0 And preset.Length = 0 Then
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Empty values for both bank and preset.")
                                PortBBox.Items.Add("p0,0=0,0|" + itm)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,0=0,0|" + itm)
                                ApplyPortB_Click()
                            Else
                                If bank > 128 Then
                                    If preset > 127 Then
                                        PortBBox.Items.Add("p127,127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127,127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortBBox.Items.Add("p127,0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127,0=0,0|" + itm)
                                    Else
                                        PortBBox.Items.Add("p127," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortB_Click()
                                ElseIf bank < 0 Then
                                    If preset > 127 Then
                                        PortBBox.Items.Add("p0,127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortBBox.Items.Add("p0,0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,0=0,0|" + itm)
                                    Else
                                        PortBBox.Items.Add("p0," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortB_Click()
                                Else
                                    If preset > 127 Then
                                        PortBBox.Items.Add("p" + bank + ",127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + ",127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortBBox.Items.Add("p" + bank + ",0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + ",0=0,0|" + itm)
                                    Else
                                        PortBBox.Items.Add("p" + bank + "," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + "," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortB_Click()
                                End If
                            End If
                        ElseIf extension.ToString = ".sf2" Or extension.ToString = ".sfpack" Or extension.ToString = ".SF2" Or extension.ToString = ".SFPACK" Then
                            PortBBox.Items.Add(itm)
                            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                            ApplyPortB_Click()
                        Else
                            If DisableCheckPortB.Checked = True Then
                                PortBBox.Items.Add(itm)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port A:")
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                                ApplyPortB_Click()
                            Else
                                Console.ForegroundColor = ConsoleColor.Red
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Invalid format: " + extension.ToString)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                                Console.ForegroundColor = ConsoleColor.Green
                            End If
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub ImportSFPortC_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Try
            Dim strlist As New List(Of String)
            If DisableCheckPortC.Checked = True Then
                PortCOpenDialog1.Filter = "All files (*.*)|*.*"
            Else
                PortCOpenDialog1.Filter = "SoundFont/SFZ Files (*.sf2, *.sfpack, *.sfz)|*.sf1;*.sf2;*.sf2pack;*.sfz"
            End If
            PortCOpenDialog1.FileName = ""
            If PortCOpenDialog1.ShowDialog = DialogResult.OK Then
                For Each itm In PortCOpenDialog1.FileNames
                    Dim infoReader As System.IO.FileInfo
                    infoReader = My.Computer.FileSystem.GetFileInfo(itm)
                    Dim fileName As String = itm
                    Dim fileName2 As String
                    Dim extension As String
                    extension = Path.GetExtension(fileName)
                    fileName2 = Path.GetFileName(fileName)
                    If infoReader.Length > 2147483647 Then
                        Dim Size As Integer = infoReader.Length / 1048576
                        MsgBox(fileName2.ToString + "'s size goes over the 32-bit memory barrier. (" + Size.ToString + "MB, while the limit is 2047MB)" & vbCrLf & vbCrLf & "Do not open 32-bit programs while using it.", 48, "Warning")
                        If extension.ToString = ".sfz" Then
                            Dim bank As String
                            Dim preset As String
                            bank = InputBox("Enter the number of the bank. (From 0 to 127)" & vbCrLf & itm, "Select a bank")
                            preset = InputBox("Enter the number of the preset. (From 0 to 127)" & vbCrLf & itm, "Select a preset")
                            If CheckForAlphaCharacters(bank) And CheckForAlphaCharacters(preset) Then
                                PortCBox.Items.Add("p0,0=0,0|" + itm)
                            ElseIf bank.Length = 0 And preset.Length = 0 Then
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Empty values for both bank and preset.")
                                PortCBox.Items.Add("p0,0=0,0|" + itm)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,0=0,0|" + itm)
                                ApplyPortC_Click()
                            Else
                                If bank > 128 Then
                                    If preset > 127 Then
                                        PortCBox.Items.Add("p127,127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127,127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortCBox.Items.Add("p127,0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127,0=0,0|" + itm)
                                    Else
                                        PortCBox.Items.Add("p127," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortC_Click()
                                ElseIf bank < 0 Then
                                    If preset > 127 Then
                                        PortCBox.Items.Add("p0,127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortCBox.Items.Add("p0,0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,0=0,0|" + itm)
                                    Else
                                        PortCBox.Items.Add("p0," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortC_Click()
                                Else
                                    If preset > 127 Then
                                        PortCBox.Items.Add("p" + bank + ",127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + ",127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortCBox.Items.Add("p" + bank + ",0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + ",0=0,0|" + itm)
                                    Else
                                        PortCBox.Items.Add("p" + bank + "," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + "," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortC_Click()
                                End If
                            End If
                        ElseIf extension.ToString = ".sf2" Or extension.ToString = ".sfpack" Or extension.ToString = ".SF2" Or extension.ToString = ".SFPACK" Then
                            PortCBox.Items.Add(itm)
                            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                            ApplyPortC_Click()
                        Else
                            If DisableCheckPortC.Checked = True Then
                                PortCBox.Items.Add(itm)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                            Else
                                Console.ForegroundColor = ConsoleColor.Red
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Invalid format: " + extension.ToString)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                                Console.ForegroundColor = ConsoleColor.Green
                            End If
                            ApplyPortC_Click()
                        End If
                    Else
                        If extension.ToString = ".sfz" Then
                            Dim bank As String
                            Dim preset As String
                            bank = InputBox("Enter the number of the bank. (From 0 to 127)" & vbCrLf & itm, "Select a bank")
                            preset = InputBox("Enter the number of the preset. (From 0 to 127)" & vbCrLf & itm, "Select a preset")
                            If CheckForAlphaCharacters(bank) And CheckForAlphaCharacters(preset) Then
                                PortCBox.Items.Add("p0,0=0,0|" + itm)
                                ApplyPortA_Click()
                            ElseIf bank.Length = 0 And preset.Length = 0 Then
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Empty values for both bank and preset.")
                                PortCBox.Items.Add("p0,0=0,0|" + itm)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,0=0,0|" + itm)
                                ApplyPortC_Click()
                            Else
                                If bank > 128 Then
                                    If preset > 127 Then
                                        PortCBox.Items.Add("p127,127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127,127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortCBox.Items.Add("p127,0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127,0=0,0|" + itm)
                                    Else
                                        PortCBox.Items.Add("p127," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortC_Click()
                                ElseIf bank < 0 Then
                                    If preset > 127 Then
                                        PortCBox.Items.Add("p0,127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortCBox.Items.Add("p0,0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,0=0,0|" + itm)
                                    Else
                                        PortCBox.Items.Add("p0," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortC_Click()
                                Else
                                    If preset > 127 Then
                                        PortCBox.Items.Add("p" + bank + ",127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + ",127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortCBox.Items.Add("p" + bank + ",0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + ",0=0,0|" + itm)
                                    Else
                                        PortCBox.Items.Add("p" + bank + "," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + "," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortC_Click()
                                End If
                            End If
                        ElseIf extension.ToString = ".sf2" Or extension.ToString = ".sfpack" Or extension.ToString = ".SF2" Or extension.ToString = ".SFPACK" Then
                            PortCBox.Items.Add(itm)
                            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                            ApplyPortC_Click()
                        Else
                            If DisableCheckPortC.Checked = True Then
                                PortCBox.Items.Add(itm)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to port C:")
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                                ApplyPortC_Click()
                            Else
                                Console.ForegroundColor = ConsoleColor.Red
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Invalid format: " + extension.ToString)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                                Console.ForegroundColor = ConsoleColor.Green
                            End If
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub ImportSFPortD_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Try
            Dim strlist As New List(Of String)
            If DisableCheckPortD.Checked = True Then
                PortDOpenDialog1.Filter = "All files (*.*)|*.*"
            Else
                PortDOpenDialog1.Filter = "SoundFont/SFZ Files (*.sf2, *.sfpack, *.sfz)|*.sf1;*.sf2;*.sf2pack;*.sfz"
            End If
            PortDOpenDialog1.FileName = ""
            If PortDOpenDialog1.ShowDialog = DialogResult.OK Then
                For Each itm In PortDOpenDialog1.FileNames
                    Dim infoReader As System.IO.FileInfo
                    infoReader = My.Computer.FileSystem.GetFileInfo(itm)
                    Dim fileName As String = itm
                    Dim fileName2 As String
                    Dim extension As String
                    extension = Path.GetExtension(fileName)
                    fileName2 = Path.GetFileName(fileName)
                    If infoReader.Length > 2147483647 Then
                        Dim Size As Integer = infoReader.Length / 1048576
                        MsgBox(fileName2.ToString + "'s size goes over the 32-bit memory barrier. (" + Size.ToString + "MB, while the limit is 2047MB)" & vbCrLf & vbCrLf & "Do not open 32-bit programs while using it.", 48, "Warning")
                        If extension.ToString = ".sfz" Then
                            Dim bank As String
                            Dim preset As String
                            bank = InputBox("Enter the number of the bank. (From 0 to 127)" & vbCrLf & itm, "Select a bank")
                            preset = InputBox("Enter the number of the preset. (From 0 to 127)" & vbCrLf & itm, "Select a preset")
                            If CheckForAlphaCharacters(bank) And CheckForAlphaCharacters(preset) Then
                                PortDBox.Items.Add("p0,0=0,0|" + itm)
                            ElseIf bank.Length = 0 And preset.Length = 0 Then
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Empty values for both bank and preset.")
                                PortDBox.Items.Add("p0,0=0,0|" + itm)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,0=0,0|" + itm)
                                ApplyPortD_Click()
                            Else
                                If bank > 128 Then
                                    If preset > 127 Then
                                        PortDBox.Items.Add("p127,127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127,127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortDBox.Items.Add("p127,0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127,0=0,0|" + itm)
                                    Else
                                        PortDBox.Items.Add("p127," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortD_Click()
                                ElseIf bank < 0 Then
                                    If preset > 127 Then
                                        PortDBox.Items.Add("p0,127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortDBox.Items.Add("p0,0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,0=0,0|" + itm)
                                    Else
                                        PortDBox.Items.Add("p0," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortD_Click()
                                Else
                                    If preset > 127 Then
                                        PortDBox.Items.Add("p" + bank + ",127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + ",127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortDBox.Items.Add("p" + bank + ",0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + ",0=0,0|" + itm)
                                    Else
                                        PortDBox.Items.Add("p" + bank + "," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + "," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortD_Click()
                                End If
                            End If
                        ElseIf extension.ToString = ".sf2" Or extension.ToString = ".sfpack" Or extension.ToString = ".SF2" Or extension.ToString = ".SFPACK" Then
                            PortDBox.Items.Add(itm)
                            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                            ApplyPortD_Click()
                        Else
                            If DisableCheckPortD.Checked = True Then
                                PortDBox.Items.Add(itm)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                            Else
                                Console.ForegroundColor = ConsoleColor.Red
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Invalid format: " + extension.ToString)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                                Console.ForegroundColor = ConsoleColor.Green
                            End If
                            ApplyPortD_Click()
                        End If
                    Else
                        If extension.ToString = ".sfz" Then
                            Dim bank As String
                            Dim preset As String
                            bank = InputBox("Enter the number of the bank. (From 0 to 127)" & vbCrLf & itm, "Select a bank")
                            preset = InputBox("Enter the number of the preset. (From 0 to 127)" & vbCrLf & itm, "Select a preset")
                            If CheckForAlphaCharacters(bank) And CheckForAlphaCharacters(preset) Then
                                PortDBox.Items.Add("p0,0=0,0|" + itm)
                                ApplyPortA_Click()
                            ElseIf bank.Length = 0 And preset.Length = 0 Then
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Empty values for both bank and preset.")
                                PortDBox.Items.Add("p0,0=0,0|" + itm)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,0=0,0|" + itm)
                                ApplyPortD_Click()
                            Else
                                If bank > 128 Then
                                    If preset > 127 Then
                                        PortDBox.Items.Add("p127,127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127,127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortDBox.Items.Add("p127,0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127,0=0,0|" + itm)
                                    Else
                                        PortDBox.Items.Add("p127," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p127," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortD_Click()
                                ElseIf bank < 0 Then
                                    If preset > 127 Then
                                        PortDBox.Items.Add("p0,127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortDBox.Items.Add("p0,0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0,0=0,0|" + itm)
                                    Else
                                        PortDBox.Items.Add("p0," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p0," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortD_Click()
                                Else
                                    If preset > 127 Then
                                        PortDBox.Items.Add("p" + bank + ",127=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + ",127=0,0|" + itm)
                                    ElseIf preset < 0 Then
                                        PortDBox.Items.Add("p" + bank + ",0=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + ",0=0,0|" + itm)
                                    Else
                                        PortDBox.Items.Add("p" + bank + "," + preset + "=0,0|" + itm)
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "p" + bank + "," + preset + "=0,0|" + itm)
                                    End If
                                    ApplyPortD_Click()
                                End If
                            End If
                        ElseIf extension.ToString = ".sf2" Or extension.ToString = ".sfpack" Or extension.ToString = ".SF2" Or extension.ToString = ".SFPACK" Then
                            PortDBox.Items.Add(itm)
                            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                            ApplyPortD_Click()
                        Else
                            If DisableCheckPortD.Checked = True Then
                                PortDBox.Items.Add(itm)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added soundfont to Port D:")
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                                ApplyPortD_Click()
                            Else
                                Console.ForegroundColor = ConsoleColor.Red
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Invalid format: " + extension.ToString)
                                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + itm)
                                Console.ForegroundColor = ConsoleColor.Green
                            End If
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub MoveUpPortA_Click(sender As Object, e As EventArgs) Handles MoveUpPortA.Click
        Try
            If PortABox.SelectedIndex > 0 Then
                Dim I = PortABox.SelectedIndex - 1
                PortABox.Items.Insert(I, PortABox.SelectedItem)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Moved soundfont up in port A:")
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + PortABox.SelectedItem.ToString)
                PortABox.Items.RemoveAt(PortABox.SelectedIndex)
                PortABox.SelectedIndex = I
            End If
            ApplyPortA_Click()
        Catch ex As Exception
            Dim causeOfFailure As String = "Unhandled exception."
            Try
                Environment.FailFast(causeOfFailure)
            Finally
                Console.WriteLine("")
            End Try
        End Try
    End Sub

    Private Sub MoveDownPortA_Click(sender As Object, e As EventArgs) Handles MoveDownPortA.Click
        Try
            If PortABox.SelectedIndex < PortABox.Items.Count - 1 Then
                Dim I = PortABox.SelectedIndex + 2
                PortABox.Items.Insert(I, PortABox.SelectedItem)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Moved soundfont down in port A:")
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + PortABox.SelectedItem.ToString)
                PortABox.Items.RemoveAt(PortABox.SelectedIndex)
                PortABox.SelectedIndex = I - 1
            End If
            ApplyPortA_Click()
        Catch ex As Exception
            Dim causeOfFailure As String = "Unhandled exception."
            Try
                Environment.FailFast(causeOfFailure)
            Finally
                Console.WriteLine("")
            End Try
        End Try
    End Sub

    Private Sub MoveUpPortB_Click(sender As Object, e As EventArgs) Handles MoveUpPortB.Click
        Try
            If PortBBox.SelectedIndex > 0 Then
                Dim I = PortBBox.SelectedIndex - 1
                PortBBox.Items.Insert(I, PortBBox.SelectedItem)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Moved soundfont up in port B:")
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + PortBBox.SelectedItem.ToString)
                PortBBox.Items.RemoveAt(PortBBox.SelectedIndex)
                PortBBox.SelectedIndex = I
            End If
            ApplyPortB_Click()
        Catch ex As Exception
            Dim causeOfFailure As String = "Unhandled exception."
            Try
                Environment.FailFast(causeOfFailure)
            Finally
                Console.WriteLine("")
            End Try
        End Try
    End Sub

    Private Sub MoveDownPortB_Click(sender As Object, e As EventArgs) Handles MoveDownPortB.Click
        Try
            If PortBBox.SelectedIndex < PortBBox.Items.Count - 1 Then
                Dim I = PortBBox.SelectedIndex + 2
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Moved soundfont down in port B:")
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + PortBBox.SelectedItem.ToString)
                PortBBox.Items.Insert(I, PortBBox.SelectedItem)
                PortBBox.Items.RemoveAt(PortBBox.SelectedIndex)
                PortBBox.SelectedIndex = I - 1
            End If
            ApplyPortB_Click()
        Catch ex As Exception
            Dim causeOfFailure As String = "Unhandled exception."
            Try
                Environment.FailFast(causeOfFailure)
            Finally
                Console.WriteLine("")
            End Try
        End Try
    End Sub

    Private Sub MoveUpPortC_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try
            If PortCBox.SelectedIndex > 0 Then
                Dim I = PortCBox.SelectedIndex - 1
                PortCBox.Items.Insert(I, PortCBox.SelectedItem)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Moved soundfont up in port C:")
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + PortCBox.SelectedItem.ToString)
                PortCBox.Items.RemoveAt(PortCBox.SelectedIndex)
                PortCBox.SelectedIndex = I
            End If
            ApplyPortC_Click()
        Catch ex As Exception
            Dim causeOfFailure As String = "Unhandled exception."
            Try
                Environment.FailFast(causeOfFailure)
            Finally
                Console.WriteLine("")
            End Try
        End Try
    End Sub

    Private Sub MoveDownPortC_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            If PortCBox.SelectedIndex < PortCBox.Items.Count - 1 Then
                Dim I = PortCBox.SelectedIndex + 2
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Moved soundfont down in port C:")
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + PortCBox.SelectedItem.ToString)
                PortCBox.Items.Insert(I, PortCBox.SelectedItem)
                PortCBox.Items.RemoveAt(PortCBox.SelectedIndex)
                PortCBox.SelectedIndex = I - 1
            End If
            ApplyPortC_Click()
        Catch ex As Exception
            Dim causeOfFailure As String = "Unhandled exception."
            Try
                Environment.FailFast(causeOfFailure)
            Finally
                Console.WriteLine("")
            End Try
        End Try
    End Sub

    Private Sub MoveUpPortD_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Try
            If PortDBox.SelectedIndex > 0 Then
                Dim I = PortDBox.SelectedIndex - 1
                PortDBox.Items.Insert(I, PortDBox.SelectedItem)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Moved soundfont up in port D:")
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + PortDBox.SelectedItem.ToString)
                PortDBox.Items.RemoveAt(PortDBox.SelectedIndex)
                PortDBox.SelectedIndex = I
            End If
            ApplyPortD_Click()
        Catch ex As Exception
            Dim causeOfFailure As String = "Unhandled exception."
            Try
                Environment.FailFast(causeOfFailure)
            Finally
                Console.WriteLine("")
            End Try
        End Try
    End Sub

    Private Sub MoveDownPortD_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Try
            If PortDBox.SelectedIndex < PortDBox.Items.Count - 1 Then
                Dim I = PortDBox.SelectedIndex + 2
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Moved soundfont down in port D:")
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + PortDBox.SelectedItem.ToString)
                PortDBox.Items.Insert(I, PortDBox.SelectedItem)
                PortDBox.Items.RemoveAt(PortDBox.SelectedIndex)
                PortDBox.SelectedIndex = I - 1
            End If
            ApplyPortD_Click()
        Catch ex As Exception
            Dim causeOfFailure As String = "Unhandled exception."
            Try
                Environment.FailFast(causeOfFailure)
            Finally
                Console.WriteLine("")
            End Try
        End Try
    End Sub

    Private Sub ClearPortA_Click(sender As Object, e As EventArgs) Handles ClearPortA.Click
        PortABox.Items.Clear()
        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Port A's listbox cleared.")
        MsgBox("List cleared!", 64, "Success")
        ApplyPortA_Click()
    End Sub

    Private Sub ClearPortB_Click(sender As Object, e As EventArgs) Handles ClearPortB.Click
        PortBBox.Items.Clear()
        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Port B's listbox cleared.")
        MsgBox("List cleared!", 64, "Success")
        ApplyPortB_Click()
    End Sub

    Private Sub ClearPortC_Click(sender As Object, e As EventArgs) Handles Button2.Click
        PortCBox.Items.Clear()
        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Port C's listbox cleared.")
        MsgBox("List cleared!", 64, "Success")
        ApplyPortC_Click()
    End Sub

    Private Sub ClearPortD_Click(sender As Object, e As EventArgs) Handles Button9.Click
        PortDBox.Items.Clear()
        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Port D's listbox cleared.")
        MsgBox("List cleared!", 64, "Success")
        ApplyPortD_Click()
    End Sub

    Private Sub RemoveSFPortA_Click(sender As Object, e As EventArgs) Handles RemoveSFPortA.Click
        Try
            Dim lst As New List(Of Object)
            For Each a As Object In PortABox.SelectedItems
                lst.Add(a)
            Next
            For Each a As Object In lst
                PortABox.Items.Remove(a)
            Next
            PortABox.SelectedIndex = 0
            ApplyPortA_Click()
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Select a soundfont to remove first!")
            Console.ForegroundColor = ConsoleColor.Green
        End Try
    End Sub

    Private Sub RemoveSFPortB_Click(sender As Object, e As EventArgs) Handles RemoveSFPortB.Click
        Try
            Dim lst As New List(Of Object)
            For Each a As Object In PortBBox.SelectedItems
                lst.Add(a)
            Next
            For Each a As Object In lst
                PortBBox.Items.Remove(a)
            Next
            PortBBox.SelectedIndex = 0
            ApplyPortB_Click()
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Select a soundfont to remove first!")
            Console.ForegroundColor = ConsoleColor.Green
        End Try
    End Sub

    Private Sub RemoveSFPortC_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Try
            Dim lst As New List(Of Object)
            For Each a As Object In PortCBox.SelectedItems
                lst.Add(a)
            Next
            For Each a As Object In lst
                PortCBox.Items.Remove(a)
            Next
            PortCBox.SelectedIndex = 0
            ApplyPortC_Click()
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Select a soundfont to remove first!")
            Console.ForegroundColor = ConsoleColor.Green
        End Try
    End Sub

    Private Sub RemoveSFPortD_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Try
            Dim lst As New List(Of Object)
            For Each a As Object In PortDBox.SelectedItems
                lst.Add(a)
            Next
            For Each a As Object In lst
                PortDBox.Items.Remove(a)
            Next
            PortDBox.SelectedIndex = 0
            ApplyPortD_Click()
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Select a soundfont to remove first!")
            Console.ForegroundColor = ConsoleColor.Green
        End Try
    End Sub

    ' Soundfont list part is over!

    Private Sub AdvancedApply_Click(sender As Object, e As EventArgs) Handles AdvancedApply.Click
        Try
            Dim UserString As String
            UserString = "Software\Keppy's Driver"
            Dim keppykey = My.Computer.Registry.CurrentUser.OpenSubKey(UserString + "\Settings", True)
            Dim keppykeyfx = My.Computer.Registry.CurrentUser.OpenSubKey(UserString + "\Effects", True)
            If NoteOff.Checked Then
                keppykey.SetValue("noteoff", "1", RegistryValueKind.DWord)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Notes will be (maybe) deleted after a noteoff event. (Depends on the MIDI)")
            Else
                keppykey.SetValue("noteoff", "0", RegistryValueKind.DWord)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Notes will be deleted after a noteoff event.")
            End If
            If FloatingDisabled.Checked Then
                keppykey.SetValue("nofloat", "0", RegistryValueKind.DWord)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Floating point audio rendering disabled.")
            Else
                keppykey.SetValue("nofloat", "1", RegistryValueKind.DWord)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Floating point audio rendering enabled.")
            End If
            If SoftwareRendering.Checked Then
                keppykey.SetValue("softwaremode", "1", RegistryValueKind.DWord)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "BASS software render enabled.")
            Else
                keppykey.SetValue("softwaremode", "0", RegistryValueKind.DWord)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "BASS software render disabled.")
            End If
            If SysResetIgnore.Checked Then
                keppykey.SetValue("sysresetignore", "1", RegistryValueKind.DWord)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "System MIDI reset commands are not going to be ignored.")
            Else
                keppykey.SetValue("sysresetignore", "0", RegistryValueKind.DWord)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "System MIDI reset commands are going to be ignored.")
            End If
            If Preload.Checked Then
                keppykey.SetValue("preload", "1", RegistryValueKind.DWord)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Soundfont preloading enabled.")
            Else
                keppykey.SetValue("preload", "0", RegistryValueKind.DWord)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Soundfont preloading disabled.")
            End If
            If DisableFX.Checked Then
                keppykey.SetValue("nofx", "1", RegistryValueKind.DWord)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Audio effects enabled.")
            Else
                keppykey.SetValue("nofx", "0", RegistryValueKind.DWord)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Audio effects disabled.")
            End If
            If NoDX8FX.Checked Then
                keppykey.SetValue("nodx8", "1", RegistryValueKind.DWord)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "DirectX8 effects enabled.")
            Else
                keppykey.SetValue("nodx8", "0", RegistryValueKind.DWord)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "DirectX8 effects disabled.")
            End If
            If SincInter.Checked Then
                keppykey.SetValue("sinc", "1", RegistryValueKind.DWord)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Sinc enabled.")
            Else
                keppykey.SetValue("sinc", "0", RegistryValueKind.DWord)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Sinc disabled.")
            End If
            If ReverbFX.Checked = True Then
                keppykeyfx.SetValue("reverbfx", "1", RegistryValueKind.DWord)
            Else
                keppykeyfx.SetValue("reverbfx", "0", RegistryValueKind.DWord)
            End If
            If ChorusFX.Checked = True Then
                keppykeyfx.SetValue("chorusfx", "1", RegistryValueKind.DWord)
            Else
                keppykeyfx.SetValue("chorusfx", "0", RegistryValueKind.DWord)
            End If
            If FlangerFX.Checked = True Then
                keppykeyfx.SetValue("flangerfx", "1", RegistryValueKind.DWord)
            Else
                keppykeyfx.SetValue("flangerfx", "0", RegistryValueKind.DWord)
            End If
            If CompressorFX.Checked = True Then
                keppykeyfx.SetValue("compressorfx", "1", RegistryValueKind.DWord)
            Else
                keppykeyfx.SetValue("compressorfx", "0", RegistryValueKind.DWord)
            End If
            If GargleFX.Checked = True Then
                keppykeyfx.SetValue("garglefx", "1", RegistryValueKind.DWord)
            Else
                keppykeyfx.SetValue("garglefx", "0", RegistryValueKind.DWord)
            End If
            If DistortionFX.Checked = True Then
                keppykeyfx.SetValue("distortionfx", "1", RegistryValueKind.DWord)
            Else
                keppykeyfx.SetValue("distortionfx", "0", RegistryValueKind.DWord)
            End If
            If EchoFX.Checked = True Then
                keppykeyfx.SetValue("echofx", "1", RegistryValueKind.DWord)
            Else
                keppykeyfx.SetValue("echofx", "0", RegistryValueKind.DWord)
            End If
            If SittingFX.Checked = True Then
                keppykeyfx.SetValue("sittingfx", "1", RegistryValueKind.DWord)
            Else
                keppykeyfx.SetValue("sittingfx", "0", RegistryValueKind.DWord)
            End If
            If MaxCPU.Text = "Disabled" Then
                keppykey.SetValue("cpu", "0", RegistryValueKind.DWord)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Max rendering time percentage: Disabled")
            Else
                keppykey.SetValue("cpu", MaxCPU.Text, RegistryValueKind.DWord)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Max rendering time percentage: " + MaxCPU.Text + "%")
            End If
            keppykey.SetValue("reverbfxnum", ReverbFXNum.Value, RegistryValueKind.DWord)
            keppykey.SetValue("chorusfxnum", ChorusFXNum.Value, RegistryValueKind.DWord)
            keppykey.SetValue("flangerfxnum", FlangerFXNum.Value, RegistryValueKind.DWord)
            keppykey.SetValue("compressorfxnum", CompressorFXNum.Value, RegistryValueKind.DWord)
            keppykey.SetValue("garglefxnum", GargleFXNum.Value, RegistryValueKind.DWord)
            keppykey.SetValue("distortionfxnum", DistortionFXNum.Value, RegistryValueKind.DWord)
            keppykey.SetValue("echofxnum", EchoFXNum.Value, RegistryValueKind.DWord)
            keppykey.SetValue("sittingfxnum", SittingFXNum.Value, RegistryValueKind.DWord)
            keppykey.SetValue("polyphony", PolyphonyLimit.Value.ToString, RegistryValueKind.DWord)
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Voice limit set to " + PolyphonyLimit.Value.ToString + ".")
            keppykey.SetValue("buflen", bufsize.Value.ToString, RegistryValueKind.DWord)
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "New buffer length: " + bufsize.Value.ToString + "ms.")
            keppykey.SetValue("frequency", Frequency.Text, RegistryValueKind.DWord)
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Frequency set to " + Frequency.Text + "Hz.")
            keppykey.SetValue("tracks", TracksLimit.Value.ToString, RegistryValueKind.DWord)
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "MIDI tracks limited to " + TracksLimit.Value.ToString + ".")
            keppykey.SetValue("volume", VolumeBar.Value.ToString, RegistryValueKind.DWord)
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Volume set to " + VolumeBar.Value.ToString + ".")
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Settings saved.")
            MsgBox("Settings saved!", 64, "Success")
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Error while writing to the registry.")
            MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Error")
            Console.ForegroundColor = ConsoleColor.Green
        End Try
    End Sub

    Private Sub BlackMIDIPreset_Click(sender As Object, e As EventArgs) Handles BlackMIDIPreset.Click
        Try
            Dim UserString As String
            UserString = "Software\Keppy's Driver"
            Dim keppykey = My.Computer.Registry.CurrentUser.OpenSubKey(UserString + "\Settings", True)
            bufsize.Value = 15
            PolyphonyLimit.Value = 1000
            TracksLimit.Value = 16
            MaxCPU.Text = "75"
            Frequency.Text = "44100"
            Preload.Checked = True
            NoDX8FX.Checked = True
            SincInter.Checked = False
            DisableFX.Checked = False
            SysResetIgnore.Checked = True
            SoftwareRendering.Checked = False
            FloatingDisabled.Checked = False
            NoteOff.Checked = True
            FloatingDisabled.Checked = False
            SoftwareRendering.Checked = True
            keppykey.SetValue("maxports", "4", RegistryValueKind.DWord)
            keppykey.SetValue("dsorxaudio", "0", RegistryValueKind.DWord)
            keppykey.SetValue("noteoff", "1", RegistryValueKind.DWord)
            keppykey.SetValue("polyphony", "1000", RegistryValueKind.DWord)
            keppykey.SetValue("cpu", "75", RegistryValueKind.DWord)
            keppykey.SetValue("nofloat", "1", RegistryValueKind.DWord)
            keppykey.SetValue("softwaremode", "1", RegistryValueKind.DWord)
            keppykey.SetValue("nofx", "0", RegistryValueKind.DWord)
            keppykey.SetValue("nodx8", 1, RegistryValueKind.DWord)
            keppykey.SetValue("sysresetignore", "1", RegistryValueKind.DWord)
            keppykey.SetValue("preload", "1", RegistryValueKind.DWord)
            keppykey.SetValue("buflen", "12", RegistryValueKind.DWord)
            keppykey.SetValue("frequency", "44100", RegistryValueKind.DWord)
            keppykey.SetValue("tracks", "16", RegistryValueKind.DWord)
            keppykey.SetValue("sinc", "0", RegistryValueKind.DWord)
            keppykey.SetValue("mono", "0", RegistryValueKind.DWord)
            Dim VolumeValue As Integer
            Dim x As Double = VolumeBar.Value.ToString / 100
            VolumeValue = Convert.ToInt32(x)
            CurrentVolumeHUE.Text = VolumeValue.ToString
            MsgBox("Black MIDI preset applied!", 64, "Success")
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Error while writing to the registry.")
            MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Error")
            Console.ForegroundColor = ConsoleColor.Green
        End Try
    End Sub

    Private Sub AdvancedReset_Click(sender As Object, e As EventArgs) Handles AdvancedReset.Click
        Try
            Dim UserString As String
            UserString = "Software\Keppy's Driver"
            Dim keppykey = My.Computer.Registry.CurrentUser.OpenSubKey(UserString + "\Settings", True)
            Dim keppykeyfx = My.Computer.Registry.CurrentUser.OpenSubKey(UserString + "\Effects", True)
            bufsize.Value = 30
            VolumeBar.Value = 10000
            PolyphonyLimit.Value = 512
            TracksLimit.Value = 16
            MaxCPU.Text = "65"
            Frequency.Text = "44100"
            Preload.Checked = True
            SincInter.Checked = False
            DisableFX.Checked = False
            NoDX8FX.Checked = True
            SysResetIgnore.Checked = False
            SoftwareRendering.Checked = False
            FloatingDisabled.Checked = False
            NoteOff.Checked = False
            FloatingDisabled.Checked = False
            SoftwareRendering.Checked = True
            ReverbFX.Checked = False
            ChorusFX.Checked = False
            SittingFX.Checked = False
            FlangerFX.Checked = False
            EchoFX.Checked = False
            GargleFX.Checked = False
            CompressorFX.Checked = False
            DistortionFX.Checked = False
            keppykey.SetValue("dsorxaudio", "0", RegistryValueKind.DWord)
            keppykey.SetValue("noteoff", "0", RegistryValueKind.DWord)
            keppykey.SetValue("polyphony", "512", RegistryValueKind.DWord)
            keppykey.SetValue("cpu", "65", RegistryValueKind.DWord)
            keppykey.SetValue("nofloat", "1", RegistryValueKind.DWord)
            keppykey.SetValue("softwaremode", "1", RegistryValueKind.DWord)
            keppykey.SetValue("nofx", "0", RegistryValueKind.DWord)
            keppykey.SetValue("nodx8", "1", RegistryValueKind.DWord)
            keppykey.SetValue("sysresetignore", "0", RegistryValueKind.DWord)
            keppykey.SetValue("preload", "1", RegistryValueKind.DWord)
            keppykey.SetValue("buflen", "30", RegistryValueKind.DWord)
            keppykey.SetValue("frequency", "44100", RegistryValueKind.DWord)
            keppykey.SetValue("tracks", "16", RegistryValueKind.DWord)
            keppykey.SetValue("sinc", "0", RegistryValueKind.DWord)
            keppykey.SetValue("volume", "10000", RegistryValueKind.DWord)
            keppykeyfx.SetValue("reverbfx", "0", RegistryValueKind.DWord)
            keppykeyfx.SetValue("chorusfx", "0", RegistryValueKind.DWord)
            keppykeyfx.SetValue("flangerfx", "0", RegistryValueKind.DWord)
            keppykeyfx.SetValue("compressorfx", "0", RegistryValueKind.DWord)
            keppykeyfx.SetValue("garglefx", "0", RegistryValueKind.DWord)
            keppykeyfx.SetValue("distortionfx", "0", RegistryValueKind.DWord)
            keppykeyfx.SetValue("echofx", "0", RegistryValueKind.DWord)
            keppykeyfx.SetValue("sittingfx", "0", RegistryValueKind.DWord)
            keppykeyfx.SetValue("reverbfxnum", "1", RegistryValueKind.DWord)
            keppykeyfx.SetValue("chorusfxnum", "2", RegistryValueKind.DWord)
            keppykeyfx.SetValue("flangerfxnum", "3", RegistryValueKind.DWord)
            keppykeyfx.SetValue("compressorfxnum", "4", RegistryValueKind.DWord)
            keppykeyfx.SetValue("garglefxnum", "5", RegistryValueKind.DWord)
            keppykeyfx.SetValue("distortionfxnum", "6", RegistryValueKind.DWord)
            keppykeyfx.SetValue("echofxnum", "7", RegistryValueKind.DWord)
            keppykeyfx.SetValue("sittingfxnum", "8", RegistryValueKind.DWord)
            Dim VolumeValue As Integer
            Dim x As Double = VolumeBar.Value.ToString / 100
            VolumeValue = Convert.ToInt32(x)
            CurrentVolumeHUE.Text = VolumeValue.ToString
            MsgBox("Settings restored!", 64, "Success")
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Error while writing to the registry.")
            MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Error")
            Console.ForegroundColor = ConsoleColor.Green
        End Try
    End Sub

    Private Sub UpdateDownload_Click() Handles UpdateDownload.Click
        Try
            Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create("https://raw.githubusercontent.com/KaleidonKep99/Keppy-s-MIDI-Driver/master/output/keppydriverupdate.txt")
            Dim response As System.Net.HttpWebResponse = request.GetResponse()
            Dim sr As System.IO.StreamReader = New System.IO.StreamReader(response.GetResponseStream())
            Dim newestversion As String = sr.ReadToEnd()
            Dim Driver As FileVersionInfo = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\keppydrv\keppydrv.dll")
            ThisVersionDriver.Text = "The current version of the driver, installed on your system, is: " + Driver.FileVersion.ToString
            LatestVersionDriver.Text = "The latest version online, in the GitHub repository, is: " + newestversion.ToString
            If newestversion > Driver.FileVersion Then
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Update found!")
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "New version: " + newestversion)
                MsgBox("New update found, press OK to open the release page.", 48, "New update found!")
                Process.Start("http://goo.gl/BHgazb")
            Else
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "No updates found.")
                MsgBox("This release is already updated.", 64, "No updates found.")
            End If
        Catch ex As Exception
            Dim Driver As FileVersionInfo = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\keppydrv\keppydrv.dll")
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Can not check for updates!")
            Console.ForegroundColor = ConsoleColor.Green
            ThisVersionDriver.Text = "The current version of the driver, installed on your system, is: " + Driver.FileVersion.ToString
            LatestVersionDriver.Text = "Can not check for updates. You're offline, or maybe the website is temporarily down."
            MsgBox("Can not check for updates!" & vbCrLf & vbCrLf & "Specific .NET error:" & vbCrLf & ex.Message.ToString, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub CheckForUpdatesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CheckForUpdatesToolStripMenuItem.Click
        Tabs1.SelectedTab = TabPage4
        Panel2.AutoScrollPosition = New Point(0, 99999)
        UpdateDownload_Click()
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles VolumeBar.Scroll
        Try
            Dim UserString As String
            UserString = "Software\Keppy's Driver"
            Dim keppykey = My.Computer.Registry.CurrentUser.OpenSubKey(UserString + "\Settings", True)
            Dim VolumeValue As Integer
            Dim x As Double = VolumeBar.Value.ToString / 100
            VolumeValue = Convert.ToInt32(x)
            CurrentVolumeHUE.Text = VolumeValue.ToString
            CurrentVolumeHUE2.Text = "Int: " & VolumeBar.Value.ToString
            keppykey.SetValue("volume", VolumeBar.Value.ToString, RegistryValueKind.DWord)
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Changed volume to: " + VolumeBar.Value.ToString)
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Unable to set the volume!")
            MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Error")
            Console.ForegroundColor = ConsoleColor.Green
        End Try
    End Sub

    Private Sub RealTimeDebug_Tick(sender As Object, e As EventArgs)
        Try
            Dim UserString As String
            UserString = "Software\Keppy's Driver"
            Dim TotalRAMValue As ULong = My.Computer.Info.TotalPhysicalMemory / 1048576
            Dim AvailableRAMValue As ULong = My.Computer.Info.AvailablePhysicalMemory / 1048576
            Dim keppykey = My.Computer.Registry.CurrentUser.OpenSubKey(UserString + "\Settings", True)
            Dim keppykeyuser = My.Computer.Registry.CurrentUser.OpenSubKey(UserString, True)
            Dim ActiveVoicesText As Integer = keppykeyuser.GetValue("currentvoices")
            Dim MaxVoicesText As Integer = keppykey.GetValue("polyphony")
            Dim CurrentCPUUsageText As Integer = keppykeyuser.GetValue("currentcpuusage")
            Dim MaxCPUUsageText As Integer = keppykey.GetValue("cpu")
            Dim CurrentTempoText As Integer = keppykeyuser.GetValue("tempo")
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Error while debugging the driver!")
            MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Error")
            Console.ForegroundColor = ConsoleColor.Green
        End Try
    End Sub

    Private Sub AddBlackList_Click(sender As Object, e As EventArgs) Handles AddBlackList.Click
        If BlackListAdvancedMode.Checked = True Then
            UserProgramsBlackList.Items.Add(ManualBlackList.Text)
            Try
                Dim BlackListText As String = Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\blacklist\keppymididrv.blacklist"
                Dim Filenum As Integer = FreeFile()

                FileOpen(Filenum, BlackListText, OpenMode.Output)
                FileClose()

                Using SW As New IO.StreamWriter(BlackListText, True)
                    For Each itm As String In Me.UserProgramsBlackList.Items
                        SW.WriteLine(itm)
                    Next
                End Using
                UserProgramsBlackList.TopIndex = UserProgramsBlackList.Items.Count - 1
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added program to the blacklist (Manual mode): " + ManualBlackList.Text)
            Catch ex As Exception
                Console.ForegroundColor = ConsoleColor.Red
                Dim BlackListText As String = Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\blacklist\keppymididrv.blacklist"
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Error while saving blacklist file: " + BlackListText)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Is the file accessible by the user/locked by SYSTEM?")
                MsgBox("There was an error while saving the file!" & vbCrLf & vbCrLf & "You do not have sufficient privilege to save the file in the current location:" & vbCrLf & BlackListText, 16, "Fatal error")
                Console.ForegroundColor = ConsoleColor.Green
            End Try
        Else
            Try
                Dim strlist As New List(Of String)
                Dim file As String
                BlackListFileDialog.Filter = "Executables|*.exe|All files|*.*"
                If BlackListFileDialog.ShowDialog = DialogResult.OK Then
                    For Each file In BlackListFileDialog.FileNames
                        Dim FileNameOnly As String
                        FileNameOnly = System.IO.Path.GetFileName(file)
                        Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added program to the blacklist: " + FileNameOnly)
                        strlist.Add(FileNameOnly)
                        UserProgramsBlackList.Items.Add(FileNameOnly)
                    Next
                End If
                Dim BlackListText As String = Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\blacklist\keppymididrv.blacklist"
                Dim Filenum As Integer = FreeFile()

                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Saving blacklist to: " + BlackListText)

                FileOpen(Filenum, BlackListText, OpenMode.Output)
                FileClose()

                Using SW As New IO.StreamWriter(BlackListText, True)
                    For Each itm As String In Me.UserProgramsBlackList.Items
                        SW.WriteLine(itm)
                    Next
                End Using
                UserProgramsBlackList.TopIndex = UserProgramsBlackList.Items.Count - 1
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Blacklist saved.")
            Catch ex As Exception
                Console.ForegroundColor = ConsoleColor.Red
                Dim BlackListText As String = Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\blacklist\keppymididrv.blacklist"
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Error while saving blacklist file: " + BlackListText)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Is the file accessible by the user/locked by SYSTEM?")
                MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Error")
                Console.ForegroundColor = ConsoleColor.Green
            End Try
        End If
    End Sub

    Private Sub RemoveBlackList_Click(sender As Object, e As EventArgs) Handles RemoveBlackList.Click
        Dim lst As New List(Of Object)
        For Each a As Object In UserProgramsBlackList.SelectedItems
            lst.Add(a)
        Next
        For Each a As Object In lst
            UserProgramsBlackList.Items.Remove(a)
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Removed program from the blacklist: " + a.ToString)
        Next
        Try
            Dim BlackListText As String = Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\blacklist\keppymididrv.blacklist"
            Dim Filenum As Integer = FreeFile()

            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Saving blacklist to file: " + BlackListText)

            FileOpen(Filenum, BlackListText, OpenMode.Output)
            FileClose()

            Using SW As New IO.StreamWriter(BlackListText, True)
                For Each itm As String In Me.UserProgramsBlackList.Items
                    SW.WriteLine(itm)
                Next
            End Using
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Dim BlackListText As String = Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\blacklist\keppymididrv.blacklist"
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Error while saving blacklist file: " + BlackListText)
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Is the file accessible by the user/locked by SYSTEM?")
            MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Error")
            Console.ForegroundColor = ConsoleColor.Green
        End Try
    End Sub

    Private Sub ClearBlackList_Click(sender As Object, e As EventArgs) Handles ClearBlacklist.Click
        UserProgramsBlackList.Items.Clear()
        Try
            Dim BlackListText As String = Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\blacklist\keppymididrv.blacklist"
            Dim Filenum As Integer = FreeFile()

            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Clearing blacklist...")

            FileOpen(Filenum, BlackListText, OpenMode.Output)
            FileClose()

            Using SW As New IO.StreamWriter(BlackListText, True)
                For Each itm As String In Me.UserProgramsBlackList.Items
                    SW.WriteLine(itm)
                Next
            End Using
            UserProgramsBlackList.TopIndex = UserProgramsBlackList.Items.Count - 1
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Blacklist cleared.")
        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Dim BlackListText As String = Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\blacklist\keppymididrv.blacklist"
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Error while saving blacklist file: " + BlackListText)
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Is the file accessible by the user/locked by SYSTEM?")
            MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Error")
            Console.ForegroundColor = ConsoleColor.Green
        End Try
    End Sub

    Private Sub SystemList_Click(sender As Object, e As EventArgs) Handles SystemList.Click
        Try
            Dim procStartInfo As New ProcessStartInfo
            Dim procExecuting As New Process
            With procStartInfo
                .UseShellExecute = True
                .FileName = "notepad.exe"
                .Arguments = Environment.GetEnvironmentVariable("WINDIR") + "\keppymididrv.defaultblacklist"
                .WindowStyle = ProcessWindowStyle.Normal
                .Verb = "runas" 'add this to prompt for elevation
            End With
            procExecuting = Process.Start(procStartInfo)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BlackListAdvancedMode_CheckedChanged(sender As Object, e As EventArgs) Handles BlackListAdvancedMode.CheckedChanged
        If BlackListAdvancedMode.Checked = True Then
            BlackListDef.Text = "Type the name of the program in the textbox."
            AddBlackList.Text = "Add executable"
            ManualBlackListLabel.Enabled = True
            ManualBlackListLabel.Visible = True
            ManualBlackList.Enabled = True
            ManualBlackList.Visible = True
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Manual blacklist mode enabled.")
        Else
            BlackListDef.Text = "Select a program by clicking ''Add executable(s)''."
            AddBlackList.Text = "Add executable(s)"
            ManualBlackListLabel.Enabled = False
            ManualBlackListLabel.Visible = False
            ManualBlackList.Enabled = False
            ManualBlackList.Visible = False
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Manual blacklist mode disabled.")
        End If
    End Sub

    Private Sub ManualBlackList_KeyDown(sender As Object, e As EventArgs) Handles ManualBlackList.KeyDown
        If GetAsyncKeyState(Keys.Enter) Then
            UserProgramsBlackList.Items.Add(ManualBlackList.Text)
            Try
                Dim BlackListText As String = Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\blacklist\keppymididrv.blacklist"
                Dim Filenum As Integer = FreeFile()

                FileOpen(Filenum, BlackListText, OpenMode.Output)
                FileClose()

                Using SW As New IO.StreamWriter(BlackListText, True)
                    For Each itm As String In Me.UserProgramsBlackList.Items
                        SW.WriteLine(itm)
                    Next
                End Using
                UserProgramsBlackList.TopIndex = UserProgramsBlackList.Items.Count - 1
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Added program to the blacklist (Manual mode): " + ManualBlackList.Text)
            Catch ex As Exception
                Console.ForegroundColor = ConsoleColor.Red
                Dim BlackListText As String = Environment.GetEnvironmentVariable("LocalAppData") + "\Keppy's Driver\blacklist\\keppymididrv.blacklist"
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Error while saving blacklist file: " + BlackListText)
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Is the file accessible by the user/locked by SYSTEM?")
                MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Error")
                Console.ForegroundColor = ConsoleColor.Green
            End Try
        End If
    End Sub

    Private Sub DisableCheckPortA_CheckedChanged(sender As Object, e As EventArgs) Handles DisableCheckPortA.CheckedChanged
        If DisableCheckPortA.Checked = True Then
            DisableCheckPortB.Checked = True
            DisableCheckPortC.Checked = True
            DisableCheckPortD.Checked = True
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "File extension check for Port A/Port B has been enabled.")
        Else
            DisableCheckPortB.Checked = False
            DisableCheckPortC.Checked = False
            DisableCheckPortD.Checked = False
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "File extension check for Port A/Port B has been disabled.")
        End If
    End Sub

    Private Sub DisableCheckPortB_CheckedChanged(sender As Object, e As EventArgs) Handles DisableCheckPortB.CheckedChanged
        If DisableCheckPortB.Checked = True Then
            DisableCheckPortA.Checked = True
            DisableCheckPortC.Checked = True
            DisableCheckPortD.Checked = True
        Else
            DisableCheckPortA.Checked = False
            DisableCheckPortC.Checked = False
            DisableCheckPortD.Checked = False
        End If
    End Sub

    Private Sub DisableCheckPortC_CheckedChanged(sender As Object, e As EventArgs) Handles DisableCheckPortC.CheckedChanged
        If DisableCheckPortB.Checked = True Then
            DisableCheckPortA.Checked = True
            DisableCheckPortB.Checked = True
            DisableCheckPortD.Checked = True
        Else
            DisableCheckPortA.Checked = False
            DisableCheckPortB.Checked = False
            DisableCheckPortD.Checked = False
        End If
    End Sub

    Private Sub DisableCheckPortD_CheckedChanged(sender As Object, e As EventArgs) Handles DisableCheckPortD.CheckedChanged
        If DisableCheckPortB.Checked = True Then
            DisableCheckPortA.Checked = True
            DisableCheckPortB.Checked = True
            DisableCheckPortC.Checked = True
        Else
            DisableCheckPortA.Checked = False
            DisableCheckPortB.Checked = False
            DisableCheckPortC.Checked = False
        End If
    End Sub

    Private Sub ClockTimer_Tick(sender As Object, e As EventArgs) Handles ClockTimer.Tick
        ClockSys.Text = Format(Now, "HH:mm:ss") & vbCrLf & Format(Now, "dd/MM/yyyy")
    End Sub

    Private Sub InformationsAboutThisProgramToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InformationsAboutThisProgramToolStripMenuItem.Click
        Informations.ShowDialog()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub DownloadTheSourceCodeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DownloadTheSourceCodeToolStripMenuItem.Click
        Process.Start("https://github.com/KaleidonKep99/Keppy-s-MIDI-Driver")
    End Sub

    Private Sub VisitKeppyStudiosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VisitKeppyStudiosToolStripMenuItem.Click
        Process.Start("http://keppystudios.com")
    End Sub

    Private Sub ReportABugToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportABugToolStripMenuItem.Click
        Select Case MsgBox("Do you want to report a bug?", MsgBoxStyle.YesNo, "Report a bug...")
            Case MsgBoxResult.Yes
                Process.Start("https://github.com/KaleidonKep99/Keppy-s-MIDI-Driver/issues")
            Case MsgBoxResult.No

        End Select
    End Sub

    Private Sub Tabs_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tabs1.SelectedIndexChanged
        If Tabs1.SelectedTab Is Port1 Or Tabs1.SelectedTab Is Port2 Or Tabs1.SelectedTab Is Port3 Or Tabs1.SelectedTab Is Port4 Then
            CanIChangeTheSoundfontInRealtimeToolStripMenuItem.Visible = True
        Else
            CanIChangeTheSoundfontInRealtimeToolStripMenuItem.Visible = False
        End If
    End Sub

    Private Sub CanIChangeTheSoundfontInRealtimeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CanIChangeTheSoundfontInRealtimeToolStripMenuItem.Click
        MsgBox("YES, you can. Just focus on the MIDI program by clicking over it, and press ''CTRL+List Number''. The driver will load the new soundfont(s)." & vbCrLf & vbCrLf & "CTRL + 1: Reload list 1" & vbCrLf & "CTRL + 2: Reload list 2" & vbCrLf & "CTRL + 3: Reload list 3" & vbCrLf & "CTRL + 4: Reload list 4", 64, "Can I change the soundfonts in real-time?")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        MsgBox(My.Resources.Credits.ToString, 64, "Credits")
        MsgBox(My.Resources.Credits2.ToString, 64, "Credits")
    End Sub

    Private Sub PolyphonyLimit_ValueChanged() Handles PolyphonyLimit.ValueChanged
        If PolyphonyLimit.Value > 1000 Then
            If PolyphonyLimit.Value <= 1500 Then
                VoiceWarning.Image = My.Resources.Warning_Box_Yellow
                VoiceWarning.Visible = True
            ElseIf PolyphonyLimit.Value >= 1501 Then
                VoiceWarning.Image = My.Resources.exclamation_red
                VoiceWarning.Visible = True
            End If
        ElseIf PolyphonyLimit.Value <= 1000 Then
            VoiceWarning.Image = My.Resources.Warning_Box_Yellow
            VoiceWarning.Visible = False
        End If
    End Sub

    Private Sub bufsize_ValueChanged() Handles bufsize.ValueChanged
        If bufsize.Value <= 10 Then
            BufferWarning.Image = My.Resources.exclamation_red
            BufferWarning.Visible = True
        End If
        If bufsize.Value >= 15 Then
            If bufsize.Value < 15 Then
                BufferWarning.Image = My.Resources.Warning_Box_Yellow
                BufferWarning.Visible = True
            ElseIf bufsize.Value >= 15 Then
                BufferWarning.Image = My.Resources.Warning_Box_Yellow
                BufferWarning.Visible = False
            End If
        End If
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click
        MsgBox(My.Resources.Shortcuts.ToString, 64, "Keyboard shortcuts")
    End Sub

    Private Sub VoiceWarning_Click(sender As Object, e As EventArgs) Handles VoiceWarning.Click
        MsgBox("Be careful with these values!" & vbCrLf & "If the value is too high, the driver will likely start stuttering, or, in the worst cases, crash.", 48, "Warning")
    End Sub

    Private Sub BufferWarning_Click(sender As Object, e As EventArgs) Handles BufferWarning.Click
        MsgBox("Be careful with these values!" & vbCrLf & "If the value is too high, the driver will likely start stuttering, or, in the worst cases, crash.", 48, "Warning")
    End Sub

    Private Sub RealTimeSet_CheckedChanged(sender As Object, e As EventArgs) Handles RealTimeSet.CheckedChanged
        If RealTimeSet.Checked = True Then
            Try
                Dim UserString As String
                UserString = "Software\Keppy's Driver"
                Dim keppykey = My.Computer.Registry.CurrentUser.OpenSubKey(UserString + "\Settings", True)
                keppykey.SetValue("realtimeset", "1", RegistryValueKind.DWord)
                RealTimeSetText.Visible = True
                SincInter.Text = "* Enable sinc interpolation. (Avoids audio corruptions, but can completely ruin the audio with Black MIDIs.)"
                DisableFX.Text = "* Disable sound effects. (Disable the sound effects, such as reverb and chorus. Also, this can reduce the CPU usage.)"
                BufferText.Text = "* Set a buffer length for the driver, from 1 to 100:"
                NoteOff.Text = "* Only release the oldest instance upon a note off event when there are overlapping instances of the note."
                SysResetIgnore.Text = "* Ignore system reset events when the system mode is unchanged."
                Label3.Text = "* Set the voice limit for the driver, from 1 to 100000:"
                Label4.Text = "* && ** Set the MIDI tracks that BASSMIDI is allowed to use (Different values from 16 will set the drums channel to melodic):"
                Label5.Text = "* Set the maximum rendering time percentage/maximum CPU usage for BASS/BASSMIDI:"
            Catch ex As Exception
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Error while disabling the real-time settings!!!")
                MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Error")
                Console.ForegroundColor = ConsoleColor.Green
            End Try
        ElseIf RealTimeSet.Checked = False Then
            Try
                Dim UserString As String
                UserString = "Software\Keppy's Driver"
                Dim keppykey = My.Computer.Registry.CurrentUser.OpenSubKey(UserString + "\Settings", True)
                keppykey.SetValue("realtimeset", "0", RegistryValueKind.DWord)
                RealTimeSetText.Visible = False
                SincInter.Text = "Enable sinc interpolation. (Avoids audio corruptions, but can completely ruin the audio with Black MIDIs.)"
                DisableFX.Text = "Disable sound effects. (Disable the sound effects, such as reverb and chorus. Also, this can reduce the CPU usage.)"
                BufferText.Text = "Set a buffer length for the driver, from 1 to 100:"
                NoteOff.Text = "Only release the oldest instance upon a note off event when there are overlapping instances of the note."
                SysResetIgnore.Text = "Ignore system reset events when the system mode is unchanged."
                Label3.Text = "Set the voice limit for the driver, from 1 to 100000:"
                Label4.Text = "** Set the MIDI tracks that BASSMIDI is allowed to use (Different values from 16 will set the drums channel to melodic):"
                Label5.Text = "Set the maximum rendering time percentage/maximum CPU usage for BASS/BASSMIDI:"
            Catch ex As Exception
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Error while disabling the real-time settings!!!")
                MsgBox(ex.Message.ToString, MsgBoxStyle.Critical, "Error")
                Console.ForegroundColor = ConsoleColor.Green
            End Try
        End If
    End Sub

    Private Sub FirstRunTimer_Tick(sender As Object, e As EventArgs) Handles FirstRunTimer.Tick
        FirstRunTimer.Stop()
        FirstRunTutorial()
    End Sub

    Private Sub FirstRunTutorial()
        Dim UserString As String
        UserString = "Software\Keppy's Driver"
        Dim keppykey = My.Computer.Registry.CurrentUser.OpenSubKey(UserString + "\Settings", True)
        MsgBox("Hello, and thanks for installing my driver! :)" & vbCrLf & "Let's get a look of the main functions here, shall we?", 64, "Hey!")
        Tabs1.SelectedTab = Port1
        PortABox.BackColor = Color.Gold
        MsgBox("This is the list of the port 1, where you place all your sound banks/soundfonts!" & vbCrLf & "Be sure to use valid files!" & vbCrLf & vbCrLf & "(There's no need to see Port 2, it's the same)", 64, "Message")
        PortABox.BackColor = Color.White
        Tabs1.SelectedTab = TabPage3
        MsgBox("This is the blacklist system, where you can blacklist some annoying programs, like Google Chrome, that are constantly locking the driver.", 64, "Message")
        SystemList.BackColor = Color.Red
        MsgBox("With this button, you'll be able to edit the system blacklist, be careful.", 48, "Warning")
        SystemList.BackColor = Color.Transparent
        Tabs1.SelectedTab = TabPage4
        MsgBox("Here is the vital part of the driver, where all the required values are stored. You can edit them with by your own, but it's better if you leave it as is.", 64, "Message")
        Tabs1.SelectedTab = Port1
        MsgBox("I (KaleidonKep99) hope you like it!" & vbCrLf & "And if you find any bug, just press the ''Report a bug'' item in the following ''?'' stripmenu!", 64, "Goodbye")
        keppykey.SetValue("firstrun", "0", RegistryValueKind.DWord)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        Dim causeOfFailure As String = "Unhandled exception."
        Try
            Environment.FailFast(causeOfFailure)
        Finally
            Console.WriteLine("")
        End Try
    End Sub

    Private Sub UpdateDownload_Click(sender As Object, e As EventArgs) Handles UpdateDownload.Click
        Try
            Dim request As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create("https://raw.githubusercontent.com/KaleidonKep99/Keppy-s-MIDI-Driver/master/output/keppydriverupdate.txt")
            Dim response As System.Net.HttpWebResponse = request.GetResponse()
            Dim sr As System.IO.StreamReader = New System.IO.StreamReader(response.GetResponseStream())
            Dim newestversion As String = sr.ReadToEnd()
            Dim Driver As FileVersionInfo = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\keppydrv\keppydrv.dll")
            ThisVersionDriver.Text = "The current version of the driver, installed on your system, is: " + Driver.FileVersion.ToString
            LatestVersionDriver.Text = "The latest version online, in the GitHub repository, is: " + newestversion.ToString
            If newestversion > Driver.FileVersion Then
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Update found!")
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "New version: " + newestversion)
                MsgBox("New update found, press OK to open the release page.", 48, "New update found!")
                Process.Start("http://goo.gl/BHgazb")
            Else
                Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "No updates found.")
                MsgBox("This release is already updated.", 64, "No updates found.")
            End If
        Catch ex As Exception
            Dim Driver As FileVersionInfo = FileVersionInfo.GetVersionInfo(Environment.SystemDirectory + "\keppydrv\keppydrv.dll")
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(Format(Now, "[HH:mm:ss]") + " " + "Can not check for updates!")
            Console.ForegroundColor = ConsoleColor.Green
            ThisVersionDriver.Text = "The current version of the driver, installed on your system, is: " + Driver.FileVersion.ToString
            LatestVersionDriver.Text = "Can not check for updates. You're offline, or maybe the website is temporarily down."
            MsgBox("Can not check for updates!" & vbCrLf & vbCrLf & "Specific .NET error:" & vbCrLf & ex.Message.ToString, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub ExtListPortA_Click(sender As Object, e As EventArgs) Handles ExtListPortA.Click
        If ExtListPortADialog.ShowDialog = DialogResult.OK Then
            For Each itm In ExtListPortADialog.FileNames
                Try
                    Dim PortASFList As String = itm
                    Dim reader As StreamReader = New StreamReader(New FileStream(PortASFList, FileMode.Open))
                    Do While Not reader.EndOfStream
                        PortABox.Items.Add(reader.ReadLine())
                    Loop
                    reader.Close()
                Catch ex As Exception

                End Try
            Next
        End If
    End Sub

    Private Sub ExtListPortB_Click(sender As Object, e As EventArgs) Handles ExtListPortB.Click
        If ExtListPortBDialog.ShowDialog = DialogResult.OK Then
            For Each itm In ExtListPortBDialog.FileNames
                Try
                    Dim PortBSFList As String = itm
                    Dim reader As StreamReader = New StreamReader(New FileStream(PortBSFList, FileMode.Open))
                    Do While Not reader.EndOfStream
                        PortBBox.Items.Add(reader.ReadLine())
                    Loop
                    reader.Close()
                Catch ex As Exception

                End Try
            Next
        End If
    End Sub

    Private Sub ExtListPortC_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If ExtListPortCDialog.ShowDialog = DialogResult.OK Then
            For Each itm In ExtListPortCDialog.FileNames
                Try
                    Dim PortCSFList As String = itm
                    Dim reader As StreamReader = New StreamReader(New FileStream(PortCSFList, FileMode.Open))
                    Do While Not reader.EndOfStream
                        PortCBox.Items.Add(reader.ReadLine())
                    Loop
                    reader.Close()
                Catch ex As Exception

                End Try
            Next
        End If
    End Sub

    Private Sub ExtListPortD_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If ExtListPortDDialog.ShowDialog = DialogResult.OK Then
            For Each itm In ExtListPortDDialog.FileNames
                Try
                    Dim PortDSFList As String = itm
                    Dim reader As StreamReader = New StreamReader(New FileStream(PortDSFList, FileMode.Open))
                    Do While Not reader.EndOfStream
                        PortDBox.Items.Add(reader.ReadLine())
                    Loop
                    reader.Close()
                Catch ex As Exception

                End Try
            Next
        End If
    End Sub

    Private Sub OpenDebugWindowToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenDebugWindowToolStripMenuItem.Click
        DebugWin.Show()
    End Sub

    Private Sub SFZ1_Click(sender As Object, e As EventArgs) Handles SFZ1.Click
        Process.Start("sfzguide.txt")
    End Sub

    Private Sub SFZ2_Click(sender As Object, e As EventArgs) Handles SFZ2.Click
        Process.Start("sfzguide.txt")
    End Sub

    Private Sub SFZ3_Click(sender As Object, e As EventArgs) Handles SFZ3.Click
        Process.Start("sfzguide.txt")
    End Sub

    Private Sub SFZ4_Click(sender As Object, e As EventArgs) Handles SFZ4.Click
        Process.Start("sfzguide.txt")
    End Sub

    Private Sub PolyphonyLimit_ValueChanged(sender As Object, e As EventArgs) Handles PolyphonyLimit.ValueChanged

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles NoDX8FX.CheckedChanged
        If NoDX8FX.Checked = True Then
            GroupBox3.Enabled = False
        Else
            GroupBox3.Enabled = True
        End If
    End Sub
End Class

Public Class Win32
    <DllImport("kernel32.dll")> Public Shared Function AllocConsole() As Boolean
    End Function
    <DllImport("kernel32.dll")> Public Shared Function FreeConsole() As Boolean
    End Function
End Class
