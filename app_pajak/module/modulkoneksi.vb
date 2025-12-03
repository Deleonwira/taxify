Imports MySql.Data.MySqlClient
Module modulkoneksi
    Public koneksi As MySqlConnection

    Public connectionString As String = "Server=localhost; Database=app_pajak; User Id=root; Password=;"

    Public Sub BukaKoneksi()
        Try
            koneksi = New MySqlConnection(connectionString)
            If koneksi.State = ConnectionState.Closed Then
                koneksi.Open()
            End If
        Catch ex As Exception
            MsgBox("Koneksi ke database GAGAL: " & ex.Message, MsgBoxStyle.Critical, "Error Koneksi")
        End Try
    End Sub

    Public Sub TutupKoneksi()
        If koneksi IsNot Nothing AndAlso koneksi.State = ConnectionState.Open Then
            koneksi.Close()
        End If
    End Sub
End Module
