Public Class wp_lapor_pajak
    Private Sub Wp_navbar1_Load(sender As Object, e As EventArgs) Handles Wp_navbar1.Load

    End Sub

    Private Sub FrmLaporPajak_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Tambahkan event handlers navbar
        AddHandler Wp_navbar1.DashboardClicked, AddressOf OnDashboardClicked
        AddHandler Wp_navbar1.LaporClicked, AddressOf OnLaporClicked
        AddHandler Wp_navbar1.BuktiPotongClicked, AddressOf OnBuktiPotongClicked
        AddHandler Wp_navbar1.RiwayatClicked, AddressOf OnRiwayatClicked
        AddHandler Wp_navbar1.DataDiriClicked, AddressOf OnDataDiriClicked
        AddHandler Wp_navbar1.LogoutClicked, AddressOf OnLogoutClicked

        ' Tandai menu Lapor sebagai aktif
        Wp_navbar1.SetActiveMenu(wp_navbar.MenuType.Lapor)
    End Sub

    Private Sub lblDendaLapor_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Guna2TextBox1_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox1.TextChanged

    End Sub

    ' ====== EVENT HANDLERS NAVBAR ======
    Private Sub OnDashboardClicked(sender As Object, e As EventArgs)
        Dim f As New wp_dashboard()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnLaporClicked(sender As Object, e As EventArgs)
        ' Sudah di halaman lapor pajak
    End Sub

    Private Sub OnBuktiPotongClicked(sender As Object, e As EventArgs)
        Dim f As New wp_riwayat_bukti_potong()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnRiwayatClicked(sender As Object, e As EventArgs)
        Dim f As New wp_riwayat_lapor_pajak()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnDataDiriClicked(sender As Object, e As EventArgs)
        Dim f As New wp_data_diri()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnLogoutClicked(sender As Object, e As EventArgs)
        Dim f As New FrmLogin()
        f.Show()
        Me.Close()
    End Sub

    Private Sub BunifuPanel1_Click(sender As Object, e As EventArgs) Handles BunifuPanel1.Click

    End Sub
End Class