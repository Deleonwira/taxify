-- ================================================
-- DATABASE LENGKAP UNTUK SISTEM APP_PAJAK
-- ================================================
-- Dibuat berdasarkan analisis semua form yang ada
-- Tanggal: 2025-12-03
-- ================================================

CREATE DATABASE IF NOT EXISTS app_pajak CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE app_pajak;

-- ================================================
-- 1. TABEL USERS (Login, User Management, Data Diri)
-- ================================================
CREATE TABLE IF NOT EXISTS users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    npwp VARCHAR(20) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    nama VARCHAR(100) NOT NULL,
    email VARCHAR(100),
    tipe_user ENUM('wajib_pajak', 'pemberi_kerja', 'admin') NOT NULL,
    status_aktif ENUM('active', 'pending', 'inactive') DEFAULT 'active',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    last_login TIMESTAMP NULL,
    INDEX idx_tipe_user (tipe_user),
    INDEX idx_status (status_aktif)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- 2. TABEL PROFIL_WP (Data Diri Wajib Pajak)
-- ================================================
CREATE TABLE IF NOT EXISTS profil_wp (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL UNIQUE,
    nik VARCHAR(20),
    alamat TEXT,
    no_telepon VARCHAR(20),
    pekerjaan VARCHAR(100),
    status_pernikahan ENUM('belum_kawin', 'kawin', 'cerai_hidup', 'cerai_mati'),
    jumlah_tanggungan INT DEFAULT 0,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- 3. TABEL PERUSAHAAN (Pemberi Kerja)
-- ================================================
CREATE TABLE IF NOT EXISTS perusahaan (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL UNIQUE,
    nama_perusahaan VARCHAR(150) NOT NULL,
    npwp_perusahaan VARCHAR(20) UNIQUE NOT NULL,
    alamat TEXT,
    kota VARCHAR(100),
    kode_pos VARCHAR(10),
    no_telepon VARCHAR(20),
    email_perusahaan VARCHAR(100),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- 4. TABEL PEGAWAI (Daftar Pegawai PK)
-- ================================================
CREATE TABLE IF NOT EXISTS pegawai (
    id INT AUTO_INCREMENT PRIMARY KEY,
    perusahaan_id INT NOT NULL,
    nama VARCHAR(100) NOT NULL,
    npwp VARCHAR(20),
    nik VARCHAR(20),
    nomor_karyawan VARCHAR(50),
    jabatan VARCHAR(100),
    gaji_pokok DECIMAL(15,2) DEFAULT 0,
    status_ptkp VARCHAR(10), -- TK/0, K/1, K/2, dll
    status_kepegawaian ENUM('tetap', 'tidak_tetap', 'kontrak') DEFAULT 'tetap',
    tanggal_bergabung DATE,
    status_aktif BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (perusahaan_id) REFERENCES perusahaan(id) ON DELETE CASCADE,
    INDEX idx_perusahaan (perusahaan_id),
    INDEX idx_status (status_aktif)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- 5. TABEL BUKTI_POTONG (Form Bukti Potong PK & WP)
-- ================================================
CREATE TABLE IF NOT EXISTS bukti_potong (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nomor_bukti VARCHAR(50) UNIQUE NOT NULL,
    perusahaan_id INT NOT NULL,
    pegawai_id INT NOT NULL,
    tanggal_bukti DATE NOT NULL,
    masa_pajak_bulan INT NOT NULL,
    masa_pajak_tahun INT NOT NULL,
    
    -- Penghasilan Bruto
    gaji_pokok DECIMAL(15,2) DEFAULT 0,
    tunjangan_tetap DECIMAL(15,2) DEFAULT 0,
    tunjangan_lainnya DECIMAL(15,2) DEFAULT 0,
    bonus_thr DECIMAL(15,2) DEFAULT 0,
    bruto_total DECIMAL(15,2) DEFAULT 0,
    
    -- Pengurangan
    biaya_jabatan DECIMAL(15,2) DEFAULT 0,
    iuran_pensiun DECIMAL(15,2) DEFAULT 0,
    zakat_sumbangan DECIMAL(15,2) DEFAULT 0,
    total_pengurangan DECIMAL(15,2) DEFAULT 0,
    netto_total DECIMAL(15,2) DEFAULT 0,
    
    -- Pajak
    ptkp_nilai DECIMAL(15,2) DEFAULT 0,
    pkp_nilai DECIMAL(15,2) DEFAULT 0,
    pph_terutang DECIMAL(15,2) DEFAULT 0,
    
    status ENUM('draft', 'terkirim', 'disetujui') DEFAULT 'draft',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (perusahaan_id) REFERENCES perusahaan(id) ON DELETE CASCADE,
    FOREIGN KEY (pegawai_id) REFERENCES pegawai(id) ON DELETE CASCADE,
    INDEX idx_tahun (masa_pajak_tahun),
    INDEX idx_status (status)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- 6. TABEL SPT_TAHUNAN (Lapor Pajak WP)
-- ================================================
CREATE TABLE IF NOT EXISTS spt_tahunan (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    tahun_pajak INT NOT NULL,
    jenis_spt ENUM('1770', '1770S', '1770SS') NOT NULL,
    
    -- Informasi WP
    npwp VARCHAR(20) NOT NULL,
    nama_wp VARCHAR(100) NOT NULL,
    alamat_wp TEXT,
    
    -- Penghasilan Bruto
    gaji_pokok DECIMAL(15,2) DEFAULT 0,
    tunjangan_tetap DECIMAL(15,2) DEFAULT 0,
    bonus_thr DECIMAL(15,2) DEFAULT 0,
    jumlah_bruto DECIMAL(15,2) DEFAULT 0,
    
    -- Pengurangan
    biaya_jabatan DECIMAL(15,2) DEFAULT 0,
    zakat_sumbangan DECIMAL(15,2) DEFAULT 0,
    total_pengurangan DECIMAL(15,2) DEFAULT 0,
    
    -- Hasil
    penghasilan_netto DECIMAL(15,2) DEFAULT 0,
    pajak_pph DECIMAL(15,2) DEFAULT 0,
    
    status ENUM('draft', 'terkirim', 'diproses', 'selesai') DEFAULT 'draft',
    tanggal_lapor TIMESTAMP NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
    INDEX idx_user_tahun (user_id, tahun_pajak),
    INDEX idx_status (status)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- 7. TABEL TANGGUNGAN_KELUARGA (Optional - untuk PTKP)
-- ================================================
CREATE TABLE IF NOT EXISTS tanggungan_keluarga (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    nama VARCHAR(100) NOT NULL,
    nik VARCHAR(20),
    hubungan ENUM('istri', 'suami', 'anak', 'tanggungan_lain') NOT NULL,
    tanggal_lahir DATE,
    status_aktif BOOLEAN DEFAULT TRUE,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
    INDEX idx_user (user_id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- 8. TABEL PENGATURAN_PTKP (Referensi PTKP)
-- ================================================
CREATE TABLE IF NOT EXISTS pengaturan_ptkp (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tahun INT NOT NULL,
    kode VARCHAR(10) NOT NULL, -- TK/0, K/1, K/2, dll
    nilai DECIMAL(15,2) NOT NULL,
    keterangan VARCHAR(255),
    UNIQUE KEY unique_tahun_kode (tahun, kode)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- 9. TABEL TARIF_PAJAK (Referensi Tarif PPh 21)
-- ================================================
CREATE TABLE IF NOT EXISTS tarif_pajak (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tahun INT NOT NULL,
    batas_bawah DECIMAL(15,2) NOT NULL,
    batas_atas DECIMAL(15,2),
    tarif_persen DECIMAL(5,2) NOT NULL,
    keterangan VARCHAR(255),
    INDEX idx_tahun (tahun)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- 10. TABEL ACTIVITY_LOG (Audit Trail)
-- ================================================
CREATE TABLE IF NOT EXISTS activity_log (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT,
    aktivitas VARCHAR(255) NOT NULL,
    deskripsi TEXT,
    ip_address VARCHAR(45),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE SET NULL,
    INDEX idx_user (user_id),
    INDEX idx_created (created_at)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================
-- DATA DEFAULT
-- ================================================

-- Data PTKP 2024
INSERT INTO pengaturan_ptkp (tahun, kode, nilai, keterangan) VALUES
(2024, 'TK/0', 54000000, 'Tidak Kawin, Tanpa Tanggungan'),
(2024, 'TK/1', 58500000, 'Tidak Kawin, 1 Tanggungan'),
(2024, 'TK/2', 63000000, 'Tidak Kawin, 2 Tanggungan'),
(2024, 'TK/3', 67500000, 'Tidak Kawin, 3 Tanggungan'),
(2024, 'K/0', 58500000, 'Kawin, Tanpa Tanggungan'),
(2024, 'K/1', 63000000, 'Kawin, 1 Tanggungan'),
(2024, 'K/2', 67500000, 'Kawin, 2 Tanggungan'),
(2024, 'K/3', 72000000, 'Kawin, 3 Tanggungan');

-- Data Tarif Pajak 2024 (PPh 21)
INSERT INTO tarif_pajak (tahun, batas_bawah, batas_atas, tarif_persen, keterangan) VALUES
(2024, 0, 60000000, 5.00, 'Layer 1 - 5%'),
(2024, 60000001, 250000000, 15.00, 'Layer 2 - 15%'),
(2024, 250000001, 500000000, 25.00, 'Layer 3 - 25%'),
(2024, 500000001, 5000000000, 30.00, 'Layer 4 - 30%'),
(2024, 5000000001, NULL, 35.00, 'Layer 5 - 35%');

-- User Admin Default (Password: admin123)
-- Hash SHA-256 untuk "admin123": 240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9
INSERT INTO users (npwp, password_hash, nama, email, tipe_user, status_aktif) VALUES
('000000000000000', '240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', 'Administrator', 'admin@app_pajak.local', 'admin', 'active');
