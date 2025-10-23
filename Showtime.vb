Public Class Showtime
    Private BasePath As String
    Private Sub Showtime_EnabledChanged(sender As Object, e As EventArgs) Handles Me.EnabledChanged

    End Sub

    Private Sub Showtime_ImeModeChanged(sender As Object, e As EventArgs) Handles Me.ImeModeChanged

    End Sub

    Private Sub Showtime_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.WindowState = FormWindowState.Maximized
        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        PictureBox1.Width = Me.Width - 20
        PictureBox1.Height = Me.Height - 40
        Timer1.Enabled = True
    End Sub
    Public Sub SetBasePath(path As String)
        BasePath = path
        If IO.File.Exists($"{BasePath}.jpg") Then
            PictureBox1.Image = Image.FromFile($"{BasePath}.jpg")
        Else
            PictureBox1.Image = Nothing
        End If
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        PictureBox1.Visible = False
        Label1.Visible = True
        Dim pd As PianoDog
        Label1.Text = "Loading Song..."
        pd = New PianoDog()
        pd.LoadSong(BasePath)
        pd.PublishSong(Label1)
        Dim syncVal As Long = pd.TimeSync(GetLocalIPv4) '+ 125
        Dim showTime As DateTime = DateTime.UtcNow.AddSeconds(10)
        Debug.Print("Show Time: " & showTime.ToString("yyyy-MM-dd HH:mm:ss"))
        Label1.Text = "Starting in 10 Seconds..."
        Label1.Visible = False
        PictureBox1.Visible = True
        pd.StartSong(showTime.AddMilliseconds(syncVal))
        Dim ap As New AudioPlayer
        ap.LoadSong($"{BasePath}.wav")
            AP.Rewind()
            While DateTime.UtcNow < showTime.AddMilliseconds(syncVal)
                Application.DoEvents()
                Threading.Thread.Sleep(10)
            End While
            AP.Play()
            While AP.getMillies < AP.TotalMillis
                Application.DoEvents()
                Threading.Thread.Sleep(100)
            End While
            AP.StopPlayback()
        Me.Close()
    End Sub

    Private Sub Showtime_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        Label1.Width = (Me.Width - 20) / 2
    End Sub
End Class