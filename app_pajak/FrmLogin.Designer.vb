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
        Dim CustomizableEdges13 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges14 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges1 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges2 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges11 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges12 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges3 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges4 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges5 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges6 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges7 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges8 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges9 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges10 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        pnlMain = New Guna.UI2.WinForms.Guna2Panel()
        pnlHeader = New Guna.UI2.WinForms.Guna2Panel()
        lblSubtitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        lblTitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        pnlForm = New Guna.UI2.WinForms.Guna2Panel()
        lblNPWP = New Guna.UI2.WinForms.Guna2HtmlLabel()
        txtNPWP = New Guna.UI2.WinForms.Guna2TextBox()
        lblPassword = New Guna.UI2.WinForms.Guna2HtmlLabel()
        txtPassword = New Guna.UI2.WinForms.Guna2TextBox()
        btnLogin = New Guna.UI2.WinForms.Guna2Button()
        lblRegisterPrompt = New Guna.UI2.WinForms.Guna2HtmlLabel()
        btnGoRegister = New Guna.UI2.WinForms.Guna2Button()
        pnlMain.SuspendLayout()
        pnlHeader.SuspendLayout()
        pnlForm.SuspendLayout()
        SuspendLayout()
        ' 
        ' pnlMain
        ' 
        pnlMain.BackColor = Color.FromArgb(CByte(247), CByte(248), CByte(252))
        pnlMain.Controls.Add(pnlHeader)
        pnlMain.Controls.Add(pnlForm)
        pnlMain.CustomizableEdges = CustomizableEdges13
        pnlMain.Dock = DockStyle.Fill
        pnlMain.Location = New Point(0, 0)
        pnlMain.Name = "pnlMain"
        pnlMain.Padding = New Padding(100, 80, 100, 80)
        pnlMain.ShadowDecoration.CustomizableEdges = CustomizableEdges14
        pnlMain.Size = New Size(560, 520)
        pnlMain.TabIndex = 0
        ' 
        ' pnlHeader
        ' 
        pnlHeader.BorderColor = Color.FromArgb(CByte(230), CByte(233), CByte(241))
        pnlHeader.BorderRadius = 12
        pnlHeader.Controls.Add(lblSubtitle)
        pnlHeader.Controls.Add(lblTitle)
        pnlHeader.CustomizableEdges = CustomizableEdges1
        pnlHeader.FillColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        pnlHeader.Location = New Point(100, 80)
        pnlHeader.Name = "pnlHeader"
        pnlHeader.Padding = New Padding(24)
        pnlHeader.ShadowDecoration.CustomizableEdges = CustomizableEdges2
        pnlHeader.Size = New Size(360, 76)
        pnlHeader.TabIndex = 0
        ' 
        ' lblSubtitle
        ' 
        lblSubtitle.BackColor = Color.Transparent
        lblSubtitle.Font = New Font("Segoe UI", 9F)
        lblSubtitle.ForeColor = Color.FromArgb(CByte(233), CByte(221), CByte(255))
        lblSubtitle.Location = New Point(24, 46)
        lblSubtitle.Name = "lblSubtitle"
        lblSubtitle.Size = New Size(193, 17)
        lblSubtitle.TabIndex = 1
        lblSubtitle.Text = "Masuk untuk mengelola pajak Anda"
        ' 
        ' lblTitle
        ' 
        lblTitle.BackColor = Color.Transparent
        lblTitle.Font = New Font("Segoe UI Semibold", 14F, FontStyle.Bold)
        lblTitle.ForeColor = Color.White
        lblTitle.Location = New Point(24, 12)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(140, 27)
        lblTitle.TabIndex = 0
        lblTitle.Text = "Selamat Datang"
        ' 
        ' pnlForm
        ' 
        pnlForm.BorderColor = Color.FromArgb(CByte(230), CByte(233), CByte(241))
        pnlForm.BorderRadius = 12
        pnlForm.BorderThickness = 1
        pnlForm.Controls.Add(lblNPWP)
        pnlForm.Controls.Add(txtNPWP)
        pnlForm.Controls.Add(lblPassword)
        pnlForm.Controls.Add(txtPassword)
        pnlForm.Controls.Add(btnLogin)
        pnlForm.Controls.Add(lblRegisterPrompt)
        pnlForm.Controls.Add(btnGoRegister)
        pnlForm.CustomizableEdges = CustomizableEdges11
        pnlForm.FillColor = Color.White
        pnlForm.Location = New Point(100, 162)
        pnlForm.Name = "pnlForm"
        pnlForm.Padding = New Padding(24)
        pnlForm.ShadowDecoration.CustomizableEdges = CustomizableEdges12
        pnlForm.Size = New Size(360, 260)
        pnlForm.TabIndex = 1
        ' 
        ' lblNPWP
        ' 
        lblNPWP.BackColor = Color.Transparent
        lblNPWP.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        lblNPWP.ForeColor = Color.FromArgb(CByte(35), CByte(44), CByte(63))
        lblNPWP.Location = New Point(24, 20)
        lblNPWP.Name = "lblNPWP"
        lblNPWP.Size = New Size(64, 17)
        lblNPWP.TabIndex = 0
        lblNPWP.Text = "Username *"
        ' 
        ' txtNPWP
        ' 
        txtNPWP.BorderRadius = 8
        txtNPWP.CustomizableEdges = CustomizableEdges3
        txtNPWP.DefaultText = ""
        txtNPWP.FillColor = Color.FromArgb(CByte(245), CByte(246), CByte(250))
        txtNPWP.FocusedState.BorderColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        txtNPWP.Font = New Font("Segoe UI", 9.0F)
        txtNPWP.ForeColor = Color.FromArgb(CByte(35), CByte(44), CByte(63))
        txtNPWP.HoverState.BorderColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        txtNPWP.Location = New Point(24, 40)
        txtNPWP.Name = "txtNPWP"
        txtNPWP.PlaceholderForeColor = Color.FromArgb(CByte(150), CByte(150), CByte(150))
        txtNPWP.PlaceholderText = "Masukkan username anda"
        txtNPWP.SelectedText = ""
        txtNPWP.ShadowDecoration.CustomizableEdges = CustomizableEdges4
        txtNPWP.Size = New Size(312, 36)
        txtNPWP.TabIndex = 1
        ' 
        ' lblPassword
        ' 
        lblPassword.BackColor = Color.Transparent
        lblPassword.Font = New Font("Segoe UI Semibold", 9.0F, FontStyle.Bold)
        lblPassword.ForeColor = Color.FromArgb(CByte(35), CByte(44), CByte(63))
        lblPassword.Location = New Point(24, 84)
        lblPassword.Name = "lblPassword"
        lblPassword.Size = New Size(61, 17)
        lblPassword.TabIndex = 2
        lblPassword.Text = "Password *"
        ' 
        ' txtPassword
        ' 
        txtPassword.BorderRadius = 8
        txtPassword.CustomizableEdges = CustomizableEdges5
        txtPassword.DefaultText = ""
        txtPassword.FillColor = Color.FromArgb(CByte(245), CByte(246), CByte(250))
        txtPassword.FocusedState.BorderColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        txtPassword.Font = New Font("Segoe UI", 9.0F)
        txtPassword.ForeColor = Color.FromArgb(CByte(35), CByte(44), CByte(63))
        txtPassword.HoverState.BorderColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        txtPassword.Location = New Point(24, 104)
        txtPassword.Name = "txtPassword"
        txtPassword.PasswordChar = "●"c
        txtPassword.PlaceholderForeColor = Color.FromArgb(CByte(150), CByte(150), CByte(150))
        txtPassword.PlaceholderText = "Masukkan password anda"
        txtPassword.SelectedText = ""
        txtPassword.ShadowDecoration.CustomizableEdges = CustomizableEdges6
        txtPassword.Size = New Size(312, 36)
        txtPassword.TabIndex = 3
        ' 
        ' btnLogin
        ' 
        btnLogin.BorderRadius = 8
        btnLogin.CustomizableEdges = CustomizableEdges7
        btnLogin.FillColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        btnLogin.Font = New Font("Segoe UI Semibold", 10F, FontStyle.Bold)
        btnLogin.ForeColor = Color.White
        btnLogin.Location = New Point(24, 160)
        btnLogin.Name = "btnLogin"
        btnLogin.ShadowDecoration.CustomizableEdges = CustomizableEdges8
        btnLogin.Size = New Size(312, 44)
        btnLogin.TabIndex = 4
        btnLogin.Text = "Masuk"
        ' 
        ' lblRegisterPrompt
        ' 
        lblRegisterPrompt.BackColor = Color.Transparent
        lblRegisterPrompt.Font = New Font("Segoe UI", 9F)
        lblRegisterPrompt.ForeColor = Color.FromArgb(CByte(120), CByte(128), CByte(146))
        lblRegisterPrompt.Location = New Point(80, 218)
        lblRegisterPrompt.Name = "lblRegisterPrompt"
        lblRegisterPrompt.Size = New Size(107, 17)
        lblRegisterPrompt.TabIndex = 5
        lblRegisterPrompt.Text = "Belum punya akun?"
        ' 
        ' btnGoRegister
        ' 
        btnGoRegister.BackColor = Color.Transparent
        btnGoRegister.BorderRadius = 8
        btnGoRegister.CustomizableEdges = CustomizableEdges9
        btnGoRegister.FillColor = Color.Empty
        btnGoRegister.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        btnGoRegister.ForeColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        btnGoRegister.HoverState.FillColor = Color.FromArgb(CByte(245), CByte(246), CByte(250))
        btnGoRegister.Location = New Point(185, 212)
        btnGoRegister.Name = "btnGoRegister"
        btnGoRegister.ShadowDecoration.CustomizableEdges = CustomizableEdges10
        btnGoRegister.Size = New Size(70, 28)
        btnGoRegister.TabIndex = 6
        btnGoRegister.Text = "Daftar"
        btnGoRegister.UseTransparentBackground = True
        ' 
        ' FrmLogin
        ' 
        AutoScaleMode = AutoScaleMode.None
        BackColor = Color.FromArgb(CByte(247), CByte(248), CByte(252))
        ClientSize = New Size(560, 520)
        Controls.Add(pnlMain)
        FormBorderStyle = FormBorderStyle.None
        Name = "FrmLogin"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Login"
        pnlMain.ResumeLayout(False)
        pnlHeader.ResumeLayout(False)
        pnlHeader.PerformLayout()
        pnlForm.ResumeLayout(False)
        pnlForm.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlMain As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents pnlHeader As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents lblTitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents lblSubtitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents pnlForm As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents lblNPWP As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents txtNPWP As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents lblPassword As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents txtPassword As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents btnLogin As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents lblRegisterPrompt As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents btnGoRegister As Guna.UI2.WinForms.Guna2Button

End Class
