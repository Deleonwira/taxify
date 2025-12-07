<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class pk_riwayat_bukti_potong
    Inherits System.Windows.Forms.Form

    ' Designer generated - do not modify by hand outside of Visual Studio Designer region.
    Private components As System.ComponentModel.IContainer

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
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As DataGridViewCellStyle = New DataGridViewCellStyle()
        
        Pk_navbar11 = New pk_navbar1()
        PanelMain = New Guna.UI2.WinForms.Guna2Panel()
        PanelTable = New Guna.UI2.WinForms.Guna2Panel()
        TxtSearch = New Guna.UI2.WinForms.Guna2TextBox()
        LblTableDesc = New Guna.UI2.WinForms.Guna2HtmlLabel()
        BtnFilter = New Guna.UI2.WinForms.Guna2Button()
        LblTableTitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        GridBuktiPotong = New Guna.UI2.WinForms.Guna2DataGridView()
        colPeriode = New DataGridViewTextBoxColumn()
        colPegawai = New DataGridViewTextBoxColumn()
        colNPWP = New DataGridViewTextBoxColumn()
        colBruto = New DataGridViewTextBoxColumn()
        colNeto = New DataGridViewTextBoxColumn()
        colPPh = New DataGridViewTextBoxColumn()
        colDetail = New DataGridViewButtonColumn()
        FlowPegawai = New FlowLayoutPanel()
        PanelHeader = New Guna.UI2.WinForms.Guna2Panel()
        LblSubtitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        LblTitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        
        PanelMain.SuspendLayout()
        PanelTable.SuspendLayout()
        CType(GridBuktiPotong, ComponentModel.ISupportInitialize).BeginInit()
        PanelHeader.SuspendLayout()
        SuspendLayout()
        
        ' 
        ' Pk_navbar11
        ' 
        Pk_navbar11.BackColor = Color.FromArgb(CByte(84), CByte(64), CByte(209))
        Pk_navbar11.Dock = DockStyle.Left
        Pk_navbar11.ForeColor = Color.White
        Pk_navbar11.Location = New Point(0, 0)
        Pk_navbar11.Margin = New Padding(3, 4, 3, 4)
        Pk_navbar11.Name = "Pk_navbar11"
        Pk_navbar11.Size = New Size(191, 720)
        Pk_navbar11.TabIndex = 0
        
        ' 
        ' PanelMain
        ' 
        PanelMain.AutoScroll = True
        PanelMain.BackColor = Color.FromArgb(CByte(247), CByte(248), CByte(252))
        PanelMain.Controls.Add(PanelTable)
        PanelMain.Controls.Add(FlowPegawai)
        PanelMain.Controls.Add(PanelHeader)
        PanelMain.CustomizableEdges = CustomizableEdges1
        PanelMain.Dock = DockStyle.Fill
        PanelMain.Location = New Point(191, 0)
        PanelMain.Name = "PanelMain"
        PanelMain.Padding = New Padding(24)
        PanelMain.ShadowDecoration.CustomizableEdges = CustomizableEdges2
        PanelMain.Size = New Size(1109, 720)
        PanelMain.TabIndex = 1
        
        ' 
        ' PanelHeader
        ' 
        PanelHeader.BorderColor = Color.FromArgb(CByte(230), CByte(233), CByte(241))
        PanelHeader.BorderRadius = 12
        PanelHeader.BorderThickness = 1
        PanelHeader.Controls.Add(LblSubtitle)
        PanelHeader.Controls.Add(LblTitle)
        PanelHeader.CustomizableEdges = CustomizableEdges3
        PanelHeader.FillColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        PanelHeader.Location = New Point(24, 24)
        PanelHeader.Name = "PanelHeader"
        PanelHeader.Padding = New Padding(24)
        PanelHeader.ShadowDecoration.CustomizableEdges = CustomizableEdges4
        PanelHeader.Size = New Size(1061, 91)
        PanelHeader.TabIndex = 1
        
        ' 
        ' LblTitle
        ' 
        LblTitle.BackColor = Color.Transparent
        LblTitle.Font = New Font("Segoe UI Semibold", 14.0F, FontStyle.Bold)
        LblTitle.ForeColor = Color.White
        LblTitle.Location = New Point(24, 14)
        LblTitle.Name = "LblTitle"
        LblTitle.Size = New Size(187, 27)
        LblTitle.TabIndex = 0
        LblTitle.Text = "Riwayat Bukti Potong"
        
        ' 
        ' LblSubtitle
        ' 
        LblSubtitle.BackColor = Color.Transparent
        LblSubtitle.Font = New Font("Segoe UI", 9.0F)
        LblSubtitle.ForeColor = Color.FromArgb(CByte(233), CByte(221), CByte(255))
        LblSubtitle.Location = New Point(24, 48)
        LblSubtitle.Name = "LblSubtitle"
        LblSubtitle.Size = New Size(392, 17)
        LblSubtitle.TabIndex = 1
        LblSubtitle.Text = "Lihat riwayat bukti potong pegawai, filter berdasarkan pegawai atau periode."
        
        ' 
        ' FlowPegawai
        ' 
        FlowPegawai.AutoScroll = True
        FlowPegawai.Location = New Point(24, 130)
        FlowPegawai.Name = "FlowPegawai"
        FlowPegawai.Padding = New Padding(4)
        FlowPegawai.Size = New Size(1061, 125)
        FlowPegawai.TabIndex = 2
        FlowPegawai.WrapContents = False
        
        ' 
        ' PanelTable
        ' 
        PanelTable.AutoScroll = True
        PanelTable.BorderColor = Color.FromArgb(CByte(230), CByte(233), CByte(241))
        PanelTable.BorderRadius = 12
        PanelTable.BorderThickness = 1
        PanelTable.Controls.Add(TxtSearch)
        PanelTable.Controls.Add(LblTableDesc)
        PanelTable.Controls.Add(BtnFilter)
        PanelTable.Controls.Add(LblTableTitle)
        PanelTable.Controls.Add(GridBuktiPotong)
        PanelTable.CustomizableEdges = CustomizableEdges5
        PanelTable.FillColor = Color.White
        PanelTable.Location = New Point(24, 261)
        PanelTable.Name = "PanelTable"
        PanelTable.Padding = New Padding(24)
        PanelTable.ShadowDecoration.CustomizableEdges = CustomizableEdges6
        PanelTable.Size = New Size(1061, 356)
        PanelTable.TabIndex = 3
        
        ' 
        ' LblTableTitle
        ' 
        LblTableTitle.BackColor = Color.Transparent
        LblTableTitle.Font = New Font("Segoe UI Semibold", 11.0F, FontStyle.Bold)
        LblTableTitle.ForeColor = Color.FromArgb(CByte(35), CByte(44), CByte(63))
        LblTableTitle.Location = New Point(24, 26)
        LblTableTitle.Name = "LblTableTitle"
        LblTableTitle.Size = New Size(176, 22)
        LblTableTitle.TabIndex = 0
        LblTableTitle.Text = "Bukti Potong per Periode"
        
        ' 
        ' LblTableDesc
        ' 
        LblTableDesc.BackColor = Color.Transparent
        LblTableDesc.Font = New Font("Segoe UI", 9.0F)
        LblTableDesc.ForeColor = Color.FromArgb(CByte(120), CByte(128), CByte(146))
        LblTableDesc.Location = New Point(24, 60)
        LblTableDesc.Name = "LblTableDesc"
        LblTableDesc.Size = New Size(302, 17)
        LblTableDesc.TabIndex = 1
        LblTableDesc.Text = "Daftar bukti potong yang dibuat untuk pegawai perusahaan."
        
        ' 
        ' TxtSearch
        ' 
        TxtSearch.BorderRadius = 8
        TxtSearch.CustomizableEdges = CustomizableEdges7
        TxtSearch.DefaultText = ""
        TxtSearch.FillColor = Color.FromArgb(CByte(245), CByte(246), CByte(250))
        TxtSearch.Font = New Font("Segoe UI", 9.0F)
        TxtSearch.Location = New Point(660, 28)
        TxtSearch.Margin = New Padding(3, 4, 3, 4)
        TxtSearch.Name = "TxtSearch"
        TxtSearch.PlaceholderText = "Cari pegawai / periode"
        TxtSearch.SelectedText = ""
        TxtSearch.ShadowDecoration.CustomizableEdges = CustomizableEdges8
        TxtSearch.Size = New Size(314, 36)
        TxtSearch.TabIndex = 2
        
        ' 
        ' BtnFilter
        ' 
        BtnFilter.BorderRadius = 8
        BtnFilter.FillColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        BtnFilter.Font = New Font("Segoe UI Semibold", 9.0F, FontStyle.Bold)
        BtnFilter.ForeColor = Color.White
        BtnFilter.Location = New Point(980, 27)
        BtnFilter.Name = "BtnFilter"
        BtnFilter.Size = New Size(62, 36)
        BtnFilter.TabIndex = 3
        BtnFilter.Text = "Cari"
        
        ' 
        ' GridBuktiPotong
        ' 
        GridBuktiPotong.AllowUserToAddRows = False
        GridBuktiPotong.AllowUserToDeleteRows = False
        GridBuktiPotong.AllowUserToResizeRows = False
        DataGridViewCellStyle1.BackColor = Color.White
        GridBuktiPotong.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        DataGridViewCellStyle2.Font = New Font("Segoe UI Semibold", 9.0F, FontStyle.Bold)
        DataGridViewCellStyle2.ForeColor = Color.White
        DataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        DataGridViewCellStyle2.SelectionForeColor = Color.White
        DataGridViewCellStyle2.WrapMode = DataGridViewTriState.True
        GridBuktiPotong.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        GridBuktiPotong.ColumnHeadersHeight = 36
        GridBuktiPotong.Columns.AddRange(New DataGridViewColumn() {colPeriode, colPegawai, colNPWP, colBruto, colNeto, colPPh, colDetail})
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = Color.White
        DataGridViewCellStyle3.Font = New Font("Segoe UI", 9.0F)
        DataGridViewCellStyle3.ForeColor = Color.FromArgb(CByte(71), CByte(69), CByte(94))
        DataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(CByte(231), CByte(229), CByte(255))
        DataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(CByte(71), CByte(69), CByte(94))
        DataGridViewCellStyle3.WrapMode = DataGridViewTriState.False
        GridBuktiPotong.DefaultCellStyle = DataGridViewCellStyle3
        GridBuktiPotong.GridColor = Color.FromArgb(CByte(231), CByte(229), CByte(255))
        GridBuktiPotong.Location = New Point(24, 100)
        GridBuktiPotong.MultiSelect = False
        GridBuktiPotong.Name = "GridBuktiPotong"
        GridBuktiPotong.ReadOnly = True
        GridBuktiPotong.RowHeadersVisible = False
        GridBuktiPotong.RowHeadersWidth = 51
        DataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter
        GridBuktiPotong.RowsDefaultCellStyle = DataGridViewCellStyle4
        GridBuktiPotong.RowTemplate.Height = 36
        GridBuktiPotong.Size = New Size(1018, 240)
        GridBuktiPotong.TabIndex = 4
        GridBuktiPotong.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White
        GridBuktiPotong.ThemeStyle.AlternatingRowsStyle.Font = Nothing
        GridBuktiPotong.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty
        GridBuktiPotong.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty
        GridBuktiPotong.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty
        GridBuktiPotong.ThemeStyle.BackColor = Color.White
        GridBuktiPotong.ThemeStyle.GridColor = Color.FromArgb(CByte(231), CByte(229), CByte(255))
        GridBuktiPotong.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        GridBuktiPotong.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None
        GridBuktiPotong.ThemeStyle.HeaderStyle.Font = New Font("Segoe UI Semibold", 9.0F, FontStyle.Bold)
        GridBuktiPotong.ThemeStyle.HeaderStyle.ForeColor = Color.White
        GridBuktiPotong.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        GridBuktiPotong.ThemeStyle.HeaderStyle.Height = 36
        GridBuktiPotong.ThemeStyle.ReadOnly = True
        GridBuktiPotong.ThemeStyle.RowsStyle.BackColor = Color.White
        GridBuktiPotong.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        GridBuktiPotong.ThemeStyle.RowsStyle.Font = New Font("Segoe UI", 9.0F)
        GridBuktiPotong.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(CByte(71), CByte(69), CByte(94))
        GridBuktiPotong.ThemeStyle.RowsStyle.Height = 36
        GridBuktiPotong.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(CByte(231), CByte(229), CByte(255))
        GridBuktiPotong.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(CByte(71), CByte(69), CByte(94))
        
        ' 
        ' colPeriode
        ' 
        colPeriode.HeaderText = "Periode"
        colPeriode.MinimumWidth = 80
        colPeriode.Name = "colPeriode"
        colPeriode.ReadOnly = True
        colPeriode.Width = 80
        
        ' 
        ' colPegawai
        ' 
        colPegawai.HeaderText = "Pegawai"
        colPegawai.MinimumWidth = 150
        colPegawai.Name = "colPegawai"
        colPegawai.ReadOnly = True
        colPegawai.Width = 180
        
        ' 
        ' colNPWP
        ' 
        colNPWP.HeaderText = "NPWP"
        colNPWP.MinimumWidth = 120
        colNPWP.Name = "colNPWP"
        colNPWP.ReadOnly = True
        colNPWP.Width = 150
        
        ' 
        ' colBruto
        ' 
        colBruto.HeaderText = "Bruto"
        colBruto.MinimumWidth = 100
        colBruto.Name = "colBruto"
        colBruto.ReadOnly = True
        colBruto.Width = 120
        
        ' 
        ' colNeto
        ' 
        colNeto.HeaderText = "Neto"
        colNeto.MinimumWidth = 100
        colNeto.Name = "colNeto"
        colNeto.ReadOnly = True
        colNeto.Width = 120
        
        ' 
        ' colPPh
        ' 
        colPPh.HeaderText = "PPh21"
        colPPh.MinimumWidth = 100
        colPPh.Name = "colPPh"
        colPPh.ReadOnly = True
        colPPh.Width = 120
        
        ' 
        ' colDetail
        ' 
        DataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle5.BackColor = Color.FromArgb(CByte(0), CByte(122), CByte(255))
        DataGridViewCellStyle5.ForeColor = Color.White
        colDetail.DefaultCellStyle = DataGridViewCellStyle5
        colDetail.HeaderText = ""
        colDetail.MinimumWidth = 70
        colDetail.Name = "colDetail"
        colDetail.ReadOnly = True
        colDetail.Text = "Detail"
        colDetail.UseColumnTextForButtonValue = True
        colDetail.Width = 80
        
        ' 
        ' pk_riwayat_bukti_potong
        ' 
        AutoScaleMode = AutoScaleMode.None
        BackColor = Color.FromArgb(CByte(247), CByte(248), CByte(252))
        ClientSize = New Size(1300, 720)
        Controls.Add(PanelMain)
        Controls.Add(Pk_navbar11)
        FormBorderStyle = FormBorderStyle.None
        Name = "pk_riwayat_bukti_potong"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Riwayat Bukti Potong"
        PanelMain.ResumeLayout(False)
        PanelTable.ResumeLayout(False)
        PanelTable.PerformLayout()
        CType(GridBuktiPotong, ComponentModel.ISupportInitialize).EndInit()
        PanelHeader.ResumeLayout(False)
        PanelHeader.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents Pk_navbar11 As pk_navbar1
    Friend WithEvents PanelMain As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents PanelHeader As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents LblTitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents LblSubtitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents FlowPegawai As FlowLayoutPanel
    Friend WithEvents PanelTable As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents LblTableTitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents LblTableDesc As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents TxtSearch As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents BtnFilter As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents GridBuktiPotong As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents colPeriode As DataGridViewTextBoxColumn
    Friend WithEvents colPegawai As DataGridViewTextBoxColumn
    Friend WithEvents colNPWP As DataGridViewTextBoxColumn
    Friend WithEvents colBruto As DataGridViewTextBoxColumn
    Friend WithEvents colNeto As DataGridViewTextBoxColumn
    Friend WithEvents colPPh As DataGridViewTextBoxColumn
    Friend WithEvents colDetail As DataGridViewButtonColumn
End Class
