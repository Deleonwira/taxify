Module ModuleSession
    ' Menyimpan data user yang sedang login (dari tabel users)
    Public CurrentUserId As Integer = 0
    Public CurrentUserName As String = ""
    Public CurrentUserRole As String = "" ' wajib_pajak, pemberi_kerja, admin

    ' Menyimpan ID profil berdasarkan tipe user
    Public CurrentWajibPajakId As Integer = 0
    Public CurrentWajibPajakNPWP As String = ""
    Public CurrentPemberiKerjaId As Integer = 0
    Public CurrentAdminId As Integer = 0

    ' Menyimpan data perusahaan (khusus pemberi_kerja)
    Public CurrentPerusahaanId As Integer = 0
    Public CurrentPerusahaanName As String = ""

    ' Legacy - untuk kompatibilitas sementara
    Public CurrentUserNPWP As String = ""

    Public Sub ClearSession()
        CurrentUserId = 0
        CurrentUserName = ""
        CurrentUserRole = ""
        CurrentWajibPajakId = 0
        CurrentWajibPajakNPWP = ""
        CurrentPemberiKerjaId = 0
        CurrentAdminId = 0
        CurrentPerusahaanId = 0
        CurrentPerusahaanName = ""
        CurrentUserNPWP = ""
    End Sub

    Public Function IsLoggedIn() As Boolean
        Return CurrentUserId > 0
    End Function
End Module
