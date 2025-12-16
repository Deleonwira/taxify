Imports MySql.Data.MySqlClient

Public Class wp_riwayat_bukti_potong

    Private Sub wp_riwayat_bukti_potong_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Navigation event handlers
        AddHandler Wp_navbar1.DashboardClicked, AddressOf OnDashboardClicked
        AddHandler Wp_navbar1.LaporPajakClicked, AddressOf OnLaporPajakClicked
        AddHandler Wp_navbar1.RiwayatLaporClicked, AddressOf OnRiwayatLaporClicked
        AddHandler Wp_navbar1.TambahBuktiPotongClicked, AddressOf OnTambahBuktiPotongClicked
        AddHandler Wp_navbar1.TimelineBuktiPotongClicked, AddressOf OnTimelineBuktiPotongClicked
        AddHandler Wp_navbar1.RiwayatBuktiPotongClicked, AddressOf OnRiwayatBuktiPotongClicked
        AddHandler Wp_navbar1.DataDiriClicked, AddressOf OnDataDiriClicked
        AddHandler Wp_navbar1.LogoutClicked, AddressOf OnLogoutClicked

        ' Set active menu
        Wp_navbar1.SetActiveMenu(wp_navbar.MenuType.RiwayatBuktiPotong)

        LoadBuktiPotong()
        LoadCompanies()
    End Sub

    Private Sub LoadBuktiPotong(Optional searchText As String = "", Optional companyId As Integer = 0, Optional sortByAmount As Boolean = False)

        GridBukti.Rows.Clear()

        Dim records As New List(Of Tuple(Of DateTime, String, String, String, Long, Long, Long))

        Try
            modulkoneksi.BukaKoneksi()

            ' 1. FETCH STANDARD RECORDS
            Dim sql As String =
            "SELECT bp.id, bp.masa_bulan, bp.masa_tahun, bp.nomor_bukti, bp.bruto_total, bp.netto_total, bp.pph21_terutang,
                    pr.nama_perusahaan 
             FROM bukti_potong bp
             JOIN pekerjaan p ON p.id = bp.pekerjaan_id
             JOIN perusahaan pr ON pr.id = p.perusahaan_id
             WHERE p.wajib_pajak_id = @wp_id"

            If companyId > 0 Then
                sql &= " AND p.perusahaan_id = @companyId"
            End If

            If Not String.IsNullOrEmpty(searchText) Then
                sql &= " AND (pr.nama_perusahaan LIKE @search OR CONCAT(masa_bulan, '/', masa_tahun) LIKE @search)"
            End If

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@wp_id", ModuleSession.CurrentWajibPajakId)

            If companyId > 0 Then
                cmd.Parameters.AddWithValue("@companyId", companyId)
            End If

            If Not String.IsNullOrEmpty(searchText) Then
                cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
            End If

            Dim rd As MySqlDataReader = cmd.ExecuteReader()
            While rd.Read()
                ' Create Date for sorting (Day 1 of Month/Year)
                Dim dt As New DateTime(CInt(rd("masa_tahun")), CInt(rd("masa_bulan")), 1)
                Dim id As String = "S-" & rd("id").ToString() ' Prefix S for Standard
                Dim periode As String = rd("masa_bulan").ToString() & "/" & rd("masa_tahun").ToString()
                Dim perusahaan As String = rd("nama_perusahaan").ToString()
                Dim bruto As Long = CLng(rd("bruto_total"))
                Dim netto As Long = CLng(rd("netto_total"))
                Dim pph As Long = CLng(rd("pph21_terutang"))

                records.Add(Tuple.Create(dt, periode, id, perusahaan, bruto, netto, pph))
            End While
            rd.Close()

            ' 2. FETCH FREELANCE RECORDS (Only if not filtering by companyId, as freelance has no linked company ID yet)
            If companyId = 0 Then
                Dim sqlF As String =
                "SELECT bpf.id, bpf.masa_bulan, bpf.masa_tahun, bpf.nomor_bukti, bpf.bruto_total, bpf.dpp, bpf.pph_dipotong,
                        bpf.nama_pemberi_kerja 
                 FROM bukti_potong_freelance bpf
                 WHERE bpf.wajib_pajak_id = @wp_id"

                If Not String.IsNullOrEmpty(searchText) Then
                    sqlF &= " AND (bpf.nama_pemberi_kerja LIKE @search OR CONCAT(masa_bulan, '/', masa_tahun) LIKE @search)"
                End If

                Dim cmdF As New MySqlCommand(sqlF, modulkoneksi.koneksi)
                cmdF.Parameters.AddWithValue("@wp_id", ModuleSession.CurrentWajibPajakId)

                If Not String.IsNullOrEmpty(searchText) Then
                    cmdF.Parameters.AddWithValue("@search", "%" & searchText & "%")
                End If

                Dim rdF As MySqlDataReader = cmdF.ExecuteReader()
                While rdF.Read()
                    Dim dt As New DateTime(CInt(rdF("masa_tahun")), CInt(rdF("masa_bulan")), 1)
                    Dim id As String = "F-" & rdF("id").ToString() ' Prefix F for Freelance
                    Dim periode As String = rdF("masa_bulan").ToString() & "/" & rdF("masa_tahun").ToString()
                    Dim perusahaan As String = rdF("nama_pemberi_kerja").ToString() & " (Freelance)"
                    Dim bruto As Long = CLng(rdF("bruto_total"))
                    Dim netto As Long = CLng(rdF("dpp")) ' Use DPP as Netto substitute
                    Dim pph As Long = CLng(rdF("pph_dipotong"))

                    records.Add(Tuple.Create(dt, periode, id, perusahaan, bruto, netto, pph))
                End While
                rdF.Close()
            End If

            ' 3. SORT & POPULATE
            Dim sortedRecords As IEnumerable(Of Tuple(Of DateTime, String, String, String, Long, Long, Long))

            If sortByAmount Then
                sortedRecords = records.OrderByDescending(Function(x) x.Item5) ' Sort by Bruto
            Else
                sortedRecords = records.OrderByDescending(Function(x) x.Item1) ' Sort by Date
            End If

            For Each rec In sortedRecords
                GridBukti.Rows.Add(
                    rec.Item2, ' Periode
                    rec.Item3, ' ID (Prefixed)
                    rec.Item4, ' Perusahaan
                    rec.Item5.ToString("N0"), ' Bruto
                    rec.Item6.ToString("N0"), ' Netto
                    rec.Item7.ToString("N0"), ' PPh
                    "Detail"
                )
            Next

        Catch ex As Exception
            MsgBox("Error memuat bukti potong: " & ex.Message)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try

    End Sub


    Private Sub LoadCompanies()
        ' Dynamically load companies from database and create cards
        Try
            modulkoneksi.BukaKoneksi()

            ' Query companies with statistics (count and total bruto)
            Dim sql As String =
                "SELECT pr.id, pr.nama_perusahaan, 
                        COUNT(bp.id) AS jumlah_bukti,
                        COALESCE(SUM(bp.bruto_total), 0) AS total_bruto
                 FROM bukti_potong bp
                 JOIN pekerjaan p ON p.id = bp.pekerjaan_id
                 JOIN perusahaan pr ON pr.id = p.perusahaan_id
                 WHERE p.wajib_pajak_id = @wp_id
                 GROUP BY pr.id, pr.nama_perusahaan
                 ORDER BY pr.nama_perusahaan"

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@wp_id", ModuleSession.CurrentWajibPajakId)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            ' Store company data
            Dim companies As New List(Of Tuple(Of Integer, String, Integer, Decimal))

            While rd.Read()
                Dim id As Integer = Convert.ToInt32(rd("id"))
                Dim nama As String = rd("nama_perusahaan").ToString()
                Dim jumlah As Integer = Convert.ToInt32(rd("jumlah_bukti"))
                Dim bruto As Decimal = Convert.ToDecimal(rd("total_bruto"))
                companies.Add(Tuple.Create(id, nama, jumlah, bruto))
            End While

            rd.Close()
            modulkoneksi.TutupKoneksi()

            ' Clear existing cards in FlowCompanies
            FlowCompanies.Controls.Clear()

            ' Define card colors (rotating)
            Dim cardColors() As Color = {
                Color.FromArgb(0, 122, 255),    ' Blue
                Color.FromArgb(52, 199, 89),    ' Green
                Color.FromArgb(255, 159, 10),   ' Orange
                Color.FromArgb(175, 82, 222),   ' Purple
                Color.FromArgb(255, 45, 85),    ' Red
                Color.FromArgb(90, 200, 250)    ' Cyan
            }

            ' Create cards dynamically
            For i As Integer = 0 To companies.Count - 1
                Dim company = companies(i)
                Dim cardColor = cardColors(i Mod cardColors.Length)

                ' Create card panel
                Dim card As New Guna.UI2.WinForms.Guna2Panel()
                card.BorderColor = Color.FromArgb(230, 233, 241)
                card.BorderRadius = 12
                card.BorderThickness = 1
                card.FillColor = Color.White
                card.Size = New Size(300, 90)
                card.Margin = New Padding(4)
                card.Tag = company.Item1 ' Store company ID

                ' Create circle picture
                Dim pic As New Guna.UI2.WinForms.Guna2CirclePictureBox()
                pic.FillColor = cardColor
                pic.Size = New Size(50, 50)
                pic.Location = New Point(18, 18)
                pic.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle

                ' Create company name label
                Dim lblName As New Guna.UI2.WinForms.Guna2HtmlLabel()
                lblName.BackColor = Color.Transparent
                lblName.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
                lblName.ForeColor = Color.FromArgb(35, 44, 63)
                lblName.Location = New Point(78, 18)
                lblName.Text = company.Item2

                ' Create stats label
                Dim lblStats As New Guna.UI2.WinForms.Guna2HtmlLabel()
                lblStats.BackColor = Color.Transparent
                lblStats.Font = New Font("Segoe UI", 9.0F)
                lblStats.ForeColor = Color.FromArgb(120, 128, 146)
                lblStats.Location = New Point(78, 48)
                lblStats.Text = company.Item3.ToString() & " bukti • Rp " & company.Item4.ToString("N0")

                ' Add controls to card
                card.Controls.Add(pic)
                card.Controls.Add(lblName)
                card.Controls.Add(lblStats)

                ' Add click handler to filter by company
                AddHandler card.Click, Sub(s, ev) FilterByCompany(company.Item1)
                AddHandler pic.Click, Sub(s, ev) FilterByCompany(company.Item1)
                AddHandler lblName.Click, Sub(s, ev) FilterByCompany(company.Item1)
                AddHandler lblStats.Click, Sub(s, ev) FilterByCompany(company.Item1)

                ' Add card to FlowCompanies
                FlowCompanies.Controls.Add(card)
            Next

            ' If no companies found, show a placeholder card
            If companies.Count = 0 Then
                Dim emptyCard As New Guna.UI2.WinForms.Guna2Panel()
                emptyCard.BorderColor = Color.FromArgb(230, 233, 241)
                emptyCard.BorderRadius = 12
                emptyCard.BorderThickness = 1
                emptyCard.FillColor = Color.White
                emptyCard.Size = New Size(300, 90)
                emptyCard.Margin = New Padding(4)

                Dim lblEmpty As New Guna.UI2.WinForms.Guna2HtmlLabel()
                lblEmpty.BackColor = Color.Transparent
                lblEmpty.Font = New Font("Segoe UI", 10.0F)
                lblEmpty.ForeColor = Color.FromArgb(120, 128, 146)
                lblEmpty.Location = New Point(24, 35)
                lblEmpty.Text = "Belum ada data perusahaan"

                emptyCard.Controls.Add(lblEmpty)
                FlowCompanies.Controls.Add(emptyCard)
            End If

        Catch ex As Exception
            MsgBox("Error load companies: " & ex.Message)
        End Try
    End Sub

    Private Sub PanelMain_Paint(sender As Object, e As PaintEventArgs) Handles PanelMain.Paint

    End Sub

    Private Sub PanelTable_Paint(sender As Object, e As PaintEventArgs) Handles PanelTable.Paint

    End Sub

    Private Sub GridBukti_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles GridBukti.CellContentClick
        Dim id = GridBukti.CurrentRow.Cells("colBuktiPotong").Value

        Dim f As New wp_detail_bukti_potong(id)
        f.Show()

        Console.WriteLine(id)

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
        Dim f As New wp_lapor_pajak()
        f.Show()
        Me.Hide()
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
        ' Already on this page
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

    ' =============================
    '   FILTER AND ACTION HANDLERS
    ' =============================
    Private Sub BtnFilter_Click(sender As Object, e As EventArgs) Handles BtnFilter.Click
        ' Apply search filter
        Dim searchText As String = TxtSearch.Text.Trim()
        LoadBuktiPotong(searchText)
    End Sub

    Private Sub BtnSortAmount_Click(sender As Object, e As EventArgs)
        ' Sort by amount (bruto total)
        Dim searchText = TxtSearch.Text.Trim
        LoadBuktiPotong(searchText, sortByAmount:=True)
    End Sub

    Private Sub BtnDownloadAll_Click(sender As Object, e As EventArgs)
        Try
            ' Export grid data to text file (simple export)
            Dim saveDialog As New SaveFileDialog
            saveDialog.Filter = "Text Files (*.txt)|*.txt|CSV Files (*.csv)|*.csv"
            saveDialog.DefaultExt = "txt"
            saveDialog.FileName = "Riwayat_Bukti_Potong_" & Date.Now.ToString("yyyyMMdd")

            If saveDialog.ShowDialog = DialogResult.OK Then
                Dim sb As New Text.StringBuilder

                ' Add headers
                sb.AppendLine("RIWAYAT BUKTI POTONG PPH21")
                sb.AppendLine("Tanggal Export: " & Date.Now.ToString("dd MMMM yyyy HH:mm"))
                sb.AppendLine("NPWP: " & CurrentUserNPWP)
                sb.AppendLine("Nama: " & CurrentUserName)
                sb.AppendLine(String.Empty)
                sb.AppendLine(String.Empty)

                ' Add column headers
                sb.AppendLine("Periode" & vbTab & "No. Bukti" & vbTab & "Perusahaan" & vbTab & "Bruto" & vbTab & "Neto" & vbTab & "PPh21")
                sb.AppendLine(New String("="c, 80))

                ' Add data rows
                For Each row As DataGridViewRow In GridBukti.Rows
                    If Not row.IsNewRow Then
                        sb.AppendLine(
                            row.Cells("colPeriode").Value.ToString & vbTab &
                            row.Cells("colBuktiPotong").Value.ToString & vbTab &
                            row.Cells("colPenghasilan").Value.ToString & vbTab &
                            row.Cells("colPPh").Value.ToString & vbTab &
                            row.Cells("colJenisPekerjaan").Value.ToString & vbTab &
                            row.Cells("colStatus").Value.ToString
                        )
                    End If
                Next

                ' Write to file
                IO.File.WriteAllText(saveDialog.FileName, sb.ToString)
                MsgBox("Data berhasil diekspor ke: " & saveDialog.FileName, MsgBoxStyle.Information, "Ekspor Berhasil")
            End If

        Catch ex As Exception
            MsgBox("Error saat mengekspor data: " & ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Sub FilterByCompany(companyId As Integer)
        ' Filter bukti potong by selected company
        LoadBuktiPotong(companyId:=companyId)
        TxtSearch.Clear()
    End Sub

    Private Sub TxtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtSearch.KeyDown
        ' Allow searching by pressing Enter in search box
        If e.KeyCode = Keys.Enter Then
            BtnFilter_Click(sender, EventArgs.Empty)
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub CardCompany1_Paint(sender As Object, e As PaintEventArgs) Handles CardCompany1.Paint

    End Sub
End Class
