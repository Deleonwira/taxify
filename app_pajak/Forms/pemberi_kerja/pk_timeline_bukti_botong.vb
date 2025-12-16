Imports MySql.Data.MySqlClient

Public Class pk_timeline_bukti_botong

    ' Properties
    Public Property EmployeeWPId As Integer = 0  ' wajib_pajak.id
    Public Property EmployeePekerjaanId As Integer = 0  ' pekerjaan.id
    Public Property PTKPStatusValue As String = ""
    Public Property EmployeeName As String = ""
    Private SelectedMonth As Integer = 0
    Private SelectedYear As Integer = DateTime.Now.Year
    Private CurrentBuktiPotongId As Integer = 0  ' ID bukti potong jika sudah ada

    ' Form load - set active menu and load data
    Private Sub pk_timeline_bukti_botong_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Pk_navbar11.SetActiveMenu(pk_navbar1.MenuType.BuktiPotong)
        AddHandler Pk_navbar11.ProfilClicked, AddressOf OnProfilClicked
        LoadEmployeeName()
        LoadBuktiPotongStatus()
        
        ' Initialize UI
        LblSelectedMonth.Text = "Pilih Bulan"
        Guna2Button2.Text = "Buat Bukti Potong"
        Guna2Button2.Enabled = False
        BtnHapusLapor.Enabled = False
        BtnHapusLapor.Visible = False  ' Sembunyikan tombol hapus sampai ada bukti potong
        
        ' Add click handlers for month cards
        AddHandler MonthCardJan.Click, Sub() SelectMonth(1)
        AddHandler MonthCardFeb.Click, Sub() SelectMonth(2)
        AddHandler MonthCardMar.Click, Sub() SelectMonth(3)
        AddHandler MonthCardApr.Click, Sub() SelectMonth(4)
        AddHandler MonthCardMay.Click, Sub() SelectMonth(5)
        AddHandler MonthCardJun.Click, Sub() SelectMonth(6)
        AddHandler MonthCardJul.Click, Sub() SelectMonth(7)
        AddHandler MonthCardAug.Click, Sub() SelectMonth(8)
        AddHandler MonthCardSep.Click, Sub() SelectMonth(9)
        AddHandler MonthCardOct.Click, Sub() SelectMonth(10)
        AddHandler MonthCardNov.Click, Sub() SelectMonth(11)
        AddHandler MonthCardDec.Click, Sub() SelectMonth(12)
        
        ' Also add handlers for labels and indicators inside cards to bubble up click
        For Each ctrl As Control In FlowTimeline.Controls
            If TypeOf ctrl Is Guna.UI2.WinForms.Guna2Panel Then
                For Each child As Control In ctrl.Controls
                    AddHandler child.Click, Sub(s, args) SelectMonth(FlowTimeline.Controls.GetChildIndex(ctrl) + 1)
                Next
            End If
        Next
    End Sub

    ' Load Employee Name
    Private Sub LoadEmployeeName()
        If EmployeeWPId = 0 Then Return

        Try
            modulkoneksi.BukaKoneksi()
            Dim query As String = "SELECT nama FROM wajib_pajak WHERE id = @wp_id"
            Dim cmd As New MySqlCommand(query, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@wp_id", EmployeeWPId)
            
            Dim result = cmd.ExecuteScalar()
            If result IsNot Nothing Then
                EmployeeName = result.ToString()
                LblTitle.Text = "Timeline Bukti Potong - " & EmployeeName
            End If
        Catch ex As Exception
            MessageBox.Show("Error loading employee name: " & ex.Message)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ' Load Bukti Potong Status untuk setiap bulan (update indikator visual)
    Private Sub LoadBuktiPotongStatus()
        If EmployeePekerjaanId = 0 Then Return

        Try
            modulkoneksi.BukaKoneksi()
            Dim query As String = "SELECT masa_bulan FROM bukti_potong WHERE pekerjaan_id = @pekerjaan_id AND masa_tahun = @tahun"
            Dim cmd As New MySqlCommand(query, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@pekerjaan_id", EmployeePekerjaanId)
            cmd.Parameters.AddWithValue("@tahun", SelectedYear)
            
            Dim reader As MySqlDataReader = cmd.ExecuteReader()
            Dim existingMonths As New List(Of Integer)
            
            While reader.Read()
                existingMonths.Add(reader.GetInt32("masa_bulan"))
            End While
            reader.Close()
            
            ' Update indikator visual untuk setiap bulan
            UpdateMonthIndicator(IndicatorJan, existingMonths.Contains(1))
            UpdateMonthIndicator(IndicatorFeb, existingMonths.Contains(2))
            UpdateMonthIndicator(IndicatorMar, existingMonths.Contains(3))
            UpdateMonthIndicator(IndicatorApr, existingMonths.Contains(4))
            UpdateMonthIndicator(IndicatorMay, existingMonths.Contains(5))
            UpdateMonthIndicator(IndicatorJun, existingMonths.Contains(6))
            UpdateMonthIndicator(IndicatorJul, existingMonths.Contains(7))
            UpdateMonthIndicator(IndicatorAug, existingMonths.Contains(8))
            UpdateMonthIndicator(IndicatorSep, existingMonths.Contains(9))
            UpdateMonthIndicator(IndicatorOct, existingMonths.Contains(10))
            UpdateMonthIndicator(IndicatorNov, existingMonths.Contains(11))
            UpdateMonthIndicator(IndicatorDec, existingMonths.Contains(12))
            
        Catch ex As Exception
            MessageBox.Show("Error loading bukti potong status: " & ex.Message)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ' Update indikator warna bulan (hijau = sudah ada, abu-abu = belum ada)
    Private Sub UpdateMonthIndicator(indicator As Guna.UI2.WinForms.Guna2CirclePictureBox, exists As Boolean)
        If exists Then
            indicator.FillColor = Color.FromArgb(0, 219, 101)  ' Hijau
        Else
            indicator.FillColor = Color.FromArgb(203, 213, 225)  ' Abu-abu
        End If
    End Sub

    ' Check if bukti potong exists for selected month
    Private Function CheckBuktiPotongExists(month As Integer) As Boolean
        If EmployeePekerjaanId = 0 Then Return False
        
        Try
            modulkoneksi.BukaKoneksi()
            Dim query As String = "SELECT id, created_at FROM bukti_potong WHERE pekerjaan_id = @pekerjaan_id AND masa_bulan = @bulan AND masa_tahun = @tahun LIMIT 1"
            Dim cmd As New MySqlCommand(query, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@pekerjaan_id", EmployeePekerjaanId)
            cmd.Parameters.AddWithValue("@bulan", month)
            cmd.Parameters.AddWithValue("@tahun", SelectedYear)
            
            Dim reader As MySqlDataReader = cmd.ExecuteReader()
            If reader.Read() Then
                CurrentBuktiPotongId = reader.GetInt32("id")
                Dim createdAt As DateTime = reader.GetDateTime("created_at")
                LblTanggalInputValue.Text = createdAt.ToString("dd MMMM yyyy")
                reader.Close()
                Return True
            End If
            reader.Close()
            Return False
        Catch ex As Exception
            MessageBox.Show("Error checking bukti potong: " & ex.Message)
            Return False
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Function

    ' Handle month selection
    Private Sub SelectMonth(month As Integer)
        SelectedMonth = month
        Dim monthName As String = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)
        LblSelectedMonth.Text = monthName & " " & SelectedYear.ToString()
        
        ' Reset all cards visual
        ResetCardsVisual()
        
        ' Highlight selected card
        Dim card As Guna.UI2.WinForms.Guna2Panel = CType(FlowTimeline.Controls(month - 1), Guna.UI2.WinForms.Guna2Panel)
        card.BorderColor = Color.FromArgb(156, 0, 219)
        card.BorderThickness = 2
        card.FillColor = Color.FromArgb(240, 230, 255)
        
        ' Check if bukti potong exists for this month
        Dim exists As Boolean = CheckBuktiPotongExists(month)
        
        ' Update buttons based on existence
        Guna2Button2.Enabled = True
        If exists Then
            Guna2Button2.Text = "Edit Laporan"
            Guna2Button1.Visible = True  ' Show "Sudah Input" badge
            BtnHapusLapor.Enabled = True
            BtnHapusLapor.Visible = True  ' Show delete button
        Else
            Guna2Button2.Text = "Buat Bukti Potong"
            Guna2Button1.Visible = False  ' Hide "Sudah Input" badge
            LblTanggalInputValue.Text = "-"
            CurrentBuktiPotongId = 0
            BtnHapusLapor.Enabled = False
            BtnHapusLapor.Visible = False  ' Hide delete button
        End If
    End Sub

    Private Sub ResetCardsVisual()
        For Each ctrl As Control In FlowTimeline.Controls
            If TypeOf ctrl Is Guna.UI2.WinForms.Guna2Panel Then
                Dim card As Guna.UI2.WinForms.Guna2Panel = CType(ctrl, Guna.UI2.WinForms.Guna2Panel)
                card.BorderColor = Color.FromArgb(224, 231, 245)
                card.BorderThickness = 1
                card.FillColor = Color.White
            End If
        Next
    End Sub

    ' Handle "Buat Bukti Potong" / "Edit Laporan" button click
    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        If SelectedMonth = 0 Then
            MessageBox.Show("Silakan pilih bulan terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim formBuktiPotong As New pk_form_bukti_potong(Me)
        formBuktiPotong.EmployeeWPId = EmployeeWPId
        formBuktiPotong.EmployeePekerjaanId = EmployeePekerjaanId
        formBuktiPotong.PTKPStatusValue = PTKPStatusValue
        formBuktiPotong.SelectedMonth = SelectedMonth
        
        ' Jika edit mode, kirim ID bukti potong yang ada
        If CurrentBuktiPotongId > 0 Then
            formBuktiPotong.EditMode = True
            formBuktiPotong.BuktiPotongId = CurrentBuktiPotongId
        End If
        
        formBuktiPotong.Show()
        Hide()
    End Sub

    ' Handle "Hapus" button click - Delete bukti potong with confirmation
    Private Sub BtnHapusLapor_Click(sender As Object, e As EventArgs) Handles BtnHapusLapor.Click
        If SelectedMonth = 0 Then
            MessageBox.Show("Silakan pilih bulan terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        
        If CurrentBuktiPotongId = 0 Then
            MessageBox.Show("Tidak ada bukti potong untuk dihapus.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        
        ' Konfirmasi sebelum menghapus
        Dim monthName As String = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(SelectedMonth)
        Dim confirmResult As DialogResult = MessageBox.Show(
            "Apakah Anda yakin ingin menghapus bukti potong untuk bulan " & monthName & " " & SelectedYear.ToString() & "?" & vbCrLf & vbCrLf &
            "Data yang dihapus tidak dapat dikembalikan.",
            "Konfirmasi Hapus",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning)
        
        If confirmResult = DialogResult.Yes Then
            DeleteBuktiPotong()
        End If
    End Sub

    ' Delete bukti potong from database
    Private Sub DeleteBuktiPotong()
        Try
            modulkoneksi.BukaKoneksi()
            Dim query As String = "DELETE FROM bukti_potong WHERE id = @id"
            Dim cmd As New MySqlCommand(query, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@id", CurrentBuktiPotongId)
            
            Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
            
            If rowsAffected > 0 Then
                MessageBox.Show("Bukti potong berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
                
                ' Refresh tampilan
                LoadBuktiPotongStatus()
                SelectMonth(SelectedMonth)  ' Re-select to update UI
            Else
                MessageBox.Show("Gagal menghapus bukti potong.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("Error menghapus bukti potong: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ' ====== NAVBAR EVENT HANDLERS ======
    Private Sub Pk_navbar11_DashboardClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.DashboardClicked
        Dim formDashboard As New pk_dashboard()
        formDashboard.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar11_DaftarPegawaiClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.DaftarPegawaiClicked
        Dim formDaftarPegawai As New pk_daftar_pegawai()
        formDaftarPegawai.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar11_BuktiPotongClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.BuktiPotongClicked
        ' Already on bukti potong timeline, no action needed
    End Sub

    Private Sub Pk_navbar11_RiwayatClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.RiwayatClicked
        Dim formRiwayat As New pk_riwayat_bukti_potong()
        formRiwayat.Show()
        Me.Close()
    End Sub

    Private Sub OnProfilClicked(sender As Object, e As EventArgs)
        Dim f As New pk_profil()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar11_LogoutClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.LogoutClicked
        Dim result As DialogResult = MessageBox.Show(
            "Apakah Anda yakin ingin keluar?",
            "Konfirmasi Logout",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)
        
        If result = DialogResult.Yes Then
            ModuleSession.ClearSession()
            Dim loginForm As New FrmLogin()
            loginForm.Show()
            Me.Close()
        End If
    End Sub

End Class