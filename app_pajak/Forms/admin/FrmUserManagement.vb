Public Class FrmUserManagement

    Private Sub FrmUserManagement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        IsiDataDummyUsers()
    End Sub

    Private Sub IsiDataDummyUsers()

        GridUsers.Rows.Clear()

        Dim dummy As New List(Of Object()) From {
            New Object() {1, "Ahmad Pratama", "ahmad@example.com", "Admin", "Active", "2023-11-01"},
            New Object() {2, "Budi Santoso", "budi.s@example.com", "Wajib Pajak", "Pending", "2023-11-05"},
            New Object() {3, "Citra Melati", "citra.m@example.com", "Pemberi Kerja", "Inactive", "2023-10-22"},
            New Object() {4, "Dewi Lestari", "dewi.l@example.com", "Wajib Pajak", "Active", "2023-11-10"},
            New Object() {5, "Eko Saputra", "eko.saputra@example.com", "Admin", "Active", "2023-09-14"},
            New Object() {6, "Farhan Yusuf", "farhan.y@example.com", "Wajib Pajak", "Inactive", "2023-08-30"},
            New Object() {7, "Gita Arum", "gita.arum@example.com", "Pemberi Kerja", "Pending", "2023-11-12"},
            New Object() {8, "Hendra Putra", "hendra.p@example.com", "Wajib Pajak", "Active", "2023-11-13"},
            New Object() {9, "Intan Pratiwi", "intan.p@example.com", "Admin", "Inactive", "2023-09-22"},
            New Object() {10, "Joko Widodo", "joko.w@example.com", "Wajib Pajak", "Active", "2023-11-14"}
        }

        For Each row In dummy
            GridUsers.Rows.Add(row)
        Next

    End Sub

End Class
