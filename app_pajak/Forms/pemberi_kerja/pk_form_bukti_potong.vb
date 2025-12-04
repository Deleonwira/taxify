
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

    ' Constructor untuk menerima parent form
    Public Sub New(Optional parent As Form = Nothing)
        InitializeComponent()
        parentForm = parent
    End Sub

    ' Fungsi untuk mapping PTKP status ke nilai tahunan
    Private Function GetPTKPTahunan(statusPTKP As String) As Decimal
        ' PTKP values per year (2025) berdasarkan PMK No. 101/PMK.010/2016
        Select Case statusPTKP.ToUpper().Trim()
            Case "TK0"
                Return 54000000D  ' Tidak Kawin, 0 tanggungan
            Case "TK1"
                Return 58500000D  ' Tidak Kawin, 1 tanggungan
            Case "TK2"
                Return 63000000D  ' Tidak Kawin, 2 tanggungan
            Case "TK3"
                Return 67500000D  ' Tidak Kawin, 3 tanggungan
            Case "K0"
                Return 58500000D  ' Kawin, 0 tanggungan
            Case "K1"
                Return 63000000D  ' Kawin, 1 tanggungan
            Case "K2"
                Return 67500000D  ' Kawin, 2 tanggungan
            Case "K3"
                Return 72000000D  ' Kawin, 3 tanggungan
            Case Else
                Return 54000000D  ' Default: TK0
        End Select
    End Function

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

    ' Event form load - Set PTKP dan field properties
    Private Sub pk_form_bukti_potong_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Pk_navbar11.SetActiveMenu(pk_navbar1.MenuType.BuktiPotong)

        ' DEBUG: Show received data
        MessageBox.Show($"EmployeeNPWP: {EmployeeNPWP}" & vbCrLf &
                       $"PTKPStatusValue: {PTKPStatusValue}" & vbCrLf &
                       $"SelectedMonth: {SelectedMonth}",
                       "Debug - Data Received")

        ' Load Employee Name
        LoadEmployeeName()
        Guna2TextBox11.ReadOnly = True
        Guna2TextBox11.FillColor = Color.FromArgb(226, 226, 226)

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

        ' Set PTKP field
        If PTKPBulananValue > 0 Then
            ' Jika sudah dikirim dari form sebelumnya
            Guna2TextBox1.Text = PTKPBulananValue.ToString("N0")
        ElseIf Not String.IsNullOrEmpty(PTKPStatusValue) Then
            ' Jika hanya status yang dikirim, hitung bulanan
            Dim ptkpTahunan As Decimal = GetPTKPTahunan(PTKPStatusValue)
            PTKPBulananValue = ptkpTahunan / 12
            Guna2TextBox1.Text = PTKPBulananValue.ToString("N0")
        Else
            ' Default jika tidak ada data
            Guna2TextBox1.Text = "4,500,000"  ' TK0 bulanan
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
        Try
            Dim gajiPokok As Decimal = If(String.IsNullOrWhiteSpace(txtPPhTerutang.Text), 0, Decimal.Parse(txtPPhTerutang.Text.Replace(".", "").Replace(",", ".")))
            Dim tunjangan As Decimal = If(String.IsNullOrWhiteSpace(Guna2TextBox10.Text), 0, Decimal.Parse(Guna2TextBox10.Text.Replace(".", "").Replace(",", ".")))
            Dim tantiem As Decimal = If(String.IsNullOrWhiteSpace(Guna2TextBox3.Text), 0, Decimal.Parse(Guna2TextBox3.Text.Replace(".", "").Replace(",", ".")))

            Dim total As Decimal = gajiPokok + tunjangan + tantiem
            Guna2TextBox4.Text = total.ToString("N0")

            ' Auto-calculate Biaya Jabatan (5% of bruto, min 500k, max 6M)
            Dim biayaJabatan As Decimal = total * 0.05D
            If biayaJabatan < 500000D Then biayaJabatan = 500000D
            If biayaJabatan > 6000000D Then biayaJabatan = 6000000D

            Guna2TextBox8.Text = biayaJabatan.ToString("N0")
        Catch ex As Exception
            ' Jika ada error parsing, set total ke 0
            Guna2TextBox4.Text = "0"
            Guna2TextBox8.Text = "500,000"  ' Minimum biaya jabatan
        End Try
    End Sub

    ' Event handler untuk perhitungan otomatis Total Pengurangan
    Private Sub CalculateTotalPengurangan()
        Try
            Dim biayaJabatan As Decimal = If(String.IsNullOrWhiteSpace(Guna2TextBox8.Text), 0, Decimal.Parse(Guna2TextBox8.Text.Replace(".", "").Replace(",", ".")))
            Dim zakat As Decimal = If(String.IsNullOrWhiteSpace(Guna2TextBox7.Text), 0, Decimal.Parse(Guna2TextBox7.Text.Replace(".", "").Replace(",", ".")))

            Dim total As Decimal = biayaJabatan + zakat
            Guna2TextBox5.Text = total.ToString("N0")
        Catch ex As Exception
            ' Jika ada error parsing, set total ke 0
            Guna2TextBox5.Text = "0"
        End Try
    End Sub

    ' Fungsi untuk menghitung tarif progresif PPh21
    Private Function CalculateProgressiveTax(pkpTahunan As Decimal) As Decimal
        Dim tax As Decimal = 0

        If pkpTahunan <= 0 Then
            Return 0
        End If

        ' Lapisan 1: 0 - 60 juta (5%)
        If pkpTahunan > 0 Then
            Dim layer1 = Math.Min(pkpTahunan, 60000000D)
            tax += layer1 * 0.05D
        End If

        ' Lapisan 2: 60 juta - 250 juta (15%)
        If pkpTahunan > 60000000D Then
            Dim layer2 = Math.Min(pkpTahunan - 60000000D, 190000000D)
            tax += layer2 * 0.15D
        End If

        ' Lapisan 3: 250 juta - 500 juta (25%)
        If pkpTahunan > 250000000D Then
            Dim layer3 = Math.Min(pkpTahunan - 250000000D, 250000000D)
            tax += layer3 * 0.25D
        End If

        ' Lapisan 4: 500 juta - 5 milyar (30%)
        If pkpTahunan > 500000000D Then
            Dim layer4 = Math.Min(pkpTahunan - 500000000D, 4500000000D)
            tax += layer4 * 0.3D
        End If

        ' Lapisan 5: > 5 milyar (35%)
        If pkpTahunan > 5000000000D Then
            Dim layer5 = pkpTahunan - 5000000000D
            tax += layer5 * 0.35D
        End If

        Return tax
    End Function

    ' Event handler untuk button Hitung - Menghitung PKP Bulanan dan PPh21
    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Try
            ' Get values
            Dim totalBruto As Decimal = If(String.IsNullOrWhiteSpace(Guna2TextBox4.Text), 0, Decimal.Parse(Guna2TextBox4.Text.Replace(".", "").Replace(",", ".")))
            Dim totalPengurangan As Decimal = If(String.IsNullOrWhiteSpace(Guna2TextBox5.Text), 0, Decimal.Parse(Guna2TextBox5.Text.Replace(".", "").Replace(",", ".")))
            Dim ptkpBulanan As Decimal = If(String.IsNullOrWhiteSpace(Guna2TextBox1.Text), 0, Decimal.Parse(Guna2TextBox1.Text.Replace(".", "").Replace(",", ".")))

            ' Calculate Penghasilan Neto = Total Bruto - Total Pengurangan
            Dim penghasilanNeto As Decimal = totalBruto - totalPengurangan

            ' Calculate PKP Bulanan = Penghasilan Neto - PTKP Bulanan
            pkp_bulanan = penghasilanNeto - ptkpBulanan

            ' Ensure PKP is not negative
            If pkp_bulanan < 0 Then pkp_bulanan = 0

            ' Calculate PPh21 using progressive tax (annualized method)
            Dim pkpTahunan As Decimal = pkp_bulanan * 12
            Dim pph21Tahunan As Decimal = CalculateProgressiveTax(pkpTahunan)
            Dim pph21Bulanan As Decimal = pph21Tahunan / 12

            ' Fill the result fields
            Guna2TextBox6.Text = penghasilanNeto.ToString("N0")  ' Penghasilan Neto
            Guna2TextBox6.ReadOnly = True
            Guna2TextBox6.FillColor = Color.FromArgb(226, 226, 226)

            Guna2TextBox9.Text = pph21Bulanan.ToString("N0")  ' PPh21 Dipotong
            Guna2TextBox9.ReadOnly = True
            Guna2TextBox9.FillColor = Color.FromArgb(226, 226, 226)

            txtPPhTerutang.Text = pph21Bulanan.ToString("N0")  ' PPh21 Terutang
            txtPPhTerutang.ReadOnly = True
            txtPPhTerutang.FillColor = Color.FromArgb(226, 226, 226)

            ' Tampilkan hasil perhitungan
            MessageBox.Show(
                $"Perhitungan PPh21 Bulanan:" & vbCrLf & vbCrLf &
                $"Total Penghasilan Bruto: Rp {totalBruto.ToString("N0")}" & vbCrLf &
                $"Total Pengurangan: Rp {totalPengurangan.ToString("N0")}" & vbCrLf &
                $"Penghasilan Neto: Rp {penghasilanNeto.ToString("N0")}" & vbCrLf &
                $"PTKP Bulanan: Rp {ptkpBulanan.ToString("N0")}" & vbCrLf &
                $"PKP Bulanan: Rp {pkp_bulanan.ToString("N0")}" & vbCrLf &
                $"PKP Tahunan (x12): Rp {pkpTahunan.ToString("N0")}" & vbCrLf & vbCrLf &
                $"PPh21 Terutang Tahunan: Rp {pph21Tahunan.ToString("N0")}" & vbCrLf &
                $"PPh21 Terutang Bulanan: Rp {pph21Bulanan.ToString("N0")}",
                "Hasil Perhitungan",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Error menghitung PKP Bulanan: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
