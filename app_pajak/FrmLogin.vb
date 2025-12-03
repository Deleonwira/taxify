Imports MySql.Data.MySqlClient

Public Class FrmLogin

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        ' 1. Validasi Input
        If String.IsNullOrWhiteSpace(Guna2TextBox1.Text) Or String.IsNullOrWhiteSpace(Guna2TextBox2.Text) Then
            MsgBox("NPWP dan Password harus diisi!", MsgBoxStyle.Exclamation, "Peringatan")
            Return
        End If

        Dim npwpInput As String = ModuleSecurity.CleanNPWP(Guna2TextBox1.Text)
        Dim passwordInput As String = Guna2TextBox2.Text

        Try
            modulkoneksi.BukaKoneksi()

            ' 2. Ambil user berdasarkan NPWP
            Dim sql As String = "
                SELECT npwp, password_hash, nama, tipe_user 
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

                rd.Close()

                ' 3. Verifikasi Password
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
    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Dim f As New FrmRegister()
        f.Show()
        Me.Hide()
    End Sub


    ' Reset textbox saat form dibuka
    Private Sub FrmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Guna2TextBox1.Clear()
        Guna2TextBox2.Clear()
    End Sub

End Class
