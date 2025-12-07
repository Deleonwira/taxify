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
        txtJabatan.Clear()
        
        ' Load daftar perusahaan dari database
        LoadPerusahaanList()

        ' Set default selections
        'cmbStatusKepegawaian.SelectedIndex = 0 ' Default: Tetap
        cmbStatusPTKP.SelectedIndex = 0 ' Default: TK0
    End Sub

    Private Sub LoadPerusahaanList()
        Try
            modulkoneksi.BukaKoneksi()

            ' Load all perusahaan from database (termasuk Freelance dengan ID = 2)
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
        Dim nikClean As String = ""
        If Not String.IsNullOrWhiteSpace(txtNIK.Text) Then
            nikClean = txtNIK.Text.Trim().Replace("-", "").Replace(".", "").Replace(" ", "")
            If nikClean.Length <> 16 OrElse Not IsNumeric(nikClean) Then
                MsgBox("NIK harus 16 digit angka!", MsgBoxStyle.Exclamation, "Validasi")
                txtNIK.Focus()
                Return
            End If
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
        'Dim statusKepegawaian As String = cmbStatusKepegawaian.SelectedItem.ToString()
        Dim statusPTKP As String = cmbStatusPTKP.SelectedItem.ToString()

        ' 6. Insert ke Database
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

            ' Insert new user - hardcode tipe_user = "wajib_pajak", status_validasi = "pending"
            Dim sqlUser As String = "INSERT INTO users (npwp, password_hash, nama, email, tipe_user, status_validasi, no_telepon, alamat, nik) VALUES (@npwp, @pass, @nama, @email, 'wajib_pajak', 'pending', @telepon, @alamat, @nik)"
            Dim cmdUser As New MySqlCommand(sqlUser, modulkoneksi.koneksi)
            cmdUser.Parameters.AddWithValue("@npwp", npwpClean)
            cmdUser.Parameters.AddWithValue("@pass", passwordHash)
            cmdUser.Parameters.AddWithValue("@nama", txtNamaLengkap.Text.Trim())
            cmdUser.Parameters.AddWithValue("@email", txtEmail.Text.Trim())

            ' Handle optional fields
            If String.IsNullOrWhiteSpace(txtNoTelepon.Text) Then
                cmdUser.Parameters.AddWithValue("@telepon", DBNull.Value)
            Else
                cmdUser.Parameters.AddWithValue("@telepon", txtNoTelepon.Text.Trim())
            End If

            If String.IsNullOrWhiteSpace(txtAlamat.Text) Then
                cmdUser.Parameters.AddWithValue("@alamat", DBNull.Value)
            Else
                cmdUser.Parameters.AddWithValue("@alamat", txtAlamat.Text.Trim())
            End If

            If String.IsNullOrWhiteSpace(nikClean) Then
                cmdUser.Parameters.AddWithValue("@nik", DBNull.Value)
            Else
                cmdUser.Parameters.AddWithValue("@nik", nikClean)
            End If

            cmdUser.ExecuteNonQuery()

            ' Insert pekerjaan data
            Dim sqlPekerjaan As String = "INSERT INTO pekerjaan (wp_npwp, perusahaan_id, jabatan, status_kepegawaian, status_ptkp) VALUES (@npwp, @perusahaan_id, @jabatan, @status_kepegawaian, @status_ptkp)"
            Dim cmdPekerjaan As New MySqlCommand(sqlPekerjaan, modulkoneksi.koneksi)
            cmdPekerjaan.Parameters.AddWithValue("@npwp", npwpClean)
            cmdPekerjaan.Parameters.AddWithValue("@perusahaan_id", perusahaanId)

            If String.IsNullOrWhiteSpace(jabatan) Then
                cmdPekerjaan.Parameters.AddWithValue("@jabatan", DBNull.Value)
            Else
                cmdPekerjaan.Parameters.AddWithValue("@jabatan", jabatan)
            End If

            'cmdPekerjaan.Parameters.AddWithValue("@status_kepegawaian", statusKepegawaian)
            cmdPekerjaan.Parameters.AddWithValue("@status_ptkp", statusPTKP)

            cmdPekerjaan.ExecuteNonQuery()

            MsgBox("Registrasi berhasil!" & vbCrLf & vbCrLf & "Akun Anda sedang menunggu verifikasi oleh admin. Silakan coba login nanti.", MsgBoxStyle.Information, "Registrasi Pending")

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