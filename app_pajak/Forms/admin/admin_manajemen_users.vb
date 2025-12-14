Imports MySql.Data.MySqlClient

Public Class FrmUserManagement

    Private isLoading As Boolean = True ' Flag to prevent events during initialization

    Private Sub FrmUserManagement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set default combo selections (events will fire but isLoading prevents LoadUsers)
        CmbRole.SelectedIndex = 0
        CmbStatus.SelectedIndex = 0
        CmbSort.SelectedIndex = 0
        
        ' Now allow events to work
        isLoading = False
        
        LoadStatistics()
        LoadUsers()
        
        ' Set active menu in navbar
        Pk_navbar1.SetActiveMenu(admin_navbar.MenuType.ManajemenUser)
    End Sub

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
        ' Already on this form, do nothing
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

    Private Sub Pk_navbar1_LogoutClicked(sender As Object, e As EventArgs) Handles Pk_navbar1.LogoutClicked
        ModuleSession.ClearSession()
        Dim f As New FrmLogin()
        f.Show()
        Me.Close()
    End Sub

    Private Sub LoadStatistics()
        Try
            modulkoneksi.BukaKoneksi()
            
            ' Total users
            Dim sqlTotal As String = "SELECT COUNT(*) FROM users"
            Dim cmdTotal As New MySqlCommand(sqlTotal, modulkoneksi.koneksi)
            Dim totalUsers As Integer = Convert.ToInt32(cmdTotal.ExecuteScalar())
            LblTotalUsersValue.Text = totalUsers.ToString()
            
            ' Active users (is_active = 1)
            Dim sqlActive As String = "SELECT COUNT(*) FROM users WHERE is_active = 1"
            Dim cmdActive As New MySqlCommand(sqlActive, modulkoneksi.koneksi)
            Dim activeUsers As Integer = Convert.ToInt32(cmdActive.ExecuteScalar())
            LblActiveUsersValue.Text = activeUsers.ToString()
            
            ' Pending users (wajib_pajak with status_validasi = 'pending')
            Dim sqlPending As String = "SELECT COUNT(*) FROM wajib_pajak WHERE status_validasi = 'pending'"
            Dim cmdPending As New MySqlCommand(sqlPending, modulkoneksi.koneksi)
            Dim pendingUsers As Integer = Convert.ToInt32(cmdPending.ExecuteScalar())
            LblPendingUsersValue.Text = pendingUsers.ToString()
            
            ' Inactive users (is_active = 0)
            Dim sqlInactive As String = "SELECT COUNT(*) FROM users WHERE is_active = 0"
            Dim cmdInactive As New MySqlCommand(sqlInactive, modulkoneksi.koneksi)
            Dim inactiveUsers As Integer = Convert.ToInt32(cmdInactive.ExecuteScalar())
            LblInactiveUsersValue.Text = inactiveUsers.ToString()
            
        Catch ex As Exception
            MsgBox("Error loading statistics: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub LoadUsers()
        Try
            modulkoneksi.BukaKoneksi()
            
            Dim searchKeyword As String = TxtSearch.Text.Trim()
            Dim roleFilter As String = If(CmbRole.SelectedItem IsNot Nothing, CmbRole.SelectedItem.ToString(), "")
            Dim statusFilter As String = If(CmbStatus.SelectedItem IsNot Nothing, CmbStatus.SelectedItem.ToString(), "")
            Dim sortOption As String = If(CmbSort.SelectedItem IsNot Nothing, CmbSort.SelectedItem.ToString(), "")
            
            Dim sql As String = "
                SELECT u.id, u.username, u.tipe_user, u.is_active, u.created_at,
                       COALESCE(wp.nama, pk.nama, a.nama, u.username) AS nama,
                       COALESCE(wp.email, pk.email, a.email, '-') AS email
                FROM users u
                LEFT JOIN wajib_pajak wp ON u.id = wp.user_id AND u.tipe_user = 'wajib_pajak'
                LEFT JOIN pemberi_kerja pk ON u.id = pk.user_id AND u.tipe_user = 'pemberi_kerja'
                LEFT JOIN admin a ON u.id = a.user_id AND u.tipe_user = 'admin'
                WHERE 1=1"
            
            If Not String.IsNullOrEmpty(searchKeyword) Then
                sql &= " AND (COALESCE(wp.nama, pk.nama, a.nama, u.username) LIKE @search OR COALESCE(wp.email, pk.email, a.email) LIKE @search)"
            End If
            
            If Not String.IsNullOrEmpty(roleFilter) And roleFilter <> "Semua Role" Then
                Select Case roleFilter
                    Case "Admin"
                        sql &= " AND u.tipe_user = 'admin'"
                    Case "Wajib Pajak"
                        sql &= " AND u.tipe_user = 'wajib_pajak'"
                    Case "Pemberi Kerja"
                        sql &= " AND u.tipe_user = 'pemberi_kerja'"
                End Select
            End If
            
            If Not String.IsNullOrEmpty(statusFilter) And statusFilter <> "Semua Status" Then
                Select Case statusFilter
                    Case "Aktif"
                        sql &= " AND u.is_active = 1"
                    Case "Non-Aktif"
                        sql &= " AND u.is_active = 0"
                End Select
            End If
            
            ' Sort order
            Select Case sortOption
                Case "Sortir: Terlama"
                    sql &= " ORDER BY u.created_at ASC"
                Case "Nama A-Z"
                    sql &= " ORDER BY nama ASC"
                Case "Nama Z-A"
                    sql &= " ORDER BY nama DESC"
                Case Else ' Default: Terbaru
                    sql &= " ORDER BY u.created_at DESC"
            End Select
            
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            If Not String.IsNullOrEmpty(searchKeyword) Then
                cmd.Parameters.AddWithValue("@search", "%" & searchKeyword & "%")
            End If
            
            Dim adapter As New MySqlDataAdapter(cmd)
            Dim table As New DataTable()
            adapter.Fill(table)
            
            ' Clear grid and populate manually for better formatting
            GridUsers.Rows.Clear()
            
            Dim rowNum As Integer = 1
            For Each row As DataRow In table.Rows
                Dim tipeUser As String = row("tipe_user").ToString()
                Dim roleDisplay As String = ""
                Select Case tipeUser
                    Case "admin"
                        roleDisplay = "Admin"
                    Case "wajib_pajak"
                        roleDisplay = "Wajib Pajak"
                    Case "pemberi_kerja"
                        roleDisplay = "Pemberi Kerja"
                    Case Else
                        roleDisplay = tipeUser
                End Select
                
                Dim isActive As Boolean = Convert.ToBoolean(row("is_active"))
                Dim statusDisplay As String = If(isActive, "Aktif", "Non-Aktif")
                
                Dim createdAt As DateTime = Convert.ToDateTime(row("created_at"))
                Dim dateDisplay As String = createdAt.ToString("dd MMM yyyy")
                
                GridUsers.Rows.Add(
                    rowNum,
                    row("nama").ToString(),
                    row("email").ToString(),
                    roleDisplay,
                    statusDisplay,
                    dateDisplay,
                    "Detail"
                )
                
                ' Store user_id in row tag for later use
                GridUsers.Rows(GridUsers.Rows.Count - 1).Tag = row("id")
                
                rowNum += 1
            Next
            
            ' Update table subtitle with count
            LblTableSubtitle.Text = $"Menampilkan {table.Rows.Count} pengguna sistem."
            
        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ' ====== FILTER EVENT HANDLERS ======
    Private Sub TxtSearch_TextChanged(sender As Object, e As EventArgs) Handles TxtSearch.TextChanged
        If isLoading Then Return
        LoadUsers()
    End Sub

    Private Sub CmbRole_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbRole.SelectedIndexChanged
        If isLoading Then Return
        LoadUsers()
    End Sub

    Private Sub CmbStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbStatus.SelectedIndexChanged
        If isLoading Then Return
        LoadUsers()
    End Sub

    Private Sub CmbSort_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbSort.SelectedIndexChanged
        If isLoading Then Return
        LoadUsers()
    End Sub

    Private Sub BtnAddUser_Click(sender As Object, e As EventArgs) Handles BtnAddUser.Click
        ShowAddUserDialog()
    End Sub
    
    Private Sub ShowAddUserDialog()
        Using dialog As New Form()
            dialog.Text = "Tambah User Baru"
            dialog.Size = New Size(480, 580)
            dialog.StartPosition = FormStartPosition.CenterParent
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog
            dialog.MaximizeBox = False
            dialog.MinimizeBox = False
            dialog.BackColor = Color.FromArgb(247, 248, 252)
            dialog.AutoScroll = True
            
            Dim yPos As Integer = 20
            Dim controls As New Dictionary(Of String, Control)
            
            ' Role Selection
            Dim lblRole As New Label() With {.Text = "Tipe User:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI Semibold", 9, FontStyle.Bold)}
            Dim cmbRole As New ComboBox() With {.Location = New Point(150, yPos - 3), .Width = 200, .DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Segoe UI", 9)}
            cmbRole.Items.AddRange(New String() {"Wajib Pajak", "Pemberi Kerja"})
            cmbRole.SelectedIndex = 0
            dialog.Controls.Add(lblRole)
            dialog.Controls.Add(cmbRole)
            controls("tipe_user") = cmbRole
            yPos += 35
            
            ' Username
            Dim lblUsername As New Label() With {.Text = "Username:*", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
            Dim txtUsername As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 200, .Font = New Font("Segoe UI", 9)}
            dialog.Controls.Add(lblUsername)
            dialog.Controls.Add(txtUsername)
            controls("username") = txtUsername
            yPos += 35
            
            ' Password
            Dim lblPassword As New Label() With {.Text = "Password:*", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
            Dim txtPassword As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 200, .Font = New Font("Segoe UI", 9), .PasswordChar = "●"c}
            dialog.Controls.Add(lblPassword)
            dialog.Controls.Add(txtPassword)
            controls("password") = txtPassword
            yPos += 35
            
            ' Confirm Password
            Dim lblConfirmPwd As New Label() With {.Text = "Konfirmasi:*", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
            Dim txtConfirmPwd As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 200, .Font = New Font("Segoe UI", 9), .PasswordChar = "●"c}
            dialog.Controls.Add(lblConfirmPwd)
            dialog.Controls.Add(txtConfirmPwd)
            controls("confirm_password") = txtConfirmPwd
            yPos += 35
            
            ' Nama
            Dim lblNama As New Label() With {.Text = "Nama:*", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
            Dim txtNama As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 200, .Font = New Font("Segoe UI", 9)}
            dialog.Controls.Add(lblNama)
            dialog.Controls.Add(txtNama)
            controls("nama") = txtNama
            yPos += 35
            
            ' Email
            Dim lblEmail As New Label() With {.Text = "Email:*", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
            Dim txtEmail As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 200, .Font = New Font("Segoe UI", 9)}
            dialog.Controls.Add(lblEmail)
            dialog.Controls.Add(txtEmail)
            controls("email") = txtEmail
            yPos += 35
            
            ' No. Telepon
            Dim lblTelp As New Label() With {.Text = "No. Telepon:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
            Dim txtTelp As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 150, .Font = New Font("Segoe UI", 9)}
            dialog.Controls.Add(lblTelp)
            dialog.Controls.Add(txtTelp)
            controls("no_telepon") = txtTelp
            yPos += 35
            
            ' === Wajib Pajak Specific Fields ===
            Dim wpStartY As Integer = yPos
            
            ' NPWP
            Dim lblNpwp As New Label() With {.Text = "NPWP:*", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9), .Tag = "wp"}
            Dim txtNpwp As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 200, .Font = New Font("Segoe UI", 9), .Tag = "wp"}
            dialog.Controls.Add(lblNpwp)
            dialog.Controls.Add(txtNpwp)
            controls("npwp") = txtNpwp
            yPos += 35
            
            ' NIK
            Dim lblNik As New Label() With {.Text = "NIK:*", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9), .Tag = "wp"}
            Dim txtNik As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 200, .Font = New Font("Segoe UI", 9), .Tag = "wp", .MaxLength = 16}
            dialog.Controls.Add(lblNik)
            dialog.Controls.Add(txtNik)
            controls("nik") = txtNik
            yPos += 35
            
            ' Alamat
            Dim lblAlamat As New Label() With {.Text = "Alamat:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9), .Tag = "wp"}
            Dim txtAlamat As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 200, .Font = New Font("Segoe UI", 9), .Tag = "wp"}
            dialog.Controls.Add(lblAlamat)
            dialog.Controls.Add(txtAlamat)
            controls("alamat") = txtAlamat
            yPos += 35
            
            ' Status PTKP
            Dim lblPtkp As New Label() With {.Text = "Status PTKP:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9), .Tag = "wp"}
            Dim cmbPtkp As New ComboBox() With {.Location = New Point(150, yPos - 3), .Width = 100, .DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Segoe UI", 9), .Tag = "wp"}
            cmbPtkp.Items.AddRange(New String() {"TK0", "TK1", "TK2", "TK3", "K0", "K1", "K2", "K3"})
            cmbPtkp.SelectedIndex = 0
            dialog.Controls.Add(lblPtkp)
            dialog.Controls.Add(cmbPtkp)
            controls("status_ptkp") = cmbPtkp
            yPos += 35
            
            Dim wpEndY As Integer = yPos
            
            ' === Pemberi Kerja Specific Fields ===
            Dim pkStartY As Integer = wpStartY
            
            ' Perusahaan ComboBox
            Dim lblPerusahaan As New Label() With {.Text = "Perusahaan:*", .Location = New Point(20, pkStartY), .Width = 120, .Font = New Font("Segoe UI", 9), .Tag = "pk", .Visible = False}
            Dim cmbPerusahaan As New ComboBox() With {.Location = New Point(150, pkStartY - 3), .Width = 200, .DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Segoe UI", 9), .Tag = "pk", .Visible = False}
            dialog.Controls.Add(lblPerusahaan)
            dialog.Controls.Add(cmbPerusahaan)
            controls("perusahaan") = cmbPerusahaan
            
            ' Load perusahaan data
            Try
                modulkoneksi.BukaKoneksi()
                Dim sqlPerusahaan As String = "SELECT id, nama_perusahaan FROM perusahaan WHERE nama_perusahaan != 'Freelance / Tidak Terdaftar' ORDER BY nama_perusahaan"
                Dim cmdPerusahaan As New MySqlCommand(sqlPerusahaan, modulkoneksi.koneksi)
                Dim reader = cmdPerusahaan.ExecuteReader()
                While reader.Read()
                    cmbPerusahaan.Items.Add(New KeyValuePair(Of Integer, String)(reader.GetInt32("id"), reader.GetString("nama_perusahaan")))
                End While
                reader.Close()
                If cmbPerusahaan.Items.Count > 0 Then cmbPerusahaan.SelectedIndex = 0
            Catch ex As Exception
                ' Ignore - will be empty
            Finally
                modulkoneksi.TutupKoneksi()
            End Try
            
            cmbPerusahaan.DisplayMember = "Value"
            cmbPerusahaan.ValueMember = "Key"
            
            ' Handle role change to show/hide fields
            AddHandler cmbRole.SelectedIndexChanged, Sub(s, ev)
                Dim isWP As Boolean = (cmbRole.SelectedItem.ToString() = "Wajib Pajak")
                For Each ctrl As Control In dialog.Controls
                    If ctrl.Tag IsNot Nothing Then
                        If ctrl.Tag.ToString() = "wp" Then
                            ctrl.Visible = isWP
                        ElseIf ctrl.Tag.ToString() = "pk" Then
                            ctrl.Visible = Not isWP
                        End If
                    End If
                Next
            End Sub
            
            ' Buttons
            yPos = wpEndY + 20
            Dim btnSave As New Button() With {
                .Text = "Simpan",
                .Location = New Point(150, yPos),
                .Size = New Size(100, 35),
                .BackColor = Color.FromArgb(156, 0, 219),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat,
                .Font = New Font("Segoe UI Semibold", 9, FontStyle.Bold)
            }
            btnSave.FlatAppearance.BorderSize = 0
            
            Dim btnCancel As New Button() With {
                .Text = "Batal",
                .Location = New Point(260, yPos),
                .Size = New Size(100, 35),
                .BackColor = Color.FromArgb(120, 128, 146),
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat,
                .Font = New Font("Segoe UI Semibold", 9, FontStyle.Bold),
                .DialogResult = DialogResult.Cancel
            }
            btnCancel.FlatAppearance.BorderSize = 0
            
            dialog.Controls.Add(btnSave)
            dialog.Controls.Add(btnCancel)
            dialog.CancelButton = btnCancel
            
            ' Save button click handler
            AddHandler btnSave.Click, Sub(s, ev)
                If SaveNewUser(controls) Then
                    dialog.DialogResult = DialogResult.OK
                    dialog.Close()
                End If
            End Sub
            
            dialog.ShowDialog()
        End Using
    End Sub
    
    Private Function SaveNewUser(controls As Dictionary(Of String, Control)) As Boolean
        Try
            ' Get values
            Dim tipeUser As String = DirectCast(controls("tipe_user"), ComboBox).SelectedItem.ToString()
            Dim username As String = DirectCast(controls("username"), TextBox).Text.Trim()
            Dim password As String = DirectCast(controls("password"), TextBox).Text
            Dim confirmPwd As String = DirectCast(controls("confirm_password"), TextBox).Text
            Dim nama As String = DirectCast(controls("nama"), TextBox).Text.Trim()
            Dim email As String = DirectCast(controls("email"), TextBox).Text.Trim()
            Dim noTelepon As String = DirectCast(controls("no_telepon"), TextBox).Text.Trim()
            
            ' Validate common fields
            If String.IsNullOrEmpty(username) Then
                MsgBox("Username wajib diisi!", MsgBoxStyle.Exclamation)
                Return False
            End If
            
            If String.IsNullOrEmpty(password) Then
                MsgBox("Password wajib diisi!", MsgBoxStyle.Exclamation)
                Return False
            End If
            
            If password <> confirmPwd Then
                MsgBox("Password dan konfirmasi password tidak cocok!", MsgBoxStyle.Exclamation)
                Return False
            End If
            
            If String.IsNullOrEmpty(nama) Then
                MsgBox("Nama wajib diisi!", MsgBoxStyle.Exclamation)
                Return False
            End If
            
            If String.IsNullOrEmpty(email) Then
                MsgBox("Email wajib diisi!", MsgBoxStyle.Exclamation)
                Return False
            End If
            
            modulkoneksi.BukaKoneksi()
            
            ' Check unique username
            Dim sqlCheckUser As String = "SELECT COUNT(*) FROM users WHERE username = @username"
            Dim cmdCheckUser As New MySqlCommand(sqlCheckUser, modulkoneksi.koneksi)
            cmdCheckUser.Parameters.AddWithValue("@username", username)
            If Convert.ToInt32(cmdCheckUser.ExecuteScalar()) > 0 Then
                MsgBox("Username sudah digunakan!", MsgBoxStyle.Exclamation)
                Return False
            End If
            
            Dim tipeUserDb As String = If(tipeUser = "Wajib Pajak", "wajib_pajak", "pemberi_kerja")
            
            If tipeUser = "Wajib Pajak" Then
                Dim npwp As String = DirectCast(controls("npwp"), TextBox).Text.Trim()
                Dim nik As String = DirectCast(controls("nik"), TextBox).Text.Trim()
                
                If String.IsNullOrEmpty(npwp) Then
                    MsgBox("NPWP wajib diisi!", MsgBoxStyle.Exclamation)
                    Return False
                End If
                
                If String.IsNullOrEmpty(nik) Then
                    MsgBox("NIK wajib diisi!", MsgBoxStyle.Exclamation)
                    Return False
                End If
                
                If nik.Length <> 16 OrElse Not nik.All(AddressOf Char.IsDigit) Then
                    MsgBox("NIK harus 16 digit angka!", MsgBoxStyle.Exclamation)
                    Return False
                End If
                
                ' Check unique NPWP
                Dim sqlCheckNpwp As String = "SELECT COUNT(*) FROM wajib_pajak WHERE npwp = @npwp"
                Dim cmdCheckNpwp As New MySqlCommand(sqlCheckNpwp, modulkoneksi.koneksi)
                cmdCheckNpwp.Parameters.AddWithValue("@npwp", npwp)
                If Convert.ToInt32(cmdCheckNpwp.ExecuteScalar()) > 0 Then
                    MsgBox("NPWP sudah terdaftar!", MsgBoxStyle.Exclamation)
                    Return False
                End If
                
                ' Check unique NIK
                Dim sqlCheckNik As String = "SELECT COUNT(*) FROM wajib_pajak WHERE nik = @nik"
                Dim cmdCheckNik As New MySqlCommand(sqlCheckNik, modulkoneksi.koneksi)
                cmdCheckNik.Parameters.AddWithValue("@nik", nik)
                If Convert.ToInt32(cmdCheckNik.ExecuteScalar()) > 0 Then
                    MsgBox("NIK sudah terdaftar!", MsgBoxStyle.Exclamation)
                    Return False
                End If
                
                ' Hash password
                Dim passwordHash As String = HashPassword(password)
                
                ' Insert user
                Dim sqlInsertUser As String = "INSERT INTO users (username, password_hash, tipe_user, is_active) VALUES (@username, @password, @tipe_user, 1)"
                Dim cmdInsertUser As New MySqlCommand(sqlInsertUser, modulkoneksi.koneksi)
                cmdInsertUser.Parameters.AddWithValue("@username", username)
                cmdInsertUser.Parameters.AddWithValue("@password", passwordHash)
                cmdInsertUser.Parameters.AddWithValue("@tipe_user", tipeUserDb)
                cmdInsertUser.ExecuteNonQuery()
                
                Dim userId As Long = cmdInsertUser.LastInsertedId
                
                ' Insert wajib_pajak
                Dim alamat As String = DirectCast(controls("alamat"), TextBox).Text.Trim()
                Dim statusPtkp As String = DirectCast(controls("status_ptkp"), ComboBox).SelectedItem.ToString()
                
                Dim sqlInsertWp As String = "INSERT INTO wajib_pajak (user_id, npwp, nik, nama, email, no_telepon, alamat, status_ptkp, status_validasi) VALUES (@user_id, @npwp, @nik, @nama, @email, @no_telepon, @alamat, @status_ptkp, 'approved')"
                Dim cmdInsertWp As New MySqlCommand(sqlInsertWp, modulkoneksi.koneksi)
                cmdInsertWp.Parameters.AddWithValue("@user_id", userId)
                cmdInsertWp.Parameters.AddWithValue("@npwp", npwp)
                cmdInsertWp.Parameters.AddWithValue("@nik", nik)
                cmdInsertWp.Parameters.AddWithValue("@nama", nama)
                cmdInsertWp.Parameters.AddWithValue("@email", email)
                cmdInsertWp.Parameters.AddWithValue("@no_telepon", If(String.IsNullOrEmpty(noTelepon), DBNull.Value, noTelepon))
                cmdInsertWp.Parameters.AddWithValue("@alamat", If(String.IsNullOrEmpty(alamat), DBNull.Value, alamat))
                cmdInsertWp.Parameters.AddWithValue("@status_ptkp", statusPtkp)
                cmdInsertWp.ExecuteNonQuery()
                
            Else
                ' Pemberi Kerja
                Dim cmbPerusahaan As ComboBox = DirectCast(controls("perusahaan"), ComboBox)
                If cmbPerusahaan.SelectedItem Is Nothing Then
                    MsgBox("Pilih perusahaan!", MsgBoxStyle.Exclamation)
                    Return False
                End If
                
                Dim perusahaanKvp As KeyValuePair(Of Integer, String) = DirectCast(cmbPerusahaan.SelectedItem, KeyValuePair(Of Integer, String))
                Dim perusahaanId As Integer = perusahaanKvp.Key
                
                ' Hash password
                Dim passwordHash As String = HashPassword(password)
                
                ' Insert user
                Dim sqlInsertUser As String = "INSERT INTO users (username, password_hash, tipe_user, is_active) VALUES (@username, @password, @tipe_user, 1)"
                Dim cmdInsertUser As New MySqlCommand(sqlInsertUser, modulkoneksi.koneksi)
                cmdInsertUser.Parameters.AddWithValue("@username", username)
                cmdInsertUser.Parameters.AddWithValue("@password", passwordHash)
                cmdInsertUser.Parameters.AddWithValue("@tipe_user", tipeUserDb)
                cmdInsertUser.ExecuteNonQuery()
                
                Dim userId As Long = cmdInsertUser.LastInsertedId
                
                ' Insert pemberi_kerja
                Dim sqlInsertPk As String = "INSERT INTO pemberi_kerja (user_id, perusahaan_id, nama, email, no_telepon) VALUES (@user_id, @perusahaan_id, @nama, @email, @no_telepon)"
                Dim cmdInsertPk As New MySqlCommand(sqlInsertPk, modulkoneksi.koneksi)
                cmdInsertPk.Parameters.AddWithValue("@user_id", userId)
                cmdInsertPk.Parameters.AddWithValue("@perusahaan_id", perusahaanId)
                cmdInsertPk.Parameters.AddWithValue("@nama", nama)
                cmdInsertPk.Parameters.AddWithValue("@email", email)
                cmdInsertPk.Parameters.AddWithValue("@no_telepon", If(String.IsNullOrEmpty(noTelepon), DBNull.Value, noTelepon))
                cmdInsertPk.ExecuteNonQuery()
            End If
            
            MsgBox("User baru berhasil ditambahkan!", MsgBoxStyle.Information)
            LoadUsers()
            LoadStatistics()
            Return True
            
        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
            Return False
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Function
    
    Private Function HashPassword(password As String) As String
        Using sha256 As System.Security.Cryptography.SHA256 = System.Security.Cryptography.SHA256.Create()
            Dim bytes As Byte() = System.Text.Encoding.UTF8.GetBytes(password)
            Dim hash As Byte() = sha256.ComputeHash(bytes)
            Dim sb As New System.Text.StringBuilder()
            For Each b As Byte In hash
                sb.Append(b.ToString("x2"))
            Next
            Return sb.ToString()
        End Using
    End Function

    Private Sub GridUsers_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles GridUsers.CellClick
        If e.RowIndex < 0 Then Return
        
        Dim userId As Integer = Convert.ToInt32(GridUsers.Rows(e.RowIndex).Tag)
        Dim userName As String = GridUsers.Rows(e.RowIndex).Cells("colNama").Value.ToString()
        Dim userRole As String = GridUsers.Rows(e.RowIndex).Cells("colRole").Value.ToString()
        
        ' Check if Edit column was clicked
        If e.ColumnIndex = GridUsers.Columns("colActions").Index Then
            ShowEditUserDialog(userId, userRole)
        End If
        
        ' Check if Delete column was clicked
        If e.ColumnIndex = GridUsers.Columns("colDelete").Index Then
            Dim result As DialogResult = MessageBox.Show(
                $"Apakah Anda yakin ingin menghapus user '{userName}'?" & vbCrLf & vbCrLf &
                "⚠️ Tindakan ini tidak dapat dibatalkan!",
                "Konfirmasi Hapus",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            )
            
            If result = DialogResult.Yes Then
                DeleteUser(userId, userRole)
            End If
        End If
    End Sub

    Private Sub ShowEditUserDialog(userId As Integer, userRole As String)
        Try
            modulkoneksi.BukaKoneksi()
            
            ' Get user data based on role
            Dim userData As New Dictionary(Of String, String)
            Dim sql As String = ""
            
            Select Case userRole
                Case "Admin"
                    sql = "SELECT u.username, u.is_active, a.nama, a.email, a.no_telepon 
                           FROM users u 
                           INNER JOIN admin a ON u.id = a.user_id 
                           WHERE u.id = @id"
                Case "Wajib Pajak"
                    sql = "SELECT u.username, u.is_active, wp.nama, wp.email, wp.no_telepon, wp.npwp, wp.nik, wp.alamat, wp.status_ptkp 
                           FROM users u 
                           INNER JOIN wajib_pajak wp ON u.id = wp.user_id 
                           WHERE u.id = @id"
                Case "Pemberi Kerja"
                    sql = "SELECT u.username, u.is_active, pk.nama, pk.email, pk.no_telepon, p.nama_perusahaan 
                           FROM users u 
                           INNER JOIN pemberi_kerja pk ON u.id = pk.user_id 
                           LEFT JOIN perusahaan p ON pk.perusahaan_id = p.id
                           WHERE u.id = @id"
            End Select
            
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@id", userId)
            Dim reader = cmd.ExecuteReader()
            
            If reader.Read() Then
                For i As Integer = 0 To reader.FieldCount - 1
                    userData(reader.GetName(i)) = If(reader.IsDBNull(i), "", reader.GetValue(i).ToString())
                Next
            End If
            reader.Close()
            modulkoneksi.TutupKoneksi()
            
            ' Create edit dialog
            Using dialog As New Form()
                dialog.Text = $"Edit User - {userRole}"
                dialog.Size = New Size(450, If(userRole = "Wajib Pajak", 550, 400))
                dialog.StartPosition = FormStartPosition.CenterParent
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog
                dialog.MaximizeBox = False
                dialog.MinimizeBox = False
                dialog.BackColor = Color.FromArgb(247, 248, 252)
                
                Dim yPos As Integer = 20
                Dim controls As New Dictionary(Of String, Control)
                
                ' Username (read-only)
                Dim lblUsername As New Label() With {.Text = "Username:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
                Dim txtUsername As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 200, .Font = New Font("Segoe UI", 9), .Text = If(userData.ContainsKey("username"), userData("username"), ""), .ReadOnly = True, .BackColor = Color.FromArgb(235, 235, 235)}
                dialog.Controls.Add(lblUsername)
                dialog.Controls.Add(txtUsername)
                controls("username") = txtUsername
                yPos += 35
                
                ' Status Aktif (checkbox)
                Dim lblStatus As New Label() With {.Text = "Status Aktif:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
                Dim chkStatus As New CheckBox() With {.Location = New Point(150, yPos - 3), .Checked = If(userData.ContainsKey("is_active"), userData("is_active") = "1" Or userData("is_active").ToLower() = "true", False)}
                dialog.Controls.Add(lblStatus)
                dialog.Controls.Add(chkStatus)
                controls("is_active") = chkStatus
                yPos += 35
                
                ' Nama
                Dim lblNama As New Label() With {.Text = "Nama:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
                Dim txtNama As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 200, .Font = New Font("Segoe UI", 9), .Text = If(userData.ContainsKey("nama"), userData("nama"), "")}
                dialog.Controls.Add(lblNama)
                dialog.Controls.Add(txtNama)
                controls("nama") = txtNama
                yPos += 35
                
                ' Email
                Dim lblEmail As New Label() With {.Text = "Email:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
                Dim txtEmail As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 200, .Font = New Font("Segoe UI", 9), .Text = If(userData.ContainsKey("email"), userData("email"), "")}
                dialog.Controls.Add(lblEmail)
                dialog.Controls.Add(txtEmail)
                controls("email") = txtEmail
                yPos += 35
                
                ' No. Telepon
                Dim lblTelp As New Label() With {.Text = "No. Telepon:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
                Dim txtTelp As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 150, .Font = New Font("Segoe UI", 9), .Text = If(userData.ContainsKey("no_telepon"), userData("no_telepon"), "")}
                dialog.Controls.Add(lblTelp)
                dialog.Controls.Add(txtTelp)
                controls("no_telepon") = txtTelp
                yPos += 35
                
                ' Role-specific fields
                Select Case userRole
                    Case "Wajib Pajak"
                        ' NPWP
                        Dim lblNpwp As New Label() With {.Text = "NPWP:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
                        Dim txtNpwp As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 200, .Font = New Font("Segoe UI", 9), .Text = If(userData.ContainsKey("npwp"), userData("npwp"), "")}
                        dialog.Controls.Add(lblNpwp)
                        dialog.Controls.Add(txtNpwp)
                        controls("npwp") = txtNpwp
                        yPos += 35
                        
                        ' NIK
                        Dim lblNik As New Label() With {.Text = "NIK:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
                        Dim txtNik As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 200, .Font = New Font("Segoe UI", 9), .Text = If(userData.ContainsKey("nik"), userData("nik"), "")}
                        dialog.Controls.Add(lblNik)
                        dialog.Controls.Add(txtNik)
                        controls("nik") = txtNik
                        yPos += 35
                        
                        ' Alamat
                        Dim lblAlamat As New Label() With {.Text = "Alamat:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
                        Dim txtAlamat As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 200, .Font = New Font("Segoe UI", 9), .Text = If(userData.ContainsKey("alamat"), userData("alamat"), "")}
                        dialog.Controls.Add(lblAlamat)
                        dialog.Controls.Add(txtAlamat)
                        controls("alamat") = txtAlamat
                        yPos += 35
                        
                        ' Status PTKP
                        Dim lblPtkp As New Label() With {.Text = "Status PTKP:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
                        Dim cmbPtkp As New ComboBox() With {.Location = New Point(150, yPos - 3), .Width = 100, .DropDownStyle = ComboBoxStyle.DropDownList, .Font = New Font("Segoe UI", 9)}
                        cmbPtkp.Items.AddRange(New String() {"TK0", "TK1", "TK2", "TK3", "K0", "K1", "K2", "K3"})
                        cmbPtkp.SelectedItem = If(userData.ContainsKey("status_ptkp"), userData("status_ptkp"), "TK0")
                        dialog.Controls.Add(lblPtkp)
                        dialog.Controls.Add(cmbPtkp)
                        controls("status_ptkp") = cmbPtkp
                        yPos += 35
                        
                    Case "Pemberi Kerja"
                        ' Perusahaan (read-only)
                        Dim lblPerusahaan As New Label() With {.Text = "Perusahaan:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
                        Dim txtPerusahaan As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 200, .Font = New Font("Segoe UI", 9), .Text = If(userData.ContainsKey("nama_perusahaan"), userData("nama_perusahaan"), ""), .ReadOnly = True, .BackColor = Color.FromArgb(235, 235, 235)}
                        dialog.Controls.Add(lblPerusahaan)
                        dialog.Controls.Add(txtPerusahaan)
                        controls("nama_perusahaan") = txtPerusahaan
                        yPos += 35
                End Select
                
                ' Buttons
                yPos += 15
                Dim btnSave As New Button() With {
                    .Text = "Simpan",
                    .Location = New Point(150, yPos),
                    .Size = New Size(100, 35),
                    .BackColor = Color.FromArgb(156, 0, 219),
                    .ForeColor = Color.White,
                    .FlatStyle = FlatStyle.Flat,
                    .Font = New Font("Segoe UI Semibold", 9, FontStyle.Bold),
                    .DialogResult = DialogResult.OK
                }
                btnSave.FlatAppearance.BorderSize = 0
                
                Dim btnCancel As New Button() With {
                    .Text = "Batal",
                    .Location = New Point(260, yPos),
                    .Size = New Size(100, 35),
                    .BackColor = Color.FromArgb(120, 128, 146),
                    .ForeColor = Color.White,
                    .FlatStyle = FlatStyle.Flat,
                    .Font = New Font("Segoe UI Semibold", 9, FontStyle.Bold),
                    .DialogResult = DialogResult.Cancel
                }
                btnCancel.FlatAppearance.BorderSize = 0
                
                dialog.Controls.Add(btnSave)
                dialog.Controls.Add(btnCancel)
                dialog.AcceptButton = btnSave
                dialog.CancelButton = btnCancel
                
                If dialog.ShowDialog() = DialogResult.OK Then
                    ' Save changes
                    SaveUserChanges(userId, userRole, controls)
                End If
            End Using
            
        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub SaveUserChanges(userId As Integer, userRole As String, controls As Dictionary(Of String, Control))
        Try
            modulkoneksi.BukaKoneksi()
            
            ' Update users table (is_active)
            Dim isActive As Boolean = DirectCast(controls("is_active"), CheckBox).Checked
            Dim sqlUser As String = "UPDATE users SET is_active = @is_active WHERE id = @id"
            Dim cmdUser As New MySqlCommand(sqlUser, modulkoneksi.koneksi)
            cmdUser.Parameters.AddWithValue("@is_active", If(isActive, 1, 0))
            cmdUser.Parameters.AddWithValue("@id", userId)
            cmdUser.ExecuteNonQuery()
            
            ' Update role-specific table
            Dim sql As String = ""
            Select Case userRole
                Case "Admin"
                    sql = "UPDATE admin SET nama = @nama, email = @email, no_telepon = @no_telepon WHERE user_id = @id"
                Case "Wajib Pajak"
                    sql = "UPDATE wajib_pajak SET nama = @nama, email = @email, no_telepon = @no_telepon, 
                           npwp = @npwp, nik = @nik, alamat = @alamat, status_ptkp = @status_ptkp WHERE user_id = @id"
                Case "Pemberi Kerja"
                    sql = "UPDATE pemberi_kerja SET nama = @nama, email = @email, no_telepon = @no_telepon WHERE user_id = @id"
            End Select
            
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@id", userId)
            cmd.Parameters.AddWithValue("@nama", DirectCast(controls("nama"), TextBox).Text)
            cmd.Parameters.AddWithValue("@email", DirectCast(controls("email"), TextBox).Text)
            cmd.Parameters.AddWithValue("@no_telepon", DirectCast(controls("no_telepon"), TextBox).Text)
            
            If userRole = "Wajib Pajak" Then
                cmd.Parameters.AddWithValue("@npwp", DirectCast(controls("npwp"), TextBox).Text)
                cmd.Parameters.AddWithValue("@nik", DirectCast(controls("nik"), TextBox).Text)
                cmd.Parameters.AddWithValue("@alamat", DirectCast(controls("alamat"), TextBox).Text)
                cmd.Parameters.AddWithValue("@status_ptkp", DirectCast(controls("status_ptkp"), ComboBox).SelectedItem.ToString())
            End If
            
            cmd.ExecuteNonQuery()
            
            MsgBox("Data user berhasil diperbarui!", MsgBoxStyle.Information)
            LoadUsers()
            LoadStatistics()
            
        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub DeleteUser(userId As Integer, userRole As String)
        Try
            modulkoneksi.BukaKoneksi()
            
            ' Delete from role-specific table first (FK constraint)
            Select Case userRole
                Case "Admin"
                    Dim cmdAdmin As New MySqlCommand("DELETE FROM admin WHERE user_id = @id", modulkoneksi.koneksi)
                    cmdAdmin.Parameters.AddWithValue("@id", userId)
                    cmdAdmin.ExecuteNonQuery()
                Case "Wajib Pajak"
                    ' Delete pekerjaan first
                    Dim sqlGetWpId As String = "SELECT id FROM wajib_pajak WHERE user_id = @id"
                    Dim cmdGetWpId As New MySqlCommand(sqlGetWpId, modulkoneksi.koneksi)
                    cmdGetWpId.Parameters.AddWithValue("@id", userId)
                    Dim wpId = cmdGetWpId.ExecuteScalar()
                    
                    If wpId IsNot Nothing Then
                        Dim cmdPekerjaan As New MySqlCommand("DELETE FROM pekerjaan WHERE wajib_pajak_id = @wp_id", modulkoneksi.koneksi)
                        cmdPekerjaan.Parameters.AddWithValue("@wp_id", wpId)
                        cmdPekerjaan.ExecuteNonQuery()
                    End If
                    
                    Dim cmdWp As New MySqlCommand("DELETE FROM wajib_pajak WHERE user_id = @id", modulkoneksi.koneksi)
                    cmdWp.Parameters.AddWithValue("@id", userId)
                    cmdWp.ExecuteNonQuery()
                Case "Pemberi Kerja"
                    Dim cmdPk As New MySqlCommand("DELETE FROM pemberi_kerja WHERE user_id = @id", modulkoneksi.koneksi)
                    cmdPk.Parameters.AddWithValue("@id", userId)
                    cmdPk.ExecuteNonQuery()
            End Select
            
            ' Delete from users table
            Dim cmdUser As New MySqlCommand("DELETE FROM users WHERE id = @id", modulkoneksi.koneksi)
            cmdUser.Parameters.AddWithValue("@id", userId)
            cmdUser.ExecuteNonQuery()
            
            MsgBox("User berhasil dihapus!", MsgBoxStyle.Information)
            LoadUsers()
            LoadStatistics()
            
        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub UpdateUserStatus(userId As Integer, newStatus As Integer)
        Try
            modulkoneksi.BukaKoneksi()
            Dim sql As String = "UPDATE users SET is_active = @status WHERE id = @id"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@status", newStatus)
            cmd.Parameters.AddWithValue("@id", userId)
            cmd.ExecuteNonQuery()
            
            Dim statusText As String = If(newStatus = 1, "diaktifkan", "dinonaktifkan")
            MsgBox($"User berhasil {statusText}!", MsgBoxStyle.Information)
            
            LoadUsers() ' Reload data
            LoadStatistics() ' Update stats
            
        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

End Class


