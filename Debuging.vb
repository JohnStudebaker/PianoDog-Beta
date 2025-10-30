Imports System.Reflection.Emit

Public Class Debuging
    Private _AP As New AudioPlayer
    Private _CurMillis As Long = 0
    Public _theSong As String
    Private _curIndex As Integer = 0
    Private _last As New theData
    Private _this As New theData
    Private _next As New theData
    Private _songData As New List(Of theData)
    Private Class theData
        Public Millis As Long
        Public Mouth As Integer
        Public Neck As Integer
        Public Head As Integer
        Public LeftH As Integer
        Public LeftV As Integer
        Public RightH As Integer
        Public RightV As Integer
        Public Sub New()
            Millis = 0
            Mouth = 0
            Neck = 0
            Head = 0
            LeftH = 0
            LeftV = 0
            RightH = 0
            RightV = 0
        End Sub
    End Class
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        _CurMillis = _AP.getMillies
        If _CurMillis >= _next.Millis AndAlso _songData.Count > _curIndex + 1 Then
            _curIndex += 1
            _last = _this
            _this = _next
            _next = New theData With {.Millis = _this.Millis, .Mouth = _this.Mouth, .Head = _this.Head, .Neck = _this.Neck, .LeftH = _this.LeftH, .LeftV = _this.LeftV, .RightH = _this.RightH, .RightV = _this.RightV}
            With _next
                .Millis = _songData(_curIndex).Millis
                If _songData(_curIndex).Mouth <> 0 Then
                    .Mouth = _songData(_curIndex).Mouth
                End If
                If _songData(_curIndex).Head <> 0 Then
                    .Head = _songData(_curIndex).Head
                End If
                If _songData(_curIndex).Neck <> 0 Then
                    .Neck = _songData(_curIndex).Neck
                End If
                If _songData(_curIndex).LeftH <> 0 Then
                    .LeftH = _songData(_curIndex).LeftH
                End If
                If _songData(_curIndex).LeftV <> 0 Then
                    .LeftV = _songData(_curIndex).LeftV
                End If
                If _songData(_curIndex).RightH <> 0 Then
                    .RightH = _songData(_curIndex).RightH
                End If
                If _songData(_curIndex).RightV <> 0 Then
                    .RightV = _songData(_curIndex).RightV
                End If
            End With
            Panel1.Invalidate()
        End If
        lbMillis.Invalidate()
        Timer1.Enabled = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim s As String
        Dim pd As PianoDog
        lbMillis.Text = "Loading Song..."
        pd = New PianoDog()
        pd.LoadSong(_theSong)
        Dim syncVal As Long = pd.TimeSync(GetLocalIPv4) '+ 125
        If syncVal = -1 Then
            MessageBox.Show("Time Sync Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lbMillis.Text = "Time Sync Failed"
            Return
        End If
        s = pd.PublishSong()
        If Not s.StartsWith("OK") Then
            MessageBox.Show(s, "Error Loading Song", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lbMillis.Text = "Error Loading Song"
            Return
        End If
        If False Then
            Using txin As IO.StreamReader = New IO.StreamReader($"{_theSong}_pianodog_out.csv")
                While Not txin.EndOfStream
                    Dim line As String = txin.ReadLine()
                    Dim parts() As String = line.Split(","c)
                    Dim td As New theData
                    td.Millis = Long.Parse(parts(1))
                    td.Mouth = Integer.Parse(parts(2))
                    td.Neck = Integer.Parse(parts(3))
                    td.Head = Integer.Parse(parts(4))
                    td.LeftH = Integer.Parse(parts(5))
                    td.LeftV = Integer.Parse(parts(6))
                    td.RightH = Integer.Parse(parts(7))
                    td.RightV = Integer.Parse(parts(8))
                    _songData.Add(td)
                End While
            End Using
        End If
        _curIndex = 0
        Dim showTime As DateTime = DateTime.UtcNow.AddSeconds(5)
        Debug.Print("Show Time: " & showTime.ToString("yyyy-MM-dd HH:mm:ss"))
        s = pd.StartSong(showTime.AddMilliseconds(-500))
        If Not s.StartsWith("OK") Then
            MessageBox.Show(s, "Error Starting Song", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lbMillis.Text = "Error Starting Song"
            Return
        End If
        _AP.LoadSong($"{_theSong}.wav")
        _AP.Rewind()
        While DateTime.UtcNow < showTime
            Application.DoEvents()
            Threading.Thread.Sleep(10)
        End While
        _AP.Play()
        Timer1.Enabled = True
    End Sub

    Private Sub lbMillis_Paint(sender As Object, e As PaintEventArgs) Handles lbMillis.Paint
        lbMillis.Text = _CurMillis.ToString()
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        lastMouth.Text = _last.Mouth.ToString()
        thisMouth.Text = _this.Mouth.ToString()
        nextMouth.Text = _next.Mouth.ToString()
        lastHead.Text = _last.Head.ToString()
        thisHead.Text = _this.Head.ToString()
        nextHead.Text = _next.Head.ToString()
        lastNeck.Text = _last.Neck.ToString()
        thisNeck.Text = _this.Neck.ToString()
        nextNeck.Text = _next.Neck.ToString()
        lastLeftH.Text = _last.LeftH.ToString()
        thisLeftH.Text = _this.LeftH.ToString()
        nextLeftH.Text = _next.LeftH.ToString()
        lastLeftV.Text = _last.LeftV.ToString()
        thisLeftV.Text = _this.LeftV.ToString()
        nextLeftV.Text = _next.LeftV.ToString()
        lastRightH.Text = _last.RightH.ToString()
        thisRightH.Text = _this.RightH.ToString()
        nextRightH.Text = _next.RightH.ToString()
        lastRightV.Text = _last.RightV.ToString()
        thisRightV.Text = _this.RightV.ToString()
        nextRightV.Text = _next.RightV.ToString()
    End Sub
End Class