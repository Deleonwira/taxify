-- =============================================
-- DATA DUMMY UNTUK app_pajak_v2
-- =============================================

-- RESET AUTO_INCREMENT (opsional, jalankan jika ingin mulai dari ID tertentu)
-- ALTER TABLE users AUTO_INCREMENT = 10;
-- ALTER TABLE wajib_pajak AUTO_INCREMENT = 10;
-- ALTER TABLE perusahaan AUTO_INCREMENT = 10;
-- ALTER TABLE pekerjaan AUTO_INCREMENT = 10;

-- =============================================
-- PERUSAHAAN
-- =============================================
INSERT INTO perusahaan (nama_perusahaan, npwp_perusahaan, alamat, kota, no_telepon, email_perusahaan) VALUES
('PT Teknologi Maju', '01.234.567.8-901.000', 'Jl. Gatot Subroto Kav. 12', 'Jakarta Selatan', '0215551234', 'info@teknologimaju.co.id'),
('CV Berkah Abadi', '02.345.678.9-012.000', 'Jl. Ahmad Yani No. 45', 'Surabaya', '0315556789', 'admin@berkah-abadi.com'),
('PT Global Solusi', '03.456.789.0-123.000', 'Jl. Sudirman No. 100', 'Jakarta Pusat', '0215559999', 'contact@globalsolusi.id'),
('CV Kreatif Digital', '04.567.890.1-234.000', 'Jl. Diponegoro No. 23', 'Bandung', '0225553333', 'hello@kreatifdigital.net'),
('PT Nusantara Jaya', '05.678.901.2-345.000', 'Jl. Pemuda No. 78', 'Semarang', '0245557777', 'hrd@nusantarajaya.co.id');

-- =============================================
-- USERS (tipe_user = wajib_pajak)
-- Password untuk semua user: password123
-- Hash SHA-256: ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f
-- =============================================
INSERT INTO users (username, password_hash, tipe_user, is_active) VALUES
('wp_budi_santoso', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'wajib_pajak', 1),
('wp_siti_rahayu', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'wajib_pajak', 1),
('wp_agus_wijaya', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'wajib_pajak', 1),
('wp_dewi_lestari', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'wajib_pajak', 1),
('wp_ahmad_hidayat', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'wajib_pajak', 1),
('wp_rina_permata', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'wajib_pajak', 1),
('wp_joko_prasetyo', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'wajib_pajak', 1),
('wp_maya_sari', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'wajib_pajak', 1);

-- =============================================
-- WAJIB PAJAK (profil untuk setiap user wajib_pajak)
-- Catatan: user_id harus sesuai dengan ID yang dihasilkan dari INSERT users di atas
-- =============================================
-- Jalankan query berikut setelah mengetahui user_id yang dihasilkan:

INSERT INTO wajib_pajak (user_id, npwp, nik, nama, email, no_telepon, alamat, status_ptkp, status_validasi)
SELECT 
    u.id,
    CONCAT('12345678901', LPAD(u.id, 4, '0')),
    CONCAT('320101010101', LPAD(u.id, 4, '0')),
    CASE 
        WHEN u.username = 'wp_budi_santoso' THEN 'Budi Santoso'
        WHEN u.username = 'wp_siti_rahayu' THEN 'Siti Rahayu'
        WHEN u.username = 'wp_agus_wijaya' THEN 'Agus Wijaya'
        WHEN u.username = 'wp_dewi_lestari' THEN 'Dewi Lestari'
        WHEN u.username = 'wp_ahmad_hidayat' THEN 'Ahmad Hidayat'
        WHEN u.username = 'wp_rina_permata' THEN 'Rina Permata'
        WHEN u.username = 'wp_joko_prasetyo' THEN 'Joko Prasetyo'
        WHEN u.username = 'wp_maya_sari' THEN 'Maya Sari'
    END,
    CONCAT(REPLACE(u.username, 'wp_', ''), '@email.com'),
    CONCAT('0812', LPAD(u.id, 8, '0')),
    'Jakarta',
    CASE 
        WHEN u.username IN ('wp_budi_santoso', 'wp_agus_wijaya', 'wp_joko_prasetyo') THEN 'K0'
        WHEN u.username IN ('wp_siti_rahayu', 'wp_dewi_lestari') THEN 'K1'
        ELSE 'TK0'
    END,
    'approved'
FROM users u
WHERE u.tipe_user = 'wajib_pajak' 
  AND u.username LIKE 'wp_%'
  AND u.id NOT IN (SELECT user_id FROM wajib_pajak);

-- =============================================
-- PEKERJAAN (hubungan wajib_pajak dengan perusahaan)
-- Menghubungkan beberapa wajib_pajak ke perusahaan yang baru ditambah
-- =============================================
-- Catatan: Jalankan setelah INSERT di atas berhasil

INSERT INTO pekerjaan (wajib_pajak_id, perusahaan_id, jabatan)
SELECT wp.id, 
       (SELECT id FROM perusahaan WHERE nama_perusahaan = 'PT Teknologi Maju' LIMIT 1),
       CASE 
           WHEN wp.nama = 'Budi Santoso' THEN 'Software Engineer'
           WHEN wp.nama = 'Siti Rahayu' THEN 'Product Manager'
           ELSE 'Staff'
       END
FROM wajib_pajak wp
WHERE wp.nama IN ('Budi Santoso', 'Siti Rahayu')
  AND wp.id NOT IN (SELECT wajib_pajak_id FROM pekerjaan WHERE perusahaan_id = (SELECT id FROM perusahaan WHERE nama_perusahaan = 'PT Teknologi Maju' LIMIT 1));

INSERT INTO pekerjaan (wajib_pajak_id, perusahaan_id, jabatan)
SELECT wp.id, 
       (SELECT id FROM perusahaan WHERE nama_perusahaan = 'CV Berkah Abadi' LIMIT 1),
       CASE 
           WHEN wp.nama = 'Agus Wijaya' THEN 'Marketing Manager'
           WHEN wp.nama = 'Dewi Lestari' THEN 'Finance Staff'
           ELSE 'Staff'
       END
FROM wajib_pajak wp
WHERE wp.nama IN ('Agus Wijaya', 'Dewi Lestari')
  AND wp.id NOT IN (SELECT wajib_pajak_id FROM pekerjaan WHERE perusahaan_id = (SELECT id FROM perusahaan WHERE nama_perusahaan = 'CV Berkah Abadi' LIMIT 1));

INSERT INTO pekerjaan (wajib_pajak_id, perusahaan_id, jabatan)
SELECT wp.id, 
       (SELECT id FROM perusahaan WHERE nama_perusahaan = 'PT Global Solusi' LIMIT 1),
       CASE 
           WHEN wp.nama = 'Ahmad Hidayat' THEN 'IT Consultant'
           WHEN wp.nama = 'Rina Permata' THEN 'HR Manager'
           ELSE 'Staff'
       END
FROM wajib_pajak wp
WHERE wp.nama IN ('Ahmad Hidayat', 'Rina Permata')
  AND wp.id NOT IN (SELECT wajib_pajak_id FROM pekerjaan WHERE perusahaan_id = (SELECT id FROM perusahaan WHERE nama_perusahaan = 'PT Global Solusi' LIMIT 1));

INSERT INTO pekerjaan (wajib_pajak_id, perusahaan_id, jabatan)
SELECT wp.id, 
       (SELECT id FROM perusahaan WHERE nama_perusahaan = 'CV Kreatif Digital' LIMIT 1),
       CASE 
           WHEN wp.nama = 'Joko Prasetyo' THEN 'Creative Director'
           WHEN wp.nama = 'Maya Sari' THEN 'Graphic Designer'
           ELSE 'Staff'
       END
FROM wajib_pajak wp
WHERE wp.nama IN ('Joko Prasetyo', 'Maya Sari')
  AND wp.id NOT IN (SELECT wajib_pajak_id FROM pekerjaan WHERE perusahaan_id = (SELECT id FROM perusahaan WHERE nama_perusahaan = 'CV Kreatif Digital' LIMIT 1));

-- =============================================
-- VERIFIKASI DATA
-- =============================================
-- Jalankan query berikut untuk memverifikasi data berhasil dimasukkan:

-- SELECT * FROM perusahaan ORDER BY id DESC LIMIT 10;
-- SELECT * FROM users WHERE tipe_user = 'wajib_pajak' ORDER BY id DESC LIMIT 10;
-- SELECT * FROM wajib_pajak ORDER BY id DESC LIMIT 10;
-- SELECT p.*, wp.nama, pr.nama_perusahaan FROM pekerjaan p 
--   JOIN wajib_pajak wp ON wp.id = p.wajib_pajak_id 
--   JOIN perusahaan pr ON pr.id = p.perusahaan_id 
--   ORDER BY p.id DESC LIMIT 20;
