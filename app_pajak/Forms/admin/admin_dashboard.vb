Public Class admin_dashboard

    Private Sub admin_dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Update subtitle with admin name
        lblSubtitle.Text = "Selamat datang, " & ModuleSession.CurrentUserName
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