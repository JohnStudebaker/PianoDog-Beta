Imports System.Diagnostics.Eventing
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports NAudio.Wave

Module GeneralSubroutines
    Public Const MSPerSample = 62
    Public Const AllPWMMin = 22
    Public Const AllPWMMid = 76
    Public Const AllPWMMax = 126
    Private Class inData
        Public Property Time As Long
        Public Property MouthDown As Integer
        Public Property MouthUp As Integer
        Public Property Head As Integer
        Public Property Neck As Integer
        Public Property LeftDown As Integer
        Public Property LeftUp As Integer

        Public Property RightDown As Integer
        Public Property RightUp As Integer

        Public Sub New()
            Time = 0
            MouthDown = 0
            MouthUp = 0
            Head = 0
            Neck = 0
            LeftDown = 0
            LeftUp = 0
            RightDown = 0
            RightUp = 0
        End Sub
    End Class

    Private Class OutData
        Public Property Time As Long
        Public Property MouthPercent As Integer
        Public Property HeadPercent As Integer
        Public Property NeckPercent As Integer
        Public Property LeftHorzPercent As Integer
        Public Property LeftVertPercent As Integer
        Public Property RightHorzPercent As Integer
        Public Property RightVertPercent As Integer
    End Class

    Private Class JustData4Sort
        Public Property Time As Long
        Public Property Val1 As Integer
        Public Property Val2 As Integer
        Public Property Val3 As Integer
        Public Property Val4 As Integer
        Public Property Val5 As Integer
        Public Property Val6 As Integer
        Public Property Val7 As Integer
        Public Sub New()
            Time = 0
            Val1 = 0
            Val2 = 0
            Val3 = 0
            Val4 = 0
            Val5 = 0
        End Sub

        Public Sub New(timeValue As Long)
            Time = timeValue
            Val1 = 0
            Val2 = 0
            Val3 = 0
            Val4 = 0
            Val5 = 0
        End Sub

    End Class
    Private allIn() As inData
    Private allOut() As OutData




    Public Sub ForceTimeSync()
        Try
            Dim psi As New ProcessStartInfo("w32tm", "/resync")
            psi.UseShellExecute = False
            psi.CreateNoWindow = True
            psi.RedirectStandardOutput = True
            psi.RedirectStandardError = True

            Dim p As Process = Process.Start(psi)
            Dim output As String = p.StandardOutput.ReadToEnd()
            Dim err As String = p.StandardError.ReadToEnd()
            p.WaitForExit()

            If p.ExitCode = 0 Then
                ' MessageBox.Show("Time sync successful." & vbCrLf & output)
            Else
                ' MessageBox.Show("Time sync failed." & vbCrLf & err)
            End If
        Catch ex As Exception
            'MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub


    Public Sub MakeHeadNeckPercent(NoExtFilename As String)
        Dim theData As New List(Of JustData4Sort)
        Dim Min1 As Integer = -1
        Dim Max1 As Integer = -1
        Dim Min2 As Integer = -1
        Dim Max2 As Integer = -1

        Using txIN As StreamReader = IO.File.OpenText($"{NoExtFilename}.csv")
            Dim line As String
            Dim parts() As String
            While Not txIN.EndOfStream
                line = txIN.ReadLine()
                parts = line.Split(","c)
                If parts.Length = 2 Then
                    If IsNumeric(parts(0)) AndAlso IsNumeric(parts(1)) Then
                        theData.Add(New JustData4Sort With {.Time = CLng(parts(0)), .Val1 = CInt(parts(1))})
                        If CInt(parts(1)) <> 0 AndAlso (Min1 = -1 OrElse CInt(parts(1)) < Min1) Then
                            Min1 = CInt(parts(1))
                        End If
                        If CInt(parts(1)) <> 0 AndAlso CInt(parts(1)) > Max1 Then
                            Max1 = CInt(parts(1))
                        End If
                    End If
                End If
            End While
        End Using
        theData.Sort(Function(x, y) x.Time.CompareTo(y.Time))
        Dim newData As New List(Of JustData4Sort)
        Dim lastData As New JustData4Sort With {.Time = -250}
        Dim nd As JustData4Sort
        Dim theMillis As Long = 0
        Dim dataIndex As Integer = 0
        nd = New JustData4Sort(0) 'offset by 1/8 second so hands and head/neck are opposite ticks
        Do
            If theData(dataIndex).Time <= nd.Time + MSPerSample Then
                'we have new data to add
                If theData(dataIndex).Val1 > 0 Then
                    nd.Val1 += theData(dataIndex).Val1
                    nd.Val2 += 1
                End If
                dataIndex += 1
            Else
                'time to add the new data
                newData.Add(nd)
                lastData = nd
                nd = New JustData4Sort(lastData.Time + MSPerSample) 'copies the last data, and adds 125 millis
            End If
        Loop While dataIndex < theData.Count
        'add the last one
        newData.Add(nd)
        'now newData has the data grouped by 1/4 second intervals.
        Dim runner As Integer = 0
        Dim DoneData As New List(Of JustData4Sort)
        For Each d As JustData4Sort In newData
            If d.Val2 > 0 Then
                runner = CInt(Math.Round(((((d.Val1 / d.Val2) - Min1) / (Max1 - Min1)) * 99) + 1, 0))
            End If
            DoneData.Add(New JustData4Sort With {.Time = d.Time, .Val1 = d.Val1, .Val2 = d.Val2, .Val3 = runner})
        Next
        Using txout As New StreamWriter($"{NoExtFilename}_percent.csv")
            For Each d As JustData4Sort In DoneData
                txout.WriteLine($"{d.Time},{Math.Round(d.Val3 / 10, 0) * 10}")
            Next
            txout.Flush()
        End Using
    End Sub

    Private Function FindTimeIndex(theTime As Long) As Integer
        Dim a As Integer
        a = Array.FindIndex(allIn, Function(x) x IsNot Nothing AndAlso x.Time = theTime)
        If a = -1 Then
            a = Array.FindIndex(allIn, Function(x) x Is Nothing)
            If a > -1 Then
                allIn(a) = New inData()
                allIn(a).Time = theTime
            Else
                ReDim Preserve allIn(allIn.Length + 5 * 60 * 10)
                a = Array.FindIndex(allIn, Function(x) x Is Nothing)
                allIn(a) = New inData()
                allIn(a).Time = theTime
            End If
        End If
        Return a
    End Function

    Private Sub Key2Percent(inkey As Integer, ByRef HorizPct As Integer, ByRef VertPct As Integer)
        HorizPct = 0
        VertPct = 0
        Select Case inkey
            Case ConsoleKey.D1, ConsoleKey.Q, ConsoleKey.A, ConsoleKey.Z, ConsoleKey.OemMinus, ConsoleKey.OemPeriod, ConsoleKey.Oem1, ConsoleKey.Oem2
                HorizPct = 1
            Case ConsoleKey.D2, ConsoleKey.W, ConsoleKey.S, ConsoleKey.X, ConsoleKey.D0, ConsoleKey.OemComma, ConsoleKey.L, ConsoleKey.P
                HorizPct = 25
            Case ConsoleKey.D3, ConsoleKey.E, ConsoleKey.D, ConsoleKey.C, ConsoleKey.D9, ConsoleKey.I, ConsoleKey.K, ConsoleKey.O
                HorizPct = 50
            Case ConsoleKey.D4, ConsoleKey.R, ConsoleKey.F, ConsoleKey.V, ConsoleKey.D8, ConsoleKey.U, ConsoleKey.J, ConsoleKey.M
                HorizPct = 75
            Case ConsoleKey.D5, ConsoleKey.T, ConsoleKey.G, ConsoleKey.B, ConsoleKey.D7, ConsoleKey.Y, ConsoleKey.H, ConsoleKey.N
                HorizPct = 100
        End Select
        Select Case inkey
            Case ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D4, ConsoleKey.D5, ConsoleKey.D6, ConsoleKey.D7, ConsoleKey.D8, ConsoleKey.D9, ConsoleKey.D0, ConsoleKey.OemMinus
                VertPct = 100
            Case ConsoleKey.Q, ConsoleKey.W, ConsoleKey.E, ConsoleKey.R, ConsoleKey.T, ConsoleKey.Y, ConsoleKey.U, ConsoleKey.I, ConsoleKey.O, ConsoleKey.P, ConsoleKey.Oem1
                VertPct = 75
            Case ConsoleKey.A, ConsoleKey.S, ConsoleKey.D, ConsoleKey.F, ConsoleKey.G, ConsoleKey.H, ConsoleKey.J, ConsoleKey.K, ConsoleKey.L, ConsoleKey.Oem1, ConsoleKey.Oem7
                VertPct = 50
            Case ConsoleKey.Z, ConsoleKey.X, ConsoleKey.C, ConsoleKey.V, ConsoleKey.B, ConsoleKey.N, ConsoleKey.M, ConsoleKey.OemComma, ConsoleKey.OemPeriod, ConsoleKey.Oem2
                VertPct = 25
        End Select
    End Sub
    Private Function Key2Percent(inkey As Integer) As Integer
        Dim HorizPct As Integer = 0
        'VertPct = 0
        Select Case inkey
            Case ConsoleKey.Z, ConsoleKey.Oem2, ConsoleKey.A, ConsoleKey.Oem1, ConsoleKey.Q, ConsoleKey.P
                HorizPct = 1  '00000000  -- absolute ignore
            Case ConsoleKey.X, ConsoleKey.OemPeriod, ConsoleKey.S, ConsoleKey.L, ConsoleKey.W, ConsoleKey.O
                HorizPct = 25'1  '00000001  -- pot 0%
            Case ConsoleKey.C, ConsoleKey.OemComma, ConsoleKey.D, ConsoleKey.K, ConsoleKey.E, ConsoleKey.I
                HorizPct = 50'2  '00000010  -- pot 33%
            Case ConsoleKey.V, ConsoleKey.M, ConsoleKey.F, ConsoleKey.J, ConsoleKey.R, ConsoleKey.U
                HorizPct = 75'4  '00000100  -- pot 66%
            Case ConsoleKey.B, ConsoleKey.N, ConsoleKey.G, ConsoleKey.H, ConsoleKey.T, ConsoleKey.Y
                HorizPct = 100'8  '00001000  -- pot to inside
            Case ConsoleKey.Spacebar
                'HorizPct = 16 '00010000  -- force hand up
        End Select
        Return HorizPct
    End Function

    Private Sub Key2What(inkey As Integer, ByRef HorizPct As Integer, ByRef HasUp As Boolean, ByRef HasDown As Boolean)
        HorizPct = 0
        HasUp = False
        HasDown = False
        Select Case inkey
            Case ConsoleKey.Z, ConsoleKey.Oem2, ConsoleKey.A, ConsoleKey.Oem1, ConsoleKey.Q, ConsoleKey.P
                HorizPct = 1  '00000000  -- absolute ignore
            Case ConsoleKey.X, ConsoleKey.OemPeriod, ConsoleKey.S, ConsoleKey.L, ConsoleKey.W, ConsoleKey.O
                HorizPct = 20'1  '00000001  -- pot 0%
            Case ConsoleKey.C, ConsoleKey.OemComma, ConsoleKey.D, ConsoleKey.K, ConsoleKey.E, ConsoleKey.I
                HorizPct = 50'2  '00000010  -- pot 33%
            Case ConsoleKey.V, ConsoleKey.M, ConsoleKey.F, ConsoleKey.J, ConsoleKey.R, ConsoleKey.U
                HorizPct = 80'4  '00000100  -- pot 66%
            Case ConsoleKey.B, ConsoleKey.N, ConsoleKey.G, ConsoleKey.H, ConsoleKey.T, ConsoleKey.Y
                HorizPct = 100'8  '00001000  -- pot to inside
            Case ConsoleKey.Spacebar
                'HorizPct = 16 '00010000  -- force hand up
        End Select
        Select Case inkey
            Case ConsoleKey.Q, ConsoleKey.W, ConsoleKey.E, ConsoleKey.R, ConsoleKey.T, ConsoleKey.Y, ConsoleKey.U, ConsoleKey.I, ConsoleKey.O, ConsoleKey.P
                HasUp = True
            Case ConsoleKey.A, ConsoleKey.S, ConsoleKey.D, ConsoleKey.F, ConsoleKey.G, ConsoleKey.H, ConsoleKey.J, ConsoleKey.K, ConsoleKey.L, ConsoleKey.Oem1, ConsoleKey.Oem7
                HasUp = True
                HasDown = True
            Case ConsoleKey.Z, ConsoleKey.X, ConsoleKey.C, ConsoleKey.V, ConsoleKey.B, ConsoleKey.N, ConsoleKey.M, ConsoleKey.OemComma, ConsoleKey.OemPeriod, ConsoleKey.Oem2
                HasDown = True
        End Select
    End Sub
    Public Sub MakeKeysPercent3(baseFilename As String)
        Dim theData As New List(Of JustData4Sort)
        Dim CurPos As Integer = 0
        '        Dim CurDown As Integer = 0
        Dim thisKey As Integer
        Dim lastKey As Integer = 0
        Dim HorizDown As Integer
        Dim HorizUp As Integer
        Dim DownHasDown As Boolean
        Dim DownHasUp As Boolean
        Dim UpHasDown As Boolean
        Dim UpHasUp As Boolean
        Using txIN As StreamReader = IO.File.OpenText($"{baseFilename}.csv")
            Dim line As String
            Dim parts() As String
            While Not txIN.EndOfStream
                line = txIN.ReadLine()
                parts = line.Split(","c)
                HorizUp = 0
                HorizDown = 0
                DownHasDown = False
                DownHasUp = False
                UpHasDown = False
                UpHasUp = False
                If parts.Length = 3 Then
                    If IsNumeric(parts(0)) Then
                        If IsNumeric(parts(1)) AndAlso CInt(parts(1)) > 0 Then
                            Key2What(CInt(parts(1)), HorizDown, DownHasUp, DownHasDown)
                        End If
                        If IsNumeric(parts(2)) AndAlso CInt(parts(2)) > 0 Then
                            Key2What(CInt(parts(2)), HorizUp, UpHasUp, UpHasDown)
                        End If
                        If DownHasDown Then
                            theData.Add(New JustData4Sort With {.Time = CLng(parts(0)), .Val1 = HorizDown, .Val2 = 100})
                        End If
                        If UpHasUp Then
                            theData.Add(New JustData4Sort With {.Time = CLng(parts(0)), .Val1 = HorizUp, .Val2 = 1})
                        End If
                    End If
                End If
            End While
        End Using
        theData.Sort(Function(x, y) x.Time.CompareTo(y.Time))
        Using txout As New StreamWriter($"{baseFilename}_test1.csv")
            For Each d As JustData4Sort In theData
                txout.WriteLine($"{d.Time},{d.Val1},{d.Val2}")
            Next
            txout.Flush()
        End Using
        Dim newData As New List(Of JustData4Sort)
        Dim nd As JustData4Sort
        Dim theMillis As Long = 0
        Dim dataIndex As Integer = 0
        nd = New JustData4Sort(0)
        Dim lastHoriz As Integer = 0
        Dim lastVert As Integer = 0
        Dim hasDown As Boolean = False
        Dim hasUp As Boolean = False
        Dim CurVert As Integer = 0
        thisKey = 0
        lastKey = 0
        Dim trythis() As JustData4Sort
        ReDim trythis((theData.Last.Time \ MSPerSample) + 1)
        For i As Integer = 0 To trythis.Length - 1
            trythis(i) = New JustData4Sort
            trythis(i).Time = i * MSPerSample
            trythis(i).Val1 = 0
            trythis(i).Val2 = 0
            trythis(i).Val3 = 0
            trythis(i).Val4 = 0
        Next
        Dim thisOne As Integer
        For Each d As JustData4Sort In theData
            thisOne = Math.Round(d.Time / MSPerSample)
            If d.Val1 > 0 Then
                trythis(thisOne).Val1 += d.Val1
            End If
            If d.Val2 = 1 Then
                trythis(thisOne).Val2 += 1
            End If
            If d.Val2 = 100 Then
                trythis(thisOne).Val3 += 1
            End If
            trythis(thisOne).Val4 += 1
        Next
        Using txout As New StreamWriter($"{baseFilename}_test2.csv")
            For Each d As JustData4Sort In trythis
                txout.WriteLine($"{d.Time},{d.Val1},{d.Val2},{d.Val3},{d.Val4}")
            Next
            txout.Flush()
        End Using
        Dim TryThis2() As JustData4Sort
        ReDim TryThis2(trythis.Length - 1)
        For i As Integer = 0 To trythis.Length - 1
            TryThis2(i) = New JustData4Sort
            TryThis2(i).Time = trythis(i).Time
            If trythis(i).Val4 > 0 Then
                TryThis2(i).Val1 = CInt(Math.Round(trythis(i).Val1 / trythis(i).Val4, 0))
                If trythis(i).Val2 > trythis(i).Val3 Then
                    TryThis2(i).Val2 = 1
                ElseIf trythis(i).Val3 > trythis(i).Val2 Then
                    TryThis2(i).Val2 = 100
                Else
                    TryThis2(i).Val2 = 0
                End If
            End If
        Next
        Using txout As New StreamWriter($"{baseFilename}_percent.csv")
            Dim lastP As Integer = 0
            Dim thisP As Integer
            Dim lastp2 As Integer = 0
            Dim thisp2 As Integer
            For Each d As JustData4Sort In TryThis2
                thisP = Math.Round(d.Val1 / 10, 0) * 10
                thisp2 = d.Val2
                txout.WriteLine($"{d.Time},{thisP},{thisp2}")
            Next
            txout.Flush()
        End Using

#If False Then

        Do
            If theData(dataIndex).Time <= nd.Time + 125 Then  'try 1/4 for the keyboard
                If theData(dataIndex).Val1 > 0 Then
                    thisKey = theData(dataIndex).Val1
                End If
                If theData(dataIndex).Val2 > 0 Then
                    Select Case theData(dataIndex).Val2
                        Case 1
                            'up
                            hasUp = True
                        Case 100
                            hasDown = True
                    End Select
                End If
                dataIndex += 1
            Else
                'time to add the new data
                If hasUp Then
                    If hasDown Then
                        If CurVert = 1 Then
                            CurVert = 100
                        Else
                            CurVert = 1
                        End If
                    Else
                        CurVert = 1
                    End If
                ElseIf hasDown Then
                    CurVert = 100
                End If
                newData.Add(New JustData4Sort With {.Time = nd.Time, .Val1 = thisKey, .Val2 = CurVert})
                'lastData = nd
                nd = New JustData4Sort(nd.Time + 125)
                hasDown = False
                hasUp = False
                thisKey = 0
                hasDown = False
                hasUp = False
                'lastHoriz = theData(dataIndex).Val1
            End If
        Loop While dataIndex < theData.Count
        'add the last one
        If hasUp Then
            If hasDown Then
                If CurVert = 1 Then
                    CurVert = 100
                Else
                    CurVert = 1
                End If
            Else
                CurVert = 1
            End If
        ElseIf hasDown Then
            CurVert = 100
        End If
        newData.Add(New JustData4Sort With {.Time = nd.Time, .Val1 = thisKey, .Val2 = CurVert})
        'now smooth out the data in val1
        Using txout As New StreamWriter($"{baseFilename}_test2.csv")
            For Each d As JustData4Sort In newData
                txout.WriteLine($"{d.Time},{d.Val1},{d.Val2}")
            Next
            txout.Flush()
        End Using



        Dim firstZero As Integer = -1
        Dim lastZero As Integer = -1
        If True Then
            For i = 1 To newData.Count - 2
                If newData(i).Val1 = 0 Then
                    If firstZero = -1 Then
                        firstZero = i
                    End If
                    lastZero = i
                Else
                    If firstZero > 1 Then
                        If lastZero <> -1 Then
                            'we have a block to fill in
                            Dim startVal As Integer = newData(firstZero - 1).Val1
                            Dim endVal As Integer = newData(lastZero + 1).Val1
                            Dim valRange As Integer = endVal - startVal
                            Dim indexRange As Integer = (lastZero + 1) - (firstZero - 1)
                            For j As Integer = firstZero To lastZero
                                Dim posInBlock As Integer = j - (firstZero - 1)
                                Dim stepVal As Double = (valRange / indexRange) * posInBlock
                                newData(j).Val1 = CInt(startVal + stepVal)
                            Next
                            firstZero = -1
                            lastZero = -1
                        End If
                    ElseIf firstZero > -1 Then
                        i = lastZero
                        firstZero = -1
                        lastZero = -1
                    End If
                End If
            Next
        End If
        'now newData has the data grouped by 1/4 second intervals.
        Using txout As New StreamWriter($"{baseFilename}_percent.csv")
            Dim lastP As Integer = 0
            Dim thisP As Integer
            Dim lastp2 As Integer = 0
            Dim thisp2 As Integer
            For Each d As JustData4Sort In newData
                thisP = Math.Round(d.Val1 / 10, 0) * 10
                thisp2 = d.Val2
                txout.WriteLine($"{d.Time},{thisP},{thisp2}")
            Next
            txout.Flush()
        End Using



        'Dim runner As Integer = 0
        'Dim vaulter As Integer = 0
        ''Dim runner2 As Integer = 0
        'Dim DoneData As New List(Of JustData4Sort)
        'For Each d As JustData4Sort In newData
        '    If d.Val1 And 16 Then
        '        vaulter = 100
        '    End If
        '    runner = runner Xor d.Val1


        '    If d.Val3 > 0 Then
        '        runner = d.Val1 / d.Val3
        '    End If
        '    DoneData.Add(New JustData4Sort With {.Time = d.Time, .Val1 = d.Val1, .Val2 = d.Val3, .Val3 = runner, .Val4 = 0, .Val5 = d.Val5})
        'Next
        'Using txout As New StreamWriter(baseFilename & "_test1.csv")
        '    For Each d As JustData4Sort In DoneData
        '        txout.WriteLine($"{d.Time},{d.Val1},{d.Val2},{d.Val3},{d.Val4},{d.Val5}")
        '    Next
        '    txout.Flush()
        'End Using
#End If
    End Sub

    Private Class TempData
        Public Property Time As Long
        Public Property MouthPercent As Integer
        Public Property HeadPercent As Integer
        Public Property NeckPercent As Integer
        Public Property LeftHorzPercent As Integer
        Public Property LeftVertPercent As Integer
        Public Property RightHorzPercent As Integer
        Public Property RightVertPercent As Integer

        Public Sub New()
            Time = 0
            MouthPercent = 0
            HeadPercent = 0
            NeckPercent = 0
            LeftHorzPercent = 0
            LeftVertPercent = 0
            RightHorzPercent = 0
            RightVertPercent = 0
        End Sub
    End Class
    Public Sub MakeMouthPercent(baseFilename As String)
        Dim theData As New List(Of JustData4Sort)
        Dim Min1 As Integer = -1
        Dim Max1 As Integer = -1
        Dim Min2 As Integer = -1
        Dim Max2 As Integer = -1

        Using txIN As StreamReader = IO.File.OpenText($"{baseFilename}_Mouth.csv")
            Dim line As String
            Dim parts() As String
            While Not txIN.EndOfStream
                line = txIN.ReadLine()
                parts = line.Split(","c)
                If parts.Length = 3 Then
                    If IsNumeric(parts(0)) AndAlso IsNumeric(parts(1)) AndAlso IsNumeric(parts(2)) Then
                        theData.Add(New JustData4Sort With {.Time = CLng(parts(0)), .Val1 = CInt(parts(1)), .Val2 = CInt(parts(2))})
                        If CInt(parts(1)) <> 0 AndAlso (Min1 = -1 OrElse CInt(parts(1)) < Min1) Then
                            Min1 = CInt(parts(1))
                        End If
                        If CInt(parts(1)) <> 0 AndAlso CInt(parts(1)) > Max1 Then
                            Max1 = CInt(parts(1))
                        End If
                    End If
                End If
            End While
        End Using
        theData.Sort(Function(x, y) x.Time.CompareTo(y.Time))
        Dim newData As New List(Of JustData4Sort)
        Dim lastdata As New JustData4Sort With {.Time = -MSPerSample}
        Dim nd As JustData4Sort
        Dim theMillis As Long = 0
        Dim dataIndex As Integer = 0
        nd = New JustData4Sort(0) 'copies the last data, and adds 250 millis (1/4 second)
        Do
            If theData(dataIndex).Time <= nd.Time + MSPerSample Then
                'we have new data to add
                If theData(dataIndex).Val1 > 0 Then
                    nd.Val1 += 1
                End If
                If theData(dataIndex).Val2 > 0 Then
                    nd.Val2 += 1
                End If
                dataIndex += 1
            Else
                'time to add the new data
                newData.Add(nd)
                lastdata = nd
                nd = New JustData4Sort(lastdata.Time + MSPerSample) 'copies the last data, and adds 125 millis
            End If
        Loop While dataIndex < theData.Count
        'add the last one
        newData.Add(nd)
        'now newData has the data grouped by 1/4 second intervals.
        Dim runner As Integer = 0
        Dim DoneData As New List(Of JustData4Sort)
        For Each d As JustData4Sort In newData
            If d.Val1 > 0 AndAlso d.Val1 >= d.Val2 Then
                runner = 100
            ElseIf d.Val2 > 0 Then
                runner = 1
            End If
            DoneData.Add(New JustData4Sort With {.Time = d.Time, .Val1 = d.Val1, .Val2 = d.Val2, .Val3 = runner})
        Next
        Using txout As New StreamWriter($"{baseFilename}_Mouth_percent.csv")
            For Each d As JustData4Sort In DoneData
                txout.WriteLine($"{d.Time},{d.Val3}")
            Next
            txout.Flush()
        End Using
    End Sub
    Public Sub MakeOutputFile(baseFile As String)
        If Not IO.File.Exists($"{baseFile}_Mouth.csv") Then
            MsgBox($"{baseFile}_Mouth.csv does not exist!")
            Exit Sub
        Else
            MakeMouthPercent(baseFile)

        End If
        If Not IO.File.Exists($"{baseFile}_Head_smoothed.csv") Then
            MsgBox($"{baseFile}_Head_Smoothed.csv does not exist!")
            Exit Sub
        End If
        If Not IO.File.Exists($"{baseFile}_Neck_smoothed.csv") Then
            MsgBox($"{baseFile}_Neck_Smoothed.csv does not exist!")
            Exit Sub
        End If
        If Not IO.File.Exists($"{baseFile}_left.csv") Then
            MsgBox($"{baseFile}_Left.csv does not exist!")
            Exit Sub
        End If
        If Not IO.File.Exists($"{baseFile}_right.csv") Then
            MsgBox($"{baseFile}_Right.csv does not exist!")
            Exit Sub
        End If
        Dim aCount As Integer = 0
        Dim aFound As Integer = 0
        ReDim allIn(5 * 60 * 10) ' 5 minutes at 10 samples per second should be close
        Using reader As New IO.StreamReader($"{baseFile}_Mouth.csv")
            While Not reader.EndOfStream
                Dim line As String = reader.ReadLine()
                Dim parts() As String = line.Split(","c)
                If parts.Length = 3 Then
                    If IsNumeric(parts(0)) AndAlso IsNumeric(parts(1)) AndAlso IsNumeric(parts(2)) Then
                        If CInt(parts(1)) <> 0 Then
                            allIn(FindTimeIndex(CLng(parts(0)))).MouthDown = CInt(parts(1))
                        End If
                        If CInt(parts(2)) <> 0 Then
                            allIn(FindTimeIndex(CLng(parts(0)))).MouthUp = CInt(parts(2))
                        End If
                    End If
                End If
            End While
        End Using
        Using reader As New IO.StreamReader($"{baseFile}_Neck_Smoothed.csv")
            While Not reader.EndOfStream
                Dim line As String = reader.ReadLine()
                Dim parts() As String = line.Split(","c)
                If parts.Length = 2 Then
                    If IsNumeric(parts(0)) AndAlso IsNumeric(parts(1)) Then
                        allIn(FindTimeIndex(CLng(parts(0)))).Neck = CInt(parts(1))
                    End If
                End If
            End While
        End Using
        Using reader As New IO.StreamReader($"{baseFile}_Head_Smoothed.csv")
            While Not reader.EndOfStream
                Dim line As String = reader.ReadLine()
                Dim parts() As String = line.Split(","c)
                If parts.Length = 2 Then
                    If IsNumeric(parts(0)) AndAlso IsNumeric(parts(1)) Then
                        allIn(FindTimeIndex(CLng(parts(0)))).Head = CInt(parts(1))
                    End If
                End If
            End While
        End Using
        Using reader As New IO.StreamReader($"{baseFile}_Left.csv")
            While Not reader.EndOfStream
                Dim line As String = reader.ReadLine()
                Dim parts() As String = line.Split(","c)
                If parts.Length = 3 Then
                    If IsNumeric(parts(0)) AndAlso IsNumeric(parts(1)) AndAlso IsNumeric(parts(2)) Then
                        If CInt(parts(1)) <> 0 Then
                            allIn(FindTimeIndex(CLng(parts(0)))).LeftDown = CInt(parts(1))
                        End If
                        If CInt(parts(2)) <> 0 Then
                            allIn(FindTimeIndex(CLng(parts(0)))).LeftUp = CInt(parts(2))
                        End If
                    End If
                End If
            End While
        End Using
        Using Reader As New IO.StreamReader($"{baseFile}_Right.csv")
            While Not Reader.EndOfStream
                Dim line As String = Reader.ReadLine()
                Dim parts() As String = line.Split(","c)
                If parts.Length = 3 Then
                    If IsNumeric(parts(0)) AndAlso IsNumeric(parts(1)) AndAlso IsNumeric(parts(2)) Then
                        If CInt(parts(1)) <> 0 Then
                            allIn(FindTimeIndex(CLng(parts(0)))).RightDown = CInt(parts(1))
                        End If
                        If CInt(parts(2)) <> 0 Then
                            allIn(FindTimeIndex(CLng(parts(0)))).RightUp = CInt(parts(2))
                        End If
                    End If
                End If
            End While
        End Using

        Array.Sort(Of inData)(allIn, Function(x, y)
                                         If x Is Nothing AndAlso y Is Nothing Then
                                             Return 0
                                         ElseIf x Is Nothing Then
                                             Return 1
                                         ElseIf y Is Nothing Then
                                             Return -1
                                         Else
                                             Return x.Time.CompareTo(y.Time)
                                         End If
                                     End Function)
        aCount = Array.FindLastIndex(allIn, Function(x) x IsNot Nothing) + 1
        'ReDim Preserve allIn(aCount - 1)
        'Ok, now need to group by 1/8 seconds, making sure if there is a change within the window, that it is always different than the last window.
        Dim tmp As New inData
        Dim last As New OutData
        Dim aCount2 As New List(Of inData)
        Dim HeadMin As Integer = -1
        Dim HeadMax As Integer = -1
        Dim NeckMin As Integer = -1
        Dim NeckMax As Integer = -1
        Dim iIndex As Integer = 0
        Dim iTemp As Long = 0
        While iIndex < aCount - 1
            If allIn(iIndex) IsNot Nothing Then
                If allIn(iIndex).Time > iTemp + MSPerSample Then
                    aCount2.Add(tmp)
                    tmp = New inData
                    iTemp += MSPerSample
                    tmp.Time = iTemp
                Else
                    If allIn(iIndex).MouthDown <> 0 Then
                        tmp.MouthDown = allIn(iIndex).MouthDown
                    End If
                    If allIn(iIndex).MouthUp <> 0 Then
                        tmp.MouthUp = allIn(iIndex).MouthUp
                    End If
                    If allIn(iIndex).Head <> 0 Then
                        tmp.Head = allIn(iIndex).Head
                        If HeadMin = -1 OrElse allIn(iIndex).Head < HeadMin Then
                            HeadMin = allIn(iIndex).Head
                        End If
                        If HeadMax = -1 OrElse allIn(iIndex).Head > HeadMax Then
                            HeadMax = allIn(iIndex).Head
                        End If
                    End If
                    If allIn(iIndex).Neck <> 0 Then
                        tmp.Neck = allIn(iIndex).Neck
                        If NeckMin = -1 OrElse allIn(iIndex).Neck < NeckMin Then
                            NeckMin = allIn(iIndex).Neck
                        End If
                        If NeckMax = -1 OrElse allIn(iIndex).Neck > NeckMax Then
                            NeckMax = allIn(iIndex).Neck
                        End If
                    End If
                    If allIn(iIndex).LeftDown <> 0 Then
                        tmp.LeftDown = allIn(iIndex).LeftDown
                    End If
                    If allIn(iIndex).LeftUp <> 0 Then
                        tmp.LeftUp = allIn(iIndex).LeftUp
                    End If
                    If allIn(iIndex).RightDown <> 0 Then
                        tmp.RightDown = allIn(iIndex).RightDown
                    End If
                    If allIn(iIndex).RightUp <> 0 Then
                        tmp.RightUp = allIn(iIndex).RightUp
                    End If
                    iIndex += 1
                End If
            End If
        End While
        aCount2.Add(tmp)
        'ok, now aCount2 has the grouped data.
        last = New OutData
        Dim newData As OutData
        Dim HorizDown As Integer
        Dim VertDown As Integer
        Dim HorizUp As Integer
        Dim VertUp As Integer
        Dim listOut As New List(Of OutData)
        For Each id As inData In aCount2
            newData = New OutData
            newData.Time = id.Time
            If id.MouthDown > 0 Then
                If id.MouthUp > 0 Then
                    'both set, look at what was last.
                    If last.MouthPercent = 100 Then 'was open
                        newData.MouthPercent = 1 'close the mouth
                    ElseIf last.MouthPercent = 1 Then
                        newData.MouthPercent = 100 'open the mouth
                    Else
                        'default to open
                        newData.MouthPercent = 100
                    End If
                Else 'only mouth down is set
                    If last.MouthPercent < 100 Then
                        newData.MouthPercent = 100
                    End If
                End If
            ElseIf id.MouthUp > 0 Then
                If last.MouthPercent > 1 Then
                    newData.MouthPercent = 1 'close the mouth
                End If
            End If
            If id.Head = HeadMin Then
                newData.HeadPercent = 1
            ElseIf id.Head = HeadMax Then
                newData.HeadPercent = 100
            ElseIf id.Head <> 0 Then
                newData.HeadPercent = CInt((id.Head - HeadMin) / (HeadMax - HeadMin) * 100)
            End If
            If id.Neck = NeckMin Then
                newData.NeckPercent = 1
            ElseIf id.Neck = NeckMax Then
                newData.NeckPercent = 100
            ElseIf id.Neck <> 0 Then
                newData.NeckPercent = CInt((id.Neck - NeckMin) / (NeckMax - NeckMin) * 100)
            End If
            If id.LeftDown > 0 Then
                Key2Percent(id.LeftDown, HorizDown, VertDown)
                newData.LeftHorzPercent = HorizDown
                newData.LeftVertPercent = VertDown
            ElseIf id.LeftUp > 0 Then
                Key2Percent(id.LeftUp, HorizUp, VertUp)
                newData.LeftHorzPercent = HorizUp
                newData.LeftVertPercent = VertUp
            Else
                newData.LeftHorzPercent = last.LeftHorzPercent
                newData.LeftVertPercent = last.LeftVertPercent
            End If
            If id.RightDown > 0 Then
                Key2Percent(id.RightDown, HorizDown, VertDown)
                newData.RightHorzPercent = HorizDown
                newData.RightVertPercent = VertDown
            ElseIf id.RightUp > 0 Then
                Key2Percent(id.RightUp, HorizUp, VertUp)
                newData.RightHorzPercent = HorizUp
                newData.RightVertPercent = VertUp
            Else
                newData.RightHorzPercent = last.RightHorzPercent
                newData.RightVertPercent = last.RightVertPercent
            End If
            If newData.MouthPercent = 0 Then newData.MouthPercent = last.MouthPercent
            If newData.HeadPercent = 0 Then newData.HeadPercent = last.HeadPercent
            If newData.NeckPercent = 0 Then newData.NeckPercent = last.NeckPercent
            If newData.LeftHorzPercent = 0 Then newData.LeftHorzPercent = last.LeftHorzPercent
            If newData.LeftVertPercent = 0 Then newData.LeftVertPercent = last.LeftVertPercent
            If newData.RightHorzPercent = 0 Then newData.RightHorzPercent = last.RightHorzPercent
            If newData.RightVertPercent = 0 Then newData.RightVertPercent = last.RightVertPercent

            listOut.Add(newData)
            last = newData
        Next

        Using txOut As IO.StreamWriter = New IO.StreamWriter($"{baseFile}_percent.csv")
            For Each od As OutData In listOut
                txOut.WriteLine($"{od.Time},{od.MouthPercent},{od.HeadPercent},{od.NeckPercent},{od.LeftHorzPercent},{od.LeftVertPercent},{od.RightHorzPercent},{od.RightVertPercent}")
            Next
            txOut.Flush()
        End Using

    End Sub
    Public Sub MakeFinalFile(baseFilename As String)
        Dim finalfile As New List(Of JustData4Sort)
        Dim tmp As JustData4Sort
        Dim sLine As String
        Dim sBreak() As String
        Dim tpct As Integer
        If True Then
            Using inMouth As StreamReader = IO.File.OpenText($"{baseFilename}_mouth_percent.csv")
                While Not inMouth.EndOfStream
                    sLine = inMouth.ReadLine
                    sBreak = sLine.Split(","c)
                    '0=ms, 1=mouth
                    If sBreak.Length > 1 Then
                        If IsNumeric(sBreak(0)) AndAlso IsNumeric(sBreak(1)) Then
                            tmp = New JustData4Sort With {.Time = CLng(sBreak(0)), .Val1 = CInt(sBreak(1))}
                            finalfile.Add(tmp)
                        End If
                    End If
                End While
            End Using
        End If
        If True Then
            Using inHead As StreamReader = IO.File.OpenText($"{baseFilename}_head_percent_smoothed.csv")
                While Not inHead.EndOfStream
                    sLine = inHead.ReadLine
                    sBreak = sLine.Split(","c)
                    '0=ms, 1=head
                    If sBreak.Length > 1 Then
                        If IsNumeric(sBreak(0)) AndAlso IsNumeric(sBreak(1)) Then
                            Dim a As Integer = finalfile.FindIndex(Function(x) x.Time = CLng(sBreak(0)))
                            If a > -1 Then
                                finalfile(a).Val2 = 10 * Math.Round(CInt(sBreak(1)) / 10, 0)
                            Else
                                finalfile.Add(New JustData4Sort With {.Time = CLng(sBreak(0)), .Val2 = 10 * Math.Round(CInt(sBreak(1)) / 10, 0)})
                            End If
                        End If
                    End If
                End While
            End Using
        End If
        If True Then
            Using inNeck As StreamReader = IO.File.OpenText($"{baseFilename}_neck_percent_smoothed.csv")
                While Not inNeck.EndOfStream
                    sLine = inNeck.ReadLine
                    sBreak = sLine.Split(","c)
                    '0=ms, 3=neck
                    If sBreak.Length > 1 Then
                        If IsNumeric(sBreak(0)) AndAlso IsNumeric(sBreak(1)) Then
                            Dim a As Integer = finalfile.FindIndex(Function(x) x.Time = CLng(sBreak(0)))
                            If a > -1 Then
                                finalfile(a).Val3 = 10 * Math.Round(CInt(sBreak(1)) / 10, 0)
                            Else
                                finalfile.Add(New JustData4Sort With {.Time = CLng(sBreak(0)), .Val3 = 10 * Math.Round(CInt(sBreak(1)) / 10, 0)})
                            End If
                        End If
                    End If
                End While
            End Using
        End If
        If True Then
            Using inRight As StreamReader = IO.File.OpenText($"{baseFilename}_left_percent.csv")
                While Not inRight.EndOfStream
                    sLine = inRight.ReadLine
                    sBreak = sLine.Split(","c)
                    '0=ms, 1=horz, 2=vert
                    If sBreak.Length > 2 Then
                        If IsNumeric(sBreak(0)) AndAlso IsNumeric(sBreak(1)) AndAlso IsNumeric(sBreak(2)) Then
                            tpct = sBreak(1)
                            Dim a As Integer = finalfile.FindIndex(Function(x) x.Time = CLng(sBreak(0)))
                            If a > -1 Then
                                finalfile(a).Val4 = 10 * Math.Round(tpct / 10, 0)
                                finalfile(a).Val5 = CInt(sBreak(2))
                            Else
                                finalfile.Add(New JustData4Sort With {.Time = CLng(sBreak(0)), .Val4 = 10 * Math.Round(tpct / 10, 0), .Val5 = CInt(sBreak(2))})
                            End If
                        End If
                    End If
                End While
            End Using
        End If
        If True Then
            Using inLeft As StreamReader = IO.File.OpenText($"{baseFilename}_right_percent.csv")
                While Not inLeft.EndOfStream
                    sLine = inLeft.ReadLine
                    sBreak = sLine.Split(","c)
                    '0=ms, 1=horz, 2=vert
                    If sBreak.Length > 2 Then
                        If IsNumeric(sBreak(0)) AndAlso IsNumeric(sBreak(1)) AndAlso IsNumeric(sBreak(2)) Then
                            tpct = sBreak(1)
                            Dim a As Integer = finalfile.FindIndex(Function(x) x.Time = CLng(sBreak(0)))
                            If a > -1 Then
                                finalfile(a).Val6 = 10 * Math.Round(tpct / 10, 0)
                                finalfile(a).Val7 = CInt(sBreak(2))
                            Else
                                finalfile.Add(New JustData4Sort With {.Time = CLng(sBreak(0)), .Val6 = 10 * Math.Round(tpct / 10, 0), .Val7 = CInt(sBreak(2))})
                            End If
                        End If
                    End If
                End While
            End Using
        End If
        Using txOut As StreamWriter = IO.File.CreateText($"{baseFilename}_prefinal_output.csv")
            txOut.WriteLine("Millis,Mouth,Head,Neck,LeftHorz,LeftVert,RightHorz,RightVert")
            For Each d As JustData4Sort In finalfile
                txOut.WriteLine($"{d.Time},{d.Val1},{d.Val2},{d.Val3},{d.Val4},{d.Val5},{d.Val6},{d.Val7}")
            Next
            txOut.Flush()
        End Using

        ' Assuming 'finalfile' is your List(Of YourObjectType)
        InterpolateProperty(finalfile, Function(item) item.Val2, Sub(item, value) item.Val2 = value)
        ' Process Val3
        InterpolateProperty(finalfile, Function(item) item.Val3, Sub(item, value) item.Val3 = value)
        ' Process Val4
        'InterpolateProperty(finalfile, Function(item) item.Val4, Sub(item, value) item.Val4 = value)
        ' Process Val5
        'InterpolateProperty(finalfile, Function(item) item.Val5, Sub(item, value) item.Val5 = value)
        ' Process Val6
        'InterpolateProperty(finalfile, Function(item) item.Val6, Sub(item, value) item.Val6 = value)
        ' Process Val7
        'InterpolateProperty(finalfile, Function(item) item.Val7, Sub(item, value) item.Val7 = value)

        Using txOut As StreamWriter = IO.File.CreateText($"{baseFilename}_final_output.csv")
            For Each d As JustData4Sort In finalfile
                txOut.WriteLine($"{d.Time},{d.Val1},{d.Val2},{d.Val3},{d.Val4},{d.Val5},{d.Val6},{d.Val7}")
            Next
            txOut.Flush()
        End Using


    End Sub


    Private Sub InterpolateProperty(ByVal data As List(Of JustData4Sort),
                                ByVal valueSelector As Func(Of JustData4Sort, Double),
                                ByVal valueSetter As Action(Of JustData4Sort, Double))

        Dim i As Integer = 0
        While i < data.Count
            ' Find the start of a block of zeros for the selected property
            If valueSelector(data(i)) = 0 Then
                Dim startIndex As Integer = i
                Dim lastNonZeroIndex As Integer = i - 1

                ' Find the end of the block
                Dim nextNonZeroIndex As Integer = startIndex
                While nextNonZeroIndex < data.Count AndAlso valueSelector(data(nextNonZeroIndex)) = 0
                    nextNonZeroIndex += 1
                End While

                ' Check if we found valid start and end points
                If lastNonZeroIndex >= 0 AndAlso nextNonZeroIndex < data.Count Then
                    ' Get the values we will interpolate between
                    Dim startValue As Double = valueSelector(data(lastNonZeroIndex))
                    Dim endValue As Double = valueSelector(data(nextNonZeroIndex))
                    Dim valueRange As Double = endValue - startValue
                    Dim indexRange As Double = nextNonZeroIndex - lastNonZeroIndex

                    ' Fill in the entire block of zeros
                    For j As Integer = startIndex To nextNonZeroIndex - 1
                        Dim positionInBlock As Double = j - lastNonZeroIndex
                        Dim stepValue As Double = (valueRange / indexRange) * positionInBlock
                        ' Use the setter to update the value
                        valueSetter(data(j), startValue + stepValue)
                    Next
                End If

                ' Skip ahead to the end of the block we just processed
                i = nextNonZeroIndex
            Else
                i += 1 ' Not a zero, move to the next item
            End If
        End While
    End Sub
    Public Sub FixPots2(infilename As String, outfilename As String)
        Dim Smoother1 As New AnalogSmoother
        'Dim Smoother2 As New AnalogSmoother
        'Dim Smoother3 As New AnalogSmoother
        Using TxIn As New IO.StreamReader(infilename)
            Using txOut As New IO.StreamWriter(outfilename)
                Dim line() As String
                txOut.WriteLine("Millis,Pot1")
                TxIn.ReadLine() ' Skip header
                While Not TxIn.EndOfStream
                    line = TxIn.ReadLine.Split(",")
                    If line.Length >= 1 Then
                        If IsNumeric(line(0)) AndAlso IsNumeric(line(1)) Then
                            Dim millis As Long = CLng(line(0))
                            Dim pot1 As Integer = CInt(line(1))
                            Dim smooth1 As Integer = CInt(Smoother1.AddSampleAndGetMedian(pot1))
                            txOut.WriteLine($"{millis},{smooth1}")
                        End If
                    End If
                End While
                txOut.Flush()
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' Gets the local IPv4 address of the machine.
    ''' </summary>
    ''' <returns>A string containing the IP address, or an error message if not found.</returns>
    Public Function GetLocalIPv4() As String
            ' Get the host name of the local machine.
            Dim hostName As String = Dns.GetHostName()

            ' Get the IP entry for the host.
            Dim ipEntry As IPHostEntry = Dns.GetHostEntry(hostName)

            ' Loop through all the IP addresses associated with the host.
            For Each ip As IPAddress In ipEntry.AddressList
                ' We are looking for an IPv4 address.
                If ip.AddressFamily = AddressFamily.InterNetwork Then
                    Return ip.ToString()
                End If
            Next

            ' If no IPv4 address was found, return a message.
            Return "No local IPv4 address found."
        End Function



    ''' <summary>
    ''' Generates a waveform image from a WAV file.
    ''' </summary>
    ''' <param name="wavFilePath">The full path to the .WAV file.</param>
    ''' <returns>A Bitmap object representing the waveform, or Nothing if an error occurs.</returns>
    ''' I had Gemini rewrite this function.  I don't know what happened to my original from a couple of years ago.
    Public Function CreateWaveformBitmap(ByVal wavFilePath As String) As Bitmap

        Dim pixelsPerSecond As Integer = 16
        Dim imageHeight As Integer = 300
        Dim imageWidth As Integer
        Dim bmp As Bitmap = New Bitmap(1, 1) ' Temporary initialization
        Dim samplesPerPixel As Integer
        Dim buffer As Byte()
        Dim midPoint As Integer
        Dim maxSample As Single
        Dim minSample As Single
        ' Determine the start sample for this pixel
        Dim startSample As Long
        Dim sampleIndex As Long
        Dim sampleValue As Single
        Try
            Using reader As New WaveFileReader(wavFilePath)
                ' --- 1. Calculate Bitmap Dimensions ---
                imageWidth = CInt(Math.Ceiling(reader.TotalTime.TotalSeconds * pixelsPerSecond))
                If imageWidth <= 0 Then imageWidth = 1 ' Ensure at least 1 pixel width
                bmp = New Bitmap(imageWidth, imageHeight)

                ' --- 2. Setup for Drawing ---
                Using g As Graphics = Graphics.FromImage(bmp)
                    Using dpen As Pen = New Pen(Color.LightSeaGreen, 1)
                        g.Clear(Color.Black) ' Background color

                        ' --- 3. Read Audio Data and Find Peaks ---
                        ' Number of samples per horizontal pixel
                        samplesPerPixel = CInt(reader.WaveFormat.SampleRate / pixelsPerSecond)
                        ' Buffer to hold all samples (assumes file fits in memory)
                        buffer = New Byte(CInt(reader.Length - 1)) {}
                        reader.Read(buffer, 0, CInt(reader.Length))

                        midPoint = imageHeight / 2

                        ' --- 4. Loop Through Each Pixel Column and Draw ---
                        For x As Integer = 0 To imageWidth - 1
                            maxSample = 0
                            minSample = 0

                            ' Determine the start sample for this pixel
                            startSample = x * samplesPerPixel

                            ' Find the min and max peak in this segment of samples
                            For i As Integer = 0 To samplesPerPixel - 1 Step reader.WaveFormat.BlockAlign
                                sampleIndex = startSample + i
                                If sampleIndex >= reader.SampleCount * (reader.WaveFormat.BitsPerSample / 8) Then Exit For

                                ' Convert byte data to a sample value (-1.0 to 1.0)
                                ' This example assumes 16-bit audio, the most common format.
                                If sampleIndex + 1 < buffer.Length Then
                                    sampleValue = BitConverter.ToInt16(buffer, CInt(sampleIndex)) / 32768.0F
                                    maxSample = Math.Max(maxSample, sampleValue)
                                    minSample = Math.Min(minSample, sampleValue)
                                End If
                            Next

                            ' --- 5. Draw the Vertical Line for this Pixel ---

                            g.DrawLine(dpen, x, CInt(midPoint - (maxSample * midPoint)), x, CInt(midPoint - (minSample * midPoint)))
                        Next

                    End Using
                End Using
            End Using

        Catch ex As Exception
            ' Handle exceptions like file not found, invalid format, etc.
            Console.WriteLine($"An error occurred: {ex.Message}")
            bmp = New Bitmap(1, 1)
        End Try
        Return bmp
    End Function

End Module
