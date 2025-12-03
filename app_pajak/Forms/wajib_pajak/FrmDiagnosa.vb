Imports System
Imports System.Text

Public Class FormDiagnosaPajak

    Private engine As PajakDiagnosisEngine
    Private currentQuestion As Question

    Private Sub FormDiagnosaPajak_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StartDiagnosis()
    End Sub

    Private Sub StartDiagnosis()
        panelResult.Visible = False
        txtResult.Text = ""
        btnRestart.Visible = False

        engine = New PajakDiagnosisEngine()

        ' Subscribing events
        AddHandler engine.QuestionChanged, AddressOf OnQuestionChanged
        AddHandler engine.Completed, AddressOf OnCompleted

        ' Ambil pertanyaan pertama
        currentQuestion = engine.GetNextQuestion()
        If currentQuestion IsNot Nothing Then
            lblQuestion.Text = currentQuestion.Text
        End If
    End Sub

    Private Sub OnQuestionChanged(q As Question)
        currentQuestion = q
        lblQuestion.Text = q.Text
    End Sub

    Private Sub OnCompleted(guidance As List(Of String))
        lblQuestion.Text = "Diagnosis Selesai!"

        panelResult.Visible = True
        btnRestart.Visible = True

        Dim sb As New StringBuilder()
        For Each line In guidance
            sb.AppendLine("• " & line)
        Next

        txtResult.Text = sb.ToString()

        btnYa.Enabled = False
        btnTidak.Enabled = False
    End Sub

    Private Sub btnYa_Click(sender As Object, e As EventArgs) Handles btnYa.Click
        If currentQuestion IsNot Nothing Then
            engine.SubmitAnswer(currentQuestion.Id, "ya")
        End If
    End Sub

    Private Sub btnTidak_Click(sender As Object, e As EventArgs) Handles btnTidak.Click
        If currentQuestion IsNot Nothing Then
            engine.SubmitAnswer(currentQuestion.Id, "tidak")
        End If
    End Sub

    Private Sub btnRestart_Click(sender As Object, e As EventArgs) Handles btnRestart.Click
        btnYa.Enabled = True
        btnTidak.Enabled = True
        StartDiagnosis()
    End Sub

End Class
