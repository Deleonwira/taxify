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
        ' TODO: Load user basic info
        Try
            modulkoneksi.BukaKoneksi()
            Dim sql As String = "SELECT u.npwp, u.nama, p.alamat FROM users u 
                                LEFT JOIN profil_wp p ON u.id = p.user_id 
                                WHERE u.id = @uid"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@uid", ModuleSession.CurrentUserId)
            Dim rd As MySqlDataReader = cmd.ExecuteReader()
            
            If rd.Read() Then
                ' TODO: Populate labels/textboxes with user data
                ' lblNPWP.Text = rd("npwp")
                ' txtNama.Text = rd("nama")
                ' txtAlamat.Text = rd("alamat")
            End If
            rd.Close()
        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub SetDefaultValues()
        ' TODO: Set default tahun pajak = current year - 1
        ' cmbJenisSPT.SelectedIndex = 0
        ' txtTahun.Text = (DateTime.Now.Year - 1).ToString()
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
            modulkoneksi.BukaKoneksi()
            
            Dim sql As String = "INSERT INTO spt_tahunan (
                user_id, tahun_pajak, jenis_spt, npwp, nama_wp, alamat_wp,
                gaji_pokok, tunjangan_tetap, bonus_thr, jumlah_bruto,
                biaya_jabatan, zakat_sumbangan, total_pengurangan,
                penghasilan_netto, pajak_pph, status, tanggal_lapor
            ) VALUES (
                @uid, @tahun, @jenis, @npwp, @nama, @alamat,
                @gaji, @tunjangan, @bonus, @bruto,
                @biaya, @zakat, @pengurangan,
                @netto, @pph, @status, @tanggal_lapor
            )"
            
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@uid", ModuleSession.CurrentUserId)
            ' TODO: Add all parameters from form fields
            cmd.Parameters.AddWithValue("@tahun", "TODO_FROM_TEXTBOX")
            cmd.Parameters.AddWithValue("@jenis", "TODO_FROM_COMBOBOX")
            cmd.Parameters.AddWithValue("@status", status)
            If status = "terkirim" Then
                cmd.Parameters.AddWithValue("@tanggal_lapor", DateTime.Now)
            Else
                cmd.Parameters.AddWithValue("@tanggal_lapor", DBNull.Value)
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

End Class