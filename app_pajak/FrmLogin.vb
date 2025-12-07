Imports MySql.Data.MySqlClient

Public Class FrmLogin

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        ' 1. Validasi Input
        If String.IsNullOrWhiteSpace(txtNPWP.Text) Or String.IsNullOrWhiteSpace(txtPassword.Text) Then
            MsgBox("NPWP dan Password harus diisi!", MsgBoxStyle.Exclamation, "Peringatan")
            Return
        End If

        Dim npwpInput As String = ModuleSecurity.CleanNPWP(txtNPWP.Text)
        Dim passwordInput As String = txtPassword.Text

        Try
            modulkoneksi.BukaKoneksi()

            ' 2. Ambil user berdasarkan NPWP
            Dim sql As String = "
                SELECT npwp, password_hash, nama, tipe_user, status_validasi 
                FROM users 
                WHERE npwp = @npwp 
                LIMIT 1
            "

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", npwpInput)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            If rd.Read() Then

                Dim dbNPWP As String = rd("npwp").ToString()
                Dim dbHash As String = rd("password_hash").ToString()
                Dim dbNama As String = rd("nama").ToString()
                Dim dbRole As String = rd("tipe_user").ToString()
                Dim dbStatus As String = If(rd("status_validasi") IsNot DBNull.Value, rd("status_validasi").ToString(), "approved")

                rd.Close()

                ' 3. Check status validasi untuk wajib_pajak
                If dbRole = "wajib_pajak" Then
                    If dbStatus = "pending" Then
                        MsgBox("Akun Anda sedang dalam proses verifikasi oleh admin." & vbCrLf & vbCrLf & "Silakan coba login lagi nanti.", MsgBoxStyle.Information, "Menunggu Verifikasi")
                        Return
                    ElseIf dbStatus = "rejected" Then
                        MsgBox("Maaf, registrasi Anda telah ditolak oleh admin." & vbCrLf & vbCrLf & "Silakan hubungi admin untuk informasi lebih lanjut.", MsgBoxStyle.Critical, "Registrasi Ditolak")
                        Return
                    End If
                End If

                ' 4. Verifikasi Password
                If ModuleSecurity.VerifyPassword(passwordInput, dbHash) Then

                    ' Simpan session pengguna
                    ModuleSession.CurrentUserNPWP = dbNPWP
                    ModuleSession.CurrentUserName = dbNama
                    ModuleSession.CurrentUserRole = dbRole

                    ' Jika Pemberi Kerja → Ambil perusahaan berdasarkan owner_npwp
                    If dbRole = "pemberi_kerja" Then
                        GetPerusahaanData(dbNPWP)
                    End If

                    MsgBox("Login Berhasil! Selamat datang, " & dbNama, MsgBoxStyle.Information)

                    ' 4. Redirect sesuai role
                    Select Case dbRole
                        Case "wajib_pajak"
                            Dim f As New wp_dashboard()
                            f.Show()

                        Case "pemberi_kerja"
                            Dim f As New pk_dashboard()
                            f.Show()

                        Case "admin"
                            Dim f As New admin_dashboard()
                            f.Show()
                    End Select

                    Me.Hide()

                Else
                    MsgBox("Password salah!", MsgBoxStyle.Critical, "Login Gagal")
                End If

            Else
                rd.Close()
                MsgBox("NPWP tidak terdaftar!", MsgBoxStyle.Critical, "Login Gagal")
            End If

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)

        Finally
            modulkoneksi.TutupKoneksi()
        End Try

    End Sub


    ' ================================
    ' AMBIL PERUSAHAAN BERDASARKAN OWNER NPWP
    ' ================================
    Private Sub GetPerusahaanData(ownerNPWP As String)
        Try
            Dim sql As String = "
                SELECT id, nama_perusahaan 
                FROM perusahaan 
                WHERE owner_npwp = @npwp 
                LIMIT 1
            "

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", ownerNPWP)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            If rd.Read() Then
                ModuleSession.CurrentPerusahaanId = Convert.ToInt32(rd("id"))
                ModuleSession.CurrentPerusahaanName = rd("nama_perusahaan").ToString()
            End If

            rd.Close()

        Catch ex As Exception
            ' ignore error, tidak fatal
        End Try
    End Sub


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

End Class
