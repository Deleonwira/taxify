Public Class pk_navbar1

    ' ====== EVENT UNTUK NAVIGASI ======
    Public Event DashboardClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event DaftarPegawaiClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event BuktiPotongClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event RiwayatClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event LogoutClicked(ByVal sender As Object, ByVal e As EventArgs)

    ' ====== ENUM UNTUK MENANDAI MENU AKTIF ======
    Public Enum MenuType
        Dashboard
        DaftarPegawai
        BuktiPotong
        Riwayat
    End Enum

    ' ====== HANDLER NAVIGASI ======

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        RaiseEvent DashboardClicked(Me, e)
    End Sub

    Private Sub btnDaftarPegawai_Click(sender As Object, e As EventArgs) Handles btnDaftarPegawai.Click
        RaiseEvent DaftarPegawaiClicked(Me, e)
    End Sub

    Private Sub btnBuktiPotong_Click(sender As Object, e As EventArgs) Handles btnBuktiPotong.Click
        RaiseEvent BuktiPotongClicked(Me, e)
    End Sub

    Private Sub btnRiwayat_Click(sender As Object, e As EventArgs) Handles btnRiwayat.Click
        RaiseEvent RiwayatClicked(Me, e)
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        RaiseEvent LogoutClicked(Me, e)
    End Sub

    ' ====== HIGHLIGHT MENU AKTIF ======

    ''' <summary>
    ''' Mengatur menu mana yang sedang aktif (akan diberi background).
    ''' Panggil dari masing-masing form (dashboard / daftar pegawai / dll).
    ''' </summary>
    ''' <param name="menu"></param>
    Public Sub SetActiveMenu(menu As MenuType)
        ' Reset logic tidak diperlukan karena Guna2Button dengan ButtonMode.RadioButton
        ' akan otomatis uncheck tombol lain. Kita hanya perlu check tombol yang sesuai.

        Select Case menu
            Case MenuType.Dashboard
                btnDashboard.Checked = True
            Case MenuType.DaftarPegawai
                btnDaftarPegawai.Checked = True
            Case MenuType.BuktiPotong
                btnBuktiPotong.Checked = True
            Case MenuType.Riwayat
                btnRiwayat.Checked = True
        End Select
    End Sub

End Class
