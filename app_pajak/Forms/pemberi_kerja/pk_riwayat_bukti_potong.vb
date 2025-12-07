Imports MySql.Data.MySqlClient

Public Class pk_riwayat_bukti_potong

    Private _selectedPegawaiNPWP As String = ""

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

            ' Get perusahaan_id from current user
            Dim perusahaanId As Integer = GetPerusahaanId()
            If perusahaanId = 0 Then
                modulkoneksi.TutupKoneksi()
                Return
            End If

            ' Query pegawai with bukti potong statistics
            Dim sql As String = "
                SELECT 
                    u.npwp, u.nama,
                    COUNT(bp.id) AS jumlah_bukti,
                    COALESCE(SUM(bp.bruto_total), 0) AS total_bruto
                FROM pekerjaan p
                JOIN users u ON u.npwp = p.wp_npwp
                LEFT JOIN bukti_potong bp ON bp.wp_npwp = p.wp_npwp 
                    AND bp.perusahaan_id = p.perusahaan_id
                WHERE p.perusahaan_id = @perusahaan_id
                GROUP BY u.npwp, u.nama
                ORDER BY u.nama"

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@perusahaan_id", perusahaanId)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            ' Store pegawai data
            Dim pegawaiList As New List(Of Tuple(Of String, String, Integer, Decimal))

            While rd.Read()
                Dim npwp As String = rd("npwp").ToString()
                Dim nama As String = rd("nama").ToString()
                Dim jumlah As Integer = Convert.ToInt32(rd("jumlah_bukti"))
                Dim bruto As Decimal = Convert.ToDecimal(rd("total_bruto"))
                pegawaiList.Add(Tuple.Create(npwp, nama, jumlah, bruto))
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
                card.Tag = pegawai.Item1
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

                Dim npwpValue As String = pegawai.Item1
                AddHandler card.Click, Sub(s, ev) FilterByPegawai(npwpValue)
                AddHandler pic.Click, Sub(s, ev) FilterByPegawai(npwpValue)
                AddHandler lblName.Click, Sub(s, ev) FilterByPegawai(npwpValue)
                AddHandler lblStats.Click, Sub(s, ev) FilterByPegawai(npwpValue)

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

    Private Sub LoadBuktiPotong(Optional searchText As String = "", Optional pegawaiNPWP As String = "")
        GridBuktiPotong.Rows.Clear()

        Try
            modulkoneksi.BukaKoneksi()

            Dim perusahaanId As Integer = GetPerusahaanId()
            If perusahaanId = 0 Then
                modulkoneksi.TutupKoneksi()
                Return
            End If

            Dim sql As String = "
                SELECT 
                    bp.id, bp.nomor_bukti,
                    bp.masa_bulan, bp.masa_tahun,
                    u.nama AS nama_pegawai, u.npwp AS npwp_pegawai,
                    bp.bruto_total, bp.netto_total, bp.pph21_terutang
                FROM bukti_potong bp
                JOIN users u ON u.npwp = bp.wp_npwp
                WHERE bp.perusahaan_id = @perusahaan_id"

            If Not String.IsNullOrEmpty(pegawaiNPWP) Then
                sql &= " AND bp.wp_npwp = @pegawaiNPWP"
            End If

            If Not String.IsNullOrEmpty(searchText) Then
                sql &= " AND (u.nama LIKE @search OR CONCAT(bp.masa_bulan, '/', bp.masa_tahun) LIKE @search)"
            End If

            sql &= " ORDER BY bp.masa_tahun DESC, bp.masa_bulan DESC"

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@perusahaan_id", perusahaanId)

            If Not String.IsNullOrEmpty(pegawaiNPWP) Then
                cmd.Parameters.AddWithValue("@pegawaiNPWP", pegawaiNPWP)
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

    Private Function GetPerusahaanId() As Integer
        Try
            Dim sql As String = "SELECT id FROM perusahaan WHERE owner_npwp = @npwp LIMIT 1"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", ModuleSession.CurrentUserNPWP)
            Dim result = cmd.ExecuteScalar()
            If result IsNot Nothing Then
                Return Convert.ToInt32(result)
            End If
        Catch ex As Exception
            MsgBox("Error mendapatkan perusahaan: " & ex.Message, MsgBoxStyle.Critical)
        End Try
        Return 0
    End Function

    ' ====== FILTER AND ACTION HANDLERS ======

    Private Sub FilterByPegawai(pegawaiNPWP As String)
        _selectedPegawaiNPWP = pegawaiNPWP
        TxtSearch.Text = ""
        LoadBuktiPotong(pegawaiNPWP:=pegawaiNPWP)

        For Each ctrl As Control In FlowPegawai.Controls
            If TypeOf ctrl Is Guna.UI2.WinForms.Guna2Panel Then
                Dim card As Guna.UI2.WinForms.Guna2Panel = CType(ctrl, Guna.UI2.WinForms.Guna2Panel)
                If card.Tag IsNot Nothing AndAlso card.Tag.ToString() = pegawaiNPWP Then
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
        _selectedPegawaiNPWP = ""

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