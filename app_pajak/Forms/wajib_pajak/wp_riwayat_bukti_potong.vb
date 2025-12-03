Imports MySql.Data.MySqlClient

Public Class wp_riwayat_bukti_potong

    Private Sub wp_riwayat_bukti_potong_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Navigation event handlers
        AddHandler Wp_navbar1.DashboardClicked, AddressOf OnDashboardClicked
        AddHandler Wp_navbar1.LaporPajakClicked, AddressOf OnLaporPajakClicked
        AddHandler Wp_navbar1.RiwayatLaporClicked, AddressOf OnRiwayatLaporClicked
        AddHandler Wp_navbar1.TimelineBuktiPotongClicked, AddressOf OnTimelineBuktiPotongClicked
        AddHandler Wp_navbar1.RiwayatBuktiPotongClicked, AddressOf OnRiwayatBuktiPotongClicked
        AddHandler Wp_navbar1.DataDiriClicked, AddressOf OnDataDiriClicked
        AddHandler Wp_navbar1.LogoutClicked, AddressOf OnLogoutClicked

        ' Set active menu
        Wp_navbar1.SetActiveMenu(wp_navbar.MenuType.RiwayatBuktiPotong)

        LoadBuktiPotong()
        LoadCompanies()
    End Sub

    Private Sub LoadBuktiPotong()

        GridBukti.Rows.Clear()

        Try
            modulkoneksi.BukaKoneksi()

            Dim sql As String =
            "SELECT bp.*, p.nama_perusahaan 
             FROM bukti_potong bp
             JOIN perusahaan p ON p.id = bp.perusahaan_id
             WHERE bp.wp_npwp = @npwp
             ORDER BY masa_tahun DESC, masa_bulan DESC"



            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", 789)
            'Taro session diatas

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            While rd.Read()

                Dim id As String = rd("id").ToString()
                Dim periode As String = rd("masa_bulan").ToString() & "/" & rd("masa_tahun").ToString()
                Dim nomorBukti As String = rd("nomor_bukti").ToString()
                Dim perusahaan As String = rd("nama_perusahaan").ToString()

                Dim bruto As Long = CLng(rd("bruto_total"))
                Dim neto As Long = CLng(rd("netto_total"))
                Dim pph As Long = CLng(rd("pph21_terutang"))

                GridBukti.Rows.Add(
                periode,
                id,
                perusahaan,
                bruto.ToString("N0"),
                neto.ToString("N0"),
                pph.ToString("N0"),
                "Detail"
            )

            End While

            rd.Close()

        Catch ex As Exception
            MsgBox("Error memuat bukti potong: " & ex.Message)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try

    End Sub


    Private Sub LoadCompanies()
        ' Menampilkan perusahaan di kartu (dummy jika tidak ada)
        Try
            modulkoneksi.BukaKoneksi()

            Dim sql As String =
                "SELECT DISTINCT p.nama_perusahaan
                 FROM bukti_potong bp
                 JOIN perusahaan p ON p.id = bp.perusahaan_id
                 WHERE bp.wp_npwp = @npwp"

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", ModuleSession.CurrentUserNPWP)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            Dim listNama As New List(Of String)

            While rd.Read()
                listNama.Add(rd("nama_perusahaan").ToString())
            End While

            rd.Close()

            ' Isi kartu
            Dim cards() As Guna.UI2.WinForms.Guna2HtmlLabel = {Company1Name, Company2Name, Company3Name}

            For i As Integer = 0 To cards.Length - 1
                If i < listNama.Count Then
                    cards(i).Text = listNama(i)
                Else
                    cards(i).Text = "(Tidak ada data)"
                End If
            Next

        Catch ex As Exception
            MsgBox("Error load companies: " & ex.Message)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub PanelMain_Paint(sender As Object, e As PaintEventArgs) Handles PanelMain.Paint

    End Sub

    Private Sub PanelTable_Paint(sender As Object, e As PaintEventArgs) Handles PanelTable.Paint

    End Sub

    Private Sub GridBukti_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles GridBukti.CellContentClick
        Dim id = GridBukti.CurrentRow.Cells("colBuktiPotong").Value

        Dim f As New wp_detail_bukti_potong(id)
        f.Show()

        Console.WriteLine(id)

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
        ' Already on this page
    End Sub

    Private Sub OnDataDiriClicked(sender As Object, e As EventArgs)
        Dim f As New wp_data_diri()
        f.Show()
        Me.Hide()
    End Sub

    Private Sub OnLogoutClicked(sender As Object, e As EventArgs)
        ModuleSession.ClearSession()
        Dim f As New FrmLogin()
        f.Show()
        Me.Close()
    End Sub
End Class
