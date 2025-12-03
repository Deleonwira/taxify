Public Class wp_riwayat_bukti_potong

    Private Sub PanelTable_Paint(sender As Object, e As PaintEventArgs) Handles PanelTable.Paint

    End Sub

    ' ====== EVENT HANDLERS NAVBAR ======
    Private Sub FrmLoad(sender As Object, e As EventArgs) Handles MyBase.Load
        AddHandler Wp_navbar1.DashboardClicked, AddressOf OnDashboardClicked
        AddHandler Wp_navbar1.LaporClicked, AddressOf OnLaporClicked
        AddHandler Wp_navbar1.BuktiPotongClicked, AddressOf OnBuktiPotongClicked
        AddHandler Wp_navbar1.RiwayatClicked, AddressOf OnRiwayatClicked
        AddHandler Wp_navbar1.DataDiriClicked, AddressOf OnDataDiriClicked
        AddHandler Wp_navbar1.LogoutClicked, AddressOf OnLogoutClicked

        ' Tandai menu Bukti Potong sebagai aktif
        Wp_navbar1.SetActiveMenu(wp_navbar.MenuType.BuktiPotong)
    End Sub

    Private Sub OnDashboardClicked(sender As Object, e As EventArgs)
        Dim f As New wp_dashboard()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnLaporClicked(sender As Object, e As EventArgs)
        Dim f As New wp_lapor_pajak()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnBuktiPotongClicked(sender As Object, e As EventArgs)
        ' Sudah di halaman riwayat bukti potong
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

End Class