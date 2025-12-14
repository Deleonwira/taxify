Imports MySql.Data.MySqlClient
Imports Guna.UI2.WinForms

Public Class wp_tambah_bukti_potong

    Private Sub wp_tambah_bukti_potong_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set user name (read-only)
        txtNama.Text = ModuleSession.CurrentUserName

        ' Setup Jenis Freelance dropdown - Only freelance options
        cboJenisFreelance.Items.Clear()
        cboJenisFreelance.Items.Add("Freelance Tenaga Ahli")  ' Index 0 - Non-Final (masuk SPT)
        cboJenisFreelance.Items.Add("Freelance Harian")       ' Index 1 - Final (tidak masuk SPT)
        cboJenisFreelance.SelectedIndex = 0  ' Default: Tenaga Ahli

        ' Setup Bulan dropdown
        cboBulan.Items.Clear()
        Dim bulanNames() As String = {"Januari", "Februari", "Maret", "April", "Mei", "Juni",
                                       "Juli", "Agustus", "September", "Oktober", "November", "Desember"}
        For Each bulan In bulanNames
            cboBulan.Items.Add(bulan)
        Next
        cboBulan.SelectedIndex = DateTime.Now.Month - 1  ' Current month

        ' Set current year
        txtTahun.Text = DateTime.Now.Year.ToString()

        ' Apply initial toggle
        ToggleInputs()
    End Sub

    Private Sub cboJenisFreelance_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboJenisFreelance.SelectedIndexChanged
        ToggleInputs()
        ClearCalculations()
    End Sub

    Private Sub ToggleInputs()
        Dim isTenagaAhli As Boolean = (cboJenisFreelance.SelectedIndex = 0)
        Dim isHarian As Boolean = (cboJenisFreelance.SelectedIndex = 1)

        ' Tenaga Ahli: Show total bruto input
        lblBrutoTotal.Visible = isTenagaAhli
        txtBrutoTotal.Visible = isTenagaAhli
        lblBrutoTotal.Text = "Total Penghasilan Bruto *"

        ' Harian: Show per-day inputs
        lblBrutoPerHari.Visible = isHarian
        txtBrutoPerHari.Visible = isHarian
        lblJumlahHariKerja.Visible = isHarian
        txtJumlahHariKerja.Visible = isHarian

        ' For Harian, reposition total bruto as calculated result
        If isHarian Then
            lblBrutoTotal.Location = New Point(541, 340)
            txtBrutoTotal.Location = New Point(541, 360)
            lblBrutoTotal.Text = "Total Bruto (Hasil)"
            txtBrutoTotal.Enabled = False
            txtBrutoTotal.FillColor = Color.FromArgb(235, 236, 240)
            lblBrutoTotal.Visible = True
            txtBrutoTotal.Visible = True
        Else
            lblBrutoTotal.Location = New Point(541, 270)
            txtBrutoTotal.Location = New Point(541, 290)
            txtBrutoTotal.Enabled = True
            txtBrutoTotal.FillColor = Color.FromArgb(245, 246, 250)
        End If
    End Sub

    Private Sub ClearCalculations()
        txtBrutoTotal.Text = ""
        txtDPP.Text = ""
        txtTarif.Text = ""
        txtPPhDipotong.Text = ""
    End Sub

    Private Sub btnHitung_Click(sender As Object, e As EventArgs) Handles btnHitung.Click
        CalculateTax()
    End Sub

    Private Sub CalculateTax()
        Try
            If cboJenisFreelance.SelectedIndex = 0 Then
                ' Tenaga Ahli (Non-Final) - 50% DPP, progressive rate
                Dim brutoTotal As Decimal = ModulePajak.ParseCurrency(txtBrutoTotal.Text)

                If brutoTotal <= 0 Then
                    MessageBox.Show("Masukkan Penghasilan Bruto terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' DPP = 50% of Bruto for Tenaga Ahli
                Dim dpp As Decimal = brutoTotal * 0.5D

                ' Calculate PPh using progressive rate on DPP
                Dim pph As Decimal = ModulePajak.CalculateFreelanceNonFinal(brutoTotal)

                ' Calculate effective rate
                Dim tarif As Decimal = If(brutoTotal > 0, (pph / brutoTotal) * 100, 0)

                ' Display results
                txtDPP.Text = dpp.ToString("N0")
                txtTarif.Text = tarif.ToString("F2")
                txtPPhDipotong.Text = pph.ToString("N0")

            ElseIf cboJenisFreelance.SelectedIndex = 1 Then
                ' Harian (Final) - 0.5% of total
                Dim brutoPerHari As Decimal = ModulePajak.ParseCurrency(txtBrutoPerHari.Text)
                Dim jumlahHari As Integer = 0
                Integer.TryParse(txtJumlahHariKerja.Text, jumlahHari)

                If brutoPerHari <= 0 OrElse jumlahHari <= 0 Then
                    MessageBox.Show("Masukkan Bruto Per Hari dan Jumlah Hari Kerja.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                Dim totalBruto As Decimal = brutoPerHari * jumlahHari
                Dim pph As Decimal = ModulePajak.CalculateFreelanceFinal(brutoPerHari, jumlahHari)

                ' DPP for Harian is same as total (final tax)
                Dim dpp As Decimal = totalBruto

                ' Fixed rate 0.5%
                Dim tarif As Decimal = 0.5D

                ' Display results
                txtBrutoTotal.Text = totalBruto.ToString("N0")
                txtDPP.Text = dpp.ToString("N0")
                txtTarif.Text = tarif.ToString("F2")
                txtPPhDipotong.Text = pph.ToString("N0")
            End If

        Catch ex As Exception
            MessageBox.Show("Error saat menghitung: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        ResetForm()
    End Sub

    Private Sub ResetForm()
        txtNamaPemberiKerja.Text = ""
        txtNpwpPemberiKerja.Text = ""
        txtBrutoPerHari.Text = ""
        txtJumlahHariKerja.Text = ""
        txtBrutoTotal.Text = ""
        ClearCalculations()
        cboJenisFreelance.SelectedIndex = 0
        cboBulan.SelectedIndex = DateTime.Now.Month - 1
        txtTahun.Text = DateTime.Now.Year.ToString()
    End Sub

    Private Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click
        ' Validate required fields
        If String.IsNullOrWhiteSpace(txtNamaPemberiKerja.Text) Then
            MessageBox.Show("Nama Pemberi Kerja harus diisi.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtNamaPemberiKerja.Focus()
            Return
        End If

        If String.IsNullOrWhiteSpace(txtPPhDipotong.Text) OrElse txtPPhDipotong.Text = "0" Then
            MessageBox.Show("Klik tombol 'Hitung' terlebih dahulu untuk menghitung PPh.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        SaveFreelanceData()
    End Sub

    Private Sub SaveFreelanceData()
        Try
            Using conn As New MySqlConnection("server=localhost;user=root;password=;database=app_pajak_v2")
                conn.Open()

                Dim cmd As New MySqlCommand()
                cmd.Connection = conn

                ' Generate nomor bukti
                Dim nomorBukti As String = $"BPF-{DateTime.Now.Year}-{(cboBulan.SelectedIndex + 1):D2}-{New Random().Next(1000, 9999)}"

                cmd.CommandText = "INSERT INTO bukti_potong_freelance " &
                                  "(wajib_pajak_id, nomor_bukti, jenis_freelance, is_pph_final, masa_tahun, masa_bulan, " &
                                  "nama_pemberi_kerja, npwp_pemberi_kerja, bruto_per_hari, jumlah_hari_kerja, " &
                                  "bruto_total, dpp, tarif_persen, pph_dipotong) " &
                                  "VALUES (@wp_id, @nomor, @jenis, @is_final, @tahun, @bulan, " &
                                  "@nama_pk, @npwp_pk, @bruto_hari, @hari, @total, @dpp, @tarif, @pph)"

                cmd.Parameters.AddWithValue("@wp_id", ModuleSession.CurrentWajibPajakId)
                cmd.Parameters.AddWithValue("@nomor", nomorBukti)

                ' Jenis freelance based on selection
                Dim jenisFreelance As String = If(cboJenisFreelance.SelectedIndex = 0, "tenaga_ahli", "harian")
                cmd.Parameters.AddWithValue("@jenis", jenisFreelance)

                ' Is PPh Final: 0 for Tenaga Ahli (included in SPT), 1 for Harian (not included)
                cmd.Parameters.AddWithValue("@is_final", If(cboJenisFreelance.SelectedIndex = 1, 1, 0))

                ' Masa pajak
                Dim tahun As Integer = Integer.Parse(txtTahun.Text)
                cmd.Parameters.AddWithValue("@tahun", tahun)
                cmd.Parameters.AddWithValue("@bulan", cboBulan.SelectedIndex + 1)

                ' Pemberi kerja
                cmd.Parameters.AddWithValue("@nama_pk", txtNamaPemberiKerja.Text.Trim())
                cmd.Parameters.AddWithValue("@npwp_pk", If(String.IsNullOrWhiteSpace(txtNpwpPemberiKerja.Text), DBNull.Value, txtNpwpPemberiKerja.Text.Trim()))

                ' Penghasilan values
                Dim brutoPerHari As Decimal = ModulePajak.ParseCurrency(txtBrutoPerHari.Text)
                Dim jumlahHari As Integer = 0
                Integer.TryParse(txtJumlahHariKerja.Text, jumlahHari)
                Dim brutoTotal As Decimal = ModulePajak.ParseCurrency(txtBrutoTotal.Text)
                Dim dpp As Decimal = ModulePajak.ParseCurrency(txtDPP.Text)
                Dim tarif As Decimal = 0
                Decimal.TryParse(txtTarif.Text, tarif)
                Dim pph As Decimal = ModulePajak.ParseCurrency(txtPPhDipotong.Text)

                cmd.Parameters.AddWithValue("@bruto_hari", brutoPerHari)
                cmd.Parameters.AddWithValue("@hari", jumlahHari)
                cmd.Parameters.AddWithValue("@total", brutoTotal)
                cmd.Parameters.AddWithValue("@dpp", dpp)
                cmd.Parameters.AddWithValue("@tarif", tarif)
                cmd.Parameters.AddWithValue("@pph", pph)

                cmd.ExecuteNonQuery()
                MessageBox.Show($"Data bukti potong freelance berhasil disimpan!{vbCrLf}Nomor Bukti: {nomorBukti}", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ResetForm()
            End Using

        Catch ex As Exception
            MessageBox.Show("Error menyimpan data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class