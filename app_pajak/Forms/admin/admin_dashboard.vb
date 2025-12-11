Public Class admin_dashboard

    Private Sub admin_dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Update subtitle with admin name
        lblSubtitle.Text = "Selamat datang, " & ModuleSession.CurrentUserName
        
        ' Set active menu in navbar
        Admin_navbar1.SetActiveMenu(admin_navbar.MenuType.Dashboard)
    End Sub

    ' ====== NAVBAR EVENT HANDLERS ======
    Private Sub Admin_navbar1_DashboardClicked(sender As Object, e As EventArgs) Handles Admin_navbar1.DashboardClicked
        ' Already on dashboard, do nothing
    End Sub

    Private Sub Admin_navbar1_ValidasiRegistrasiClicked(sender As Object, e As EventArgs) Handles Admin_navbar1.ValidasiRegistrasiClicked
        Dim f As New admin_validasi_registrasi()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Admin_navbar1_ManajemenUserClicked(sender As Object, e As EventArgs) Handles Admin_navbar1.ManajemenUserClicked
        Dim f As New FrmUserManagement()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Admin_navbar1_ManajemenPerusahaanClicked(sender As Object, e As EventArgs) Handles Admin_navbar1.ManajemenPerusahaanClicked
        Dim f As New FrmManagementPerusahaan()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Admin_navbar1_LogoutClicked(sender As Object, e As EventArgs) Handles Admin_navbar1.LogoutClicked
        ModuleSession.ClearSession()
        Dim f As New FrmLogin()
        f.Show()
        Me.Close()
    End Sub

    Private Sub btnValidasiRegistrasi_Click(sender As Object, e As EventArgs) Handles btnValidasiRegistrasi.Click
        Dim f As New admin_validasi_registrasi()
        f.Show()
        Me.Close()
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        ModuleSession.ClearSession()
        Dim f As New FrmLogin()
        f.Show()
        Me.Close()
    End Sub

End Class