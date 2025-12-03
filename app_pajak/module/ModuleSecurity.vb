Imports System.Security.Cryptography
Imports System.Text

Module ModuleSecurity
    ''' &lt;summary&gt;
    ''' Hash password menggunakan SHA-256
    ''' &lt;/summary&gt;
    Public Function HashPassword(password As String) As String
        Using sha256 As SHA256 = SHA256.Create()
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(password)
            Dim hash As Byte() = sha256.ComputeHash(bytes)
            
            Dim builder As New StringBuilder()
            For Each b As Byte In hash
                builder.Append(b.ToString("x2"))
            Next
            
            Return builder.ToString()
        End Using
    End Function

    ''' &lt;summary&gt;
    ''' Verifikasi password dengan hash
    ''' &lt;/summary&gt;
    Public Function VerifyPassword(password As String, hashedPassword As String) As Boolean
        Dim hashOfInput As String = HashPassword(password)
        Return String.Equals(hashOfInput, hashedPassword, StringComparison.OrdinalIgnoreCase)
    End Function

    ''' &lt;summary&gt;
    ''' Membersihkan format NPWP menjadi angka saja
    ''' &lt;/summary&gt;
    Public Function CleanNPWP(npwp As String) As String
        If String.IsNullOrEmpty(npwp) Then Return ""
        Return npwp.Replace(".", "").Replace("-", "").Trim()
    End Function
End Module
