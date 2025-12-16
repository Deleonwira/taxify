<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class admin_manajemen_spt
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
        Dim CustomizableEdges3 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges4 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges5 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges6 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges7 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges8 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges9 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges10 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges11 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges12 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Pk_navbar1 = New app_pajak.admin_navbar()
        Me.PanelHeader = New Guna.UI2.WinForms.Guna2Panel()
        Me.LblSubtitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.LblTitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.PanelFilters = New Guna.UI2.WinForms.Guna2Panel()
        Me.CmbTahun = New Guna.UI2.WinForms.Guna2ComboBox()
        Me.CmbStatus = New Guna.UI2.WinForms.Guna2ComboBox()
        Me.TxtSearch = New Guna.UI2.WinForms.Guna2TextBox()
        Me.PanelTable = New Guna.UI2.WinForms.Guna2Panel()
        Me.LblTableSubtitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.LblTableTitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.GridSPT = New Guna.UI2.WinForms.Guna2DataGridView()
        Me.colNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colNamaWP = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colNPWP = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTahun = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTanggalLapor = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colEdit = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.PanelHeader.SuspendLayout()
        Me.PanelFilters.SuspendLayout()
        Me.PanelTable.SuspendLayout()
        CType(Me.GridSPT, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Pk_navbar1
        '
        Me.Pk_navbar1.BackColor = System.Drawing.Color.White
        Me.Pk_navbar1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Pk_navbar1.ForeColor = System.Drawing.Color.Black
        Me.Pk_navbar1.Location = New System.Drawing.Point(0, 0)
        Me.Pk_navbar1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Pk_navbar1.Name = "Pk_navbar1"
        Me.Pk_navbar1.Size = New System.Drawing.Size(200, 720)
        Me.Pk_navbar1.TabIndex = 0
        '
        'PanelHeader
        '
        Me.PanelHeader.BorderColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.PanelHeader.BorderRadius = 10
        Me.PanelHeader.BorderThickness = 1
        Me.PanelHeader.Controls.Add(Me.LblSubtitle)
        Me.PanelHeader.Controls.Add(Me.LblTitle)
        Me.PanelHeader.CustomizableEdges = CustomizableEdges1
        Me.PanelHeader.FillColor = System.Drawing.Color.FromArgb(CType(CType(156, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(219, Byte), Integer))
        Me.PanelHeader.Location = New System.Drawing.Point(210, 20)
        Me.PanelHeader.Name = "PanelHeader"
        Me.PanelHeader.Padding = New System.Windows.Forms.Padding(16)
        Me.PanelHeader.ShadowDecoration.CustomizableEdges = CustomizableEdges2
        Me.PanelHeader.Size = New System.Drawing.Size(960, 90)
        Me.PanelHeader.TabIndex = 1
        '
        'LblSubtitle
        '
        Me.LblSubtitle.BackColor = System.Drawing.Color.Transparent
        Me.LblSubtitle.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblSubtitle.ForeColor = System.Drawing.Color.Silver
        Me.LblSubtitle.Location = New System.Drawing.Point(20, 56)
        Me.LblSubtitle.Name = "LblSubtitle"
        Me.LblSubtitle.Size = New System.Drawing.Size(401, 17)
        Me.LblSubtitle.TabIndex = 1
        Me.LblSubtitle.Text = "Kelola data SPT Tahunan wajib pajak, verifikasi status, dan pantau pelaporan."
        '
        'LblTitle
        '
        Me.LblTitle.BackColor = System.Drawing.Color.Transparent
        Me.LblTitle.Font = New System.Drawing.Font("Segoe UI Semibold", 14.0!, System.Drawing.FontStyle.Bold)
        Me.LblTitle.ForeColor = System.Drawing.Color.White
        Me.LblTitle.Location = New System.Drawing.Point(20, 16)
        Me.LblTitle.Name = "LblTitle"
        Me.LblTitle.Size = New System.Drawing.Size(222, 27)
        Me.LblTitle.TabIndex = 0
        Me.LblTitle.Text = "Manajemen SPT Tahunan"
        '
        'PanelFilters
        '
        Me.PanelFilters.BorderColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.PanelFilters.BorderRadius = 10
        Me.PanelFilters.BorderThickness = 1
        Me.PanelFilters.Controls.Add(Me.CmbTahun)
        Me.PanelFilters.Controls.Add(Me.CmbStatus)
        Me.PanelFilters.Controls.Add(Me.TxtSearch)
        Me.PanelFilters.CustomizableEdges = CustomizableEdges3
        Me.PanelFilters.FillColor = System.Drawing.Color.White
        Me.PanelFilters.Location = New System.Drawing.Point(210, 126)
        Me.PanelFilters.Name = "PanelFilters"
        Me.PanelFilters.Padding = New System.Windows.Forms.Padding(12)
        Me.PanelFilters.ShadowDecoration.CustomizableEdges = CustomizableEdges4
        Me.PanelFilters.Size = New System.Drawing.Size(960, 78)
        Me.PanelFilters.TabIndex = 2
        '
        'CmbTahun
        '
        Me.CmbTahun.BackColor = System.Drawing.Color.Transparent
        Me.CmbTahun.BorderRadius = 8
        Me.CmbTahun.CustomizableEdges = CustomizableEdges5
        Me.CmbTahun.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.CmbTahun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbTahun.FillColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.CmbTahun.FocusedColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.CmbTahun.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.CmbTahun.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.CmbTahun.ForeColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(76, Byte), Integer), CType(CType(97, Byte), Integer))
        Me.CmbTahun.ItemHeight = 30
        Me.CmbTahun.Items.AddRange(New Object() {"Semua Tahun", "2024", "2025", "2026"})
        Me.CmbTahun.Location = New System.Drawing.Point(480, 20)
        Me.CmbTahun.Name = "CmbTahun"
        Me.CmbTahun.ShadowDecoration.CustomizableEdges = CustomizableEdges6
        Me.CmbTahun.Size = New System.Drawing.Size(160, 36)
        Me.CmbTahun.TabIndex = 2
        '
        'CmbStatus
        '
        Me.CmbStatus.BackColor = System.Drawing.Color.Transparent
        Me.CmbStatus.BorderRadius = 8
        Me.CmbStatus.CustomizableEdges = CustomizableEdges7
        Me.CmbStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.CmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmbStatus.FillColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.CmbStatus.FocusedColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.CmbStatus.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.CmbStatus.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.CmbStatus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(60, Byte), Integer), CType(CType(76, Byte), Integer), CType(CType(97, Byte), Integer))
        Me.CmbStatus.ItemHeight = 30
        Me.CmbStatus.Items.AddRange(New Object() {"Semua Status", "Lebih Bayar", "Kurang Bayar", "Nihil"})
        Me.CmbStatus.Location = New System.Drawing.Point(310, 20)
        Me.CmbStatus.Name = "CmbStatus"
        Me.CmbStatus.ShadowDecoration.CustomizableEdges = CustomizableEdges8
        Me.CmbStatus.Size = New System.Drawing.Size(160, 36)
        Me.CmbStatus.TabIndex = 1
        '
        'TxtSearch
        '
        Me.TxtSearch.BorderRadius = 8
        Me.TxtSearch.CustomizableEdges = CustomizableEdges9
        Me.TxtSearch.DefaultText = ""
        Me.TxtSearch.FillColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.TxtSearch.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.TxtSearch.Location = New System.Drawing.Point(18, 20)
        Me.TxtSearch.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TxtSearch.Name = "TxtSearch"
        Me.TxtSearch.PlaceholderText = "Cari nama WP atau NPWP..."
        Me.TxtSearch.SelectedText = ""
        Me.TxtSearch.ShadowDecoration.CustomizableEdges = CustomizableEdges10
        Me.TxtSearch.Size = New System.Drawing.Size(280, 36)
        Me.TxtSearch.TabIndex = 0
        '
        'PanelTable
        '
        Me.PanelTable.BorderColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(233, Byte), Integer), CType(CType(241, Byte), Integer))
        Me.PanelTable.BorderRadius = 10
        Me.PanelTable.BorderThickness = 1
        Me.PanelTable.Controls.Add(Me.LblTableSubtitle)
        Me.PanelTable.Controls.Add(Me.LblTableTitle)
        Me.PanelTable.Controls.Add(Me.GridSPT)
        Me.PanelTable.CustomizableEdges = CustomizableEdges11
        Me.PanelTable.FillColor = System.Drawing.Color.White
        Me.PanelTable.Location = New System.Drawing.Point(210, 216)
        Me.PanelTable.Name = "PanelTable"
        Me.PanelTable.Padding = New System.Windows.Forms.Padding(18)
        Me.PanelTable.ShadowDecoration.CustomizableEdges = CustomizableEdges12
        Me.PanelTable.Size = New System.Drawing.Size(960, 381)
        Me.PanelTable.TabIndex = 3
        '
        'LblTableSubtitle
        '
        Me.LblTableSubtitle.BackColor = System.Drawing.Color.Transparent
        Me.LblTableSubtitle.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LblTableSubtitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(120, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(146, Byte), Integer))
        Me.LblTableSubtitle.Location = New System.Drawing.Point(24, 60)
        Me.LblTableSubtitle.Name = "LblTableSubtitle"
        Me.LblTableSubtitle.Size = New System.Drawing.Size(389, 17)
        Me.LblTableSubtitle.TabIndex = 1
        Me.LblTableSubtitle.Text = "Data SPT terdaftar dengan status pelaporannya."
        '
        'LblTableTitle
        '
        Me.LblTableTitle.BackColor = System.Drawing.Color.Transparent
        Me.LblTableTitle.Font = New System.Drawing.Font("Segoe UI Semibold", 11.0!, System.Drawing.FontStyle.Bold)
        Me.LblTableTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(35, Byte), Integer), CType(CType(44, Byte), Integer), CType(CType(63, Byte), Integer))
        Me.LblTableTitle.Location = New System.Drawing.Point(24, 26)
        Me.LblTableTitle.Name = "LblTableTitle"
        Me.LblTableTitle.Size = New System.Drawing.Size(130, 22)
        Me.LblTableTitle.TabIndex = 0
        Me.LblTableTitle.Text = "Daftar SPT Tahunan"
        '
        'GridSPT
        '
        Me.GridSPT.AllowUserToAddRows = False
        Me.GridSPT.AllowUserToDeleteRows = False
        Me.GridSPT.AllowUserToResizeRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.GridSPT.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(156, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(219, Byte), Integer))
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold)
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(156, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(219, Byte), Integer))
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True
        Me.GridSPT.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.GridSPT.ColumnHeadersHeight = 36
        Me.GridSPT.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colNo, Me.colNamaWP, Me.colNPWP, Me.colTahun, Me.colStatus, Me.colTanggalLapor, Me.colEdit})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(89, Byte), Integer))
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(252, Byte), Integer))
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(35, Byte), Integer), CType(CType(44, Byte), Integer), CType(CType(63, Byte), Integer))
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False
        Me.GridSPT.DefaultCellStyle = DataGridViewCellStyle3
        Me.GridSPT.GridColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.GridSPT.Location = New System.Drawing.Point(24, 96)
        Me.GridSPT.MultiSelect = False
        Me.GridSPT.Name = "GridSPT"
        Me.GridSPT.ReadOnly = True
        Me.GridSPT.RowHeadersVisible = False
        Me.GridSPT.RowHeadersWidth = 51
        Me.GridSPT.RowTemplate.Height = 36
        Me.GridSPT.Size = New System.Drawing.Size(912, 240)
        Me.GridSPT.TabIndex = 2
        Me.GridSPT.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White
        Me.GridSPT.ThemeStyle.AlternatingRowsStyle.Font = Nothing
        Me.GridSPT.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty
        Me.GridSPT.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty
        Me.GridSPT.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty
        Me.GridSPT.ThemeStyle.BackColor = System.Drawing.Color.White
        Me.GridSPT.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(247, Byte), Integer))
        Me.GridSPT.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.GridSPT.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.GridSPT.ThemeStyle.HeaderStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GridSPT.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White
        Me.GridSPT.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.GridSPT.ThemeStyle.HeaderStyle.Height = 36
        Me.GridSPT.ThemeStyle.ReadOnly = True
        Me.GridSPT.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White
        Me.GridSPT.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.GridSPT.ThemeStyle.RowsStyle.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.GridSPT.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.GridSPT.ThemeStyle.RowsStyle.Height = 36
        Me.GridSPT.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.GridSPT.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        '
        'colNo
        '
        Me.colNo.FillWeight = 30.0!
        Me.colNo.HeaderText = "No"
        Me.colNo.MinimumWidth = 6
        Me.colNo.Name = "colNo"
        Me.colNo.ReadOnly = True
        '
        'colNamaWP
        '
        Me.colNamaWP.FillWeight = 70.0!
        Me.colNamaWP.HeaderText = "Nama WP"
        Me.colNamaWP.MinimumWidth = 6
        Me.colNamaWP.Name = "colNamaWP"
        Me.colNamaWP.ReadOnly = True
        '
        'colNPWP
        '
        Me.colNPWP.FillWeight = 50.0!
        Me.colNPWP.HeaderText = "NPWP"
        Me.colNPWP.MinimumWidth = 6
        Me.colNPWP.Name = "colNPWP"
        Me.colNPWP.ReadOnly = True
        '
        'colTahun
        '
        Me.colTahun.FillWeight = 30.0!
        Me.colTahun.HeaderText = "Tahun"
        Me.colTahun.MinimumWidth = 6
        Me.colTahun.Name = "colTahun"
        Me.colTahun.ReadOnly = True
        '
        'colStatus
        '
        Me.colStatus.FillWeight = 50.0!
        Me.colStatus.HeaderText = "Status"
        Me.colStatus.MinimumWidth = 6
        Me.colStatus.Name = "colStatus"
        Me.colStatus.ReadOnly = True
        '
        'colTanggalLapor
        '
        Me.colTanggalLapor.FillWeight = 50.0!
        Me.colTanggalLapor.HeaderText = "Tanggal Lapor"
        Me.colTanggalLapor.MinimumWidth = 6
        Me.colTanggalLapor.Name = "colTanggalLapor"
        Me.colTanggalLapor.ReadOnly = True
        '
        'colEdit
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(122, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle4.ForeColor = System.Drawing.Color.White
        Me.colEdit.DefaultCellStyle = DataGridViewCellStyle4
        Me.colEdit.FillWeight = 40.0!
        Me.colEdit.HeaderText = "Action"
        Me.colEdit.MinimumWidth = 6
        Me.colEdit.Name = "colEdit"
        Me.colEdit.ReadOnly = True
        Me.colEdit.Text = "Ubah Status"
        Me.colEdit.UseColumnTextForButtonValue = True
        '
        'admin_manajemen_spt
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(247, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1200, 720)
        Me.Controls.Add(Me.PanelTable)
        Me.Controls.Add(Me.PanelFilters)
        Me.Controls.Add(Me.PanelHeader)
        Me.Controls.Add(Me.Pk_navbar1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "admin_manajemen_spt"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Manajemen SPT"
        Me.PanelHeader.ResumeLayout(False)
        Me.PanelHeader.PerformLayout()
        Me.PanelFilters.ResumeLayout(False)
        Me.PanelTable.ResumeLayout(False)
        Me.PanelTable.PerformLayout()
        CType(Me.GridSPT, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Pk_navbar1 As app_pajak.admin_navbar
    Friend WithEvents PanelHeader As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents LblTitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents LblSubtitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents PanelFilters As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents TxtSearch As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents CmbStatus As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents CmbTahun As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents PanelTable As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents LblTableTitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents LblTableSubtitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents GridSPT As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents colNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colNamaWP As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colNPWP As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTahun As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colStatus As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTanggalLapor As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colEdit As System.Windows.Forms.DataGridViewButtonColumn
End Class
