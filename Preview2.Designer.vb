<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Preview2
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
        pbHead = New PictureBox()
        pbLeft = New PictureBox()
        pbRight = New PictureBox()
        Button1 = New Button()
        Panel1 = New Panel()
        Panel2 = New Panel()
        NumericUpDown1 = New NumericUpDown()
        Button2 = New Button()
        Button3 = New Button()
        Button4 = New Button()
        CType(pbHead, ComponentModel.ISupportInitialize).BeginInit()
        CType(pbLeft, ComponentModel.ISupportInitialize).BeginInit()
        CType(pbRight, ComponentModel.ISupportInitialize).BeginInit()
        CType(NumericUpDown1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' pbHead
        ' 
        pbHead.Location = New Point(611, 451)
        pbHead.Name = "pbHead"
        pbHead.Size = New Size(284, 111)
        pbHead.TabIndex = 0
        pbHead.TabStop = False
        ' 
        ' pbLeft
        ' 
        pbLeft.Location = New Point(611, 568)
        pbLeft.Name = "pbLeft"
        pbLeft.Size = New Size(139, 63)
        pbLeft.TabIndex = 1
        pbLeft.TabStop = False
        ' 
        ' pbRight
        ' 
        pbRight.Location = New Point(756, 568)
        pbRight.Name = "pbRight"
        pbRight.Size = New Size(139, 63)
        pbRight.TabIndex = 2
        pbRight.TabStop = False
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(21, 594)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 23)
        Button1.TabIndex = 3
        Button1.Text = "Start"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Panel1
        ' 
        Panel1.BackColor = Color.Black
        Panel1.Location = New Point(7, 5)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(781, 220)
        Panel1.TabIndex = 4
        ' 
        ' Panel2
        ' 
        Panel2.BackColor = Color.Black
        Panel2.Location = New Point(7, 225)
        Panel2.Name = "Panel2"
        Panel2.Size = New Size(781, 220)
        Panel2.TabIndex = 5
        ' 
        ' NumericUpDown1
        ' 
        NumericUpDown1.Location = New Point(808, 215)
        NumericUpDown1.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        NumericUpDown1.Minimum = New Decimal(New Integer() {10, 0, 0, 0})
        NumericUpDown1.Name = "NumericUpDown1"
        NumericUpDown1.Size = New Size(87, 23)
        NumericUpDown1.TabIndex = 6
        NumericUpDown1.Value = New Decimal(New Integer() {200, 0, 0, 0})
        ' 
        ' Button2
        ' 
        Button2.Location = New Point(119, 469)
        Button2.Name = "Button2"
        Button2.Size = New Size(117, 23)
        Button2.TabIndex = 7
        Button2.Text = "Play / Pause"
        Button2.UseVisualStyleBackColor = True
        ' 
        ' Button3
        ' 
        Button3.Location = New Point(254, 471)
        Button3.Name = "Button3"
        Button3.Size = New Size(75, 23)
        Button3.TabIndex = 8
        Button3.Text = "Back 5 seconds"
        Button3.UseVisualStyleBackColor = True
        ' 
        ' Button4
        ' 
        Button4.Location = New Point(350, 471)
        Button4.Name = "Button4"
        Button4.Size = New Size(115, 23)
        Button4.TabIndex = 9
        Button4.Text = "Modify Delay"
        Button4.UseVisualStyleBackColor = True
        ' 
        ' Preview2
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(907, 647)
        Controls.Add(Button4)
        Controls.Add(Button3)
        Controls.Add(Button2)
        Controls.Add(NumericUpDown1)
        Controls.Add(Panel2)
        Controls.Add(Panel1)
        Controls.Add(Button1)
        Controls.Add(pbRight)
        Controls.Add(pbLeft)
        Controls.Add(pbHead)
        Name = "Preview2"
        Text = "Preview2"
        CType(pbHead, ComponentModel.ISupportInitialize).EndInit()
        CType(pbLeft, ComponentModel.ISupportInitialize).EndInit()
        CType(pbRight, ComponentModel.ISupportInitialize).EndInit()
        CType(NumericUpDown1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents pbHead As PictureBox
    Friend WithEvents pbLeft As PictureBox
    Friend WithEvents pbRight As PictureBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents NumericUpDown1 As NumericUpDown
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
End Class
