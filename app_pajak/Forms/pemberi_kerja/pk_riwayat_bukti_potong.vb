Imports MySql.Data.MySqlClient

Public Class pk_riwayat_bukti_potong

    ' Form load - set active menu and load data
    Private Sub pk_riwayat_bukti_potong_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Pk_navbar11.SetActiveMenu(pk_navbar1.MenuType.Riwayat)
        LoadBuktiPotongHistory()
    End Sub

    Private Sub LoadBuktiPotongHistory(Optional searchKeyword As String = "")
        ' TODO: Load bukti potong history to DataGridView
        Try
            modulkoneksi.BukaKoneksi()
            
            Dim sql As String = "
                SELECT bp.id, bp.nomor_bukti, bp.tanggal_bukti, 
                       p.nama AS nama_pegawai, p.npwp,
                       bp.bruto_total, bp.pph_terutang, bp.status
                FROM bukti_potong bp
                JOIN pegawai p ON bp.pegawai_id = p.id
                WHERE bp.perusahaan_id = @pid"
            
            If Not String.IsNullOrEmpty(searchKeyword) Then
                sql &= " AND (bp.nomor_bukti LIKE @search OR p.nama LIKE @search)"
            End If
            
            sql &= " ORDER BY bp.tanggal_bukti DESC"
            
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@pid", ModuleSession.CurrentPerusahaanId)
            If Not String.IsNullOrEmpty(searchKeyword) Then
                cmd.Parameters.AddWithValue("@search", "%" & searchKeyword & "%")
            End If
            
            Dim adapter As New MySqlDataAdapter(cmd)
            Dim table As New DataTable()
            adapter.Fill(table)
            
            ' TODO: Bind to DataGridView
            ' GridBuktiPotong.DataSource = table
            
        Catch ex As Exception
            MsgBox("Error loading data: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ' TODO: Add event handlers for:
    ' - Search textbox changed
    ' - View detail button clicked
    ' - Delete button clicked
    ' - Export to PDF button clicked

    Private Sub ViewDetail(buktiPotongId As Integer)
        ' TODO: Open detail form or print preview
        ' Can use Crystal Reports or create custom print layout
    End Sub

    Private Sub DeleteBuktiPotong(buktiPotongId As Integer)
        Try
            If MsgBox("Hapus bukti potong ini?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) = MsgBoxResult.No Then
                Return
            End If
            
            modulkoneksi.BukaKoneksi()
            Dim sql As String = "DELETE FROM bukti_potong WHERE id = @id AND perusahaan_id = @pid"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@id", buktiPotongId)
            cmd.Parameters.AddWithValue("@pid", ModuleSession.CurrentPerusahaanId)
            cmd.ExecuteNonQuery()
            
            MsgBox("Bukti potong berhasil dihapus!", MsgBoxStyle.Information)
            LoadBuktiPotongHistory() ' Reload
            
        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ' ====== NAVBAR EVENT HANDLERS ======
    Private Sub Pk_navbar11_DashboardClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.DashboardClicked
        Dim formDashboard As New pk_dashboard()
        formDashboard.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar11_DaftarPegawaiClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.DaftarPegawaiClicked
        Dim formDaftarPegawai As New pk_daftar_pegawai()
        formDaftarPegawai.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar11_BuktiPotongClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.BuktiPotongClicked
        Dim formTimeline As New pk_timeline_bukti_botong()
        formTimeline.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar11_RiwayatClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.RiwayatClicked
        ' Already on riwayat, no action needed
    End Sub

    Private Sub Pk_navbar11_LogoutClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.LogoutClicked
        Dim result As DialogResult = MessageBox.Show(
            "Apakah Anda yakin ingin keluar?",
            "Konfirmasi Logout",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)
        
        If result = DialogResult.Yes Then
            ' TODO: Implement logout logic (return to login form)
            Application.Exit()
        End If
    End Sub

End Class