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
        x.SetBasePath(IO.Path.Combine(FinalPath, BaseName))
        x.ShowDialog(Me)
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Using p As New Preview2
            p.BasePath = IO.Path.Combine(FinalPath, BaseName)
            p.ShowDialog(Me)
        End Using
    End Sub

    Private Sub Button5_Click_1(sender As Object, e As EventArgs) Handles Button5.Click
        Using d As New Debuging
            d._theSong = IO.Path.Combine(FinalPath, BaseName)
            d.ShowDialog(Me)
        End Using
    End Sub
End Class
