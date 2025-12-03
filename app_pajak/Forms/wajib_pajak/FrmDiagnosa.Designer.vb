<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormDiagnosaPajak
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
        lblQuestion = New Label()
        btnYa = New Button()
        btnTidak = New Button()
        panelResult = New Panel()
        txtResult = New TextBox()
        btnRestart = New Button()
        panelResult.SuspendLayout()
        SuspendLayout()
        ' 
        ' lblQuestion
        ' 
        lblQuestion.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        lblQuestion.Location = New Point(20, 20)
        lblQuestion.Name = "lblQuestion"
        lblQuestion.Size = New Size(560, 80)
        lblQuestion.TabIndex = 0
        lblQuestion.Text = "Pertanyaan muncul di sini"
        ' 
        ' btnYa
        ' 
        btnYa.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        btnYa.Location = New Point(100, 120)
        btnYa.Name = "btnYa"
        btnYa.Size = New Size(120, 40)
        btnYa.TabIndex = 1
        btnYa.Text = "YA"
        btnYa.UseVisualStyleBackColor = True
        ' 
        ' btnTidak
        ' 
        btnTidak.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        btnTidak.Location = New Point(310, 120)
        btnTidak.Name = "btnTidak"
        btnTidak.Size = New Size(120, 40)
        btnTidak.TabIndex = 2
        btnTidak.Text = "TIDAK"
        btnTidak.UseVisualStyleBackColor = True
        ' 
        ' panelResult
        ' 
        panelResult.BorderStyle = BorderStyle.FixedSingle
        panelResult.Controls.Add(txtResult)
        panelResult.Location = New Point(20, 190)
        panelResult.Name = "panelResult"
        panelResult.Size = New Size(560, 250)
        panelResult.TabIndex = 3
        panelResult.Visible = False
        ' 
        ' txtResult
        ' 
        txtResult.Dock = DockStyle.Fill
        txtResult.Font = New Font("Segoe UI", 10F)
        txtResult.Location = New Point(0, 0)
        txtResult.Multiline = True
        txtResult.Name = "txtResult"
        txtResult.ReadOnly = True
        txtResult.ScrollBars = ScrollBars.Vertical
        txtResult.Size = New Size(558, 248)
        txtResult.TabIndex = 0
        ' 
        ' btnRestart
        ' 
        btnRestart.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        btnRestart.Location = New Point(210, 460)
        btnRestart.Name = "btnRestart"
        btnRestart.Size = New Size(160, 40)
        btnRestart.TabIndex = 4
        btnRestart.Text = "Ulangi Diagnosa"
        btnRestart.UseVisualStyleBackColor = True
        btnRestart.Visible = False
        ' 
        ' FormDiagnosaPajak
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.None
        ClientSize = New Size(598, 530)
        Controls.Add(btnRestart)
        Controls.Add(panelResult)
        Controls.Add(btnTidak)
        Controls.Add(btnYa)
        Controls.Add(lblQuestion)
        FormBorderStyle = FormBorderStyle.FixedDialog
        Name = "FormDiagnosaPajak"
        StartPosition = FormStartPosition.CenterScreen
        Text = "+++++++++++++++++++++++++++++++++"
        panelResult.ResumeLayout(False)
        panelResult.PerformLayout()
        ResumeLayout(False)

    End Sub

    Friend WithEvents lblQuestion As Label
    Friend WithEvents btnYa As Button
    Friend WithEvents btnTidak As Button
    Friend WithEvents panelResult As Panel
    Friend WithEvents txtResult As TextBox
    Friend WithEvents btnRestart As Button
End Class
