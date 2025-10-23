Imports System.Runtime.CompilerServices
Imports NAudio.Wave
Imports NAudio.Wave.SampleProviders
Public Class AudioPlayer
    Private waveOut As WaveOutEvent
    Private wavReader As AudioFileReader
    '    Private waveformData As List(Of Single)
    '   Private viewPortCenterTimeMs As Integer = -1
    '  Private viewPortDurationSeconds As Integer = 10
    '    Private speedControl As SmbPitchShiftingSampleProvider
    Private startMilis As Long
    '   Public WaveformImage As Bitmap
    Public TotalMillis As Long

    Private playstate As Integer = 0 ' 0=stopped, 1=playing, 2=paused
    Public ReadOnly Property getTime As TimeSpan
        Get
            If wavReader IsNot Nothing Then
                Return wavReader.CurrentTime
            Else
                Return TimeSpan.Zero
            End If
        End Get
    End Property


    ' Load a song by file path
    Public Sub LoadSong(filePath As String)
        StopPlayback()

        wavReader = New AudioFileReader(filePath)
        TotalMillis = wavReader.TotalTime.TotalMilliseconds
        waveOut = New WaveOutEvent()
        waveOut.Init(wavReader)
        'waveOut.Play()
    End Sub

    Public Sub PlayPause()
        If waveOut IsNot Nothing Then
            If playstate = 1 Then
                Pause()
            ElseIf playstate = 2 Or playstate = 0 Then
                Play()
            End If
        End If
    End Sub

    ' Play the loaded song
    Public Sub Play()
        If waveOut IsNot Nothing Then
            waveOut.Play()
            playstate = 1
        End If
    End Sub

    ' Pause the playback
    Public Sub Pause()
        If waveOut IsNot Nothing Then
            waveOut.Pause()
            playstate = 2
        End If
    End Sub

    ' Stop playback
    Public Sub StopPlayback()
        If waveOut IsNot Nothing Then
            waveOut.Stop()
            waveOut.Dispose()
            wavReader.Dispose()
            waveOut = Nothing
            wavReader = Nothing
            playstate = 0
        End If
    End Sub

    ' Rewind the song (go to the beginning)
    Public Sub Rewind()
        If wavReader IsNot Nothing Then
            wavReader.Position = 0
        End If
    End Sub
    Public Sub BackUp(numSecs As Integer)
        If wavReader IsNot Nothing Then
            Dim newPosition As Long = wavReader.Position - (wavReader.WaveFormat.AverageBytesPerSecond * numSecs)
            wavReader.Position = Math.Max(newPosition, 0)
        End If
    End Sub
    ' Fast forward the song by a specified number of seconds
    Public Sub FastForward(seconds As Integer)
        If wavReader IsNot Nothing Then
            Dim newPosition As Long = wavReader.Position + (wavReader.WaveFormat.AverageBytesPerSecond * seconds)
            wavReader.Position = Math.Min(newPosition, wavReader.Length)
        End If
    End Sub
    Public ReadOnly Property getMillies As Long
        Get
            If wavReader Is Nothing Then
                Return 0
            Else
                Return wavReader.CurrentTime.TotalMilliseconds 'msAccurate
            End If
        End Get
    End Property


    ' Seek to a specific position using the trackbar
    Public Sub SeekTrackBarPosition(trackBar As TrackBar)
        If wavReader IsNot Nothing AndAlso trackBar IsNot Nothing Then
            wavReader.CurrentTime = TimeSpan.FromMilliseconds(trackBar.Value * 100)
        End If
    End Sub

    Public Sub SeekMS(newVal As Long)
        wavReader.CurrentTime = TimeSpan.FromMilliseconds(newVal)
    End Sub

End Class

Public Module audiostuff
    'Public AP As New AudioPlayer
End Module