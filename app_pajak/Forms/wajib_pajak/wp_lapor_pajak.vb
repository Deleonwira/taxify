Imports MySql.Data.MySqlClient

Public Class wp_lapor_pajak

    Private Sub wp_lapor_pajak_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Navigation event handlers
        AddHandler Wp_navbar1.DashboardClicked, AddressOf OnDashboardClicked
        AddHandler Wp_navbar1.LaporPajakClicked, AddressOf OnLaporPajakClicked
        AddHandler Wp_navbar1.RiwayatLaporClicked, AddressOf OnRiwayatLaporClicked
        AddHandler Wp_navbar1.TimelineBuktiPotongClicked, AddressOf OnTimelineBuktiPotongClicked
        AddHandler Wp_navbar1.RiwayatBuktiPotongClicked, AddressOf OnRiwayatBuktiPotongClicked
        AddHandler Wp_navbar1.DataDiriClicked, AddressOf OnDataDiriClicked
        AddHandler Wp_navbar1.LogoutClicked, AddressOf OnLogoutClicked

        ' Set active menu
        Wp_navbar1.SetActiveMenu(wp_navbar.MenuType.LaporPajak)

        LoadUserData()
        SetDefaultValues()
    End Sub

    Private Sub LoadUserData()
        ' Load user basic info
        Try
            modulkoneksi.BukaKoneksi()
            Dim sql As String = "SELECT npwp, nama, alamat FROM users WHERE npwp = @npwp"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", ModuleSession.CurrentUserNPWP)
            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            If rd.Read() Then
                ' User data can be displayed if needed
                ' Currently no labels for this in the form
            End If
            rd.Close()
        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub SetDefaultValues()
        ' Set default tahun pajak = current year - 1 (FIX, tidak bisa dirubah)
        Dim tahunPajak As Integer = DateTime.Now.Year - 1

        ' Load PTKP from pekerjaan table
        LoadPTKP()

        ' Load data from spt_tahunan if exists
        LoadDataFromSPT(tahunPajak)
    End Sub

    ' ========== LOAD PTKP FROM PEKERJAAN ==========
    Private Sub LoadPTKP()
        Try
            modulkoneksi.BukaKoneksi()
            Dim sql As String = "SELECT status_ptkp FROM pekerjaan WHERE wp_npwp = @npwp LIMIT 1"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", ModuleSession.CurrentUserNPWP)
            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            If rd.Read() Then
                Dim statusPTKP As String = If(IsDBNull(rd("status_ptkp")), "", rd("status_ptkp").ToString())
                ' Display PTKP in label if exists (Guna2HtmlLabel1 is for PTKP)
                ' If there's a label for PTKP, set it here
                ' Guna2HtmlLabel1.Text = If(String.IsNullOrEmpty(statusPTKP), "-", statusPTKP)
            End If
            rd.Close()
        Catch ex As Exception
            ' Silent fail
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ' ========== LOAD DATA FROM SPT_TAHUNAN ==========
    Private Sub LoadDataFromSPT(tahunPajak As Integer)
        Try
            If String.IsNullOrWhiteSpace(ModuleSession.CurrentUserNPWP) Then
                Return
            End If

            modulkoneksi.BukaKoneksi()

            Dim sql As String = "SELECT * FROM spt_tahunan 
                                WHERE wp_npwp = @npwp AND tahun_pajak = @tahun 
                                LIMIT 1"

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", ModuleSession.CurrentUserNPWP)
            cmd.Parameters.AddWithValue("@tahun", tahunPajak)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            If rd.Read() Then
                ' Populate form fields from database sesuai struktur tabel spt_tahunan
                ' Gaji Pokok (gaji_setahun)
                txtPPhTerutang.Text = If(IsDBNull(rd("gaji_setahun")), "0", Convert.ToDecimal(rd("gaji_setahun")).ToString("N0"))

                ' Tunjangan Tetap (tunjangan_setahun)
                Guna2TextBox1.Text = If(IsDBNull(rd("tunjangan_setahun")), "0", Convert.ToDecimal(rd("tunjangan_setahun")).ToString("N0"))

                ' Bonus/THR (bonus_thr_setahun)
                Guna2TextBox3.Text = If(IsDBNull(rd("bonus_thr_setahun")), "0", Convert.ToDecimal(rd("bonus_thr_setahun")).ToString("N0"))

                ' Jumlah Bruto (bruto_setahun)
                Guna2TextBox4.Text = If(IsDBNull(rd("bruto_setahun")), "0", Convert.ToDecimal(rd("bruto_setahun")).ToString("N0"))

                ' Biaya Jabatan (biaya_jabatan_setahun)
                Guna2TextBox8.Text = If(IsDBNull(rd("biaya_jabatan_setahun")), "0", Convert.ToDecimal(rd("biaya_jabatan_setahun")).ToString("N0"))

                ' Iuran Pensiun (iuran_pensiun_setahun) - mapped to Zakat/Sumbangan field
                Guna2TextBox7.Text = If(IsDBNull(rd("iuran_pensiun_setahun")), "0", Convert.ToDecimal(rd("iuran_pensiun_setahun")).ToString("N0"))

                ' Total Pengurangan (biaya jabatan + iuran pensiun)
                Dim biayaJabatan As Decimal = If(IsDBNull(rd("biaya_jabatan_setahun")), 0, Convert.ToDecimal(rd("biaya_jabatan_setahun")))
                Dim iuranPensiun As Decimal = If(IsDBNull(rd("iuran_pensiun_setahun")), 0, Convert.ToDecimal(rd("iuran_pensiun_setahun")))
                Guna2TextBox5.Text = (biayaJabatan + iuranPensiun).ToString("N0")

                ' Penghasilan Netto (netto_setahun)
                Guna2TextBox9.Text = If(IsDBNull(rd("netto_setahun")), "0", Convert.ToDecimal(rd("netto_setahun")).ToString("N0"))

                ' Pajak PPh (pph21_terutang)
                Guna2TextBox6.Text = If(IsDBNull(rd("pph21_terutang")), "0", Convert.ToDecimal(rd("pph21_terutang")).ToString("N0"))

                ' Total Pengurangan (Hasil section) - same as above
                Guna2TextBox2.Text = (biayaJabatan + iuranPensiun).ToString("N0")
            End If

            rd.Close()

        Catch ex As Exception
            ' Silent fail - form can be used for new entry
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ' ========== AUTO CALCULATION ==========
    Private Sub CalculateJumlahBruto()
        ' TODO: Sum all penghasilan bruto fields
        ' jumlah_bruto = gaji_pokok + tunjangan_tetap + bonus_thr
    End Sub

    Private Sub CalculateTotalPengurangan()
        ' TODO: Calculate total pengurangan
        ' total_pengurangan = biaya_jabatan + zakat_sumbangan
    End Sub

    Private Sub CalculateHasil()
        ' TODO: Calculate hasil
        ' penghasilan_netto = jumlah_bruto - total_pengurangan

        ' TODO: Get PTKP from database based on user's status
        ' Then: pajak_pph = Calculate using progressive tax rates from tarif_pajak table
    End Sub

    ' ========== SAVE ==========
    Private Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click
        SaveSPT(status:="draft")
    End Sub

    Private Sub btnKirim_Click(sender As Object, e As EventArgs) Handles btnKirim.Click
        SaveSPT(status:="terkirim")
    End Sub

    Private Sub SaveSPT(status As String)
        Try
            ' Tahun pajak fix = current year - 1
            Dim tahunPajak As Integer = DateTime.Now.Year - 1

            ' Helper function to parse decimal from formatted text
            Dim parseDecimal As Func(Of String, Decimal) = Function(text As String) As Decimal
                                                               If String.IsNullOrWhiteSpace(text) Then Return 0
                                                               ' Remove thousand separators (dots and commas)
                                                               Dim cleanText As String = text.Replace(".", "").Replace(",", "").Trim()
                                                               Dim result As Decimal = 0
                                                               Decimal.TryParse(cleanText, result)
                                                               Return result
                                                           End Function

            ' Get values from form
            Dim gajiPokok As Decimal = parseDecimal(txtPPhTerutang.Text)
            Dim tunjangan As Decimal = parseDecimal(Guna2TextBox1.Text)
            Dim bonusThr As Decimal = parseDecimal(Guna2TextBox3.Text)
            Dim bruto As Decimal = parseDecimal(Guna2TextBox4.Text)
            Dim biayaJabatan As Decimal = parseDecimal(Guna2TextBox8.Text)
            Dim iuranPensiun As Decimal = parseDecimal(Guna2TextBox7.Text)
            Dim netto As Decimal = parseDecimal(Guna2TextBox9.Text)
            Dim pphTerutang As Decimal = parseDecimal(Guna2TextBox6.Text)

            modulkoneksi.BukaKoneksi()

            ' Get PTKP from pekerjaan table (REQUIRED)
            Dim statusPTKP As String = ""
            Try
                Dim sqlPTKP As String = "SELECT status_ptkp FROM pekerjaan WHERE wp_npwp = @npwp LIMIT 1"
                Dim cmdPTKP As New MySqlCommand(sqlPTKP, modulkoneksi.koneksi)
                cmdPTKP.Parameters.AddWithValue("@npwp", ModuleSession.CurrentUserNPWP)
                Dim rdPTKP As MySqlDataReader = cmdPTKP.ExecuteReader()
                If rdPTKP.Read() Then
                    statusPTKP = If(IsDBNull(rdPTKP("status_ptkp")), "", rdPTKP("status_ptkp").ToString())
                End If
                rdPTKP.Close()

                If String.IsNullOrEmpty(statusPTKP) Then
                    MsgBox("Status PTKP tidak ditemukan di data pekerjaan. Silakan lengkapi data pekerjaan terlebih dahulu.", MsgBoxStyle.Exclamation)
                    Return
                End If
            Catch ex As Exception
                MsgBox("Error mengambil data PTKP: " & ex.Message, MsgBoxStyle.Critical)
                Return
            End Try

            ' Check if record exists
            Dim sqlCheck As String = "SELECT id FROM spt_tahunan WHERE wp_npwp = @npwp AND tahun_pajak = @tahun"
            Dim cmdCheck As New MySqlCommand(sqlCheck, modulkoneksi.koneksi)
            cmdCheck.Parameters.AddWithValue("@npwp", ModuleSession.CurrentUserNPWP)
            cmdCheck.Parameters.AddWithValue("@tahun", tahunPajak)
            Dim exists As Boolean = cmdCheck.ExecuteScalar() IsNot Nothing

            Dim sql As String
            If exists Then
                ' UPDATE existing record
                sql = "UPDATE spt_tahunan SET 
                       status_ptkp = @ptkp,
                       gaji_setahun = @gaji,
                       tunjangan_setahun = @tunjangan,
                       bonus_thr_setahun = @bonus,
                       bruto_setahun = @bruto,
                       biaya_jabatan_setahun = @biaya,
                       iuran_pensiun_setahun = @iuran,
                       netto_setahun = @netto,
                       pph21_terutang = @pph"

                If status = "terkirim" Then
                    sql &= ", tanggal_lapor = @tanggal"
                End If

                sql &= " WHERE wp_npwp = @npwp AND tahun_pajak = @tahun"
            Else
                ' INSERT new record
                sql = "INSERT INTO spt_tahunan (
                    wp_npwp, tahun_pajak, status_ptkp,
                    gaji_setahun, tunjangan_setahun, bonus_thr_setahun, bruto_setahun,
                    biaya_jabatan_setahun, iuran_pensiun_setahun, netto_setahun,
                    pph21_terutang"

                If status = "terkirim" Then
                    sql &= ", tanggal_lapor"
                End If

                sql &= ") VALUES (
                    @npwp, @tahun, @ptkp,
                    @gaji, @tunjangan, @bonus, @bruto,
                    @biaya, @iuran, @netto,
                    @pph"

                If status = "terkirim" Then
                    sql &= ", @tanggal"
                End If

                sql &= ")"
            End If

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", ModuleSession.CurrentUserNPWP)
            cmd.Parameters.AddWithValue("@tahun", tahunPajak)
            cmd.Parameters.AddWithValue("@ptkp", If(String.IsNullOrEmpty(statusPTKP), DBNull.Value, statusPTKP))
            cmd.Parameters.AddWithValue("@gaji", gajiPokok)
            cmd.Parameters.AddWithValue("@tunjangan", tunjangan)
            cmd.Parameters.AddWithValue("@bonus", bonusThr)
            cmd.Parameters.AddWithValue("@bruto", bruto)
            cmd.Parameters.AddWithValue("@biaya", biayaJabatan)
            cmd.Parameters.AddWithValue("@iuran", iuranPensiun)
            cmd.Parameters.AddWithValue("@netto", netto)
            cmd.Parameters.AddWithValue("@pph", pphTerutang)

            If status = "terkirim" Then
                cmd.Parameters.AddWithValue("@tanggal", DateTime.Now)
            End If

            cmd.ExecuteNonQuery()

            MsgBox("SPT berhasil disimpan!", MsgBoxStyle.Information)
            Me.Close()

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        ' Discard
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

    Private Sub BunifuPanel1_Click(sender As Object, e As EventArgs) Handles BunifuPanel1.Click

    End Sub
End Class