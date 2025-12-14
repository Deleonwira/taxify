<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class admin_master_pajak
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As DataGridViewCellStyle = New DataGridViewCellStyle()

        Admin_navbar1 = New admin_navbar()
        PanelHeader = New Guna.UI2.WinForms.Guna2Panel()
        LblSubtitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        LblTitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        PanelPTKP = New Guna.UI2.WinForms.Guna2Panel()
        BtnAddPTKP = New Guna.UI2.WinForms.Guna2Button()
        LblPTKPSubtitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        LblPTKPTitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        GridPTKP = New Guna.UI2.WinForms.Guna2DataGridView()
        colPTKPNo = New DataGridViewTextBoxColumn()
        colPTKPKode = New DataGridViewTextBoxColumn()
        colPTKPKeterangan = New DataGridViewTextBoxColumn()
        colPTKPNilai = New DataGridViewTextBoxColumn()
        colPTKPEdit = New DataGridViewButtonColumn()
        PanelTarif = New Guna.UI2.WinForms.Guna2Panel()
        BtnAddTarif = New Guna.UI2.WinForms.Guna2Button()
        LblTarifSubtitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        LblTarifTitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        GridTarif = New Guna.UI2.WinForms.Guna2DataGridView()
        colTarifNo = New DataGridViewTextBoxColumn()
        colTarifLapisan = New DataGridViewTextBoxColumn()
        colTarifBawah = New DataGridViewTextBoxColumn()
        colTarifAtas = New DataGridViewTextBoxColumn()
        colTarifPersen = New DataGridViewTextBoxColumn()
        colTarifEdit = New DataGridViewButtonColumn()

        PanelHeader.SuspendLayout()
        PanelPTKP.SuspendLayout()
        CType(GridPTKP, ComponentModel.ISupportInitialize).BeginInit()
        PanelTarif.SuspendLayout()
        CType(GridTarif, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()

        ' 
        ' Admin_navbar1
        ' 
        Admin_navbar1.BackColor = Color.White
        Admin_navbar1.Dock = DockStyle.Left
        Admin_navbar1.ForeColor = Color.Black
        Admin_navbar1.Location = New Point(0, 0)
        Admin_navbar1.Margin = New Padding(3, 4, 3, 4)
        Admin_navbar1.Name = "Admin_navbar1"
        Admin_navbar1.Size = New Size(200, 720)
        Admin_navbar1.TabIndex = 0

        ' 
        ' PanelHeader
        ' 
        PanelHeader.BorderColor = Color.FromArgb(CByte(230), CByte(233), CByte(241))
        PanelHeader.BorderRadius = 10
        PanelHeader.BorderThickness = 1
        PanelHeader.Controls.Add(LblSubtitle)
        PanelHeader.Controls.Add(LblTitle)
        PanelHeader.FillColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        PanelHeader.Location = New Point(210, 20)
        PanelHeader.Name = "PanelHeader"
        PanelHeader.Padding = New Padding(16)
        PanelHeader.Size = New Size(1060, 90)
        PanelHeader.TabIndex = 1

        ' 
        ' LblTitle
        ' 
        LblTitle.BackColor = Color.Transparent
        LblTitle.Font = New Font("Segoe UI Semibold", 14F, FontStyle.Bold)
        LblTitle.ForeColor = Color.White
        LblTitle.Location = New Point(20, 16)
        LblTitle.Name = "LblTitle"
        LblTitle.Size = New Size(220, 27)
        LblTitle.TabIndex = 0
        LblTitle.Text = "Master Data Perpajakan"

        ' 
        ' LblSubtitle
        ' 
        LblSubtitle.BackColor = Color.Transparent
        LblSubtitle.Font = New Font("Segoe UI", 9F)
        LblSubtitle.ForeColor = Color.Silver
        LblSubtitle.Location = New Point(20, 50)
        LblSubtitle.Name = "LblSubtitle"
        LblSubtitle.Size = New Size(350, 17)
        LblSubtitle.TabIndex = 1
        LblSubtitle.Text = "Kelola nilai PTKP dan tarif progresif PPh 21 sesuai regulasi terbaru."

        ' 
        ' PanelPTKP
        ' 
        PanelPTKP.BorderColor = Color.FromArgb(CByte(230), CByte(233), CByte(241))
        PanelPTKP.BorderRadius = 10
        PanelPTKP.BorderThickness = 1
        PanelPTKP.Controls.Add(BtnAddPTKP)
        PanelPTKP.Controls.Add(LblPTKPSubtitle)
        PanelPTKP.Controls.Add(LblPTKPTitle)
        PanelPTKP.Controls.Add(GridPTKP)
        PanelPTKP.FillColor = Color.White
        PanelPTKP.Location = New Point(210, 126)
        PanelPTKP.Name = "PanelPTKP"
        PanelPTKP.Padding = New Padding(18)
        PanelPTKP.Size = New Size(520, 340)
        PanelPTKP.TabIndex = 2

        ' 
        ' LblPTKPTitle
        ' 
        LblPTKPTitle.BackColor = Color.Transparent
        LblPTKPTitle.Font = New Font("Segoe UI Semibold", 11F, FontStyle.Bold)
        LblPTKPTitle.ForeColor = Color.FromArgb(CByte(35), CByte(44), CByte(63))
        LblPTKPTitle.Location = New Point(24, 20)
        LblPTKPTitle.Name = "LblPTKPTitle"
        LblPTKPTitle.Size = New Size(150, 22)
        LblPTKPTitle.TabIndex = 0
        LblPTKPTitle.Text = "Master PTKP"

        ' 
        ' LblPTKPSubtitle
        ' 
        LblPTKPSubtitle.BackColor = Color.Transparent
        LblPTKPSubtitle.Font = New Font("Segoe UI", 9F)
        LblPTKPSubtitle.ForeColor = Color.FromArgb(CByte(120), CByte(128), CByte(146))
        LblPTKPSubtitle.Location = New Point(24, 46)
        LblPTKPSubtitle.Name = "LblPTKPSubtitle"
        LblPTKPSubtitle.Size = New Size(280, 17)
        LblPTKPSubtitle.TabIndex = 1
        LblPTKPSubtitle.Text = "Penghasilan Tidak Kena Pajak per status keluarga"

        ' 
        ' BtnAddPTKP
        ' 
        BtnAddPTKP.BorderRadius = 8
        BtnAddPTKP.FillColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        BtnAddPTKP.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        BtnAddPTKP.ForeColor = Color.WhiteSmoke
        BtnAddPTKP.Location = New Point(380, 20)
        BtnAddPTKP.Name = "BtnAddPTKP"
        BtnAddPTKP.Size = New Size(120, 32)
        BtnAddPTKP.TabIndex = 2
        BtnAddPTKP.Text = "+ Tambah PTKP"

        ' 
        ' GridPTKP
        ' 
        GridPTKP.AllowUserToAddRows = False
        GridPTKP.AllowUserToDeleteRows = False
        GridPTKP.AllowUserToResizeRows = False
        DataGridViewCellStyle1.BackColor = Color.FromArgb(CByte(248), CByte(249), CByte(252))
        GridPTKP.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        DataGridViewCellStyle2.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        DataGridViewCellStyle2.ForeColor = Color.White
        DataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        DataGridViewCellStyle2.SelectionForeColor = Color.White
        DataGridViewCellStyle2.WrapMode = DataGridViewTriState.True
        GridPTKP.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        GridPTKP.ColumnHeadersHeight = 36
        GridPTKP.Columns.AddRange(New DataGridViewColumn() {colPTKPNo, colPTKPKode, colPTKPKeterangan, colPTKPNilai, colPTKPEdit})
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = Color.White
        DataGridViewCellStyle3.Font = New Font("Segoe UI", 9F)
        DataGridViewCellStyle3.ForeColor = Color.FromArgb(CByte(64), CByte(74), CByte(89))
        DataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(CByte(240), CByte(244), CByte(252))
        DataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(CByte(35), CByte(44), CByte(63))
        DataGridViewCellStyle3.WrapMode = DataGridViewTriState.False
        GridPTKP.DefaultCellStyle = DataGridViewCellStyle3
        GridPTKP.GridColor = Color.FromArgb(CByte(235), CByte(240), CByte(247))
        GridPTKP.Location = New Point(24, 76)
        GridPTKP.MultiSelect = False
        GridPTKP.Name = "GridPTKP"
        GridPTKP.ReadOnly = True
        GridPTKP.RowHeadersVisible = False
        GridPTKP.RowTemplate.Height = 32
        GridPTKP.Size = New Size(476, 245)
        GridPTKP.TabIndex = 3
        GridPTKP.ThemeStyle.BackColor = Color.White
        GridPTKP.ThemeStyle.GridColor = Color.FromArgb(CByte(235), CByte(240), CByte(247))
        GridPTKP.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        GridPTKP.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None
        GridPTKP.ThemeStyle.HeaderStyle.ForeColor = Color.White
        GridPTKP.ThemeStyle.HeaderStyle.Height = 36
        GridPTKP.ThemeStyle.ReadOnly = True
        GridPTKP.ThemeStyle.RowsStyle.BackColor = Color.White
        GridPTKP.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        GridPTKP.ThemeStyle.RowsStyle.Height = 32

        ' 
        ' colPTKPNo
        ' 
        colPTKPNo.HeaderText = "No"
        colPTKPNo.Name = "colPTKPNo"
        colPTKPNo.ReadOnly = True
        colPTKPNo.Width = 40

        ' 
        ' colPTKPKode
        ' 
        colPTKPKode.HeaderText = "Kode"
        colPTKPKode.Name = "colPTKPKode"
        colPTKPKode.ReadOnly = True
        colPTKPKode.Width = 60

        ' 
        ' colPTKPKeterangan
        ' 
        colPTKPKeterangan.HeaderText = "Keterangan"
        colPTKPKeterangan.Name = "colPTKPKeterangan"
        colPTKPKeterangan.ReadOnly = True
        colPTKPKeterangan.Width = 180

        ' 
        ' colPTKPNilai
        ' 
        colPTKPNilai.HeaderText = "Nilai Tahunan"
        colPTKPNilai.Name = "colPTKPNilai"
        colPTKPNilai.ReadOnly = True
        colPTKPNilai.Width = 120

        ' 
        ' colPTKPEdit
        ' 
        colPTKPEdit.HeaderText = "Aksi"
        colPTKPEdit.Name = "colPTKPEdit"
        colPTKPEdit.ReadOnly = True
        colPTKPEdit.Text = "Edit"
        colPTKPEdit.UseColumnTextForButtonValue = True
        colPTKPEdit.Width = 60

        ' 
        ' PanelTarif
        ' 
        PanelTarif.BorderColor = Color.FromArgb(CByte(230), CByte(233), CByte(241))
        PanelTarif.BorderRadius = 10
        PanelTarif.BorderThickness = 1
        PanelTarif.Controls.Add(BtnAddTarif)
        PanelTarif.Controls.Add(LblTarifSubtitle)
        PanelTarif.Controls.Add(LblTarifTitle)
        PanelTarif.Controls.Add(GridTarif)
        PanelTarif.FillColor = Color.White
        PanelTarif.Location = New Point(750, 126)
        PanelTarif.Name = "PanelTarif"
        PanelTarif.Padding = New Padding(18)
        PanelTarif.Size = New Size(520, 340)
        PanelTarif.TabIndex = 3

        ' 
        ' LblTarifTitle
        ' 
        LblTarifTitle.BackColor = Color.Transparent
        LblTarifTitle.Font = New Font("Segoe UI Semibold", 11F, FontStyle.Bold)
        LblTarifTitle.ForeColor = Color.FromArgb(CByte(35), CByte(44), CByte(63))
        LblTarifTitle.Location = New Point(24, 20)
        LblTarifTitle.Name = "LblTarifTitle"
        LblTarifTitle.Size = New Size(180, 22)
        LblTarifTitle.TabIndex = 0
        LblTarifTitle.Text = "Tarif Progresif PPh 21"

        ' 
        ' LblTarifSubtitle
        ' 
        LblTarifSubtitle.BackColor = Color.Transparent
        LblTarifSubtitle.Font = New Font("Segoe UI", 9F)
        LblTarifSubtitle.ForeColor = Color.FromArgb(CByte(120), CByte(128), CByte(146))
        LblTarifSubtitle.Location = New Point(24, 46)
        LblTarifSubtitle.Name = "LblTarifSubtitle"
        LblTarifSubtitle.Size = New Size(250, 17)
        LblTarifSubtitle.TabIndex = 1
        LblTarifSubtitle.Text = "Lapisan tarif pajak berdasarkan UU HPP 2021"

        ' 
        ' BtnAddTarif
        ' 
        BtnAddTarif.BorderRadius = 8
        BtnAddTarif.FillColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        BtnAddTarif.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        BtnAddTarif.ForeColor = Color.WhiteSmoke
        BtnAddTarif.Location = New Point(380, 20)
        BtnAddTarif.Name = "BtnAddTarif"
        BtnAddTarif.Size = New Size(120, 32)
        BtnAddTarif.TabIndex = 2
        BtnAddTarif.Text = "+ Tambah Tarif"

        ' 
        ' GridTarif
        ' 
        GridTarif.AllowUserToAddRows = False
        GridTarif.AllowUserToDeleteRows = False
        GridTarif.AllowUserToResizeRows = False
        DataGridViewCellStyle4.BackColor = Color.FromArgb(CByte(248), CByte(249), CByte(252))
        GridTarif.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle4
        DataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle5.BackColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        DataGridViewCellStyle5.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        DataGridViewCellStyle5.ForeColor = Color.White
        DataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        DataGridViewCellStyle5.SelectionForeColor = Color.White
        DataGridViewCellStyle5.WrapMode = DataGridViewTriState.True
        GridTarif.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle5
        GridTarif.ColumnHeadersHeight = 36
        GridTarif.Columns.AddRange(New DataGridViewColumn() {colTarifNo, colTarifLapisan, colTarifBawah, colTarifAtas, colTarifPersen, colTarifEdit})
        DataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = Color.White
        DataGridViewCellStyle6.Font = New Font("Segoe UI", 9F)
        DataGridViewCellStyle6.ForeColor = Color.FromArgb(CByte(64), CByte(74), CByte(89))
        DataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(CByte(240), CByte(244), CByte(252))
        DataGridViewCellStyle6.SelectionForeColor = Color.FromArgb(CByte(35), CByte(44), CByte(63))
        DataGridViewCellStyle6.WrapMode = DataGridViewTriState.False
        GridTarif.DefaultCellStyle = DataGridViewCellStyle6
        GridTarif.GridColor = Color.FromArgb(CByte(235), CByte(240), CByte(247))
        GridTarif.Location = New Point(24, 76)
        GridTarif.MultiSelect = False
        GridTarif.Name = "GridTarif"
        GridTarif.ReadOnly = True
        GridTarif.RowHeadersVisible = False
        GridTarif.RowTemplate.Height = 32
        GridTarif.Size = New Size(476, 245)
        GridTarif.TabIndex = 3
        GridTarif.ThemeStyle.BackColor = Color.White
        GridTarif.ThemeStyle.GridColor = Color.FromArgb(CByte(235), CByte(240), CByte(247))
        GridTarif.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        GridTarif.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None
        GridTarif.ThemeStyle.HeaderStyle.ForeColor = Color.White
        GridTarif.ThemeStyle.HeaderStyle.Height = 36
        GridTarif.ThemeStyle.ReadOnly = True
        GridTarif.ThemeStyle.RowsStyle.BackColor = Color.White
        GridTarif.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        GridTarif.ThemeStyle.RowsStyle.Height = 32

        ' 
        ' colTarifNo
        ' 
        colTarifNo.HeaderText = "No"
        colTarifNo.Name = "colTarifNo"
        colTarifNo.ReadOnly = True
        colTarifNo.Width = 35

        ' 
        ' colTarifLapisan
        ' 
        colTarifLapisan.HeaderText = "Lapisan"
        colTarifLapisan.Name = "colTarifLapisan"
        colTarifLapisan.ReadOnly = True
        colTarifLapisan.Width = 60

        ' 
        ' colTarifBawah
        ' 
        colTarifBawah.HeaderText = "Batas Bawah"
        colTarifBawah.Name = "colTarifBawah"
        colTarifBawah.ReadOnly = True
        colTarifBawah.Width = 110

        ' 
        ' colTarifAtas
        ' 
        colTarifAtas.HeaderText = "Batas Atas"
        colTarifAtas.Name = "colTarifAtas"
        colTarifAtas.ReadOnly = True
        colTarifAtas.Width = 110

        ' 
        ' colTarifPersen
        ' 
        colTarifPersen.HeaderText = "Tarif (%)"
        colTarifPersen.Name = "colTarifPersen"
        colTarifPersen.ReadOnly = True
        colTarifPersen.Width = 70

        ' 
        ' colTarifEdit
        ' 
        colTarifEdit.HeaderText = "Aksi"
        colTarifEdit.Name = "colTarifEdit"
        colTarifEdit.ReadOnly = True
        colTarifEdit.Text = "Edit"
        colTarifEdit.UseColumnTextForButtonValue = True
        colTarifEdit.Width = 55

        ' 
        ' admin_master_pajak
        ' 
        AutoScaleMode = AutoScaleMode.None
        BackColor = Color.FromArgb(CByte(247), CByte(248), CByte(252))
        ClientSize = New Size(1300, 720)
        Controls.Add(PanelTarif)
        Controls.Add(PanelPTKP)
        Controls.Add(PanelHeader)
        Controls.Add(Admin_navbar1)
        FormBorderStyle = FormBorderStyle.None
        Name = "admin_master_pajak"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Master Data Perpajakan"

        PanelHeader.ResumeLayout(False)
        PanelHeader.PerformLayout()
        PanelPTKP.ResumeLayout(False)
        PanelPTKP.PerformLayout()
        CType(GridPTKP, ComponentModel.ISupportInitialize).EndInit()
        PanelTarif.ResumeLayout(False)
        PanelTarif.PerformLayout()
        CType(GridTarif, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Admin_navbar1 As admin_navbar
    Friend WithEvents PanelHeader As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents LblTitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents LblSubtitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents PanelPTKP As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents LblPTKPTitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents LblPTKPSubtitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents BtnAddPTKP As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents GridPTKP As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents colPTKPNo As DataGridViewTextBoxColumn
    Friend WithEvents colPTKPKode As DataGridViewTextBoxColumn
    Friend WithEvents colPTKPKeterangan As DataGridViewTextBoxColumn
    Friend WithEvents colPTKPNilai As DataGridViewTextBoxColumn
    Friend WithEvents colPTKPEdit As DataGridViewButtonColumn
    Friend WithEvents PanelTarif As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents LblTarifTitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents LblTarifSubtitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents BtnAddTarif As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents GridTarif As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents colTarifNo As DataGridViewTextBoxColumn
    Friend WithEvents colTarifLapisan As DataGridViewTextBoxColumn
    Friend WithEvents colTarifBawah As DataGridViewTextBoxColumn
    Friend WithEvents colTarifAtas As DataGridViewTextBoxColumn
    Friend WithEvents colTarifPersen As DataGridViewTextBoxColumn
    Friend WithEvents colTarifEdit As DataGridViewButtonColumn
End Class
