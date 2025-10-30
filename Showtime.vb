Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel

Public Class Showtime
    Public WAVFile As String
    Public CSVFile As String
    Public JPGFile As String
    Private WithEvents pd As PianoDog
    Private Sub Showtime_EnabledChanged(sender As Object, e As EventArgs) Handles Me.EnabledChanged

    End Sub

    Private Sub Showtime_ImeModeChanged(sender As Object, e As EventArgs) Handles Me.ImeModeChanged

    End Sub

    Private Sub UpdateImage(ByVal thePercent As Integer) Handles pd.UploadProgress
        UpdateProgressImage(PictureBox1, My.Resources.ComingSoonInverted1, My.Resources.ComingSoon1, thePercent)
        PictureBox1.Refresh()
        'PictureBox2.Invalidate()
    End Sub

    Private Sub Showtime_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.WindowState = FormWindowState.Maximized
        Me.Showtime_Resize(sender, e)
        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        PictureBox3.SizeMode = PictureBoxSizeMode.Zoom

        'PictureBox1.Width = Me.Width - 20
        'PictureBox1.Height = Me.Height - 40
        Timer1.Enabled = True
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False

        Dim ap As New AudioPlayer
        Dim s As String
        Threading.Thread.Sleep(500)
        Me.Invalidate()
        pd = New PianoDog()
        pd.LoadSong(CSVFile)
        ap.LoadSong($"{WAVFile}")
        ap.Rewind()
        Me.Invalidate()
        'Threading.Thread.Sleep(3000)
        'Dim syncVal As Long = pd.TimeSync(GetLocalIPv4) '+ 125
        'If syncVal = 0 Then
        '    Debug.Assert(False)
        '    Label1.Text = "Time Sync Failed. Check Network Connection."
        '    Me.Invalidate()
        '    Threading.Thread.Sleep(3000)
        '    Me.Close()
        '    Return
        'End If
        s = pd.PublishSong()
        If Not s.StartsWith("OK") Then
            Debug.Assert(False)
            Me.Invalidate()
            'Threading.Thread.Sleep(3000)
            Me.Close()
            Return
        End If
        Me.Invalidate()
        If IO.File.Exists($"{JPGFile}") Then
            PictureBox1.Image = Image.FromFile($"{JPGFile}")
        Else
            PictureBox1.Image = Nothing
        End If
        Dim showTime As DateTime = DateTime.UtcNow.AddSeconds(4)
        s = pd.StartSong(showTime)
        If Not s.StartsWith("OK") Then
            Debug.Assert(False)
            'Label1.Text = s
            Me.Invalidate()
            Threading.Thread.Sleep(3000)
            Me.Close()
            Return
        End If
        Me.Invalidate()
        While DateTime.UtcNow < showTime.AddMilliseconds(MSPerSample)
            Application.DoEvents()
            Threading.Thread.Sleep(10)
        End While
        ap.Play()
        Me.Invalidate()

        While ap.getMillies < ap.TotalMillis AndAlso ((justSeconds = 0) OrElse (ap.getMillies < justSeconds * 1000))
            Application.DoEvents()
            Threading.Thread.Sleep(100)
        End While
        ap.StopPlayback()
        Me.Close()
    End Sub


    Private Sub Showtime_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        'images are 768x1024
        If Me.WindowState = FormWindowState.Maximized Then
            PictureBox1.Top = 0
            PictureBox1.Height = Me.ClientSize.Height
            PictureBox1.Width = PictureBox1.Height * 0.75
            PictureBox1.Left = (Me.ClientSize.Width - PictureBox1.Width) / 2
            Panel1.Top = 0
            Panel1.Height = Me.ClientSize.Height
            Panel1.Width = PictureBox1.Width * 0.75
            Panel1.Left = 0
            PictureBox3.Width = Panel1.Width
            PictureBox3.Left = Me.ClientSize.Width - PictureBox3.Width
            PictureBox3.Top = 0
            PictureBox3.Height = Me.ClientSize.Height
            TextBox1.Left = Panel1.Width * 0.1 / 2
            TextBox1.Width = Panel1.Width * 0.9
            TextBox1.Top = 0
            TextBox1.Height = Panel1.Height
            Label1.Left = (Panel1.Width - Label1.Width) / 2
            Label1.Top = Panel1.Height - Label1.Height - 20
        End If
    End Sub


End Class