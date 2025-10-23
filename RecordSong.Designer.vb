<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RecordSong
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
        bnRecMouth = New Button()
        bnStopMouth = New Button()
        bnRecHead = New Button()
        bnStopHead = New Button()
        bnRecNeck = New Button()
        bnStopNeck = New Button()
        bnRecLeft = New Button()
        bnStopLeft = New Button()
        bnRecRight = New Button()
        bnStopRight = New Button()
        SuspendLayout()
        ' 
        ' bnRecMouth
        ' 
        bnRecMouth.Location = New Point(39, 182)
        bnRecMouth.Name = "bnRecMouth"
        bnRecMouth.Size = New Size(106, 23)
        bnRecMouth.TabIndex = 0
        bnRecMouth.Text = "Record Mouth"
        bnRecMouth.UseVisualStyleBackColor = True
        ' 
        ' bnStopMouth
        ' 
        bnStopMouth.Location = New Point(39, 211)
        bnStopMouth.Name = "bnStopMouth"
        bnStopMouth.Size = New Size(106, 23)
        bnStopMouth.TabIndex = 1
        bnStopMouth.Text = "Stop Mouth"
        bnStopMouth.UseVisualStyleBackColor = True
        ' 
        ' bnRecHead
        ' 
        bnRecHead.Location = New Point(151, 182)
        bnRecHead.Name = "bnRecHead"
        bnRecHead.Size = New Size(106, 23)
        bnRecHead.TabIndex = 2
        bnRecHead.Text = "Record Head"
        bnRecHead.UseVisualStyleBackColor = True
        ' 
        ' bnStopHead
        ' 
        bnStopHead.Location = New Point(151, 211)
        bnStopHead.Name = "bnStopHead"
        bnStopHead.Size = New Size(106, 23)
        bnStopHead.TabIndex = 3
        bnStopHead.Text = "Stop Head"
        bnStopHead.UseVisualStyleBackColor = True
        ' 
        ' bnRecNeck
        ' 
        bnRecNeck.Location = New Point(263, 182)
        bnRecNeck.Name = "bnRecNeck"
        bnRecNeck.Size = New Size(106, 23)
        bnRecNeck.TabIndex = 4
        bnRecNeck.Text = "Record Neck"
        bnRecNeck.UseVisualStyleBackColor = True
        ' 
        ' bnStopNeck
        ' 
        bnStopNeck.Location = New Point(263, 211)
        bnStopNeck.Name = "bnStopNeck"
        bnStopNeck.Size = New Size(106, 23)
        bnStopNeck.TabIndex = 5
        bnStopNeck.Text = "Stop Neck"
        bnStopNeck.UseVisualStyleBackColor = True
        ' 
        ' bnRecLeft
        ' 
        bnRecLeft.Location = New Point(375, 182)
        bnRecLeft.Name = "bnRecLeft"
        bnRecLeft.Size = New Size(106, 23)
        bnRecLeft.TabIndex = 6
        bnRecLeft.Text = "Record Left"
        bnRecLeft.UseVisualStyleBackColor = True
        ' 
        ' bnStopLeft
        ' 
        bnStopLeft.Location = New Point(375, 211)
        bnStopLeft.Name = "bnStopLeft"
        bnStopLeft.Size = New Size(106, 23)
        bnStopLeft.TabIndex = 7
        bnStopLeft.Text = "Stop Left"
        bnStopLeft.UseVisualStyleBackColor = True
        ' 
        ' bnRecRight
        ' 
        bnRecRight.Location = New Point(487, 182)
        bnRecRight.Name = "bnRecRight"
        bnRecRight.Size = New Size(106, 23)
        bnRecRight.TabIndex = 8
        bnRecRight.Text = "Record Right"
        bnRecRight.UseVisualStyleBackColor = True
        ' 
        ' bnStopRight
        ' 
        bnStopRight.Location = New Point(487, 211)
        bnStopRight.Name = "bnStopRight"
        bnStopRight.Size = New Size(106, 23)
        bnStopRight.TabIndex = 9
        bnStopRight.Text = "Stop Right"
        bnStopRight.UseVisualStyleBackColor = True
        ' 
        ' RecordSong
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(648, 450)
        Controls.Add(bnStopRight)
        Controls.Add(bnRecRight)
        Controls.Add(bnStopLeft)
        Controls.Add(bnRecLeft)
        Controls.Add(bnStopNeck)
        Controls.Add(bnRecNeck)
        Controls.Add(bnStopHead)
        Controls.Add(bnRecHead)
        Controls.Add(bnStopMouth)
        Controls.Add(bnRecMouth)
        Name = "RecordSong"
        Text = "RecordSong"
        ResumeLayout(False)
    End Sub

    Friend WithEvents bnRecMouth As Button
    Friend WithEvents bnStopMouth As Button
    Friend WithEvents bnRecHead As Button
    Friend WithEvents bnStopHead As Button
    Friend WithEvents bnRecNeck As Button
    Friend WithEvents bnStopNeck As Button
    Friend WithEvents bnRecLeft As Button
    Friend WithEvents bnStopLeft As Button
    Friend WithEvents bnRecRight As Button
    Friend WithEvents bnStopRight As Button
End Class
