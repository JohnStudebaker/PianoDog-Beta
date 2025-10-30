<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Preview3
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Preview3))
        DataGridView1 = New DataGridView()
        Millis = New DataGridViewTextBoxColumn()
        Mouth = New DataGridViewTextBoxColumn()
        Head = New DataGridViewTextBoxColumn()
        Neck = New DataGridViewTextBoxColumn()
        LeftH = New DataGridViewTextBoxColumn()
        LeftV = New DataGridViewTextBoxColumn()
        RightH = New DataGridViewTextBoxColumn()
        RightV = New DataGridViewTextBoxColumn()
        OpenFileDialog1 = New OpenFileDialog()
        Button1 = New Button()
        AxWindowsMediaPlayer1 = New AxWMPLib.AxWindowsMediaPlayer()
        Timer1 = New Timer(components)
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        CType(AxWindowsMediaPlayer1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' DataGridView1
        ' 
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridView1.Columns.AddRange(New DataGridViewColumn() {Millis, Mouth, Head, Neck, LeftH, LeftV, RightH, RightV})
        DataGridView1.Location = New Point(12, 12)
        DataGridView1.Name = "DataGridView1"
        DataGridView1.Size = New Size(849, 426)
        DataGridView1.TabIndex = 0
        ' 
        ' Millis
        ' 
        Millis.HeaderText = "Milliseconds"
        Millis.Name = "Millis"
        Millis.ReadOnly = True
        ' 
        ' Mouth
        ' 
        Mouth.HeaderText = "Mouth"
        Mouth.Name = "Mouth"
        ' 
        ' Head
        ' 
        Head.HeaderText = "Head"
        Head.Name = "Head"
        ' 
        ' Neck
        ' 
        Neck.HeaderText = "Neck"
        Neck.Name = "Neck"
        ' 
        ' LeftH
        ' 
        LeftH.HeaderText = "Left Horizontal"
        LeftH.Name = "LeftH"
        ' 
        ' LeftV
        ' 
        LeftV.HeaderText = "Left Vertical"
        LeftV.Name = "LeftV"
        ' 
        ' RightH
        ' 
        RightH.HeaderText = "Right Horizontal"
        RightH.Name = "RightH"
        ' 
        ' RightV
        ' 
        RightV.HeaderText = "Right Vertical"
        RightV.Name = "RightV"
        ' 
        ' OpenFileDialog1
        ' 
        OpenFileDialog1.FileName = "OpenFileDialog1"
        ' 
        ' Button1
        ' 
        Button1.Location = New Point(1021, 44)
        Button1.Name = "Button1"
        Button1.Size = New Size(75, 23)
        Button1.TabIndex = 1
        Button1.Text = "Button1"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' AxWindowsMediaPlayer1
        ' 
        AxWindowsMediaPlayer1.Enabled = True
        AxWindowsMediaPlayer1.Location = New Point(867, 73)
        AxWindowsMediaPlayer1.Name = "AxWindowsMediaPlayer1"
        AxWindowsMediaPlayer1.OcxState = CType(resources.GetObject("AxWindowsMediaPlayer1.OcxState"), AxHost.State)
        AxWindowsMediaPlayer1.Size = New Size(307, 365)
        AxWindowsMediaPlayer1.TabIndex = 2
        ' 
        ' Timer1
        ' 
        Timer1.Interval = 20
        ' 
        ' Preview3
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1186, 450)
        Controls.Add(AxWindowsMediaPlayer1)
        Controls.Add(Button1)
        Controls.Add(DataGridView1)
        Name = "Preview3"
        Text = "Preview3"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        CType(AxWindowsMediaPlayer1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Millis As DataGridViewTextBoxColumn
    Friend WithEvents Mouth As DataGridViewTextBoxColumn
    Friend WithEvents Head As DataGridViewTextBoxColumn
    Friend WithEvents Neck As DataGridViewTextBoxColumn
    Friend WithEvents LeftH As DataGridViewTextBoxColumn
    Friend WithEvents LeftV As DataGridViewTextBoxColumn
    Friend WithEvents RightH As DataGridViewTextBoxColumn
    Friend WithEvents RightV As DataGridViewTextBoxColumn
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents Button1 As Button
    Friend WithEvents AxWindowsMediaPlayer1 As AxWMPLib.AxWindowsMediaPlayer
    Friend WithEvents Timer1 As Timer
End Class
