<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class JoystickRecorderForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Button1 = New Button()
        Button2 = New Button()
        SaveFileDialog1 = New SaveFileDialog()
        Button3 = New Button()
        Button4 = New Button()
        Button5 = New Button()
        Button6 = New Button()
        Panel1 = New Panel()
        Button7 = New Button()
        Timer1 = New Timer(components)
        lbMouth = New Label()
        lbHeadNeck = New Label()
        lbRight = New Label()
        lbLeft = New Label()
        lbFinal = New Label()
        Button8 = New Button()
        Button9 = New Button()
        lbMHN = New Label()
        SuspendLayout()
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(75, 342)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 23)
        Button1.TabIndex = 0
        Button1.Text = "Start"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(547, 251)
        Button2.Name = "Button2"
        Button2.Size = New Size(241, 23)
        Button2.TabIndex = 1
        Button2.Text = "Stop Head && Neck"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(547, 222)
        Button3.Name = "Button3"
        Button3.Size = New Size(241, 23)
        Button3.TabIndex = 2
        Button3.Text = "Stop Mouth"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' Button4
        ' 
        Button4.Location = New Point(547, 345)
        Button4.Name = "Button4"
        Button4.Size = New Size(241, 23)
        Button4.TabIndex = 3
        Button4.Text = "Stop Left"
        Button4.UseVisualStyleBackColor = True
        ' 
        ' Button5
        ' 
        Button5.Location = New Point(547, 316)
        Button5.Name = "Button5"
        Button5.Size = New Size(241, 23)
        Button5.TabIndex = 4
        Button5.Text = "Stop Right"
        Button5.UseVisualStyleBackColor = True
        ' 
        ' Button6
        ' 
        Button6.Location = New Point(75, 371)
        Button6.Name = "Button6"
        Button6.Size = New Size(75, 23)
        Button6.TabIndex = 5
        Button6.Text = "Merge"
        Button6.UseVisualStyleBackColor = True
        ' 
        ' Panel1
        ' 
        Panel1.Location = New Point(10, 11)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(1201, 205)
        Panel1.TabIndex = 6
        ' 
        ' Button7
        ' 
        Button7.Location = New Point(466, 238)
        Button7.Name = "Button7"
        Button7.Size = New Size(75, 23)
        Button7.TabIndex = 7
        Button7.Text = "do not useBack 10/5"
        Button7.UseVisualStyleBackColor = True
        ' 
        ' Timer1
        ' 
        ' 
        ' lbMouth
        ' 
        lbMouth.AutoSize = True
        lbMouth.Location = New Point(807, 226)
        lbMouth.Name = "lbMouth"
        lbMouth.Size = New Size(41, 15)
        lbMouth.TabIndex = 8
        lbMouth.Text = "Label1"
        ' 
        ' lbHeadNeck
        ' 
        lbHeadNeck.AutoSize = True
        lbHeadNeck.Location = New Point(807, 255)
        lbHeadNeck.Name = "lbHeadNeck"
        lbHeadNeck.Size = New Size(41, 15)
        lbHeadNeck.TabIndex = 9
        lbHeadNeck.Text = "Label2"
        ' 
        ' lbRight
        ' 
        lbRight.AutoSize = True
        lbRight.Location = New Point(807, 320)
        lbRight.Name = "lbRight"
        lbRight.Size = New Size(41, 15)
        lbRight.TabIndex = 10
        lbRight.Text = "Label3"
        ' 
        ' lbLeft
        ' 
        lbLeft.AutoSize = True
        lbLeft.Location = New Point(807, 349)
        lbLeft.Name = "lbLeft"
        lbLeft.Size = New Size(41, 15)
        lbLeft.TabIndex = 11
        lbLeft.Text = "Label4"
        ' 
        ' lbFinal
        ' 
        lbFinal.AutoSize = True
        lbFinal.Location = New Point(807, 403)
        lbFinal.Name = "lbFinal"
        lbFinal.Size = New Size(41, 15)
        lbFinal.TabIndex = 12
        lbFinal.Text = "Label4"
        ' 
        ' Button8
        ' 
        Button8.Location = New Point(547, 403)
        Button8.Name = "Button8"
        Button8.Size = New Size(241, 23)
        Button8.TabIndex = 13
        Button8.Text = "Stop Discard"
        Button8.UseVisualStyleBackColor = True
        ' 
        ' Button9
        ' 
        Button9.Location = New Point(547, 374)
        Button9.Name = "Button9"
        Button9.Size = New Size(241, 23)
        Button9.TabIndex = 14
        Button9.Text = "Stop Mouth, Head and Neck"
        Button9.UseVisualStyleBackColor = True
        ' 
        ' lbMHN
        ' 
        lbMHN.AutoSize = True
        lbMHN.Location = New Point(807, 378)
        lbMHN.Name = "lbMHN"
        lbMHN.Size = New Size(41, 15)
        lbMHN.TabIndex = 15
        lbMHN.Text = "Label4"
        ' 
        ' JoystickRecorderForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1223, 450)
        Controls.Add(lbMHN)
        Controls.Add(Button9)
        Controls.Add(Button8)
        Controls.Add(lbFinal)
        Controls.Add(lbLeft)
        Controls.Add(lbRight)
        Controls.Add(lbHeadNeck)
        Controls.Add(lbMouth)
        Controls.Add(Button7)
        Controls.Add(Panel1)
        Controls.Add(Button6)
        Controls.Add(Button5)
        Controls.Add(Button4)
        Controls.Add(Button3)
        Controls.Add(Button2)
        Controls.Add(Button1)
        Name = "JoystickRecorderForm"
        Text = "JoystickRecorderForm"
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button5 As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Button7 As Button
    Friend WithEvents Timer1 As Timer
    Friend WithEvents lbMouth As Label
    Friend WithEvents lbHeadNeck As Label
    Friend WithEvents lbRight As Label
    Friend WithEvents lbLeft As Label
    Friend WithEvents lbFinal As Label
    Friend WithEvents Button8 As Button
    Friend WithEvents Button9 As Button
    Friend WithEvents lbMHN As Label
End Class
