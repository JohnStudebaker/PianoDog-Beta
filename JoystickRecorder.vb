Imports SharpDX.DirectInput
Imports System.IO
Imports System.ComponentModel

Public Class JoystickRecorder
    Private joystickGuid As Guid = Guid.Empty
    Private Joystick As Joystick
    Private WithEvents JoystickWorker As New BackgroundWorker()
    Private DI As New DirectInput()
    Private AP As New AudioPlayer

    Private theData As New List(Of JoyInfo)
    Private Class JoyInfo
        Public Property Milliseconds As Long
        Public Property JoyState As JoystickState
    End Class
    Public Sub New(ByVal SongFile As String)
        For Each deviceInstance In DI.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices)
            joystickGuid = deviceInstance.InstanceGuid
            Exit For
        Next
        If joystickGuid = Guid.Empty Then
            Console.WriteLine("No joystick connected.")
            Return
        End If
        JoystickWorker.WorkerSupportsCancellation = True
        JoystickWorker.WorkerReportsProgress = True
        AddHandler JoystickWorker.DoWork, AddressOf JoystickWorker_DoWork
        AP.LoadSong(SongFile)
    End Sub

    Public Sub JoystickWorker_DoWork(sender As Object, e As DoWorkEventArgs)
        Dim s As JoystickState
        Dim lastSlider As Integer = 0
        Dim thisSlider As Integer = 0
        Dim thisButton8 As Boolean = False
        Dim lastButton8 As Boolean = False
        Dim StoreIt As Boolean = False
        While Not JoystickWorker.CancellationPending
            theData.Add(New JoyInfo With {.Milliseconds = AP.getMillies, .JoyState = Joystick.GetCurrentState})
            Threading.Thread.Sleep(10)
        End While
    End Sub


    Public Sub StartRecording()
        ' Initialize joystick
        Joystick = New Joystick(DI, joystickGuid)
        Console.WriteLine("Joystick found: " & Joystick.Information.ProductName)
        Joystick.Acquire()
        AP.Play()
        JoystickWorker.RunWorkerAsync()
    End Sub

End Class



'Public Class oldJoystickRecorder
'    Private DirectInputInstance As New DirectInput()
'    Private JoystickDevice As Joystick

'    Private Worker As BackgroundWorker
'    Private Worker2 As BackgroundWorker
'    Private IsRecording As Boolean = False
'    Private StartTicks As Long
'    Private lastSlider As Integer = 0
'    Public Sub New(joystickGuid As Guid)
'        JoystickDevice = New Joystick(DirectInputInstance, joystickGuid)
'        JoystickDevice.Acquire()

'        Worker = New BackgroundWorker()
'        AddHandler Worker.DoWork, AddressOf Worker_DoWork
'        AddHandler Worker.RunWorkerCompleted, AddressOf Worker_Completed
'        Worker2 = New BackgroundWorker()
'        AddHandler Worker2.DoWork, AddressOf Worker2_DoWork
'        AddHandler Worker2.RunWorkerCompleted, AddressOf Worker2_Completed
'    End Sub

'    Public Sub StartRecording(StartTicks As Long)
'        IsRecording = True
'        'Worker.RunWorkerAsync()
'        Worker2.RunWorkerAsync()
'        Me.StartTicks = StartTicks
'    End Sub

'    Private Sub Worker2_DoWork(sender As Object, e As DoWorkEventArgs)
'        Dim s As JoystickState
'        Dim lastSlider As Integer = 0
'        Dim thisSlider As Integer = 0
'        Dim thisButton8 As Boolean = False
'        Dim lastButton8 As Boolean = False
'        Dim StoreIt As Boolean = False
'        While IsRecording
'            '10,922    32,767
'            s = JoystickDevice.GetCurrentState
'            StoreIt = False
'            thisButton8 = s.Buttons(8)
'            If Not (thisButton8 = lastButton8) Then
'                StoreIt = True
'                lastButton8 = thisButton8
'            End If
'            If thisButton8 Then
'                If s.Sliders(0) < 21845 Then
'                    thisSlider = 1
'                ElseIf s.Sliders(0) > 43689 Then
'                    thisSlider = 3
'                Else
'                    thisSlider = 2
'                End If
'                If Not (thisSlider = lastSlider) Then
'                    StoreIt = True
'                End If
'            End If
'            'if button8 changed, or the slider changed while depressing button 8
'            If StoreIt Then
'                lastSlider = thisSlider
'                RecordingData2.Add(New RecordingClass2() With {._SliderTristate = thisSlider, ._TheTicks = Date.Now.Ticks, ._Button8 = lastButton8})
'            End If
'            Threading.Thread.Sleep(10)
'        End While
'    End Sub

'    Private Sub Worker2_Completed(sender As Object, e As RunWorkerCompletedEventArgs)
'        If e.Error IsNot Nothing Then
'            Console.WriteLine("Error: " & e.Error.Message)
'        End If
'    End Sub

'    Private Sub Worker_DoWork(sender As Object, e As DoWorkEventArgs)

'        While IsRecording
'            RecordingData.Add(New RecordingClass(Date.Now.Ticks, JoystickDevice.GetCurrentState))
'            ' Pause for 1/8 of a second
'            Threading.Thread.Sleep(125)
'        End While
'    End Sub

'    Private Sub Worker_Completed(sender As Object, e As RunWorkerCompletedEventArgs)
'        If e.Error IsNot Nothing Then
'            Console.WriteLine("Error: " & e.Error.Message)
'        End If
'    End Sub

'    Protected Overrides Sub Finalize()
'        JoystickDevice.Unacquire()
'        MyBase.Finalize()
'    End Sub
'End Class
