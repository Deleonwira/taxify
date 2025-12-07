
Imports MySql.Data.MySqlClient

Public Class pk_form_bukti_potong

    ' Public properties untuk menerima data dari form sebelumnya
    Public Property PTKPStatusValue As String = ""
    Public Property PTKPBulananValue As Decimal = 0
    Public Property EmployeeNPWP As String = ""
    Public Property EmployeeName As String = ""
    Public Property SelectedMonth As Integer = 0

    ' Variable untuk menyimpan PKP Bulanan dan parent form reference
    Private pkp_bulanan As Decimal = 0
    Private parentForm As Form = Nothing
    Private isCalculating As Boolean = False  ' Flag untuk mencegah loop kalkulasi

    ' Constructor untuk menerima parent form
    Public Sub New(Optional parent As Form = Nothing)
        InitializeComponent()
        parentForm = parent
    End Sub

    ' NOTE: PTKP calculation functions moved to ModulePajak.vb for reusability

    ' Load Employee Name from Database
    Private Sub LoadEmployeeName()
        If String.IsNullOrEmpty(EmployeeNPWP) Then Return

        Try
            modulkoneksi.BukaKoneksi()
            Dim query As String = "SELECT nama FROM users WHERE npwp = @npwp"
            Dim cmd As New MySqlCommand(query, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", EmployeeNPWP)

            Dim result = cmd.ExecuteScalar()
            If result IsNot Nothing Then
                EmployeeName = result.ToString()
                Guna2TextBox11.Text = EmployeeName
            End If
        Catch ex As Exception
            MessageBox.Show("Error loading employee name: " & ex.Message)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ' Load PTKP status from database (pekerjaan table) based on EmployeeNPWP
    Private Sub LoadPTKPFromDatabase()
        If String.IsNullOrEmpty(EmployeeNPWP) Then Return
        If Not String.IsNullOrEmpty(PTKPStatusValue) Then Return ' Already has value from previous form

        Try
            modulkoneksi.BukaKoneksi()
            Dim query As String = "SELECT status_ptkp FROM pekerjaan WHERE wp_npwp = @npwp LIMIT 1"
            Dim cmd As New MySqlCommand(query, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", EmployeeNPWP)

            Dim result = cmd.ExecuteScalar()
            If result IsNot Nothing AndAlso result IsNot DBNull.Value Then
                PTKPStatusValue = result.ToString()
            End If
        Catch ex As Exception
            MessageBox.Show("Error loading PTKP from database: " & ex.Message)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ' Event form load - Set PTKP dan field properties
    Private Sub pk_form_bukti_potong_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Pk_navbar11.SetActiveMenu(pk_navbar1.MenuType.BuktiPotong)

        ' Load Employee Name
        LoadEmployeeName()
        Guna2TextBox11.ReadOnly = True
        Guna2TextBox11.FillColor = Color.FromArgb(226, 226, 226)

        ' Load PTKP from database jika belum ada dari form sebelumnya
        LoadPTKPFromDatabase()

        ' DEBUG: Show received data (setelah load dari database)
        MessageBox.Show($"EmployeeNPWP: {EmployeeNPWP}" & vbCrLf &
                       $"PTKPStatusValue: {PTKPStatusValue}" & vbCrLf &
                       $"SelectedMonth: {SelectedMonth}",
                       "Debug - Data Received")

        ' Set Month in textbox if selected
        If SelectedMonth > 0 Then
            Dim monthName As String = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(SelectedMonth)
            ' Set in Guna2TextBox12 (Bulan Bukti Potong field)
            Guna2TextBox12.Text = monthName
            Guna2TextBox12.ReadOnly = True
            Guna2TextBox12.FillColor = Color.FromArgb(226, 226, 226)
            ' Also set in header
            Guna2HtmlLabel2.Text = "Bulan " & monthName
        Else
            ' Default if no month selected
            Guna2TextBox12.Text = "Januari"
            Guna2TextBox12.ReadOnly = True
            Guna2TextBox12.FillColor = Color.FromArgb(226, 226, 226)
        End If

        ' Set PTKP field - sekarang PTKPStatusValue pasti sudah ada dari database
        If PTKPBulananValue > 0 Then
            ' Jika sudah dikirim dari form sebelumnya (pre-calculated)
            Guna2TextBox1.Text = ModulePajak.FormatCurrency(PTKPBulananValue)
        ElseIf Not String.IsNullOrEmpty(PTKPStatusValue) Then
            ' Hitung bulanan dari status menggunakan ModulePajak
            PTKPBulananValue = ModulePajak.GetPTKPBulanan(PTKPStatusValue)
            Guna2TextBox1.Text = ModulePajak.FormatCurrency(PTKPBulananValue)
        Else
            ' Fallback: Default TK0 jika benar-benar tidak ada data
            PTKPBulananValue = ModulePajak.GetPTKPBulanan("TK0")
            Guna2TextBox1.Text = ModulePajak.FormatCurrency(PTKPBulananValue)
        End If

        ' Set PTKP field menjadi read-only
        Guna2TextBox1.ReadOnly = True
        Guna2TextBox1.FillColor = Color.FromArgb(226, 226, 226)  ' Abu-abu untuk menandakan read-only

        ' Set Biaya Jabatan field menjadi read-only
        Guna2TextBox8.ReadOnly = True
        Guna2TextBox8.FillColor = Color.FromArgb(226, 226, 226)  ' Abu-abu untuk menandakan read-only
    End Sub

    ' Event handler untuk perhitungan otomatis Total Penghasilan Bruto
    Private Sub CalculateTotalPenghasilanBruto()
        ' Cegah recursive call saat sedang kalkulasi
        If isCalculating Then Return

        Try
            ' Menggunakan ModulePajak.ParseCurrency untuk parsing
            ' txtPPhTerutang = Gaji Pokok (nama control menyesatkan dari designer)
            Dim gajiPokok As Decimal = ModulePajak.ParseCurrency(txtPPhTerutang.Text)
            Dim tunjangan As Decimal = ModulePajak.ParseCurrency(Guna2TextBox10.Text)
            Dim tantiem As Decimal = ModulePajak.ParseCurrency(Guna2TextBox3.Text)

            Dim total As Decimal = gajiPokok + tunjangan + tantiem
            Guna2TextBox4.Text = ModulePajak.FormatCurrency(total)

            ' Menggunakan ModulePajak.CalculateBiayaJabatan
            Dim biayaJabatan As Decimal = ModulePajak.CalculateBiayaJabatan(total)
            Guna2TextBox8.Text = ModulePajak.FormatCurrency(biayaJabatan)
        Catch ex As Exception
            ' Jika ada error parsing, set total ke 0
            Guna2TextBox4.Text = "0"
            Guna2TextBox8.Text = "0"  ' Set ke 0 karena tidak ada bruto
        End Try
    End Sub

    ' Event handler untuk perhitungan otomatis Total Pengurangan
    Private Sub CalculateTotalPengurangan()
        Try
            ' Menggunakan ModulePajak.ParseCurrency untuk parsing
            Dim biayaJabatan As Decimal = ModulePajak.ParseCurrency(Guna2TextBox8.Text)
            Dim zakat As Decimal = ModulePajak.ParseCurrency(Guna2TextBox7.Text)

            Dim total As Decimal = biayaJabatan + zakat
            Guna2TextBox5.Text = ModulePajak.FormatCurrency(total)
        Catch ex As Exception
            ' Jika ada error parsing, set total ke 0
            Guna2TextBox5.Text = "0"
        End Try
    End Sub

    ' NOTE: CalculateProgressiveTax moved to ModulePajak.vb for reusability

    ' Event handler untuk button Hitung - Menghitung PKP Bulanan dan PPh21
    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Try
            ' Set flag untuk mencegah TextChanged events trigger ulang perhitungan
            isCalculating = True

            ' Menggunakan ModulePajak.ParseCurrency untuk parsing
            Dim totalBruto As Decimal = ModulePajak.ParseCurrency(Guna2TextBox4.Text)
            Dim totalPengurangan As Decimal = ModulePajak.ParseCurrency(Guna2TextBox5.Text)
            Dim ptkpBulanan As Decimal = ModulePajak.ParseCurrency(Guna2TextBox1.Text)

            ' Menggunakan ModulePajak untuk perhitungan
            Dim penghasilanNeto As Decimal = ModulePajak.CalculatePenghasilanNeto(totalBruto, totalPengurangan)
            pkp_bulanan = ModulePajak.CalculatePKPBulanan(totalBruto, totalPengurangan, ptkpBulanan)
            Dim pph21Bulanan As Decimal = ModulePajak.CalculatePPh21Bulanan(pkp_bulanan)
            Dim pkpTahunan As Decimal = pkp_bulanan * 12
            Dim pph21Tahunan As Decimal = ModulePajak.CalculateProgressiveTax(pkpTahunan)

            ' Fill the result fields
            ' Guna2TextBox9 = Penghasilan Neto (sesuai label Guna2HtmlLabel21)
            Guna2TextBox9.Text = ModulePajak.FormatCurrency(penghasilanNeto)
            Guna2TextBox9.ReadOnly = True
            Guna2TextBox9.FillColor = Color.FromArgb(226, 226, 226)

            ' Guna2TextBox6 = PPh21 Dipotong (sesuai label Guna2HtmlLabel20)
            Guna2TextBox6.Text = ModulePajak.FormatCurrency(pph21Bulanan)
            Guna2TextBox6.ReadOnly = True
            Guna2TextBox6.FillColor = Color.FromArgb(226, 226, 226)

            ' Guna2TextBox2 = PPh21 Terutang (sesuai label Guna2HtmlLabel22)
            Guna2TextBox2.Text = ModulePajak.FormatCurrency(pph21Bulanan)
            Guna2TextBox2.ReadOnly = True
            Guna2TextBox2.FillColor = Color.FromArgb(226, 226, 226)

            ' Tampilkan hasil perhitungan
            MessageBox.Show(
                $"Perhitungan PPh21 Bulanan:" & vbCrLf & vbCrLf &
                $"Total Penghasilan Bruto: Rp {ModulePajak.FormatCurrency(totalBruto)}" & vbCrLf &
                $"Total Pengurangan: Rp {ModulePajak.FormatCurrency(totalPengurangan)}" & vbCrLf &
                $"Penghasilan Neto: Rp {ModulePajak.FormatCurrency(penghasilanNeto)}" & vbCrLf &
                $"PTKP Bulanan: Rp {ModulePajak.FormatCurrency(ptkpBulanan)}" & vbCrLf &
                $"PKP Bulanan: Rp {ModulePajak.FormatCurrency(pkp_bulanan)}" & vbCrLf &
                $"PKP Tahunan (x12): Rp {ModulePajak.FormatCurrency(pkpTahunan)}" & vbCrLf & vbCrLf &
                $"PPh21 Terutang Tahunan: Rp {ModulePajak.FormatCurrency(pph21Tahunan)}" & vbCrLf &
                $"PPh21 Terutang Bulanan: Rp {ModulePajak.FormatCurrency(pph21Bulanan)}",
                "Hasil Perhitungan",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Error menghitung PKP Bulanan: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Reset flag
            isCalculating = False
        End Try
    End Sub

    ' Event handler untuk button "Discard Changes"
    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Dim result As DialogResult = MessageBox.Show(
            "Apakah Anda yakin ingin membatalkan perubahan?" & vbCrLf &
            "Semua data yang belum disimpan akan hilang.",
            "Konfirmasi",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            ' Navigate back to parent form
            NavigateBack()
        End If
    End Sub

    ' Event handlers untuk field Penghasilan Bruto
    Private Sub txtPPhTerutang_TextChanged(sender As Object, e As EventArgs) Handles txtPPhTerutang.TextChanged
        CalculateTotalPenghasilanBruto()
    End Sub

    Private Sub Guna2TextBox10_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox10.TextChanged
        CalculateTotalPenghasilanBruto()
    End Sub

    Private Sub Guna2TextBox3_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox3.TextChanged
        CalculateTotalPenghasilanBruto()
    End Sub

    ' Event handlers untuk field Pengurangan
    Private Sub Guna2TextBox8_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox8.TextChanged
        CalculateTotalPengurangan()
    End Sub

    Private Sub Guna2TextBox7_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox7.TextChanged
        CalculateTotalPengurangan()
    End Sub

    ' Method untuk navigasi kembali ke parent form
    Private Sub NavigateBack()
        If parentForm IsNot Nothing Then
            parentForm.Show()
        End If
        Me.Close()
    End Sub

    ' ====== NAVBAR EVENT HANDLERS ======
    Private Sub Pk_navbar11_DashboardClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.DashboardClicked
        Dim formDashboard As New pk_dashboard()
        formDashboard.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar11_DaftarPegawaiClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.DaftarPegawaiClicked
        If parentForm IsNot Nothing AndAlso TypeOf parentForm Is pk_daftar_pegawai Then
            parentForm.Show()
            Me.Close()
        Else
            Dim formDaftarPegawai As New pk_daftar_pegawai()
            formDaftarPegawai.Show()
            Me.Close()
        End If
    End Sub

    Private Sub Pk_navbar11_BuktiPotongClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.BuktiPotongClicked
        ' Already on bukti potong form, no action needed
    End Sub

    Private Sub Pk_navbar11_RiwayatClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.RiwayatClicked
        Dim formRiwayat As New pk_riwayat_bukti_potong()
        formRiwayat.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar11_LogoutClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.LogoutClicked
        Dim result As DialogResult = MessageBox.Show(
            "Apakah Anda yakin ingin keluar? Semua data yang belum disimpan akan hilang.",
            "Konfirmasi Logout",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            ' TODO: Implement logout logic (return to login form)
            Application.Exit()
        End If
    End Sub

    Private Sub Guna2Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Guna2Panel1.Paint

    End Sub

    Private Sub Guna2TextBox11_TextChanged(sender As Object, e As EventArgs) Handles Guna2TextBox11.TextChanged

    End Sub
End Class
