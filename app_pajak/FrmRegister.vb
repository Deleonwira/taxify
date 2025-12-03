Imports MySql.Data.MySqlClient

Public Class FrmRegister

    Private Sub FrmRegister_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Clear all fields on load
        txtNama.Clear()
        txtEmail.Clear()
        txtPassword.Clear()
        txtConfirmPassword.Clear()
    End Sub

    Private Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        ' 1. Validasi Input
        If String.IsNullOrWhiteSpace(txtNama.Text) Then
            MsgBox("NPWP harus diisi!", MsgBoxStyle.Exclamation, "Validasi")
            txtNama.Focus()
            Return
        End If

        If String.IsNullOrWhiteSpace(txtEmail.Text) Then
            MsgBox("Email harus diisi!", MsgBoxStyle.Exclamation, "Validasi")
            txtEmail.Focus()
            Return
        End If

        If String.IsNullOrWhiteSpace(txtPassword.Text) Then
            MsgBox("Password harus diisi!", MsgBoxStyle.Exclamation, "Validasi")
            txtPassword.Focus()
            Return
        End If

        If txtPassword.Text <> txtConfirmPassword.Text Then
            MsgBox("Password dan konfirmasi password tidak cocok!", MsgBoxStyle.Exclamation, "Validasi")
            txtConfirmPassword.Focus()
            Return
        End If

        ' 2. Clean NPWP
        Dim npwpClean As String = ModuleSecurity.CleanNPWP(txtNama.Text)
        
        ' Validasi format NPWP (15 digit)
        If npwpClean.Length <> 15 Then
            MsgBox("NPWP harus 15 digit!", MsgBoxStyle.Exclamation, "Validasi")
            txtNama.Focus()
            Return
        End If

        ' 3. Hash Password
        Dim passwordHash As String = ModuleSecurity.HashPassword(txtPassword.Text)

        ' 4. Insert ke Database
        Try
            modulkoneksi.BukaKoneksi()

            ' Check if NPWP already exists
            Dim checkSql As String = "SELECT COUNT(*) FROM users WHERE npwp = @npwp"
            Dim cmdCheck As New MySqlCommand(checkSql, modulkoneksi.koneksi)
            cmdCheck.Parameters.AddWithValue("@npwp", npwpClean)
            Dim count As Integer = Convert.ToInt32(cmdCheck.ExecuteScalar())

            If count > 0 Then
                MsgBox("NPWP sudah terdaftar! Silakan login.", MsgBoxStyle.Exclamation, "NPWP Sudah Ada")
                Return
            End If

            ' Insert new user (default as wajib_pajak, admin can change later)
            Dim sql As String = "INSERT INTO users (npwp, password_hash, nama, email, tipe_user, status_aktif) 
                                VALUES (@npwp, @pass, @nama, @email, 'wajib_pajak', 'active')"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", npwpClean)
            cmd.Parameters.AddWithValue("@pass", passwordHash)
            cmd.Parameters.AddWithValue("@nama", "User " & npwpClean) ' Temporary name, will update in profile
            cmd.Parameters.AddWithValue("@email", txtEmail.Text)

            cmd.ExecuteNonQuery()

            MsgBox("Registrasi berhasil! Silakan login.", MsgBoxStyle.Information, "Sukses")
            
            ' Go to login
            Dim f As New FrmLogin()
            f.Show()
            Me.Close()

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical, "Error Registrasi")
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub btnGoLogin_Click(sender As Object, e As EventArgs) Handles btnGoLogin.Click
        Dim f As New FrmLogin()
        f.Show()
        Me.Close()
    End Sub

End Class