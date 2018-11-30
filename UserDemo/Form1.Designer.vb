<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SiteField = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ScreennameField = New System.Windows.Forms.TextBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.PicLabel1 = New System.Windows.Forms.Label()
        Me.PicLabel2 = New System.Windows.Forms.Label()
        Me.PicLabel3 = New System.Windows.Forms.Label()
        Me.BackButton = New System.Windows.Forms.Button()
        Me.NextButton = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(25, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Site"
        '
        'SiteField
        '
        Me.SiteField.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SiteField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.SiteField.FormattingEnabled = True
        Me.SiteField.Items.AddRange(New Object() {"FurAffinity", "Weasyl"})
        Me.SiteField.Location = New System.Drawing.Point(88, 12)
        Me.SiteField.Name = "SiteField"
        Me.SiteField.Size = New System.Drawing.Size(482, 21)
        Me.SiteField.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Screen name"
        '
        'ScreennameField
        '
        Me.ScreennameField.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ScreennameField.Location = New System.Drawing.Point(88, 39)
        Me.ScreennameField.Name = "ScreennameField"
        Me.ScreennameField.Size = New System.Drawing.Size(482, 20)
        Me.ScreennameField.TabIndex = 3
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PictureBox1.Location = New System.Drawing.Point(12, 65)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(200, 150)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 4
        Me.PictureBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PictureBox2.Location = New System.Drawing.Point(218, 65)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(200, 150)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox2.TabIndex = 5
        Me.PictureBox2.TabStop = False
        '
        'PictureBox3
        '
        Me.PictureBox3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom), System.Windows.Forms.AnchorStyles)
        Me.PictureBox3.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PictureBox3.Location = New System.Drawing.Point(424, 65)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(200, 150)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox3.TabIndex = 6
        Me.PictureBox3.TabStop = False
        '
        'PicLabel1
        '
        Me.PicLabel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.PicLabel1.AutoEllipsis = True
        Me.PicLabel1.Location = New System.Drawing.Point(12, 218)
        Me.PicLabel1.Name = "PicLabel1"
        Me.PicLabel1.Size = New System.Drawing.Size(200, 37)
        Me.PicLabel1.TabIndex = 5
        Me.PicLabel1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'PicLabel2
        '
        Me.PicLabel2.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.PicLabel2.AutoEllipsis = True
        Me.PicLabel2.Location = New System.Drawing.Point(218, 218)
        Me.PicLabel2.Name = "PicLabel2"
        Me.PicLabel2.Size = New System.Drawing.Size(200, 37)
        Me.PicLabel2.TabIndex = 6
        Me.PicLabel2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'PicLabel3
        '
        Me.PicLabel3.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.PicLabel3.AutoEllipsis = True
        Me.PicLabel3.Location = New System.Drawing.Point(424, 218)
        Me.PicLabel3.Name = "PicLabel3"
        Me.PicLabel3.Size = New System.Drawing.Size(200, 37)
        Me.PicLabel3.TabIndex = 7
        Me.PicLabel3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'BackButton
        '
        Me.BackButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BackButton.Location = New System.Drawing.Point(12, 258)
        Me.BackButton.Name = "BackButton"
        Me.BackButton.Size = New System.Drawing.Size(75, 23)
        Me.BackButton.TabIndex = 8
        Me.BackButton.Text = "Back"
        Me.BackButton.UseVisualStyleBackColor = True
        '
        'NextButton
        '
        Me.NextButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NextButton.Location = New System.Drawing.Point(549, 258)
        Me.NextButton.Name = "NextButton"
        Me.NextButton.Size = New System.Drawing.Size(75, 23)
        Me.NextButton.TabIndex = 9
        Me.NextButton.Text = "Next"
        Me.NextButton.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(576, 12)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(48, 47)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Load"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(636, 293)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.NextButton)
        Me.Controls.Add(Me.BackButton)
        Me.Controls.Add(Me.PicLabel3)
        Me.Controls.Add(Me.PicLabel2)
        Me.Controls.Add(Me.PicLabel1)
        Me.Controls.Add(Me.PictureBox3)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.ScreennameField)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.SiteField)
        Me.Controls.Add(Me.Label1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents SiteField As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents ScreennameField As TextBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents PictureBox3 As PictureBox
    Friend WithEvents PicLabel1 As Label
    Friend WithEvents PicLabel2 As Label
    Friend WithEvents PicLabel3 As Label
    Friend WithEvents BackButton As Button
    Friend WithEvents NextButton As Button
    Friend WithEvents Button1 As Button
End Class
