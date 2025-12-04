Imports MySql.Data.MySqlClient

Public Class wp_detail_bukti_potong
    Private buktiId As String

    Public Sub New(id As String)
        InitializeComponent()
        buktiId = id
    End Sub

    Private Sub wp_detail_bukti_potong_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadDetail()
    End Sub

    Private Sub LoadDetail()

        Try
            modulkoneksi.BukaKoneksi()

            Dim sql As String =
        "SELECT bp.*, 
                p.nama_perusahaan, p.npwp_perusahaan, p.alamat AS alamat_perusahaan,
                u.nama AS nama_wp, u.alamat AS alamat_wp, u.nik AS nik_wp,
                pk.status_kepegawaian, pk.status_ptkp
                FROM bukti_potong bp
                JOIN perusahaan p ON p.id = bp.perusahaan_id
                JOIN users u ON u.npwp = bp.wp_npwp
                LEFT JOIN pekerjaan pk ON pk.wp_npwp = bp.wp_npwp AND pk.perusahaan_id = bp.perusahaan_id
                WHERE bp.id = @id LIMIT 1"

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@id", buktiId)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            If rd.Read() Then

                ' ====== DATA PERUSAHAAN ======
                ' Update Purple Header Panel with company name
                Guna2HtmlLabel16.Text = rd("nama_perusahaan").ToString()

                LblNoBuktiValue.Text = rd("nomor_bukti").ToString()
                LblTanggalBuktiValue.Text = Convert.ToDateTime(rd("created_at")).ToString("dd MMMM yyyy")
                LblNamaPerusahaanValue.Text = rd("nama_perusahaan").ToString()
                LblNPWPPerusahaanValue.Text = rd("npwp_perusahaan").ToString()
                LblAlamatPerusahaanValue.Text = rd("alamat_perusahaan").ToString()

                ' ====== DATA PEGAWAI ======
                LblNamaPegawaiValue.Text = rd("nama_wp").ToString()
                LblNPWPPegawaiValue.Text = rd("wp_npwp").ToString()
                LblAlamatKaryawanValue.Text = If(IsDBNull(rd("alamat_wp")), "-", rd("alamat_wp").ToString())

                ' Status & NIK
                LblStatusPTKPValue.Text = If(IsDBNull(rd("status_ptkp")), "-", rd("status_ptkp").ToString())
                LblStatusKepegawaianValue.Text = If(IsDBNull(rd("status_kepegawaian")), "-", rd("status_kepegawaian").ToString())
                LblNomorKaryawanValue.Text = If(IsDBNull(rd("nik_wp")), "-", rd("nik_wp").ToString())

                ' ====== KOMONEN PENGHASILAN ======
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

    Private Sub BunifuPanel1_Click(sender As Object, e As EventArgs) Handles BunifuPanel1.Click

    End Sub
End Class