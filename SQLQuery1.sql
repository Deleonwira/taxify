CREATE TABLE users (
    npwp NVARCHAR(24) NOT NULL PRIMARY KEY,
    password_hash NVARCHAR(255) NOT NULL,
    nama NVARCHAR(100) NOT NULL,
    email NVARCHAR(100) NOT NULL,
    tipe_user NVARCHAR(20) NOT NULL CHECK (tipe_user IN ('admin', 'pemberi_kerja', 'wajib_pajak')),
    no_telepon NVARCHAR(20) NULL,
    alamat NVARCHAR(MAX) NULL,
    nik NVARCHAR(20) NULL,
    status_aktif NVARCHAR(20) DEFAULT 'active',
    created_at DATETIME DEFAULT GETDATE()
);

CREATE TABLE perusahaan (
    id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    owner_npwp NVARCHAR(24) NOT NULL,
    nama_perusahaan NVARCHAR(150) NOT NULL,
    npwp_perusahaan NVARCHAR(30) NULL UNIQUE,
    alamat NVARCHAR(MAX) NULL,
    kota NVARCHAR(100) NULL,
    no_telepon NVARCHAR(20) NULL,
    email_perusahaan NVARCHAR(100) NULL,
    CONSTRAINT FK_Perusahaan_Owner FOREIGN KEY (owner_npwp) REFERENCES users(npwp) ON DELETE CASCADE
);

CREATE TABLE pekerjaan (
    id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    wp_npwp NVARCHAR(24) NOT NULL,
    perusahaan_id INT NOT NULL,
    jabatan NVARCHAR(100) NULL,
    status_kepegawaian NVARCHAR(20) DEFAULT 'Tetap' CHECK (status_kepegawaian IN ('Tetap', 'Kontrak', 'Freelance', 'Resign')),
    gaji_pokok DECIMAL(15,2) DEFAULT 0.00,
    status_ptkp NVARCHAR(10) NULL,
    CONSTRAINT FK_Pekerjaan_User FOREIGN KEY (wp_npwp) REFERENCES users(npwp) ON DELETE CASCADE,
    CONSTRAINT FK_Pekerjaan_Perusahaan FOREIGN KEY (perusahaan_id) REFERENCES perusahaan(id)
);


CREATE TABLE bukti_potong (
    id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    nomor_bukti NVARCHAR(50) NULL UNIQUE,
    perusahaan_id INT NOT NULL,
    wp_npwp NVARCHAR(24) NOT NULL,
    masa_bulan INT NOT NULL,
    masa_tahun INT NOT NULL,
    gaji_pokok DECIMAL(15,2) DEFAULT 0.00,
    tunjangan DECIMAL(15,2) DEFAULT 0.00,
    bonus_thr DECIMAL(15,2) DEFAULT 0.00,
    bruto_total DECIMAL(15,2) DEFAULT 0.00,
    biaya_jabatan DECIMAL(15,2) DEFAULT 0.00,
    iuran_pensiun DECIMAL(15,2) DEFAULT 0.00,
    netto_total DECIMAL(15,2) DEFAULT 0.00,
    ptkp DECIMAL(15,2) DEFAULT 0.00,
    pkp DECIMAL(15,2) DEFAULT 0.00,
    pph21_terutang DECIMAL(15,2) DEFAULT 0.00,
    created_at DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_BuktiPotong_Perusahaan FOREIGN KEY (perusahaan_id) REFERENCES perusahaan(id) ON DELETE CASCADE,
    CONSTRAINT FK_BuktiPotong_User FOREIGN KEY (wp_npwp) REFERENCES users(npwp) ON DELETE CASCADE
);

-- Seed Data
INSERT INTO users (npwp, password_hash, nama, email, tipe_user, no_telepon, alamat, nik, created_at) VALUES
('111111111111111', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Super Admin', 'admin@taxify.com', 'admin', '08110000001', 'Jakarta Pusat', NULL, GETDATE()),
('222222222222222', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'PT Maju Mundur (HRD)', 'hrd@majumundur.com', 'pemberi_kerja', '08120000002', 'Jakarta Selatan', NULL, GETDATE()),
('444444444444444', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Budi Pratama', 'budi@gmail.com', 'wajib_pajak', '08140000004', 'Bogor', '3202020202020002', GETDATE()),
('789', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Andi Pegawai', 'andi@gmail.com', 'wajib_pajak', '08130000003', 'Depok', '3201010101010001', GETDATE());

INSERT INTO perusahaan (owner_npwp, nama_perusahaan, npwp_perusahaan, alamat, kota, no_telepon, email_perusahaan) VALUES
('222222222222222', 'PT Maju Mundur', '012345678901234', 'Jl. Sudirman No 12', 'Jakarta Selatan', '0219876543', 'office@majumundur.com');

INSERT INTO pekerjaan (wp_npwp, perusahaan_id, jabatan, status_kepegawaian, gaji_pokok, status_ptkp) VALUES
('789', 1, 'Staff IT', 'Tetap', 6000000.00, 'TK0'),
('444444444444444', 1, 'Marketing', 'Kontrak', 5500000.00, 'K0');

INSERT INTO bukti_potong (nomor_bukti, perusahaan_id, wp_npwp, masa_bulan, masa_tahun, gaji_pokok, tunjangan, bonus_thr, bruto_total, biaya_jabatan, iuran_pensiun, netto_total, ptkp, pkp, pph21_terutang, created_at) VALUES
('BP-2025-0001', 1, '789', 1, 2025, 6000000.00, 500000.00, 0.00, 6500000.00, 300000.00, 50000.00, 6150000.00, 5400000.00, 750000.00, 75000.00, GETDATE()),
('BP-2025-0002', 1, '444444444444444', 1, 2025, 5500000.00, 300000.00, 0.00, 5800000.00, 290000.00, 50000.00, 5460000.00, 5400000.00, 60000.00, 6000.00, GETDATE());
