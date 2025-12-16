Imports MySql.Data.MySqlClient

Public Class FrmRegister

    Private Sub FrmRegister_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Clear all fields on load
        txtUsername.Clear()
        txtNPWP.Clear()
        txtNamaLengkap.Clear()
        txtEmail.Clear()
        txtNIK.Clear()
        txtNoTelepon.Clear()
        txtAlamat.Clear()
        txtPassword.Clear()
        txtConfirmPassword.Clear()
        txtJabatan.Clear()

        ' Load daftar perusahaan dari database
        LoadPerusahaanList()

        ' Set default selections
        cmbStatusPTKP.SelectedIndex = 0 ' Default: TK0
    End Sub

    Private Sub LoadPerusahaanList()
        Try
            modulkoneksi.BukaKoneksi()

            ' Load all perusahaan from database
            Dim sql As String = "SELECT id, nama_perusahaan FROM perusahaan ORDER BY nama_perusahaan"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            cmbPerusahaan.Items.Clear()

            While rd.Read()
                Dim id As Integer = Convert.ToInt32(rd("id"))
                Dim nama As String = rd("nama_perusahaan").ToString()
                cmbPerusahaan.Items.Add(New KeyValuePair(Of Integer, String)(id, nama))
            End While

            rd.Close()

            If cmbPerusahaan.Items.Count > 0 Then
                cmbPerusahaan.SelectedIndex = 0
            End If

        Catch ex As Exception
            ' Silent fail
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        ' 1. Validasi Input User
        If String.IsNullOrWhiteSpace(txtUsername.Text) Then
            MsgBox("Username harus diisi!", MsgBoxStyle.Exclamation, "Validasi")
            txtUsername.Focus()
            Return
        End If

        ' Validasi username (hanya huruf, angka, underscore)
        Dim usernameClean As String = txtUsername.Text.Trim().ToLower()
        If usernameClean.Length < 4 Then
            MsgBox("Username minimal 4 karakter!", MsgBoxStyle.Exclamation, "Validasi")
            txtUsername.Focus()
            Return
        End If

        If Not System.Text.RegularExpressions.Regex.IsMatch(usernameClean, "^[a-z0-9_]+$") Then
            MsgBox("Username hanya boleh mengandung huruf, angka, dan underscore!", MsgBoxStyle.Exclamation, "Validasi")
            txtUsername.Focus()
            Return
        End If

        ' Validasi NPWP (Jika tidak dicentang)
        If Not chkBelumPunyaNPWP.Checked Then
            If String.IsNullOrWhiteSpace(txtNPWP.Text) Then
                MsgBox("NPWP harus diisi!", MsgBoxStyle.Exclamation, "Validasi")
                txtNPWP.Focus()
                Return
            End If
            
            Dim npwpRaw As String = ModuleSecurity.CleanNPWP(txtNPWP.Text)
            If npwpRaw.Length <> 15 Then
                MsgBox("NPWP harus 15 digit!", MsgBoxStyle.Exclamation, "Validasi")
                txtNPWP.Focus()
                Return
            End If
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

        If String.IsNullOrWhiteSpace(txtNIK.Text) Then
            MsgBox("NIK harus diisi!", MsgBoxStyle.Exclamation, "Validasi")
            txtNIK.Focus()
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
        Dim npwpClean As String = ""
        If Not chkBelumPunyaNPWP.Checked Then
            npwpClean = ModuleSecurity.CleanNPWP(txtNPWP.Text)
        End If

        ' Validasi NIK (16 digit)
        Dim nikClean As String = txtNIK.Text.Trim().Replace("-", "").Replace(".", "").Replace(" ", "")
        If nikClean.Length <> 16 OrElse Not IsNumeric(nikClean) Then
            MsgBox("NIK harus 16 digit angka!", MsgBoxStyle.Exclamation, "Validasi")
            txtNIK.Focus()
            Return
        End If

        ' 3. Validasi pekerjaan - perusahaan harus dipilih
        If cmbPerusahaan.SelectedItem Is Nothing Then
            MsgBox("Silakan pilih perusahaan!", MsgBoxStyle.Exclamation, "Validasi")
            cmbPerusahaan.Focus()
            Return
        End If

        ' 4. Hash Password
        Dim passwordHash As String = ModuleSecurity.HashPassword(txtPassword.Text)

        ' 5. Get pekerjaan values
        Dim selectedPerusahaan = CType(cmbPerusahaan.SelectedItem, KeyValuePair(Of Integer, String))
        Dim perusahaanId As Integer = selectedPerusahaan.Key
        Dim jabatan As String = txtJabatan.Text.Trim()
        Dim statusPTKP As String = cmbStatusPTKP.SelectedItem.ToString()

        ' 6. Use user-input username
        Dim username As String = usernameClean

        ' 7. Insert ke Database (Transaction)
        Try
            modulkoneksi.BukaKoneksi()

            ' Check if username already exists
            Dim checkUserSql As String = "SELECT COUNT(*) FROM users WHERE username = @username"
            Dim cmdCheckUser As New MySqlCommand(checkUserSql, modulkoneksi.koneksi)
            cmdCheckUser.Parameters.AddWithValue("@username", username)
            Dim countUser As Integer = Convert.ToInt32(cmdCheckUser.ExecuteScalar())

            If countUser > 0 Then
                MsgBox("Username sudah digunakan! Silakan pilih username lain.", MsgBoxStyle.Exclamation, "Username Sudah Ada")
                Return
            End If

            ' Check if NPWP already exists in wajib_pajak (If manually entered)
            If Not String.IsNullOrEmpty(npwpClean) Then
                Dim checkNpwpSql As String = "SELECT COUNT(*) FROM wajib_pajak WHERE npwp = @npwp"
                Dim cmdCheckNpwp As New MySqlCommand(checkNpwpSql, modulkoneksi.koneksi)
                cmdCheckNpwp.Parameters.AddWithValue("@npwp", npwpClean)
                Dim countNpwp As Integer = Convert.ToInt32(cmdCheckNpwp.ExecuteScalar())

                If countNpwp > 0 Then
                    MsgBox("NPWP sudah terdaftar! Silakan login.", MsgBoxStyle.Exclamation, "NPWP Sudah Ada")
                    Return
                End If
            End If

            ' Check if NIK already exists
            Dim checkNikSql As String = "SELECT COUNT(*) FROM wajib_pajak WHERE nik = @nik"
            Dim cmdCheckNik As New MySqlCommand(checkNikSql, modulkoneksi.koneksi)
            cmdCheckNik.Parameters.AddWithValue("@nik", nikClean)
            Dim countNik As Integer = Convert.ToInt32(cmdCheckNik.ExecuteScalar())

            If countNik > 0 Then
                MsgBox("NIK sudah terdaftar!", MsgBoxStyle.Exclamation, "NIK Sudah Ada")
                Return
            End If

            ' Generate Random NPWP if Checkbox checked
            If chkBelumPunyaNPWP.Checked Then
                Dim isUnique As Boolean = False
                Dim rnd As New Random()
                
                While Not isUnique
                    npwpClean = ""
                    For i As Integer = 1 To 15
                        npwpClean &= rnd.Next(0, 10).ToString()
                    Next
                    
                    ' Check DB for Uniqueness
                    Dim checkUniqueSql As String = "SELECT COUNT(*) FROM wajib_pajak WHERE npwp = @npwp"
                    Dim cmdUnique As New MySqlCommand(checkUniqueSql, modulkoneksi.koneksi)
                    cmdUnique.Parameters.AddWithValue("@npwp", npwpClean)
                    Dim count As Integer = Convert.ToInt32(cmdUnique.ExecuteScalar())
                    
                    If count = 0 Then isUnique = True
                End While
            End If

            ' === INSERT INTO users ===
            Dim sqlUser As String = "INSERT INTO users (username, password_hash, tipe_user, is_active) VALUES (@username, @pass, 'wajib_pajak', 1)"
            Dim cmdUser As New MySqlCommand(sqlUser, modulkoneksi.koneksi)
            cmdUser.Parameters.AddWithValue("@username", username)
            cmdUser.Parameters.AddWithValue("@pass", passwordHash)
            cmdUser.ExecuteNonQuery()

            ' Get the inserted user ID
            Dim userId As Integer = Convert.ToInt32(cmdUser.LastInsertedId)

            ' === INSERT INTO wajib_pajak ===
            ' Status Validasi = 'approved' (Auto Approve)
            Dim sqlWP As String = "INSERT INTO wajib_pajak (user_id, npwp, nik, nama, email, no_telepon, alamat, status_ptkp, status_validasi) VALUES (@user_id, @npwp, @nik, @nama, @email, @telepon, @alamat, @status_ptkp, 'approved')"
            Dim cmdWP As New MySqlCommand(sqlWP, modulkoneksi.koneksi)
            cmdWP.Parameters.AddWithValue("@user_id", userId)
            cmdWP.Parameters.AddWithValue("@npwp", npwpClean)
            cmdWP.Parameters.AddWithValue("@nik", nikClean)
            cmdWP.Parameters.AddWithValue("@nama", txtNamaLengkap.Text.Trim())
            cmdWP.Parameters.AddWithValue("@email", txtEmail.Text.Trim())

            If String.IsNullOrWhiteSpace(txtNoTelepon.Text) Then
                cmdWP.Parameters.AddWithValue("@telepon", DBNull.Value)
            Else
                cmdWP.Parameters.AddWithValue("@telepon", txtNoTelepon.Text.Trim())
            End If

            If String.IsNullOrWhiteSpace(txtAlamat.Text) Then
                cmdWP.Parameters.AddWithValue("@alamat", DBNull.Value)
            Else
                cmdWP.Parameters.AddWithValue("@alamat", txtAlamat.Text.Trim())
            End If

            cmdWP.Parameters.AddWithValue("@status_ptkp", statusPTKP)
            cmdWP.ExecuteNonQuery()

            ' Get the inserted wajib_pajak ID
            Dim wpId As Integer = Convert.ToInt32(cmdWP.LastInsertedId)

            ' === INSERT INTO pekerjaan ===
            Dim sqlPekerjaan As String = "INSERT INTO pekerjaan (wajib_pajak_id, perusahaan_id, jabatan) VALUES (@wp_id, @perusahaan_id, @jabatan)"
            Dim cmdPekerjaan As New MySqlCommand(sqlPekerjaan, modulkoneksi.koneksi)
            cmdPekerjaan.Parameters.AddWithValue("@wp_id", wpId)
            cmdPekerjaan.Parameters.AddWithValue("@perusahaan_id", perusahaanId)

            If String.IsNullOrWhiteSpace(jabatan) Then
                cmdPekerjaan.Parameters.AddWithValue("@jabatan", DBNull.Value)
            Else
                cmdPekerjaan.Parameters.AddWithValue("@jabatan", jabatan)
            End If

            cmdPekerjaan.ExecuteNonQuery()

            Dim msg As String = "Registrasi berhasil!" & vbCrLf & vbCrLf & "Username: " & username 
            If chkBelumPunyaNPWP.Checked Then
                msg &= vbCrLf & "NPWP Anda (Generated): " & npwpClean
            End If
            msg &= vbCrLf & vbCrLf & "Akun Anda telah aktif, silakan Log In."

            MsgBox(msg, MsgBoxStyle.Information, "Registrasi Berhasil")

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

    Private Sub chkBelumPunyaNPWP_CheckedChanged(sender As Object, e As EventArgs) Handles chkBelumPunyaNPWP.CheckedChanged
        If chkBelumPunyaNPWP.Checked Then
            txtNPWP.Enabled = False
            txtNPWP.Clear()
            txtNPWP.PlaceholderText = "NPWP akan digenerate otomatis"
        Else
            txtNPWP.Enabled = True
            txtNPWP.PlaceholderText = "Contoh: 12.345.678.9-123.000"
        End If
    End Sub

    Private Sub btnGoLogin_Click(sender As Object, e As EventArgs) Handles btnGoLogin.Click
        Dim f As New FrmLogin()
        f.Show()
        Me.Close()
    End Sub

    Private Sub pnlMain_Paint(sender As Object, e As PaintEventArgs) Handles pnlMain.Paint

    End Sub
End Class