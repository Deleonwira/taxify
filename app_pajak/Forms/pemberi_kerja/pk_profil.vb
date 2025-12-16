Imports MySql.Data.MySqlClient

Public Class pk_profil

    Private Sub pk_profil_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Setup navbar handlers
        AddHandler Pk_navbar11.DashboardClicked, AddressOf OnDashboardClicked
        AddHandler Pk_navbar11.DaftarPegawaiClicked, AddressOf OnDaftarPegawaiClicked
        AddHandler Pk_navbar11.BuktiPotongClicked, AddressOf OnBuktiPotongClicked
        AddHandler Pk_navbar11.RiwayatClicked, AddressOf OnRiwayatClicked
        AddHandler Pk_navbar11.LogoutClicked, AddressOf OnLogoutClicked
        
        AddHandler Pk_navbar11.ProfilClicked, AddressOf OnProfilClicked
        
        Pk_navbar11.SetActiveMenu(pk_navbar1.MenuType.Profil)

        LoadEmployerData()
    End Sub

    Private Sub LoadEmployerData()
        Try
            modulkoneksi.BukaKoneksi()

            ' Query to get employer and company details
            Dim sql As String = "
                SELECT pk.nama, pk.email, pk.no_telepon, pr.nama_perusahaan
                FROM pemberi_kerja pk
                JOIN perusahaan pr ON pr.id = pk.perusahaan_id
                WHERE pk.id = @pk_id"

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@pk_id", ModuleSession.CurrentPemberiKerjaId)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            If rd.Read() Then
                txtNama.Text = rd("nama").ToString()
                txtEmail.Text = rd("email").ToString()
                txtNoTelepon.Text = If(IsDBNull(rd("no_telepon")), "-", rd("no_telepon").ToString())
                txtPerusahaan.Text = rd("nama_perusahaan").ToString()
            End If

            rd.Close()

        Catch ex As Exception
            MsgBox("Gagal memuat data profil: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ' =============================
    '   NAVIGATION HANDLERS
    ' =============================
    Private Sub OnDashboardClicked(sender As Object, e As EventArgs)
        Dim f As New pk_dashboard()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnDaftarPegawaiClicked(sender As Object, e As EventArgs)
        Dim f As New pk_daftar_pegawai()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnBuktiPotongClicked(sender As Object, e As EventArgs)
        ' Logic for navigating to bukti potong if needed from navbar
    End Sub

    Private Sub OnRiwayatClicked(sender As Object, e As EventArgs)
        Dim f As New pk_riwayat_bukti_potong()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnProfilClicked(sender As Object, e As EventArgs)
        ' Stay on this page
    End Sub

    Private Sub OnLogoutClicked(sender As Object, e As EventArgs)
        ModuleSession.ClearSession()
        Dim f As New FrmLogin()
        f.Show()
        Me.Close()
    End Sub

End Class
