Imports MySql.Data.MySqlClient

Public Class wp_data_diri

    ' ====== EVENT HANDLERS NAVBAR ======
    Private Sub wp_data_diri_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Setup Navbar
        AddHandler Wp_navbar1.DashboardClicked, AddressOf OnDashboardClicked
        AddHandler Wp_navbar1.LaporClicked, AddressOf OnLaporClicked
        AddHandler Wp_navbar1.BuktiPotongClicked, AddressOf OnBuktiPotongClicked
        AddHandler Wp_navbar1.RiwayatClicked, AddressOf OnRiwayatClicked
        AddHandler Wp_navbar1.DataDiriClicked, AddressOf OnDataDiriClicked
        AddHandler Wp_navbar1.LogoutClicked, AddressOf OnLogoutClicked

        ' Tandai menu Data Diri sebagai aktif
        Wp_navbar1.SetActiveMenu(wp_navbar.MenuType.DataDiri)

        ' Load Data User
        LoadUserData()
    End Sub

    Private Sub LoadUserData()
        Try
            modulkoneksi.BukaKoneksi()

            ' Join users and profil_wp to get all data
            Dim sql As String = "
                SELECT u.nama, u.npwp, u.email, p.no_telepon, p.alamat 
                FROM users u 
                LEFT JOIN profil_wp p ON u.id = p.user_id 
                WHERE u.id = @uid
            "

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@uid", ModuleSession.CurrentUserId)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            If rd.Read() Then
                txtNama.Text = rd("nama").ToString()
                txtNIK.Text = rd("npwp").ToString() ' Label is NPWP
                Guna2TextBox1.Text = rd("email").ToString() ' Label is Email

                ' Handle nulls for profile data
                If Not IsDBNull(rd("no_telepon")) Then
                    txtEmail.Text = rd("no_telepon").ToString() ' Label is Nomor Telpon
                Else
                    txtEmail.Text = ""
                End If

                If Not IsDBNull(rd("alamat")) Then
                    txtAlamat.Text = rd("alamat").ToString()
                Else
                    txtAlamat.Text = ""
                End If
            End If

            rd.Close()

        Catch ex As Exception
            MsgBox("Gagal memuat data: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click
        ' 1. Validasi Password Change (Optional)
        Dim changePassword As Boolean = False
        Dim newPasswordHash As String = ""

        If Not String.IsNullOrEmpty(Guna2TextBox3.Text) Or Not String.IsNullOrEmpty(Guna2TextBox2.Text) Then
            If String.IsNullOrEmpty(Guna2TextBox3.Text) Or String.IsNullOrEmpty(Guna2TextBox2.Text) Then
                MsgBox("Untuk mengganti password, isi kedua kolom password (lama dan baru)!", MsgBoxStyle.Exclamation)
                Return
            End If

            ' Verify old password
            If Not VerifyOldPassword(Guna2TextBox3.Text) Then
                MsgBox("Password lama salah!", MsgBoxStyle.Critical)
                Return
            End If

            changePassword = True
            newPasswordHash = ModuleSecurity.HashPassword(Guna2TextBox2.Text)
        End If

        ' 2. Update Data
        Try
            modulkoneksi.BukaKoneksi()

            ' Update Users Table (Nama, Email, Password)
            Dim sqlUser As String = "UPDATE users SET nama = @nama, email = @email"
            If changePassword Then
                sqlUser &= ", password_hash = @pass"
            End If
            sqlUser &= " WHERE id = @uid"

            Dim cmdUser As New MySqlCommand(sqlUser, modulkoneksi.koneksi)
            cmdUser.Parameters.AddWithValue("@nama", txtNama.Text)
            cmdUser.Parameters.AddWithValue("@email", Guna2TextBox1.Text)
            cmdUser.Parameters.AddWithValue("@uid", ModuleSession.CurrentUserId)
            If changePassword Then
                cmdUser.Parameters.AddWithValue("@pass", newPasswordHash)
            End If

            cmdUser.ExecuteNonQuery()

            ' Update/Insert Profil WP Table
            ' Check if profile exists first
            Dim checkSql As String = "SELECT COUNT(*) FROM profil_wp WHERE user_id = @uid"
            Dim cmdCheck As New MySqlCommand(checkSql, modulkoneksi.koneksi)
            cmdCheck.Parameters.AddWithValue("@uid", ModuleSession.CurrentUserId)
            Dim count As Integer = Convert.ToInt32(cmdCheck.ExecuteScalar())

            Dim sqlProfil As String
            If count > 0 Then
                sqlProfil = "UPDATE profil_wp SET no_telepon = @telp, alamat = @alamat WHERE user_id = @uid"
            Else
                sqlProfil = "INSERT INTO profil_wp (user_id, no_telepon, alamat) VALUES (@uid, @telp, @alamat)"
            End If

            Dim cmdProfil As New MySqlCommand(sqlProfil, modulkoneksi.koneksi)
            cmdProfil.Parameters.AddWithValue("@uid", ModuleSession.CurrentUserId)
            cmdProfil.Parameters.AddWithValue("@telp", txtEmail.Text)
            cmdProfil.Parameters.AddWithValue("@alamat", txtAlamat.Text)

            cmdProfil.ExecuteNonQuery()

            MsgBox("Data berhasil disimpan!", MsgBoxStyle.Information)

            ' Clear password fields
            Guna2TextBox2.Clear()
            Guna2TextBox3.Clear()

            ' Update Session Name if changed
            ModuleSession.CurrentUserName = txtNama.Text

        Catch ex As Exception
            MsgBox("Gagal menyimpan data: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Function VerifyOldPassword(oldPass As String) As Boolean
        Try
            modulkoneksi.BukaKoneksi()
            Dim sql As String = "SELECT password_hash FROM users WHERE id = @uid"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@uid", ModuleSession.CurrentUserId)

            Dim dbHash As String = cmd.ExecuteScalar().ToString()

            Return ModuleSecurity.VerifyPassword(oldPass, dbHash)
        Catch ex As Exception
            Return False
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Function

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        ' Discard changes - reload data
        LoadUserData()
        Guna2TextBox2.Clear()
        Guna2TextBox3.Clear()
    End Sub

    ' ====== NAVIGATION HANDLERS ======
    Private Sub OnDashboardClicked(sender As Object, e As EventArgs)
        Dim f As New wp_dashboard()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnLaporClicked(sender As Object, e As EventArgs)
        Dim f As New wp_lapor_pajak()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnBuktiPotongClicked(sender As Object, e As EventArgs)
        Dim f As New wp_riwayat_bukti_potong()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnRiwayatClicked(sender As Object, e As EventArgs)
        Dim f As New wp_riwayat_lapor_pajak()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnDataDiriClicked(sender As Object, e As EventArgs)
        ' Sudah di halaman data diri
    End Sub

    Private Sub OnLogoutClicked(sender As Object, e As EventArgs)
        ModuleSession.ClearSession()
        Dim f As New FrmLogin()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Guna2Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Guna2Panel1.Paint

    End Sub
End Class