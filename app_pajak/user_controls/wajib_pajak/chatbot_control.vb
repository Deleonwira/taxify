Public Class chatbot_control

    ' Event untuk menangani jawaban user (Ya/Tidak)
    Public Event AnswerSelected(sender As Object, answer As String)

    Private currentQuestion As String = ""
    Private currentQuestionObj As Question = Nothing
    Private messageHistory As New List(Of ChatMessage)
    Private diagnosisEngine As PajakDiagnosisEngine = Nothing
    Private isDiagnosisActive As Boolean = False

    ' Class untuk menyimpan history chat
    Private Class ChatMessage
        Public Property Text As String
        Public Property IsBot As Boolean
        Public Property Timestamp As DateTime

        Public Sub New(text As String, isBot As Boolean)
            Me.Text = text
            Me.IsBot = isBot
            Me.Timestamp = DateTime.Now
        End Sub
    End Class

    Private Sub chatbot_control_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Inisialisasi control
        pnlChatArea.AutoScroll = True
        btnYes.Visible = False
        btnNo.Visible = False
        ClearChat()
        
        ' Pesan selamat datang (tanpa tombol)
        AddBotMessage("Halo! Saya adalah asisten chatbot untuk membantu Anda dengan diagnosis pajak. Saya akan mengajukan beberapa pertanyaan yang hanya dapat dijawab dengan Ya atau Tidak.", False)
        
        ' Mulai diagnosis otomatis
        StartDiagnosis()
    End Sub

    ' Method untuk menambahkan pesan bot
    Public Sub AddBotMessage(message As String, Optional showButtons As Boolean = True)
        If showButtons Then
            currentQuestion = message
        End If
        messageHistory.Add(New ChatMessage(message, True))
        RefreshChatDisplay()
        
        If showButtons Then
            ShowYesNoButtons()
        Else
            HideYesNoButtons()
        End If
    End Sub

    ' Method untuk menambahkan pesan user (Ya/Tidak)
    Private Sub AddUserMessage(message As String)
        messageHistory.Add(New ChatMessage(message, False))
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

        For Each msg As ChatMessage In messageHistory
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
        If isDiagnosisActive AndAlso currentQuestionObj IsNot Nothing Then
            diagnosisEngine.SubmitAnswer(currentQuestionObj.Id, "ya")
        Else
            RaiseEvent AnswerSelected(Me, "Ya")
        End If
    End Sub

    ' Handler untuk tombol Tidak
    Private Sub btnNo_Click(sender As Object, e As EventArgs) Handles btnNo.Click
        AddUserMessage("Tidak")
        
        ' Jika diagnosis aktif, submit ke engine
        If isDiagnosisActive AndAlso currentQuestionObj IsNot Nothing Then
            diagnosisEngine.SubmitAnswer(currentQuestionObj.Id, "tidak")
        Else
            RaiseEvent AnswerSelected(Me, "Tidak")
        End If
    End Sub

    ' Handler untuk tombol Close
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        ' Event untuk close bisa di-handle di form parent
        Me.Visible = False
    End Sub

    ' Method untuk clear chat
    Public Sub ClearChat()
        messageHistory.Clear()
        pnlChatArea.Controls.Clear()
        HideYesNoButtons()
    End Sub

    ' Method untuk menambahkan typing indicator (optional)
    Public Sub ShowTypingIndicator()
        Dim typingPanel As New Guna.UI2.WinForms.Guna2Panel()
        typingPanel.BackColor = Color.Transparent
        typingPanel.FillColor = Color.FromArgb(245, 245, 250)
        typingPanel.BorderRadius = 10
        typingPanel.Size = New Size(60, 42)
        typingPanel.Location = New Point(10, pnlChatArea.Controls.Count * 60 + 10)

        Dim dots As New Guna.UI2.WinForms.Guna2HtmlLabel()
        dots.Text = "..."
        dots.Location = New Point(20, 10)
        dots.ForeColor = Color.FromArgb(128, 128, 128)
        dots.Font = New Font("Segoe UI", 12.0F)
        typingPanel.Controls.Add(dots)

        pnlChatArea.Controls.Add(typingPanel)
    End Sub

    ' Property untuk mendapatkan tinggi control
    Public ReadOnly Property ChatHeight As Integer
        Get
            Return Me.Height
        End Get
    End Property

    ' ========== PAJAK DIAGNOSIS ENGINE INTEGRATION ==========

    ' Method untuk memulai diagnosis
    Public Sub StartDiagnosis()
        ' Reset jika ada diagnosis sebelumnya
        If diagnosisEngine IsNot Nothing Then
            diagnosisEngine.Reset()
            RemoveHandler diagnosisEngine.QuestionChanged, AddressOf OnEngineQuestionChanged
            RemoveHandler diagnosisEngine.Completed, AddressOf OnEngineCompleted
        End If

        ' Buat engine baru
        diagnosisEngine = New PajakDiagnosisEngine()
        
        ' Subscribe ke events
        AddHandler diagnosisEngine.QuestionChanged, AddressOf OnEngineQuestionChanged
        AddHandler diagnosisEngine.Completed, AddressOf OnEngineCompleted

        isDiagnosisActive = True

        ' Ambil pertanyaan pertama
        currentQuestionObj = diagnosisEngine.GetNextQuestion()
        If currentQuestionObj IsNot Nothing Then
            currentQuestion = currentQuestionObj.Text
            AddBotMessage(currentQuestionObj.Text)
        End If
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
        currentQuestionObj = q
        currentQuestion = q.Text
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
        isDiagnosisActive = False
        HideYesNoButtons()

        ' Tambahkan pesan bahwa diagnosis selesai (tanpa tombol)
        AddBotMessage("Diagnosis selesai! Berikut hasil evaluasi:", False)
        
        ' Tambahkan setiap guidance sebagai pesan terpisah (tanpa tombol)
        For Each line In guidance
            AddBotMessage("• " & line, False)
        Next
        
        ' Pesan akhir (tanpa tombol)
        AddBotMessage("Terima kasih telah menggunakan layanan diagnosis pajak kami!", False)
    End Sub

    ' Method untuk restart diagnosis
    Public Sub RestartDiagnosis()
        ClearChat()
        AddBotMessage("Mari kita mulai diagnosis pajak Anda dari awal.", False)
        StartDiagnosis()
    End Sub

End Class