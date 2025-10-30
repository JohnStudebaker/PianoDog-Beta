Imports System.IO
Imports System.Reflection.Emit
Imports System.Drawing.Drawing2D
Public Class Preview2
    Private _BitmapXStart As Integer

    ' For handling mouse dragging
    Private _isDragging As Boolean = False
    Private _dragStartPoint As Point

    Public basePath As String
    Private SongData As New List(Of PlayData)

    Private _sourceBitmap As Bitmap
    Private _pctBitmap As Bitmap
    Private _BMPPerView As Integer
    Private AP As New AudioPlayer
    Private _zoomFactor As Single = 1.0F
    Private APOffset As Long = 0
    Private Class PlayData
        Public Property Millies As Long
        Public Property Mouth As Integer
        Public Property Head As Integer
        Public Property Neck As Integer
        Public Property LeftHoriz As Integer
        Public Property LeftVert As Integer
        Public Property RightHoriz As Integer
        Public Property RightVert As Integer
    End Class

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim TD As PlayData
        _sourceBitmap = CreateWaveformBitmap($"{BasePath}.wav")  '16pps should equal 62ms...
        _pctBitmap = New Bitmap(_sourceBitmap.Width, _sourceBitmap.Height)
        Dim xCount As Integer = 0
        Dim drawSize As Integer = Math.Floor(_pctBitmap.Height / 7)
        Dim halfHeight As Integer = drawSize / 2
        Dim thisHeight As Integer
        Dim thisStart As Integer
        Dim pixelsPerSecond As Integer = 16 'make sure this matches the waveform generation rate
        Dim remember As New PlayData With {.Millies = 0, .Mouth = 0, .Head = 0, .Neck = 0, .LeftHoriz = 0, .LeftVert = 0, .RightHoriz = 0, .RightVert = 0}
        Using txIn As StreamReader = IO.File.OpenText($"{basePath}_final_Output.csv")
            Dim line() As String
            Using g As Graphics = Graphics.FromImage(_pctBitmap)
                g.SmoothingMode = SmoothingMode.AntiAlias
                g.Clear(Color.Black)
                g.DrawLine(Pens.Gray, 0, drawSize * 0, _pctBitmap.Width, drawSize * 0)
                g.DrawLine(Pens.Gray, 0, drawSize * 1, _pctBitmap.Width, drawSize * 1)
                g.DrawLine(Pens.Gray, 0, drawSize * 2, _pctBitmap.Width, drawSize * 2)
                g.DrawLine(Pens.Gray, 0, drawSize * 3, _pctBitmap.Width, drawSize * 3)
                g.DrawLine(Pens.Gray, 0, drawSize * 4, _pctBitmap.Width, drawSize * 4)
                g.DrawLine(Pens.Gray, 0, drawSize * 5, _pctBitmap.Width, drawSize * 5)
                g.DrawLine(Pens.Gray, 0, drawSize * 6, _pctBitmap.Width, drawSize * 6)
                For xCount = 0 To _pctBitmap.Width - 1
                    If remember.Millies < (xCount * 1000 / pixelsPerSecond) Then
                        If Not txIn.EndOfStream Then
                            line = txIn.ReadLine().Split(","c)
                            If Long.TryParse(line(0), Nothing) Then
                                TD = New PlayData
                                With TD
                                    .Millies = CInt(line(0))
                                    .Mouth = CInt(line(1) / 10) + 1
                                    .Head = CInt(pbHead.Height / 100.0 * CInt(line(2)))
                                    .Neck = CInt(pbHead.Width / 100.0 * CInt(line(3)))
                                    .LeftHoriz = CInt(pbLeft.Width / 100.0 * CInt(line(4)))
                                    .LeftVert = CInt(line(5) / 10) + 1
                                    .RightHoriz = CInt(pbRight.Width / 100.0 * CInt(line(6)))
                                    .RightVert = CInt(line(7) / 10) + 1
                                End With
                                SongData.Add(TD)
                                With remember
                                    .Millies = CInt(line(0))
                                    If CInt(line(1)) <> 0 Then
                                        .Mouth = Math.Max(1, CInt(drawSize * 0.9 * line(1) / 100) \ 2)
                                    End If
                                    If CInt(line(2)) <> 0 Then
                                        .Head = Math.Max(1, CInt(drawSize * 0.9 * line(2) / 100) \ 2)
                                    End If
                                    If CInt(line(3)) <> 0 Then
                                        .Neck = Math.Max(1, CInt(drawSize * 0.9 * line(3) / 100) \ 2)
                                    End If
                                    If CInt(line(4)) <> 0 Then
                                        .LeftHoriz = Math.Max(1, CInt(drawSize * 0.9 * line(4) / 100) \ 2)
                                    End If
                                    If CInt(line(5)) <> 0 Then
                                        .LeftVert = Math.Max(1, CInt(drawSize * 0.9 * line(5) / 100) \ 2)
                                    End If
                                    If CInt(line(6)) <> 0 Then
                                        .RightHoriz = Math.Max(1, CInt(drawSize * 0.9 * line(6) / 100) \ 2)
                                    End If
                                    If CInt(line(7)) <> 0 Then
                                        .RightVert = Math.Max(1, CInt(drawSize * 0.9 * line(7) / 100) \ 2)
                                    End If
                                End With
                            End If
                        End If
                    End If
                    ' Draw to pct bitmap
                    ' Mouth
                    '   Debug.Assert(remember.Mouth = 0)
                    g.DrawLine(Pens.Coral, xCount, CInt((drawSize * 0) + halfHeight - remember.Mouth), xCount, CInt((drawSize * 0) + halfHeight + remember.Mouth))
                    ' Head
                    g.DrawLine(Pens.Magenta, xCount, CInt((drawSize * 1) + halfHeight - remember.Head), xCount, CInt((drawSize * 1) + halfHeight + remember.Head))
                    ' Neck
                    g.DrawLine(Pens.DarkMagenta, xCount, CInt((drawSize * 2) + halfHeight - remember.Neck), xCount, CInt((drawSize * 2) + halfHeight + remember.Neck))
                    ' Left Horiz
                    g.DrawLine(Pens.LightBlue, xCount, CInt((drawSize * 3) + halfHeight - remember.LeftHoriz), xCount, CInt((drawSize * 3) + halfHeight + remember.LeftHoriz))
                    ' Left Vert
                    g.DrawLine(Pens.LightBlue, xCount, CInt((drawSize * 4) + halfHeight - remember.LeftVert), xCount, CInt((drawSize * 4) + halfHeight + remember.LeftVert))
                    ' Right Horiz
                    g.DrawLine(Pens.Red, xCount, CInt((drawSize * 5) + halfHeight - remember.RightHoriz), xCount, CInt((drawSize * 5) + halfHeight + remember.RightHoriz))
                    ' Right Vert
                    g.DrawLine(Pens.Red, xCount, CInt((drawSize * 6) + halfHeight - remember.RightVert), xCount, CInt((drawSize * 6) + halfHeight + remember.RightVert))
                Next
            End Using
        End Using

        Dim curVals As New PlayData With {.Millies = 0, .Mouth = 0, .Head = 0, .Neck = 0, .LeftHoriz = 0, .LeftVert = 0, .RightHoriz = 0, .RightVert = 0}
        _BMPPerView = _sourceBitmap.Width / Panel1.Width

        AP.LoadSong($"{BasePath}.wav")
        AP.Rewind()
        AP.Seekms(APOffset)
        AP.Play()

        ' startMillis = Environment.TickCount
        Dim curMillis As Integer

        Dim curData As Integer = 0

        While curData < SongData.Count - 1
            curMillis = AP.getMillies
            Dim playheadPositionInBitmap As Single = CSng(curMillis / AP.TotalMillis) * _sourceBitmap.Width

            Dim sourceViewWidth As Single = Panel1.Width / _zoomFactor

            ' 2. Calculate the ideal start to center the playhead within the *zoomed view*.
            Dim idealSourceX As Single = playheadPositionInBitmap - (sourceViewWidth / 2.0F)

            ' 3. Clamp the value to the edges of the full bitmap.
            _BitmapXStart = Math.Max(0, idealSourceX)
            ' The maximum starting point is the total width minus one "view's" width.
            _BitmapXStart = Math.Min(_BitmapXStart, _sourceBitmap.Width - sourceViewWidth)

            ' Update current values if we've reached the next timestamp
            If curMillis >= SongData(curData).Millies Then
                ' carry forward non-zero values
                With SongData(curData)
                    If .Head <> 0 Then curVals.Head = .Head
                    If .Mouth <> 0 Then curVals.Mouth = .Mouth
                    If .Neck <> 0 Then curVals.Neck = .Neck
                    If .LeftHoriz <> 0 Then curVals.LeftHoriz = .LeftHoriz
                    If .LeftVert <> 0 Then curVals.LeftVert = .LeftVert
                    If .RightHoriz <> 0 Then curVals.RightHoriz = .RightHoriz
                    If .RightVert <> 0 Then curVals.RightVert = .RightVert
                End With

                ' HEAD/NECK picture
                If pbHead.Width > 0 AndAlso pbHead.Height > 0 Then
                    Dim bmpHead As New Bitmap(pbHead.Width, pbHead.Height)
                    Using g As Graphics = Graphics.FromImage(bmpHead)
                        g.Clear(Color.White)
                        Using p As Pen = New Pen(Color.Magenta, curVals.Mouth)
                            g.DrawLine(p, 0, curVals.Head, pbHead.Width, curVals.Head)
                        End Using
                        Using p As Pen = New Pen(Color.DarkMagenta, curVals.Mouth)
                            g.DrawLine(p, curVals.Neck, 0, curVals.Neck, pbHead.Height)
                        End Using
                    End Using
                    Dim oldHead = pbHead.Image
                    pbHead.Image = bmpHead
                    oldHead?.Dispose()
                End If

                ' LEFT picture
                If pbLeft.Width > 0 AndAlso pbLeft.Height > 0 Then
                    Dim bmpLeft As New Bitmap(pbLeft.Width, pbLeft.Height)
                    Using g As Graphics = Graphics.FromImage(bmpLeft)
                        g.Clear(Color.White)
                        Using p As Pen = New Pen(Color.LightBlue, curVals.LeftVert)
                            g.DrawLine(p, curVals.LeftHoriz, 0, curVals.LeftHoriz, pbLeft.Height)
                        End Using
                    End Using
                    Dim oldLeft = pbLeft.Image
                    pbLeft.Image = bmpLeft
                    oldLeft?.Dispose()
                End If

                ' RIGHT picture
                If pbRight.Width > 0 AndAlso pbRight.Height > 0 Then
                    Dim bmpRight As New Bitmap(pbRight.Width, pbRight.Height)
                    Using g As Graphics = Graphics.FromImage(bmpRight)
                        g.Clear(Color.White)
                        Using p As Pen = New Pen(Color.Red, curVals.RightVert)
                            g.DrawLine(p, curVals.RightHoriz, 0, curVals.RightHoriz, pbRight.Height)
                        End Using
                    End Using
                    Dim oldRight = pbRight.Image
                    pbRight.Image = bmpRight
                    oldRight?.Dispose()
                End If
                curData += 1
            End If
            ' Refresh the panel to show the updated waveform position
            Panel1.Invalidate()
            Panel2.Invalidate()
            pbHead.Invalidate()
            pbLeft.Invalidate()
            pbRight.Invalidate()
            ' ~60 FPS is plenty for preview
            Await Task.Delay(5)
        End While
        While AP.getMillies < AP.TotalMillis
            Await Task.Delay(5)
        End While
        AP.StopPlayback()
    End Sub

    Private Sub Preview2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Panel1.GetType().GetProperty("DoubleBuffered",
            Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic).SetValue(Panel1, True)
        Me.Panel2.GetType().GetProperty("DoubleBuffered",
            Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic).SetValue(Panel2, True)
        _BitmapXStart = 0
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint
        If _sourceBitmap Is Nothing OrElse AP.TotalMillis = 0 Then
            e.Graphics.Clear(BackColor)
            Return
        End If
        e.Graphics.InterpolationMode = Drawing.Drawing2D.InterpolationMode.NearestNeighbor
        ' The destination is always the entire panel.
        Dim destRect As New Rectangle(0, 0, Panel1.Width, Panel1.Height)

        ' The source rectangle's width is now controlled by the zoom factor.
        Dim sourceViewWidth As Single = Panel1.Width / _zoomFactor
        Dim sourceRect As New RectangleF(_BitmapXStart, 0, sourceViewWidth, _sourceBitmap.Height)

        ' Draw the (potentially zoomed) part of the bitmap onto the panel.
        e.Graphics.DrawImage(_sourceBitmap, destRect, sourceRect, GraphicsUnit.Pixel)

        ' --- CORRECTED PLAYHEAD LOGIC ---
        Dim curMillis = AP.getMillies
        Dim playheadPositionInBitmap As Single = CSng(curMillis / AP.TotalMillis) * _sourceBitmap.Width

        ' Calculate the playhead's position relative to the start of the view,
        ' then scale it by the zoom factor to find its screen position.
        Dim playheadPanelX As Single = (playheadPositionInBitmap - _BitmapXStart) * _zoomFactor

        Using playheadPen As New Pen(Color.Red, 2)
            e.Graphics.DrawLine(playheadPen, playheadPanelX, 0, playheadPanelX, Panel1.Height)
        End Using
    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

        If _pctBitmap Is Nothing OrElse AP.TotalMillis = 0 Then
            e.Graphics.Clear(BackColor)
            Return
        End If
        e.Graphics.InterpolationMode = Drawing.Drawing2D.InterpolationMode.NearestNeighbor
        If _pctBitmap Is Nothing OrElse AP.TotalMillis = 0 Then Return

        Dim destRect As New Rectangle(0, 0, Panel2.Width, Panel2.Height)

        ' Use the same zoom factor for a consistent view.
        Dim sourceViewWidth As Single = Panel2.Width / _zoomFactor
        Dim sourceRect As New RectangleF(_BitmapXStart, 0, sourceViewWidth, _pctBitmap.Height)

        e.Graphics.DrawImage(_pctBitmap, destRect, sourceRect, GraphicsUnit.Pixel)

        Dim curMillis = AP.getMillies
        Dim playheadPositionInBitmap As Single = CSng(curMillis / AP.TotalMillis) * _pctBitmap.Width
        Dim playheadPanelX As Single = (playheadPositionInBitmap - _BitmapXStart) * _zoomFactor

        Using playheadPen As New Pen(Color.Red, 2)
            e.Graphics.DrawLine(playheadPen, playheadPanelX, 0, playheadPanelX, Panel2.Height)
        End Using

    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        _zoomFactor = NumericUpDown1.Value / 100.0F
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        AP.PlayPause()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        AP.BackUp(5)
    End Sub

    Private Class WarpData
        Public Millis As Long
        Public whichServo As Integer
        Public whatValue As Integer
    End Class

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim startTime As Long = AP.getMillies
        Dim changeTime As Long
        Dim newFile As String
        Dim l As New List(Of WarpData)
        Dim line() As String
        Dim WD As WarpData
        Using tw As New TimeWarp
            tw.ShowDialog(Me)
            changeTime = tw.theMillis
            If tw.theAction <> "Cancel" Then
                AP.StopPlayback()
                APOffset = startTime
                newFile = $"{BasePath}_final_output_{Format(Date.Now, "yyyyMMddHHmmssfff")}.csv"
                IO.File.Move($"{BasePath}_final_output.csv", newFile, True)
                Using txIn As StreamReader = IO.File.OpenText(newFile)
                    While Not txIn.EndOfStream
                        line = txIn.ReadLine().Split(","c)
                        If Not Integer.TryParse(line(0), Nothing) Then Continue While
                        WD = New WarpData
                        With WD
                            .Millis = CInt(line(0))
                            .whichServo = 1
                            .whatValue = CInt(line(1))
                        End With
                        If CInt(line(0)) >= startTime AndAlso tw.theMouth Then
                            WD.Millis += changeTime
                        End If
                        l.Add(WD)
                        WD = New WarpData
                        With WD
                            .Millis = CInt(line(0))
                            .whichServo = 2
                            .whatValue = CInt(line(2))
                        End With
                        If CInt(line(0)) >= startTime AndAlso tw.theHead Then
                            WD.Millis += changeTime
                        End If
                        l.Add(WD)
                        WD = New WarpData
                        With WD
                            .Millis = CInt(line(0))
                            .whichServo = 3
                            .whatValue = CInt(line(3))
                        End With
                        If CInt(line(0)) >= startTime AndAlso tw.theNeck Then
                            WD.Millis += changeTime
                        End If
                        l.Add(WD)
                        WD = New WarpData
                        With WD
                            .Millis = CInt(line(0))
                            .whichServo = 4
                            .whatValue = CInt(line(4))
                        End With
                        If CInt(line(0)) >= startTime AndAlso tw.theLeftH Then
                            WD.Millis += changeTime
                        End If
                        l.Add(WD)
                        WD = New WarpData
                        With WD
                            .Millis = CInt(line(0))
                            .whichServo = 5
                            .whatValue = CInt(line(5))
                        End With
                        If CInt(line(0)) >= startTime AndAlso tw.theLeftV Then
                            WD.Millis += changeTime
                        End If
                        l.Add(WD)
                        WD = New WarpData
                        With WD
                            .Millis = CInt(line(0))
                            .whichServo = 6
                            .whatValue = CInt(line(6))
                        End With
                        If CInt(line(0)) >= startTime AndAlso tw.theRightH Then
                            WD.Millis += changeTime
                        End If
                        l.Add(WD)
                        WD = New WarpData
                        With WD
                            .Millis = CInt(line(0))
                            .whichServo = 7
                            .whatValue = CInt(line(7))
                        End With
                        If CInt(line(0)) >= startTime AndAlso tw.theRightV Then
                            WD.Millis += changeTime
                        End If
                        l.Add(WD)
                    End While
                End Using
                l.Sort(Function(a, b)
                           If a.Millis = b.Millis Then
                               Return a.whichServo.CompareTo(b.whichServo)
                           Else
                               Return a.Millis.CompareTo(b.Millis)
                           End If
                       End Function)
                Dim aSave() As SaveClass
                ReDim aSave(l.Count) ' way more than we need
                For i As Integer = 0 To aSave.Length - 1
                    aSave(i) = New SaveClass
                Next
                Dim curIndex As Integer
                For Each WD In l
                    curIndex = Array.FindIndex(Of SaveClass)(aSave, Function(s) s.Time = WD.Millis)
                    If curIndex = -1 Then
                        ' New time entry
                        curIndex = Array.FindIndex(Of SaveClass)(aSave, Function(s) s.Time = 0)
                        aSave(curIndex).Time = WD.Millis
                    End If
                    Select Case WD.whichServo
                        Case 1
                            aSave(curIndex).Mouth = WD.whatValue
                        Case 2
                            aSave(curIndex).Head = WD.whatValue
                        Case 3
                            aSave(curIndex).Neck = WD.whatValue
                        Case 4
                            aSave(curIndex).LeftHorz = WD.whatValue
                        Case 5
                            aSave(curIndex).LeftVert = WD.whatValue
                        Case 6
                            aSave(curIndex).RightHorz = WD.whatValue
                        Case 7
                            aSave(curIndex).RightVert = WD.whatValue
                    End Select
                Next

                Using txOut As StreamWriter = IO.File.CreateText($"{BasePath}_final_output.csv")
                    For curIndex = 0 To aSave.Length - 1
                        If aSave(curIndex).Time > 0 Then
                            txOut.WriteLine($"{aSave(curIndex).Time},{aSave(curIndex).Mouth},{aSave(curIndex).Head},{aSave(curIndex).Neck},{aSave(curIndex).LeftHorz},{aSave(curIndex).LeftVert},{aSave(curIndex).RightHorz},{aSave(curIndex).RightVert}")
                        End If
                    Next
                End Using
                Button1_Click(Me, EventArgs.Empty)

            End If
        End Using
    End Sub
End Class