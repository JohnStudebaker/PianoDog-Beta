Imports System.IO

Public Class Preview3

    Private _songData As New DataTable
    Private _baseFile As String
    Private _CurPos As Integer

    Public Sub Setup()

        _songData.Columns.Add("Timestamp", GetType(Long))
        _songData.Columns.Add("Mouth", GetType(Integer))
        _songData.Columns.Add("Head", GetType(Integer))
        _songData.Columns.Add("Neck", GetType(Integer))
        _songData.Columns.Add("LeftH", GetType(Integer))
        _songData.Columns.Add("LeftV", GetType(Integer))
        _songData.Columns.Add("RightH", GetType(Integer))
        _songData.Columns.Add("RightV", GetType(Integer))

        Using txIn As StreamReader = IO.File.OpenText($"{_baseFile}_final_output.csv")
            While Not txIn.EndOfStream
                Dim line As String = txIn.ReadLine()
                Dim parts As String() = line.Split(","c)
                If parts.Length = 8 Then
                    Dim timestamp As Long = CLng(Double.Parse(parts(0))) ' Convert to milliseconds
                    Dim mouth As Integer = Integer.Parse(parts(1))
                    Dim head As Integer = Integer.Parse(parts(2))
                    Dim neck As Integer = Integer.Parse(parts(3))
                    Dim leftH As Integer = Integer.Parse(parts(4))
                    Dim leftV As Integer = Integer.Parse(parts(5))
                    Dim rightH As Integer = Integer.Parse(parts(6))
                    Dim rightV As Integer = Integer.Parse(parts(7))
                    _songData.Rows.Add(timestamp, mouth, head, neck, leftH, leftV, rightH, rightV)
                End If
            End While
        End Using
        ' 3. Bind the DataTable to your DataGridView
        _CurPos = 0
        DataGridView1.Columns.Clear()
        DataGridView1.AutoGenerateColumns = True
        DataGridView1.DataSource = _songData
        AxWindowsMediaPlayer1.URL = $"{_baseFile}.avi"
        AxWindowsMediaPlayer1.Ctlcontrols.play()
        Timer1.Enabled = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.Filter = "AVI Files (*.avi)|*.avi"
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            _baseFile = IO.Path.Combine(IO.Path.GetDirectoryName(OpenFileDialog1.FileName), IO.Path.GetFileNameWithoutExtension(OpenFileDialog1.FileName))
            Setup()
        End If
    End Sub

    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If _CurPos >= DataGridView1.Rows.Count Then
            Timer1.Enabled = False
            Return
        End If
        Dim currentTime As Long = AxWindowsMediaPlayer1.Ctlcontrols.currentPosition * 1000 ' Convert to milliseconds
        Dim row As DataRow = _songData.Rows(_CurPos)
        Dim timestamp As Long = CLng(row("Timestamp"))
        Console.WriteLine(row("mouth"))
        If currentTime >= timestamp Then
            DataGridView1.ClearSelection()
            DataGridView1.Rows(_CurPos).Selected = True
            DataGridView1.FirstDisplayedScrollingRowIndex = _CurPos
            _CurPos += 1
        End If
    End Sub
End Class