Imports MySql.Data.MySqlClient

Public Class pk_daftar_pegawai

    ' Form load event - Load data from database
    Private Sub pk_daftar_pegawai_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Pk_navbar11.SetActiveMenu(pk_navbar1.MenuType.DaftarPegawai)
        LoadEmployeesFromDatabase()
    End Sub

    ' Load employees from database based on logged-in company
    Private Sub LoadEmployeesFromDatabase()
        FlowEmployees.Controls.Clear() ' Clear existing cards

        Try
            BukaKoneksi()

            ' Get perusahaan_id dari logged-in user (pemberi_kerja)
            Dim queryPerusahaan As String = "SELECT id FROM perusahaan WHERE owner_npwp = @npwp LIMIT 1"
            Dim cmdPerusahaan As New MySqlCommand(queryPerusahaan, koneksi)
            cmdPerusahaan.Parameters.AddWithValue("@npwp", CurrentUserNPWP)

            Dim perusahaanId As Integer = 0
            Dim readerPerusahaan As MySqlDataReader = cmdPerusahaan.ExecuteReader()
            If readerPerusahaan.Read() Then
                perusahaanId = Convert.ToInt32(readerPerusahaan("id"))
            End If
            readerPerusahaan.Close()

            If perusahaanId = 0 Then
                MessageBox.Show("Perusahaan tidak ditemukan untuk user ini.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
                TutupKoneksi()
                Return
            End If

            ' Query untuk get employees based on perusahaan_id
            Dim query As String = "SELECT p.wp_npwp, u.nama, p.jabatan, p.status_kepegawaian, p.status_ptkp " &
                                  "FROM pekerjaan p " &
                                  "INNER JOIN users u ON p.wp_npwp = u.npwp " &
                                  "WHERE p.perusahaan_id = @perusahaan_id"

            Dim cmdEmployees As New MySqlCommand(query, koneksi)
            cmdEmployees.Parameters.AddWithValue("@perusahaan_id", perusahaanId)

            Dim reader As MySqlDataReader = cmdEmployees.ExecuteReader()

            Dim employeeCount As Integer = 0
            While reader.Read()
                ' Create employee card dynamically
                Dim card As Guna.UI2.WinForms.Guna2Panel = CreateEmployeeCard(
                    reader("nama").ToString(),
                    reader("wp_npwp").ToString(),
                    reader("jabatan").ToString(),
                    reader("status_ptkp").ToString(),
                    employeeCount
                )

                FlowEmployees.Controls.Add(card)
                employeeCount += 1
            End While

            reader.Close()
            TutupKoneksi()

            If employeeCount = 0 Then
                ' Show message if no employees found
                Dim lblNoData As New Guna.UI2.WinForms.Guna2HtmlLabel()
                lblNoData.Text = "Tidak ada pegawai yang terdaftar untuk perusahaan ini."
                lblNoData.Font = New Font("Segoe UI", 10, FontStyle.Regular)
                lblNoData.ForeColor = Color.Gray
                lblNoData.AutoSize = True
                lblNoData.Margin = New Padding(20)
                FlowEmployees.Controls.Add(lblNoData)
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading employees: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If koneksi IsNot Nothing AndAlso koneksi.State = ConnectionState.Open Then
                TutupKoneksi()
            End If
        End Try
    End Sub

    ' Create employee card dynamically
    Private Function CreateEmployeeCard(nama As String, npwp As String, jabatan As String, ptkpStatus As String, index As Integer) As Guna.UI2.WinForms.Guna2Panel
        ' Create main card panel
        Dim card As New Guna.UI2.WinForms.Guna2Panel()
        card.BorderColor = Color.FromArgb(230, 233, 241)
        card.BorderRadius = 12
        card.BorderThickness = 1
        card.FillColor = Color.White
        card.Size = New Size(450, 140)
        card.Margin = New Padding(4)
        card.Padding = New Padding(16)

        ' Create picture box (avatar)
        Dim pic As New Guna.UI2.WinForms.Guna2CirclePictureBox()
        pic.Location = New Point(16, 32)
        pic.Size = New Size(72, 72)
        pic.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle
        ' Assign different colors based on index
        Dim colors As Color() = {
            Color.FromArgb(236, 252, 203),
            Color.FromArgb(219, 234, 254),
            Color.FromArgb(221, 214, 254),
            Color.FromArgb(254, 242, 242)
        }
        pic.FillColor = colors(index Mod 4)

        ' Create name label
        Dim lblName As New Guna.UI2.WinForms.Guna2HtmlLabel()
        lblName.Text = nama
        lblName.Font = New Font("Segoe UI Semibold", 11, FontStyle.Bold)
        lblName.ForeColor = Color.FromArgb(35, 44, 63)
        lblName.Location = New Point(104, 32)
        lblName.AutoSize = True

        ' Create NPWP label
        Dim lblNpwp As New Guna.UI2.WinForms.Guna2HtmlLabel()
        lblNpwp.Text = "NPWP " & npwp
        lblNpwp.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        lblNpwp.ForeColor = Color.FromArgb(71, 79, 99)
        lblNpwp.Location = New Point(104, 64)
        lblNpwp.AutoSize = True

        ' Create role label
        Dim lblRole As New Guna.UI2.WinForms.Guna2HtmlLabel()
        lblRole.Text = jabatan
        lblRole.Font = New Font("Segoe UI", 9, FontStyle.Regular)
        lblRole.ForeColor = Color.FromArgb(120, 128, 146)
        lblRole.Location = New Point(104, 96)
        lblRole.AutoSize = True

        ' Create "Lihat Profil" button
        Dim btnView As New Guna.UI2.WinForms.Guna2Button()
        btnView.Text = "Buat Bukti Potong"
        btnView.Font = New Font("Segoe UI Semibold", 8.5F, FontStyle.Bold)
        btnView.FillColor = Color.FromArgb(156, 0, 219)
        btnView.ForeColor = Color.White
        btnView.BorderRadius = 6
        btnView.Size = New Size(140, 32)
        btnView.Location = New Point(292, 90)
        btnView.Tag = ptkpStatus & "|" & npwp ' Store PTKP status and NPWP for later use
        AddHandler btnView.Click, AddressOf BtnCreateBuktiPotong_Click

        ' Add all controls to card
        card.Controls.Add(pic)
        card.Controls.Add(lblName)
        card.Controls.Add(lblNpwp)
        card.Controls.Add(lblRole)
        card.Controls.Add(btnView)

        Return card
    End Function

    ' Handle "Buat Bukti Potong" button click
    Private Sub BtnCreateBuktiPotong_Click(sender As Object, e As EventArgs)
        Dim btn As Guna.UI2.WinForms.Guna2Button = CType(sender, Guna.UI2.WinForms.Guna2Button)
        Dim tagData As String() = btn.Tag.ToString().Split("|"c)
        Dim ptkpStatus As String = tagData(0)
        Dim wpNPWP As String = tagData(1)

        ' Open pk_timeline_bukti_botong with employee data
        Dim formTimeline As New pk_timeline_bukti_botong()
        formTimeline.PTKPStatusValue = ptkpStatus
        formTimeline.EmployeeNPWP = wpNPWP
        formTimeline.Show()
        Me.Hide() ' Hide current form
    End Sub

    ' Search functionality
    Private Sub TxtSearch_TextChanged(sender As Object, e As EventArgs) Handles TxtSearch.TextChanged
        ' Filter employees based on search text
        Dim searchText As String = TxtSearch.Text.ToLower()

        For Each ctrl As Control In FlowEmployees.Controls
            If TypeOf ctrl Is Guna.UI2.WinForms.Guna2Panel Then
                Dim card As Guna.UI2.WinForms.Guna2Panel = CType(ctrl, Guna.UI2.WinForms.Guna2Panel)

                ' Find name and NPWP labels
                Dim shouldShow As Boolean = False
                For Each innerCtrl As Control In card.Controls
                    If TypeOf innerCtrl Is Guna.UI2.WinForms.Guna2HtmlLabel Then
                        Dim lbl As Guna.UI2.WinForms.Guna2HtmlLabel = CType(innerCtrl, Guna.UI2.WinForms.Guna2HtmlLabel)
                        If lbl.Text.ToLower().Contains(searchText) Then
                            shouldShow = True
                            Exit For
                        End If
                    End If
                Next

                card.Visible = shouldShow Or String.IsNullOrWhiteSpace(searchText)
            End If
        Next
    End Sub

    ' ====== NAVBAR EVENT HANDLERS ======
    Private Sub Pk_navbar11_DashboardClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.DashboardClicked
        Dim formDashboard As New pk_dashboard()
        formDashboard.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar11_DaftarPegawaiClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.DaftarPegawaiClicked
        ' Already on daftar pegawai, no action needed
    End Sub

    Private Sub Pk_navbar11_BuktiPotongClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.BuktiPotongClicked
        Dim formTimeline As New pk_timeline_bukti_botong()
        formTimeline.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar11_RiwayatClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.RiwayatClicked
        Dim formRiwayat As New pk_riwayat_bukti_potong()
        formRiwayat.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar11_LogoutClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.LogoutClicked
        Dim result As DialogResult = MessageBox.Show(
            "Apakah Anda yakin ingin keluar?",
            "Konfirmasi Logout",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            ' TODO: Implement logout logic (return to login form)
            Application.Exit()
        End If
    End Sub

End Class
