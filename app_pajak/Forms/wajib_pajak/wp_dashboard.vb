Imports MySql.Data.MySqlClient
Imports System.Drawing.Drawing2D

Public Class wp_dashboard

    ' Dashboard data storage
    Private TotalBruto As Decimal = 0
    Private TotalPPh21 As Decimal = 0
    Private JumlahBuktiPotong As Integer = 0
    Private StatusSPT As String = "-"
    Private MonthlyData As New Dictionary(Of Integer, Decimal)

    Private Sub FrmDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Navigation event handlers
        AddHandler Wp_navbar1.DashboardClicked, AddressOf OnDashboardClicked
        AddHandler Wp_navbar1.LaporPajakClicked, AddressOf OnLaporPajakClicked
        AddHandler Wp_navbar1.RiwayatLaporClicked, AddressOf OnRiwayatLaporClicked
        AddHandler Wp_navbar1.TimelineBuktiPotongClicked, AddressOf OnTimelineBuktiPotongClicked
        AddHandler Wp_navbar1.RiwayatBuktiPotongClicked, AddressOf OnRiwayatBuktiPotongClicked
        AddHandler Wp_navbar1.DataDiriClicked, AddressOf OnDataDiriClicked
        AddHandler Wp_navbar1.LogoutClicked, AddressOf OnLogoutClicked
        AddHandler Wp_navbar1.ChatbotClicked, AddressOf OnChatbotClicked

        ' Set active menu
        Wp_navbar1.SetActiveMenu(wp_navbar.MenuType.Dashboard)

        ' Initialize chatbot
        Chatbot_control1.Visible = False
        btnChatbotFAB.Visible = True

        ' Load all dashboard data
        LoadDashboardData()
    End Sub

    Private Sub LoadDashboardData()
        LoadUserInfo()
        LoadStatistics()
        LoadRecentBuktiPotong()
        LoadMonthlyChartData()
    End Sub

    Private Sub LoadUserInfo()
        Try
            modulkoneksi.BukaKoneksi()

            Dim sql As String = "SELECT nama FROM users WHERE npwp = @npwp"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", ModuleSession.CurrentUserNPWP)

            Dim result = cmd.ExecuteScalar()
            If result IsNot Nothing Then
                Dim userName As String = result.ToString()
                LblGreeting.Text = GetGreeting() & ", " & userName
                ModuleSession.CurrentUserName = userName
            Else
                LblGreeting.Text = GetGreeting() & ", Wajib Pajak"
            End If

        Catch ex As Exception
            LblGreeting.Text = GetGreeting()
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Function GetGreeting() As String
        Dim hour As Integer = DateTime.Now.Hour
        If hour < 12 Then
            Return "Selamat Pagi"
        ElseIf hour < 15 Then
            Return "Selamat Siang"
        ElseIf hour < 18 Then
            Return "Selamat Sore"
        Else
            Return "Selamat Malam"
        End If
    End Function

    Private Sub LoadStatistics()
        Try
            modulkoneksi.BukaKoneksi()

            Dim currentYear As Integer = DateTime.Now.Year

            ' Get aggregate statistics from bukti_potong
            Dim sql As String = "
                SELECT 
                    COALESCE(SUM(bruto_total), 0) AS total_bruto,
                    COALESCE(SUM(pph21_terutang), 0) AS total_pph21,
                    COUNT(*) AS jumlah_bp
                FROM bukti_potong 
                WHERE wp_npwp = @npwp AND masa_tahun = @tahun"

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", ModuleSession.CurrentUserNPWP)
            cmd.Parameters.AddWithValue("@tahun", currentYear)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()
            If rd.Read() Then
                TotalBruto = Convert.ToDecimal(rd("total_bruto"))
                TotalPPh21 = Convert.ToDecimal(rd("total_pph21"))
                JumlahBuktiPotong = Convert.ToInt32(rd("jumlah_bp"))
            End If
            rd.Close()

            ' Get SPT status
            Dim sqlSpt As String = "SELECT status_spt FROM spt_tahunan WHERE wp_npwp = @npwp AND tahun_pajak = @tahun ORDER BY id DESC LIMIT 1"
            Dim cmdSpt As New MySqlCommand(sqlSpt, modulkoneksi.koneksi)
            cmdSpt.Parameters.AddWithValue("@npwp", ModuleSession.CurrentUserNPWP)
            cmdSpt.Parameters.AddWithValue("@tahun", currentYear)

            Dim sptResult = cmdSpt.ExecuteScalar()
            If sptResult IsNot Nothing Then
                StatusSPT = sptResult.ToString()
            Else
                StatusSPT = "Belum Lapor"
            End If

            ' Update UI labels
            LblPenghasilanValue.Text = "Rp " & TotalBruto.ToString("N0")
            LblBuktiPotongValue.Text = JumlahBuktiPotong.ToString()
            LblStatusPajakValue.Text = StatusSPT

            ' Set status color based on value
            Select Case StatusSPT.ToLower()
                Case "nihil"
                    LblStatusPajakValue.ForeColor = Color.FromArgb(34, 197, 94) ' Green
                Case "kurang bayar"
                    LblStatusPajakValue.ForeColor = Color.FromArgb(239, 68, 68) ' Red
                Case "lebih bayar"
                    LblStatusPajakValue.ForeColor = Color.FromArgb(59, 130, 246) ' Blue
                Case Else
                    LblStatusPajakValue.ForeColor = Color.FromArgb(251, 191, 36) ' Yellow
            End Select

            ' Calculate and display PPh21 in draft card
            LblDraftValue.Text = "Rp " & TotalPPh21.ToString("N0")
            LblDraftTitle.Text = "PPh21 Tahun Ini"

        Catch ex As Exception
            Console.WriteLine("Error loading statistics: " & ex.Message)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub LoadRecentBuktiPotong()
        Try
            modulkoneksi.BukaKoneksi()

            Dim sql As String = "
                SELECT bp.id, bp.nomor_bukti, bp.masa_bulan, bp.masa_tahun, bp.bruto_total, bp.pph21_terutang, p.nama_perusahaan
                FROM bukti_potong bp
                JOIN perusahaan p ON p.id = bp.perusahaan_id
                WHERE bp.wp_npwp = @npwp
                ORDER BY bp.masa_tahun DESC, bp.masa_bulan DESC
                LIMIT 5"

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", ModuleSession.CurrentUserNPWP)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            GridRiwayat.Rows.Clear()

            While rd.Read()
                Dim periode As String = GetMonthName(Convert.ToInt32(rd("masa_bulan"))) & " " & rd("masa_tahun").ToString()
                Dim nomorBukti As String = rd("nomor_bukti").ToString()
                Dim perusahaan As String = rd("nama_perusahaan").ToString()
                Dim bruto As Decimal = Convert.ToDecimal(rd("bruto_total"))

                GridRiwayat.Rows.Add(
                    periode,
                    nomorBukti,
                    perusahaan,
                    "Rp " & bruto.ToString("N0"),
                    "Selesai"
                )
            End While

            rd.Close()

        Catch ex As Exception
            Console.WriteLine("Error loading recent bukti potong: " & ex.Message)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub LoadMonthlyChartData()
        Try
            modulkoneksi.BukaKoneksi()

            Dim currentYear As Integer = DateTime.Now.Year
            MonthlyData.Clear()

            ' Initialize all months with 0
            For i As Integer = 1 To 12
                MonthlyData(i) = 0
            Next

            Dim sql As String = "
                SELECT masa_bulan, SUM(bruto_total) AS total_bruto
                FROM bukti_potong
                WHERE wp_npwp = @npwp AND masa_tahun = @tahun
                GROUP BY masa_bulan
                ORDER BY masa_bulan"

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", ModuleSession.CurrentUserNPWP)
            cmd.Parameters.AddWithValue("@tahun", currentYear)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            While rd.Read()
                Dim bulan As Integer = Convert.ToInt32(rd("masa_bulan"))
                Dim total As Decimal = Convert.ToDecimal(rd("total_bruto"))
                MonthlyData(bulan) = total
            End While

            rd.Close()

            ' Render charts with data
            RenderCharts()

        Catch ex As Exception
            Console.WriteLine("Error loading chart data: " & ex.Message)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub RenderCharts()
        Try
            ' Configure Chart1 - Monthly Income Bar Chart (Simple purple bars)
            Chart1.Series.Clear()
            Chart1.ChartAreas.Clear()
            Chart1.Legends.Clear()

            ' Create chart area with grid lines like the image
            Dim chartArea As New System.Windows.Forms.DataVisualization.Charting.ChartArea("MainArea")
            chartArea.BackColor = Color.White

            ' X Axis settings
            chartArea.AxisX.MajorGrid.Enabled = True
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(220, 220, 220)
            chartArea.AxisX.LabelStyle.Font = New Font("Segoe UI", 9)
            chartArea.AxisX.LabelStyle.ForeColor = Color.FromArgb(100, 100, 100)
            chartArea.AxisX.LineColor = Color.FromArgb(180, 180, 180)
            chartArea.AxisX.Interval = 1

            ' Y Axis settings  
            chartArea.AxisY.MajorGrid.Enabled = True
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(220, 220, 220)
            chartArea.AxisY.LabelStyle.Font = New Font("Segoe UI", 9)
            chartArea.AxisY.LabelStyle.ForeColor = Color.FromArgb(100, 100, 100)
            chartArea.AxisY.LineColor = Color.FromArgb(180, 180, 180)

            Chart1.ChartAreas.Add(chartArea)

            ' Create series for monthly income - purple bars
            Dim series As New System.Windows.Forms.DataVisualization.Charting.Series("Penghasilan")
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column
            series.Color = Color.FromArgb(156, 0, 219) ' Purple color like the image
            series.BorderWidth = 0

            ' Add data points for each month (1-12)
            For i As Integer = 1 To 12
                Dim value As Decimal = MonthlyData(i)
                ' Convert to millions for display
                Dim valueInMillions As Double = CDbl(value) / 1000000
                series.Points.AddXY(i, valueInMillions)
            Next

            Chart1.Series.Add(series)

            ' Add simple legend
            Dim legend As New System.Windows.Forms.DataVisualization.Charting.Legend("Legend1")
            legend.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Right
            legend.Font = New Font("Segoe UI", 9)
            legend.ForeColor = Color.FromArgb(156, 0, 219)
            Chart1.Legends.Add(legend)

            ' Set Y axis title
            Chart1.ChartAreas(0).AxisY.Title = "Juta Rupiah"
            Chart1.ChartAreas(0).AxisY.TitleFont = New Font("Segoe UI", 9)

            ' Set X axis title
            Chart1.ChartAreas(0).AxisX.Title = "Bulan"
            Chart1.ChartAreas(0).AxisX.TitleFont = New Font("Segoe UI", 9)

        Catch ex As Exception
            Console.WriteLine("Error rendering charts: " & ex.Message)
        End Try
    End Sub

    Private Function GetMonthName(month As Integer) As String
        Dim months() As String = {"", "Januari", "Februari", "Maret", "April", "Mei", "Juni",
                                  "Juli", "Agustus", "September", "Oktober", "November", "Desember"}
        If month >= 1 And month <= 12 Then
            Return months(month)
        End If
        Return ""
    End Function

    Private Function GetMonthAbbrev(month As Integer) As String
        Dim months() As String = {"", "Jan", "Feb", "Mar", "Apr", "Mei", "Jun",
                                  "Jul", "Agu", "Sep", "Okt", "Nov", "Des"}
        If month >= 1 And month <= 12 Then
            Return months(month)
        End If
        Return ""
    End Function

    ' ========== NAVIGATION HANDLERS ==========

    Private Sub OnDashboardClicked(sender As Object, e As EventArgs)
        ' Already on dashboard - refresh data
        LoadDashboardData()
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

    ' ========== CHATBOT HANDLERS ==========

    Private Sub OnChatbotClicked(sender As Object, e As EventArgs)
        ToggleChatbot()
    End Sub

    Private Sub btnChatbotFAB_Click(sender As Object, e As EventArgs) Handles btnChatbotFAB.Click
        ToggleChatbot()
    End Sub

    Private Sub ToggleChatbot()
        If Chatbot_control1.Visible Then
            Chatbot_control1.Visible = False
            btnChatbotFAB.Visible = True
        Else
            Dim fabLocation As Point = btnChatbotFAB.Location
            Chatbot_control1.Location = New Point(
                fabLocation.X + btnChatbotFAB.Width - Chatbot_control1.Width,
                fabLocation.Y - Chatbot_control1.Height - 10
            )
            Chatbot_control1.Visible = True
            Chatbot_control1.BringToFront()
            btnChatbotFAB.Visible = False
        End If
    End Sub

    Private Sub Chatbot_control1_VisibleChanged(sender As Object, e As EventArgs) Handles Chatbot_control1.VisibleChanged
        If Not Chatbot_control1.Visible Then
            btnChatbotFAB.Visible = True
        End If
    End Sub

    ' ========== GRID EVENT ==========

    Private Sub GridRiwayat_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles GridRiwayat.CellClick
        If e.RowIndex >= 0 Then
            ' Navigate to detail
            Dim f As New wp_riwayat_bukti_potong()
            f.Show()
            Me.Hide()
        End If
    End Sub

    Private Sub FlowStats_Paint(sender As Object, e As PaintEventArgs) Handles FlowStats.Paint
    End Sub

    Private Sub PanelMain_Paint(sender As Object, e As PaintEventArgs) Handles PanelMain.Paint
    End Sub

    Private Sub CardBuktiPotong_Paint(sender As Object, e As PaintEventArgs) Handles CardBuktiPotong.Paint
    End Sub

    Private Sub PanelChart_Paint(sender As Object, e As PaintEventArgs) Handles PanelChart.Paint
    End Sub

    Private Sub Wp_navbar1_Load(sender As Object, e As EventArgs) Handles Wp_navbar1.Load

    End Sub
End Class