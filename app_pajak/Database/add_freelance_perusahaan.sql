-- ============================================
-- Script: Tambah perusahaan "Freelance / Tidak Terdaftar"
-- Untuk mendukung wajib pajak yang tidak bekerja
-- di perusahaan terdaftar dalam sistem
-- ============================================

-- Pertama, buat user sistem untuk owner perusahaan freelance
-- (Hanya jalankan jika belum ada)
INSERT IGNORE INTO users (npwp, password_hash, nama, email, tipe_user) 
VALUES ('000000000000000', 'system', 'System', 'system@taxify.com', 'admin');

-- Tambah perusahaan "Freelance / Tidak Terdaftar"
-- ID = 0 atau ID otomatis (tergantung kebutuhan)
INSERT INTO perusahaan (owner_npwp, nama_perusahaan, npwp_perusahaan, alamat, kota) 
VALUES ('000000000000000', 'Freelance / Tidak Terdaftar', NULL, NULL, NULL);

-- Untuk mendapatkan ID perusahaan yang baru dibuat:
-- SELECT LAST_INSERT_ID();
