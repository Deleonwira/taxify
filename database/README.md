# Database App Pajak (Versi Sederhana)

Database ini dirancang khusus untuk mendukung fitur-fitur yang sudah ada di aplikasi `app_pajak` Anda.

## Struktur Database
Database: `app_pajak`

Tabel yang dibuat:
1.  **`users`**: Menyimpan data login untuk semua tipe user (Admin, Pemberi Kerja, Wajib Pajak).
2.  **`perusahaan`**: Data perusahaan milik Pemberi Kerja.
3.  **`pegawai`**: Daftar pegawai yang dikelola oleh Pemberi Kerja.
4.  **`bukti_potong`**: Menyimpan data bukti potong pajak.
5.  **`profil_wp`**: Detail data diri untuk Wajib Pajak.

## Cara Install

1.  Buka **XAMPP Control Panel** dan start **Apache** & **MySQL**.
2.  Buka browser dan akses [http://localhost/phpmyadmin](http://localhost/phpmyadmin).
3.  Buat database baru dengan nama `app_pajak` (jika belum ada).
4.  Import file `database/app_pajak_simple.sql`.
5.  (Opsional) Import file `database/sample_data_simple.sql` untuk data contoh.

## Akun Login (Data Contoh)

Password untuk semua akun di bawah ini adalah: **`password123`**

| Tipe User | NPWP (Username) | Nama |
| :--- | :--- | :--- |
| **Admin** | `0000000000000000` | Administrator |
| **Pemberi Kerja** | `1111111111111111` | Budi Santoso (HRD) |
| **Wajib Pajak** | `2222222222222222` | Andi Pegawai |

## Perubahan Kode
Saya telah memperbarui `FrmLogin.vb` dan menambahkan modul berikut agar login berfungsi dengan aman:
-   `app_pajak/module/ModuleSecurity.vb`: Untuk mengamankan password (hashing).
-   `app_pajak/module/ModuleSession.vb`: Untuk menyimpan status login user.
