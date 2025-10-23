Imports System.ComponentModel
Imports System.Windows.Forms.AxHost
Imports SharpDX.DirectInput

Public Class JoystickRecorderForm
    Private _sourceBitmap As Bitmap
    Private joystickGuid As Guid = Guid.Empty
    Private JoystickDevice As Joystick
    Private WithEvents JoystickWorker As New BackgroundWorker()
    Private DI As New DirectInput()
    Private AP As New AudioPlayer
    Private theData As New List(Of JoyInfo)
    Public baseName As String
    Public finalPath As String
    Public tempPath As String
    Private _BitmapXStart As Single = 0.0F
    Private Const JoyMin As Integer = 0
    Private Const JoyMax As Integer = 65535

    Private Class JoyInfo
        Public Property Milliseconds As Long
        Public Property JoyState As JoystickState
    End Class

    Private Sub JoystickRecorderForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lbMouth.Text = $"{IO.Path.Combine(tempPath, baseName)}_mouth_raw.csv"
        If Not IO.File.Exists(lbMouth.Text) Then
            lbMouth.Text = ""
        End If
        lbHeadNeck.Text = $"{IO.Path.Combine(tempPath, baseName)}_head_raw.csv"
        If Not IO.File.Exists(lbHeadNeck.Text) Then
            lbHeadNeck.Text = ""
        End If
        lbLeft.Text = $"{IO.Path.Combine(tempPath, baseName)}_left_raw.csv"
        If Not IO.File.Exists(lbLeft.Text) Then
            lbLeft.Text = ""
        End If
        lbRight.Text = $"{IO.Path.Combine(tempPath, baseName)}_right_raw.csv"
        If Not IO.File.Exists(lbRight.Text) Then
            lbRight.Text = ""
        End If
        lbFinal.Text = $"{IO.Path.Combine(finalPath, baseName)}_final_output.csv"
        If Not IO.File.Exists(lbFinal.Text) Then
            lbFinal.Text = ""
        End If
        lbMHN.Text = $"{IO.Path.Combine(tempPath, baseName)}_mouth_head_neck.csv"
        If Not IO.File.Exists(lbMHN.Text) Then
            lbMHN.Text = ""
        End If
        JoystickWorker.WorkerSupportsCancellation = True
        JoystickWorker.WorkerReportsProgress = True
        AddHandler JoystickWorker.DoWork, AddressOf JoystickWorker_DoWork
        AddHandler JoystickWorker.RunWorkerCompleted, AddressOf joystickworker_runworkercompleted
        Me.Panel1.GetType().GetProperty("DoubleBuffered",
            Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic).SetValue(Panel1, True)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        _sourceBitmap = CreateWaveformBitmap($"{IO.Path.Combine(finalPath, baseName)}.wav")
        For Each deviceInstance In DI.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices)
            joystickGuid = deviceInstance.InstanceGuid
            Exit For
        Next
        If joystickGuid = Guid.Empty Then
            Console.WriteLine("No joystick connected.")
            Return
        End If
        JoystickDevice = New Joystick(DI, joystickGuid)
        JoystickDevice.Acquire()
        theData.Clear()
        AP.LoadSong($"{IO.Path.Combine(finalPath, baseName)}.wav")
        AP.Play()
        JoystickWorker.RunWorkerAsync()
        Timer1.Enabled = True
    End Sub

    Public Sub JoystickWorker_DoWork(sender As Object, e As DoWorkEventArgs)
        Dim s As JoystickState
        Dim lastSlider As Integer = 0
        Dim thisSlider As Integer = 0
        Dim thisButton8 As Boolean = False
        Dim lastButton8 As Boolean = False
        Dim StoreIt As Boolean = False
        While Not JoystickWorker.CancellationPending
            theData.Add(New JoyInfo With {.Milliseconds = AP.getMillies, .JoyState = JoystickDevice.GetCurrentState})
            Threading.Thread.Sleep(10)
        End While
        Panel1.Invalidate()
    End Sub

    Private Sub joystickworker_runworkercompleted(sender As Object, e As RunWorkerCompletedEventArgs)
        AP.StopPlayback()
        JoystickDevice.Unacquire()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        JoystickWorker.CancelAsync()
        lbHeadNeck.Text = $"{IO.Path.Combine(tempPath, baseName)}_head_raw.csv"
        SaveData(lbHeadNeck.Text)
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        JoystickWorker.CancelAsync()
        lbMouth.Text = $"{IO.Path.Combine(tempPath, baseName)}_mouth_raw.csv"
        SaveData(lbMouth.Text)
    End Sub

    Private Sub SaveData(filename As String)
        Timer1.Enabled = False
        'first need to find if any data needs to be deleted
        Dim startDelete As Long = -1
        Dim endDelete As Long = -1
        Dim bAgain As Boolean = False
        Dim DelRecs As New List(Of JoyInfo)
        Dim DelMarker As Integer
        Do
            bAgain = False
            For Each ji As JoyInfo In theData
                If ji.JoyState.Z = -3 AndAlso ji.JoyState.Y = -2 AndAlso ji.JoyState.X = -1 Then
                    'this is a marker for deletion
                    DelMarker = theData.IndexOf(ji)
                    startDelete = theData(DelMarker).Milliseconds - 5000 'delete the last 5 seconds
                    DelRecs.AddRange(theData.Where(Function(x) x.Milliseconds >= startDelete AndAlso theData.IndexOf(x) < DelMarker))
                    DelRecs.Add(ji) 'also delete the marker itself
                    DelRecs.AddRange(theData.Where(Function(x) x.Milliseconds < startDelete AndAlso theData.IndexOf(x) > DelMarker))
                    ' we backup 10, but only want 5, so delete the first 5 seconds after the marker too
                    bAgain = True
                    Exit For
                End If
            Next
            If DelRecs.Count > 0 Then
                'remove the deleted records from theData and start over
                For Each dr As JoyInfo In DelRecs
                    theData.Remove(dr)
                Next
                DelRecs.Clear()
            End If
        Loop While bAgain
        theData.Sort(Function(a, b) a.Milliseconds.CompareTo(b.Milliseconds))

        Using sw As New IO.StreamWriter(filename)
            sw.WriteLine("Milliseconds,X,Y,Z,Slider,Trigger,Hat,Button") ',Button2,Button3,Button4,Button5,Button6,Button7,Button8,Button9,Button10,Button11,Button12,Button13,Button14,Button15,Hat")
            For Each ji In theData
                Dim s As JoystickState = ji.JoyState
                'sw.WriteLine($"{ji.Milliseconds},{s.X},{s.Y},{s.RotationZ},{s.Sliders(0)},{s.Buttons(0)},{s.Buttons(1)},{s.Buttons(2)},{s.Buttons(3)},{s.Buttons(4)},{s.Buttons(5)},{s.Buttons(6)},{s.Buttons(7)},{s.Buttons(8)},{s.Buttons(9)},{s.Buttons(10)},{s.Buttons(11)},{s.Buttons(12)},{s.Buttons(13)},{s.Buttons(14)},{s.Buttons(15)},{s.PointOfViewControllers(0).ToString()}")
                sw.WriteLine($"{ji.Milliseconds},{s.X},{s.Y},{s.RotationZ},{s.Sliders(0)},{s.Buttons(0)},{s.PointOfViewControllers(0).ToString()}") ' keep using the trigger ,{s.Buttons(6)}")
            Next
        End Using
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        JoystickWorker.CancelAsync
        lbLeft.Text = $"{IO.Path.Combine(tempPath, baseName)}_left_raw.csv"
        SaveData(lbLeft.Text)
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        JoystickWorker.CancelAsync()
        lbRight.Text = $"{IO.Path.Combine(tempPath, baseName)}_right_raw.csv"
        SaveData(lbRight.Text)
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim jd As New JoyData
        'jd.MakeFinal(lbMouth.Text, lbHeadNeck.Text, lbRight.Text, lbLeft.Text, $"{IO.Path.Combine(finalPath, baseName)}_final_output.csv")
        jd.MakeFinal2(lbMHN.Text, lbRight.Text, lbLeft.Text, $"{IO.Path.Combine(finalPath, baseName)}_final_output.csv")
        MsgBox("DONE")
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        theData.Add(New JoyInfo With {.Milliseconds = AP.getMillies, .JoyState = New JoystickState() With {.X = -1, .Y = -2, .Z = -3}})
        AP.BackUp(10)
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        Const _zoomFactor As Single = 1.0F ' No zoom for now; can be adjusted later.
        Dim _bitmapXStart As Integer = 0

        If _sourceBitmap Is Nothing OrElse AP.TotalMillis = 0 Then
            e.Graphics.Clear(BackColor)
            Return
        End If
        e.Graphics.InterpolationMode = Drawing.Drawing2D.InterpolationMode.NearestNeighbor
        ' The destination is always the entire panel.
        Dim destRect As New Rectangle(0, 0, Panel1.Width, Panel1.Height)
        Dim srcRect As New Rectangle(0, 0, _sourceBitmap.Width, _sourceBitmap.Height)

        Dim playheadPositionInPanel As Single = CSng(AP.getMillies / AP.TotalMillis) * Panel1.Width
        ' Draw the (potentially zoomed) part of the bitmap onto the panel.
        e.Graphics.DrawImage(_sourceBitmap, destRect, srcRect, GraphicsUnit.Pixel)

        ' Calculate the playhead's position relative to the start of the view,
        ' then scale it by the zoom factor to find its screen position.
        Using playheadPen As New Pen(Color.Red, 2)
            e.Graphics.DrawLine(playheadPen, playheadPositionInPanel, 0, playheadPositionInPanel, Panel1.Height)
        End Using
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Panel1.Invalidate()
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        JoystickWorker.CancelAsync()
        Timer1.Enabled = False
        AP.StopPlayback()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        JoystickWorker.CancelAsync()
        lbMHN.Text = $"{IO.Path.Combine(tempPath, baseName)}_mouth_head_neck.csv"
        SaveData(lbMHN.Text)
    End Sub
End Class