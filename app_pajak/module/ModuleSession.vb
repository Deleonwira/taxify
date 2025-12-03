Module ModuleSession
    ' Menyimpan data user yang sedang login
    Public CurrentUserId As Integer = 0
    Public CurrentUserName As String = ""
    Public CurrentUserNPWP As String = ""
    Public CurrentUserRole As String = "" ' wajib_pajak, pemberi_kerja, admin

    ' Menyimpan data perusahaan (khusus pemberi_kerja)
    Public CurrentPerusahaanId As Integer = 0
    Public CurrentPerusahaanName As String = ""

    Public Sub ClearSession()
        CurrentUserId = 0
        CurrentUserName = ""
        CurrentUserNPWP = ""
        CurrentUserRole = ""
        CurrentPerusahaanId = 0
        CurrentPerusahaanName = ""
    End Sub

    Public Function IsLoggedIn() As Boolean
        Return CurrentUserId > 0
    End Function
End Module
