Imports MySql.Data.MySqlClient

Public Class pk_timeline_bukti_botong

    ' Properties
    Public Property EmployeeNPWP As String = ""
    Public Property PTKPStatusValue As String = ""
    Public Property EmployeeName As String = ""
    Private SelectedMonth As Integer = 0

    ' Form load - set active menu and load data
    Private Sub pk_timeline_bukti_botong_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Pk_navbar11.SetActiveMenu(pk_navbar1.MenuType.BuktiPotong)
        LoadEmployeeName()
        
        ' Initialize UI
        LblSelectedMonth.Text = "Pilih Bulan"
        BtnInputLapor.Text = "Buat Bukti Potong"
        BtnInputLapor.Enabled = False
        
        ' Add click handlers for month cards
        AddHandler MonthCardJan.Click, Sub() SelectMonth(1)
        AddHandler MonthCardFeb.Click, Sub() SelectMonth(2)
        AddHandler MonthCardMar.Click, Sub() SelectMonth(3)
        AddHandler MonthCardApr.Click, Sub() SelectMonth(4)
        AddHandler MonthCardMay.Click, Sub() SelectMonth(5)
        AddHandler MonthCardJun.Click, Sub() SelectMonth(6)
        AddHandler MonthCardJul.Click, Sub() SelectMonth(7)
        AddHandler MonthCardAug.Click, Sub() SelectMonth(8)
        AddHandler MonthCardSep.Click, Sub() SelectMonth(9)
        AddHandler MonthCardOct.Click, Sub() SelectMonth(10)
        AddHandler MonthCardNov.Click, Sub() SelectMonth(11)
        AddHandler MonthCardDec.Click, Sub() SelectMonth(12)
        
        ' Also add handlers for labels and indicators inside cards to bubble up click
        For Each ctrl As Control In FlowTimeline.Controls
            If TypeOf ctrl Is Guna.UI2.WinForms.Guna2Panel Then
                For Each child As Control In ctrl.Controls
                    AddHandler child.Click, Sub(s, args) SelectMonth(FlowTimeline.Controls.GetChildIndex(ctrl) + 1)
                Next
            End If
        Next
    End Sub

    ' Load Employee Name
    Private Sub LoadEmployeeName()
        If String.IsNullOrEmpty(EmployeeNPWP) Then Return

        Try
            modulkoneksi.BukaKoneksi()
            Dim query As String = "SELECT nama FROM users WHERE npwp = @npwp"
            Dim cmd As New MySqlCommand(query, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@npwp", EmployeeNPWP)
            
            Dim result = cmd.ExecuteScalar()
            If result IsNot Nothing Then
                EmployeeName = result.ToString()
                LblTitle.Text = "Timeline Bukti Potong - " & EmployeeName
            End If
        Catch ex As Exception
            MessageBox.Show("Error loading employee name: " & ex.Message)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ' Handle month selection
    Private Sub SelectMonth(month As Integer)
        SelectedMonth = month
        Dim monthName As String = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)
        LblSelectedMonth.Text = monthName & " " & DateTime.Now.Year.ToString()
        BtnInputLapor.Enabled = True
        
        ' Reset all cards visual
        ResetCardsVisual()
        
        ' Highlight selected card
        Dim card As Guna.UI2.WinForms.Guna2Panel = CType(FlowTimeline.Controls(month - 1), Guna.UI2.WinForms.Guna2Panel)
        card.BorderColor = Color.FromArgb(156, 0, 219)
        card.BorderThickness = 2
        card.FillColor = Color.FromArgb(240, 230, 255)
    End Sub

    Private Sub ResetCardsVisual()
        For Each ctrl As Control In FlowTimeline.Controls
            If TypeOf ctrl Is Guna.UI2.WinForms.Guna2Panel Then
                Dim card As Guna.UI2.WinForms.Guna2Panel = CType(ctrl, Guna.UI2.WinForms.Guna2Panel)
                card.BorderColor = Color.FromArgb(224, 231, 245)
                card.BorderThickness = 1
                card.FillColor = Color.White
            End If
        Next
    End Sub

    ' Handle "Buat Bukti Potong" button click
    Private Sub BtnInputLapor_Click(sender As Object, e As EventArgs) Handles BtnInputLapor.Click
        If SelectedMonth = 0 Then
            MessageBox.Show("Silakan pilih bulan terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim formBuktiPotong As New pk_form_bukti_potong(Me)
        formBuktiPotong.EmployeeNPWP = Me.EmployeeNPWP
        formBuktiPotong.PTKPStatusValue = Me.PTKPStatusValue
        formBuktiPotong.SelectedMonth = Me.SelectedMonth
        formBuktiPotong.Show()
        Me.Hide()
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
        ' Already on bukti potong timeline, no action needed
    End Sub

    Private Sub Pk_navbar11_RiwayatClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.RiwayatClicked
        Dim formRiwayat As New pk_riwayat_bukti_potong()
        formRiwayat.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar11_LogoutClicked(sender As Object, e As EventArgs) Handles Pk_navbar11.LogoutClicked
        Dim result As DialogResult = MessageBox.Show(
            "Apakah Anda yakin ingin keluar?",
            "Konfirmasi Logout",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)
        
        If result = DialogResult.Yes Then
            ' TODO: Implement logout logic (return to login form)
            Application.Exit()
        End If
    End Sub

End Class