<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Button1 = New Button()
        lbWav = New Label()
        OpenFileDialog1 = New OpenFileDialog()
        Button2 = New Button()
        Button3 = New Button()
        Button4 = New Button()
        lbFinal = New Label()
        Button6 = New Button()
        Button8 = New Button()
        Button5 = New Button()
        Button7 = New Button()
        Button9 = New Button()
        Button10 = New Button()
        SuspendLayout()
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(26, 37)
        Button1.Name = "Button1"
        Button1.Size = New Size(219, 23)
        Button1.TabIndex = 0
        Button1.Text = "Click Here to Select Song"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' lbWav
        ' 
        lbWav.AutoSize = True
        lbWav.Location = New Point(26, 73)
        lbWav.Name = "lbWav"
        lbWav.Size = New Size(41, 15)
        lbWav.TabIndex = 1
        lbWav.Text = "Label1"
        ' 
        ' OpenFileDialog1
        ' 
        OpenFileDialog1.FileName = "OpenFileDialog1"
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(640, 396)
        Button2.Name = "Button2"
        Button2.Size = New Size(122, 23)
        Button2.TabIndex = 7
        Button2.Text = "Record Inputs"
        Button2.UseVisualStyleBackColor = True
        Button2.Visible = False
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(619, 415)
        Button3.Name = "Button3"
        Button3.Size = New Size(169, 23)
        Button3.TabIndex = 8
        Button3.Text = "Process Datafiles"
        Button3.UseVisualStyleBackColor = True
        Button3.Visible = False
        ' 
        ' Button4
        ' 
        Button4.Location = New Point(26, 267)
        Button4.Name = "Button4"
        Button4.Size = New Size(75, 23)
        Button4.TabIndex = 9
        Button4.Text = "Preview"
        Button4.UseVisualStyleBackColor = True
        ' 
        ' lbFinal
        ' 
        lbFinal.AutoSize = True
        lbFinal.Location = New Point(26, 88)
        lbFinal.Name = "lbFinal"
        lbFinal.Size = New Size(41, 15)
        lbFinal.TabIndex = 11
        lbFinal.Text = "Label1"
        ' 
        ' Button6
        ' 
        Button6.Location = New Point(26, 165)
        Button6.Name = "Button6"
        Button6.Size = New Size(139, 23)
        Button6.TabIndex = 12
        Button6.Text = "Record Joystick"
        Button6.UseVisualStyleBackColor = True
        ' 
        ' Button8
        ' 
        Button8.Location = New Point(304, 267)
        Button8.Name = "Button8"
        Button8.Size = New Size(75, 23)
        Button8.TabIndex = 14
        Button8.Text = "Run"
        Button8.UseVisualStyleBackColor = True
        ' 
        ' Button5
        ' 
        Button5.Location = New Point(136, 267)
        Button5.Name = "Button5"
        Button5.Size = New Size(75, 23)
        Button5.TabIndex = 15
        Button5.Text = "Debug"
        Button5.UseVisualStyleBackColor = True
        ' 
        ' Button7
        ' 
        Button7.Location = New Point(259, 370)
        Button7.Name = "Button7"
        Button7.Size = New Size(75, 23)
        Button7.TabIndex = 16
        Button7.Text = "Autorun"
        Button7.UseVisualStyleBackColor = True
        ' 
        ' Button9
        ' 
        Button9.Location = New Point(561, 210)
        Button9.Name = "Button9"
        Button9.Size = New Size(75, 23)
        Button9.TabIndex = 17
        Button9.Text = "Button9"
        Button9.UseVisualStyleBackColor = True
        ' 
        ' Button10
        ' 
        Button10.Location = New Point(409, 88)
        Button10.Name = "Button10"
        Button10.Size = New Size(75, 23)
        Button10.TabIndex = 18
        Button10.Text = "YOLO"
        Button10.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(Button10)
        Controls.Add(Button9)
        Controls.Add(Button7)
        Controls.Add(Button5)
        Controls.Add(Button8)
        Controls.Add(Button6)
        Controls.Add(lbFinal)
        Controls.Add(Button4)
        Controls.Add(Button3)
        Controls.Add(Button2)
        Controls.Add(lbWav)
        Controls.Add(Button1)
        Name = "Form1"
        Text = "Form1"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents lbWav As Label
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents lbFinal As Label
    Friend WithEvents Button6 As Button
    Friend WithEvents Button8 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents Button7 As Button
    Friend WithEvents Button9 As Button
    Friend WithEvents Button10 As Button

End Class
