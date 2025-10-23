Public Class TimeWarp


    Public theMillis As Long
    Public theMouth As Boolean
    Public theHead As Boolean
    Public theNeck As Boolean
    Public theLeftH As Boolean
    Public theRightH As Boolean
    Public theLeftV As Boolean
    Public theRightV As Boolean
    Public theAction As String
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        saveVals()
        theAction = "Cancel"
        Me.Hide()
    End Sub


    Private Sub saveVals()
        theMillis = NumericUpDown1.Value
        theMouth = CheckBox1.Checked
        theHead = CheckBox2.Checked
        theNeck = CheckBox3.Checked
        theLeftH = CheckBox4.Checked
        theRightH = CheckBox5.Checked
        theLeftV = CheckBox6.Checked
        theRightV = CheckBox7.Checked
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        saveVals()
        theAction = "Subtract"
        Me.Hide()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        saveVals()
        theAction = "Add"
        Me.Hide()
    End Sub
End Class