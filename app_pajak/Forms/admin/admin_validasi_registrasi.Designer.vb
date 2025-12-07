<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class admin_validasi_registrasi
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
        Dim CustomizableEdges1 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges2 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges3 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges4 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges5 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges6 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As DataGridViewCellStyle = New DataGridViewCellStyle()
        pnlMain = New Guna.UI2.WinForms.Guna2Panel()
        pnlHeader = New Guna.UI2.WinForms.Guna2Panel()
        lblSubtitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        lblTitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        pnlContent = New Guna.UI2.WinForms.Guna2Panel()
        lblPending = New Guna.UI2.WinForms.Guna2HtmlLabel()
        GridPending = New Guna.UI2.WinForms.Guna2DataGridView()
        pnlActions = New Panel()
        btnApprove = New Guna.UI2.WinForms.Guna2Button()
        btnReject = New Guna.UI2.WinForms.Guna2Button()
        btnBack = New Guna.UI2.WinForms.Guna2Button()
        btnRefresh = New Guna.UI2.WinForms.Guna2Button()
        pnlMain.SuspendLayout()
        pnlHeader.SuspendLayout()
        pnlContent.SuspendLayout()
        CType(GridPending, ComponentModel.ISupportInitialize).BeginInit()
        pnlActions.SuspendLayout()
        SuspendLayout()
        ' 
        ' pnlMain
        ' 
        pnlMain.BackColor = Color.FromArgb(CByte(247), CByte(248), CByte(252))
        pnlMain.Controls.Add(pnlHeader)
        pnlMain.Controls.Add(pnlContent)
        pnlMain.CustomizableEdges = CustomizableEdges1
        pnlMain.Dock = DockStyle.Fill
        pnlMain.Location = New Point(0, 0)
        pnlMain.Name = "pnlMain"
        pnlMain.Padding = New Padding(24)
        pnlMain.ShadowDecoration.CustomizableEdges = CustomizableEdges2
        pnlMain.Size = New Size(900, 600)
        pnlMain.TabIndex = 0
        ' 
        ' pnlHeader
        ' 
        pnlHeader.BorderRadius = 12
        pnlHeader.Controls.Add(lblSubtitle)
        pnlHeader.Controls.Add(lblTitle)
        pnlHeader.CustomizableEdges = CustomizableEdges3
        pnlHeader.FillColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        pnlHeader.Location = New Point(24, 24)
        pnlHeader.Name = "pnlHeader"
        pnlHeader.ShadowDecoration.CustomizableEdges = CustomizableEdges4
        pnlHeader.Size = New Size(852, 80)
        pnlHeader.TabIndex = 0
        ' 
        ' lblTitle
        ' 
        lblTitle.BackColor = Color.Transparent
        lblTitle.Font = New Font("Segoe UI Semibold", 14.0F, FontStyle.Bold)
        lblTitle.ForeColor = Color.White
        lblTitle.Location = New Point(24, 16)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(200, 27)
        lblTitle.TabIndex = 0
        lblTitle.Text = "Validasi Registrasi User"
        ' 
        ' lblSubtitle
        ' 
        lblSubtitle.BackColor = Color.Transparent
        lblSubtitle.Font = New Font("Segoe UI", 9.0F)
        lblSubtitle.ForeColor = Color.FromArgb(CByte(233), CByte(221), CByte(255))
        lblSubtitle.Location = New Point(24, 48)
        lblSubtitle.Name = "lblSubtitle"
        lblSubtitle.Size = New Size(300, 17)
        lblSubtitle.TabIndex = 1
        lblSubtitle.Text = "Approve atau tolak pendaftaran wajib pajak baru"
        ' 
        ' pnlContent
        ' 
        pnlContent.BorderColor = Color.FromArgb(CByte(230), CByte(233), CByte(241))
        pnlContent.BorderRadius = 12
        pnlContent.BorderThickness = 1
        pnlContent.Controls.Add(lblPending)
        pnlContent.Controls.Add(GridPending)
        pnlContent.Controls.Add(pnlActions)
        pnlContent.CustomizableEdges = CustomizableEdges5
        pnlContent.FillColor = Color.White
        pnlContent.Location = New Point(24, 120)
        pnlContent.Name = "pnlContent"
        pnlContent.ShadowDecoration.CustomizableEdges = CustomizableEdges6
        pnlContent.Size = New Size(852, 456)
        pnlContent.TabIndex = 1
        ' 
        ' lblPending
        ' 
        lblPending.BackColor = Color.Transparent
        lblPending.Font = New Font("Segoe UI Semibold", 11.0F, FontStyle.Bold)
        lblPending.ForeColor = Color.FromArgb(CByte(35), CByte(44), CByte(63))
        lblPending.Location = New Point(24, 20)
        lblPending.Name = "lblPending"
        lblPending.Size = New Size(200, 22)
        lblPending.TabIndex = 0
        lblPending.Text = "Daftar Registrasi Pending"
        ' 
        ' GridPending
        ' 
        GridPending.AllowUserToAddRows = False
        GridPending.AllowUserToDeleteRows = False
        GridPending.AllowUserToResizeRows = False
        DataGridViewCellStyle1.BackColor = Color.White
        GridPending.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        DataGridViewCellStyle2.Font = New Font("Segoe UI Semibold", 9.0F, FontStyle.Bold)
        DataGridViewCellStyle2.ForeColor = Color.White
        DataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        DataGridViewCellStyle2.SelectionForeColor = Color.White
        GridPending.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        GridPending.ColumnHeadersHeight = 36
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = Color.White
        DataGridViewCellStyle3.Font = New Font("Segoe UI", 9.0F)
        DataGridViewCellStyle3.ForeColor = Color.FromArgb(CByte(71), CByte(69), CByte(94))
        DataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(CByte(231), CByte(229), CByte(255))
        DataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(CByte(71), CByte(69), CByte(94))
        GridPending.DefaultCellStyle = DataGridViewCellStyle3
        GridPending.GridColor = Color.FromArgb(CByte(231), CByte(229), CByte(255))
        GridPending.Location = New Point(24, 50)
        GridPending.MultiSelect = False
        GridPending.Name = "GridPending"
        GridPending.ReadOnly = True
        GridPending.RowHeadersVisible = False
        GridPending.RowTemplate.Height = 36
        GridPending.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        GridPending.Size = New Size(804, 340)
        GridPending.TabIndex = 1
        GridPending.ThemeStyle.BackColor = Color.White
        GridPending.ThemeStyle.GridColor = Color.FromArgb(CByte(231), CByte(229), CByte(255))
        GridPending.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(CByte(156), CByte(0), CByte(219))
        GridPending.ThemeStyle.HeaderStyle.ForeColor = Color.White
        GridPending.ThemeStyle.HeaderStyle.Height = 36
        GridPending.ThemeStyle.RowsStyle.BackColor = Color.White
        GridPending.ThemeStyle.RowsStyle.Height = 36
        ' 
        ' pnlActions
        ' 
        pnlActions.BackColor = Color.Transparent
        pnlActions.Controls.Add(btnBack)
        pnlActions.Controls.Add(btnRefresh)
        pnlActions.Controls.Add(btnApprove)
        pnlActions.Controls.Add(btnReject)
        pnlActions.Location = New Point(24, 400)
        pnlActions.Name = "pnlActions"
        pnlActions.Size = New Size(804, 44)
        pnlActions.TabIndex = 2
        ' 
        ' btnApprove
        ' 
        btnApprove.BorderRadius = 8
        btnApprove.FillColor = Color.FromArgb(CByte(52), CByte(199), CByte(89))
        btnApprove.Font = New Font("Segoe UI Semibold", 9.0F, FontStyle.Bold)
        btnApprove.ForeColor = Color.White
        btnApprove.Location = New Point(524, 0)
        btnApprove.Name = "btnApprove"
        btnApprove.Size = New Size(130, 40)
        btnApprove.TabIndex = 0
        btnApprove.Text = "✓ Approve"
        ' 
        ' btnReject
        ' 
        btnReject.BorderRadius = 8
        btnReject.FillColor = Color.FromArgb(CByte(255), CByte(59), CByte(48))
        btnReject.Font = New Font("Segoe UI Semibold", 9.0F, FontStyle.Bold)
        btnReject.ForeColor = Color.White
        btnReject.Location = New Point(664, 0)
        btnReject.Name = "btnReject"
        btnReject.Size = New Size(130, 40)
        btnReject.TabIndex = 1
        btnReject.Text = "✗ Reject"
        ' 
        ' btnBack
        ' 
        btnBack.BorderRadius = 8
        btnBack.FillColor = Color.FromArgb(CByte(120), CByte(128), CByte(146))
        btnBack.Font = New Font("Segoe UI Semibold", 9.0F, FontStyle.Bold)
        btnBack.ForeColor = Color.White
        btnBack.Location = New Point(0, 0)
        btnBack.Name = "btnBack"
        btnBack.Size = New Size(100, 40)
        btnBack.TabIndex = 2
        btnBack.Text = "← Kembali"
        ' 
        ' btnRefresh
        ' 
        btnRefresh.BorderRadius = 8
        btnRefresh.FillColor = Color.FromArgb(CByte(0), CByte(122), CByte(255))
        btnRefresh.Font = New Font("Segoe UI Semibold", 9.0F, FontStyle.Bold)
        btnRefresh.ForeColor = Color.White
        btnRefresh.Location = New Point(110, 0)
        btnRefresh.Name = "btnRefresh"
        btnRefresh.Size = New Size(100, 40)
        btnRefresh.TabIndex = 3
        btnRefresh.Text = "↻ Refresh"
        ' 
        ' admin_validasi_registrasi
        ' 
        AutoScaleMode = AutoScaleMode.None
        BackColor = Color.FromArgb(CByte(247), CByte(248), CByte(252))
        ClientSize = New Size(900, 600)
        Controls.Add(pnlMain)
        FormBorderStyle = FormBorderStyle.None
        Name = "admin_validasi_registrasi"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Validasi Registrasi"
        pnlMain.ResumeLayout(False)
        pnlHeader.ResumeLayout(False)
        pnlHeader.PerformLayout()
        pnlContent.ResumeLayout(False)
        pnlContent.PerformLayout()
        CType(GridPending, ComponentModel.ISupportInitialize).EndInit()
        pnlActions.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlMain As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents pnlHeader As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents lblTitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents lblSubtitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents pnlContent As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents lblPending As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents GridPending As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents pnlActions As Panel
    Friend WithEvents btnApprove As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnReject As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnBack As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnRefresh As Guna.UI2.WinForms.Guna2Button

End Class
