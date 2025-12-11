Imports MySql.Data.MySqlClient

Public Class FrmLogin

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        ' 1. Validasi Input
        If String.IsNullOrWhiteSpace(txtNPWP.Text) Or String.IsNullOrWhiteSpace(txtPassword.Text) Then
            MsgBox("Username dan Password harus diisi!", MsgBoxStyle.Exclamation, "Peringatan")
            Return
        End If

        Dim usernameInput As String = txtNPWP.Text.Trim()
        Dim passwordInput As String = txtPassword.Text

        Try
            modulkoneksi.BukaKoneksi()

            ' 2. Ambil user berdasarkan username dari tabel users
            Dim sql As String = "
                SELECT id, username, password_hash, tipe_user, is_active 
                FROM users 
                WHERE username = @username 
                LIMIT 1
            "

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@username", usernameInput)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            If rd.Read() Then
                Dim dbUserId As Integer = Convert.ToInt32(rd("id"))
                Dim dbHash As String = rd("password_hash").ToString()
                Dim dbRole As String = rd("tipe_user").ToString()
                Dim dbIsActive As Boolean = Convert.ToBoolean(rd("is_active"))

                rd.Close()

                ' 3. Check jika user aktif
                If Not dbIsActive Then
                    MsgBox("Akun Anda tidak aktif. Silakan hubungi admin.", MsgBoxStyle.Critical, "Akun Nonaktif")
                    Return
                End If

                ' 4. Verifikasi Password
                If ModuleSecurity.VerifyPassword(passwordInput, dbHash) Then

                    ' Simpan session dasar
                    ModuleSession.CurrentUserId = dbUserId
                    ModuleSession.CurrentUserRole = dbRole

                    ' 5. Load profil berdasarkan tipe user
                    Select Case dbRole
                        Case "wajib_pajak"
                            If Not LoadWajibPajakProfile(dbUserId) Then
                                MsgBox("Profil wajib pajak tidak ditemukan!", MsgBoxStyle.Critical, "Error")
                                Return
                            End If

                            Dim f As New wp_dashboard()
                            f.Show()

                        Case "pemberi_kerja"
                            If Not LoadPemberiKerjaProfile(dbUserId) Then
                                MsgBox("Profil pemberi kerja tidak ditemukan!", MsgBoxStyle.Critical, "Error")
                                Return
                            End If

                            Dim f As New pk_dashboard()
                            f.Show()

                        Case "admin"
                            If Not LoadAdminProfile(dbUserId) Then
                                MsgBox("Profil admin tidak ditemukan!", MsgBoxStyle.Critical, "Error")
                                Return
                            End If

                            Dim f As New admin_dashboard()
                            f.Show()
                    End Select

                    MsgBox("Login Berhasil! Selamat datang, " & ModuleSession.CurrentUserName, MsgBoxStyle.Information)
                    Me.Hide()

                Else
                    Dim computedHash As String = ModuleSecurity.HashPassword(passwordInput)
                    MsgBox("Password salah!" & vbCrLf & vbCrLf & "Debug info:" & vbCrLf & "Input hash: " & computedHash & vbCrLf & "DB hash: " & dbHash, MsgBoxStyle.Critical, "Login Gagal")
                End If

            Else
                rd.Close()
                MsgBox("Username tidak terdaftar!", MsgBoxStyle.Critical, "Login Gagal")
            End If

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)

        Finally
            modulkoneksi.TutupKoneksi()
        End Try

    End Sub


    ' ================================
    ' LOAD WAJIB PAJAK PROFILE
    ' ================================
    Private Function LoadWajibPajakProfile(userId As Integer) As Boolean
        Try
            Dim sql As String = "
                SELECT id, npwp, nama, status_validasi 
                FROM wajib_pajak 
                WHERE user_id = @user_id
            "

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@user_id", userId)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            If rd.Read() Then
                Dim statusValidasi As String = rd("status_validasi").ToString()

                ' Check status validasi
                If statusValidasi = "pending" Then
                    rd.Close()
                    MsgBox("Akun Anda sedang dalam proses verifikasi oleh admin." & vbCrLf & vbCrLf & "Silakan coba login lagi nanti.", MsgBoxStyle.Information, "Menunggu Verifikasi")
                    Return False
                ElseIf statusValidasi = "rejected" Then
                    rd.Close()
                    MsgBox("Maaf, registrasi Anda telah ditolak oleh admin." & vbCrLf & vbCrLf & "Silakan hubungi admin untuk informasi lebih lanjut.", MsgBoxStyle.Critical, "Registrasi Ditolak")
                    Return False
                End If

                ModuleSession.CurrentWajibPajakId = Convert.ToInt32(rd("id"))
                ModuleSession.CurrentWajibPajakNPWP = rd("npwp").ToString()
                ModuleSession.CurrentUserName = rd("nama").ToString()
                ModuleSession.CurrentUserNPWP = rd("npwp").ToString() ' Legacy compatibility

                rd.Close()
                Return True
            End If

            rd.Close()
            Return False

        Catch ex As Exception
            Return False
        End Try
    End Function


    ' ================================
    ' LOAD PEMBERI KERJA PROFILE
    ' ================================
    Private Function LoadPemberiKerjaProfile(userId As Integer) As Boolean
        Try
            Dim sql As String = "
                SELECT pk.id, pk.nama, pk.perusahaan_id, p.nama_perusahaan 
                FROM pemberi_kerja pk
                JOIN perusahaan p ON p.id = pk.perusahaan_id
                WHERE pk.user_id = @user_id
            "

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@user_id", userId)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            If rd.Read() Then
                ModuleSession.CurrentPemberiKerjaId = Convert.ToInt32(rd("id"))
                ModuleSession.CurrentUserName = rd("nama").ToString()
                ModuleSession.CurrentPerusahaanId = Convert.ToInt32(rd("perusahaan_id"))
                ModuleSession.CurrentPerusahaanName = rd("nama_perusahaan").ToString()

                rd.Close()
                Return True
            End If

            rd.Close()
            Return False

        Catch ex As Exception
            Return False
        End Try
    End Function


    ' ================================
    ' LOAD ADMIN PROFILE
    ' ================================
    Private Function LoadAdminProfile(userId As Integer) As Boolean
        Try
            Dim sql As String = "
                SELECT id, nama 
                FROM admin 
                WHERE user_id = @user_id
            "

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@user_id", userId)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            If rd.Read() Then
                ModuleSession.CurrentAdminId = Convert.ToInt32(rd("id"))
                ModuleSession.CurrentUserName = rd("nama").ToString()

                rd.Close()
                Return True
            End If

            rd.Close()
            Return False

        Catch ex As Exception
            Return False
        End Try
    End Function


    ' Tombol Register
    Private Sub btnGoRegister_Click(sender As Object, e As EventArgs) Handles btnGoRegister.Click
        Dim f As New FrmRegister()
        f.Show()
        Me.Hide()
    End Sub


    ' Reset textbox saat form dibuka
    Private Sub FrmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtNPWP.Clear()
        txtPassword.Clear()
    End Sub

    Private Sub txtNPWP_TextChanged(sender As Object, e As EventArgs) Handles txtNPWP.TextChanged

    End Sub

    Private Sub pnlMain_Paint(sender As Object, e As PaintEventArgs) Handles pnlMain.Paint

    End Sub
End Class
