INSERT INTO users (npwp, password_hash, nama, email, tipe_user, no_telepon, alamat, nik, created_at) VALUES
('111111111111111', 'ef92b7...', 'Super Admin', 'admin@taxify.com', 'admin', '08110000001', 'Jakarta Pusat', NULL, GETDATE()),
('222222222222222', 'ef92b7...', 'PT Maju Mundur (HRD)', 'hrd@majumundur.com', 'pemberi_kerja', '08120000002', 'Jakarta Selatan', NULL, GETDATE()),
('444444444444444', 'ef92b7...', 'Budi Pratama', 'budi@gmail.com', 'wajib_pajak', '08140000004', 'Bogor', '3202020202020002', GETDATE()),
('789', 'ef92b7...', 'Andi Pegawai', 'andi@gmail.com', 'wajib_pajak', '08130000003', 'Depok', '3201010101010001', GETDATE());
GO
