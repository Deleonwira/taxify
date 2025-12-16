Imports MySql.Data.MySqlClient

Public Class admin_manajemen_spt

    Private isLoading As Boolean = True

    ' ====== NAVBAR EVENT HANDLERS ======
    Private Sub Pk_navbar1_DashboardClicked(sender As Object, e As EventArgs) Handles Pk_navbar1.DashboardClicked
        Dim f As New admin_dashboard()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar1_ManajemenPemberiKerjaClicked(sender As Object, e As EventArgs) Handles Pk_navbar1.ManajemenPemberiKerjaClicked
        Dim f As New FrmManagementPemberiKerja()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar1_ManajemenUserClicked(sender As Object, e As EventArgs) Handles Pk_navbar1.ManajemenUserClicked
        Dim f As New FrmUserManagement()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar1_ManajemenPerusahaanClicked(sender As Object, e As EventArgs) Handles Pk_navbar1.ManajemenPerusahaanClicked
        Dim f As New FrmManagementPerusahaan()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar1_MasterPajakClicked(sender As Object, e As EventArgs) Handles Pk_navbar1.MasterPajakClicked
        Dim f As New admin_master_pajak()
        f.Show()
        Me.Close()
    End Sub

    ' New Event Handler for ManajemenSPT (Self)
    Private Sub Pk_navbar1_ManajemenSPTClicked(sender As Object, e As EventArgs) Handles Pk_navbar1.ManajemenSPTClicked
        ' Already on this form
    End Sub
    
    Private Sub Pk_navbar1_LogoutClicked(sender As Object, e As EventArgs) Handles Pk_navbar1.LogoutClicked
        ModuleSession.ClearSession()
        Dim f As New FrmLogin()
        f.Show()
        Me.Close()
    End Sub

    Private Sub admin_manajemen_spt_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set Active Menu to ManajemenSPT (Need to update Enum first, for now assume index or update logic)
        Pk_navbar1.SetActiveMenu(admin_navbar.MenuType.ManajemenSPT)
        
        ' Initialize ComboBoxes
        CmbTahun.SelectedIndex = 0
        CmbStatus.SelectedIndex = 0
        
        LoadSPT()
        isLoading = False
    End Sub

    Private Sub LoadSPT()
        Try
            modulkoneksi.BukaKoneksi()
            GridSPT.Rows.Clear()

            Dim sql As String = "
                SELECT s.id, w.nama, w.npwp, s.tahun_pajak, s.status_spt, s.tanggal_lapor
                FROM spt_tahunan s
                JOIN wajib_pajak w ON s.wajib_pajak_id = w.id
                WHERE 1=1"

            ' Filter Search
            If Not String.IsNullOrWhiteSpace(TxtSearch.Text) Then
                sql &= " AND (w.nama LIKE @search OR w.npwp LIKE @search)"
            End If

            ' Filter Tahun
            If CmbTahun.SelectedIndex > 0 Then
                sql &= " AND s.tahun_pajak = @tahun"
            End If

            ' Filter Status
            If CmbStatus.SelectedIndex > 0 Then
                sql &= " AND s.status_spt = @status"
            End If

            sql &= " ORDER BY s.tanggal_lapor DESC"

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            
            If Not String.IsNullOrWhiteSpace(TxtSearch.Text) Then
                cmd.Parameters.AddWithValue("@search", "%" & TxtSearch.Text & "%")
            End If
            
            If CmbTahun.SelectedIndex > 0 Then
                cmd.Parameters.AddWithValue("@tahun", CmbTahun.SelectedItem.ToString())
            End If
            
            If CmbStatus.SelectedIndex > 0 Then
                cmd.Parameters.AddWithValue("@status", CmbStatus.SelectedItem.ToString())
            End If

            Dim rd As MySqlDataReader = cmd.ExecuteReader()
            Dim no As Integer = 1

            While rd.Read()
                Dim id As Integer = Convert.ToInt32(rd("id"))
                Dim nama As String = rd("nama").ToString()
                Dim npwp As String = rd("npwp").ToString()
                Dim tahun As String = rd("tahun_pajak").ToString()
                Dim status As String = rd("status_spt").ToString()
                Dim tanggal As String = Convert.ToDateTime(rd("tanggal_lapor")).ToString("dd/MM/yyyy")

                GridSPT.Rows.Add(no, nama, npwp, tahun, status, tanggal, "Ubah Status")
                GridSPT.Rows(GridSPT.Rows.Count - 1).Tag = id
                no += 1
            End While
            rd.Close()
            
            LblTableSubtitle.Text = $"Menampilkan {no - 1} data SPT Tahunan."

        Catch ex As Exception
            MsgBox("Error loading SPT: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub TxtSearch_TextChanged(sender As Object, e As EventArgs) Handles TxtSearch.TextChanged
        If Not isLoading Then LoadSPT()
    End Sub

    Private Sub CmbTahun_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbTahun.SelectedIndexChanged
        If Not isLoading Then LoadSPT()
    End Sub

    Private Sub CmbStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbStatus.SelectedIndexChanged
        If Not isLoading Then LoadSPT()
    End Sub

    Private Sub GridSPT_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles GridSPT.CellContentClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex = GridSPT.Columns("colEdit").Index Then
            Dim id As Integer = Convert.ToInt32(GridSPT.Rows(e.RowIndex).Tag)
            ShowUpdateStatusDialog(id)
        End If
    End Sub

    Private Sub ShowUpdateStatusDialog(id As Integer)
        ' Create a simple dialog to select new status
        Dim dialog As New Form()
        dialog.Text = "Ubah Status SPT"
        dialog.Size = New Size(300, 200)
        dialog.StartPosition = FormStartPosition.CenterParent
        dialog.FormBorderStyle = FormBorderStyle.FixedDialog
        dialog.MaximizeBox = False
        dialog.MinimizeBox = False
        
        Dim lbl As New Label() With {.Text = "Pilih Status Baru:", .Location = New Point(20, 20), .AutoSize = True}
        Dim cmb As New ComboBox() With {.Location = New Point(20, 50), .Width = 240, .DropDownStyle = ComboBoxStyle.DropDownList}
        cmb.Items.AddRange(New Object() {"Lebih Bayar", "Kurang Bayar", "Nihil"})
        
        Dim btnSave As New Button() With {.Text = "Simpan", .Location = New Point(20, 90), .DialogResult = DialogResult.OK}
        Dim btnCancel As New Button() With {.Text = "Batal", .Location = New Point(100, 90), .DialogResult = DialogResult.Cancel}
        
        dialog.Controls.Add(lbl)
        dialog.Controls.Add(cmb)
        dialog.Controls.Add(btnSave)
        dialog.Controls.Add(btnCancel)
        dialog.AcceptButton = btnSave
        dialog.CancelButton = btnCancel
        
        If dialog.ShowDialog() = DialogResult.OK Then
            If cmb.SelectedItem IsNot Nothing Then
                UpdateStatus(id, cmb.SelectedItem.ToString())
            End If
        End If
    End Sub

    Private Sub UpdateStatus(id As Integer, newStatus As String)
        Try
            modulkoneksi.BukaKoneksi()
            Dim cmd As New MySqlCommand("UPDATE spt_tahunan SET status_spt = @status WHERE id = @id", modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@status", newStatus)
            cmd.Parameters.AddWithValue("@id", id)
            cmd.ExecuteNonQuery()
            
            MsgBox("Status berhasil diubah!", MsgBoxStyle.Information)
            LoadSPT()
        Catch ex As Exception
            MsgBox("Error updating status: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

End Class
