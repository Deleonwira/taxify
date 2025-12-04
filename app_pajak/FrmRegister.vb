Imports MySql.Data.MySqlClient

Public Class FrmRegister

    Private Sub FrmRegister_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Clear all fields on load
        txtNPWP.Clear()
        txtNamaLengkap.Clear()
        txtEmail.Clear()
        txtNIK.Clear()
        txtNoTelepon.Clear()
        txtAlamat.Clear()
        txtPassword.Clear()
        txtConfirmPassword.Clear()
        cmbTipeUser.SelectedIndex = 0 ' Default to Wajib Pajak
    End Sub

    Private Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        ' 1. Validasi Input
        If String.IsNullOrWhiteSpace(txtNPWP.Text) Then
            MsgBox("NPWP harus diisi!", MsgBoxStyle.Exclamation, "Validasi")
            txtNPWP.Focus()
            Return
        End If

        If String.IsNullOrWhiteSpace(txtNamaLengkap.Text) Then
            MsgBox("Nama Lengkap harus diisi!", MsgBoxStyle.Exclamation, "Validasi")
            txtNamaLengkap.Focus()
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
        Dim npwpClean As String = ModuleSecurity.CleanNPWP(txtNPWP.Text)
        
        ' Validasi format NPWP (15 digit)
        If npwpClean.Length <> 15 Then
            MsgBox("NPWP harus 15 digit!", MsgBoxStyle.Exclamation, "Validasi")
            txtNPWP.Focus()
            Return
        End If

        ' Validasi NIK (optional, tapi jika diisi harus 16 digit)
        If Not String.IsNullOrWhiteSpace(txtNIK.Text) Then
            Dim nikClean As String = txtNIK.Text.Trim().Replace("-", "").Replace(".", "").Replace(" ", "")
            If nikClean.Length <> 16 OrElse Not IsNumeric(nikClean) Then
                MsgBox("NIK harus 16 digit angka!", MsgBoxStyle.Exclamation, "Validasi")
                txtNIK.Focus()
                Return
            End If
        End If

        ' 3. Hash Password
        Dim passwordHash As String = ModuleSecurity.HashPassword(txtPassword.Text)

        ' 4. Map tipe user from ComboBox
        Dim tipeUser As String = "wajib_pajak" ' default
        Select Case cmbTipeUser.SelectedIndex
            Case 0 ' Wajib Pajak
                tipeUser = "wajib_pajak"
            Case 1 ' Pemberi Kerja
                tipeUser = "pemberi_kerja"
            Case 2 ' Admin
                tipeUser = "admin"
        End Select

        ' 5. Insert ke Database
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

            ' Insert new user with all fields
            Dim sql As String = "INSERT INTO users (npwp, password_hash, nama, email, tipe_user, no_telepon, alamat, nik) 
                                VALUES (@npwp, @pass, @nama, @email, @tipe, @telepon, @alamat, @nik)"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", npwpClean)
            cmd.Parameters.AddWithValue("@pass", passwordHash)
            cmd.Parameters.AddWithValue("@nama", txtNamaLengkap.Text.Trim())
            cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim())
            cmd.Parameters.AddWithValue("@tipe", tipeUser)
            
            ' Handle optional fields
            If String.IsNullOrWhiteSpace(txtNoTelepon.Text) Then
                cmd.Parameters.AddWithValue("@telepon", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@telepon", txtNoTelepon.Text.Trim())
            End If

            If String.IsNullOrWhiteSpace(txtAlamat.Text) Then
                cmd.Parameters.AddWithValue("@alamat", DBNull.Value)
            Else
                cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text.Trim())
            End If

            If String.IsNullOrWhiteSpace(txtNIK.Text) Then
                cmd.Parameters.AddWithValue("@nik", DBNull.Value)
            Else
                Dim nikClean As String = txtNIK.Text.Trim().Replace("-", "").Replace(".", "").Replace(" ", "")
                cmd.Parameters.AddWithValue("@nik", nikClean)
            End If

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