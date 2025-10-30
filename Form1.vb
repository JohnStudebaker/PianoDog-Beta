Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel

Public Class Form1
    Public FinalPath As String
    Public TempPath As String
    Public BaseName As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        ForceTimeSync()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If True Then
            With OpenFileDialog1
                .Title = "Select a WAV"
                .Filter = "Wav files (*.wav)|*.wav"
                .Multiselect = False
                .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                .CheckFileExists = True
                If .ShowDialog() = DialogResult.OK Then
                    FinalPath = IO.Path.GetDirectoryName(.FileName)
                    TempPath = IO.Path.Combine(FinalPath, "Temp")
                    BaseName = IO.Path.GetFileNameWithoutExtension(.FileName)
                    If Not IO.Directory.Exists(TempPath) Then
                        IO.Directory.CreateDirectory(TempPath)
                    End If
                End If
            End With
        Else
            FinalPath = ""
            TempPath = ""
            BaseName = ""
        End If
        If BaseName.Length > 0 Then
            If IO.File.Exists($"{IO.Path.Combine(FinalPath, BaseName)}.wav") Then
                lbWav.Text = $"{IO.Path.Combine(FinalPath, BaseName)}.wav"
            Else
                lbWav.Text = $".wav Not Found."
            End If
            If IO.File.Exists($"{IO.Path.Combine(FinalPath, BaseName)}_Final_Output.csv") Then
                lbFinal.Text = $"{IO.Path.Combine(FinalPath, BaseName)}_Final_Output.csv"
            Else
                lbFinal.Text = "Final Not Found."
            End If
        End If

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim j As New JoystickRecorderForm
        j.baseName = BaseName
        j.finalPath = FinalPath
        j.tempPath = TempPath
        j.ShowDialog(Me)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Dim x As New Showtime
        x.WAVFile = IO.Path.Combine(FinalPath, BaseName & ".wav")
        x.CSVFile = IO.Path.Combine(FinalPath, BaseName & "_final_output.csv")
        x.JPGFile = IO.Path.Combine(FinalPath, BaseName & ".jpg")
        x.ShowDialog(Me)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        OpenFileDialog1.Title = "Select a WAV"
        OpenFileDialog1.Filter = "WAV files (*.Wav)|*.wav"
        OpenFileDialog1.Multiselect = False
        OpenFileDialog1.InitialDirectory = IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "PianoDog", "Done")
        OpenFileDialog1.CheckFileExists = True
        If OpenFileDialog1.ShowDialog() <> DialogResult.OK Then
            Return
        End If

        Using p As New Preview2
            p.basePath = IO.Path.Combine(IO.Path.GetDirectoryName(OpenFileDialog1.FileName), IO.Path.GetFileNameWithoutExtension(OpenFileDialog1.FileName))
            p.ShowDialog(Me)
        End Using
    End Sub

    Private Sub Button5_Click_1(sender As Object, e As EventArgs) Handles Button5.Click
        Using d As New Debuging
            d._theSong = IO.Path.Combine(FinalPath, BaseName)
            d.ShowDialog(Me)
        End Using
    End Sub

    Private Sub Button7_Click_1(sender As Object, e As EventArgs) Handles Button7.Click
        Dim sFiles() As String
        Dim pd As New PianoDog
        pd.TimeSync(GetLocalIPv4)
        sFiles = IO.Directory.GetFiles("C:\Users\john_\Desktop\PianoDog\Done", "*.wav")
        For i = 0 To sFiles.Length - 1
            sFiles(i) = IO.Path.Combine(IO.Path.GetDirectoryName(sFiles(i)), IO.Path.GetFileNameWithoutExtension(sFiles(i)))
            If Not IO.File.Exists($"{sFiles(i)}.wav") Then
                sFiles(i) = ""
            End If
            If Not IO.File.Exists($"{sFiles(i)}_final_output.csv") Then
                sFiles(i) = ""
            End If
            If Not IO.File.Exists($"{sFiles(i)}.jpg") Then
                sFiles(i) = ""
            End If
        Next
        Dim rnd As New Random
        sFiles = sFiles.OrderBy(Function(x) rnd.Next).ToArray
        For Each s In sFiles
            If s.Length > 0 Then
                Using t As New Showtime
                    t.WAVFile = s & ".wav"
                    t.CSVFile = s & "_final_output.csv"
                    t.JPGFile = s & ".jpg"
                    t.ShowDialog(Me)
                End Using
            End If
        Next
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Preview3.Show()
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Dim sfiles() As String
        sfiles = IO.Directory.GetFiles("C:\Users\john_\Desktop\PianoDog\Done", "*_final_output.csv")
        For i = 8 To 8 'sfiles.Length - 1
            ModifyFinal(sfiles(i))
        Next
        MsgBox("Done")
    End Sub
End Class
