-- ========================================
-- DATABASE APP_PAJAK - SIMPLIFIED SCHEMA
-- ========================================
-- Created: 2025-12-03
-- Purpose: Minimal database for existing app features
-- ========================================

CREATE DATABASE IF NOT EXISTS app_pajak;
USE app_pajak;

-- 1. TABEL USERS (Login & Register)
CREATE TABLE IF NOT EXISTS users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    npwp VARCHAR(20) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    nama VARCHAR(100) NOT NULL,
    email VARCHAR(100),
    tipe_user ENUM('wajib_pajak', 'pemberi_kerja', 'admin') NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 2. TABEL PERUSAHAAN (Untuk Pemberi Kerja)
CREATE TABLE IF NOT EXISTS perusahaan (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL, -- Link ke akun pemberi_kerja
    nama_perusahaan VARCHAR(150) NOT NULL,
    npwp_perusahaan VARCHAR(20) UNIQUE NOT NULL,
    alamat TEXT,
    kota VARCHAR(100),
    no_telepon VARCHAR(20),
    email_perusahaan VARCHAR(100),
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 3. TABEL PEGAWAI (Data Pegawai di Pemberi Kerja)
CREATE TABLE IF NOT EXISTS pegawai (
    id INT AUTO_INCREMENT PRIMARY KEY,
    perusahaan_id INT NOT NULL,
    nama VARCHAR(100) NOT NULL,
    npwp VARCHAR(20),
    nik VARCHAR(20),
    jabatan VARCHAR(100),
    gaji_pokok DECIMAL(15,2) DEFAULT 0,
    status_ptkp VARCHAR(10), -- TK/0, K/1, dll
    status_kepegawaian VARCHAR(50), -- Tetap/Tidak Tetap
    FOREIGN KEY (perusahaan_id) REFERENCES perusahaan(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 4. TABEL BUKTI POTONG (Transaksi Utama)
CREATE TABLE IF NOT EXISTS bukti_potong (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nomor_bukti VARCHAR(50) UNIQUE NOT NULL,
    perusahaan_id INT NOT NULL,
    pegawai_id INT NOT NULL,
    tanggal_bukti DATE,
    masa_pajak_bulan INT,
    masa_pajak_tahun INT,
    
    -- Rincian Nilai
    gaji_pokok DECIMAL(15,2) DEFAULT 0,
    tunjangan_lainnya DECIMAL(15,2) DEFAULT 0,
    bonus_thr DECIMAL(15,2) DEFAULT 0,
    bruto_total DECIMAL(15,2) DEFAULT 0,
    
    biaya_jabatan DECIMAL(15,2) DEFAULT 0,
    iuran_pensiun DECIMAL(15,2) DEFAULT 0,
    netto_total DECIMAL(15,2) DEFAULT 0,
    
    ptkp_nilai DECIMAL(15,2) DEFAULT 0,
    pkp_nilai DECIMAL(15,2) DEFAULT 0,
    pph_terutang DECIMAL(15,2) DEFAULT 0,
    
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (perusahaan_id) REFERENCES perusahaan(id) ON DELETE CASCADE,
    FOREIGN KEY (pegawai_id) REFERENCES pegawai(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- 5. TABEL PROFIL WAJIB PAJAK (Detail Data Diri WP)
CREATE TABLE IF NOT EXISTS profil_wp (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    nik VARCHAR(20),
    alamat TEXT,
    no_telepon VARCHAR(20),
    pekerjaan VARCHAR(100),
    status_pernikahan VARCHAR(20),
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
