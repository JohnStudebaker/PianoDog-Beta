Imports System.IO
Imports System.Net.Http
Imports System.Reflection.Emit
Imports System.Security.Policy
Imports System.Text
Imports NAudio.CoreAudioApi
'Imports Windows.ApplicationModel.Chat
'Imports Windows.Web.Http
'Imports Windows.Web.Http.Filters

Public Class PianoDog
    Public Event UploadProgress(ByVal thePercent As Integer)

    Private Const cMouthClosed As Integer = 68
    Private Const cMouthOpen As Integer = 74   'was 74, trying 72 to see if it helps with overextension
    Private Const cMouthHalf As Integer = 72  'open or closed, that's it
    Private Const cHeadDown As Integer = 60
    Private Const cHeadStraight As Integer = 50
    Private Const cHeadUp As Integer = 30
    Private Const cNeckLeft As Integer = 40
    Private Const cNeckStraight As Integer = 70
    Private Const cNeckRight As Integer = 100
    Private Const cLeftHorOut As Integer = 70
    Private Const cLeftHorCenter As Integer = 50
    Private Const cLeftHorIn As Integer = 30
    Private Const cLeftVertDown As Integer = 70 '98
    Private Const cLeftVertMid As Integer = 76
    Private Const cLeftVertHigh As Integer = 80 '52
    Private Const cRightHorOut As Integer = 70
    Private Const cRightHorCenter As Integer = 50 'NOT SET
    Private Const cRightHorIn As Integer = 30
    Private Const cRightVertDown As Integer = 58
    Private Const cRightVertMid As Integer = 52
    Private Const cRightVertHigh As Integer = 48

    Private _SongName As String
    Private _theSong As List(Of songData)
    Private _indexS As String = ""
    Public Sub New()
        _theSong = Nothing
    End Sub

    Public Sub New(index As Integer)
        _theSong = Nothing
        _indexS = $"{index.ToString}"
    End Sub

    Public Sub ClearSong()
        _theSong = Nothing
    End Sub

    Public Sub LoadSong(ByVal theDataFile As String)
        ClearSong()
        _SongName = theDataFile
        _theSong = New List(Of songData)
        Dim s() As String
        Using txIN As IO.StreamReader = IO.File.OpenText($"{theDataFile}")
            While Not txIN.EndOfStream
                s = txIN.ReadLine.Split(",")
                If s.Length > 7 AndAlso IsNumeric(s(0)) Then
                    If (justSeconds = 0) OrElse (s(0) < justSeconds * 1000) Then
                        _theSong.Add(New songData With {.Milliseconds = s(0), .MouthPercentOpen = s(1), .HeadPercentUp = s(2), .NeckPercentRight = s(3), .LeftHorizontalPercentOut = s(4), .LeftVerticalPercentUp = s(5), .RightHorizontalPercentOut = s(6), .RightVerticalPercentUp = s(7)})
                    End If
                End If
            End While
        End Using
        'now try simplifying the data

    End Sub

    Public Function PublishSong() As String

        Dim lastHead As Integer = 0
        Dim lastMouth As Integer = 0
        Dim lastNeck As Integer = 0
        Dim lastLeftH As Integer = 0
        Dim lastLeftV As Integer = 0
        Dim lastRightH As Integer = 0
        Dim lastRightV As Integer = 0

        Dim s1 As String
        Dim s2 As String
        Dim sb As New StringBuilder
        Dim iCount As Integer = 0
        Dim st As New StringBuilder
        Dim sRet As String = "OK"

        If _theSong IsNot Nothing Then
            '  Using txOUT As IO.StreamWriter = IO.File.CreateText($"{_SongName}_pianodog_out.csv")
            s1 = HTTPHelper($"http://pianodog.local/Size?as={_theSong.Count}")
                If Not s1.StartsWith("OK") Then
                    sRet = "ERROR: " & s1
                End If
                'RaiseEvent UploadProgress(5)
                sb.Length = 0
                If sRet = "OK" Then
                    For iCount = 1 To _theSong.Count - 1
                        st.Length = 0
                        st.Append($"{iCount.ToString},{_theSong(iCount).Milliseconds},")
                        If _theSong(iCount).MouthPWM <> lastMouth Then
                            st.Append(_theSong(iCount).MouthPWM.ToString)
                            lastMouth = _theSong(iCount).MouthPWM
                        Else
                            st.Append("0")
                        End If
                        st.Append(",")
                        If _theSong(iCount).HeadPWM <> lastHead Then
                            st.Append(_theSong(iCount).HeadPWM.ToString)
                            lastHead = _theSong(iCount).HeadPWM
                        Else
                            st.Append("0")
                        End If
                        st.Append(",")
                        If _theSong(iCount).NeckPWM <> lastNeck Then
                            st.Append(_theSong(iCount).NeckPWM.ToString)
                            lastNeck = _theSong(iCount).NeckPWM
                        Else
                            st.Append("0")
                        End If
                        st.Append(",")
                        If _theSong(iCount).LeftHorizontalPWM <> lastLeftH Then
                            st.Append(_theSong(iCount).LeftHorizontalPWM.ToString)
                            lastLeftH = _theSong(iCount).LeftHorizontalPWM
                        Else
                            st.Append("0")
                        End If
                        st.Append(",")
                        If _theSong(iCount).LeftVerticalPWM <> lastLeftV Then
                            st.Append(_theSong(iCount).LeftVerticalPWM.ToString)
                            lastLeftV = _theSong(iCount).LeftVerticalPWM
                        Else
                            st.Append("0")
                        End If
                        st.Append(",")
                        If _theSong(iCount).RightHorizontalPWM <> lastRightH Then
                            st.Append(_theSong(iCount).RightHorizontalPWM.ToString)
                            lastRightH = _theSong(iCount).RightHorizontalPWM
                        Else
                            st.Append("0")
                        End If
                        st.Append(",")
                        If _theSong(iCount).RightVerticalPWM <> lastRightV Then
                            st.Append(_theSong(iCount).RightVerticalPWM.ToString)
                            lastRightV = _theSong(iCount).RightVerticalPWM
                        Else
                            st.Append("0")
                        End If
                        sb.Append(st.ToString)
                    '             txOUT.WriteLine(st.ToString)
                    sb.Append(vbLf)
                        If sb.Length > (1024) Then
                            s1 = HTTPHelper($"http://pianodog.local/Data", sb.ToString)
                            If Not s1.StartsWith("OK") Then
                                sRet = "ERROR: " & s1
                                Exit For
                            End If
                            RaiseEvent UploadProgress((Math.Round(100 * iCount / _theSong.Count, 0)))
                            'Threading.Thread.Sleep(100)
                            sb.Length = 0
                        End If
                    Next
                    If sb.Length > 0 AndAlso sRet = "OK" Then
                        s1 = HTTPHelper($"http://pianodog.local/Data", sb.ToString)
                        If Not s1.StartsWith("OK") Then
                            sRet = "ERROR: " & s1
                        End If
                    End If
                    RaiseEvent UploadProgress(100)
                End If
            'End Using
        End If
        Return sRet
    End Function

    Public Function StartSong(ByRef WhenToStart As DateTime) As String
        'ForceTimeSync()
        Dim sRet As String = "OK"
        Dim s As String
        Dim x As DateTimeOffset = New DateTimeOffset(WhenToStart)
        s = HTTPHelper($"http://pianodog.local/Run?run={x.ToUnixTimeMilliseconds.ToString}")
        Return sRet
    End Function

    Public Function TimeSync(ByVal ThisIP As String) As Long
        Dim s As String
        If testingnow Then
            Return 1
        End If
        s = HTTPHelper($"http://pianodog.local/Sync?sync={ThisIP}")
        If Not IsNumeric(s) Then
            Return 0
        Else
            Return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds - CLng(s)
        End If
    End Function

    Public Function ScheduleStart(WhenToStart As DateTime) As String
        Dim sRet As String = "OK"
        Dim s As String
        s = HTTPHelper($"http://pianodog.local/Run?run={Format(WhenToStart, "yyyy-MM-dd HH:mm:ss")}")
        If Not s.StartsWith("OK") Then
            sRet = "ERROR: " & s
        End If
        Return sRet
    End Function
    Private Function HTTPHelper(ByVal theURL As String, Optional ByVal theData As String = "") As String
        ' HttpClient is best used when instantiated once and reused.
        ' But for a simple synchronous function, a new instance is acceptable.
        Dim response As HttpResponseMessage
        Dim responseData As String = "ERROR"
        If testingnow Then
            Return "OK:"
        End If
        Using H As New HttpClient()
            Try
                ' 1. Set the timeout to 30 seconds.
                H.Timeout = TimeSpan.FromSeconds(90)
                If String.IsNullOrEmpty(theData) Then
                    ' If no data, do a simple GET
                    response = H.GetAsync(theURL).Result
                    response.EnsureSuccessStatusCode()
                    responseData = response.Content.ReadAsStringAsync().Result
                Else
                    Dim content As New StringContent(theData, System.Text.Encoding.UTF8, "text/plain")
                    ' 2. Make the web request synchronous by getting the .Result
                    response = H.PostAsync(theURL, content).Result
                    response.EnsureSuccessStatusCode()
                    ' 3. Read the response synchronously
                    responseData = response.Content.ReadAsStringAsync().Result
                End If
                If String.IsNullOrEmpty(responseData) Then responseData = "NO REPLY"
                ' 4. Catch the specific exception for a timeout
            Catch aggEx As AggregateException When aggEx.InnerExceptions.OfType(Of TaskCanceledException)().Any()
                responseData = "ERROR: The request timed out after 30 seconds."
            Catch ex As HttpRequestException
                ' Handle HTTP-specific exceptions (like 404 Not Found, 500 Server Error)
                responseData = $"ERROR: HTTP Error - {ex.Message}"
            Catch ex As Exception
                ' Handle all other exceptions (like DNS issues, etc.)
                responseData = $"ERROR: A general error occurred - {ex.Message}"
            End Try
        End Using
        Return responseData
    End Function
    Private Class songData
        Private _Milliseconds As Long
        Private _MouthPercent As Integer   'Storage values are 1-51 (0=no change)
        Private _HeadPercent As Integer   'Storage values are 1-51 (0=no change)
        Private _NeckPercent As Integer   'Storage values are 1-51 (0=no change)
        Private _LeftHorizontalPercent As Integer   'Storage values are 1-51 (0=no change)
        Private _LeftVerticalPercent As Integer   'Storage values are 1-51 (0=no change)
        Private _RightHorizontalPercent As Integer   'Storage values are 1-51 (0=no change)
        Private _RightVerticalPercent As Integer   'Storage values are 1-51 (0=no change)
        Public Sub New()
            _Milliseconds = 0
            _MouthPercent = 0
            _HeadPercent = 0
            _NeckPercent = 0
            _LeftHorizontalPercent = 0
            _LeftVerticalPercent = 0
            _RightHorizontalPercent = 0
            _RightVerticalPercent = 0
        End Sub

        Public Property Milliseconds As Long
            Get
                Return _Milliseconds
            End Get
            Set(value As Long)
                _Milliseconds = value
            End Set
        End Property

        Public Property MouthPercentOpen As Integer
            Get
                Return _MouthPercent
            End Get
            Set(value As Integer)
                ClampPercent(value)
                _MouthPercent = value
                '_MouthPercent = MapAdvanced(value, 1, 50, 100, cMouthClosed, cMouthHalf, cMouthOpen)
            End Set
        End Property
        Public ReadOnly Property MouthPWM As Integer
            Get
                Return MapAdvanced(_MouthPercent, 1, 70, 100, cMouthClosed, cMouthHalf, cMouthOpen)
            End Get
        End Property

        Public Property NeckPercentRight As Integer
            Get
                Return _NeckPercent
            End Get
            Set(value As Integer)
                ClampPercent(value)
                _NeckPercent = value
                '_NeckPercent = MapAdvanced(value, 1, 50, 100, cNeckLeft, cNeckStraight, cNeckRight)
            End Set
        End Property
        Public ReadOnly Property NeckPWM As Integer
            Get
                Return MapAdvanced(_NeckPercent, 0, 50, 100, cNeckLeft, cNeckStraight, cNeckRight)
            End Get
        End Property
        Public Property HeadPercentUp As Integer
            Get
                Return _HeadPercent
            End Get
            Set(value As Integer)
                ClampPercent(value)
                _HeadPercent = value
                '_HeadPercent = MapAdvanced(value, 1, 50, 100, cHeadDown, cHeadStraight, cHeadUp)
            End Set
        End Property
        Public ReadOnly Property HeadPWM As Integer
            Get
                Return MapAdvanced(_HeadPercent, 0, 50, 100, cHeadDown, cHeadStraight, cHeadUp)
            End Get
        End Property

        Public Property LeftHorizontalPercentOut As Integer
            Get
                Return _LeftHorizontalPercent
            End Get
            Set(value As Integer)
                ClampPercent(value)
                _LeftHorizontalPercent = value
                '_LeftHorizontalPercent = MapAdvanced(value, 1, 50, 100, cLeftHorLeft, cLeftHorCenter, cLeftHorRight)
            End Set
        End Property
        Public ReadOnly Property LeftHorizontalPWM As Integer
            Get
                Return MapAdvanced(_LeftHorizontalPercent, 0, 50, 100, cLeftHorIn, cLeftHorCenter, cLeftHorOut)
            End Get
        End Property
        Public Property LeftVerticalPercentUp As Integer
            Get
                Return _LeftVerticalPercent
            End Get
            Set(value As Integer)
                ClampPercent(value)
                _LeftVerticalPercent = value
                '_LeftVerticalPercent = MapAdvanced(value, 1, 50, 100, cLeftVertDown, cLeftVertMid, cLeftVertHigh)
            End Set
        End Property
        Public ReadOnly Property LeftVerticalPWM As Integer
            Get
                Return MapAdvanced(_LeftVerticalPercent, 0, 50, 100, cLeftVertDown, cLeftVertMid, cLeftVertHigh)
            End Get
        End Property
        Public Property RightHorizontalPercentOut As Integer
            Get
                Return _RightHorizontalPercent
            End Get
            Set(value As Integer)
                ClampPercent(value)
                _RightHorizontalPercent = value
                '_RightHorizontalPercent = MapAdvanced(value, 1, 50, 100, cRightHorLeft, cRightHorCenter, cRightHorRight)
            End Set
        End Property
        Public ReadOnly Property RightHorizontalPWM As Integer
            Get
                Return MapAdvanced(_RightHorizontalPercent, 0, 50, 100, cRightHorIn, cRightHorCenter, cRightHorOut)
            End Get
        End Property
        Public Property RightVerticalPercentUp As Integer
            Get
                Return _RightVerticalPercent
            End Get
            Set(value As Integer)
                ClampPercent(value)
                _RightVerticalPercent = value
                '_RightVerticalPercent = MapAdvanced(value, 1, 50, 100, cRightVertDown, cRightVertMid, cRightVertHigh)
            End Set
        End Property
        Public ReadOnly Property RightVerticalPWM As Integer
            Get
                Return MapAdvanced(_RightVerticalPercent, 0, 50, 100, cRightVertDown, cRightVertMid, cRightVertHigh)
            End Get
        End Property
        Private Sub ClampPercent(ByRef pVal As Integer)
            If pVal < 0 Then pVal = 0
            If pVal > 100 Then pVal = 100
        End Sub
        Private Function MapAdvanced(X As Integer,
                                 Min_X As Integer, Mid_X As Integer, Max_X As Integer,
                                 Out_Min As Integer, Out_Mid As Integer, Out_Max As Integer) As Integer
            Dim outVal As Single
            Dim ratio As Double
            If X = 0 OrElse X < Min_X Then
                Return Out_Min
            ElseIf X > Max_X Then
                Return Out_Max
            Else
                If X <= Mid_X Then
                    If Mid_X = Min_X Then Return Out_Min ' avoid div by zero
                    ratio = (X - Min_X) / (Mid_X - Min_X)
                    outVal = (Out_Min + ratio * (Out_Mid - Out_Min))
                Else
                    If Max_X = Mid_X Then Return Out_Mid ' avoid div by zero
                    ratio = (X - Mid_X) / (Max_X - Mid_X)
                    outVal = (Out_Mid + ratio * (Out_Max - Out_Mid))
                End If
                Return CInt(Math.Round((outVal / 2), 0)) * 2 ' make even
            End If
        End Function
    End Class

End Class
