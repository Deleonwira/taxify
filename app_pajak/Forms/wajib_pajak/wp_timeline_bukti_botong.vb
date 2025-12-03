Imports MySql.Data.MySqlClient

Public Class wp_timeline_bukti_botong
    ' Class-level variables to track state
    Private selectedMonth As Integer = 0
    Private currentYear As Integer = DateTime.Now.Year
    Private monthCards As List(Of Guna.UI2.WinForms.Guna2Panel)
    Private buktiPotongId As String = ""

    Private Sub wp_timeline_bukti_botong_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize month cards list
        monthCards = New List(Of Guna.UI2.WinForms.Guna2Panel) From {
            MonthCardJan, MonthCardFeb, MonthCardMar, MonthCardApr,
            MonthCardMay, MonthCardJun, MonthCardJul, MonthCardAug,
            MonthCardSep, MonthCardOct, MonthCardNov, MonthCardDec
        }

        ' Add click handlers to all month cards
        For i As Integer = 0 To monthCards.Count - 1
            Dim monthIndex As Integer = i + 1 ' Store month number (1-12)
            AddHandler monthCards(i).Click, Sub(s, ev) SelectMonth(monthIndex)
            
            ' Make cursor hand pointer for better UX
            monthCards(i).Cursor = Cursors.Hand
        Next

        ' Navigation event handlers
        AddHandler Wp_navbar1.DashboardClicked, AddressOf OnDashboardClicked
        AddHandler Wp_navbar1.LaporPajakClicked, AddressOf OnLaporPajakClicked
        AddHandler Wp_navbar1.RiwayatLaporClicked, AddressOf OnRiwayatLaporClicked
        AddHandler Wp_navbar1.TimelineBuktiPotongClicked, AddressOf OnTimelineBuktiPotongClicked
        AddHandler Wp_navbar1.RiwayatBuktiPotongClicked, AddressOf OnRiwayatBuktiPotongClicked
        AddHandler Wp_navbar1.DataDiriClicked, AddressOf OnDataDiriClicked
        AddHandler Wp_navbar1.LogoutClicked, AddressOf OnLogoutClicked

        ' Set active menu
        Wp_navbar1.SetActiveMenu(wp_navbar.MenuType.TimelineBuktiPotong)

        ' Load indicators for all months to show which have data
        LoadAllMonthsIndicators()

        ' Select current month by default
        SelectMonth(DateTime.Now.Month)
    End Sub

    Private Sub SelectMonth(monthNumber As Integer)
        ' Validate month number
        If monthNumber < 1 Or monthNumber > 12 Then Return

        ' Update selected month
        selectedMonth = monthNumber

        ' Reset all month cards to default border
        For Each card In monthCards
            card.BorderColor = Color.FromArgb(224, 231, 245) ' Light gray border
        Next

        ' Highlight selected month card with purple border
        monthCards(monthNumber - 1).BorderColor = Color.FromArgb(156, 0, 219) ' Purple

        ' Update the selected month label
        LblSelectedMonth.Text = GetMonthName(monthNumber) & " " & currentYear.ToString()

        ' Load data for the selected month
        LoadMonthData(monthNumber)
    End Sub

    Private Sub LoadMonthData(monthNumber As Integer)
        Try
            modulkoneksi.BukaKoneksi()

            ' Query to get bukti_potong for the selected month
            Dim sql As String =
                "SELECT bp.*, p.nama_perusahaan
                 FROM bukti_potong bp
                 JOIN perusahaan p ON p.id = bp.perusahaan_id
                 WHERE bp.wp_npwp = @npwp
                 AND bp.masa_bulan = @bulan
                 AND bp.masa_tahun = @tahun
                 LIMIT 1"

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", ModuleSession.CurrentUserNPWP)
            cmd.Parameters.AddWithValue("@bulan", monthNumber)
            cmd.Parameters.AddWithValue("@tahun", currentYear)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            If rd.Read() Then
                ' Data exists for this month
                buktiPotongId = rd("id").ToString()

                ' Update input date
                If Not IsDBNull(rd("created_at")) Then
                    LblTanggalInputValue.Text = Convert.ToDateTime(rd("created_at")).ToString("dd MMMM yyyy")
                Else
                    LblTanggalInputValue.Text = "-"
                End If

                ' Enable Detail button
                BtnInputLapor.Enabled = True
                BtnInputLapor.Text = "Detail"

                ' Show status as "Sudah Input"
                Guna2Button1.Visible = True
                Guna2Button1.Text = "Sudah Input"
                Guna2Button1.FillColor = Color.FromArgb(192, 255, 192)
                Guna2Button1.ForeColor = Color.FromArgb(0, 219, 101)

            Else
                ' No data for this month
                buktiPotongId = ""
                LblTanggalInputValue.Text = "Belum ada data"

                ' Change button to Input mode
                BtnInputLapor.Enabled = True
                BtnInputLapor.Text = "Input"

                ' Hide status badge
                Guna2Button1.Visible = False
            End If

            rd.Close()

        Catch ex As Exception
            MsgBox("Error memuat data bulan: " & ex.Message, MsgBoxStyle.Critical)
            buktiPotongId = ""
            LblTanggalInputValue.Text = "Error"
            BtnInputLapor.Enabled = False
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub LoadAllMonthsIndicators()
        ' Load indicators for all 12 months to show which have data
        Try
            modulkoneksi.BukaKoneksi()

            ' Query to get all months that have bukti_potong data for this year
            Dim sql As String =
                "SELECT DISTINCT bp.masa_bulan
                 FROM bukti_potong bp
                 WHERE bp.wp_npwp = @npwp
                 AND bp.masa_tahun = @tahun
                 ORDER BY bp.masa_bulan"

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", ModuleSession.CurrentUserNPWP)
            cmd.Parameters.AddWithValue("@tahun", currentYear)

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            ' Create a list of months that have data
            Dim monthsWithData As New List(Of Integer)
            While rd.Read()
                monthsWithData.Add(CInt(rd("masa_bulan")))
            End While

            rd.Close()

            ' Update all 12 month indicators
            For monthNum As Integer = 1 To 12
                If monthsWithData.Contains(monthNum) Then
                    ' Green for months with data
                    UpdateMonthIndicator(monthNum, Color.FromArgb(0, 219, 101))
                Else
                    ' Gray for months without data
                    UpdateMonthIndicator(monthNum, Color.FromArgb(203, 213, 225))
                End If
            Next

        Catch ex As Exception
            ' If error, set all indicators to gray
            For monthNum As Integer = 1 To 12
                UpdateMonthIndicator(monthNum, Color.FromArgb(203, 213, 225))
            Next
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub UpdateMonthIndicator(monthNumber As Integer, indicatorColor As Color)
        ' Update the indicator circle for the specific month
        Select Case monthNumber
            Case 1
                IndicatorJan.FillColor = indicatorColor
            Case 2
                IndicatorFeb.FillColor = indicatorColor
            Case 3
                IndicatorMar.FillColor = indicatorColor
            Case 4
                IndicatorApr.FillColor = indicatorColor
            Case 5
                IndicatorMay.FillColor = indicatorColor
            Case 6
                IndicatorJun.FillColor = indicatorColor
            Case 7
                IndicatorJul.FillColor = indicatorColor
            Case 8
                IndicatorAug.FillColor = indicatorColor
            Case 9
                IndicatorSep.FillColor = indicatorColor
            Case 10
                IndicatorOct.FillColor = indicatorColor
            Case 11
                IndicatorNov.FillColor = indicatorColor
            Case 12
                IndicatorDec.FillColor = indicatorColor
        End Select
    End Sub

    Private Function GetMonthName(monthNumber As Integer) As String
        ' Return Indonesian month names
        Select Case monthNumber
            Case 1
                Return "Januari"
            Case 2
                Return "Februari"
            Case 3
                Return "Maret"
            Case 4
                Return "April"
            Case 5
                Return "Mei"
            Case 6
                Return "Juni"
            Case 7
                Return "Juli"
            Case 8
                Return "Agustus"
            Case 9
                Return "September"
            Case 10
                Return "Oktober"
            Case 11
                Return "November"
            Case 12
                Return "Desember"
            Case Else
                Return ""
        End Select
    End Function

    Private Sub BtnInputLapor_Click(sender As Object, e As EventArgs) Handles BtnInputLapor.Click
        ' Handle Detail button click
        If BtnInputLapor.Text = "Detail" And buktiPotongId <> "" Then
            ' Open detail form with the bukti potong ID
            Dim detailForm As New wp_detail_bukti_potong(buktiPotongId)
            detailForm.Show()
        ElseIf BtnInputLapor.Text = "Input" Then
            ' TODO: Navigate to input form for new bukti potong
            MsgBox("Fitur input bukti potong akan segera hadir.", MsgBoxStyle.Information)
        End If
    End Sub

    Private Sub BtnEditLapor_Click(sender As Object, e As EventArgs) Handles BtnEditLapor.Click
        ' Handle Edit button click
        If buktiPotongId <> "" Then
            ' TODO: Open edit form
            MsgBox("Fitur edit bukti potong akan segera hadir.", MsgBoxStyle.Information)
        Else
            MsgBox("Tidak ada data untuk diedit.", MsgBoxStyle.Information)
        End If
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
        ' Already on this page
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
End Class