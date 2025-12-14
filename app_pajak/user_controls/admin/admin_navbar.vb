Public Class admin_navbar

    ' ====== EVENT UNTUK NAVIGASI ======
    Public Event DashboardClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event ManajemenPemberiKerjaClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event ManajemenUserClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event ManajemenPerusahaanClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event MasterPajakClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event LogoutClicked(ByVal sender As Object, ByVal e As EventArgs)

    ' ====== ENUM UNTUK MENANDAI MENU AKTIF ======
    Public Enum MenuType
        Dashboard
        ManajemenPemberiKerja
        ManajemenUser
        ManajemenPerusahaan
        MasterPajak
    End Enum

    ' ====== INITIALIZATION ======
    Private Sub admin_navbar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeIconSwapHandlers()
    End Sub

    ''' <summary>
    ''' Setup CheckedChanged handlers for automatic icon swapping
    ''' </summary>
    Private Sub InitializeIconSwapHandlers()
        ' Add CheckedChanged handlers to swap icons when button state changes
        AddHandler btnDashboard.CheckedChanged, AddressOf btnDashboard_CheckedChanged
        AddHandler btnUsers.CheckedChanged, AddressOf btnUsers_CheckedChanged
        AddHandler btnPerusahaan.CheckedChanged, AddressOf btnPerusahaan_CheckedChanged
        AddHandler btnMasterPajak.CheckedChanged, AddressOf btnMasterPajak_CheckedChanged
    End Sub

    ' ====== ICON SWAP HANDLERS ======
    Private Sub btnDashboard_CheckedChanged(sender As Object, e As EventArgs)
        If btnDashboard.Checked Then
            btnDashboard.Image = My.Resources.Resources.dashboard_white
        Else
            btnDashboard.Image = My.Resources.Resources.dashboard
        End If
    End Sub



    Private Sub btnUsers_CheckedChanged(sender As Object, e As EventArgs)
        If btnUsers.Checked Then
            btnUsers.Image = My.Resources.Resources.user_white
        Else
            btnUsers.Image = My.Resources.Resources.user__2_
        End If
    End Sub

    Private Sub btnPerusahaan_CheckedChanged(sender As Object, e As EventArgs)
        If btnPerusahaan.Checked Then
            btnPerusahaan.Image = My.Resources.Resources.newspaper_white
        Else
            btnPerusahaan.Image = My.Resources.Resources.newspaper
        End If
    End Sub

    Private Sub btnMasterPajak_CheckedChanged(sender As Object, e As EventArgs)
        ' Master Pajak uses text only, no icon swap needed
    End Sub

    ' ====== HANDLER NAVIGASI ======

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        RaiseEvent DashboardClicked(Me, e)
    End Sub

    Private Sub btnPemberiKerja_Click(sender As Object, e As EventArgs)
        RaiseEvent ManajemenPemberiKerjaClicked(Me, e)
    End Sub

    Private Sub btnUsers_Click(sender As Object, e As EventArgs) Handles btnUsers.Click
        RaiseEvent ManajemenUserClicked(Me, e)
    End Sub

    Private Sub btnPerusahaan_Click(sender As Object, e As EventArgs) Handles btnPerusahaan.Click
        RaiseEvent ManajemenPerusahaanClicked(Me, e)
    End Sub

    Private Sub btnMasterPajak_Click(sender As Object, e As EventArgs) Handles btnMasterPajak.Click
        RaiseEvent MasterPajakClicked(Me, e)
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs) Handles btnLogout.Click
        RaiseEvent LogoutClicked(Me, e)
    End Sub

    ' ====== HIGHLIGHT MENU AKTIF ======

    ''' <summary>
    ''' Mengatur menu mana yang sedang aktif (akan diberi background).
    ''' Panggil dari masing-masing form.
    ''' Icon akan berubah otomatis melalui CheckedChanged event.
    ''' </summary>
    ''' <param name="menu"></param>
    Public Sub SetActiveMenu(menu As MenuType)
        Select Case menu
            Case MenuType.Dashboard
                btnDashboard.Checked = True


            Case MenuType.ManajemenUser
                btnUsers.Checked = True

            Case MenuType.ManajemenPerusahaan
                btnPerusahaan.Checked = True

            Case MenuType.MasterPajak
                btnMasterPajak.Checked = True
        End Select
    End Sub

End Class
