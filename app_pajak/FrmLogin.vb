Imports MySql.Data.MySqlClient

Public Class FrmLogin

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        ' 1. Validasi Input
        If String.IsNullOrWhiteSpace(Guna2TextBox1.Text) Or String.IsNullOrWhiteSpace(Guna2TextBox2.Text) Then
            MsgBox("NPWP dan Password harus diisi!", MsgBoxStyle.Exclamation, "Peringatan")
            Return
        End If

        Dim npwpRaw As String = Guna2TextBox1.Text
        Dim passwordRaw As String = Guna2TextBox2.Text

        ' Bersihkan NPWP
        Dim npwpClean As String = ModuleSecurity.CleanNPWP(npwpRaw)

        Try
            modulkoneksi.BukaKoneksi()

            ' 2. Ambil user berdasarkan NPWP
            Dim sql As String = "SELECT id, nama, password_hash, tipe_user FROM users WHERE npwp = @npwp LIMIT 1"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", npwpClean)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            If rd.Read() Then
                Dim dbId As Integer = Convert.ToInt32(rd("id"))
                Dim dbNama As String = rd("nama").ToString()
                Dim dbHash As String = rd("password_hash").ToString()
                Dim dbTipe As String = rd("tipe_user").ToString()

                rd.Close()

                ' 3. Verifikasi Password
                If ModuleSecurity.VerifyPassword(passwordRaw, dbHash) Then
                    ' Login Berhasil -> Simpan Session
                    ModuleSession.CurrentUserId = dbId
                    ModuleSession.CurrentUserName = dbNama
                    ModuleSession.CurrentUserNPWP = npwpClean
                    ModuleSession.CurrentUserRole = dbTipe

                    ' Jika Pemberi Kerja, ambil data perusahaan
                    If dbTipe = "pemberi_kerja" Then
                        GetPerusahaanData(dbId)
                    End If

                    MsgBox("Login Berhasil! Selamat datang, " & dbNama, MsgBoxStyle.Information)

                    ' 4. Redirect
                    Select Case dbTipe
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

    Private Sub GetPerusahaanData(userId As Integer)
        Try
            Dim sql As String = "SELECT id, nama_perusahaan FROM perusahaan WHERE user_id = @uid LIMIT 1"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@uid", userId)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()
            If rd.Read() Then
                ModuleSession.CurrentPerusahaanId = Convert.ToInt32(rd("id"))
                ModuleSession.CurrentPerusahaanName = rd("nama_perusahaan").ToString()
            End If
            rd.Close()
        Catch ex As Exception
            ' Ignore error fetching company data, not critical for login
        End Try
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Dim f As New FrmRegister()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub FrmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Guna2TextBox1.Clear()
        Guna2TextBox2.Clear()
    End Sub
End Class
