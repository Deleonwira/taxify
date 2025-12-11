Imports MySql.Data.MySqlClient

Public Class admin_validasi_registrasi

    Private Sub admin_validasi_registrasi_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadPendingUsers()
        
        ' Set active menu in navbar
        Admin_navbar1.SetActiveMenu(admin_navbar.MenuType.ValidasiRegistrasi)
    End Sub

    ' ====== NAVBAR EVENT HANDLERS ======
    Private Sub Admin_navbar1_DashboardClicked(sender As Object, e As EventArgs) Handles Admin_navbar1.DashboardClicked
        Dim f As New admin_dashboard()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Admin_navbar1_ValidasiRegistrasiClicked(sender As Object, e As EventArgs) Handles Admin_navbar1.ValidasiRegistrasiClicked
        ' Already on this form, do nothing
    End Sub

    Private Sub Admin_navbar1_ManajemenUserClicked(sender As Object, e As EventArgs) Handles Admin_navbar1.ManajemenUserClicked
        Dim f As New FrmUserManagement()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Admin_navbar1_ManajemenPerusahaanClicked(sender As Object, e As EventArgs) Handles Admin_navbar1.ManajemenPerusahaanClicked
        Dim f As New FrmManagementPerusahaan()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Admin_navbar1_LogoutClicked(sender As Object, e As EventArgs) Handles Admin_navbar1.LogoutClicked
        ModuleSession.ClearSession()
        Dim f As New FrmLogin()
        f.Show()
        Me.Close()
    End Sub

    ''' <summary>
    ''' Load semua user dengan status_validasi = 'pending'
    ''' </summary>
    Private Sub LoadPendingUsers()
        Try
            modulkoneksi.BukaKoneksi()

            Dim sql As String = "
                SELECT 
                    wp.id as wp_id,
                    wp.npwp,
                    wp.nama,
                    wp.email,
                    wp.no_telepon,
                    wp.created_at as tanggal_daftar,
                    COALESCE(pr.nama_perusahaan, 'Freelance') as perusahaan,
                    COALESCE(wp.status_ptkp, '-') as status_ptkp
                FROM wajib_pajak wp
                LEFT JOIN pekerjaan p ON wp.id = p.wajib_pajak_id
                LEFT JOIN perusahaan pr ON p.perusahaan_id = pr.id
                WHERE wp.status_validasi = 'pending'
                ORDER BY wp.created_at DESC
            "

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            Dim adapter As New MySqlDataAdapter(cmd)
            Dim dt As New DataTable()
            adapter.Fill(dt)

            GridPending.DataSource = dt

            ' Format columns
            If GridPending.Columns.Count > 0 Then
                GridPending.Columns("wp_id").Visible = False
                GridPending.Columns("npwp").HeaderText = "NPWP"
                GridPending.Columns("nama").HeaderText = "Nama"
                GridPending.Columns("email").HeaderText = "Email"
                GridPending.Columns("no_telepon").HeaderText = "No. Telepon"
                GridPending.Columns("tanggal_daftar").HeaderText = "Tanggal Daftar"
                GridPending.Columns("perusahaan").HeaderText = "Perusahaan"

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

        Dim selectedWPId As Integer = Convert.ToInt32(GridPending.SelectedRows(0).Cells("wp_id").Value)
        Dim selectedNama As String = GridPending.SelectedRows(0).Cells("nama").Value.ToString()

        Dim confirm = MsgBox($"Approve registrasi {selectedNama}?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Konfirmasi")
        If confirm = MsgBoxResult.No Then Return

        Try
            modulkoneksi.BukaKoneksi()

            Dim sql As String = "UPDATE wajib_pajak SET status_validasi = 'approved' WHERE id = @wp_id"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@wp_id", selectedWPId)
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

        Dim selectedWPId As Integer = Convert.ToInt32(GridPending.SelectedRows(0).Cells("wp_id").Value)
        Dim selectedNama As String = GridPending.SelectedRows(0).Cells("nama").Value.ToString()

        Dim confirm = MsgBox($"Tolak registrasi {selectedNama}? User akan dihapus dari sistem.", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, "Konfirmasi")
        If confirm = MsgBoxResult.No Then Return

        Try
            modulkoneksi.BukaKoneksi()

            ' Delete pekerjaan first (FK constraint)
            Dim sqlPekerjaan As String = "DELETE FROM pekerjaan WHERE wajib_pajak_id = @wp_id"
            Dim cmdPekerjaan As New MySqlCommand(sqlPekerjaan, modulkoneksi.koneksi)
            cmdPekerjaan.Parameters.AddWithValue("@wp_id", selectedWPId)
            cmdPekerjaan.ExecuteNonQuery()

            ' Get user_id from wajib_pajak
            Dim sqlGetUserId As String = "SELECT user_id FROM wajib_pajak WHERE id = @wp_id"
            Dim cmdGetUserId As New MySqlCommand(sqlGetUserId, modulkoneksi.koneksi)
            cmdGetUserId.Parameters.AddWithValue("@wp_id", selectedWPId)
            Dim userId As Integer = Convert.ToInt32(cmdGetUserId.ExecuteScalar())

            ' Delete wajib_pajak
            Dim sqlDeleteWP As String = "DELETE FROM wajib_pajak WHERE id = @wp_id"
            Dim cmdDeleteWP As New MySqlCommand(sqlDeleteWP, modulkoneksi.koneksi)
            cmdDeleteWP.Parameters.AddWithValue("@wp_id", selectedWPId)
            cmdDeleteWP.ExecuteNonQuery()

            ' Delete user
            Dim sql As String = "DELETE FROM users WHERE id = @user_id"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@user_id", userId)
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

    Private Sub pnlMain_Paint(sender As Object, e As PaintEventArgs) Handles pnlMain.Paint

    End Sub
End Class
