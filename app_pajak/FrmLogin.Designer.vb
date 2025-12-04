<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmLogin
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim CustomizableEdges1 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges2 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges3 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges4 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges5 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges6 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges7 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges8 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        pnlMain = New Guna.UI2.WinForms.Guna2Panel()
        btnGoRegister = New Guna.UI2.WinForms.Guna2Button()
        lblRegisterPrompt = New Guna.UI2.WinForms.Guna2HtmlLabel()
        btnLogin = New Guna.UI2.WinForms.Guna2Button()
        Guna2TextBox2 = New Guna.UI2.WinForms.Guna2TextBox()
        Guna2TextBox1 = New Guna.UI2.WinForms.Guna2TextBox()
        lblSubtitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        lblTitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        pnlMain.SuspendLayout()
        SuspendLayout()
        ' 
        ' pnlMain
        ' 
        pnlMain.BackColor = Color.Transparent
        pnlMain.BorderColor = Color.FromArgb(CByte(220), CByte(220), CByte(225))
        pnlMain.BorderRadius = 24
        pnlMain.BorderThickness = 1
        pnlMain.Controls.Add(btnGoRegister)
        pnlMain.Controls.Add(lblRegisterPrompt)
        pnlMain.Controls.Add(btnLogin)
        pnlMain.Controls.Add(Guna2TextBox2)
        pnlMain.Controls.Add(Guna2TextBox1)
        pnlMain.Controls.Add(lblSubtitle)
        pnlMain.Controls.Add(lblTitle)
        pnlMain.CustomizableEdges = CustomizableEdges7
        pnlMain.FillColor = Color.White
        pnlMain.Location = New Point(200, 135)
        pnlMain.Name = "pnlMain"
        pnlMain.ShadowDecoration.Color = Color.FromArgb(CByte(0), CByte(0), CByte(0), CByte(20))
        pnlMain.ShadowDecoration.CustomizableEdges = CustomizableEdges8
        pnlMain.ShadowDecoration.Enabled = True
        pnlMain.Size = New Size(400, 440)
        pnlMain.TabIndex = 0
        ' 
        ' btnGoRegister
        ' 
        btnGoRegister.BackColor = Color.Transparent
        btnGoRegister.BorderRadius = 8
        btnGoRegister.CustomizableEdges = CustomizableEdges1
        btnGoRegister.DefaultAutoSize = True
        btnGoRegister.DisabledState.BorderColor = Color.DarkGray
        btnGoRegister.DisabledState.CustomBorderColor = Color.DarkGray
        btnGoRegister.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnGoRegister.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnGoRegister.FillColor = Color.Empty
        btnGoRegister.Font = New Font("Segoe UI", 9F)
        btnGoRegister.ForeColor = Color.FromArgb(CByte(0), CByte(122), CByte(255))
        btnGoRegister.HoverState.FillColor = Color.FromArgb(CByte(240), CByte(240), CByte(245))
        btnGoRegister.Location = New Point(167, 368)
        btnGoRegister.Name = "btnGoRegister"
        btnGoRegister.ShadowDecoration.CustomizableEdges = CustomizableEdges2
        btnGoRegister.Size = New Size(74, 27)
        btnGoRegister.TabIndex = 8
        btnGoRegister.Text = "Register"
        btnGoRegister.UseTransparentBackground = True
        ' 
        ' lblRegisterPrompt
        ' 
        lblRegisterPrompt.BackColor = Color.Transparent
        lblRegisterPrompt.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblRegisterPrompt.ForeColor = Color.FromArgb(CByte(142), CByte(142), CByte(147))
        lblRegisterPrompt.Location = New Point(50, 374)
        lblRegisterPrompt.Name = "lblRegisterPrompt"
        lblRegisterPrompt.Size = New Size(127, 17)
        lblRegisterPrompt.TabIndex = 7
        lblRegisterPrompt.Text = "Don't have an account?"
        lblRegisterPrompt.TextAlignment = ContentAlignment.MiddleCenter
        ' 
        ' btnLogin
        ' 
        btnLogin.BorderRadius = 12
        btnLogin.CustomizableEdges = CustomizableEdges3
        btnLogin.DisabledState.BorderColor = Color.DarkGray
        btnLogin.DisabledState.CustomBorderColor = Color.DarkGray
        btnLogin.DisabledState.FillColor = Color.FromArgb(CByte(169), CByte(169), CByte(169))
        btnLogin.DisabledState.ForeColor = Color.FromArgb(CByte(141), CByte(141), CByte(141))
        btnLogin.FillColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        btnLogin.Font = New Font("Segoe UI", 9F)
        btnLogin.ForeColor = Color.White
        btnLogin.HoverState.FillColor = Color.FromArgb(CByte(0), CByte(100), CByte(210))
        btnLogin.Location = New Point(50, 304)
        btnLogin.Name = "btnLogin"
        btnLogin.ShadowDecoration.CustomizableEdges = CustomizableEdges4
        btnLogin.Size = New Size(300, 50)
        btnLogin.TabIndex = 6
        btnLogin.Text = "Sign In"
        ' 
        ' Guna2TextBox2
        ' 
        Guna2TextBox2.BorderColor = Color.FromArgb(CByte(220), CByte(220), CByte(225))
        Guna2TextBox2.BorderRadius = 12
        Guna2TextBox2.CustomizableEdges = CustomizableEdges5
        Guna2TextBox2.DefaultText = ""
        Guna2TextBox2.DisabledState.BorderColor = Color.FromArgb(CByte(208), CByte(208), CByte(208))
        Guna2TextBox2.DisabledState.FillColor = Color.FromArgb(CByte(226), CByte(226), CByte(226))
        Guna2TextBox2.DisabledState.ForeColor = Color.FromArgb(CByte(138), CByte(138), CByte(138))
        Guna2TextBox2.DisabledState.PlaceholderForeColor = Color.FromArgb(CByte(138), CByte(138), CByte(138))
        Guna2TextBox2.FillColor = Color.FromArgb(CByte(250), CByte(250), CByte(252))
        Guna2TextBox2.FocusedState.BorderColor = Color.FromArgb(CByte(0), CByte(122), CByte(255))
        Guna2TextBox2.Font = New Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Guna2TextBox2.ForeColor = Color.FromArgb(CByte(28), CByte(28), CByte(30))
        Guna2TextBox2.HoverState.BorderColor = Color.FromArgb(CByte(0), CByte(122), CByte(255))
        Guna2TextBox2.Location = New Point(50, 229)
        Guna2TextBox2.Margin = New Padding(4, 5, 4, 5)
        Guna2TextBox2.Name = "Guna2TextBox2"
        Guna2TextBox2.PasswordChar = "●"c
        Guna2TextBox2.PlaceholderForeColor = Color.FromArgb(CByte(142), CByte(142), CByte(147))
        Guna2TextBox2.PlaceholderText = "Password"
        Guna2TextBox2.SelectedText = ""
        Guna2TextBox2.ShadowDecoration.CustomizableEdges = CustomizableEdges6
        Guna2TextBox2.Size = New Size(300, 48)
        Guna2TextBox2.TabIndex = 4
        ' 
        ' Guna2TextBox1
        ' 
        Guna2TextBox1.BorderColor = Color.FromArgb(CByte(220), CByte(220), CByte(225))
        Guna2TextBox1.BorderRadius = 12
        Guna2TextBox1.CustomizableEdges = CustomizableEdges7
        Guna2TextBox1.DefaultText = ""
        Guna2TextBox1.DisabledState.BorderColor = Color.FromArgb(CByte(208), CByte(208), CByte(208))
        Guna2TextBox1.DisabledState.FillColor = Color.FromArgb(CByte(226), CByte(226), CByte(226))
        Guna2TextBox1.DisabledState.ForeColor = Color.FromArgb(CByte(138), CByte(138), CByte(138))
        Guna2TextBox1.DisabledState.PlaceholderForeColor = Color.FromArgb(CByte(138), CByte(138), CByte(138))
        Guna2TextBox1.FillColor = Color.FromArgb(CByte(250), CByte(250), CByte(252))
        Guna2TextBox1.FocusedState.BorderColor = Color.FromArgb(CByte(0), CByte(122), CByte(255))
        Guna2TextBox1.Font = New Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Guna2TextBox1.ForeColor = Color.FromArgb(CByte(28), CByte(28), CByte(30))
        Guna2TextBox1.HoverState.BorderColor = Color.FromArgb(CByte(0), CByte(122), CByte(255))
        Guna2TextBox1.Location = New Point(50, 154)
        Guna2TextBox1.Margin = New Padding(4, 5, 4, 5)
        Guna2TextBox1.Name = "Guna2TextBox1"
        Guna2TextBox1.PlaceholderForeColor = Color.FromArgb(CByte(142), CByte(142), CByte(147))
        Guna2TextBox1.PlaceholderText = "NPWP (15 digits)"
        Guna2TextBox1.SelectedText = ""
        Guna2TextBox1.ShadowDecoration.CustomizableEdges = CustomizableEdges8
        Guna2TextBox1.Size = New Size(300, 48)
        Guna2TextBox1.TabIndex = 3
        ' 
        ' lblSubtitle
        ' 
        lblSubtitle.BackColor = Color.Transparent
        lblSubtitle.Font = New Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        lblSubtitle.ForeColor = Color.FromArgb(CByte(142), CByte(142), CByte(147))
        lblSubtitle.Location = New Point(50, 115)
        lblSubtitle.Name = "lblSubtitle"
        lblSubtitle.Size = New Size(209, 19)
        lblSubtitle.TabIndex = 1
        lblSubtitle.Text = "Sign in to manage your tax account"
        lblSubtitle.TextAlignment = ContentAlignment.MiddleCenter
        ' 
        ' lblTitle
        ' 
        lblTitle.BackColor = Color.Transparent
        lblTitle.Font = New Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTitle.ForeColor = Color.FromArgb(CByte(28), CByte(28), CByte(30))
        lblTitle.Location = New Point(50, 50)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(142, 47)
        lblTitle.TabIndex = 0
        lblTitle.Text = "Welcome"
        lblTitle.TextAlignment = ContentAlignment.MiddleCenter
        ' 
        ' FrmLogin
        ' 
        AutoScaleMode = AutoScaleMode.None
        BackColor = Color.FromArgb(CByte(242), CByte(242), CByte(247))
        ClientSize = New Size(800, 700)
        Controls.Add(pnlMain)
        FormBorderStyle = FormBorderStyle.None
        Name = "FrmLogin"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Login"
        pnlMain.ResumeLayout(False)
        pnlMain.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlMain As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents lblTitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents lblSubtitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2TextBox1 As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Guna2TextBox2 As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents btnLogin As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents lblRegisterPrompt As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents btnGoRegister As Guna.UI2.WinForms.Guna2Button

End Class
