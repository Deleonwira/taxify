Imports MySql.Data.MySqlClient

Public Class wp_lapor_pajak

    Private _tahunPajak As Integer

    Private Sub wp_lapor_pajak_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Navigation event handlers
        AddHandler Wp_navbar1.DashboardClicked, AddressOf OnDashboardClicked
        AddHandler Wp_navbar1.LaporPajakClicked, AddressOf OnLaporPajakClicked
        AddHandler Wp_navbar1.RiwayatLaporClicked, AddressOf OnRiwayatLaporClicked
        AddHandler Wp_navbar1.TambahBuktiPotongClicked, AddressOf OnTambahBuktiPotongClicked
        AddHandler Wp_navbar1.TimelineBuktiPotongClicked, AddressOf OnTimelineBuktiPotongClicked
        AddHandler Wp_navbar1.RiwayatBuktiPotongClicked, AddressOf OnRiwayatBuktiPotongClicked
        AddHandler Wp_navbar1.DataDiriClicked, AddressOf OnDataDiriClicked
        AddHandler Wp_navbar1.LogoutClicked, AddressOf OnLogoutClicked
        AddHandler Wp_navbar1.ChatbotClicked, AddressOf OnChatbotClicked

        ' Set active menu
        Wp_navbar1.SetActiveMenu(wp_navbar.MenuType.LaporPajak)

        ' Set tahun pajak = current year (untuk bukti potong tahun berjalan)
        _tahunPajak = DateTime.Now.Year

        ' Refresh SPT calculation first (call stored procedure)
        RefreshSPTCalculation()

        ' Load calculated data from spt_tahunan
        LoadDataFromSPT()

        ' Set all calculated fields to read-only
        SetFieldsReadOnly()
    End Sub

    ' ========== REFRESH SPT CALCULATION ==========
    Private Sub RefreshSPTCalculation()
        Try
            modulkoneksi.BukaKoneksi()

            ' Call stored procedure to recalculate SPT
            Dim cmd As New MySqlCommand("sp_kalkulasi_spt_tahunan", modulkoneksi.koneksi)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@p_wajib_pajak_id", ModuleSession.CurrentWajibPajakId)
            cmd.Parameters.AddWithValue("@p_tahun_pajak", _tahunPajak)
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            ' Silent fail - SPT might not exist yet, will be created on save
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ' ========== LOAD DATA FROM SPT_TAHUNAN ==========
    Private Sub LoadDataFromSPT()
        Try
            modulkoneksi.BukaKoneksi()

            Dim sql As String = "SELECT * FROM spt_tahunan 
                                WHERE wajib_pajak_id = @wp_id AND tahun_pajak = @tahun 
                                LIMIT 1"

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@wp_id", ModuleSession.CurrentWajibPajakId)
            cmd.Parameters.AddWithValue("@tahun", _tahunPajak)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            If rd.Read() Then
                ' ===== Section: Info PTKP & Tahun =====
                ' Guna2TextBox10 = Status PTKP
                Guna2TextBox10.Text = If(IsDBNull(rd("status_ptkp")), "TK0", rd("status_ptkp").ToString())

                ' Guna2TextBox11 = Tahun Pajak
                Guna2TextBox11.Text = _tahunPajak.ToString()

                ' ===== Section: Penghasilan Bruto =====
                ' txtPPhTerutang = Gaji Pokok Setahun (misleading name from original)
                txtPPhTerutang.Text = If(IsDBNull(rd("gaji_setahun")), "0", Convert.ToDecimal(rd("gaji_setahun")).ToString("N0"))

                ' Guna2TextBox1 = Tunjangan Tetap Setahun
                Guna2TextBox1.Text = If(IsDBNull(rd("tunjangan_setahun")), "0", Convert.ToDecimal(rd("tunjangan_setahun")).ToString("N0"))

                ' Guna2TextBox3 = Bonus/THR Setahun
                Guna2TextBox3.Text = If(IsDBNull(rd("bonus_thr_setahun")), "0", Convert.ToDecimal(rd("bonus_thr_setahun")).ToString("N0"))

                ' Guna2TextBox4 = Jumlah Penghasilan Bruto
                Guna2TextBox4.Text = If(IsDBNull(rd("bruto_setahun")), "0", Convert.ToDecimal(rd("bruto_setahun")).ToString("N0"))

                ' ===== Section: Pengurangan =====
                ' Guna2TextBox8 = Biaya Jabatan Setahun
                Guna2TextBox8.Text = If(IsDBNull(rd("biaya_jabatan_setahun")), "0", Convert.ToDecimal(rd("biaya_jabatan_setahun")).ToString("N0"))

                ' Guna2TextBox7 = Iuran Pensiun (Zakat/Sumbangan field)
                Guna2TextBox7.Text = If(IsDBNull(rd("iuran_pensiun_setahun")), "0", Convert.ToDecimal(rd("iuran_pensiun_setahun")).ToString("N0"))

                ' Guna2TextBox5 = Total Pengurangan
                Dim biayaJabatan As Decimal = If(IsDBNull(rd("biaya_jabatan_setahun")), 0, Convert.ToDecimal(rd("biaya_jabatan_setahun")))
                Dim iuranPensiun As Decimal = If(IsDBNull(rd("iuran_pensiun_setahun")), 0, Convert.ToDecimal(rd("iuran_pensiun_setahun")))
                Guna2TextBox5.Text = (biayaJabatan + iuranPensiun).ToString("N0")

                ' ===== Section: Perhitungan Pajak SPT Tahunan =====
                ' Guna2TextBox9 = PTKP Nilai (nilai_tahunan dari master_ptkp)
                Guna2TextBox9.Text = If(IsDBNull(rd("ptkp")), "0", Convert.ToDecimal(rd("ptkp")).ToString("N0"))

                ' Guna2TextBox6 = PPh 21 Terutang
                Guna2TextBox6.Text = If(IsDBNull(rd("pph21_terutang")), "0", Convert.ToDecimal(rd("pph21_terutang")).ToString("N0"))

                ' Guna2TextBox2 = PPh 21 Dipotong
                Guna2TextBox2.Text = If(IsDBNull(rd("pph21_dipotong")), "0", Convert.ToDecimal(rd("pph21_dipotong")).ToString("N0"))

                ' ===== Update Status Badge =====
                Dim statusSpt As String = If(IsDBNull(rd("status_spt")), "Nihil", rd("status_spt").ToString())
                Dim kurangBayar As Decimal = If(IsDBNull(rd("pph21_kurang_bayar")), 0, Convert.ToDecimal(rd("pph21_kurang_bayar")))
                Dim lebihBayar As Decimal = If(IsDBNull(rd("pph21_lebih_bayar")), 0, Convert.ToDecimal(rd("pph21_lebih_bayar")))

                UpdateStatusBadge(statusSpt, kurangBayar, lebihBayar)
            Else
                ' No SPT data found - show empty form with just PTKP and Year
                LoadEmptyForm()
            End If

            rd.Close()

        Catch ex As Exception
            ' Silent fail - form can be used for new entry
            LoadEmptyForm()
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub LoadEmptyForm()
        ' Set tahun pajak
        Guna2TextBox11.Text = _tahunPajak.ToString()

        ' Load PTKP status from wajib_pajak
        Try
            modulkoneksi.BukaKoneksi()
            Dim sql As String = "SELECT status_ptkp FROM wajib_pajak WHERE id = @wp_id"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@wp_id", ModuleSession.CurrentWajibPajakId)
            Dim result = cmd.ExecuteScalar()
            Guna2TextBox10.Text = If(result Is Nothing OrElse IsDBNull(result), "TK0", result.ToString())
        Catch ex As Exception
            Guna2TextBox10.Text = "TK0"
        Finally
            modulkoneksi.TutupKoneksi()
        End Try

        ' Set all numeric fields to 0
        txtPPhTerutang.Text = "0"
        Guna2TextBox1.Text = "0"
        Guna2TextBox3.Text = "0"
        Guna2TextBox4.Text = "0"
        Guna2TextBox5.Text = "0"
        Guna2TextBox6.Text = "0"
        Guna2TextBox7.Text = "0"
        Guna2TextBox8.Text = "0"
        Guna2TextBox9.Text = "0"
        Guna2TextBox2.Text = "0"

        ' Update status badge
        UpdateStatusBadge("Nihil", 0, 0)
    End Sub

    Private Sub UpdateStatusBadge(statusSpt As String, kurangBayar As Decimal, lebihBayar As Decimal)
        ' Update the BunifuPanel3 and Guna2HtmlLabel9 for status badge
        Select Case statusSpt
            Case "Kurang Bayar"
                BunifuPanel3.BackgroundColor = Color.FromArgb(192, 0, 0) ' Red
                Guna2HtmlLabel9.Text = "Kurang Bayar: " & kurangBayar.ToString("N0")
            Case "Lebih Bayar"
                BunifuPanel3.BackgroundColor = Color.FromArgb(0, 128, 0) ' Green
                Guna2HtmlLabel9.Text = "Lebih Bayar: " & lebihBayar.ToString("N0")
            Case Else ' Nihil
                BunifuPanel3.BackgroundColor = Color.FromArgb(100, 100, 100) ' Gray
                Guna2HtmlLabel9.Text = "Nihil"
        End Select
    End Sub

    Private Sub SetFieldsReadOnly()
        ' Set all calculated fields to read-only
        txtPPhTerutang.ReadOnly = True
        Guna2TextBox1.ReadOnly = True
        Guna2TextBox2.ReadOnly = True
        Guna2TextBox3.ReadOnly = True
        Guna2TextBox4.ReadOnly = True
        Guna2TextBox5.ReadOnly = True
        Guna2TextBox6.ReadOnly = True
        Guna2TextBox7.ReadOnly = True
        Guna2TextBox8.ReadOnly = True
        Guna2TextBox9.ReadOnly = True
        Guna2TextBox10.ReadOnly = True
        Guna2TextBox11.ReadOnly = True
    End Sub

    ' ========== SAVE ==========
    Private Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click
        SaveSPT()
    End Sub

    Private Sub btnKirim_Click(sender As Object, e As EventArgs) Handles btnKirim.Click
        SaveSPT()
    End Sub

    Private Sub SaveSPT()
        Try
            modulkoneksi.BukaKoneksi()

            ' Check if SPT record exists
            Dim sqlCheck As String = "SELECT id FROM spt_tahunan WHERE wajib_pajak_id = @wp_id AND tahun_pajak = @tahun"
            Dim cmdCheck As New MySqlCommand(sqlCheck, modulkoneksi.koneksi)
            cmdCheck.Parameters.AddWithValue("@wp_id", ModuleSession.CurrentWajibPajakId)
            cmdCheck.Parameters.AddWithValue("@tahun", _tahunPajak)
            Dim sptId = cmdCheck.ExecuteScalar()

            If sptId Is Nothing Then
                ' No SPT record found - call stored procedure to create one first
                RefreshSPTCalculation()

                ' Check again
                sptId = cmdCheck.ExecuteScalar()
                If sptId Is Nothing Then
                    MsgBox("Tidak ada data bukti potong untuk tahun " & _tahunPajak & ". Silakan input bukti potong terlebih dahulu.", MsgBoxStyle.Exclamation)
                    Return
                End If
            End If

            ' Update tanggal_lapor to mark as submitted
            Dim sqlUpdate As String = "UPDATE spt_tahunan SET tanggal_lapor = @tanggal WHERE wajib_pajak_id = @wp_id AND tahun_pajak = @tahun"
            Dim cmdUpdate As New MySqlCommand(sqlUpdate, modulkoneksi.koneksi)
            cmdUpdate.Parameters.AddWithValue("@tanggal", DateTime.Now)
            cmdUpdate.Parameters.AddWithValue("@wp_id", ModuleSession.CurrentWajibPajakId)
            cmdUpdate.Parameters.AddWithValue("@tahun", _tahunPajak)
            cmdUpdate.ExecuteNonQuery()

            MsgBox("SPT Tahunan berhasil disimpan!", MsgBoxStyle.Information)

            ' Navigate to riwayat lapor
            Dim f As New wp_riwayat_lapor_pajak()
            f.Show()
            Me.Close()

        Catch ex As Exception
            MsgBox("Error menyimpan SPT: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        ' Discard / Cancel
        Dim f As New wp_dashboard()
        f.Show()
        Me.Close()
    End Sub

    ' =============================
    '   NAVIGATION HANDLERS
    ' =============================
    Private Sub OnDashboardClicked(sender As Object, e As EventArgs)
        Dim f As New wp_dashboard()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnLaporPajakClicked(sender As Object, e As EventArgs)
        ' Already on this page
    End Sub

    Private Sub OnRiwayatLaporClicked(sender As Object, e As EventArgs)
        Dim f As New wp_riwayat_lapor_pajak()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnTambahBuktiPotongClicked(sender As Object, e As EventArgs)
        Dim f As New wp_tambah_bukti_potong()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnTimelineBuktiPotongClicked(sender As Object, e As EventArgs)
        Dim f As New wp_timeline_bukti_botong()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnRiwayatBuktiPotongClicked(sender As Object, e As EventArgs)
        Dim f As New wp_riwayat_bukti_potong()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnDataDiriClicked(sender As Object, e As EventArgs)
        Dim f As New wp_data_diri()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnLogoutClicked(sender As Object, e As EventArgs)
        ModuleSession.ClearSession()
        Dim f As New FrmLogin()
        f.Show()
        Me.Close()
    End Sub

    Private Sub OnChatbotClicked(sender As Object, e As EventArgs)
        Dim f As New wp_dashboard()
        f.AutoShowChatbot = True
        f.Show()
        Me.Hide()
    End Sub

    Private Sub BunifuPanel1_Click(sender As Object, e As EventArgs) Handles BunifuPanel1.Click

    End Sub
End Class