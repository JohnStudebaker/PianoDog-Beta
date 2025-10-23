Imports System.Drawing.Text
Imports System.IO

Public Class JoyData
    Private mouthData As New List(Of JoyInfo)
    Private headData As New List(Of JoyInfo)
    Private neckData As New List(Of JoyInfo)
    Private leftData As New List(Of JoyInfo)
    Private rightData As New List(Of JoyInfo)
    Public Class JoyInfo
        Public Property Milliseconds As Long
        Public Property X As Integer
        Public Property Y As Integer
        Public Property Z As Integer
        Public Property Slider As Integer
        Public Property Trigger As Boolean
        Public Property Hat As Integer
    End Class

    Public Class NewDataTest
        Public Property Milliseconds As Long
        Public Property MouthVal As Long
        Public Property MouthCount As Integer
        Public Property MouthPercent As Integer

        Public Property MouthTrigger As Integer
        Public Property MouthHat As Integer
        Public Property MouthClosed As Integer

        Public Property HeadVal As Long
        Public Property HeadCount As Integer
        Public Property HeadPercent As Integer
        Public Property NeckVal As Long
        Public Property NeckCount As Integer

        Public Property NeckPercent As Integer
        Public Property LeftVal As Long
        Public Property LeftCount As Integer

        Public Property LeftPercent As Integer
        Public Property LeftTrue As Integer
        Public Property LeftFalse As Integer
        Public Property RightVal As Long
        Public Property RightCount As Integer
        Public Property RightPercent As Integer
        Public Property RightTrue As Integer
        Public Property RightFalse As Integer
        Public Sub New()
            MouthVal = 0
            MouthCount = 0
            MouthPercent = 0
            HeadVal = 0
            HeadCount = 0
            HeadPercent = 0
            NeckVal = 0
            NeckCount = 0
            NeckPercent = 0
            MouthClosed = 0
            MouthHat = 0
            MouthTrigger = 0

            LeftVal = 0
            LeftCount = 0
            LeftPercent = 0
            LeftTrue = 0
            LeftFalse = 0
            RightVal = 0
            RightCount = 0
            RightPercent = 0
            RightTrue = 0
            RightFalse = 0
        End Sub
    End Class
    Public NewDataList As New List(Of NewDataTest)


    Private Function ReadData(fullname As String) As List(Of JoyInfo)
        Dim Ret As List(Of JoyInfo) = New List(Of JoyInfo)
        Dim b As Boolean
        Using txin As StreamReader = IO.File.OpenText(fullname)
            Dim line As String = txin.ReadLine 'header
            While Not txin.EndOfStream
                line = txin.ReadLine
                Dim parts() As String = line.Split(","c)
                If parts.Length >= 8 Then
                    b = parts(7) = "True"
                ElseIf parts.Length >= 6 Then
                    b = parts(5) = "True"
                Else
                    b = False
                End If
                If parts.Length >= 7 Then
                    Dim ji As New JoyInfo With {
                       .Milliseconds = Long.Parse(parts(0)),
                        .X = Integer.Parse(parts(1)),
                        .Y = Integer.Parse(parts(2)),
                        .Z = Integer.Parse(parts(3)),
                        .Slider = Integer.Parse(parts(4)),
                        .Hat = Integer.Parse(parts(6)),
                        .Trigger = b
                    }
                    Ret.Add(ji)
                End If
            End While
        End Using
        Return Ret
    End Function

    Public Sub MakeFinal(MouthPath As String, HeadNeckPath As String, RightPath As String, LeftPath As String, FinishedPath As String)
        If IO.File.Exists($"{MouthPath}") Then
            mouthData = ReadData($"{MouthPath}")
        End If
        If IO.File.Exists($"{HeadNeckPath}") Then
            headData = ReadData($"{HeadNeckPath}")
        End If
        If IO.File.Exists($"{LeftPath}") Then
            leftData = ReadData($"{LeftPath}")
        End If
        If IO.File.Exists($"{RightPath}") Then
            rightData = ReadData($"{RightPath}")
        End If
        Dim thisMillies As Long = MSPerSample
        Dim mouthIDX As Integer = 0
        Dim headIDX As Integer = 0
        Dim neckIDX As Integer = 0
        Dim leftIDX As Integer = 0
        Dim rightIDX As Integer = 0
        Dim mouthMin As Integer = -1
        Dim mouthMax As Integer = -1
        Dim headMin As Integer = -1
        Dim headMax As Integer = -1
        Dim neckMin As Integer = -1
        Dim neckMax As Integer = -1
        Dim leftMin As Integer = -1
        Dim leftMax As Integer = -1
        Dim rightMin As Integer = -1
        Dim rightMax As Integer = -1
        Dim thisOne As New NewDataTest
        Dim zeroZ As Integer = leftData(0).Z
        thisOne.Milliseconds = thisMillies
        Do
            While mouthIDX < mouthData.Count AndAlso mouthData(mouthIDX).Milliseconds <= thisOne.Milliseconds
                thisOne.MouthVal += mouthData(mouthIDX).Slider
                thisOne.MouthCount += 1
                If mouthMin = -1 OrElse mouthData(mouthIDX).Slider < mouthMin Then
                    mouthMin = mouthData(mouthIDX).Slider
                End If
                If mouthMax = -1 OrElse mouthData(mouthIDX).Slider > mouthMax Then
                    mouthMax = mouthData(mouthIDX).Slider
                End If
                'thisOne.MouthPercent = CInt(Math.Round((mouthData(mouthIDX).Slider - 0) / (65535 - 0) * 100, 0) * 1)
                mouthIDX += 1
            End While
            While headIDX < headData.Count AndAlso headData(headIDX).Milliseconds <= thisOne.Milliseconds
                thisOne.HeadVal += headData(headIDX).Y
                thisOne.HeadCount += 1
                If headMin = -1 OrElse headData(headIDX).Y < headMin Then
                    headMin = headData(headIDX).Y
                End If
                If headMax = -1 OrElse headData(headIDX).Y > headMax Then
                    headMax = headData(headIDX).Y
                End If
                'thisOne.HeadPercent = CInt(Math.Round((headData(headIDX).Y - 0) / (65535 - 0) * 100, 0) * 1)
                thisOne.NeckVal += headData(headIDX).Z
                thisOne.NeckCount += 1
                If neckMin = -1 OrElse headData(headIDX).Z < neckMin Then
                    neckMin = headData(headIDX).Z
                End If
                If neckMax = -1 OrElse headData(headIDX).Z > neckMax Then
                    neckMax = headData(headIDX).Z
                End If
                'thisOne.NeckPercent = CInt(Math.Round((headData(headIDX).Z - 0) / (65535 - 0) * 100, 0) * 1)
                headIDX += 1
            End While
            While leftIDX < leftData.Count AndAlso leftData(leftIDX).Milliseconds <= thisOne.Milliseconds
                Debug.Assert(True Or leftData(leftIDX).Milliseconds < 9000)
                thisOne.LeftVal += leftData(leftIDX).Z
                thisOne.LeftCount += 1
                If leftMin = -1 OrElse leftData(leftIDX).Z < leftMin Then
                    leftMin = leftData(leftIDX).Z
                End If
                If leftMax = -1 OrElse leftData(leftIDX).Z > leftMax Then
                    leftMax = leftData(leftIDX).Z
                End If
                If leftData(leftIDX).Trigger Then
                    thisOne.LeftTrue += 1
                Else
                    thisOne.LeftFalse += 1
                End If
                leftIDX += 1
            End While
            While rightIDX < rightData.Count AndAlso rightData(rightIDX).Milliseconds <= thisOne.Milliseconds
                thisOne.RightVal += rightData(rightIDX).Z
                thisOne.RightCount += 1
                If rightMin = -1 OrElse rightData(rightIDX).Z < rightMin Then
                    rightMin = rightData(rightIDX).Z
                End If
                If rightMax = -1 OrElse rightData(rightIDX).Z > rightMax Then
                    rightMax = rightData(rightIDX).Z
                End If
                If rightData(rightIDX).Trigger Then
                    thisOne.RightTrue += 1
                Else
                    thisOne.RightFalse += 1
                End If
                rightIDX += 1
            End While
            NewDataList.Add(thisOne)
            thisMillies += MSPerSample
            thisOne = New NewDataTest
            thisOne.Milliseconds = thisMillies
        Loop Until mouthIDX >= mouthData.Count AndAlso headIDX >= headData.Count AndAlso neckIDX >= neckData.Count AndAlso leftIDX >= leftData.Count AndAlso rightIDX >= rightData.Count
        'now everything is loaded.  sort and group

        Dim newnewdatalist As New List(Of NewDataTest)
        For Each ndt In NewDataList
            'Debug.Assert(ndt.Milliseconds < 9000)
            If ndt.MouthCount > 0 OrElse ndt.HeadCount > 0 OrElse ndt.NeckCount > 0 OrElse ndt.LeftCount > 0 OrElse ndt.RightCount > 0 Then
                thisOne = New NewDataTest
                thisOne.Milliseconds = ndt.Milliseconds
                If ndt.MouthCount > 0 Then
                    thisOne.MouthPercent = Math.Min(100, Math.Max(1, ((ndt.MouthVal \ ndt.MouthCount) - mouthMin) / (mouthMax - mouthMin) * 100))
                Else
                    thisOne.MouthPercent = 0
                End If
                If ndt.HeadCount > 0 Then
                    thisOne.HeadPercent = Math.Min(100, Math.Max(1, ((ndt.HeadVal \ ndt.HeadCount) - headMin) / (headMax - headMin) * 100))
                Else
                    thisOne.HeadPercent = 0
                End If
                If ndt.NeckCount > 0 Then
                    thisOne.NeckPercent = Math.Min(100, Math.Max(1, ((ndt.NeckVal \ ndt.NeckCount) - neckMin) / (neckMax - neckMin) * 100))
                Else
                    thisOne.NeckPercent = 0
                End If
                If ndt.LeftCount > 0 Then
                    thisOne.LeftPercent = Math.Min(100, Math.Max(1, ((ndt.LeftVal \ ndt.LeftCount) - leftMin) / (leftMax - leftMin) * 100))
                    If ndt.LeftTrue > ndt.LeftFalse Then
                        thisOne.LeftTrue = 1
                    Else
                        thisOne.LeftFalse = 1
                    End If
                Else
                    thisOne.LeftPercent = 0
                    thisOne.LeftTrue = 0
                    thisOne.LeftFalse = 0
                End If
                If ndt.RightCount > 0 Then
                    thisOne.RightPercent = Math.Min(100, Math.Max(1, ((ndt.RightVal \ ndt.RightCount) - rightMin) / (rightMax - rightMin) * 100))
                    If ndt.RightTrue > ndt.RightFalse Then
                        thisOne.RightTrue = 1
                    Else
                        thisOne.RightFalse = 1
                    End If
                Else
                    thisOne.RightPercent = 0
                    thisOne.RightTrue = 0
                    thisOne.RightFalse = 0
                End If
                newnewdatalist.Add(thisOne)
            End If
        Next
        Dim tdt As New NewDataTest
        tdt.Milliseconds = 0
        tdt.MouthPercent = newnewdatalist(0).MouthPercent
        tdt.HeadPercent = newnewdatalist(0).HeadPercent
        tdt.NeckPercent = newnewdatalist(0).NeckPercent
        tdt.LeftPercent = newnewdatalist(0).LeftPercent
        tdt.LeftTrue = newnewdatalist(0).LeftTrue
        tdt.RightPercent = newnewdatalist(0).RightPercent
        tdt.RightTrue = newnewdatalist(0).RightTrue
        Dim wt As Boolean = True
        Using txOut As IO.StreamWriter = IO.File.CreateText($"{FinishedPath}")
            'if any value changes, write all values, otherwise skip
            'txOut.WriteLine("Milliseconds,MouthVal,MouthPercent,HeadVal,HeadPercent,NeckVal,NeckPercent,LeftVal,LeftPercent,LeftTrigger,RightVal,RightPercent,RightTrigger")
            For Each ndt In newnewdatalist
                tdt.Milliseconds = ndt.Milliseconds
                If ndt.MouthPercent <> 0 Then
                    If ndt.MouthPercent <> tdt.MouthPercent Then
                        wt = True
                        tdt.MouthPercent = ndt.MouthPercent
                    End If
                End If
                If ndt.HeadPercent <> 0 Then
                    If ndt.HeadPercent <> tdt.HeadPercent Then
                        wt = True
                        tdt.HeadPercent = ndt.HeadPercent
                    End If
                End If
                If ndt.NeckPercent <> 0 Then
                    If ndt.NeckPercent <> tdt.NeckPercent Then
                        wt = True
                        tdt.NeckPercent = ndt.NeckPercent
                    End If
                End If
                If ndt.LeftPercent <> 0 Then
                    If ndt.LeftPercent <> tdt.LeftPercent Then
                        wt = True
                        tdt.LeftPercent = ndt.LeftPercent
                    End If
                    If ndt.LeftTrue <> tdt.LeftTrue Then
                        wt = True
                        tdt.LeftTrue = ndt.LeftTrue  '1 or 0
                    End If
                End If
                If ndt.RightPercent <> 0 Then
                    If ndt.RightPercent <> tdt.RightPercent Then
                        wt = True
                        tdt.RightPercent = ndt.RightPercent
                    End If
                    If ndt.RightTrue <> tdt.RightTrue Then
                        wt = True
                        tdt.RightTrue = ndt.RightTrue
                    End If
                End If
                If wt Then
                    txOut.WriteLine($"{tdt.Milliseconds},{tdt.MouthPercent},{tdt.HeadPercent},{tdt.NeckPercent},{tdt.LeftPercent},{If(tdt.LeftTrue = 1, "100", "1")},{tdt.RightPercent},{If(tdt.RightTrue = 1, "100", "1")}")
                    wt = False
                End If
            Next
        End Using
    End Sub

    Public Sub MakeFinal2(MouthHeadNeck As String, RightPath As String, LeftPath As String, FinishedPath As String)
        If IO.File.Exists($"{MouthHeadNeck}") Then
            mouthData = ReadData($"{MouthHeadNeck}")
        End If
        If IO.File.Exists($"{LeftPath}") Then
            leftData = ReadData($"{LeftPath}")
        End If
        If IO.File.Exists($"{RightPath}") Then
            rightData = ReadData($"{RightPath}")
        End If
        Dim thisMillies As Long = MSPerSample
        Dim mouthIDX As Integer = 0
        Dim headIDX As Integer = 0
        Dim neckIDX As Integer = 0
        Dim leftIDX As Integer = 0
        Dim rightIDX As Integer = 0
        Dim headMin As Integer = -1
        Dim headMax As Integer = -1
        Dim neckMin As Integer = -1
        Dim neckMax As Integer = -1
        Dim leftMin As Integer = -1
        Dim leftMax As Integer = -1
        Dim rightMin As Integer = -1
        Dim rightMax As Integer = -1
        Dim thisOne As New NewDataTest
        Dim zeroZ As Integer = leftData(0).Z
        thisOne.Milliseconds = thisMillies
        Do
            While mouthIDX < mouthData.Count AndAlso mouthData(mouthIDX).Milliseconds <= thisOne.Milliseconds
                If mouthData(mouthIDX).Hat > -1 Then
                    thisOne.MouthHat += 1
                ElseIf mouthData(mouthIDX).Trigger Then
                    thisOne.MouthTrigger += 1
                Else
                    thisOne.MouthClosed += 1
                End If
                thisOne.MouthCount += 1
                thisOne.HeadVal += mouthData(mouthIDX).Y
                thisOne.HeadCount += 1
                If headMin = -1 OrElse mouthData(mouthIDX).Y < headMin Then
                    headMin = mouthData(mouthIDX).Y
                End If
                If headMax = -1 OrElse mouthData(mouthIDX).Y > headMax Then
                    headMax = mouthData(mouthIDX).Y
                End If
                thisOne.NeckVal += mouthData(mouthIDX).Z
                thisOne.NeckCount += 1
                If neckMin = -1 OrElse mouthData(mouthIDX).Z < neckMin Then
                    neckMin = mouthData(mouthIDX).Z
                End If
                If neckMax = -1 OrElse mouthData(mouthIDX).Z > neckMax Then
                    neckMax = mouthData(mouthIDX).Z
                End If
                mouthIDX += 1
            End While
            While leftIDX < leftData.Count AndAlso leftData(leftIDX).Milliseconds <= thisOne.Milliseconds
                Debug.Assert(True Or leftData(leftIDX).Milliseconds < 9000)
                thisOne.LeftVal += leftData(leftIDX).Z
                thisOne.LeftCount += 1
                If leftMin = -1 OrElse leftData(leftIDX).Z < leftMin Then
                    leftMin = leftData(leftIDX).Z
                End If
                If leftMax = -1 OrElse leftData(leftIDX).Z > leftMax Then
                    leftMax = leftData(leftIDX).Z
                End If
                If leftData(leftIDX).Trigger Then
                    thisOne.LeftTrue += 1
                Else
                    thisOne.LeftFalse += 1
                End If
                leftIDX += 1
            End While
            While rightIDX < rightData.Count AndAlso rightData(rightIDX).Milliseconds <= thisOne.Milliseconds
                thisOne.RightVal += rightData(rightIDX).Z
                thisOne.RightCount += 1
                If rightMin = -1 OrElse rightData(rightIDX).Z < rightMin Then
                    rightMin = rightData(rightIDX).Z
                End If
                If rightMax = -1 OrElse rightData(rightIDX).Z > rightMax Then
                    rightMax = rightData(rightIDX).Z
                End If
                If rightData(rightIDX).Trigger Then
                    thisOne.RightTrue += 1
                Else
                    thisOne.RightFalse += 1
                End If
                rightIDX += 1
            End While
            NewDataList.Add(thisOne)
            thisMillies += MSPerSample
            thisOne = New NewDataTest
            thisOne.Milliseconds = thisMillies
        Loop Until mouthIDX >= mouthData.Count AndAlso leftIDX >= leftData.Count AndAlso rightIDX >= rightData.Count
        'now everything is loaded.  sort and group
        Dim lastMouthPercent As Integer = -1
        Dim newnewdatalist As New List(Of NewDataTest)
        For Each ndt In NewDataList
            'Debug.Assert(ndt.Milliseconds < 9000)
            If ndt.MouthCount > 0 OrElse ndt.HeadCount > 0 OrElse ndt.NeckCount > 0 OrElse ndt.LeftCount > 0 OrElse ndt.RightCount > 0 Then
                thisOne = New NewDataTest
                thisOne.Milliseconds = ndt.Milliseconds
                If ndt.MouthCount > 0 Then
                    If ndt.MouthHat > ndt.MouthTrigger AndAlso ndt.MouthHat > ndt.MouthClosed Then
                        thisOne.MouthPercent = 100
                        If lastMouthPercent = 100 Then
                            If ndt.MouthTrigger + ndt.MouthClosed > 0 Then
                                thisOne.MouthPercent = 70
                            End If
                        End If
                    ElseIf ndt.MouthTrigger > ndt.MouthClosed Then
                        thisOne.MouthPercent = 70
                        If lastMouthPercent = 70 Then
                            If ndt.MouthClosed > 0 Then
                                thisOne.MouthPercent = 1
                            End If
                        End If
                    Else
                        thisOne.MouthPercent = 1
                        If lastMouthPercent = 1 Then
                            If ndt.MouthHat + ndt.MouthTrigger > 0 Then
                                thisOne.MouthPercent = 70
                            End If
                        End If
                    End If
                Else
                    thisOne.MouthPercent = 0
                End If
                If ndt.HeadCount > 0 Then
                    thisOne.HeadPercent = Math.Min(100, Math.Max(1, ((ndt.HeadVal \ ndt.HeadCount) - headMin) / (headMax - headMin) * 100))
                Else
                    thisOne.HeadPercent = 0
                End If
                If ndt.NeckCount > 0 Then
                    thisOne.NeckPercent = Math.Min(100, Math.Max(1, ((ndt.NeckVal \ ndt.NeckCount) - neckMin) / (neckMax - neckMin) * 100))
                Else
                    thisOne.NeckPercent = 0
                End If
                If ndt.LeftCount > 0 Then
                    thisOne.LeftPercent = Math.Min(100, Math.Max(1, ((ndt.LeftVal \ ndt.LeftCount) - leftMin) / (leftMax - leftMin) * 100))
                    If ndt.LeftTrue > ndt.LeftFalse Then
                        thisOne.LeftTrue = 1
                    Else
                        thisOne.LeftFalse = 1
                    End If
                Else
                    thisOne.LeftPercent = 0
                    thisOne.LeftTrue = 0
                    thisOne.LeftFalse = 0
                End If
                If ndt.RightCount > 0 Then
                    thisOne.RightPercent = Math.Min(100, Math.Max(1, ((ndt.RightVal \ ndt.RightCount) - rightMin) / (rightMax - rightMin) * 100))
                    If ndt.RightTrue > ndt.RightFalse Then
                            thisOne.RightTrue = 1
                        Else
                            thisOne.RightFalse = 1
                        End If
                    Else
                        thisOne.RightPercent = 0
                        thisOne.RightTrue = 0
                        thisOne.RightFalse = 0
                    End If
                newnewdatalist.Add(thisOne)
                lastMouthPercent = thisOne.MouthPercent
            End If
        Next
        Dim tdt As New NewDataTest
        tdt.Milliseconds = 0
        tdt.MouthPercent = newnewdatalist(0).MouthPercent
        tdt.HeadPercent = newnewdatalist(0).HeadPercent
        tdt.NeckPercent = newnewdatalist(0).NeckPercent
        tdt.LeftPercent = newnewdatalist(0).LeftPercent
        tdt.LeftTrue = newnewdatalist(0).LeftTrue
        tdt.RightPercent = newnewdatalist(0).RightPercent
        tdt.RightTrue = newnewdatalist(0).RightTrue
        Dim wt As Boolean = True
        Using txOut As IO.StreamWriter = IO.File.CreateText($"{FinishedPath}")
            'if any value changes, write all values, otherwise skip
            'txOut.WriteLine("Milliseconds,MouthVal,MouthPercent,HeadVal,HeadPercent,NeckVal,NeckPercent,LeftVal,LeftPercent,LeftTrigger,RightVal,RightPercent,RightTrigger")
            For Each ndt In newnewdatalist
                tdt.Milliseconds = ndt.Milliseconds
                If ndt.MouthPercent <> 0 Then
                    If ndt.MouthPercent <> tdt.MouthPercent Then
                        wt = True
                        tdt.MouthPercent = ndt.MouthPercent
                    End If
                End If
                If ndt.HeadPercent <> 0 Then
                    If ndt.HeadPercent <> tdt.HeadPercent Then
                        wt = True
                        tdt.HeadPercent = ndt.HeadPercent
                    End If
                End If
                If ndt.NeckPercent <> 0 Then
                    If ndt.NeckPercent <> tdt.NeckPercent Then
                        wt = True
                        tdt.NeckPercent = ndt.NeckPercent
                    End If
                End If
                If ndt.LeftPercent <> 0 Then
                    If ndt.LeftPercent <> tdt.LeftPercent Then
                        wt = True
                        tdt.LeftPercent = ndt.LeftPercent
                    End If
                    If ndt.LeftTrue <> tdt.LeftTrue Then
                        wt = True
                        tdt.LeftTrue = ndt.LeftTrue  '1 or 0
                    End If
                End If
                If ndt.RightPercent <> 0 Then
                    If ndt.RightPercent <> tdt.RightPercent Then
                        wt = True
                        tdt.RightPercent = ndt.RightPercent
                    End If
                    If ndt.RightTrue <> tdt.RightTrue Then
                        wt = True
                        tdt.RightTrue = ndt.RightTrue
                    End If
                End If
                If wt Then
                    txOut.WriteLine($"{tdt.Milliseconds},{tdt.MouthPercent},{tdt.HeadPercent},{tdt.NeckPercent},{tdt.LeftPercent},{If(tdt.LeftTrue = 1, "100", "1")},{tdt.RightPercent},{If(tdt.RightTrue = 1, "100", "1")}")
                    wt = False
                End If
            Next
        End Using
    End Sub
End Class
