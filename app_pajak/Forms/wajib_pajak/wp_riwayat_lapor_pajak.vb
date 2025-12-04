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

        ' Set active menu
        Wp_navbar1.SetActiveMenu(wp_navbar.MenuType.RiwayatLapor)

        ' Add event handlers for filter and search
        AddHandler TxtSearch.TextChanged, AddressOf TxtSearch_TextChanged
        AddHandler CmbStatus.SelectedIndexChanged, AddressOf CmbStatus_SelectedIndexChanged
        AddHandler BtnFilterReset.Click, AddressOf BtnFilterReset_Click
        AddHandler BtnCreate.Click, AddressOf BtnCreate_Click

        LoadSPTHistory()
    End Sub
    
    Private Sub TxtSearch_TextChanged(sender As Object, e As EventArgs)
        LoadSPTHistory(TxtSearch.Text)
    End Sub
    
    Private Sub CmbStatus_SelectedIndexChanged(sender As Object, e As EventArgs)
        ApplyFilters()
    End Sub
    
    Private Sub BtnFilterReset_Click(sender As Object, e As EventArgs)
        TxtSearch.Text = ""
        CmbStatus.SelectedIndex = 0
        CmbJenis.SelectedIndex = 0
        LoadSPTHistory()
    End Sub
    
    Private Sub BtnCreate_Click(sender As Object, e As EventArgs)
        Dim f As New wp_lapor_pajak()
        f.Show()
        Me.Hide()
    End Sub
    
    Private Sub ApplyFilters()
        Dim searchKeyword As String = TxtSearch.Text
        Dim statusFilter As String = ""
        
        If CmbStatus.SelectedIndex > 0 Then
            Select Case CmbStatus.SelectedIndex
                Case 1 ' Selesai
                    statusFilter = "Nihil"
                Case 2 ' Perlu Review
                    statusFilter = "Kurang Bayar"
                Case 3 ' Belum Kirim
                    statusFilter = ""
            End Select
        End If
        
        LoadSPTHistoryWithFilter(searchKeyword, statusFilter)
    End Sub
    
    Private Sub LoadSPTHistoryWithFilter(Optional searchKeyword As String = "", Optional statusFilter As String = "")
        Try
            modulkoneksi.BukaKoneksi()
            
            Dim sql As String = "
                SELECT tahun_pajak, bruto_setahun, netto_setahun, pph21_terutang, status_spt
                FROM spt_tahunan
                WHERE wp_npwp = @npwp"
            
            If Not String.IsNullOrEmpty(searchKeyword) Then
                sql &= " AND (tahun_pajak LIKE @search OR status_spt LIKE @search)"
            End If
            
            If Not String.IsNullOrEmpty(statusFilter) Then
                sql &= " AND status_spt = @status"
            End If
            
            sql &= " ORDER BY tahun_pajak DESC"
            
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", ModuleSession.CurrentUserNPWP)
            
            If Not String.IsNullOrEmpty(searchKeyword) Then
                cmd.Parameters.AddWithValue("@search", "%" & searchKeyword & "%")
            End If
            
            If Not String.IsNullOrEmpty(statusFilter) Then
                cmd.Parameters.AddWithValue("@status", statusFilter)
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
        LoadSPTHistoryWithFilter(searchKeyword, "")
    End Sub

    ' TODO: Add event handlers for view detail SPT

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
End Class