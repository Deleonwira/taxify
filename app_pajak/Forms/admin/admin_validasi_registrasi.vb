Imports MySql.Data.MySqlClient

Public Class admin_validasi_registrasi

    Private Sub admin_validasi_registrasi_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadPendingUsers()
    End Sub

    ''' <summary>
    ''' Load semua user dengan status_validasi = 'pending'
    ''' </summary>
    Private Sub LoadPendingUsers()
        Try
            modulkoneksi.BukaKoneksi()

            Dim sql As String = "
                SELECT 
                    u.npwp,
                    u.nama,
                    u.email,
                    u.no_telepon,
                    u.created_at as tanggal_daftar,
                    COALESCE(p.nama_perusahaan, 'Freelance') as perusahaan,
                    COALESCE(pek.jabatan, '-') as jabatan,
                    COALESCE(pek.status_ptkp, '-') as status_ptkp
                FROM users u
                LEFT JOIN pekerjaan pek ON u.npwp = pek.wp_npwp
                LEFT JOIN perusahaan p ON pek.perusahaan_id = p.id
                WHERE u.tipe_user = 'wajib_pajak' 
                AND u.status_validasi = 'pending'
                ORDER BY u.created_at DESC
            "

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            Dim adapter As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            adapter.Fill(dt)

            GridPending.DataSource = dt

            ' Format columns
            If GridPending.Columns.Count > 0 Then
                GridPending.Columns("npwp").HeaderText = "NPWP"
                GridPending.Columns("nama").HeaderText = "Nama"
                GridPending.Columns("email").HeaderText = "Email"
                GridPending.Columns("no_telepon").HeaderText = "No. Telepon"
                GridPending.Columns("tanggal_daftar").HeaderText = "Tanggal Daftar"
                GridPending.Columns("perusahaan").HeaderText = "Perusahaan"
                GridPending.Columns("jabatan").HeaderText = "Jabatan"
                GridPending.Columns("status_ptkp").HeaderText = "PTKP"

                ' Auto size
                GridPending.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            End If

            ' Update label
            lblPending.Text = $"Daftar Registrasi Pending ({dt.Rows.Count})"

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ''' <summary>
    ''' Approve selected user
    ''' </summary>
    Private Sub btnApprove_Click(sender As Object, e As EventArgs) Handles btnApprove.Click
        If GridPending.SelectedRows.Count = 0 Then
            MsgBox("Pilih user yang akan di-approve!", MsgBoxStyle.Exclamation)
            Return
        End If

        Dim selectedNPWP As String = GridPending.SelectedRows(0).Cells("npwp").Value.ToString()
        Dim selectedNama As String = GridPending.SelectedRows(0).Cells("nama").Value.ToString()

        Dim confirm = MsgBox($"Approve registrasi {selectedNama}?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Konfirmasi")
        If confirm = MsgBoxResult.No Then Return

        Try
            modulkoneksi.BukaKoneksi()

            Dim sql As String = "UPDATE users SET status_validasi = 'approved' WHERE npwp = @npwp"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", selectedNPWP)
            cmd.ExecuteNonQuery()

            MsgBox($"User {selectedNama} berhasil di-approve!", MsgBoxStyle.Information, "Sukses")
            LoadPendingUsers()

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ''' <summary>
    ''' Reject selected user
    ''' </summary>
    Private Sub btnReject_Click(sender As Object, e As EventArgs) Handles btnReject.Click
        If GridPending.SelectedRows.Count = 0 Then
            MsgBox("Pilih user yang akan di-reject!", MsgBoxStyle.Exclamation)
            Return
        End If

        Dim selectedNPWP As String = GridPending.SelectedRows(0).Cells("npwp").Value.ToString()
        Dim selectedNama As String = GridPending.SelectedRows(0).Cells("nama").Value.ToString()

        Dim confirm = MsgBox($"Tolak registrasi {selectedNama}? User akan dihapus dari sistem.", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "Konfirmasi")
        If confirm = MsgBoxResult.No Then Return

        Try
            modulkoneksi.BukaKoneksi()

            ' Delete pekerjaan first (FK constraint)
            Dim sqlPekerjaan As String = "DELETE FROM pekerjaan WHERE wp_npwp = @npwp"
            Dim cmdPekerjaan As New MySqlCommand(sqlPekerjaan, modulkoneksi.koneksi)
            cmdPekerjaan.Parameters.AddWithValue("@npwp", selectedNPWP)
            cmdPekerjaan.ExecuteNonQuery()

            ' Delete user
            Dim sql As String = "DELETE FROM users WHERE npwp = @npwp"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", selectedNPWP)
            cmd.ExecuteNonQuery()

            MsgBox($"Registrasi {selectedNama} ditolak dan data dihapus.", MsgBoxStyle.Information, "Sukses")
            LoadPendingUsers()

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadPendingUsers()
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Dim f As New admin_dashboard()
        f.Show()
        Me.Close()
    End Sub

End Class
