Imports MySql.Data.MySqlClient
Imports PdfSharp.Pdf
Imports PdfSharp.Drawing
Imports System.IO

Public Class wp_detail_lapor_pajak
    Private sptId As String

    Public Sub New(id As String)
        InitializeComponent()
        sptId = id
    End Sub

    Private Sub wp_detail_lapor_pajak_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Navigation event handlers
        AddHandler Wp_navbar1.DashboardClicked, AddressOf OnDashboardClicked
        AddHandler Wp_navbar1.LaporPajakClicked, AddressOf OnLaporPajakClicked
        AddHandler Wp_navbar1.RiwayatLaporClicked, AddressOf OnRiwayatLaporClicked
        AddHandler Wp_navbar1.TambahBuktiPotongClicked, AddressOf OnTambahBuktiPotongClicked
        AddHandler Wp_navbar1.TimelineBuktiPotongClicked, AddressOf OnTimelineBuktiPotongClicked
        AddHandler Wp_navbar1.RiwayatBuktiPotongClicked, AddressOf OnRiwayatBuktiPotongClicked
        AddHandler Wp_navbar1.DataDiriClicked, AddressOf OnDataDiriClicked
        AddHandler Wp_navbar1.LogoutClicked, AddressOf OnLogoutClicked

        Wp_navbar1.SetActiveMenu(wp_navbar.MenuType.RiwayatLapor)

        LoadDetail()
    End Sub

    Private Sub LoadDetail()
        Try
            modulkoneksi.BukaKoneksi()

            Dim sql As String =
                "SELECT st.*, wp.nama, wp.npwp, wp.nik 
                 FROM spt_tahunan st
                 JOIN wajib_pajak wp ON wp.id = st.wajib_pajak_id
                 WHERE st.id = @id LIMIT 1"

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@id", sptId)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            If rd.Read() Then
                ' Header
                Guna2HtmlLabel16.Text = "SPT Tahunan " & rd("tahun_pajak").ToString()

                ' Data Wajib Pajak
                LblTahunPajakValue.Text = If(IsDBNull(rd("tahun_pajak")), "-", rd("tahun_pajak").ToString())
                LblNamaValue.Text = If(IsDBNull(rd("nama")), "-", rd("nama").ToString())
                LblNPWPValue.Text = If(IsDBNull(rd("npwp")), "-", rd("npwp").ToString())
                LblNIKValue.Text = If(IsDBNull(rd("nik")), "-", rd("nik").ToString())
                LblStatusPTKPValue.Text = If(IsDBNull(rd("status_ptkp")), "-", rd("status_ptkp").ToString())

                ' Perhitungan Pajak
                Dim dBruto As Decimal = If(IsDBNull(rd("bruto_setahun")), 0D, Convert.ToDecimal(rd("bruto_setahun")))
                Dim dNetto As Decimal = If(IsDBNull(rd("netto_setahun")), 0D, Convert.ToDecimal(rd("netto_setahun")))
                Dim dPkp As Decimal = If(IsDBNull(rd("pkp")), 0D, Convert.ToDecimal(rd("pkp")))
                Dim dPphTerutang As Decimal = If(IsDBNull(rd("pph21_terutang")), 0D, Convert.ToDecimal(rd("pph21_terutang")))

                LblBrutoValue.Text = "Rp " & dBruto.ToString("N0")
                LblNettoValue.Text = "Rp " & dNetto.ToString("N0")
                LblPKPValue.Text = "Rp " & dPkp.ToString("N0")
                LblPPhTerutangValue.Text = "Rp " & dPphTerutang.ToString("N0")

                Dim statusSpt As String = If(IsDBNull(rd("status_spt")), "", rd("status_spt").ToString())
                LblStatusSPTValue.Text = statusSpt

                Dim nilaiAkhir As Decimal = 0
                If statusSpt = "Kurang Bayar" Then
                    nilaiAkhir = If(IsDBNull(rd("pph21_kurang_bayar")), 0D, Convert.ToDecimal(rd("pph21_kurang_bayar")))
                    LblStatusSPTValue.ForeColor = Color.FromArgb(220, 38, 38) ' Red
                ElseIf statusSpt = "Lebih Bayar" Then
                    nilaiAkhir = If(IsDBNull(rd("pph21_lebih_bayar")), 0D, Convert.ToDecimal(rd("pph21_lebih_bayar")))
                    LblStatusSPTValue.ForeColor = Color.FromArgb(34, 197, 94) ' Green
                Else
                    nilaiAkhir = 0
                    LblStatusSPTValue.ForeColor = Color.FromArgb(35, 44, 63) ' Black
                End If

                LblKurangLebihBayarValue.Text = "Rp " & nilaiAkhir.ToString("N0")

            Else
                MsgBox("Data laporan pajak tidak ditemukan.", MsgBoxStyle.Exclamation)
                Me.Close()
            End If
            rd.Close()

        Catch ex As Exception
            MsgBox("Gagal memuat detail laporan pajak: " & ex.Message, MsgBoxStyle.Critical)
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

    ' =============================
    '   PDF EXPORT LOGIC
    ' =============================
    Private Sub BtnDownload_Click(sender As Object, e As EventArgs) Handles BtnDownload.Click
        ' 1. Capture the panel content first
        Dim panelToPrint As Panel = BunifuPanel1
        Dim bmp As Bitmap = Nothing

        Dim originalAutoScroll As Boolean = panelToPrint.AutoScroll
        Dim originalSize As Size = panelToPrint.Size
        Dim originalLocation As Point = panelToPrint.Location
        Dim originalDock As DockStyle = panelToPrint.Dock

        Try
            ' Disable AutoScroll to measure full size
            panelToPrint.AutoScroll = False
            panelToPrint.Dock = DockStyle.None

            ' Calculate total height
            Dim maxBottom As Integer = 0
            For Each ctrl As Control In panelToPrint.Controls
                If ctrl.Bottom > maxBottom Then maxBottom = ctrl.Bottom
            Next

            Dim newHeight As Integer = maxBottom + 50
            panelToPrint.Size = New Size(panelToPrint.Width, newHeight)

            ' Create Bitmap
            bmp = New Bitmap(panelToPrint.Width, panelToPrint.Height)
            panelToPrint.DrawToBitmap(bmp, New Rectangle(0, 0, panelToPrint.Width, panelToPrint.Height))

        Catch ex As Exception
            MsgBox("Gagal mengambil gambar halaman: " & ex.Message, MsgBoxStyle.Critical)
            Return
        Finally
            ' Restore original state
            panelToPrint.AutoScroll = originalAutoScroll
            panelToPrint.Dock = originalDock
            panelToPrint.Size = originalSize
            panelToPrint.Location = originalLocation
        End Try

        If bmp Is Nothing Then Return

        ' 2. Save as PDF using PdfSharp
        Dim sfd As New SaveFileDialog()
        sfd.Filter = "PDF Files|*.pdf"
        sfd.FileName = "SPTTahunan_" & sptId & ".pdf"

        If sfd.ShowDialog() = DialogResult.OK Then
            Try
                ' Create PDF Document
                Dim document As New PdfDocument()
                document.Info.Title = "SPT Tahunan " & sptId

                ' Create Page (A4)
                Dim page As PdfPage = document.AddPage()
                page.Size = PdfSharp.PageSize.A4

                ' Get Graphics
                Dim gfx As XGraphics = XGraphics.FromPdfPage(page)

                ' Convert Bitmap to XImage
                Dim xImg As XImage
                Using ms As New MemoryStream()
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
                    xImg = XImage.FromStream(ms)

                    ' Calculate scaling to fit page width (leaving some margin)
                    Dim margin As Double = 20
                    Dim pageWidth As Double = page.Width.Point - (margin * 2)
                    Dim scale As Double = pageWidth / xImg.PixelWidth
                    Dim finalHeight As Double = xImg.PixelHeight * scale

                    If finalHeight > (page.Height.Point - margin * 2) Then
                        ' Resize page height to fit the long image
                        page.Height = New XUnit(finalHeight + margin * 2)
                    End If

                    ' Draw image
                    gfx.DrawImage(xImg, margin, margin, pageWidth, finalHeight)
                End Using

                ' Save
                document.Save(sfd.FileName)
                MsgBox("Berhasil diekspor ke PDF!", MsgBoxStyle.Information)

            Catch ex As Exception
                MsgBox("Gagal menyimpan PDF: " & ex.Message, MsgBoxStyle.Critical)
            Finally
                If bmp IsNot Nothing Then bmp.Dispose()
            End Try
        End If
    End Sub

    Private Sub PanelWPInfo_Paint(sender As Object, e As PaintEventArgs) Handles PanelWPInfo.Paint

    End Sub
End Class
