<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmManagementPerusahaan
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim CustomizableEdges1 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges2 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges11 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges12 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges3 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges4 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges5 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges6 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges7 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges8 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges9 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges10 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges13 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges14 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Pk_navbar1 = New admin_navbar()
        PanelHeader = New Guna.UI2.WinForms.Guna2Panel()
        LblSubtitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        LblTitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        PanelFilters = New Guna.UI2.WinForms.Guna2Panel()
        BtnAddPerusahaan = New Guna.UI2.WinForms.Guna2Button()
        CmbSort = New Guna.UI2.WinForms.Guna2ComboBox()
        CmbStatus = New Guna.UI2.WinForms.Guna2ComboBox()
        TxtSearch = New Guna.UI2.WinForms.Guna2TextBox()
        PanelTable = New Guna.UI2.WinForms.Guna2Panel()
        LblTableSubtitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        LblTableTitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        GridPerusahaan = New Guna.UI2.WinForms.Guna2DataGridView()
        colNo = New DataGridViewTextBoxColumn()
        colNamaPerusahaan = New DataGridViewTextBoxColumn()
        colNPWP = New DataGridViewTextBoxColumn()
        colStatus = New DataGridViewTextBoxColumn()
        colTanggalDaftar = New DataGridViewTextBoxColumn()
        colActions = New DataGridViewButtonColumn()
        colDelete = New DataGridViewButtonColumn()
        PanelHeader.SuspendLayout()
        PanelFilters.SuspendLayout()
        PanelTable.SuspendLayout()
        CType(GridPerusahaan, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Pk_navbar1
        ' 
        Pk_navbar1.BackColor = Color.White
        Pk_navbar1.Dock = DockStyle.Left
        Pk_navbar1.ForeColor = Color.Black
        Pk_navbar1.Location = New Point(0, 0)
        Pk_navbar1.Margin = New Padding(3, 4, 3, 4)
        Pk_navbar1.Name = "Pk_navbar1"
        Pk_navbar1.Size = New Size(200, 720)
        Pk_navbar1.TabIndex = 0
        ' 
        ' PanelHeader
        ' 
        PanelHeader.BorderColor = Color.FromArgb(CByte(230), CByte(233), CByte(241))
        PanelHeader.BorderRadius = 10
        PanelHeader.BorderThickness = 1
        PanelHeader.Controls.Add(LblSubtitle)
        PanelHeader.Controls.Add(LblTitle)
        PanelHeader.CustomizableEdges = CustomizableEdges1
        PanelHeader.FillColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        PanelHeader.Location = New Point(210, 20)
        PanelHeader.Name = "PanelHeader"
        PanelHeader.Padding = New Padding(16)
        PanelHeader.ShadowDecoration.CustomizableEdges = CustomizableEdges2
        PanelHeader.Size = New Size(960, 90)
        PanelHeader.TabIndex = 1
        ' 
        ' LblSubtitle
        ' 
        LblSubtitle.BackColor = Color.Transparent
        LblSubtitle.Font = New Font("Segoe UI", 9F)
        LblSubtitle.ForeColor = Color.Silver
        LblSubtitle.Location = New Point(20, 56)
        LblSubtitle.Name = "LblSubtitle"
        LblSubtitle.Size = New Size(401, 17)
        LblSubtitle.TabIndex = 1
        LblSubtitle.Text = "Kelola data perusahaan, verifikasi status, dan pantau aktivitas pemberi kerja."
        ' 
        ' LblTitle
        ' 
        LblTitle.BackColor = Color.Transparent
        LblTitle.Font = New Font("Segoe UI Semibold", 14F, FontStyle.Bold)
        LblTitle.ForeColor = Color.White
        LblTitle.Location = New Point(20, 16)
        LblTitle.Name = "LblTitle"
        LblTitle.Size = New Size(222, 27)
        LblTitle.TabIndex = 0
        LblTitle.Text = "Management Perusahaan"
        ' 
        ' PanelFilters
        ' 
        PanelFilters.BorderColor = Color.FromArgb(CByte(230), CByte(233), CByte(241))
        PanelFilters.BorderRadius = 10
        PanelFilters.BorderThickness = 1
        PanelFilters.Controls.Add(BtnAddPerusahaan)
        PanelFilters.Controls.Add(CmbSort)
        PanelFilters.Controls.Add(CmbStatus)
        PanelFilters.Controls.Add(TxtSearch)
        PanelFilters.CustomizableEdges = CustomizableEdges11
        PanelFilters.FillColor = Color.White
        PanelFilters.Location = New Point(210, 126)
        PanelFilters.Name = "PanelFilters"
        PanelFilters.Padding = New Padding(12)
        PanelFilters.ShadowDecoration.CustomizableEdges = CustomizableEdges12
        PanelFilters.Size = New Size(960, 78)
        PanelFilters.TabIndex = 3
        ' 
        ' BtnAddPerusahaan
        ' 
        BtnAddPerusahaan.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        BtnAddPerusahaan.BorderRadius = 8
        BtnAddPerusahaan.CustomizableEdges = CustomizableEdges3
        BtnAddPerusahaan.FillColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        BtnAddPerusahaan.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        BtnAddPerusahaan.ForeColor = Color.WhiteSmoke
        BtnAddPerusahaan.Location = New Point(660, 20)
        BtnAddPerusahaan.Name = "BtnAddPerusahaan"
        BtnAddPerusahaan.ShadowDecoration.CustomizableEdges = CustomizableEdges4
        BtnAddPerusahaan.Size = New Size(280, 36)
        BtnAddPerusahaan.TabIndex = 3
        BtnAddPerusahaan.Text = "Tambah Perusahaan Baru"
        ' 
        ' CmbSort
        ' 
        CmbSort.BackColor = Color.Transparent
        CmbSort.BorderRadius = 8
        CmbSort.CustomizableEdges = CustomizableEdges5
        CmbSort.DrawMode = DrawMode.OwnerDrawFixed
        CmbSort.DropDownStyle = ComboBoxStyle.DropDownList
        CmbSort.FillColor = Color.FromArgb(CByte(245), CByte(246), CByte(250))
        CmbSort.FocusedColor = Color.FromArgb(CByte(0), CByte(122), CByte(255))
        CmbSort.FocusedState.BorderColor = Color.FromArgb(CByte(0), CByte(122), CByte(255))
        CmbSort.Font = New Font("Segoe UI", 9F)
        CmbSort.ForeColor = Color.FromArgb(CByte(60), CByte(76), CByte(97))
        CmbSort.ItemHeight = 30
        CmbSort.Items.AddRange(New Object() {"Sortir: Terbaru", "Sortir: Terlama", "Nama A-Z", "Nama Z-A"})
        CmbSort.Location = New Point(480, 20)
        CmbSort.Name = "CmbSort"
        CmbSort.ShadowDecoration.CustomizableEdges = CustomizableEdges6
        CmbSort.Size = New Size(160, 36)
        CmbSort.TabIndex = 2
        ' 
        ' CmbStatus
        ' 
        CmbStatus.BackColor = Color.Transparent
        CmbStatus.BorderRadius = 8
        CmbStatus.CustomizableEdges = CustomizableEdges7
        CmbStatus.DrawMode = DrawMode.OwnerDrawFixed
        CmbStatus.DropDownStyle = ComboBoxStyle.DropDownList
        CmbStatus.FillColor = Color.FromArgb(CByte(245), CByte(246), CByte(250))
        CmbStatus.FocusedColor = Color.FromArgb(CByte(0), CByte(122), CByte(255))
        CmbStatus.FocusedState.BorderColor = Color.FromArgb(CByte(0), CByte(122), CByte(255))
        CmbStatus.Font = New Font("Segoe UI", 9F)
        CmbStatus.ForeColor = Color.FromArgb(CByte(60), CByte(76), CByte(97))
        CmbStatus.ItemHeight = 30
        CmbStatus.Items.AddRange(New Object() {"Semua Status", "Active", "Pending", "Inactive"})
        CmbStatus.Location = New Point(310, 20)
        CmbStatus.Name = "CmbStatus"
        CmbStatus.ShadowDecoration.CustomizableEdges = CustomizableEdges8
        CmbStatus.Size = New Size(160, 36)
        CmbStatus.TabIndex = 1
        ' 
        ' TxtSearch
        ' 
        TxtSearch.BorderRadius = 8
        TxtSearch.CustomizableEdges = CustomizableEdges9
        TxtSearch.DefaultText = ""
        TxtSearch.FillColor = Color.FromArgb(CByte(245), CByte(246), CByte(250))
        TxtSearch.Font = New Font("Segoe UI", 9F)
        TxtSearch.Location = New Point(18, 20)
        TxtSearch.Margin = New Padding(3, 4, 3, 4)
        TxtSearch.Name = "TxtSearch"
        TxtSearch.PlaceholderText = "Cari nama perusahaan atau NPWP..."
        TxtSearch.SelectedText = ""
        TxtSearch.ShadowDecoration.CustomizableEdges = CustomizableEdges10
        TxtSearch.Size = New Size(280, 36)
        TxtSearch.TabIndex = 0
        ' 
        ' PanelTable
        ' 
        PanelTable.BorderColor = Color.FromArgb(CByte(230), CByte(233), CByte(241))
        PanelTable.BorderRadius = 10
        PanelTable.BorderThickness = 1
        PanelTable.Controls.Add(LblTableSubtitle)
        PanelTable.Controls.Add(LblTableTitle)
        PanelTable.Controls.Add(GridPerusahaan)
        PanelTable.CustomizableEdges = CustomizableEdges13
        PanelTable.FillColor = Color.White
        PanelTable.Location = New Point(210, 216)
        PanelTable.Name = "PanelTable"
        PanelTable.Padding = New Padding(18)
        PanelTable.ShadowDecoration.CustomizableEdges = CustomizableEdges14
        PanelTable.Size = New Size(960, 381)
        PanelTable.TabIndex = 4
        ' 
        ' LblTableSubtitle
        ' 
        LblTableSubtitle.BackColor = Color.Transparent
        LblTableSubtitle.Font = New Font("Segoe UI", 9F)
        LblTableSubtitle.ForeColor = Color.FromArgb(CByte(120), CByte(128), CByte(146))
        LblTableSubtitle.Location = New Point(24, 60)
        LblTableSubtitle.Name = "LblTableSubtitle"
        LblTableSubtitle.Size = New Size(389, 17)
        LblTableSubtitle.TabIndex = 1
        LblTableSubtitle.Text = "Data perusahaan terdaftar dengan informasi lengkap dan status verifikasi."
        ' 
        ' LblTableTitle
        ' 
        LblTableTitle.BackColor = Color.Transparent
        LblTableTitle.Font = New Font("Segoe UI Semibold", 11F, FontStyle.Bold)
        LblTableTitle.ForeColor = Color.FromArgb(CByte(35), CByte(44), CByte(63))
        LblTableTitle.Location = New Point(24, 26)
        LblTableTitle.Name = "LblTableTitle"
        LblTableTitle.Size = New Size(130, 22)
        LblTableTitle.TabIndex = 0
        LblTableTitle.Text = "Daftar Perusahaan"
        ' 
        ' GridPerusahaan
        ' 
        GridPerusahaan.AllowUserToAddRows = False
        GridPerusahaan.AllowUserToDeleteRows = False
        GridPerusahaan.AllowUserToResizeRows = False
        DataGridViewCellStyle1.BackColor = Color.FromArgb(CByte(248), CByte(249), CByte(252))
        GridPerusahaan.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        DataGridViewCellStyle2.Font = New Font("Segoe UI Semibold", 9F, FontStyle.Bold)
        DataGridViewCellStyle2.ForeColor = Color.White
        DataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        DataGridViewCellStyle2.SelectionForeColor = Color.White
        DataGridViewCellStyle2.WrapMode = DataGridViewTriState.True
        GridPerusahaan.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        GridPerusahaan.ColumnHeadersHeight = 36
        GridPerusahaan.Columns.AddRange(New DataGridViewColumn() {colNo, colNamaPerusahaan, colNPWP, colStatus, colTanggalDaftar, colActions, colDelete})
        DataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = Color.White
        DataGridViewCellStyle5.Font = New Font("Segoe UI", 9F)
        DataGridViewCellStyle5.ForeColor = Color.FromArgb(CByte(64), CByte(74), CByte(89))
        DataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(CByte(240), CByte(244), CByte(252))
        DataGridViewCellStyle5.SelectionForeColor = Color.FromArgb(CByte(35), CByte(44), CByte(63))
        DataGridViewCellStyle5.WrapMode = DataGridViewTriState.False
        GridPerusahaan.DefaultCellStyle = DataGridViewCellStyle5
        GridPerusahaan.GridColor = Color.FromArgb(CByte(235), CByte(240), CByte(247))
        GridPerusahaan.Location = New Point(24, 96)
        GridPerusahaan.MultiSelect = False
        GridPerusahaan.Name = "GridPerusahaan"
        GridPerusahaan.ReadOnly = True
        GridPerusahaan.RowHeadersVisible = False
        GridPerusahaan.RowHeadersWidth = 51
        GridPerusahaan.RowTemplate.Height = 36
        GridPerusahaan.Size = New Size(912, 240)
        GridPerusahaan.TabIndex = 2
        GridPerusahaan.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White
        GridPerusahaan.ThemeStyle.AlternatingRowsStyle.Font = Nothing
        GridPerusahaan.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty
        GridPerusahaan.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty
        GridPerusahaan.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty
        GridPerusahaan.ThemeStyle.BackColor = Color.White
        GridPerusahaan.ThemeStyle.GridColor = Color.FromArgb(CByte(235), CByte(240), CByte(247))
        GridPerusahaan.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(CByte(100), CByte(88), CByte(255))
        GridPerusahaan.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None
        GridPerusahaan.ThemeStyle.HeaderStyle.Font = New Font("Segoe UI", 9F)
        GridPerusahaan.ThemeStyle.HeaderStyle.ForeColor = Color.White
        GridPerusahaan.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        GridPerusahaan.ThemeStyle.HeaderStyle.Height = 36
        GridPerusahaan.ThemeStyle.ReadOnly = True
        GridPerusahaan.ThemeStyle.RowsStyle.BackColor = Color.White
        GridPerusahaan.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        GridPerusahaan.ThemeStyle.RowsStyle.Font = New Font("Segoe UI", 9F)
        GridPerusahaan.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(CByte(71), CByte(69), CByte(94))
        GridPerusahaan.ThemeStyle.RowsStyle.Height = 36
        GridPerusahaan.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(CByte(231), CByte(229), CByte(255))
        GridPerusahaan.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(CByte(71), CByte(69), CByte(94))
        ' 
        ' colNo
        ' 
        colNo.FillWeight = 50F
        colNo.HeaderText = "No"
        colNo.MinimumWidth = 6
        colNo.Name = "colNo"
        colNo.ReadOnly = True
        ' 
        ' colNamaPerusahaan
        ' 
        colNamaPerusahaan.FillWeight = 72.99465F
        colNamaPerusahaan.HeaderText = "Nama Perusahaan"
        colNamaPerusahaan.MinimumWidth = 6
        colNamaPerusahaan.Name = "colNamaPerusahaan"
        colNamaPerusahaan.ReadOnly = True
        ' 
        ' colNPWP
        ' 
        colNPWP.FillWeight = 72.99465F
        colNPWP.HeaderText = "NPWP"
        colNPWP.MinimumWidth = 6
        colNPWP.Name = "colNPWP"
        colNPWP.ReadOnly = True
        ' 
        ' colStatus
        ' 
        colStatus.FillWeight = 72.99465F
        colStatus.HeaderText = "Status"
        colStatus.MinimumWidth = 6
        colStatus.Name = "colStatus"
        colStatus.ReadOnly = True
        ' 
        ' colTanggalDaftar
        ' 
        colTanggalDaftar.FillWeight = 72.99465F
        colTanggalDaftar.HeaderText = "Tanggal Daftar"
        colTanggalDaftar.MinimumWidth = 6
        colTanggalDaftar.Name = "colTanggalDaftar"
        colTanggalDaftar.ReadOnly = True
        ' 
        ' colActions
        ' 
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.BackColor = Color.FromArgb(CByte(0), CByte(122), CByte(255))
        DataGridViewCellStyle3.ForeColor = Color.White
        colActions.DefaultCellStyle = DataGridViewCellStyle3
        colActions.FillWeight = 60F
        colActions.HeaderText = "Edit"
        colActions.MinimumWidth = 6
        colActions.Name = "colActions"
        colActions.ReadOnly = True
        colActions.Text = "Edit"
        colActions.UseColumnTextForButtonValue = True
        ' 
        ' colDelete
        ' 
        DataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.BackColor = Color.FromArgb(CByte(255), CByte(59), CByte(48))
        DataGridViewCellStyle4.ForeColor = Color.White
        colDelete.DefaultCellStyle = DataGridViewCellStyle4
        colDelete.FillWeight = 60F
        colDelete.HeaderText = "Hapus"
        colDelete.MinimumWidth = 6
        colDelete.Name = "colDelete"
        colDelete.ReadOnly = True
        colDelete.Text = "Hapus"
        colDelete.UseColumnTextForButtonValue = True
        ' 
        ' FrmManagementPerusahaan
        ' 
        AutoScaleMode = AutoScaleMode.None
        BackColor = Color.FromArgb(CByte(247), CByte(248), CByte(252))
        ClientSize = New Size(1200, 720)
        Controls.Add(PanelTable)
        Controls.Add(PanelFilters)
        Controls.Add(PanelHeader)
        Controls.Add(Pk_navbar1)
        FormBorderStyle = FormBorderStyle.None
        Name = "FrmManagementPerusahaan"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Management Perusahaan"
        PanelHeader.ResumeLayout(False)
        PanelHeader.PerformLayout()
        PanelFilters.ResumeLayout(False)
        PanelTable.ResumeLayout(False)
        PanelTable.PerformLayout()
        CType(GridPerusahaan, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Pk_navbar1 As admin_navbar
    Friend WithEvents PanelHeader As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents LblTitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents LblSubtitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents PanelFilters As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents TxtSearch As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents CmbStatus As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents CmbSort As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents BtnAddPerusahaan As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents PanelTable As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents LblTableTitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents LblTableSubtitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents GridPerusahaan As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents colNo As DataGridViewTextBoxColumn
    Friend WithEvents colNamaPerusahaan As DataGridViewTextBoxColumn
    Friend WithEvents colNPWP As DataGridViewTextBoxColumn
    Friend WithEvents colStatus As DataGridViewTextBoxColumn
    Friend WithEvents colTanggalDaftar As DataGridViewTextBoxColumn
    Friend WithEvents colActions As DataGridViewButtonColumn
    Friend WithEvents colDelete As DataGridViewButtonColumn
End Class
