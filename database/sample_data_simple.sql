-- ========================================
-- SAMPLE DATA FOR APP_PAJAK (SIMPLIFIED)
-- ========================================
-- Password default untuk semua user: password123
-- Hash SHA-256: ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f
-- ========================================

USE app_pajak;

-- 1. INSERT USERS
-- Admin
INSERT INTO users (npwp, password_hash, nama, email, tipe_user) VALUES 
('0000000000000000', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Administrator', 'admin@pajak.go.id', 'admin');

-- Pemberi Kerja (PT Maju Mundur)
INSERT INTO users (npwp, password_hash, nama, email, tipe_user) VALUES 
('1111111111111111', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Budi Santoso (HRD)', 'hrd@majumundur.com', 'pemberi_kerja');

-- Wajib Pajak (Pegawai)
INSERT INTO users (npwp, password_hash, nama, email, tipe_user) VALUES 
('2222222222222222', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Andi Pegawai', 'andi@gmail.com', 'wajib_pajak');

-- 2. INSERT PERUSAHAAN
-- Mengambil ID user pemberi_kerja (asumsi ID 2 karena auto_increment)
INSERT INTO perusahaan (user_id, nama_perusahaan, npwp_perusahaan, alamat, kota, no_telepon, email_perusahaan) VALUES 
(2, 'PT Maju Mundur', '99.999.999.9-999.000', 'Jl. Sudirman No. 1', 'Jakarta', '021-5555555', 'info@majumundur.com');

-- 3. INSERT PEGAWAI
-- Pegawai 1: Andi (yang juga punya akun WP)
INSERT INTO pegawai (perusahaan_id, nama, npwp, nik, jabatan, gaji_pokok, status_ptkp, status_kepegawaian) VALUES 
(1, 'Andi Pegawai', '22.222.222.2-222.222', '3171000000000001', 'Staff IT', 10000000, 'TK/0', 'Tetap');

-- Pegawai 2: Siti (belum punya akun WP di sistem)
INSERT INTO pegawai (perusahaan_id, nama, npwp, nik, jabatan, gaji_pokok, status_ptkp, status_kepegawaian) VALUES 
(1, 'Siti Aminah', '33.333.333.3-333.333', '3171000000000002', 'Staff Keuangan', 8000000, 'K/1', 'Tetap');

-- 4. INSERT BUKTI POTONG (Contoh)
INSERT INTO bukti_potong (nomor_bukti, perusahaan_id, pegawai_id, tanggal_bukti, masa_pajak_bulan, masa_pajak_tahun, gaji_pokok, tunjangan_lainnya, bruto_total, pph_terutang) VALUES 
('1.1-12.24-0000001', 1, 1, '2024-12-31', 12, 2024, 120000000, 10000000, 130000000, 5000000);

-- 5. INSERT PROFIL WP
INSERT INTO profil_wp (user_id, nik, alamat, no_telepon, pekerjaan, status_pernikahan) VALUES 
(3, '3171000000000001', 'Jl. Kebon Jeruk No. 10', '081234567890', 'Karyawan Swasta', 'Belum Kawin');
