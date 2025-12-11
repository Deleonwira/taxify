<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class admin_dashboard
    Inherits System.Windows.Forms.Form

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
        Dim CustomizableEdges9 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges10 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges1 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges2 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges7 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges8 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges3 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges4 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges5 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges6 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Admin_navbar1 = New admin_navbar()
        pnlMain = New Guna.UI2.WinForms.Guna2Panel()
        pnlHeader = New Guna.UI2.WinForms.Guna2Panel()
        lblSubtitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        lblTitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        pnlMenu = New Guna.UI2.WinForms.Guna2Panel()
        btnValidasiRegistrasi = New Guna.UI2.WinForms.Guna2Button()
        btnLogout = New Guna.UI2.WinForms.Guna2Button()
        pnlMain.SuspendLayout()
        pnlHeader.SuspendLayout()
        pnlMenu.SuspendLayout()
        SuspendLayout()
        ' 
        ' Admin_navbar1
        ' 
        Admin_navbar1.BackColor = Color.White
        Admin_navbar1.Dock = DockStyle.Left
        Admin_navbar1.ForeColor = Color.Black
        Admin_navbar1.Location = New Point(0, 0)
        Admin_navbar1.Margin = New Padding(3, 4, 3, 4)
        Admin_navbar1.Name = "Admin_navbar1"
        Admin_navbar1.Size = New Size(200, 720)
        Admin_navbar1.TabIndex = 10
        ' 
        ' pnlMain
        ' 
        pnlMain.BackColor = Color.FromArgb(CByte(247), CByte(248), CByte(252))
        pnlMain.Controls.Add(pnlHeader)
        pnlMain.Controls.Add(pnlMenu)
        pnlMain.CustomizableEdges = CustomizableEdges9
        pnlMain.Dock = DockStyle.Fill
        pnlMain.Location = New Point(200, 0)
        pnlMain.Name = "pnlMain"
        pnlMain.Padding = New Padding(24)
        pnlMain.ShadowDecoration.CustomizableEdges = CustomizableEdges10
        pnlMain.Size = New Size(1000, 720)
        pnlMain.TabIndex = 0
        ' 
        ' pnlHeader
        ' 
        pnlHeader.BorderRadius = 12
        pnlHeader.Controls.Add(lblSubtitle)
        pnlHeader.Controls.Add(lblTitle)
        pnlHeader.CustomizableEdges = CustomizableEdges1
        pnlHeader.FillColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        pnlHeader.Location = New Point(24, 24)
        pnlHeader.Name = "pnlHeader"
        pnlHeader.ShadowDecoration.CustomizableEdges = CustomizableEdges2
        pnlHeader.Size = New Size(752, 80)
        pnlHeader.TabIndex = 0
        ' 
        ' lblSubtitle
        ' 
        lblSubtitle.BackColor = Color.Transparent
        lblSubtitle.Font = New Font("Segoe UI", 9F)
        lblSubtitle.ForeColor = Color.FromArgb(CByte(233), CByte(221), CByte(255))
        lblSubtitle.Location = New Point(24, 48)
        lblSubtitle.Name = "lblSubtitle"
        lblSubtitle.Size = New Size(103, 17)
        lblSubtitle.TabIndex = 1
        lblSubtitle.Text = "Kelola sistem pajak"
        ' 
        ' lblTitle
        ' 
        lblTitle.BackColor = Color.Transparent
        lblTitle.Font = New Font("Segoe UI Semibold", 14F, FontStyle.Bold)
        lblTitle.ForeColor = Color.White
        lblTitle.Location = New Point(24, 16)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(158, 27)
        lblTitle.TabIndex = 0
        lblTitle.Text = "Admin Dashboard"
        ' 
        ' pnlMenu
        ' 
        pnlMenu.BorderColor = Color.FromArgb(CByte(230), CByte(233), CByte(241))
        pnlMenu.BorderRadius = 12
        pnlMenu.BorderThickness = 1
        pnlMenu.Controls.Add(btnValidasiRegistrasi)
        pnlMenu.Controls.Add(btnLogout)
        pnlMenu.CustomizableEdges = CustomizableEdges7
        pnlMenu.FillColor = Color.White
        pnlMenu.Location = New Point(24, 120)
        pnlMenu.Name = "pnlMenu"
        pnlMenu.ShadowDecoration.CustomizableEdges = CustomizableEdges8
        pnlMenu.Size = New Size(752, 356)
        pnlMenu.TabIndex = 1
        ' 
        ' btnValidasiRegistrasi
        ' 
        btnValidasiRegistrasi.BorderRadius = 8
        btnValidasiRegistrasi.CustomizableEdges = CustomizableEdges3
        btnValidasiRegistrasi.FillColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        btnValidasiRegistrasi.Font = New Font("Segoe UI Semibold", 11F, FontStyle.Bold)
        btnValidasiRegistrasi.ForeColor = Color.White
        btnValidasiRegistrasi.Location = New Point(24, 24)
        btnValidasiRegistrasi.Name = "btnValidasiRegistrasi"
        btnValidasiRegistrasi.ShadowDecoration.CustomizableEdges = CustomizableEdges4
        btnValidasiRegistrasi.Size = New Size(300, 50)
        btnValidasiRegistrasi.TabIndex = 0
        btnValidasiRegistrasi.Text = "📋 Validasi Registrasi"
        ' 
        ' btnLogout
        ' 
        btnLogout.BorderRadius = 8
        btnLogout.CustomizableEdges = CustomizableEdges5
        btnLogout.FillColor = Color.FromArgb(CByte(255), CByte(59), CByte(48))
        btnLogout.Font = New Font("Segoe UI Semibold", 10F, FontStyle.Bold)
        btnLogout.ForeColor = Color.White
        btnLogout.Location = New Point(24, 290)
        btnLogout.Name = "btnLogout"
        btnLogout.ShadowDecoration.CustomizableEdges = CustomizableEdges6
        btnLogout.Size = New Size(150, 44)
        btnLogout.TabIndex = 1
        btnLogout.Text = "Logout"
        ' 
        ' admin_dashboard
        ' 
        AutoScaleMode = AutoScaleMode.None
        BackColor = Color.FromArgb(CByte(247), CByte(248), CByte(252))
        ClientSize = New Size(1200, 720)
        Controls.Add(pnlMain)
        Controls.Add(Admin_navbar1)
        FormBorderStyle = FormBorderStyle.None
        Name = "admin_dashboard"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Admin Dashboard"
        pnlMain.ResumeLayout(False)
        pnlHeader.ResumeLayout(False)
        pnlHeader.PerformLayout()
        pnlMenu.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlMain As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents pnlHeader As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents lblTitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents lblSubtitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents pnlMenu As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents btnValidasiRegistrasi As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnLogout As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Admin_navbar1 As admin_navbar

End Class
