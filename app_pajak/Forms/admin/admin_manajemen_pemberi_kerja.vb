Imports MySql.Data.MySqlClient

Public Class FrmManagementPemberiKerja

    Private isLoading As Boolean = True ' Flag to prevent events during initialization

    Private Sub FrmManagementPemberiKerja_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set default combo selections (events will fire but isLoading prevents LoadPemberiKerja)
        CmbStatus.SelectedIndex = 0
        CmbSort.SelectedIndex = 0

        ' Now allow events to work
        isLoading = False

        ' Load data
        LoadStatistics()
        LoadPemberiKerja()

        ' Set active menu in navbar
        Pk_navbar1.SetActiveMenu(admin_navbar.MenuType.ManajemenPemberiKerja)
    End Sub

    ' ====== NAVBAR EVENT HANDLERS ======
    Private Sub Pk_navbar1_DashboardClicked(sender As Object, e As EventArgs) Handles Pk_navbar1.DashboardClicked
        Dim f As New admin_dashboard()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar1_ManajemenPemberiKerjaClicked(sender As Object, e As EventArgs) Handles Pk_navbar1.ManajemenPemberiKerjaClicked
        ' Already on this form, do nothing
    End Sub

    Private Sub Pk_navbar1_ManajemenUserClicked(sender As Object, e As EventArgs) Handles Pk_navbar1.ManajemenUserClicked
        Dim f As New FrmUserManagement()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar1_ManajemenPerusahaanClicked(sender As Object, e As EventArgs) Handles Pk_navbar1.ManajemenPerusahaanClicked
        Dim f As New FrmManagementPerusahaan()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar1_MasterPajakClicked(sender As Object, e As EventArgs) Handles Pk_navbar1.MasterPajakClicked
        Dim f As New admin_master_pajak()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Pk_navbar1_LogoutClicked(sender As Object, e As EventArgs) Handles Pk_navbar1.LogoutClicked
        ModuleSession.ClearSession()
        Dim f As New FrmLogin()
        f.Show()
        Me.Close()
    End Sub

    ''' <summary>
    ''' Load statistik pemberi kerja untuk dashboard cards
    ''' </summary>
    Private Sub LoadStatistics()
        Try
            modulkoneksi.BukaKoneksi()

            ' Total pemberi kerja
            Dim sqlTotal As String = "SELECT COUNT(*) FROM pemberi_kerja"
            Dim cmdTotal As New MySqlCommand(sqlTotal, modulkoneksi.koneksi)
            Dim totalPemberiKerja As Integer = Convert.ToInt32(cmdTotal.ExecuteScalar())
            LblTotalPemberiKerjaValue.Text = totalPemberiKerja.ToString()

            ' Pemberi kerja dengan user aktif
            Dim sqlActive As String = "SELECT COUNT(*) FROM pemberi_kerja pk INNER JOIN users u ON pk.user_id = u.id WHERE u.is_active = 1"
            Dim cmdActive As New MySqlCommand(sqlActive, modulkoneksi.koneksi)
            Dim activePemberiKerja As Integer = Convert.ToInt32(cmdActive.ExecuteScalar())
            LblActivePemberiKerjaValue.Text = activePemberiKerja.ToString()

            ' Pemberi kerja inactive
            Dim inactivePemberiKerja As Integer = totalPemberiKerja - activePemberiKerja
            LblInactivePemberiKerjaValue.Text = inactivePemberiKerja.ToString()

        Catch ex As Exception
            MsgBox("Error loading statistics: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    ''' <summary>
    ''' Load daftar pemberi kerja ke DataGridView
    ''' </summary>
    Private Sub LoadPemberiKerja(Optional searchKeyword As String = "", Optional sortBy As String = "Terbaru")
        Try
            modulkoneksi.BukaKoneksi()

            ' Clear existing rows
            GridPemberiKerja.Rows.Clear()

            Dim sql As String = "
                SELECT pk.id, pk.nama, pk.email, pk.no_telepon, 
                       p.nama_perusahaan, u.is_active, u.created_at
                FROM pemberi_kerja pk
                INNER JOIN users u ON pk.user_id = u.id
                INNER JOIN perusahaan p ON pk.perusahaan_id = p.id
                WHERE 1=1"

            If Not String.IsNullOrEmpty(searchKeyword) Then
                sql &= " AND (pk.nama LIKE @search OR pk.email LIKE @search OR p.nama_perusahaan LIKE @search)"
            End If

            ' Sorting
            Select Case sortBy
                Case "Terbaru", "Sortir: Terbaru"
                    sql &= " ORDER BY u.created_at DESC"
                Case "Terlama", "Sortir: Terlama"
                    sql &= " ORDER BY u.created_at ASC"
                Case "Nama A-Z"
                    sql &= " ORDER BY pk.nama ASC"
                Case "Nama Z-A"
                    sql &= " ORDER BY pk.nama DESC"
                Case Else
                    sql &= " ORDER BY u.created_at DESC"
            End Select

            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)

            If Not String.IsNullOrEmpty(searchKeyword) Then
                cmd.Parameters.AddWithValue("@search", "%" & searchKeyword & "%")
            End If

            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            Dim rowNum As Integer = 1
            While rd.Read()
                Dim id As Integer = Convert.ToInt32(rd("id"))
                Dim nama As String = rd("nama").ToString()
                Dim email As String = rd("email").ToString()
                Dim noTelepon As String = If(IsDBNull(rd("no_telepon")), "-", rd("no_telepon").ToString())
                Dim namaPerusahaan As String = rd("nama_perusahaan").ToString()
                Dim isActive As Boolean = Convert.ToBoolean(rd("is_active"))
                Dim tanggal As String = If(IsDBNull(rd("created_at")), "-", Convert.ToDateTime(rd("created_at")).ToString("dd/MM/yyyy"))

                ' Determine status based on is_active
                Dim status As String = If(isActive, "Aktif", "Nonaktif")

                ' Filter by status if selected
                Dim statusFilter As String = If(CmbStatus.SelectedItem IsNot Nothing, CmbStatus.SelectedItem.ToString(), "Semua Status")
                If statusFilter <> "Semua Status" Then
                    If statusFilter = "Active" AndAlso Not isActive Then Continue While
                    If statusFilter = "Inactive" AndAlso isActive Then Continue While
                End If

                GridPemberiKerja.Rows.Add(rowNum, nama, email, namaPerusahaan, noTelepon, status, tanggal)
                GridPemberiKerja.Rows(GridPemberiKerja.Rows.Count - 1).Tag = id ' Store ID in tag

                rowNum += 1
            End While

            rd.Close()

            ' Update table subtitle with count
            LblTableSubtitle.Text = $"Menampilkan {rowNum - 1} pemberi kerja terdaftar."

        Catch ex As Exception
            MsgBox("Error loading pemberi kerja: " & ex.Message, MsgBoxStyle.Critical)
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
        LoadPemberiKerja(TxtSearch.Text, sortBy)
    End Sub

    ''' <summary>
    ''' Handle status filter change
    ''' </summary>
    Private Sub CmbStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbStatus.SelectedIndexChanged
        If isLoading Then Return
        Dim sortBy As String = If(CmbSort.SelectedItem IsNot Nothing, CmbSort.SelectedItem.ToString(), "Terbaru")
        LoadPemberiKerja(TxtSearch.Text, sortBy)
    End Sub

    ''' <summary>
    ''' Handle sort change
    ''' </summary>
    Private Sub CmbSort_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbSort.SelectedIndexChanged
        If isLoading Then Return
        Dim sortBy As String = If(CmbSort.SelectedItem IsNot Nothing, CmbSort.SelectedItem.ToString(), "Terbaru")
        LoadPemberiKerja(TxtSearch.Text, sortBy)
    End Sub

    ''' <summary>
    ''' Handle grid cell click for Edit and Delete buttons
    ''' </summary>
    Private Sub GridPemberiKerja_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles GridPemberiKerja.CellContentClick
        If e.RowIndex < 0 Then Return

        Dim pemberiKerjaId As Integer = Convert.ToInt32(GridPemberiKerja.Rows(e.RowIndex).Tag)
        Dim namaPemberiKerja As String = GridPemberiKerja.Rows(e.RowIndex).Cells("colNama").Value.ToString()

        ' Check if clicked on Edit column
        If e.ColumnIndex = GridPemberiKerja.Columns("colActions").Index Then
            ShowEditPemberiKerjaDialog(pemberiKerjaId)
        End If

        ' Check if clicked on Delete column
        If e.ColumnIndex = GridPemberiKerja.Columns("colDelete").Index Then
            Dim result As DialogResult = MessageBox.Show(
                $"Apakah Anda yakin ingin menghapus pemberi kerja '{namaPemberiKerja}'?" & vbCrLf & vbCrLf &
                "⚠️ Tindakan ini tidak dapat dibatalkan!" & vbCrLf &
                "Akun user terkait akan ikut dihapus.",
                "Konfirmasi Hapus",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            )

            If result = DialogResult.Yes Then
                DeletePemberiKerja(pemberiKerjaId)
            End If
        End If
    End Sub

    Private Sub ShowEditPemberiKerjaDialog(pemberiKerjaId As Integer)
        Try
            modulkoneksi.BukaKoneksi()

            ' Get pemberi kerja data
            Dim sql As String = "SELECT nama, email, no_telepon FROM pemberi_kerja WHERE id = @id"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            cmd.Parameters.AddWithValue("@id", pemberiKerjaId)
            Dim reader = cmd.ExecuteReader()

            Dim pemberiKerjaData As New Dictionary(Of String, String)
            If reader.Read() Then
                pemberiKerjaData("nama") = If(reader.IsDBNull(0), "", reader.GetString(0))
                pemberiKerjaData("email") = If(reader.IsDBNull(1), "", reader.GetString(1))
                pemberiKerjaData("no_telepon") = If(reader.IsDBNull(2), "", reader.GetString(2))
            End If
            reader.Close()
            modulkoneksi.TutupKoneksi()

            ' Create edit dialog
            Using dialog As New Form()
                dialog.Text = "Edit Pemberi Kerja"
                dialog.Size = New Size(450, 260)
                dialog.StartPosition = FormStartPosition.CenterParent
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog
                dialog.MaximizeBox = False
                dialog.MinimizeBox = False
                dialog.BackColor = Color.FromArgb(247, 248, 252)

                Dim yPos As Integer = 20

                ' Nama
                Dim lblNama As New Label() With {.Text = "Nama:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
                Dim txtNama As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 250, .Font = New Font("Segoe UI", 9), .Text = pemberiKerjaData("nama")}
                dialog.Controls.Add(lblNama)
                dialog.Controls.Add(txtNama)
                yPos += 35

                ' Email
                Dim lblEmail As New Label() With {.Text = "Email:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
                Dim txtEmail As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 250, .Font = New Font("Segoe UI", 9), .Text = pemberiKerjaData("email")}
                dialog.Controls.Add(lblEmail)
                dialog.Controls.Add(txtEmail)
                yPos += 35

                ' No Telepon
                Dim lblTelepon As New Label() With {.Text = "No Telepon:", .Location = New Point(20, yPos), .Width = 120, .Font = New Font("Segoe UI", 9)}
                Dim txtTelepon As New TextBox() With {.Location = New Point(150, yPos - 3), .Width = 200, .Font = New Font("Segoe UI", 9), .Text = pemberiKerjaData("no_telepon")}
                dialog.Controls.Add(lblTelepon)
                dialog.Controls.Add(txtTelepon)
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
                        Dim updateSql As String = "UPDATE pemberi_kerja SET nama = @nama, email = @email, no_telepon = @no_telepon WHERE id = @id"
                        Dim updateCmd As New MySqlCommand(updateSql, modulkoneksi.koneksi)
                        updateCmd.Parameters.AddWithValue("@id", pemberiKerjaId)
                        updateCmd.Parameters.AddWithValue("@nama", txtNama.Text)
                        updateCmd.Parameters.AddWithValue("@email", txtEmail.Text)
                        updateCmd.Parameters.AddWithValue("@no_telepon", If(String.IsNullOrWhiteSpace(txtTelepon.Text), DBNull.Value, txtTelepon.Text))
                        updateCmd.ExecuteNonQuery()

                        MsgBox("Data pemberi kerja berhasil diperbarui!", MsgBoxStyle.Information)
                        LoadPemberiKerja()
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

    Private Sub DeletePemberiKerja(pemberiKerjaId As Integer)
        Try
            modulkoneksi.BukaKoneksi()

            ' Get user_id for deletion
            Dim getUserIdSql As String = "SELECT user_id FROM pemberi_kerja WHERE id = @id"
            Dim getUserIdCmd As New MySqlCommand(getUserIdSql, modulkoneksi.koneksi)
            getUserIdCmd.Parameters.AddWithValue("@id", pemberiKerjaId)
            Dim userId As Integer = Convert.ToInt32(getUserIdCmd.ExecuteScalar())

            ' Delete pemberi_kerja (user will cascade delete due to FK)
            Dim cmdUser As New MySqlCommand("DELETE FROM users WHERE id = @id", modulkoneksi.koneksi)
            cmdUser.Parameters.AddWithValue("@id", userId)
            cmdUser.ExecuteNonQuery()

            MsgBox("Pemberi kerja berhasil dihapus!", MsgBoxStyle.Information)
            LoadStatistics()
            LoadPemberiKerja()

        Catch ex As Exception
            MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

End Class
