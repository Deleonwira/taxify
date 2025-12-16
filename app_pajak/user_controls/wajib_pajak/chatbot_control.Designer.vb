<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class chatbot_control
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim CustomizableEdges4 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges5 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges1 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges2 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges3 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges6 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges7 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges12 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges13 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges8 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges9 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges10 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges11 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        pnlHeader = New Guna.UI2.WinForms.Guna2Panel()
        lblTitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        btnClose = New Guna.UI2.WinForms.Guna2Button()
        picIcon = New Guna.UI2.WinForms.Guna2CirclePictureBox()
        pnlChatArea = New Guna.UI2.WinForms.Guna2Panel()
        pnlYesNo = New Guna.UI2.WinForms.Guna2Panel()
        btnNo = New Guna.UI2.WinForms.Guna2Button()
        btnYes = New Guna.UI2.WinForms.Guna2Button()
        pnlHeader.SuspendLayout()
        CType(picIcon, ComponentModel.ISupportInitialize).BeginInit()
        pnlYesNo.SuspendLayout()
        SuspendLayout()
        ' 
        ' pnlHeader
        ' 
        pnlHeader.BackColor = Color.FromArgb(CByte(106), CByte(90), CByte(232))
        pnlHeader.Controls.Add(lblTitle)
        pnlHeader.Controls.Add(btnClose)
        pnlHeader.Controls.Add(picIcon)
        pnlHeader.CustomizableEdges = CustomizableEdges4
        pnlHeader.Dock = DockStyle.Top
        pnlHeader.FillColor = Color.FromArgb(CByte(106), CByte(90), CByte(232))
        pnlHeader.Location = New Point(0, 0)
        pnlHeader.Margin = New Padding(0)
        pnlHeader.Name = "pnlHeader"
        pnlHeader.ShadowDecoration.CustomizableEdges = CustomizableEdges5
        pnlHeader.Size = New Size(320, 45)
        pnlHeader.TabIndex = 0
        ' 
        ' lblTitle
        ' 
        lblTitle.BackColor = Color.Transparent
        lblTitle.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTitle.ForeColor = Color.White
        lblTitle.Location = New Point(49, 11)
        lblTitle.Margin = New Padding(0)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(121, 23)
        lblTitle.TabIndex = 2
        lblTitle.Text = "Diagnosa Pajak"
        ' 
        ' btnClose
        ' 
        btnClose.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnClose.BackColor = Color.Transparent
        btnClose.BorderRadius = 10
        btnClose.CustomizableEdges = CustomizableEdges1
        btnClose.DisabledState.BorderColor = Color.DarkGray
        btnClose.DisabledState.CustomBorderColor = Color.DarkGray
        btnClose.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnClose.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnClose.FillColor = Color.Transparent
        btnClose.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        btnClose.ForeColor = Color.White
        btnClose.Image = My.Resources.Resources.circle_xmark__1_
        btnClose.Location = New Point(285, 8)
        btnClose.Margin = New Padding(0)
        btnClose.Name = "btnClose"
        btnClose.ShadowDecoration.CustomizableEdges = CustomizableEdges2
        btnClose.Size = New Size(30, 30)
        btnClose.TabIndex = 1
        btnClose.TextOffset = New Point(0, -2)
        ' 
        ' picIcon
        ' 
        picIcon.BackColor = Color.Transparent
        picIcon.ImageRotate = 0F
        picIcon.Location = New Point(10, 7)
        picIcon.Margin = New Padding(0)
        picIcon.Name = "picIcon"
        picIcon.ShadowDecoration.CustomizableEdges = CustomizableEdges3
        picIcon.Size = New Size(30, 30)
        picIcon.SizeMode = PictureBoxSizeMode.CenterImage
        picIcon.TabIndex = 0
        picIcon.TabStop = False
        ' 
        ' pnlChatArea
        ' 
        pnlChatArea.AutoScroll = True
        pnlChatArea.BackColor = Color.White
        pnlChatArea.CustomizableEdges = CustomizableEdges6
        pnlChatArea.Dock = DockStyle.Fill
        pnlChatArea.FillColor = Color.White
        pnlChatArea.Location = New Point(0, 45)
        pnlChatArea.Margin = New Padding(0)
        pnlChatArea.Name = "pnlChatArea"
        pnlChatArea.ShadowDecoration.CustomizableEdges = CustomizableEdges7
        pnlChatArea.Size = New Size(320, 305)
        pnlChatArea.TabIndex = 1
        ' 
        ' pnlYesNo
        ' 
        pnlYesNo.BackColor = Color.White
        pnlYesNo.Controls.Add(btnNo)
        pnlYesNo.Controls.Add(btnYes)
        pnlYesNo.CustomizableEdges = CustomizableEdges12
        pnlYesNo.Dock = DockStyle.Bottom
        pnlYesNo.FillColor = Color.White
        pnlYesNo.Location = New Point(0, 350)
        pnlYesNo.Margin = New Padding(0)
        pnlYesNo.Name = "pnlYesNo"
        pnlYesNo.ShadowDecoration.CustomizableEdges = CustomizableEdges13
        pnlYesNo.Size = New Size(320, 50)
        pnlYesNo.TabIndex = 2
        ' 
        ' btnNo
        ' 
        btnNo.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnNo.BorderRadius = 15
        btnNo.CustomizableEdges = CustomizableEdges8
        btnNo.DisabledState.BorderColor = Color.DarkGray
        btnNo.DisabledState.CustomBorderColor = Color.DarkGray
        btnNo.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnNo.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnNo.FillColor = Color.FromArgb(CByte(106), CByte(90), CByte(232))
        btnNo.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        btnNo.ForeColor = Color.White
        btnNo.Location = New Point(231, 10)
        btnNo.Margin = New Padding(0)
        btnNo.Name = "btnNo"
        btnNo.ShadowDecoration.CustomizableEdges = CustomizableEdges9
        btnNo.Size = New Size(79, 30)
        btnNo.TabIndex = 1
        btnNo.Text = "Tidak"
        btnNo.Visible = False
        ' 
        ' btnYes
        ' 
        btnYes.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnYes.BorderRadius = 15
        btnYes.CustomizableEdges = CustomizableEdges10
        btnYes.DisabledState.BorderColor = Color.DarkGray
        btnYes.DisabledState.CustomBorderColor = Color.DarkGray
        btnYes.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnYes.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnYes.FillColor = Color.FromArgb(CByte(106), CByte(90), CByte(232))
        btnYes.Font = New Font("Segoe UI", 8F, FontStyle.Bold)
        btnYes.ForeColor = Color.White
        btnYes.Location = New Point(119, 10)
        btnYes.Margin = New Padding(0)
        btnYes.Name = "btnYes"
        btnYes.ShadowDecoration.CustomizableEdges = CustomizableEdges11
        btnYes.Size = New Size(90, 30)
        btnYes.TabIndex = 0
        btnYes.Text = "Ya"
        btnYes.Visible = False
        ' 
        ' chatbot_control
        ' 
        AutoScaleMode = AutoScaleMode.None
        BackColor = Color.White
        Controls.Add(pnlChatArea)
        Controls.Add(pnlYesNo)
        Controls.Add(pnlHeader)
        Margin = New Padding(0)
        MaximumSize = New Size(380, 400)
        MinimumSize = New Size(320, 400)
        Name = "chatbot_control"
        Size = New Size(320, 400)
        pnlHeader.ResumeLayout(False)
        pnlHeader.PerformLayout()
        CType(picIcon, ComponentModel.ISupportInitialize).EndInit()
        pnlYesNo.ResumeLayout(False)
        ResumeLayout(False)

    End Sub

    Friend WithEvents pnlHeader As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents picIcon As Guna.UI2.WinForms.Guna2CirclePictureBox
    Friend WithEvents btnClose As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents lblTitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents pnlChatArea As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents pnlYesNo As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents btnYes As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnNo As Guna.UI2.WinForms.Guna2Button
End Class