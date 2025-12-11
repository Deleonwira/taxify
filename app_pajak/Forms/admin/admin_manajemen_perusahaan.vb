Imports MySql.Data.MySqlClient

Public Class FrmManagementPerusahaan

    Private isLoading As Boolean = True ' Flag to prevent events during initialization

    Private Sub FrmManagementPerusahaan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set default combo selections (events will fire but isLoading prevents LoadPerusahaan)
        CmbStatus.SelectedIndex = 0
        CmbSort.SelectedIndex = 0

        ' Now allow events to work
        isLoading = False

        ' Load data
        LoadStatistics()
        LoadPerusahaan()
        
        ' Set active menu in navbar
        Pk_navbar1.SetActiveMenu(admin_navbar.MenuType.ManajemenPerusahaan)
    End Sub

    ' ====== NAVBAR EVENT HANDLERS ======
    Private Sub Pk_navbar1_DashboardClicked(sender As Object, e As EventArgs) Handles Pk_navbar1.DashboardClicked
        Dim f As New admin_dashboard()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar1_ValidasiRegistrasiClicked(sender As Object, e As EventArgs) Handles Pk_navbar1.ValidasiRegistrasiClicked
        Dim f As New admin_validasi_registrasi()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar1_ManajemenUserClicked(sender As Object, e As EventArgs) Handles Pk_navbar1.ManajemenUserClicked
        Dim f As New FrmUserManagement()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar1_ManajemenPerusahaanClicked(sender As Object, e As EventArgs) Handles Pk_navbar1.ManajemenPerusahaanClicked
        ' Already on this form, do nothing
    End Sub

    Private Sub Pk_navbar1_LogoutClicked(sender As Object, e As EventArgs) Handles Pk_navbar1.LogoutClicked
        ModuleSession.ClearSession()
        Dim f As New FrmLogin()
        f.Show()
        Me.Close()
    End Sub

    ''' <summary>
    ''' Load statistik perusahaan untuk dashboard cards
    ''' </summary>
    Private Sub LoadStatistics()
        Try
            modulkoneksi.BukaKoneksi()

            ' Total perusahaan
            Dim sqlTotal As String = "SELECT COUNT(*) FROM perusahaan"
            Dim cmdTotal As New MySqlCommand(sqlTotal, modulkoneksi.koneksi)
            Dim totalPerusahaan As Integer = Convert.ToInt32(cmdTotal.ExecuteScalar())
            LblTotalPerusahaanValue.Text = totalPerusahaan.ToString()

            ' Perusahaan dengan pegawai (Active)
            Dim sqlActive As String = "SELECT COUNT(DISTINCT p.perusahaan_id) FROM pekerjaan p"
            Dim cmdActive As New MySqlCommand(sqlActive, modulkoneksi.koneksi)
            Dim activePerusahaan As Integer = Convert.ToInt32(cmdActive.ExecuteScalar())
            LblActivePerusahaanValue.Text = activePerusahaan.ToString()

            ' Perusahaan tanpa pegawai (baru terdaftar)
            Dim inactivePerusahaan As Integer = totalPerusahaan - activePerusahaan
            LblInactivePerusahaanValue.Text = inactivePerusahaan.ToString()

            ' Perusahaan freelance/tidak terdaftar (id=2 based on schema)
            LblPendingPerusahaanValue.Text = "1" ' Freelance placeholder

        Catch ex As Exception
            MsgBox("Error loading statistics: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ''' <summary>
    ''' Load daftar perusahaan ke DataGridView
    ''' </summary>
    Private Sub LoadPerusahaan(Optional searchKeyword As String = "", Optional sortBy As String = "Terbaru")
        Try
            modulkoneksi.BukaKoneksi()

            ' Clear existing rows
            GridPerusahaan.Rows.Clear()

            Dim sql As String = "
                SELECT p.id, p.nama_perusahaan, p.npwp_perusahaan, p.kota, p.created_at,
                       COUNT(pk.id) AS jumlah_pegawai
                FROM perusahaan p
                LEFT JOIN pekerjaan pk ON p.id = pk.perusahaan_id
                WHERE 1=1"

            If Not String.IsNullOrEmpty(searchKeyword) Then
                sql &= " AND (p.nama_perusahaan LIKE @search OR p.npwp_perusahaan LIKE @search)"
            End If

            sql &= " GROUP BY p.id, p.nama_perusahaan, p.npwp_perusahaan, p.kota, p.created_at"

            ' Sorting
            Select Case sortBy
                Case "Terbaru", "Sortir: Terbaru"
                    sql &= " ORDER BY p.created_at DESC"
                Case "Terlama", "Sortir: Terlama"
                    sql &= " ORDER BY p.created_at ASC"
                Case "Nama A-Z"
                    sql &= " ORDER BY p.nama_perusahaan ASC"
                Case "Nama Z-A"
                    sql &= " ORDER BY p.nama_perusahaan DESC"
                Case Else
                    sql &= " ORDER BY p.created_at DESC"
            End Select

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)

            If Not String.IsNullOrEmpty(searchKeyword) Then
                cmd.Parameters.AddWithValue("@search", "%" & searchKeyword & "%")
            End If

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            Dim rowNum As Integer = 1
            While rd.Read()
                Dim id As Integer = Convert.ToInt32(rd("id"))
                Dim namaPerusahaan As String = rd("nama_perusahaan").ToString()
                Dim npwp As String = If(IsDBNull(rd("npwp_perusahaan")), "-", rd("npwp_perusahaan").ToString())
                Dim jumlahPegawai As Integer = Convert.ToInt32(rd("jumlah_pegawai"))
                Dim tanggal As String = If(IsDBNull(rd("created_at")), "-", Convert.ToDateTime(rd("created_at")).ToString("dd/MM/yyyy"))

                ' Determine status based on pegawai count
                Dim status As String = If(jumlahPegawai > 0, "Aktif", "Belum Ada Pegawai")

                ' Filter by status if selected
                Dim statusFilter As String = If(CmbStatus.SelectedItem IsNot Nothing, CmbStatus.SelectedItem.ToString(), "Semua Status")
                If statusFilter <> "Semua Status" Then
                    If statusFilter = "Active" AndAlso jumlahPegawai = 0 Then Continue While
                    If statusFilter = "Inactive" AndAlso jumlahPegawai > 0 Then Continue While
                End If

                GridPerusahaan.Rows.Add(rowNum, namaPerusahaan, npwp, status, tanggal)
                GridPerusahaan.Rows(GridPerusahaan.Rows.Count - 1).Tag = id ' Store ID in tag

                rowNum += 1
            End While

            rd.Close()

            ' Update table subtitle with count
            LblTableSubtitle.Text = $"Menampilkan {rowNum - 1} perusahaan terdaftar."

        Catch ex As Exception
            MsgBox("Error loading perusahaan: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ''' <summary>
    ''' Handle search text change
    ''' </summary>
    Private Sub TxtSearch_TextChanged(sender As Object, e As EventArgs) Handles TxtSearch.TextChanged
        If isLoading Then Return
        Dim sortBy As String = If(CmbSort.SelectedItem IsNot Nothing, CmbSort.SelectedItem.ToString(), "Terbaru")
        LoadPerusahaan(TxtSearch.Text, sortBy)
    End Sub

    ''' <summary>
    ''' Handle status filter change
    ''' </summary>
    Private Sub CmbStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbStatus.SelectedIndexChanged
        If isLoading Then Return
        Dim sortBy As String = If(CmbSort.SelectedItem IsNot Nothing, CmbSort.SelectedItem.ToString(), "Terbaru")
        LoadPerusahaan(TxtSearch.Text, sortBy)
    End Sub

    ''' <summary>
    ''' Handle sort change
    ''' </summary>
    Private Sub CmbSort_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbSort.SelectedIndexChanged
        If isLoading Then Return
        Dim sortBy As String = If(CmbSort.SelectedItem IsNot Nothing, CmbSort.SelectedItem.ToString(), "Terbaru")
        LoadPerusahaan(TxtSearch.Text, sortBy)
    End Sub

    ''' <summary>
    ''' Handle Add Perusahaan button click
    ''' </summary>
    Private Sub BtnAddPerusahaan_Click(sender As Object, e As EventArgs) Handles BtnAddPerusahaan.Click
        ' Show input dialog for new perusahaan
        Dim namaPerusahaan As String = InputBox("Masukkan nama perusahaan:", "Tambah Perusahaan Baru")
        If String.IsNullOrWhiteSpace(namaPerusahaan) Then Return

        Dim npwpPerusahaan As String = InputBox("Masukkan NPWP perusahaan (opsional):", "Tambah Perusahaan Baru")
        Dim alamat As String = InputBox("Masukkan alamat perusahaan (opsional):", "Tambah Perusahaan Baru")
        Dim kota As String = InputBox("Masukkan kota perusahaan (opsional):", "Tambah Perusahaan Baru")

        Try
            modulkoneksi.BukaKoneksi()

            Dim sql As String = "INSERT INTO perusahaan (nama_perusahaan, npwp_perusahaan, alamat, kota) VALUES (@nama, @npwp, @alamat, @kota)"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@nama", namaPerusahaan)
            cmd.Parameters.AddWithValue("@npwp", If(String.IsNullOrWhiteSpace(npwpPerusahaan), DBNull.Value, npwpPerusahaan))
            cmd.Parameters.AddWithValue("@alamat", If(String.IsNullOrWhiteSpace(alamat), DBNull.Value, alamat))
            cmd.Parameters.AddWithValue("@kota", If(String.IsNullOrWhiteSpace(kota), DBNull.Value, kota))
            cmd.ExecuteNonQuery()

            MsgBox("Perusahaan berhasil ditambahkan!", MsgBoxStyle.Information)
            LoadStatistics()
            LoadPerusahaan()

        Catch ex As Exception
            MsgBox("Error menambah perusahaan: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ''' <summary>
    ''' Handle grid cell click for Edit and Delete buttons
    ''' </summary>
    Private Sub GridPerusahaan_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles GridPerusahaan.CellContentClick
        If e.RowIndex < 0 Then Return

        Dim perusahaanId As Integer = Convert.ToInt32(GridPerusahaan.Rows(e.RowIndex).Tag)
        Dim namaPerusahaan As String = GridPerusahaan.Rows(e.RowIndex).Cells("colNamaPerusahaan").Value.ToString()

        ' Check if clicked on Edit column
        If e.ColumnIndex = GridPerusahaan.Columns("colActions").Index Then
            ShowEditPerusahaanDialog(perusahaanId)
        End If

        ' Check if clicked on Delete column
        If e.ColumnIndex = GridPerusahaan.Columns("colDelete").Index Then
            Dim result As DialogResult = MessageBox.Show(
                $"Apakah Anda yakin ingin menghapus perusahaan '{namaPerusahaan}'?" & vbCrLf & vbCrLf &
                "⚠️ Tindakan ini tidak dapat dibatalkan!" & vbCrLf &
                "Semua data pegawai terkait akan ikut dihapus.",
                "Konfirmasi Hapus",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            )

            If result = DialogResult.Yes Then
                DeletePerusahaan(perusahaanId)
            End If
        End If
    End Sub

    Private Sub ShowEditPerusahaanDialog(perusahaanId As Integer)
        Try
            modulkoneksi.BukaKoneksi()

            ' Get perusahaan data
            Dim sql As String = "SELECT nama_perusahaan, npwp_perusahaan, alamat, kota FROM perusahaan WHERE id = @id"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@id", perusahaanId)
            Dim reader = cmd.ExecuteReader()

            Dim perusahaanData As New Dictionary(Of String, String)
            If reader.Read() Then
                perusahaanData("nama_perusahaan") = If(reader.IsDBNull(0), "", reader.GetString(0))
                perusahaanData("npwp_perusahaan") = If(reader.IsDBNull(1), "", reader.GetString(1))
                perusahaanData("alamat") = If(reader.IsDBNull(2), "", reader.GetString(2))
                perusahaanData("kota") = If(reader.IsDBNull(3), "", reader.GetString(3))
            End If
            reader.Close()
            modulkoneksi.TutupKoneksi()

            ' Create edit dialog
            Using dialog As New Form()
                dialog.Text = "Edit Perusahaan"
                dialog.Size = New Size(450, 320)
                dialog.StartPosition = FormStartPosition.CenterParent
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog
                dialog.MaximizeBox = False
                dialog.MinimizeBox = False
                dialog.BackColor = Color.FromArgb(247, 248, 252)

                Dim yPos As Integer = 20

                ' Nama Perusahaan
                Dim lblNama As New Label() With {.Text = "Nama Perusahaan:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
                Dim txtNama As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 250, .Font = New Font("Segoe UI", 9), .Text = perusahaanData("nama_perusahaan")}
                dialog.Controls.Add(lblNama)
                dialog.Controls.Add(txtNama)
                yPos += 35

                ' NPWP Perusahaan
                Dim lblNpwp As New Label() With {.Text = "NPWP:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
                Dim txtNpwp As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 200, .Font = New Font("Segoe UI", 9), .Text = perusahaanData("npwp_perusahaan")}
                dialog.Controls.Add(lblNpwp)
                dialog.Controls.Add(txtNpwp)
                yPos += 35

                ' Alamat
                Dim lblAlamat As New Label() With {.Text = "Alamat:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
                Dim txtAlamat As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 250, .Font = New Font("Segoe UI", 9), .Text = perusahaanData("alamat")}
                dialog.Controls.Add(lblAlamat)
                dialog.Controls.Add(txtAlamat)
                yPos += 35

                ' Kota
                Dim lblKota As New Label() With {.Text = "Kota:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
                Dim txtKota As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 200, .Font = New Font("Segoe UI", 9), .Text = perusahaanData("kota")}
                dialog.Controls.Add(lblKota)
                dialog.Controls.Add(txtKota)
                yPos += 50

                ' Buttons
                Dim btnSave As New Button() With {
                    .Text = "Simpan",
                    .Location = New Point(150, yPos),
                    .Size = New Size(100, 35),
                    .BackColor = Color.FromArgb(156, 0, 219),
                    .ForeColor = Color.White,
                    .FlatStyle = FlatStyle.Flat,
                    .Font = New Font("Segoe UI Semibold", 9, FontStyle.Bold),
                    .DialogResult = DialogResult.OK
                }
                btnSave.FlatAppearance.BorderSize = 0

                Dim btnCancel As New Button() With {
                    .Text = "Batal",
                    .Location = New Point(260, yPos),
                    .Size = New Size(100, 35),
                    .BackColor = Color.FromArgb(120, 128, 146),
                    .ForeColor = Color.White,
                    .FlatStyle = FlatStyle.Flat,
                    .Font = New Font("Segoe UI Semibold", 9, FontStyle.Bold),
                    .DialogResult = DialogResult.Cancel
                }
                btnCancel.FlatAppearance.BorderSize = 0

                dialog.Controls.Add(btnSave)
                dialog.Controls.Add(btnCancel)
                dialog.AcceptButton = btnSave
                dialog.CancelButton = btnCancel

                If dialog.ShowDialog() = DialogResult.OK Then
                    ' Save changes
                    Try
                        modulkoneksi.BukaKoneksi()
                        Dim updateSql As String = "UPDATE perusahaan SET nama_perusahaan = @nama, npwp_perusahaan = @npwp, alamat = @alamat, kota = @kota WHERE id = @id"
                        Dim updateCmd As New MySqlCommand(updateSql, modulkoneksi.koneksi)
                        updateCmd.Parameters.AddWithValue("@id", perusahaanId)
                        updateCmd.Parameters.AddWithValue("@nama", txtNama.Text)
                        updateCmd.Parameters.AddWithValue("@npwp", If(String.IsNullOrWhiteSpace(txtNpwp.Text), DBNull.Value, txtNpwp.Text))
                        updateCmd.Parameters.AddWithValue("@alamat", If(String.IsNullOrWhiteSpace(txtAlamat.Text), DBNull.Value, txtAlamat.Text))
                        updateCmd.Parameters.AddWithValue("@kota", If(String.IsNullOrWhiteSpace(txtKota.Text), DBNull.Value, txtKota.Text))
                        updateCmd.ExecuteNonQuery()

                        MsgBox("Data perusahaan berhasil diperbarui!", MsgBoxStyle.Information)
                        LoadPerusahaan()
                    Catch ex As Exception
                        MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
                    Finally
                        modulkoneksi.TutupKoneksi()
                    End Try
                End If
            End Using

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub DeletePerusahaan(perusahaanId As Integer)
        Try
            modulkoneksi.BukaKoneksi()

            ' Delete related pekerjaan first (FK constraint)
            Dim cmdPekerjaan As New MySqlCommand("DELETE FROM pekerjaan WHERE perusahaan_id = @id", modulkoneksi.koneksi)
            cmdPekerjaan.Parameters.AddWithValue("@id", perusahaanId)
            cmdPekerjaan.ExecuteNonQuery()

            ' Delete pemberi_kerja related to this perusahaan
            Dim cmdPk As New MySqlCommand("DELETE FROM pemberi_kerja WHERE perusahaan_id = @id", modulkoneksi.koneksi)
            cmdPk.Parameters.AddWithValue("@id", perusahaanId)
            cmdPk.ExecuteNonQuery()

            ' Delete perusahaan
            Dim cmdPerusahaan As New MySqlCommand("DELETE FROM perusahaan WHERE id = @id", modulkoneksi.koneksi)
            cmdPerusahaan.Parameters.AddWithValue("@id", perusahaanId)
            cmdPerusahaan.ExecuteNonQuery()

            MsgBox("Perusahaan berhasil dihapus!", MsgBoxStyle.Information)
            LoadStatistics()
            LoadPerusahaan()

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

End Class
