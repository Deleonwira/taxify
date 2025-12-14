' Module untuk logika perhitungan pajak PPh21
' Dipisahkan dari pk_form_bukti_potong untuk reusability

Imports MySql.Data.MySqlClient

Module ModulePajak

    ''' <summary>
    ''' Mapping PTKP status ke nilai tahunan dari database
    ''' Fallback ke default jika database error
    ''' </summary>
    ''' <param name="statusPTKP">Status PTKP (TK0, TK1, TK2, TK3, K0, K1, K2, K3)</param>
    ''' <returns>Nilai PTKP tahunan dalam Rupiah</returns>
    Public Function GetPTKPTahunan(statusPTKP As String) As Decimal
        Try
            modulkoneksi.BukaKoneksi()
            Dim sql As String = "SELECT nilai_tahunan FROM master_ptkp WHERE kode_status = @kode AND is_active = 1"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@kode", statusPTKP.ToUpper().Trim())
            Dim result = cmd.ExecuteScalar()
            
            If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                Return Convert.ToDecimal(result)
            End If
        Catch ex As Exception
            ' Fallback to hardcoded if database error
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
        
        ' Fallback values (PMK No. 101/PMK.010/2016)
        Select Case statusPTKP.ToUpper().Trim()
            Case "TK0" : Return 54000000D
            Case "TK1" : Return 58500000D
            Case "TK2" : Return 63000000D
            Case "TK3" : Return 67500000D
            Case "K0" : Return 58500000D
            Case "K1" : Return 63000000D
            Case "K2" : Return 67500000D
            Case "K3" : Return 72000000D
            Case Else : Return 54000000D
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
    ''' Menghitung tarif progresif PPh21 dari database
    ''' Fallback ke UU HPP 2021 jika database error
    ''' </summary>
    ''' <param name="pkpTahunan">Penghasilan Kena Pajak Tahunan</param>
    ''' <returns>PPh21 terutang tahunan</returns>
    Public Function CalculateProgressiveTax(pkpTahunan As Decimal) As Decimal
        Dim tax As Decimal = 0

        If pkpTahunan <= 0 Then
            Return 0
        End If

        ' Try to get tax brackets from database
        Dim brackets As New List(Of Tuple(Of Decimal, Decimal, Decimal))() ' (batas_bawah, batas_atas, tarif_persen)
        
        Try
            modulkoneksi.BukaKoneksi()
            Dim sql As String = "SELECT batas_bawah, batas_atas, tarif_persen FROM master_tarif_pph WHERE is_active = 1 ORDER BY lapisan ASC"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            Dim rd As MySqlDataReader = cmd.ExecuteReader()
            
            While rd.Read()
                Dim bawah As Decimal = Convert.ToDecimal(rd("batas_bawah"))
                Dim atas As Decimal = Convert.ToDecimal(rd("batas_atas"))
                Dim tarif As Decimal = Convert.ToDecimal(rd("tarif_persen")) / 100D
                brackets.Add(Tuple.Create(bawah, atas, tarif))
            End While
            rd.Close()
        Catch ex As Exception
            ' Fallback to hardcoded
            brackets.Clear()
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
        
        ' If got brackets from database, use them
        If brackets.Count > 0 Then
            For Each bracket In brackets
                Dim bawah As Decimal = bracket.Item1
                Dim atas As Decimal = bracket.Item2
                Dim tarif As Decimal = bracket.Item3
                
                If pkpTahunan > bawah Then
                    Dim taxableInBracket As Decimal = Math.Min(pkpTahunan, atas) - bawah
                    If taxableInBracket > 0 Then
                        tax += taxableInBracket * tarif
                    End If
                End If
            Next
            Return tax
        End If
        
        ' Fallback: Hardcoded UU HPP 2021
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

    ''' <summary>
    ''' Menghitung PPh Final / Harian (Berdasarkan TER Harian / PP 58 Tahun 2023)
    ''' - Penghasilan <= 450.000 sehari: Tarif 0%
    ''' - Penghasilan > 450.000 - 2.500.000 sehari: Tarif 0.5% dari Bruto Harian
    ''' </summary>
    ''' <param name="brutoHarian">Penghasilan bruto per hari</param>
    ''' <param name="jumlahHari">Jumlah hari kerja</param>
    ''' <returns>Total PPh terutang</returns>
    Public Function CalculateFreelanceFinal(brutoHarian As Decimal, jumlahHari As Integer) As Decimal
        Dim taxPerHari As Decimal = 0

        If brutoHarian <= 450000D Then
            taxPerHari = 0
        ElseIf brutoHarian <= 2500000D Then
            taxPerHari = brutoHarian * 0.005D
        Else
            ' Jika > 2.5 juta, masuk skema Pasal 17 x 50% (Non-Final)
            ' Tapi untuk fungsi ini kita asumsikan user memilih kategori yang tepat.
            ' Jika dipaksa masuk sini, kita gunakan fallback 0.5% atau return error?
            ' Sesuai instruksi "behave like Final 0.5%", kita gunakan 0.5% untuk simplifikasi range ini,
            ' tapi idealnya > 2.5jt harusnya masuk kategori Tenaga Ahli/Non-Final.
            ' Kita batasi max tax base perhitungan 'Harian' di sini.
            taxPerHari = brutoHarian * 0.005D
        End If

        Return taxPerHari * jumlahHari
    End Function

    ''' <summary>
    ''' Menghitung PPh Non-Final (Tenaga Ahli)
    ''' DPP = 50% x Penghasilan Bruto
    ''' Tarif = Pasal 17 (Progresif)
    ''' </summary>
    ''' <param name="brutoTotal">Penghasilan Bruto</param>
    ''' <returns>Total PPh dipotong</returns>
    Public Function CalculateFreelanceNonFinal(brutoTotal As Decimal) As Decimal
        Dim dpp As Decimal = brutoTotal * 0.5D
        
        ' Gunakan fungsi progresif yang sudah ada
        ' Asumsi: Perhitungan per masa pajak, akumulasi dihandle manual atau di database.
        ' Untuk form input ini, kita hitung based on current amount (single transaction bracket).
        Return CalculateProgressiveTax(dpp)
    End Function

End Module
