' Module untuk logika perhitungan pajak PPh21
' Dipisahkan dari pk_form_bukti_potong untuk reusability

Module ModulePajak

    ''' <summary>
    ''' Mapping PTKP status ke nilai tahunan berdasarkan PMK No. 101/PMK.010/2016
    ''' </summary>
    ''' <param name="statusPTKP">Status PTKP (TK0, TK1, TK2, TK3, K0, K1, K2, K3)</param>
    ''' <returns>Nilai PTKP tahunan dalam Rupiah</returns>
    Public Function GetPTKPTahunan(statusPTKP As String) As Decimal
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

    ''' <summary>
    ''' Menghitung PTKP bulanan dari status PTKP
    ''' </summary>
    ''' <param name="statusPTKP">Status PTKP (TK0, TK1, TK2, TK3, K0, K1, K2, K3)</param>
    ''' <returns>Nilai PTKP bulanan dalam Rupiah</returns>
    Public Function GetPTKPBulanan(statusPTKP As String) As Decimal
        Return GetPTKPTahunan(statusPTKP) / 12
    End Function

    ''' <summary>
    ''' Menghitung tarif progresif PPh21 berdasarkan UU HPP 2021
    ''' Lapisan 1: 0 - 60 juta (5%)
    ''' Lapisan 2: 60 juta - 250 juta (15%)
    ''' Lapisan 3: 250 juta - 500 juta (25%)
    ''' Lapisan 4: 500 juta - 5 milyar (30%)
    ''' Lapisan 5: > 5 milyar (35%)
    ''' </summary>
    ''' <param name="pkpTahunan">Penghasilan Kena Pajak Tahunan</param>
    ''' <returns>PPh21 terutang tahunan</returns>
    Public Function CalculateProgressiveTax(pkpTahunan As Decimal) As Decimal
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

    ''' <summary>
    ''' Menghitung PPh21 bulanan dari PKP bulanan menggunakan metode annualized
    ''' </summary>
    ''' <param name="pkpBulanan">Penghasilan Kena Pajak Bulanan</param>
    ''' <returns>PPh21 terutang bulanan</returns>
    Public Function CalculatePPh21Bulanan(pkpBulanan As Decimal) As Decimal
        If pkpBulanan <= 0 Then
            Return 0
        End If

        Dim pkpTahunan As Decimal = pkpBulanan * 12
        Dim pph21Tahunan As Decimal = CalculateProgressiveTax(pkpTahunan)
        Return pph21Tahunan / 12
    End Function

    ''' <summary>
    ''' Menghitung Biaya Jabatan sesuai PMK No. 250/PMK.03/2008
    ''' - 5% dari penghasilan bruto
    ''' - Maximum Rp 500.000 per bulan (Rp 6.000.000 per tahun)
    ''' - Tidak ada minimum
    ''' </summary>
    ''' <param name="penghasilanBruto">Total penghasilan bruto bulanan</param>
    ''' <returns>Biaya jabatan bulanan</returns>
    Public Function CalculateBiayaJabatan(penghasilanBruto As Decimal) As Decimal
        If penghasilanBruto <= 0 Then
            Return 0
        End If

        Dim biayaJabatan As Decimal = penghasilanBruto * 0.05D
        ' Maximum Rp 500.000 per bulan
        If biayaJabatan > 500000D Then biayaJabatan = 500000D
        Return biayaJabatan
    End Function

    ''' <summary>
    ''' Helper untuk parsing nilai currency dari string (format Indonesia dengan titik sebagai separator ribuan)
    ''' </summary>
    ''' <param name="value">String nilai currency</param>
    ''' <returns>Nilai decimal</returns>
    Public Function ParseCurrency(value As String) As Decimal
        If String.IsNullOrWhiteSpace(value) Then
            Return 0
        End If

        Try
            ' Remove thousand separators (.) and replace decimal separator (,) with (.)
            Return Decimal.Parse(value.Replace(".", "").Replace(",", "."))
        Catch ex As Exception
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Format nilai decimal ke format currency Indonesia
    ''' </summary>
    ''' <param name="value">Nilai decimal</param>
    ''' <returns>String format currency</returns>
    Public Function FormatCurrency(value As Decimal) As String
        Return value.ToString("N0")
    End Function

    ''' <summary>
    ''' Menghitung PKP Bulanan
    ''' PKP = Penghasilan Neto - PTKP Bulanan
    ''' Penghasilan Neto = Total Bruto - Total Pengurangan
    ''' </summary>
    ''' <param name="totalBruto">Total penghasilan bruto</param>
    ''' <param name="totalPengurangan">Total pengurangan (biaya jabatan + zakat)</param>
    ''' <param name="ptkpBulanan">PTKP bulanan</param>
    ''' <returns>PKP Bulanan (minimum 0)</returns>
    Public Function CalculatePKPBulanan(totalBruto As Decimal, totalPengurangan As Decimal, ptkpBulanan As Decimal) As Decimal
        Dim penghasilanNeto As Decimal = totalBruto - totalPengurangan
        Dim pkpBulanan As Decimal = penghasilanNeto - ptkpBulanan
        
        ' PKP tidak boleh negatif
        If pkpBulanan < 0 Then pkpBulanan = 0
        
        Return pkpBulanan
    End Function

    ''' <summary>
    ''' Menghitung Penghasilan Neto
    ''' </summary>
    ''' <param name="totalBruto">Total penghasilan bruto</param>
    ''' <param name="totalPengurangan">Total pengurangan</param>
    ''' <returns>Penghasilan Neto</returns>
    Public Function CalculatePenghasilanNeto(totalBruto As Decimal, totalPengurangan As Decimal) As Decimal
        Return totalBruto - totalPengurangan
    End Function

End Module
