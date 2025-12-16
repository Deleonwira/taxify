Public Class admin_navbar

    ' ====== EVENT UNTUK NAVIGASI ======
    Public Event DashboardClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event ManajemenPemberiKerjaClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event ManajemenUserClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event ManajemenPerusahaanClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event ManajemenSPTClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event MasterPajakClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event LogoutClicked(ByVal sender As Object, ByVal e As EventArgs)

    ' ====== ENUM UNTUK MENANDAI MENU AKTIF ======
    Public Enum MenuType
        Dashboard
        ManajemenPemberiKerja
        ManajemenUser
        ManajemenPerusahaan
        ManajemenSPT
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
        AddHandler btnManajemenSPT.CheckedChanged, AddressOf btnManajemenSPT_CheckedChanged
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

    Private Sub btnManajemenSPT_CheckedChanged(sender As Object, e As EventArgs)
        If btnManajemenSPT.Checked Then
            btnManajemenSPT.Image = My.Resources.Resources.report_card ' Assuming white version exists or re-using same if not available. Wait, I should check if report_card_white exists.
            ' Checking resources... user_controls/admin_navbar.vb:65 uses settings_white.
            ' Let's look at Resx again. report_card exists. report_card_white? 
            ' The user didn't ask for a white icon for this one specifically, but consistency matters.
            ' I'll use report_card for now, as I don't have a verifyable white version.
            ' Actually, looking at previous artifacts, I see `diploma_white`, `dashboard_white`, `newspaper_white`, `user-robot-white`, `user_white`, `history_white`, `settings_white`.
            ' I DON'T see `report_card_white` in the viewed `Resources.Designer.vb` or `Resources.resx`.
            ' I will just use `report_card` for both states for now, or maybe `diploma_white` if it's close enough? 
            ' Let's stick to `report_card` for unchecked and maybe `diploma_white` for checked if it implies "Report"? 
            ' No, better to keep it safe. I'll use `report_card` for now and maybe changing it to something else if checked.
            ' Wait, `newspaper_white` is used for Perusahaan. 
            ' Let's assume `report_card` is the icon. If I don't have a white one, I can't swap it effectively.
            ' I'll just leave it as `report_card` for both for now to avoid compilation errors.
            btnManajemenSPT.Image = My.Resources.Resources.report_card
        Else
            btnManajemenSPT.Image = My.Resources.Resources.report_card
        End If
    End Sub

    Private Sub btnMasterPajak_CheckedChanged(sender As Object, e As EventArgs)
        If btnMasterPajak.Checked Then
            btnMasterPajak.Image = My.Resources.Resources.settings_white
        Else
            btnMasterPajak.Image = My.Resources.Resources.settings
        End If
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

    Private Sub btnManajemenSPT_Click(sender As Object, e As EventArgs) Handles btnManajemenSPT.Click
        RaiseEvent ManajemenSPTClicked(Me, e)
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

            Case MenuType.ManajemenSPT
                btnManajemenSPT.Checked = True

            Case MenuType.MasterPajak
                btnMasterPajak.Checked = True
        End Select
    End Sub

End Class
