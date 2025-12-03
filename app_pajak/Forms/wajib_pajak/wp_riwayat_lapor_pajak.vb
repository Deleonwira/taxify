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

        LoadSPTHistory()
    End Sub

    Private Sub LoadSPTHistory(Optional searchKeyword As String = "")
        Try
            modulkoneksi.BukaKoneksi()
            
            Dim sql As String = "
                SELECT id, tahun_pajak, jenis_spt, tanggal_lapor,
                       jumlah_bruto, penghasilan_netto, pajak_pph, status
                FROM spt_tahunan
                WHERE user_id = @uid"
            
            If Not String.IsNullOrEmpty(searchKeyword) Then
                sql &= " AND (tahun_pajak LIKE @search OR jenis_spt LIKE @search)"
            End If
            
            sql &= " ORDER BY tahun_pajak DESC, tanggal_lapor DESC"
            
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@uid", ModuleSession.CurrentUserId)
            If Not String.IsNullOrEmpty(searchKeyword) Then
                cmd.Parameters.AddWithValue("@search", "%" & searchKeyword & "%")
            End If
            
            Dim adapter As New MySqlDataAdapter(cmd)
            Dim table As New DataTable()
            adapter.Fill(table)
            
            ' TODO: Bind to DataGridView
            ' GridSPT.DataSource = table
            
        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
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