Imports MySql.Data.MySqlClient

Public Class wp_data_diri

    Private Sub wp_data_diri_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Navbar
        AddHandler Wp_navbar1.DashboardClicked, AddressOf OnDashboardClicked
        AddHandler Wp_navbar1.LaporPajakClicked, AddressOf OnLaporPajakClicked
        AddHandler Wp_navbar1.RiwayatLaporClicked, AddressOf OnRiwayatLaporClicked
        AddHandler Wp_navbar1.TimelineBuktiPotongClicked, AddressOf OnTimelineBuktiPotongClicked
        AddHandler Wp_navbar1.RiwayatBuktiPotongClicked, AddressOf OnRiwayatBuktiPotongClicked
        AddHandler Wp_navbar1.DataDiriClicked, AddressOf OnDataDiriClicked
        AddHandler Wp_navbar1.LogoutClicked, AddressOf OnLogoutClicked

        Wp_navbar1.SetActiveMenu(wp_navbar.MenuType.DataDiri)

        LoadUserData()
    End Sub


    ' =============================
    '   LOAD DATA USER
    ' =============================
    Private Sub LoadUserData()
        Try
            modulkoneksi.BukaKoneksi()

            Dim sql As String = "
                SELECT nama, npwp, email, no_telepon, alamat
                FROM users
                WHERE npwp = @npwp
                LIMIT 1
            "

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", ModuleSession.CurrentUserNPWP)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            If rd.Read() Then
                txtNama.Text = rd("nama").ToString()
                txtNIK.Text = rd("npwp").ToString()
                Guna2TextBox1.Text = rd("email").ToString()

                txtEmail.Text = If(IsDBNull(rd("no_telepon")), "", rd("no_telepon").ToString())
                txtAlamat.Text = If(IsDBNull(rd("alamat")), "", rd("alamat").ToString())
            End If

            rd.Close()

        Catch ex As Exception
            MsgBox("Gagal memuat data: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub



    ' =============================
    '   SIMPAN PERUBAHAN DATA
    ' =============================
    Private Sub btnSimpan_Click(sender As Object, e As EventArgs) Handles btnSimpan.Click

        Dim changePassword As Boolean = False
        Dim newPasswordHash As String = ""

        ' Cek jika user ingin mengganti password
        If Guna2TextBox3.Text <> "" Or Guna2TextBox2.Text <> "" Then

            If Guna2TextBox3.Text = "" Or Guna2TextBox2.Text = "" Then
                MsgBox("Isi password lama dan password baru untuk mengganti password!", MsgBoxStyle.Exclamation)
                Return
            End If

            If Not VerifyOldPassword(Guna2TextBox3.Text) Then
                MsgBox("Password lama salah!", MsgBoxStyle.Critical)
                Return
            End If

            changePassword = True
            newPasswordHash = ModuleSecurity.HashPassword(Guna2TextBox2.Text)
        End If


        Try
            modulkoneksi.BukaKoneksi()

            Dim sql As String = "
                UPDATE users 
                SET nama = @nama,
                    email = @email,
                    no_telepon = @telp,
                    alamat = @alamat
            "

            If changePassword Then
                sql &= ", password_hash = @pass"
            End If

            sql &= " WHERE npwp = @npwp"

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)

            cmd.Parameters.AddWithValue("@nama", txtNama.Text)
            cmd.Parameters.AddWithValue("@email", Guna2TextBox1.Text)
            cmd.Parameters.AddWithValue("@telp", txtEmail.Text)
            cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text)
            cmd.Parameters.AddWithValue("@npwp", ModuleSession.CurrentUserNPWP)

            If changePassword Then
                cmd.Parameters.AddWithValue("@pass", newPasswordHash)
            End If

            cmd.ExecuteNonQuery()

            MsgBox("Data berhasil disimpan!", MsgBoxStyle.Information)

            Guna2TextBox2.Clear()
            Guna2TextBox3.Clear()

            ModuleSession.CurrentUserName = txtNama.Text

        Catch ex As Exception
            MsgBox("Gagal menyimpan data: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub



    ' =============================
    '   VERIFY PASSWORD LAMA
    ' =============================
    Private Function VerifyOldPassword(oldPass As String) As Boolean
        Try
            modulkoneksi.BukaKoneksi()

            Dim sql As String = "
                SELECT password_hash 
                FROM users 
                WHERE npwp = @npwp
            "

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", ModuleSession.CurrentUserNPWP)

            Dim dbHash As String = cmd.ExecuteScalar().ToString()

            Return ModuleSecurity.VerifyPassword(oldPass, dbHash)

        Catch ex As Exception
            Return False
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Function



    ' =============================
    '   BUTTON DISCARD CHANGES
    ' =============================
    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        LoadUserData()
        Guna2TextBox2.Clear()
        Guna2TextBox3.Clear()
    End Sub



    ' =============================
    '   NAVIGATION HANDLERS
    ' =============================
    Private Sub OnDashboardClicked(sender As Object, e As EventArgs)
        Dim f As New wp_dashboard()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnLaporPajakClicked(sender As Object, e As EventArgs)
        Dim f As New wp_lapor_pajak()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnRiwayatLaporClicked(sender As Object, e As EventArgs)
        Dim f As New wp_riwayat_lapor_pajak()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnTimelineBuktiPotongClicked(sender As Object, e As EventArgs)
        Dim f As New wp_timeline_bukti_botong()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnRiwayatBuktiPotongClicked(sender As Object, e As EventArgs)
        Dim f As New wp_riwayat_bukti_potong()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnDataDiriClicked(sender As Object, e As EventArgs)
        ' Stay on this page
    End Sub

    Private Sub OnLogoutClicked(sender As Object, e As EventArgs)
        ModuleSession.ClearSession()
        Dim f As New FrmLogin()
        f.Show()
        Me.Close()
    End Sub

End Class
