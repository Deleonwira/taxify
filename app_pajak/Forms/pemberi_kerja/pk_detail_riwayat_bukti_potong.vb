Imports MySql.Data.MySqlClient

Public Class pk_detail_riwayat_bukti_potong
    Private buktiId As Integer

    Public Sub New(id As Integer)
        InitializeComponent()
        buktiId = id
    End Sub

    Private Sub pk_detail_riwayat_bukti_potong_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Pk_navbar11.SetActiveMenu(pk_navbar1.MenuType.Riwayat)
        AddHandler Pk_navbar11.ProfilClicked, AddressOf OnProfilClicked
        LoadDetail()
    End Sub

    Private Sub LoadDetail()
        Try
            modulkoneksi.BukaKoneksi()

            ' Updated SQL using pekerjaan-based joins (app_pajak_v2 schema)
            Dim sql As String = "
                SELECT bp.*, 
                    pr.nama_perusahaan, pr.npwp_perusahaan, pr.alamat AS alamat_perusahaan,
                    wp.nama AS nama_wp, wp.alamat AS alamat_wp, wp.nik AS nik_wp, wp.npwp AS npwp_wp,
                    wp.status_ptkp
                FROM bukti_potong bp
                JOIN pekerjaan pk ON pk.id = bp.pekerjaan_id
                JOIN perusahaan pr ON pr.id = pk.perusahaan_id
                JOIN wajib_pajak wp ON wp.id = pk.wajib_pajak_id
                WHERE bp.id = @id LIMIT 1"

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@id", buktiId)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            If rd.Read() Then
                ' ====== COMPANY NAME HEADER ======
                LblCompanyName.Text = "    " & rd("nama_perusahaan").ToString()

                ' ====== DATA PERUSAHAAN ======
                LblNoBuktiValue.Text = rd("nomor_bukti").ToString()
                LblTanggalBuktiValue.Text = Convert.ToDateTime(rd("created_at")).ToString("dd MMMM yyyy")
                LblNamaPerusahaanValue.Text = rd("nama_perusahaan").ToString()
                LblNPWPPerusahaanValue.Text = If(IsDBNull(rd("npwp_perusahaan")), "-", rd("npwp_perusahaan").ToString())
                LblAlamatPerusahaanValue.Text = If(IsDBNull(rd("alamat_perusahaan")), "-", rd("alamat_perusahaan").ToString())

                ' ====== DATA PEGAWAI ======
                LblNamaPegawaiValue.Text = rd("nama_wp").ToString()
                LblNPWPPegawaiValue.Text = rd("npwp_wp").ToString()
                LblAlamatKaryawanValue.Text = If(IsDBNull(rd("alamat_wp")), "-", rd("alamat_wp").ToString())

                ' Status & NIK
                LblStatusPTKPValue.Text = If(IsDBNull(rd("status_ptkp")), "-", rd("status_ptkp").ToString())


                ' ====== KOMPONEN PENGHASILAN ======
                ' Gaji Bruto (Total Bruto)
                LblGajiBrutoValue.Text = "Rp " & Format(CLng(rd("bruto_total")), "N0")

                ' Tunjangan (Total Tunjangan + Bonus)
                Dim totalTunjangan As Long = CLng(rd("tunjangan")) + CLng(rd("bonus_thr"))
                LblTunjanganValue.Text = "Rp " & Format(totalTunjangan, "N0")

                ' Potongan (Biaya Jabatan + Iuran Pensiun)
                Dim potongan As Long = CLng(rd("biaya_jabatan")) + CLng(rd("iuran_pensiun"))
                LblPotonganValue.Text = "Rp " & Format(potongan, "N0")

                ' PPh 21
                LblPPh21DipungutValue.Text = "Rp " & Format(CLng(rd("pph21_terutang")), "N0")
                LblPPh21DisetorValue.Text = LblPPh21DipungutValue.Text

            End If

            rd.Close()

        Catch ex As Exception
            MsgBox("Gagal memuat detail bukti potong: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ' ====== NAVBAR EVENT HANDLERS ======
    Private Sub Pk_navbar11_DashboardClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.DashboardClicked
        Dim formDashboard As New pk_dashboard()
        formDashboard.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar11_DaftarPegawaiClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.DaftarPegawaiClicked
        Dim formDaftarPegawai As New pk_daftar_pegawai()
        formDaftarPegawai.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar11_BuktiPotongClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.BuktiPotongClicked
        Dim formTimeline As New pk_timeline_bukti_botong()
        formTimeline.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar11_RiwayatClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.RiwayatClicked
        Dim formRiwayat As New pk_riwayat_bukti_potong()
        formRiwayat.Show()
        Me.Close()
    End Sub

    Private Sub OnProfilClicked(sender As Object, e As EventArgs)
        Dim f As New pk_profil()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar11_LogoutClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.LogoutClicked
        Dim result As DialogResult = MessageBox.Show(
            "Apakah Anda yakin ingin keluar?",
            "Konfirmasi Logout",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            ModuleSession.ClearSession()
            Dim loginForm As New FrmLogin()
            loginForm.Show()
            Me.Close()
        End If
    End Sub

    Private Sub BtnDownload_Click(sender As Object, e As EventArgs) Handles BtnDownload.Click
        ' TODO: Implement PDF download
        MsgBox("Fitur download PDF akan segera hadir.", MsgBoxStyle.Information)
    End Sub

    Private Sub PanelEmployer_Paint(sender As Object, e As PaintEventArgs) Handles PanelEmployer.Paint

    End Sub
End Class
