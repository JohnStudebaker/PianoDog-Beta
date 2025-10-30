<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Showtime
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
        Timer1 = New Timer(components)
        PictureBox1 = New PictureBox()
        PictureBox3 = New PictureBox()
        Panel1 = New Panel()
        Label1 = New Label()
        TextBox1 = New TextBox()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        CType(PictureBox3, ComponentModel.ISupportInitialize).BeginInit()
        Panel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' Timer1
        ' 
        ' 
        ' PictureBox1
        ' 
        PictureBox1.BackColor = Color.Black
        PictureBox1.Location = New Point(337, 0)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(534, 611)
        PictureBox1.TabIndex = 0
        PictureBox1.TabStop = False
        ' 
        ' PictureBox3
        ' 
        PictureBox3.BackColor = Color.Black
        PictureBox3.Image = My.Resources.Resources.PianoDog2test
        PictureBox3.Location = New Point(877, 0)
        PictureBox3.Name = "PictureBox3"
        PictureBox3.Size = New Size(337, 599)
        PictureBox3.SizeMode = PictureBoxSizeMode.Zoom
        PictureBox3.TabIndex = 3
        PictureBox3.TabStop = False
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(Label1)
        Panel1.Controls.Add(TextBox1)
        Panel1.Location = New Point(2, 0)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(319, 611)
        Panel1.TabIndex = 4
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label1.ForeColor = Color.LightGray
        Label1.Location = New Point(31, 549)
        Label1.Name = "Label1"
        Label1.Size = New Size(474, 65)
        Label1.TabIndex = 1
        Label1.Text = "https://PianoDog.org"
        ' 
        ' TextBox1
        ' 
        TextBox1.BackColor = Color.Black
        TextBox1.BorderStyle = BorderStyle.None
        TextBox1.Enabled = False
        TextBox1.Font = New Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        TextBox1.ForeColor = Color.WhiteSmoke
        TextBox1.Location = New Point(22, 36)
        TextBox1.Multiline = True
        TextBox1.Name = "TextBox1"
        TextBox1.ReadOnly = True
        TextBox1.Size = New Size(277, 529)
        TextBox1.TabIndex = 0
        TextBox1.Text = ChrW(8220) & "PianoDog brings STEAM to life — blending coding, engineering, art, and music into one unforgettable experience. It’s not augmented reality; it’s hands-on reality built by students’ own hands." & ChrW(8221)
        ' 
        ' Showtime
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.Black
        ClientSize = New Size(1211, 611)
        ControlBox = False
        Controls.Add(Panel1)
        Controls.Add(PictureBox3)
        Controls.Add(PictureBox1)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        MaximizeBox = False
        MinimizeBox = False
        Name = "Showtime"
        ShowIcon = False
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        CType(PictureBox3, ComponentModel.ISupportInitialize).EndInit()
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Timer1 As Timer
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents PictureBox3 As PictureBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label1 As Label
End Class
