Imports System
Imports System.Collections.Generic

Public Class Question
    Public Property Id As String
    Public Property Text As String
    Public Property Condition As Func(Of Dictionary(Of String, String), Boolean)

    Public Sub New(id As String, text As String, Optional condition As Func(Of Dictionary(Of String, String), Boolean) = Nothing)
        Me.Id = id
        Me.Text = text
        Me.Condition = condition
    End Sub
End Class

Public Class PajakDiagnosisEngine
    Private ReadOnly questions As List(Of Question)
    Private ReadOnly _answers As Dictionary(Of String, String)
    Private curIndex As Integer

    Public Event QuestionChanged(ByVal q As Question)
    Public Event Completed(ByVal guidance As List(Of String))

    Public Sub New()
        _answers = New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
        questions = New List(Of Question)

        ' Pertanyaan-pertanyaan
        questions.Add(New Question("q1", "Apakah Anda memiliki NPWP (Nomor Pokok Wajib Pajak) yang masih aktif?"))
        questions.Add(New Question("q2", "Apakah Anda memiliki penghasilan (gaji/honor) pada tahun pajak ini?"))

        questions.Add(New Question("q3",
        "Apakah Anda hanya bekerja pada satu perusahaan tanpa pekerjaan sampingan?",
        Function(a) a.ContainsKey("q2") AndAlso a("q2") = "ya"))

        questions.Add(New Question("q5",
        "Apakah Anda menerima bonus/THR/honorarium dalam jumlah besar?",
        Function(a) a.ContainsKey("q3") AndAlso a("q3") = "ya"))

        questions.Add(New Question("q4",
        "Apakah Anda menerima bukti potong dari pekerjaan sampingan/freelance?",
        Function(a) a.ContainsKey("q2") AndAlso a("q2") = "ya" AndAlso
                   (Not a.ContainsKey("q3") OrElse a("q3") = "tidak")))

        questions.Add(New Question("q6",
        "Apakah Anda membayar zakat/sumbangan wajib (ada buktinya)?",
        Function(a) a.ContainsKey("q2") AndAlso a("q2") = "ya"))

        questions.Add(New Question("q7",
        "Apakah Anda kesulitan menjumlahkan total penghasilan dan total pajak?",
        Function(a) a.ContainsKey("q2") AndAlso a("q2") = "ya"))

        questions.Add(New Question("q8",
        "Apakah Anda akan melapor setelah tanggal 31 Maret?",
        Function(a) a.ContainsKey("q1") AndAlso a("q1") = "ya"))

        curIndex = 0
    End Sub


    Public ReadOnly Property Answers As Dictionary(Of String, String)
        Get
            Return _answers
        End Get
    End Property

    Public Function GetNextQuestion() As Question
        For i As Integer = curIndex To questions.Count - 1
            Dim q = questions(i)
            If q.Condition Is Nothing OrElse q.Condition(_answers) Then
                curIndex = i + 1
                Return q
            End If
        Next
        Return Nothing
    End Function

    Public Sub SubmitAnswer(id As String, answer As String)
        If String.IsNullOrWhiteSpace(id) Then Throw New ArgumentNullException(NameOf(id))
        answer = answer.Trim().ToLower()

        If _answers.ContainsKey(id) Then
            _answers(id) = answer
        Else
            _answers.Add(id, answer)
        End If

        Dim nextQ = GetNextQuestion()
        If nextQ Is Nothing Then
            RaiseEvent Completed(Evaluate())
        Else
            RaiseEvent QuestionChanged(nextQ)
        End If
    End Sub

    Private Function Evaluate() As List(Of String)
        Dim out As New List(Of String)

        If _answers.ContainsKey("q1") AndAlso _answers("q1") = "ya" Then
            out.Add("STATUS: Anda wajib melapor SPT karena memiliki NPWP aktif.")
        Else
            out.Add("STATUS: Anda tidak wajib lapor karena tidak memiliki NPWP.")
        End If

        If _answers.ContainsKey("q2") AndAlso _answers("q2") = "tidak" Then
            out.Add("PREDIKSI: NIHIL. Tidak ada penghasilan selama tahun pajak.")
        Else
            Dim q3 = If(_answers.ContainsKey("q3"), _answers("q3"), Nothing)
            Dim q4 = If(_answers.ContainsKey("q4"), _answers("q4"), Nothing)
            Dim q5 = If(_answers.ContainsKey("q5"), _answers("q5"), Nothing)

            If q3 = "ya" AndAlso q5 = "tidak" Then
                out.Add("PREDIKSI: NIHIL. Karyawan tunggal tanpa bonus besar.")
            ElseIf q3 = "ya" AndAlso q5 = "ya" Then
                out.Add("PREDIKSI: LEBIH BAYAR. Terjadi karena bonus besar.")
            ElseIf q3 = "tidak" Then
                out.Add("PREDIKSI: KURANG BAYAR. Karena multi income atau freelance.")
            End If
        End If

        If _answers.ContainsKey("q6") AndAlso _answers("q6") = "ya" Then out.Add("Catatan: Zakat dapat mengurangi PKP.")
        If _answers.ContainsKey("q7") AndAlso _answers("q7") = "ya" Then out.Add("Catatan: Risiko salah hitung bukti potong.")
        If _answers.ContainsKey("q8") AndAlso _answers("q8") = "ya" Then out.Add("Catatan: Anda akan dikenakan denda keterlambatan.")

        Return out
    End Function

    Public Sub Reset()
        _answers.Clear()
        curIndex = 0
    End Sub
End Class
