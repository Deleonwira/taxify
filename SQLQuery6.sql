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
    CONSTRAINT FK_BuktiPotong_Perusahaan FOREIGN KEY (perusahaan_id)
        REFERENCES perusahaan(id) ON DELETE NO ACTION,
    CONSTRAINT FK_BuktiPotong_User FOREIGN KEY (wp_npwp)
        REFERENCES users(npwp) ON DELETE NO ACTION
);
