Imports System.ComponentModel
Imports System.IO
Imports System.IO.Ports
Imports System.Threading
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel

Public Class RecordSong
    Public BaseName As String
    Private SP As New SerialPort
    Private AP As New AudioPlayer
    Private WithEvents BWPots As New BackgroundWorker
    Private Class RecordAll
        Public Property Time As Long
        Public Property ASCDown As Integer
        Public Property ASCUp As Integer
    End Class
    Private theRecord As New List(Of RecordAll)
    Private lastDown As Integer = 0
    Private LastUp As Integer = 0

    Private Class RecPots
        Private Millies As Long
        Private POTS As String

        Public Sub New(theMillies As Long, thePOTS As String)
            Millies = theMillies
            POTS = thePOTS
        End Sub

        Public ReadOnly Property theMillies As Long
            Get
                Return Millies
            End Get
        End Property
        Public ReadOnly Property thePOTS As String
            Get
                Return POTS
            End Get
        End Property
    End Class


    Private PotList As New List(Of RecPots)



    Private Sub DisableAllButtons()
        bnStopHead.Enabled = False
        bnRecHead.Enabled = False
        bnStopMouth.Enabled = False
        bnRecMouth.Enabled = False
        bnStopLeft.Enabled = False
        bnRecLeft.Enabled = False
        bnStopRight.Enabled = False
        bnRecRight.Enabled = False
        bnStopNeck.Enabled = False
        bnRecNeck.Enabled = False
    End Sub
    Private Sub EnableAllButtons()
        bnRecHead.Enabled = True
        bnRecMouth.Enabled = True
        bnRecLeft.Enabled = True
        bnRecRight.Enabled = True
        bnRecNeck.Enabled = True
    End Sub
    Private Sub bnRecMouth_Click(sender As Object, e As EventArgs) Handles bnRecMouth.Click
        DisableAllButtons()
        bnStopMouth.Enabled = True
        KeyPreview = True
        theRecord.Clear()
        lastDown = 0
        LastUp = 0
        AP.LoadSong($"{BaseName}.wav")
        AP.Rewind()
        AP.Play()
    End Sub

    Private Sub bnStopMouth_Click(sender As Object, e As EventArgs) Handles bnStopMouth.Click
        AP.StopPlayback()
        KeyPreview = False
        Using writerKey As New StreamWriter($"{BaseName}_Mouth.csv")
            writerKey.WriteLine("Time,ASCDown,ASCUp")
            For Each rec In theRecord
                If rec.ASCDown <> 0 OrElse rec.ASCUp <> 0 Then
                    writerKey.WriteLine($"{rec.Time},{rec.ASCDown},{rec.ASCUp}")
                End If
            Next
        End Using
        DisableAllButtons()
        EnableAllButtons()

    End Sub

    Private Sub bnRecHead_Click(sender As Object, e As EventArgs) Handles bnRecHead.Click
        DisableAllButtons()
        bnStopHead.Enabled = True
        If Not SP.IsOpen Then
            SP.Open()
            Thread.Sleep(1000) ' Wait for the port to open
        End If
        PotList.Clear()
        AP.LoadSong(BaseName & ".wav")
        AP.Rewind()
        AP.Play()
        BWPots.RunWorkerAsync()

    End Sub

    Private Sub RecordSong_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SP.PortName = "COM8"  ' Adjust as needed
        SP.BaudRate = 115200
        SP.NewLine = vbLf
        BWPots.WorkerReportsProgress = True
        BWPots.WorkerSupportsCancellation = True

    End Sub

    Private Sub thebw_DoWork(sender As Object, e As DoWorkEventArgs) Handles BWPots.DoWork
        If SP.IsOpen Then
            While Not BWPots.CancellationPending
                PotList.Add(New RecPots(AP.getMillies, SP.ReadLine()))
                Threading.Thread.Sleep(100)  ' grab all data as fast as possible.  sort it out later!
            End While
        End If
    End Sub

    Private Sub bnStopHead_Click(sender As Object, e As EventArgs) Handles bnStopHead.Click
        BWPots.CancelAsync()

        While BWPots.CancellationPending
            Application.DoEvents()
        End While
        AP.StopPlayback()

        If SP.IsOpen Then
            SP.Close()
        End If
        Dim fileName = $"{BaseName}_head.csv"
        Using writer As New StreamWriter(fileName)
            For Each rec In PotList
                writer.WriteLine($"{rec.theMillies},{rec.thePOTS}")
            Next
        End Using
        bnStopHead.Enabled = False
        EnableAllButtons()
    End Sub

    Private Sub bnRecNeck_Click(sender As Object, e As EventArgs) Handles bnRecNeck.Click
        DisableAllButtons()
        bnStopNeck.Enabled = True
        If Not SP.IsOpen Then
            SP.Open()
            Thread.Sleep(1000) ' Wait for the port to open
        End If
        PotList.Clear()
        AP.LoadSong(BaseName & ".wav")
        AP.Rewind()
        AP.Play()
        BWPots.RunWorkerAsync()
    End Sub

    Private Sub bnStopNeck_Click(sender As Object, e As EventArgs) Handles bnStopNeck.Click
        BWPots.CancelAsync()

        While BWPots.CancellationPending
            Application.DoEvents()
        End While
        AP.StopPlayback()

        If SP.IsOpen Then
            SP.Close()
        End If
        Dim fileName = $"{BaseName}_neck.csv"
        Using writer As New StreamWriter(fileName)
            For Each rec In PotList
                writer.WriteLine($"{rec.theMillies},{rec.thePOTS}")
            Next
        End Using
        bnStopNeck.Enabled = False
        EnableAllButtons()
        MsgBox("Done")
    End Sub

    Private Sub RecordSong_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If LastUp = 0 OrElse e.KeyValue <> LastUp Then
            theRecord.Add(New RecordAll With {
                .Time = AP.getMillies,
                .ASCDown = 0,
                .ASCUp = e.KeyValue
            })
            lastDown = 0
            LastUp = e.KeyValue
        End If
        e.Handled = True
    End Sub

    Private Sub RecordSong_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If lastDown = 0 OrElse e.KeyValue <> lastDown Then
            theRecord.Add(New RecordAll With {
                .Time = AP.getMillies,
                .ASCDown = e.KeyValue,
                .ASCUp = 0
            })
            lastDown = e.KeyValue
            LastUp = 0
        End If
        e.Handled = True
    End Sub

    Private Sub bnStopLeft_Click(sender As Object, e As EventArgs) Handles bnStopLeft.Click
        AP.StopPlayback()
        KeyPreview = False
        Using writerKey As New StreamWriter($"{BaseName}_Left.csv")
            writerKey.WriteLine("Time,ASCDown,ASCUp")
            For Each rec In theRecord
                If rec.ASCDown <> 0 OrElse rec.ASCUp <> 0 Then
                    writerKey.WriteLine($"{rec.Time},{rec.ASCDown},{rec.ASCUp}")
                End If
            Next
        End Using
        DisableAllButtons()
        EnableAllButtons()
    End Sub

    Private Sub bnStopRight_Click(sender As Object, e As EventArgs) Handles bnStopRight.Click
        AP.StopPlayback()
        KeyPreview = False
        Using writerKey As New StreamWriter($"{BaseName}_Right.csv")
            writerKey.WriteLine("Time,ASCDown,ASCUp")
            For Each rec In theRecord
                If rec.ASCDown <> 0 OrElse rec.ASCUp <> 0 Then
                    writerKey.WriteLine($"{rec.Time},{rec.ASCDown},{rec.ASCUp}")
                End If
            Next
        End Using
        DisableAllButtons()
        EnableAllButtons()
    End Sub

    Private Sub bnRecLeft_Click(sender As Object, e As EventArgs) Handles bnRecLeft.Click
        DisableAllButtons()
        bnStopLeft.Enabled = True
        KeyPreview = True
        theRecord.Clear()
        lastDown = 0
        LastUp = 0
        AP.LoadSong($"{BaseName}.wav")
        AP.Rewind()
        AP.Play()

    End Sub

    Private Sub bnRecRight_Click(sender As Object, e As EventArgs) Handles bnRecRight.Click
        DisableAllButtons()
        bnStopRight.Enabled = True
        KeyPreview = True
        theRecord.Clear()
        lastDown = 0
        LastUp = 0
        AP.LoadSong($"{BaseName}.wav")
        AP.Rewind()
        AP.Play()
    End Sub
End Class