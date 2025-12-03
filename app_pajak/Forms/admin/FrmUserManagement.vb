Imports MySql.Data.MySqlClient

Public Class FrmUserManagement

    Private Sub FrmUserManagement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadStatistics()
        LoadUsers()
    End Sub

    Private Sub LoadStatistics()
        ' TODO: Load statistik jumlah user
        Try
            modulkoneksi.BukaKoneksi()
            
            ' Total users
            Dim sqlTotal As String = "SELECT COUNT(*) FROM users"
            Dim cmdTotal As New MySqlCommand(sqlTotal, modulkoneksi.koneksi)
            Dim totalUsers As Integer = Convert.ToInt32(cmdTotal.ExecuteScalar())
            ' TODO: LblTotalUsersValue.Text = totalUsers.ToString()
            
            ' Active users
            Dim sqlActive As String = "SELECT COUNT(*) FROM users WHERE status_aktif = 'active'"
            Dim cmdActive As New MySqlCommand(sqlActive, modulkoneksi.koneksi)
            Dim activeUsers As Integer = Convert.ToInt32(cmdActive.ExecuteScalar())
            ' TODO: LblActiveUsersValue.Text = activeUsers.ToString()
            
            ' Pending users
            Dim sqlPending As String = "SELECT COUNT(*) FROM users WHERE status_aktif = 'pending'"
            Dim cmdPending As New MySqlCommand(sqlPending, modulkoneksi.koneksi)
            Dim pendingUsers As Integer = Convert.ToInt32(cmdPending.ExecuteScalar())
            ' TODO: LblPendingUsersValue.Text = pendingUsers.ToString()
            
            ' Inactive users
            Dim sqlInactive As String = "SELECT COUNT(*) FROM users WHERE status_aktif = 'inactive'"
            Dim cmdInactive As New MySqlCommand(sqlInactive, modulkoneksi.koneksi)
            Dim inactiveUsers As Integer = Convert.ToInt32(cmdInactive.ExecuteScalar())
            ' TODO: LblInactiveUsersValue.Text = inactiveUsers.ToString()
            
        Catch ex As Exception
            MsgBox("Error loading statistics: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub LoadUsers(Optional searchKeyword As String = "", Optional roleFilter As String = "", Optional statusFilter As String = "")
        Try
            modulkoneksi.BukaKoneksi()
            
            Dim sql As String = "SELECT id, npwp, nama, email, tipe_user, status_aktif, created_at FROM users WHERE 1=1"
            
            If Not String.IsNullOrEmpty(searchKeyword) Then
                sql &= " AND (nama LIKE @search OR email LIKE @search OR npwp LIKE @search)"
            End If
            
            If Not String.IsNullOrEmpty(roleFilter) And roleFilter <> "Semua Role" Then
                sql &= " AND tipe_user = @role"
            End If
            
            If Not String.IsNullOrEmpty(statusFilter) And statusFilter <> "Semua Status" Then
                sql &= " AND status_aktif = @status"
            End If
            
            sql &= " ORDER BY created_at DESC"
            
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            If Not String.IsNullOrEmpty(searchKeyword) Then
                cmd.Parameters.AddWithValue("@search", "%" & searchKeyword & "%")
            End If
            If Not String.IsNullOrEmpty(roleFilter) And roleFilter <> "Semua Role" Then
                cmd.Parameters.AddWithValue("@role", roleFilter.ToLower().Replace(" ", "_"))
            End If
            If Not String.IsNullOrEmpty(statusFilter) And statusFilter <> "Semua Status" Then
                cmd.Parameters.AddWithValue("@status", statusFilter.ToLower())
            End If
            
            Dim adapter As New MySqlDataAdapter(cmd)
            Dim table As New DataTable()
            adapter.Fill(table)
            
            ' TODO: Bind to GridUsers DataGridView
            ' GridUsers.DataSource = table
            
        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ' TODO: Add event handlers for:
    ' - TxtSearch TextChanged
    ' - CmbRole SelectionChanged
    ' - CmbStatus SelectionChanged
    ' - BtnAddUser Click
    ' - GridUsers CellClick for edit button

    Private Sub UpdateUserStatus(userId As Integer, newStatus As String)
        Try
            modulkoneksi.BukaKoneksi()
            Dim sql As String = "UPDATE users SET status_aktif = @status WHERE id = @id"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@status", newStatus)
            cmd.Parameters.AddWithValue("@id", userId)
            cmd.ExecuteNonQuery()
            
            MsgBox("Status user berhasil diupdate!", MsgBoxStyle.Information)
            LoadUsers() ' Reload
            LoadStatistics() ' Update stats
            
        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

End Class
