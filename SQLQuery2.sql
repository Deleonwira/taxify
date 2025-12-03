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
