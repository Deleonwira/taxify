# Database Schema - App Pajak

## Overview
Database lengkap untuk sistem App Pajak yang mencakup semua kebutuhan form yang ada di aplikasi.

## Tabel yang Dibuat

### 1. **users** - Login & User Management
**Digunakan di:**
- `FrmLogin.vb` - Login
- `FrmRegister.vb` - Registrasi
- `FrmUserManagement.vb` - Admin user management
- `wp_data_diri.vb` - Data diri WP

**Kolom:**
- `id` - Primary key
- `npwp` - Username (unique)
- `password_hash` - SHA-256 hashed password
- `nama` - Nama lengkap
- `email` - Email
- `tipe_user` - Enum: 'wajib_pajak', 'pemberi_kerja', 'admin'
- `status_aktif` - Enum: 'active', 'pending', 'inactive'
- `created_at`, `updated_at`, `last_login`

### 2. **profil_wp** - Data Diri Wajib Pajak
**Digunakan di:**
- `wp_data_diri.vb` - Form data diri wajib pajak

**Kolom:**
- `id` - Primary key
- `user_id` - Foreign key ke users
- `nik` - NIK
- `alamat` - Alamat lengkap
- `no_telepon` - Nomor telepon
- `pekerjaan` - Pekerjaan
- `status_pernikahan` - Status pernikahan
- `jumlah_tanggungan` - Jumlah tanggungan

### 3. **perusahaan** - Data Perusahaan Pemberi Kerja
**Digunakan di:**
- `pk_dashboard.vb` - Dashboard pemberi kerja
- `pk_form_bukti_potong.vb` - Data pemberi kerja di bukti potong

**Kolom:**
- `id` - Primary key
- `user_id` - Foreign key ke users
- `nama_perusahaan` - Nama perusahaan
- `npwp_perusahaan` - NPWP perusahaan
- `alamat`, `kota`, `kode_pos` - Alamat lengkap
- `no_telepon`, `email_perusahaan` - Kontak

### 4. **pegawai** - Daftar Pegawai
**Digunakan di:**
- `pk_daftar_pegawai.vb` - Daftar pegawai
- `pk_form_bukti_potong.vb` - Data pegawai di bukti potong

**Kolom:**
- `id` - Primary key
- `perusahaan_id` - Foreign key ke perusahaan
- `nama`, `npwp`, `nik`, `nomor_karyawan`
- `jabatan`, `gaji_pokok`
- `status_ptkp` - TK/0, K/1, dll
- `status_kepegawaian` - tetap/tidak_tetap/kontrak
- `status_aktif` - Boolean

### 5. **bukti_potong** - Bukti Potong Pajak
**Digunakan di:**
- `pk_form_bukti_potong.vb` - Form buat bukti potong (PK)
- `pk_riwayat_bukti_potong.vb` - Riwayat bukti potong (PK)
- `wp_riwayat_bukti_potong.vb` - Riwayat bukti potong (WP)
- `wp_detail_bukti_potong.vb` - Detail bukti potong (WP)

**Kolom:**
- Nomor & tanggal bukti
- Penghasilan bruto (gaji, tunjangan, bonus, THR)
- Pengurangan (biaya jabatan, iuran pensiun, zakat)
- Pajak (PTKP, PKP, PPh terutang)
- Status: draft/terkirim/disetujui

### 6. **spt_tahunan** - SPT Tahunan Wajib Pajak
**Digunakan di:**
- `wp_lapor_pajak.vb` - Form lapor SPT tahunan

**Kolom:**
- Tahun pajak, jenis SPT (1770/1770S/1770SS)
- Data WP (NPWP, nama, alamat)
- Penghasilan bruto & pengurangan
- Hasil perhitungan (netto, PPh)
- Status: draft/terkirim/diproses/selesai

### 7. **tanggungan_keluarga** - Data Tanggungan untuk PTKP
**Digunakan di:**
- (Optional) Untuk perhitungan PTKP yang lebih akurat

**Kolom:**
- `user_id` - Foreign key ke users
- `nama`, `nik`, `hubungan` (istri/suami/anak)
- `tanggal_lahir`, `status_aktif`

### 8. **pengaturan_ptkp** - Referensi PTKP
**Data Master:** Nilai PTKP per tahun dan kode (TK/0, K/1, dll)

### 9. **tarif_pajak** - Referensi Tarif PPh 21
**Data Master:** Layer tarif pajak progresif per tahun

### 10. **activity_log** - Audit Trail
**Untuk tracking:** Semua aktivitas user di sistem

## Instalasi

```bash
# 1. Buka MySQL/phpmyadmin
# 2. Import file SQL
mysql -u root -p < database/app_pajak_full.sql
```

## Data Default

### Admin User
- **NPWP:** `000000000000000`
- **Password:** `admin123`
- **Tipe:** Admin

### PTKP 2024
Data PTKP sudah diinput untuk tahun 2024 (TK/0 s/d K/3)

### Tarif Pajak 2024
Layer tarif pajak PPh 21 tahun 2024 sudah diinput

## Relasi Antar Tabel

```
users (1) --- (1) profil_wp
users (1) --- (1) perusahaan
perusahaan (1) --- (N) pegawai
perusahaan (1) --- (N) bukti_potong
pegawai (1) --- (N) bukti_potong
users (1) --- (N) spt_tahunan
users (1) --- (N) tanggungan_keluarga
users (1) --- (N) activity_log
```

## Bug Fixes
- ✅ Fixed `FrmLogin.vb` - Restored `id` field in SELECT query
- ✅ Fixed `ModuleSession` - Restored `CurrentUserId` assignment
