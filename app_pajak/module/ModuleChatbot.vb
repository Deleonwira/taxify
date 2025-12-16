Imports System.Collections.Generic

Public Module ModuleChatbot
    ' Class definition for chat messages
    Public Class ChatMessage
        Public Property Text As String
        Public Property IsBot As Boolean
        Public Property Timestamp As DateTime

        Public Sub New(text As String, isBot As Boolean)
            Me.Text = text
            Me.IsBot = isBot
            Me.Timestamp = DateTime.Now
        End Sub
    End Class

    ' Global state for Chatbot
    Public MessageHistory As New List(Of ChatMessage)
    Public DiagnosisEngine As PajakDiagnosisEngine = Nothing
    Public IsDiagnosisActive As Boolean = False
    Public CurrentQuestionObj As Question = Nothing

    ' Method to reset chatbot state
    Public Sub ResetChatbot()
        MessageHistory.Clear()
        IsDiagnosisActive = False
        CurrentQuestionObj = Nothing
        If DiagnosisEngine IsNot Nothing Then
            DiagnosisEngine.Reset()
        End If
        DiagnosisEngine = Nothing
    End Sub
End Module
