Imports MySql.Data.MySqlClient

Public Class wp_riwayat_lapor_pajak

    Private Sub wp_riwayat_lapor_pajak_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Navigation event handlers
        AddHandler Wp_navbar1.DashboardClicked, AddressOf OnDashboardClicked
        AddHandler Wp_navbar1.LaporPajakClicked, AddressOf OnLaporPajakClicked
        AddHandler Wp_navbar1.RiwayatLaporClicked, AddressOf OnRiwayatLaporClicked
        AddHandler Wp_navbar1.TimelineBuktiPotongClicked, AddressOf OnTimelineBuktiPotongClicked
        AddHandler Wp_navbar1.RiwayatBuktiPotongClicked, AddressOf OnRiwayatBuktiPotongClicked
        AddHandler Wp_navbar1.DataDiriClicked, AddressOf OnDataDiriClicked
        AddHandler Wp_navbar1.LogoutClicked, AddressOf OnLogoutClicked
        AddHandler Wp_navbar1.ChatbotClicked, AddressOf OnChatbotClicked

        ' Set active menu
        Wp_navbar1.SetActiveMenu(wp_navbar.MenuType.RiwayatLapor)

        ' Initialize year filter dropdown
        PopulateYearFilter()

        LoadSPTHistory()
    End Sub

    Private Sub PopulateYearFilter()
        ' Clear and repopulate year combo with dynamic years
        CmbTahun.Items.Clear()
        CmbTahun.Items.Add("Semua Tahun")

        ' Add years from current year going back 5 years
        Dim currentYear As Integer = DateTime.Now.Year
        For i As Integer = 0 To 5
            CmbTahun.Items.Add((currentYear - i).ToString())
        Next

        CmbTahun.SelectedIndex = 0
    End Sub

    Private Sub TxtSearch_TextChanged(sender As Object, e As EventArgs) Handles TxtSearch.TextChanged
        ' Real-time search as user types
        ApplyFilters()
    End Sub

    Private Sub CmbStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbStatus.SelectedIndexChanged
        ApplyFilters()
    End Sub

    Private Sub CmbTahun_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbTahun.SelectedIndexChanged
        ApplyFilters()
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        ' Search button click
        ApplyFilters()
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        ' Reset button click
        TxtSearch.Text = ""
        CmbStatus.SelectedIndex = 0
        CmbTahun.SelectedIndex = 0
        LoadSPTHistory()
    End Sub

    Private Sub BtnCreate_Click(sender As Object, e As EventArgs) Handles BtnCreate.Click
        Dim f As New wp_lapor_pajak()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub ApplyFilters()
        Dim searchKeyword As String = TxtSearch.Text.Trim()
        Dim statusFilter As String = ""
        Dim yearFilter As String = ""

        ' Get status filter
        If CmbStatus.SelectedIndex > 0 Then
            Select Case CmbStatus.SelectedIndex
                Case 1 ' Selesai
                    statusFilter = "Nihil"
                Case 2 ' Perlu Review
                    statusFilter = "Kurang Bayar"
                Case 3 ' Belum Kirim
                    statusFilter = "Lebih Bayar"
            End Select
        End If

        ' Get year filter
        If CmbTahun.SelectedIndex > 0 Then
            yearFilter = CmbTahun.SelectedItem.ToString()
        End If

        LoadSPTHistoryWithFilter(searchKeyword, statusFilter, yearFilter)
    End Sub

    Private Sub LoadSPTHistoryWithFilter(Optional searchKeyword As String = "", Optional statusFilter As String = "", Optional yearFilter As String = "")
        Try
            modulkoneksi.BukaKoneksi()

            Dim sql As String = "
                SELECT tahun_pajak, bruto_setahun, netto_setahun, pph21_terutang, status_spt
                FROM spt_tahunan
                WHERE wajib_pajak_id = @wp_id"

            If Not String.IsNullOrEmpty(searchKeyword) Then
                sql &= " AND (CAST(tahun_pajak AS CHAR) LIKE @search OR status_spt LIKE @search)"
            End If

            If Not String.IsNullOrEmpty(statusFilter) Then
                sql &= " AND status_spt = @status"
            End If

            If Not String.IsNullOrEmpty(yearFilter) Then
                sql &= " AND tahun_pajak = @year"
            End If

            sql &= " ORDER BY tahun_pajak DESC"

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@wp_id", ModuleSession.CurrentWajibPajakId)

            If Not String.IsNullOrEmpty(searchKeyword) Then
                cmd.Parameters.AddWithValue("@search", "%" & searchKeyword & "%")
            End If

            If Not String.IsNullOrEmpty(statusFilter) Then
                cmd.Parameters.AddWithValue("@status", statusFilter)
            End If

            If Not String.IsNullOrEmpty(yearFilter) Then
                cmd.Parameters.AddWithValue("@year", Convert.ToInt32(yearFilter))
            End If

            Dim adapter As New MySqlDataAdapter(cmd)
            Dim table As New DataTable()
            adapter.Fill(table)

            ' Clear existing rows
            GridRiwayat.Rows.Clear()

            ' Populate DataGridView
            For Each row As DataRow In table.Rows
                Dim tahun As String = If(IsDBNull(row("tahun_pajak")), "", row("tahun_pajak").ToString())
                Dim bruto As Decimal = If(IsDBNull(row("bruto_setahun")), 0, Convert.ToDecimal(row("bruto_setahun")))
                Dim netto As Decimal = If(IsDBNull(row("netto_setahun")), 0, Convert.ToDecimal(row("netto_setahun")))
                Dim pph21 As Decimal = If(IsDBNull(row("pph21_terutang")), 0, Convert.ToDecimal(row("pph21_terutang")))
                Dim status As String = If(IsDBNull(row("status_spt")), "", row("status_spt").ToString())

                ' Format currency values with Rp prefix
                Dim brutoFormatted As String = "Rp " & bruto.ToString("N0")
                Dim nettoFormatted As String = "Rp " & netto.ToString("N0")
                Dim pph21Formatted As String = "Rp " & pph21.ToString("N0")

                ' Translate status to Indonesian if needed
                Dim statusText As String = status
                Select Case status.ToLower()
                    Case "lebih bayar"
                        statusText = "Lebih Bayar"
                    Case "kurang bayar"
                        statusText = "Kurang Bayar"
                    Case "nihil"
                        statusText = "Nihil"
                    Case Else
                        statusText = If(String.IsNullOrEmpty(status), "-", status)
                End Select

                ' Add row to DataGridView
                GridRiwayat.Rows.Add(tahun, brutoFormatted, nettoFormatted, pph21Formatted, statusText)
            Next

        Catch ex As Exception
            MsgBox("Error loading data: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub LoadSPTHistory(Optional searchKeyword As String = "")
        LoadSPTHistoryWithFilter(searchKeyword, "", "")
    End Sub

    Private Sub BtnDownloadSemua_Click(sender As Object, e As EventArgs) Handles BtnDownloadSemua.Click
        Try
            If GridRiwayat.Rows.Count = 0 Then
                MsgBox("Tidak ada data untuk diunduh.", MsgBoxStyle.Information, "Info")
                Return
            End If

            ' Export grid data to CSV file
            Dim saveDialog As New SaveFileDialog
            saveDialog.Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt"
            saveDialog.DefaultExt = "csv"
            saveDialog.FileName = "Riwayat_SPT_Tahunan_" & Date.Now.ToString("yyyyMMdd")

            If saveDialog.ShowDialog = DialogResult.OK Then
                Dim sb As New Text.StringBuilder

                ' Add headers
                sb.AppendLine("RIWAYAT SPT TAHUNAN PPH21")
                sb.AppendLine("Tanggal Export: " & Date.Now.ToString("dd MMMM yyyy HH:mm"))
                sb.AppendLine("NPWP: " & ModuleSession.CurrentUserNPWP)
                sb.AppendLine("Nama: " & ModuleSession.CurrentUserName)
                sb.AppendLine(String.Empty)

                ' Add column headers (CSV format)
                sb.AppendLine("Tahun,Penghasilan Bruto,Penghasilan Neto,PPh21 Terutang,Status")

                ' Add data rows
                For Each row As DataGridViewRow In GridRiwayat.Rows
                    If Not row.IsNewRow Then
                        Dim tahun As String = If(row.Cells("colTahun").Value IsNot Nothing, row.Cells("colTahun").Value.ToString(), "")
                        Dim bruto As String = If(row.Cells("colPenghasilanBruto").Value IsNot Nothing, row.Cells("colPenghasilanBruto").Value.ToString(), "")
                        Dim netto As String = If(row.Cells("colPenghasilanNeto").Value IsNot Nothing, row.Cells("colPenghasilanNeto").Value.ToString(), "")
                        Dim pph21 As String = If(row.Cells("colPph21").Value IsNot Nothing, row.Cells("colPph21").Value.ToString(), "")
                        Dim status As String = If(row.Cells("colStatus").Value IsNot Nothing, row.Cells("colStatus").Value.ToString(), "")

                        sb.AppendLine($"""{tahun}"",""{bruto}"",""{netto}"",""{pph21}"",""{status}""")
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

    Private Sub GridRiwayat_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles GridRiwayat.CellClick
        ' Handle row click to view detail
        If e.RowIndex >= 0 Then
            Dim tahun As String = GridRiwayat.Rows(e.RowIndex).Cells("colTahun").Value?.ToString()
            If Not String.IsNullOrEmpty(tahun) Then
                ' Show detail info for now (can be expanded to a detail form later)
                Dim bruto As String = GridRiwayat.Rows(e.RowIndex).Cells("colPenghasilanBruto").Value?.ToString()
                Dim netto As String = GridRiwayat.Rows(e.RowIndex).Cells("colPenghasilanNeto").Value?.ToString()
                Dim pph21 As String = GridRiwayat.Rows(e.RowIndex).Cells("colPph21").Value?.ToString()
                Dim status As String = GridRiwayat.Rows(e.RowIndex).Cells("colStatus").Value?.ToString()

                Dim detailMsg As String = $"Detail SPT Tahun {tahun}" & vbCrLf & vbCrLf &
                    $"Penghasilan Bruto: {bruto}" & vbCrLf &
                    $"Penghasilan Neto: {netto}" & vbCrLf &
                    $"PPh21 Terutang: {pph21}" & vbCrLf &
                    $"Status: {status}"

                MsgBox(detailMsg, MsgBoxStyle.Information, "Detail SPT Tahunan")
            End If
        End If
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
        ' Already on this page
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
        ' Navigate to dashboard to show chatbot
        Dim f As New wp_dashboard()
        f.Show()
        Me.Hide()
    End Sub
End Class