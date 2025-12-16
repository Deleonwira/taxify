Imports MySql.Data.MySqlClient

Public Class wp_detail_bukti_potong
    Private buktiId As String

    Public Sub New(id As String)
        InitializeComponent()
        buktiId = id
    End Sub

    Private Sub wp_detail_bukti_potong_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Navigation event handlers
        AddHandler Wp_navbar1.DashboardClicked, AddressOf OnDashboardClicked
        AddHandler Wp_navbar1.LaporPajakClicked, AddressOf OnLaporPajakClicked
        AddHandler Wp_navbar1.RiwayatLaporClicked, AddressOf OnRiwayatLaporClicked
        AddHandler Wp_navbar1.TambahBuktiPotongClicked, AddressOf OnTambahBuktiPotongClicked
        AddHandler Wp_navbar1.TimelineBuktiPotongClicked, AddressOf OnTimelineBuktiPotongClicked
        AddHandler Wp_navbar1.RiwayatBuktiPotongClicked, AddressOf OnRiwayatBuktiPotongClicked
        AddHandler Wp_navbar1.DataDiriClicked, AddressOf OnDataDiriClicked
        AddHandler Wp_navbar1.LogoutClicked, AddressOf OnLogoutClicked

        ' Set active menu (this is a detail form from RiwayatBuktiPotong)
        Wp_navbar1.SetActiveMenu(wp_navbar.MenuType.RiwayatBuktiPotong)

        LoadDetail()
    End Sub

    Private Sub LoadDetail()

        Try
            modulkoneksi.BukaKoneksi()

            Dim cleanId As String = buktiId
            Dim searchType As String = "AUTO" ' AUTO, STANDARD, FREELANCE

            ' Parse Prefix
            If buktiId.StartsWith("S-") Then
                cleanId = buktiId.Substring(2)
                searchType = "STANDARD"
            ElseIf buktiId.StartsWith("F-") Then
                cleanId = buktiId.Substring(2)
                searchType = "FREELANCE"
            End If

            Dim found As Boolean = False

            ' 1. Cek tabel bukti_potong (Pegawai Tetap) - IF mode is AUTO or STANDARD
            If searchType = "AUTO" Or searchType = "STANDARD" Then
                Dim sql As String =
            "SELECT bp.*, 
                    pr.nama_perusahaan, pr.npwp_perusahaan, pr.alamat AS alamat_perusahaan,
                    wp.nama AS nama_wp, wp.alamat AS alamat_wp, wp.nik AS nik_wp, wp.npwp AS npwp_wp,
                    wp.status_ptkp
                    FROM bukti_potong bp
                    JOIN pekerjaan pk ON pk.id = bp.pekerjaan_id
                    JOIN perusahaan pr ON pr.id = pk.perusahaan_id
                    JOIN wajib_pajak wp ON wp.id = pk.wajib_pajak_id
                    WHERE bp.id = @id LIMIT 1"

                Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
                cmd.Parameters.AddWithValue("@id", cleanId)

                Dim rd As MySqlDataReader = cmd.ExecuteReader()

                If rd.Read() Then
                    found = True
                    ' ====== DATA PERUSAHAAN (Pegawai Tetap) ======
                    Guna2HtmlLabel16.Text = rd("nama_perusahaan").ToString()

                    LblNoBuktiValue.Text = If(IsDBNull(rd("nomor_bukti")), "-", rd("nomor_bukti").ToString())

                    ' Handle Tanggal Bukti (created_at)
                    If IsDBNull(rd("created_at")) Then
                        LblTanggalBuktiValue.Text = "-"
                    Else
                        LblTanggalBuktiValue.Text = Convert.ToDateTime(rd("created_at")).ToString("dd MMMM yyyy")
                    End If

                    LblNamaPerusahaanValue.Text = rd("nama_perusahaan").ToString()
                    LblNPWPPerusahaanValue.Text = If(IsDBNull(rd("npwp_perusahaan")), "-", rd("npwp_perusahaan").ToString())

                    ' Handle Alamat Perusahaan
                    LblAlamatPerusahaanValue.Text = If(IsDBNull(rd("alamat_perusahaan")), "-", rd("alamat_perusahaan").ToString())

                    ' ====== DATA PEGAWAI ======
                    LblNamaPegawaiValue.Text = rd("nama_wp").ToString()
                    LblNPWPPegawaiValue.Text = rd("npwp_wp").ToString()
                    LblAlamatKaryawanValue.Text = If(IsDBNull(rd("alamat_wp")), "-", rd("alamat_wp").ToString())
                    LblStatusPTKPValue.Text = If(IsDBNull(rd("status_ptkp")), "-", rd("status_ptkp").ToString())

                    ' ====== KOMONEN PENGHASILAN ======
                    LblGajiBrutoValue.Text = "Rp " & Format(CLng(rd("bruto_total")), "N0")

                    Dim totalTunjangan As Long = CLng(rd("tunjangan")) + CLng(rd("bonus_thr"))
                    LblTunjanganValue.Text = "Rp " & Format(totalTunjangan, "N0")

                    Dim potongan As Long = CLng(rd("biaya_jabatan")) + CLng(rd("iuran_pensiun"))
                    LblPotonganValue.Text = "Rp " & Format(potongan, "N0")

                    LblPPh21DipungutValue.Text = "Rp " & Format(CLng(rd("pph21_terutang")), "N0")
                    LblPPh21DisetorValue.Text = LblPPh21DipungutValue.Text
                End If
                rd.Close()
            End If

            ' 2. Cek tabel bukti_potong_freelance - IF not found yet AND (mode is AUTO or FREELANCE)
            If Not found AndAlso (searchType = "AUTO" Or searchType = "FREELANCE") Then
                Dim sqlFreelance As String =
               "SELECT bpf.*, wp.nama AS nama_wp, wp.alamat AS alamat_wp, wp.npwp AS npwp_wp, wp.status_ptkp
                 FROM bukti_potong_freelance bpf
                 JOIN wajib_pajak wp ON wp.id = bpf.wajib_pajak_id
                 WHERE bpf.id = @id LIMIT 1"

                Dim cmdFreelance As New MySqlCommand(sqlFreelance, modulkoneksi.koneksi)
                cmdFreelance.Parameters.AddWithValue("@id", cleanId)

                Dim rdF As MySqlDataReader = cmdFreelance.ExecuteReader()

                If rdF.Read() Then
                    found = True
                    ' ====== DATA PERUSAHAAN (Freelance) ======
                    Guna2HtmlLabel16.Text = rdF("nama_pemberi_kerja").ToString()

                    LblNoBuktiValue.Text = If(IsDBNull(rdF("nomor_bukti")), "-", rdF("nomor_bukti").ToString())

                    If IsDBNull(rdF("created_at")) Then
                        LblTanggalBuktiValue.Text = "-"
                    Else
                        LblTanggalBuktiValue.Text = Convert.ToDateTime(rdF("created_at")).ToString("dd MMMM yyyy")
                    End If

                    LblNamaPerusahaanValue.Text = rdF("nama_pemberi_kerja").ToString()
                    LblNPWPPerusahaanValue.Text = If(IsDBNull(rdF("npwp_pemberi_kerja")), "-", rdF("npwp_pemberi_kerja").ToString())
                    LblAlamatPerusahaanValue.Text = "-" ' Tidak ada alamat di tabel freelance

                    ' ====== DATA PEGAWAI ======
                    LblNamaPegawaiValue.Text = rdF("nama_wp").ToString()
                    LblNPWPPegawaiValue.Text = rdF("npwp_wp").ToString()
                    LblAlamatKaryawanValue.Text = If(IsDBNull(rdF("alamat_wp")), "-", rdF("alamat_wp").ToString())
                    LblStatusPTKPValue.Text = If(IsDBNull(rdF("status_ptkp")), "-", rdF("status_ptkp").ToString())

                    ' ====== KOMONEN PENGHASILAN ======
                    LblGajiBrutoValue.Text = "Rp " & Format(CLng(rdF("bruto_total")), "N0")
                    LblTunjanganValue.Text = "-" ' Freelance biasanya lump sum / harian
                    LblPotonganValue.Text = "-"

                    LblPPh21DipungutValue.Text = "Rp " & Format(CLng(rdF("pph_dipotong")), "N0")
                    LblPPh21DisetorValue.Text = LblPPh21DipungutValue.Text
                End If

                rdF.Close()
            End If

            If Not found Then
                MsgBox("Data bukti potong tidak ditemukan.", MsgBoxStyle.Exclamation)
                Me.Close()
            End If

        Catch ex As Exception
            MsgBox("Gagal memuat detail bukti potong: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try

    End Sub

    ' =============================
    '   NAVIGATION HANDLERS
    ' =============================
    Private Sub OnDashboardClicked(sender As Object, e As EventArgs)
        Dim f As New wp_dashboard()
        f.Show()
        Me.Close()
    End Sub

    Private Sub OnLaporPajakClicked(sender As Object, e As EventArgs)
        Dim f As New wp_lapor_pajak()
        f.Show()
        Me.Close()
    End Sub

    Private Sub OnRiwayatLaporClicked(sender As Object, e As EventArgs)
        Dim f As New wp_riwayat_lapor_pajak()
        f.Show()
        Me.Close()
    End Sub

    Private Sub OnTambahBuktiPotongClicked(sender As Object, e As EventArgs)
        Dim f As New wp_tambah_bukti_potong()
        f.Show()
        Me.Close()
    End Sub

    Private Sub OnTimelineBuktiPotongClicked(sender As Object, e As EventArgs)
        Dim f As New wp_timeline_bukti_botong()
        f.Show()
        Me.Close()
    End Sub

    Private Sub OnRiwayatBuktiPotongClicked(sender As Object, e As EventArgs)
        Dim f As New wp_riwayat_bukti_potong()
        f.Show()
        Me.Close()
    End Sub

    Private Sub OnDataDiriClicked(sender As Object, e As EventArgs)
        Dim f As New wp_data_diri()
        f.Show()
        Me.Close()
    End Sub

    Private Sub OnLogoutClicked(sender As Object, e As EventArgs)
        ModuleSession.ClearSession()
        Dim f As New FrmLogin()
        f.Show()
        Me.Close()
    End Sub

    Private Sub PanelEmployer_Paint(sender As Object, e As PaintEventArgs) Handles PanelEmployer.Paint

    End Sub
End Class