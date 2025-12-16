Imports System.Text
Imports System.Collections.Generic

Public Class chatbot_control

    ' Event untuk menangani jawaban user (Ya/Tidak)
    Public Event AnswerSelected(sender As Object, answer As String)

    Private Sub chatbot_control_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Inisialisasi control
        pnlChatArea.AutoScroll = True
        
        If ModuleChatbot.MessageHistory.Count = 0 Then
            ' Sesi baru
            ResetChat()
        Else
            ' Restore sesi
            RefreshChatDisplay()
            
            ' Re-attach handlers jika engine aktif
            If ModuleChatbot.IsDiagnosisActive AndAlso ModuleChatbot.DiagnosisEngine IsNot Nothing Then
                AddHandler ModuleChatbot.DiagnosisEngine.QuestionChanged, AddressOf OnEngineQuestionChanged
                AddHandler ModuleChatbot.DiagnosisEngine.Completed, AddressOf OnEngineCompleted
                
                ' Tampilkan tombol jika ada pertanyaan aktif
                If ModuleChatbot.CurrentQuestionObj IsNot Nothing Then
                    ShowYesNoButtons()
                Else
                    HideYesNoButtons()
                End If
            Else
                HideYesNoButtons()
            End If
        End If
    End Sub

    ' Method untuk reset chat dan mulai baru
    Private Sub ResetChat()
        ModuleChatbot.ResetChatbot()
        pnlChatArea.Controls.Clear()
        HideYesNoButtons()
        
        ' Pesan selamat datang (tanpa tombol)
        AddBotMessage("Halo! Saya adalah asisten chatbot untuk membantu Anda dengan diagnosis pajak. Saya akan mengajukan beberapa pertanyaan yang hanya dapat dijawab dengan Ya atau Tidak.", False)
        
        ' Mulai diagnosis otomatis
        StartDiagnosis()
    End Sub

    ' Method untuk memulai diagnosis
    Public Sub StartDiagnosis()
        ' Buat engine baru via ModuleChatbot, atau reset jika sudah ada namun tidak aktif
        If ModuleChatbot.DiagnosisEngine Is Nothing Then
            ModuleChatbot.DiagnosisEngine = New PajakDiagnosisEngine()
        Else
            ModuleChatbot.DiagnosisEngine.Reset()
        End If

        ' Subscribe ke events
        RemoveHandler ModuleChatbot.DiagnosisEngine.QuestionChanged, AddressOf OnEngineQuestionChanged
        RemoveHandler ModuleChatbot.DiagnosisEngine.Completed, AddressOf OnEngineCompleted
        AddHandler ModuleChatbot.DiagnosisEngine.QuestionChanged, AddressOf OnEngineQuestionChanged
        AddHandler ModuleChatbot.DiagnosisEngine.Completed, AddressOf OnEngineCompleted

        ModuleChatbot.IsDiagnosisActive = True

        ' Ambil pertanyaan pertama
        ModuleChatbot.CurrentQuestionObj = ModuleChatbot.DiagnosisEngine.GetNextQuestion()
        If ModuleChatbot.CurrentQuestionObj IsNot Nothing Then
            AddBotMessage(ModuleChatbot.CurrentQuestionObj.Text)
        End If
    End Sub

    ' Method untuk menambahkan pesan bot
    Public Sub AddBotMessage(message As String, Optional showButtons As Boolean = True)
        ModuleChatbot.MessageHistory.Add(New ModuleChatbot.ChatMessage(message, True))
        RefreshChatDisplay()
        
        If showButtons Then
            ShowYesNoButtons()
        Else
            HideYesNoButtons()
        End If
    End Sub

    ' Method untuk menambahkan pesan user (Ya/Tidak)
    Private Sub AddUserMessage(message As String)
        ModuleChatbot.MessageHistory.Add(New ModuleChatbot.ChatMessage(message, False))
        RefreshChatDisplay()
        HideYesNoButtons()
    End Sub

    ' Method untuk refresh tampilan chat
    Private Sub RefreshChatDisplay()
        pnlChatArea.Controls.Clear()

        ' Pastikan panel sudah memiliki ukuran
        If pnlChatArea.Width <= 0 Then
            Return
        End If

        Dim yPosition As Integer = 10
        Dim maxWidth As Integer = Math.Max(pnlChatArea.Width - 40, 200) ' Minimum width

        For Each msg As ModuleChatbot.ChatMessage In ModuleChatbot.MessageHistory
            If msg.IsBot Then
                ' Bot message bubble (kiri)
                Dim bubble As Guna.UI2.WinForms.Guna2Panel = CreateBotBubble(msg.Text, maxWidth)
                bubble.Location = New Point(10, yPosition)
                pnlChatArea.Controls.Add(bubble)
                yPosition += bubble.Height + 10
            Else
                ' User message bubble (kanan)
                Dim bubble As Guna.UI2.WinForms.Guna2Panel = CreateUserBubble(msg.Text, maxWidth)
                bubble.Location = New Point(pnlChatArea.Width - bubble.Width - 10, yPosition)
                pnlChatArea.Controls.Add(bubble)
                yPosition += bubble.Height + 10
            End If
        Next

        ' Set minimum scroll size untuk enable scrolling
        If yPosition > pnlChatArea.Height Then
            pnlChatArea.AutoScrollMinSize = New Size(0, yPosition + 10)
        End If

        ' Scroll ke bawah setelah semua kontrol ditambahkan
        Application.DoEvents()
        pnlChatArea.PerformLayout()
        
        If pnlChatArea.VerticalScroll.Maximum > 0 Then
            pnlChatArea.VerticalScroll.Value = pnlChatArea.VerticalScroll.Maximum
        End If
    End Sub

    ' Method untuk membuat bot message bubble
    Private Function CreateBotBubble(text As String, maxWidth As Integer) As Guna.UI2.WinForms.Guna2Panel
        Dim bubble As New Guna.UI2.WinForms.Guna2Panel()
        bubble.BackColor = Color.Transparent
        bubble.FillColor = Color.FromArgb(245, 245, 250) ' Light gray/lavender
        bubble.BorderRadius = 10
        bubble.Padding = New Padding(12, 10, 12, 10)

        ' Avatar bot
        Dim avatar As New Guna.UI2.WinForms.Guna2CirclePictureBox()
        avatar.Size = New Size(32, 32)
        avatar.FillColor = Color.FromArgb(106, 90, 232) ' Purple
        avatar.SizeMode = PictureBoxSizeMode.CenterImage
        avatar.BackColor = Color.Transparent
        avatar.Enabled = False
        bubble.Controls.Add(avatar)

        ' Label untuk text
        Dim lblText As New Guna.UI2.WinForms.Guna2HtmlLabel()
        lblText.Text = text
        lblText.Location = New Point(42, 5)
        lblText.ForeColor = Color.FromArgb(64, 64, 64)
        lblText.Font = New Font("Segoe UI", 9.0F)
        lblText.BackColor = Color.Transparent
        lblText.AutoSize = False
        bubble.Controls.Add(lblText)

        ' Ukur text dengan TextRenderer
        Dim font As Font = New Font("Segoe UI", 9.0F)
        Dim textWidth As Integer = maxWidth - 70
        Dim textSize As Size = TextRenderer.MeasureText(text, font, New Size(textWidth, Integer.MaxValue), TextFormatFlags.WordBreak Or TextFormatFlags.TextBoxControl)
        
        Dim labelWidth As Integer = Math.Min(textSize.Width + 5, textWidth)
        Dim labelHeight As Integer = Math.Max(textSize.Height + 5, 20)
        
        lblText.Size = New Size(labelWidth, labelHeight)
        
        ' Hitung tinggi bubble
        Dim bubbleHeight As Integer = Math.Max(labelHeight + 10, 42)
        bubble.Size = New Size(labelWidth + 60, bubbleHeight)
        
        ' Posisi ulang avatar di tengah vertikal
        avatar.Location = New Point(5, (bubbleHeight - 32) \ 2)

        Return bubble
    End Function

    ' Method untuk membuat user message bubble
    Private Function CreateUserBubble(text As String, maxWidth As Integer) As Guna.UI2.WinForms.Guna2Panel
        Dim bubble As New Guna.UI2.WinForms.Guna2Panel()
        bubble.BackColor = Color.Transparent
        bubble.FillColor = Color.FromArgb(106, 90, 232) ' Purple
        bubble.BorderRadius = 10
        bubble.Padding = New Padding(12, 10, 12, 10)

        ' Label untuk text
        Dim lblText As New Guna.UI2.WinForms.Guna2HtmlLabel()
        lblText.Text = text
        lblText.Location = New Point(5, 5)
        lblText.ForeColor = Color.White
        lblText.Font = New Font("Segoe UI", 9.0F, FontStyle.Regular)
        lblText.BackColor = Color.Transparent
        lblText.AutoSize = False
        bubble.Controls.Add(lblText)

        ' Ukur text dengan TextRenderer
        Dim font As Font = New Font("Segoe UI", 9.0F, FontStyle.Regular)
        Dim textWidth As Integer = maxWidth - 70
        Dim textSize As Size = TextRenderer.MeasureText(text, font, New Size(textWidth, Integer.MaxValue), TextFormatFlags.WordBreak Or TextFormatFlags.TextBoxControl)
        
        Dim labelWidth As Integer = Math.Min(textSize.Width + 5, textWidth)
        Dim labelHeight As Integer = Math.Max(textSize.Height + 5, 20)
        
        lblText.Size = New Size(labelWidth, labelHeight)
        
        ' Hitung tinggi bubble
        Dim bubbleHeight As Integer = labelHeight + 10
        bubble.Size = New Size(labelWidth + 24, bubbleHeight)

        Return bubble
    End Function

    ' Method untuk menampilkan tombol Ya/Tidak
    Private Sub ShowYesNoButtons()
        btnYes.Visible = True
        btnNo.Visible = True
    End Sub

    ' Method untuk menyembunyikan tombol Ya/Tidak
    Private Sub HideYesNoButtons()
        btnYes.Visible = False
        btnNo.Visible = False
    End Sub

    ' Handler untuk tombol Ya
    Private Sub btnYes_Click(sender As Object, e As EventArgs) Handles btnYes.Click
        AddUserMessage("Ya")
        
        ' Jika diagnosis aktif, submit ke engine
        If ModuleChatbot.IsDiagnosisActive AndAlso ModuleChatbot.DiagnosisEngine IsNot Nothing AndAlso ModuleChatbot.CurrentQuestionObj IsNot Nothing Then
            ModuleChatbot.DiagnosisEngine.SubmitAnswer(ModuleChatbot.CurrentQuestionObj.Id, "ya")
        Else
            RaiseEvent AnswerSelected(Me, "Ya")
        End If
    End Sub

    ' Handler untuk tombol Tidak
    Private Sub btnNo_Click(sender As Object, e As EventArgs) Handles btnNo.Click
        AddUserMessage("Tidak")
        
        ' Jika diagnosis aktif, submit ke engine
        If ModuleChatbot.IsDiagnosisActive AndAlso ModuleChatbot.DiagnosisEngine IsNot Nothing AndAlso ModuleChatbot.CurrentQuestionObj IsNot Nothing Then
            ModuleChatbot.DiagnosisEngine.SubmitAnswer(ModuleChatbot.CurrentQuestionObj.Id, "tidak")
        Else
            RaiseEvent AnswerSelected(Me, "Tidak")
        End If
    End Sub

    ' Handler untuk tombol Close
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        ' Event untuk close bisa di-handle di form parent
        Me.Visible = False
    End Sub

    ' Method untuk clear chat (untuk keperluan eksternal / restart)
    Public Sub ClearChat()
        ResetChat()
    End Sub

    ' Handler untuk event QuestionChanged dari engine
    Private Sub OnEngineQuestionChanged(q As Question)
        If Me.InvokeRequired Then
            Me.Invoke(New Action(Sub() HandleQuestionChanged(q)))
        Else
            HandleQuestionChanged(q)
        End If
    End Sub

    ' Method untuk menangani perubahan pertanyaan
    Private Sub HandleQuestionChanged(q As Question)
        ModuleChatbot.CurrentQuestionObj = q
        AddBotMessage(q.Text)
    End Sub

    ' Handler untuk event Completed dari engine
    Private Sub OnEngineCompleted(guidance As List(Of String))
        If Me.InvokeRequired Then
            Me.Invoke(New Action(Of List(Of String))(AddressOf HandleDiagnosisCompleted), guidance)
        Else
            HandleDiagnosisCompleted(guidance)
        End If
    End Sub

    ' Method untuk menangani selesainya diagnosis
    Private Sub HandleDiagnosisCompleted(guidance As List(Of String))
        ModuleChatbot.IsDiagnosisActive = False
        HideYesNoButtons()
        ModuleChatbot.CurrentQuestionObj = Nothing

        Dim sb As New StringBuilder()
        sb.AppendLine("Diagnosis selesai! Berikut hasil evaluasi:")
        sb.AppendLine()
        
        For Each line In guidance
            sb.AppendLine("• " & line)
        Next
        
        sb.AppendLine()
        sb.Append("Terima kasih telah menggunakan layanan diagnosis pajak kami!")
        
        ' Tampilkan sebagai satu output (consolidated bubble)
        AddBotMessage(sb.ToString(), False)
    End Sub

    ' Method untuk restart diagnosis
    Public Sub RestartDiagnosis()
        ResetChat()
    End Sub

    ' Property untuk mendapatkan tinggi control
    Public ReadOnly Property ChatHeight As Integer
        Get
            Return Me.Height
        End Get
    End Property

End Class