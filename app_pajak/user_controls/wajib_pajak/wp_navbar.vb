Public Class wp_navbar

    ' ====== EVENT UNTUK NAVIGASI ======
    Public Event DashboardClicked(ByVal sender As Object, ByVal e As EventArgs)

    ' Bukti Potong submenu events
    Public Event TimelineBuktiPotongClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event RiwayatBuktiPotongClicked(ByVal sender As Object, ByVal e As EventArgs)

    ' Lapor SPT submenu events  
    Public Event LaporPajakClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event RiwayatLaporClicked(ByVal sender As Object, ByVal e As EventArgs)

    Public Event DataDiriClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event LogoutClicked(ByVal sender As Object, ByVal e As EventArgs)

    ' ====== ENUM UNTUK MENANDAI MENU AKTIF ======
    Public Enum MenuType
        Dashboard
        TimelineBuktiPotong
        RiwayatBuktiPotong
        LaporPajak
        RiwayatLapor
        DataDiri
    End Enum

    ' Dropdown panels and buttons
    Private PanelLaporDropdown As Guna.UI2.WinForms.Guna2Panel
    Private PanelBuktiPotongDropdown As Guna.UI2.WinForms.Guna2Panel
    Private btnLaporPajak As Guna.UI2.WinForms.Guna2Button
    Private btnRiwayatLaporSubmenu As Guna.UI2.WinForms.Guna2Button
    Private btnTimelineBuktiPotong As Guna.UI2.WinForms.Guna2Button
    Private btnRiwayatBuktiPotongSubmenu As Guna.UI2.WinForms.Guna2Button

    ' ====== INITIALIZATION ======
    Private Sub wp_navbar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeDropdownPanels()
    End Sub

    Private Sub InitializeDropdownPanels()
        ' ===== CREATE LAPOR DROPDOWN PANEL =====
        PanelLaporDropdown = New Guna.UI2.WinForms.Guna2Panel()
        PanelLaporDropdown.Size = New Size(160, 90)
        PanelLaporDropdown.Location = New Point(17, 231) ' Below btnLapor
        PanelLaporDropdown.FillColor = Color.FromArgb(96, 76, 219) ' Slightly darker purple
        PanelLaporDropdown.BorderRadius = 8
        PanelLaporDropdown.Visible = False

        ' Lapor Pajak submenu button
        btnLaporPajak = New Guna.UI2.WinForms.Guna2Button()
        btnLaporPajak.Text = "Lapor Pajak"
        btnLaporPajak.Size = New Size(150, 35)
        btnLaporPajak.Location = New Point(5, 5)
        btnLaporPajak.FillColor = Color.Transparent
        btnLaporPajak.Font = New Font("Segoe UI", 9.0F)
        btnLaporPajak.ForeColor = Color.White
        btnLaporPajak.TextAlign = HorizontalAlignment.Left
        btnLaporPajak.Image = My.Resources.Resources.contact_form
        btnLaporPajak.ImageSize = New Size(16, 16)
        btnLaporPajak.ImageAlign = HorizontalAlignment.Left
        btnLaporPajak.ImageOffset = New Point(15, 0)
        btnLaporPajak.TextOffset = New Point(15, 0)
        btnLaporPajak.BorderRadius = 6
        btnLaporPajak.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton
        btnLaporPajak.CheckedState.FillColor = Color.FromArgb(106, 90, 232)
        AddHandler btnLaporPajak.Click, AddressOf btnLaporPajak_Click

        ' Riwayat Lapor submenu button
        btnRiwayatLaporSubmenu = New Guna.UI2.WinForms.Guna2Button()
        btnRiwayatLaporSubmenu.Text = "Riwayat"
        btnRiwayatLaporSubmenu.Size = New Size(150, 35)
        btnRiwayatLaporSubmenu.Location = New Point(5, 45)
        btnRiwayatLaporSubmenu.FillColor = Color.Transparent
        btnRiwayatLaporSubmenu.Font = New Font("Segoe UI", 9.0F)
        btnRiwayatLaporSubmenu.ForeColor = Color.White
        btnRiwayatLaporSubmenu.TextAlign = HorizontalAlignment.Left
        btnRiwayatLaporSubmenu.Image = My.Resources.Resources.history
        btnRiwayatLaporSubmenu.ImageSize = New Size(16, 16)
        btnRiwayatLaporSubmenu.ImageAlign = HorizontalAlignment.Left
        btnRiwayatLaporSubmenu.ImageOffset = New Point(15, 0)
        btnRiwayatLaporSubmenu.TextOffset = New Point(15, 0)
        btnRiwayatLaporSubmenu.BorderRadius = 6
        btnRiwayatLaporSubmenu.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton
        btnRiwayatLaporSubmenu.CheckedState.FillColor = Color.FromArgb(106, 90, 232)
        AddHandler btnRiwayatLaporSubmenu.Click, AddressOf btnRiwayatLaporSubmenu_Click

        PanelLaporDropdown.Controls.Add(btnLaporPajak)
        PanelLaporDropdown.Controls.Add(btnRiwayatLaporSubmenu)
        Guna2Panel1.Controls.Add(PanelLaporDropdown)
        PanelLaporDropdown.BringToFront()

        ' ===== CREATE BUKTI POTONG DROPDOWN PANEL =====
        PanelBuktiPotongDropdown = New Guna.UI2.WinForms.Guna2Panel()
        PanelBuktiPotongDropdown.Size = New Size(160, 90)
        PanelBuktiPotongDropdown.Location = New Point(17, 280) ' Below btnBuktiPotong
        PanelBuktiPotongDropdown.FillColor = Color.FromArgb(96, 76, 219) ' Slightly darker purple
        PanelBuktiPotongDropdown.BorderRadius = 8
        PanelBuktiPotongDropdown.Visible = False

        ' Timeline Bukti Potong submenu button
        btnTimelineBuktiPotong = New Guna.UI2.WinForms.Guna2Button()
        btnTimelineBuktiPotong.Text = "Timeline"
        btnTimelineBuktiPotong.Size = New Size(150, 35)
        btnTimelineBuktiPotong.Location = New Point(5, 5)
        btnTimelineBuktiPotong.FillColor = Color.Transparent
        btnTimelineBuktiPotong.Font = New Font("Segoe UI", 9.0F)
        btnTimelineBuktiPotong.ForeColor = Color.White
        btnTimelineBuktiPotong.TextAlign = HorizontalAlignment.Left
        btnTimelineBuktiPotong.Image = My.Resources.Resources.report_card
        btnTimelineBuktiPotong.ImageSize = New Size(16, 16)
        btnTimelineBuktiPotong.ImageAlign = HorizontalAlignment.Left
        btnTimelineBuktiPotong.ImageOffset = New Point(15, 0)
        btnTimelineBuktiPotong.TextOffset = New Point(15, 0)
        btnTimelineBuktiPotong.BorderRadius = 6
        btnTimelineBuktiPotong.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton
        btnTimelineBuktiPotong.CheckedState.FillColor = Color.FromArgb(106, 90, 232)
        AddHandler btnTimelineBuktiPotong.Click, AddressOf btnTimelineBuktiPotong_Click

        ' Riwayat Bukti Potong submenu button
        btnRiwayatBuktiPotongSubmenu = New Guna.UI2.WinForms.Guna2Button()
        btnRiwayatBuktiPotongSubmenu.Text = "Riwayat"
        btnRiwayatBuktiPotongSubmenu.Size = New Size(150, 35)
        btnRiwayatBuktiPotongSubmenu.Location = New Point(5, 45)
        btnRiwayatBuktiPotongSubmenu.FillColor = Color.Transparent
        btnRiwayatBuktiPotongSubmenu.Font = New Font("Segoe UI", 9.0F)
        btnRiwayatBuktiPotongSubmenu.ForeColor = Color.White
        btnRiwayatBuktiPotongSubmenu.TextAlign = HorizontalAlignment.Left
        btnRiwayatBuktiPotongSubmenu.Image = My.Resources.Resources.history
        btnRiwayatBuktiPotongSubmenu.ImageSize = New Size(16, 16)
        btnRiwayatBuktiPotongSubmenu.ImageAlign = HorizontalAlignment.Left
        btnRiwayatBuktiPotongSubmenu.ImageOffset = New Point(15, 0)
        btnRiwayatBuktiPotongSubmenu.TextOffset = New Point(15, 0)
        btnRiwayatBuktiPotongSubmenu.BorderRadius = 6
        btnRiwayatBuktiPotongSubmenu.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton
        btnRiwayatBuktiPotongSubmenu.CheckedState.FillColor = Color.FromArgb(106, 90, 232)
        AddHandler btnRiwayatBuktiPotongSubmenu.Click, AddressOf btnRiwayatBuktiPotongSubmenu_Click

        PanelBuktiPotongDropdown.Controls.Add(btnTimelineBuktiPotong)
        PanelBuktiPotongDropdown.Controls.Add(btnRiwayatBuktiPotongSubmenu)
        Guna2Panel1.Controls.Add(PanelBuktiPotongDropdown)
        PanelBuktiPotongDropdown.BringToFront()
    End Sub

    ' ====== HANDLER NAVIGASI ======

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs) Handles btnDashboard.Click
        CollapseAllSubmenus()
        RaiseEvent DashboardClicked(Me, e)
    End Sub

    Private Sub btnLapor_Click(sender As Object, e As EventArgs) Handles btnLapor.Click
        ' Toggle Lapor dropdown with accordion behavior
        If PanelLaporDropdown.Visible Then
            ' Collapse - move buttons back up
            PanelLaporDropdown.Visible = False
            btnBuktiPotong.Location = New Point(17, 231)
            btnRiwayat.Location = New Point(17, 280)
            btnDataDiri.Location = New Point(17, 329)
        Else
            ' Expand - move buttons down
            PanelLaporDropdown.Visible = True
            btnBuktiPotong.Location = New Point(17, 321) ' 231 + 90 (panel height)
            btnRiwayat.Location = New Point(17, 370)
            btnDataDiri.Location = New Point(17, 419)

            ' Also collapse Bukti Potong dropdown if open and reset those positions
            If PanelBuktiPotongDropdown.Visible Then
                PanelBuktiPotongDropdown.Visible = False
                ' btnRiwayat, btnDataDiri already moved above
            End If
        End If
    End Sub

    Private Sub btnBuktiPotong_Click(sender As Object, e As EventArgs) Handles btnBuktiPotong.Click
        ' First, check if Lapor dropdown is open and close it
        If PanelLaporDropdown.Visible Then
            PanelLaporDropdown.Visible = False
            ' Reset button positions to original
            btnBuktiPotong.Location = New Point(17, 231)
            btnRiwayat.Location = New Point(17, 280)
            btnDataDiri.Location = New Point(17, 329)
        End If

        ' Now toggle Bukti Potong dropdown
        If PanelBuktiPotongDropdown.Visible Then
            ' Collapse - move buttons back up
            PanelBuktiPotongDropdown.Visible = False
            btnRiwayat.Location = New Point(17, 280)
            btnDataDiri.Location = New Point(17, 329)
        Else
            ' Expand - move buttons down
            PanelBuktiPotongDropdown.Visible = True
            PanelBuktiPotongDropdown.Location = New Point(17, 280) ' Right below btnBuktiPotong
            btnRiwayat.Location = New Point(17, 370) ' 280 + 90 (panel height)
            btnDataDiri.Location = New Point(17, 419)
        End If
    End Sub

    Private Sub btnRiwayat_Click(sender As Object, e As EventArgs) Handles btnRiwayat.Click
        CollapseAllSubmenus()
        ' Navigate to Riwayat Lapor directly
        RaiseEvent RiwayatLaporClicked(Me, e)
    End Sub

    Private Sub btnDataDiri_Click(sender As Object, e As EventArgs) Handles btnDataDiri.Click
        CollapseAllSubmenus()
        RaiseEvent DataDiriClicked(Me, e)
    End Sub

    Private Sub btnLogout_Click(sender As Object, e As EventArgs)
        CollapseAllSubmenus()
        RaiseEvent LogoutClicked(Me, e)
    End Sub

    ' ====== SUBMENU BUTTON HANDLERS ======

    Private Sub btnLaporPajak_Click(sender As Object, e As EventArgs)
        PanelLaporDropdown.Visible = False
        RaiseEvent LaporPajakClicked(Me, e)
    End Sub

    Private Sub btnRiwayatLaporSubmenu_Click(sender As Object, e As EventArgs)
        PanelLaporDropdown.Visible = False
        RaiseEvent RiwayatLaporClicked(Me, e)
    End Sub

    Private Sub btnTimelineBuktiPotong_Click(sender As Object, e As EventArgs)
        PanelBuktiPotongDropdown.Visible = False
        RaiseEvent TimelineBuktiPotongClicked(Me, e)
    End Sub

    Private Sub btnRiwayatBuktiPotongSubmenu_Click(sender As Object, e As EventArgs)
        PanelBuktiPotongDropdown.Visible = False
        RaiseEvent RiwayatBuktiPotongClicked(Me, e)
    End Sub

    Private Sub CollapseAllSubmenus()
        If PanelLaporDropdown IsNot Nothing Then
            PanelLaporDropdown.Visible = False
        End If
        If PanelBuktiPotongDropdown IsNot Nothing Then
            PanelBuktiPotongDropdown.Visible = False
        End If
    End Sub

    ' ====== PUBLIC METHODS FOR SUBMENU NAVIGATION ======

    ''' <summary>
    ''' Navigate to Timeline Bukti Potong
    ''' </summary>
    Public Sub NavigateToTimelineBuktiPotong()
        CollapseAllSubmenus()
        RaiseEvent TimelineBuktiPotongClicked(Me, EventArgs.Empty)
    End Sub

    ''' <summary>
    ''' Navigate to Riwayat Bukti Potong
    ''' </summary>
    Public Sub NavigateToRiwayatBuktiPotong()
        CollapseAllSubmenus()
        RaiseEvent RiwayatBuktiPotongClicked(Me, EventArgs.Empty)
    End Sub

    ''' <summary>
    ''' Navigate to Lapor Pajak
    ''' </summary>
    Public Sub NavigateToLaporPajak()
        CollapseAllSubmenus()
        RaiseEvent LaporPajakClicked(Me, EventArgs.Empty)
    End Sub

    ''' <summary>
    ''' Navigate to Riwayat Lapor
    ''' </summary>
    Public Sub NavigateToRiwayatLapor()
        CollapseAllSubmenus()
        RaiseEvent RiwayatLaporClicked(Me, EventArgs.Empty)
    End Sub

    ' ====== HIGHLIGHT MENU AKTIF ======

    ''' <summary>
    ''' Mengatur menu mana yang sedang aktif (akan diberi background).
    ''' Panggil dari masing-masing form (dashboard / lapor / dll).
    ''' </summary>
    ''' <param name="menu"></param>
    Public Sub SetActiveMenu(menu As MenuType)
        ' Reset logic tidak diperlukan karena Guna2Button dengan ButtonMode.RadioButton
        ' akan otomatis uncheck tombol lain. Kita hanya perlu check tombol yang sesuai.

        CollapseAllSubmenus()

        Select Case menu
            Case MenuType.Dashboard
                btnDashboard.Checked = True

            Case MenuType.TimelineBuktiPotong
                btnBuktiPotong.Checked = True
                If btnTimelineBuktiPotong IsNot Nothing Then
                    btnTimelineBuktiPotong.Checked = True
                End If

            Case MenuType.RiwayatBuktiPotong
                btnBuktiPotong.Checked = True
                If btnRiwayatBuktiPotongSubmenu IsNot Nothing Then
                    btnRiwayatBuktiPotongSubmenu.Checked = True
                End If

            Case MenuType.LaporPajak
                btnLapor.Checked = True
                If btnLaporPajak IsNot Nothing Then
                    btnLaporPajak.Checked = True
                End If

            Case MenuType.RiwayatLapor
                btnRiwayat.Checked = True
                If btnRiwayatLaporSubmenu IsNot Nothing Then
                    btnRiwayatLaporSubmenu.Checked = True
                End If

            Case MenuType.DataDiri
                btnDataDiri.Checked = True
        End Select
    End Sub

    Private Sub Guna2Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Guna2Panel1.Paint

    End Sub
End Class
