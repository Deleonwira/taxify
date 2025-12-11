Imports MySql.Data.MySqlClient

Public Class pk_riwayat_bukti_potong

    Private _selectedWajibPajakId As Integer = 0

    ' Form load - set active menu and load data
    Private Sub pk_riwayat_bukti_potong_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Pk_navbar11.SetActiveMenu(pk_navbar1.MenuType.Riwayat)
        LoadPegawai()
        LoadBuktiPotong()
    End Sub

    ' ====== DATA LOADING FUNCTIONS ======

    ''' <summary>
    ''' Load pegawai cards dynamically from database
    ''' </summary>
    Private Sub LoadPegawai()
        FlowPegawai.Controls.Clear()

        Try
            modulkoneksi.BukaKoneksi()

            ' Get perusahaan_id from current session (pemberi_kerja)
            Dim perusahaanId As Integer = ModuleSession.CurrentPerusahaanId
            If perusahaanId = 0 Then
                modulkoneksi.TutupKoneksi()
                Return
            End If

            ' Query pegawai with bukti potong statistics using new schema
            Dim sql As String = "
                SELECT 
                    wp.id AS wp_id, wp.npwp, wp.nama,
                    COUNT(bp.id) AS jumlah_bukti,
                    COALESCE(SUM(bp.bruto_total), 0) AS total_bruto
                FROM pekerjaan p
                JOIN wajib_pajak wp ON wp.id = p.wajib_pajak_id
                LEFT JOIN bukti_potong bp ON bp.pekerjaan_id = p.id
                WHERE p.perusahaan_id = @perusahaan_id
                GROUP BY wp.id, wp.npwp, wp.nama
                ORDER BY wp.nama"

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@perusahaan_id", perusahaanId)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            ' Store pegawai data (wp_id, nama, jumlah_bukti, total_bruto)
            Dim pegawaiList As New List(Of Tuple(Of Integer, String, Integer, Decimal))

            While rd.Read()
                Dim wpId As Integer = Convert.ToInt32(rd("wp_id"))
                Dim nama As String = rd("nama").ToString()
                Dim jumlah As Integer = Convert.ToInt32(rd("jumlah_bukti"))
                Dim bruto As Decimal = Convert.ToDecimal(rd("total_bruto"))
                pegawaiList.Add(Tuple.Create(wpId, nama, jumlah, bruto))
            End While

            rd.Close()
            modulkoneksi.TutupKoneksi()

            ' Define card colors (rotating)
            Dim cardColors() As Color = {
                Color.FromArgb(0, 122, 255),
                Color.FromArgb(52, 199, 89),
                Color.FromArgb(255, 159, 10),
                Color.FromArgb(175, 82, 222),
                Color.FromArgb(255, 45, 85),
                Color.FromArgb(90, 200, 250)
            }

            ' Create cards dynamically
            For i As Integer = 0 To pegawaiList.Count - 1
                Dim pegawai = pegawaiList(i)
                Dim cardColor = cardColors(i Mod cardColors.Length)

                Dim card As New Guna.UI2.WinForms.Guna2Panel()
                card.BorderColor = Color.FromArgb(230, 233, 241)
                card.BorderRadius = 12
                card.BorderThickness = 1
                card.FillColor = Color.White
                card.Size = New Size(300, 90)
                card.Margin = New Padding(4)
                card.Tag = pegawai.Item1 ' Store wp_id
                card.Cursor = Cursors.Hand

                Dim pic As New Guna.UI2.WinForms.Guna2CirclePictureBox()
                pic.FillColor = cardColor
                pic.Size = New Size(50, 50)
                pic.Location = New Point(18, 18)
                pic.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle

                Dim lblName As New Guna.UI2.WinForms.Guna2HtmlLabel()
                lblName.BackColor = Color.Transparent
                lblName.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
                lblName.ForeColor = Color.FromArgb(35, 44, 63)
                lblName.Location = New Point(78, 18)
                lblName.Text = pegawai.Item2

                Dim lblStats As New Guna.UI2.WinForms.Guna2HtmlLabel()
                lblStats.BackColor = Color.Transparent
                lblStats.Font = New Font("Segoe UI", 9.0F)
                lblStats.ForeColor = Color.FromArgb(120, 128, 146)
                lblStats.Location = New Point(78, 48)
                lblStats.Text = pegawai.Item3.ToString() & " bukti • Rp " & pegawai.Item4.ToString("N0")

                card.Controls.Add(pic)
                card.Controls.Add(lblName)
                card.Controls.Add(lblStats)

                Dim wpIdValue As Integer = pegawai.Item1
                AddHandler card.Click, Sub(s, ev) FilterByPegawai(wpIdValue)
                AddHandler pic.Click, Sub(s, ev) FilterByPegawai(wpIdValue)
                AddHandler lblName.Click, Sub(s, ev) FilterByPegawai(wpIdValue)
                AddHandler lblStats.Click, Sub(s, ev) FilterByPegawai(wpIdValue)

                FlowPegawai.Controls.Add(card)
            Next

            If pegawaiList.Count = 0 Then
                Dim emptyCard As New Guna.UI2.WinForms.Guna2Panel()
                emptyCard.BorderColor = Color.FromArgb(230, 233, 241)
                emptyCard.BorderRadius = 12
                emptyCard.BorderThickness = 1
                emptyCard.FillColor = Color.White
                emptyCard.Size = New Size(300, 90)
                emptyCard.Margin = New Padding(4)

                Dim lblEmpty As New Guna.UI2.WinForms.Guna2HtmlLabel()
                lblEmpty.BackColor = Color.Transparent
                lblEmpty.Font = New Font("Segoe UI", 10.0F)
                lblEmpty.ForeColor = Color.FromArgb(120, 128, 146)
                lblEmpty.Location = New Point(24, 35)
                lblEmpty.Text = "Belum ada data pegawai"

                emptyCard.Controls.Add(lblEmpty)
                FlowPegawai.Controls.Add(emptyCard)
            End If

        Catch ex As Exception
            MsgBox("Error memuat data pegawai: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub LoadBuktiPotong(Optional searchText As String = "", Optional wajibPajakId As Integer = 0)
        GridBuktiPotong.Rows.Clear()

        Try
            modulkoneksi.BukaKoneksi()

            Dim perusahaanId As Integer = ModuleSession.CurrentPerusahaanId
            If perusahaanId = 0 Then
                modulkoneksi.TutupKoneksi()
                Return
            End If

            ' Updated query using new schema (pekerjaan-based)
            Dim sql As String = "
                SELECT 
                    bp.id, bp.nomor_bukti,
                    bp.masa_bulan, bp.masa_tahun,
                    wp.nama AS nama_pegawai, wp.npwp AS npwp_pegawai,
                    bp.bruto_total, bp.netto_total, bp.pph21_terutang
                FROM bukti_potong bp
                JOIN pekerjaan p ON p.id = bp.pekerjaan_id
                JOIN wajib_pajak wp ON wp.id = p.wajib_pajak_id
                WHERE p.perusahaan_id = @perusahaan_id"

            If wajibPajakId > 0 Then
                sql &= " AND p.wajib_pajak_id = @wp_id"
            End If

            If Not String.IsNullOrEmpty(searchText) Then
                sql &= " AND (wp.nama LIKE @search OR CONCAT(bp.masa_bulan, '/', bp.masa_tahun) LIKE @search)"
            End If

            sql &= " ORDER BY bp.masa_tahun DESC, bp.masa_bulan DESC"

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@perusahaan_id", perusahaanId)

            If wajibPajakId > 0 Then
                cmd.Parameters.AddWithValue("@wp_id", wajibPajakId)
            End If

            If Not String.IsNullOrEmpty(searchText) Then
                cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")
            End If

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            While rd.Read()
                Dim id As String = rd("id").ToString()
                Dim periode As String = rd("masa_bulan").ToString() & "/" & rd("masa_tahun").ToString()
                Dim namaPegawai As String = rd("nama_pegawai").ToString()
                Dim npwpPegawai As String = rd("npwp_pegawai").ToString()
                Dim bruto As Long = CLng(rd("bruto_total"))
                Dim neto As Long = CLng(rd("netto_total"))
                Dim pph As Long = CLng(rd("pph21_terutang"))

                GridBuktiPotong.Rows.Add(periode, namaPegawai, npwpPegawai, bruto.ToString("N0"), neto.ToString("N0"), pph.ToString("N0"), "Detail")
                GridBuktiPotong.Rows(GridBuktiPotong.Rows.Count - 1).Tag = id
            End While

            rd.Close()

        Catch ex As Exception
            MsgBox("Error memuat bukti potong: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ' ====== FILTER AND ACTION HANDLERS ======

    Private Sub FilterByPegawai(wajibPajakId As Integer)
        _selectedWajibPajakId = wajibPajakId
        TxtSearch.Text = ""
        LoadBuktiPotong(wajibPajakId:=wajibPajakId)

        For Each ctrl As Control In FlowPegawai.Controls
            If TypeOf ctrl Is Guna.UI2.WinForms.Guna2Panel Then
                Dim card As Guna.UI2.WinForms.Guna2Panel = CType(ctrl, Guna.UI2.WinForms.Guna2Panel)
                If card.Tag IsNot Nothing AndAlso Convert.ToInt32(card.Tag) = wajibPajakId Then
                    card.BorderColor = Color.FromArgb(156, 0, 219)
                    card.BorderThickness = 2
                Else
                    card.BorderColor = Color.FromArgb(230, 233, 241)
                    card.BorderThickness = 1
                End If
            End If
        Next
    End Sub

    Private Sub BtnFilter_Click(sender As Object, e As EventArgs) Handles BtnFilter.Click
        Dim searchText As String = TxtSearch.Text.Trim()
        _selectedWajibPajakId = 0

        For Each ctrl As Control In FlowPegawai.Controls
            If TypeOf ctrl Is Guna.UI2.WinForms.Guna2Panel Then
                Dim card As Guna.UI2.WinForms.Guna2Panel = CType(ctrl, Guna.UI2.WinForms.Guna2Panel)
                card.BorderColor = Color.FromArgb(230, 233, 241)
                card.BorderThickness = 1
            End If
        Next

        LoadBuktiPotong(searchText:=searchText)
    End Sub

    Private Sub TxtSearch_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            BtnFilter_Click(sender, EventArgs.Empty)
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub GridBuktiPotong_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles GridBuktiPotong.CellContentClick
        If e.RowIndex < 0 Then Return

        If e.ColumnIndex = GridBuktiPotong.Columns("colDetail").Index Then
            Dim row = GridBuktiPotong.Rows(e.RowIndex)
            If row.Tag IsNot Nothing Then
                Dim buktiPotongId As Integer = Convert.ToInt32(row.Tag)
                Dim detailForm As New pk_detail_riwayat_bukti_potong(buktiPotongId)
                detailForm.Show()
                Me.Hide()
            End If
        End If
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
        ' Already on riwayat
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

End Class