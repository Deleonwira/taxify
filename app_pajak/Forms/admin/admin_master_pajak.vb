Imports MySql.Data.MySqlClient
Imports Guna.UI2.WinForms

Public Class admin_master_pajak

    Private Sub admin_master_pajak_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set active menu in navbar
        Admin_navbar1.SetActiveMenu(admin_navbar.MenuType.MasterPajak)
        
        LoadPTKPData()
        LoadTarifData()
    End Sub

    ' ====== NAVBAR EVENT HANDLERS ======
    Private Sub Admin_navbar1_DashboardClicked(sender As Object, e As EventArgs) Handles Admin_navbar1.DashboardClicked
        Dim f As New admin_dashboard()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Admin_navbar1_ManajemenPemberiKerjaClicked(sender As Object, e As EventArgs) Handles Admin_navbar1.ManajemenPemberiKerjaClicked
        Dim f As New FrmManagementPemberiKerja()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Admin_navbar1_ManajemenUserClicked(sender As Object, e As EventArgs) Handles Admin_navbar1.ManajemenUserClicked
        Dim f As New FrmUserManagement()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Admin_navbar1_ManajemenPerusahaanClicked(sender As Object, e As EventArgs) Handles Admin_navbar1.ManajemenPerusahaanClicked
        Dim f As New FrmManagementPerusahaan()
        f.Show()
        Me.Close()
    End Sub

    Private Sub Admin_navbar1_MasterPajakClicked(sender As Object, e As EventArgs) Handles Admin_navbar1.MasterPajakClicked
        ' Already on this form, do nothing
    End Sub

    Private Sub Admin_navbar1_LogoutClicked(sender As Object, e As EventArgs) Handles Admin_navbar1.LogoutClicked
        ModuleSession.ClearSession()
        Dim f As New FrmLogin()
        f.Show()
        Me.Close()
    End Sub

    ' =============================================
    ' PTKP SECTION
    ' =============================================

    Private Sub LoadPTKPData()
        Try
            modulkoneksi.BukaKoneksi()
            GridPTKP.Rows.Clear()

            Dim sql As String = "SELECT id, kode_status, keterangan, nilai_tahunan FROM master_ptkp WHERE is_active = 1 ORDER BY kode_status"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            Dim rowNum As Integer = 1
            While rd.Read()
                Dim id As Integer = Convert.ToInt32(rd("id"))
                Dim kode As String = rd("kode_status").ToString()
                Dim keterangan As String = rd("keterangan").ToString()
                Dim nilai As Decimal = Convert.ToDecimal(rd("nilai_tahunan"))

                GridPTKP.Rows.Add(rowNum, kode, keterangan, "Rp " & nilai.ToString("N0"))
                GridPTKP.Rows(GridPTKP.Rows.Count - 1).Tag = id
                rowNum += 1
            End While

            rd.Close()
        Catch ex As Exception
            MessageBox.Show("Error loading PTKP data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub BtnAddPTKP_Click(sender As Object, e As EventArgs) Handles BtnAddPTKP.Click
        ShowPTKPDialog(0)
    End Sub

    Private Sub GridPTKP_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles GridPTKP.CellContentClick
        If e.RowIndex < 0 Then Return

        If e.ColumnIndex = GridPTKP.Columns("colPTKPEdit").Index Then
            Dim id As Integer = Convert.ToInt32(GridPTKP.Rows(e.RowIndex).Tag)
            ShowPTKPDialog(id)
        End If
    End Sub

    Private Sub ShowPTKPDialog(ptkpId As Integer)
        Dim isEdit As Boolean = (ptkpId > 0)
        Dim dialogTitle As String = If(isEdit, "Edit PTKP", "Tambah PTKP Baru")

        ' Get existing data if editing
        Dim kode As String = ""
        Dim keterangan As String = ""
        Dim nilai As Decimal = 0

        If isEdit Then
            Try
                modulkoneksi.BukaKoneksi()
                Dim sql As String = "SELECT kode_status, keterangan, nilai_tahunan FROM master_ptkp WHERE id = @id"
                Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
                cmd.Parameters.AddWithValue("@id", ptkpId)
                Dim rd = cmd.ExecuteReader()

                If rd.Read() Then
                    kode = rd("kode_status").ToString()
                    keterangan = rd("keterangan").ToString()
                    nilai = Convert.ToDecimal(rd("nilai_tahunan"))
                End If
                rd.Close()
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
                Return
            Finally
                modulkoneksi.TutupKoneksi()
            End Try
        End If

        ' Create dialog form
        Dim dialog As New Form()
        dialog.Text = dialogTitle
        dialog.Size = New Size(400, 280)
        dialog.StartPosition = FormStartPosition.CenterParent
        dialog.FormBorderStyle = FormBorderStyle.FixedDialog
        dialog.MaximizeBox = False
        dialog.MinimizeBox = False
        dialog.BackColor = Color.White

        ' Labels and inputs
        Dim lblKode As New Label() With {.Text = "Kode Status:", .Location = New Point(20, 20), .AutoSize = True}
        Dim txtKode As New TextBox() With {.Location = New Point(20, 45), .Width = 340, .Text = kode}
        txtKode.Enabled = Not isEdit ' Disable editing kode for existing records

        Dim lblKeterangan As New Label() With {.Text = "Keterangan:", .Location = New Point(20, 80), .AutoSize = True}
        Dim txtKeterangan As New TextBox() With {.Location = New Point(20, 105), .Width = 340, .Text = keterangan}

        Dim lblNilai As New Label() With {.Text = "Nilai Tahunan (Rp):", .Location = New Point(20, 140), .AutoSize = True}
        Dim txtNilai As New TextBox() With {.Location = New Point(20, 165), .Width = 340, .Text = nilai.ToString("N0")}

        Dim btnSave As New Button() With {
            .Text = "Simpan",
            .Location = New Point(180, 210),
            .Width = 80,
            .BackColor = Color.FromArgb(156, 0, 219),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }
        btnSave.FlatAppearance.BorderSize = 0

        Dim btnCancel As New Button() With {
            .Text = "Batal",
            .Location = New Point(280, 210),
            .Width = 80
        }

        dialog.Controls.AddRange({lblKode, txtKode, lblKeterangan, txtKeterangan, lblNilai, txtNilai, btnSave, btnCancel})

        AddHandler btnCancel.Click, Sub() dialog.Close()

        AddHandler btnSave.Click, Sub()
                                      If String.IsNullOrWhiteSpace(txtKode.Text) OrElse String.IsNullOrWhiteSpace(txtKeterangan.Text) Then
                                          MessageBox.Show("Kode dan Keterangan harus diisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                          Return
                                      End If

                                      Dim nilaiParsed As Decimal
                                      If Not Decimal.TryParse(txtNilai.Text.Replace(",", "").Replace(".", ""), nilaiParsed) Then
                                          MessageBox.Show("Nilai tidak valid!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                          Return
                                      End If

                                      Try
                                          modulkoneksi.BukaKoneksi()
                                          Dim cmdSave As New MySqlCommand()
                                          cmdSave.Connection = modulkoneksi.koneksi

                                          If isEdit Then
                                              cmdSave.CommandText = "UPDATE master_ptkp SET keterangan = @ket, nilai_tahunan = @nilai WHERE id = @id"
                                              cmdSave.Parameters.AddWithValue("@id", ptkpId)
                                          Else
                                              cmdSave.CommandText = "INSERT INTO master_ptkp (kode_status, keterangan, nilai_tahunan) VALUES (@kode, @ket, @nilai)"
                                              cmdSave.Parameters.AddWithValue("@kode", txtKode.Text.Trim().ToUpper())
                                          End If

                                          cmdSave.Parameters.AddWithValue("@ket", txtKeterangan.Text.Trim())
                                          cmdSave.Parameters.AddWithValue("@nilai", nilaiParsed)
                                          cmdSave.ExecuteNonQuery()

                                          MessageBox.Show("Data PTKP berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                          dialog.Close()
                                          LoadPTKPData()
                                      Catch ex As Exception
                                          MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                      Finally
                                          modulkoneksi.TutupKoneksi()
                                      End Try
                                  End Sub

        dialog.ShowDialog()
    End Sub

    ' =============================================
    ' TARIF SECTION
    ' =============================================

    Private Sub LoadTarifData()
        Try
            modulkoneksi.BukaKoneksi()
            GridTarif.Rows.Clear()

            Dim sql As String = "SELECT id, lapisan, batas_bawah, batas_atas, tarif_persen FROM master_tarif_pph WHERE is_active = 1 ORDER BY lapisan"
            Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
            Dim rd As MySqlDataReader = cmd.ExecuteReader()

            Dim rowNum As Integer = 1
            While rd.Read()
                Dim id As Integer = Convert.ToInt32(rd("id"))
                Dim lapisan As Integer = Convert.ToInt32(rd("lapisan"))
                Dim bawah As Decimal = Convert.ToDecimal(rd("batas_bawah"))
                Dim atas As Decimal = Convert.ToDecimal(rd("batas_atas"))
                Dim tarif As Decimal = Convert.ToDecimal(rd("tarif_persen"))

                GridTarif.Rows.Add(rowNum, lapisan, "Rp " & bawah.ToString("N0"), "Rp " & atas.ToString("N0"), tarif.ToString("F1") & "%")
                GridTarif.Rows(GridTarif.Rows.Count - 1).Tag = id
                rowNum += 1
            End While

            rd.Close()
        Catch ex As Exception
            MessageBox.Show("Error loading Tarif data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            modulkoneksi.TutupKoneksi()
        End Try
    End Sub

    Private Sub BtnAddTarif_Click(sender As Object, e As EventArgs) Handles BtnAddTarif.Click
        ShowTarifDialog(0)
    End Sub

    Private Sub GridTarif_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles GridTarif.CellContentClick
        If e.RowIndex < 0 Then Return

        If e.ColumnIndex = GridTarif.Columns("colTarifEdit").Index Then
            Dim id As Integer = Convert.ToInt32(GridTarif.Rows(e.RowIndex).Tag)
            ShowTarifDialog(id)
        End If
    End Sub

    Private Sub ShowTarifDialog(tarifId As Integer)
        Dim isEdit As Boolean = (tarifId > 0)
        Dim dialogTitle As String = If(isEdit, "Edit Tarif PPh", "Tambah Tarif PPh Baru")

        ' Get existing data if editing
        Dim lapisan As Integer = 0
        Dim bawah As Decimal = 0
        Dim atas As Decimal = 0
        Dim tarif As Decimal = 0
        Dim keterangan As String = ""

        If isEdit Then
            Try
                modulkoneksi.BukaKoneksi()
                Dim sql As String = "SELECT lapisan, batas_bawah, batas_atas, tarif_persen, keterangan FROM master_tarif_pph WHERE id = @id"
                Dim cmd As New MySqlCommand(sql, modulkoneksi.koneksi)
                cmd.Parameters.AddWithValue("@id", tarifId)
                Dim rd = cmd.ExecuteReader()

                If rd.Read() Then
                    lapisan = Convert.ToInt32(rd("lapisan"))
                    bawah = Convert.ToDecimal(rd("batas_bawah"))
                    atas = Convert.ToDecimal(rd("batas_atas"))
                    tarif = Convert.ToDecimal(rd("tarif_persen"))
                    keterangan = If(IsDBNull(rd("keterangan")), "", rd("keterangan").ToString())
                End If
                rd.Close()
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
                Return
            Finally
                modulkoneksi.TutupKoneksi()
            End Try
        End If

        ' Create dialog form
        Dim dialog As New Form()
        dialog.Text = dialogTitle
        dialog.Size = New Size(400, 350)
        dialog.StartPosition = FormStartPosition.CenterParent
        dialog.FormBorderStyle = FormBorderStyle.FixedDialog
        dialog.MaximizeBox = False
        dialog.MinimizeBox = False
        dialog.BackColor = Color.White

        ' Labels and inputs
        Dim lblLapisan As New Label() With {.Text = "Lapisan:", .Location = New Point(20, 20), .AutoSize = True}
        Dim txtLapisan As New TextBox() With {.Location = New Point(20, 45), .Width = 100, .Text = lapisan.ToString()}

        Dim lblBawah As New Label() With {.Text = "Batas Bawah (Rp):", .Location = New Point(20, 80), .AutoSize = True}
        Dim txtBawah As New TextBox() With {.Location = New Point(20, 105), .Width = 340, .Text = bawah.ToString("N0")}

        Dim lblAtas As New Label() With {.Text = "Batas Atas (Rp):", .Location = New Point(20, 140), .AutoSize = True}
        Dim txtAtas As New TextBox() With {.Location = New Point(20, 165), .Width = 340, .Text = atas.ToString("N0")}

        Dim lblTarif As New Label() With {.Text = "Tarif (%):", .Location = New Point(20, 200), .AutoSize = True}
        Dim txtTarif As New TextBox() With {.Location = New Point(20, 225), .Width = 100, .Text = tarif.ToString("F2")}

        Dim btnSave As New Button() With {
            .Text = "Simpan",
            .Location = New Point(180, 280),
            .Width = 80,
            .BackColor = Color.FromArgb(156, 0, 219),
            .ForeColor = Color.White,
            .FlatStyle = FlatStyle.Flat
        }
        btnSave.FlatAppearance.BorderSize = 0

        Dim btnCancel As New Button() With {
            .Text = "Batal",
            .Location = New Point(280, 280),
            .Width = 80
        }

        dialog.Controls.AddRange({lblLapisan, txtLapisan, lblBawah, txtBawah, lblAtas, txtAtas, lblTarif, txtTarif, btnSave, btnCancel})

        AddHandler btnCancel.Click, Sub() dialog.Close()

        AddHandler btnSave.Click, Sub()
                                      Dim lapisanParsed As Integer
                                      Dim bawahParsed, atasParsed, tarifParsed As Decimal

                                      If Not Integer.TryParse(txtLapisan.Text, lapisanParsed) Then
                                          MessageBox.Show("Lapisan harus berupa angka!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                          Return
                                      End If

                                      If Not Decimal.TryParse(txtBawah.Text.Replace(",", "").Replace(".", ""), bawahParsed) Then
                                          MessageBox.Show("Batas bawah tidak valid!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                          Return
                                      End If

                                      If Not Decimal.TryParse(txtAtas.Text.Replace(",", "").Replace(".", ""), atasParsed) Then
                                          MessageBox.Show("Batas atas tidak valid!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                          Return
                                      End If

                                      If Not Decimal.TryParse(txtTarif.Text.Replace(",", "."), tarifParsed) Then
                                          MessageBox.Show("Tarif tidak valid!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                          Return
                                      End If

                                      Try
                                          modulkoneksi.BukaKoneksi()
                                          Dim cmdSave As New MySqlCommand()
                                          cmdSave.Connection = modulkoneksi.koneksi

                                          If isEdit Then
                                              cmdSave.CommandText = "UPDATE master_tarif_pph SET lapisan = @lap, batas_bawah = @bawah, batas_atas = @atas, tarif_persen = @tarif WHERE id = @id"
                                              cmdSave.Parameters.AddWithValue("@id", tarifId)
                                          Else
                                              cmdSave.CommandText = "INSERT INTO master_tarif_pph (lapisan, batas_bawah, batas_atas, tarif_persen) VALUES (@lap, @bawah, @atas, @tarif)"
                                          End If

                                          cmdSave.Parameters.AddWithValue("@lap", lapisanParsed)
                                          cmdSave.Parameters.AddWithValue("@bawah", bawahParsed)
                                          cmdSave.Parameters.AddWithValue("@atas", atasParsed)
                                          cmdSave.Parameters.AddWithValue("@tarif", tarifParsed)
                                          cmdSave.ExecuteNonQuery()

                                          MessageBox.Show("Data Tarif berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                          dialog.Close()
                                          LoadTarifData()
                                      Catch ex As Exception
                                          MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                      Finally
                                          modulkoneksi.TutupKoneksi()
                                      End Try
                                  End Sub

        dialog.ShowDialog()
    End Sub

End Class
