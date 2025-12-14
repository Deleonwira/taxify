-- Migration Script: Separate Freelance Data
-- 1. Create new table `bukti_potong_freelance`
CREATE TABLE `bukti_potong_freelance` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `wajib_pajak_id` int(11) NOT NULL,
  `nomor_bukti` varchar(50) DEFAULT NULL,
  `jenis_freelance` enum('harian','tenaga_ahli') NOT NULL,
  `is_pph_final` tinyint(1) DEFAULT 0,
  `masa_tahun` int(11) NOT NULL,
  `masa_bulan` int(11) NOT NULL,
  `nama_pemberi_kerja` varchar(150) NOT NULL,
  `npwp_pemberi_kerja` varchar(30) DEFAULT NULL,
  `bruto_per_hari` decimal(15,2) DEFAULT 0.00,
  `jumlah_hari_kerja` int(11) DEFAULT 0,
  `bruto_total` decimal(15,2) DEFAULT 0.00,
  `dpp` decimal(15,2) DEFAULT 0.00,
  `tarif_persen` decimal(5,2) DEFAULT 0.00,
  `pph_dipotong` decimal(15,2) DEFAULT 0.00,
  `created_at` datetime DEFAULT current_timestamp(),
  PRIMARY KEY (`id`),
  KEY `wajib_pajak_id` (`wajib_pajak_id`),
  CONSTRAINT `bukti_potong_freelance_ibfk_1` FOREIGN KEY (`wajib_pajak_id`) REFERENCES `wajib_pajak` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- 2. Clean up `bukti_potong` table (Remove freelance specific columns)
ALTER TABLE `bukti_potong`
  DROP COLUMN `jenis_bukti_potong`,
  DROP COLUMN `is_pph_final`,
  DROP COLUMN `bruto_per_hari`,
  DROP COLUMN `jumlah_hari_kerja`,
  DROP COLUMN `nama_pemberi_kerja`,
  DROP COLUMN `npwp_pemberi_kerja`;

-- 3. Update Stored Procedure `sp_kalkulasi_spt_tahunan`
DROP PROCEDURE IF EXISTS `sp_kalkulasi_spt_tahunan`;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_kalkulasi_spt_tahunan` (IN `p_wajib_pajak_id` INT, IN `p_tahun_pajak` INT)
BEGIN
    DECLARE v_gaji_setahun DECIMAL(15,2) DEFAULT 0;
    DECLARE v_tunjangan_setahun DECIMAL(15,2) DEFAULT 0;
    DECLARE v_bonus_thr_setahun DECIMAL(15,2) DEFAULT 0;
    DECLARE v_bruto_setahun DECIMAL(15,2) DEFAULT 0;
    DECLARE v_biaya_jabatan_setahun DECIMAL(15,2) DEFAULT 0;
    DECLARE v_iuran_pensiun_setahun DECIMAL(15,2) DEFAULT 0;
    DECLARE v_netto_setahun DECIMAL(15,2) DEFAULT 0;
    DECLARE v_pph21_dipotong DECIMAL(15,2) DEFAULT 0;
    
    DECLARE v_netto_freelance DECIMAL(15,2) DEFAULT 0;
    DECLARE v_pph_freelance DECIMAL(15,2) DEFAULT 0;
    
    DECLARE v_status_ptkp VARCHAR(10);
    DECLARE v_ptkp DECIMAL(15,2);
    DECLARE v_pkp DECIMAL(15,2);
    DECLARE v_pph21_terutang DECIMAL(15,2);
    DECLARE v_pph21_kurang_bayar DECIMAL(15,2);
    DECLARE v_pph21_lebih_bayar DECIMAL(15,2);
    DECLARE v_status_spt VARCHAR(20);
    
    -- 1. Hitung total dari Pegawai Tetap (Tabel bukti_potong)
    SELECT 
        COALESCE(SUM(bp.gaji_pokok), 0),
        COALESCE(SUM(bp.tunjangan), 0),
        COALESCE(SUM(bp.bonus_thr), 0),
        COALESCE(SUM(bp.bruto_total), 0),
        COALESCE(SUM(bp.biaya_jabatan), 0),
        COALESCE(SUM(bp.iuran_pensiun), 0),
        COALESCE(SUM(bp.netto_total), 0),
        COALESCE(SUM(bp.pph21_terutang), 0)
    INTO 
        v_gaji_setahun,
        v_tunjangan_setahun,
        v_bonus_thr_setahun,
        v_bruto_setahun,
        v_biaya_jabatan_setahun,
        v_iuran_pensiun_setahun,
        v_netto_setahun,
        v_pph21_dipotong
    FROM bukti_potong bp
    INNER JOIN pekerjaan p ON bp.pekerjaan_id = p.id
    WHERE p.wajib_pajak_id = p_wajib_pajak_id 
      AND bp.masa_tahun = p_tahun_pajak;

    -- 2. Hitung total dari Freelance (Tenaga Ahli - Non Final)
    -- Ambil DPP sebagai netto (karena DPP = 50% Bruto untuk tenaga ahli)
    SELECT 
        COALESCE(SUM(dpp), 0),
        COALESCE(SUM(pph_dipotong), 0)
    INTO 
        v_netto_freelance,
        v_pph_freelance
    FROM bukti_potong_freelance
    WHERE wajib_pajak_id = p_wajib_pajak_id
      AND masa_tahun = p_tahun_pajak
      AND is_pph_final = 0; -- Hanya yang Non-Final yang masuk hitungan SPT Tahunan

    -- 3. Gabungkan
    SET v_netto_setahun = v_netto_setahun + v_netto_freelance;
    SET v_pph21_dipotong = v_pph21_dipotong + v_pph_freelance;
    
    -- Lanjut Hitungan PTKP dll
    
    -- Ambil status PTKP dari tabel wajib_pajak
    SELECT COALESCE(wp.status_ptkp, 'TK0') INTO v_status_ptkp
    FROM wajib_pajak wp
    WHERE wp.id = p_wajib_pajak_id;
    
    -- Hitung PTKP berdasarkan status (untuk setahun)
    SET v_ptkp = CASE v_status_ptkp
        WHEN 'TK0' THEN 54000000
        WHEN 'TK1' THEN 58500000
        WHEN 'TK2' THEN 63000000
        WHEN 'TK3' THEN 67500000
        WHEN 'K0'  THEN 58500000
        WHEN 'K1'  THEN 63000000
        WHEN 'K2'  THEN 67500000
        WHEN 'K3'  THEN 72000000
        ELSE 54000000
    END;
    
    -- Hitung PKP (Penghasilan Kena Pajak)
    SET v_pkp = GREATEST(v_netto_setahun - v_ptkp, 0);
    
    -- Hitung PPh21 Terutang Tahunan (tarif progresif 2024)
    SET v_pph21_terutang = 0;
    
    IF v_pkp > 0 THEN
        IF v_pkp <= 60000000 THEN
            SET v_pph21_terutang = v_pkp * 0.05;
        ELSE
            SET v_pph21_terutang = 60000000 * 0.05;
            IF v_pkp <= 250000000 THEN
                SET v_pph21_terutang = v_pph21_terutang + ((v_pkp - 60000000) * 0.15);
            ELSE
                SET v_pph21_terutang = v_pph21_terutang + (190000000 * 0.15);
                IF v_pkp <= 500000000 THEN
                    SET v_pph21_terutang = v_pph21_terutang + ((v_pkp - 250000000) * 0.25);
                ELSE
                    SET v_pph21_terutang = v_pph21_terutang + (250000000 * 0.25);
                    IF v_pkp <= 5000000000 THEN
                        SET v_pph21_terutang = v_pph21_terutang + ((v_pkp - 500000000) * 0.30);
                    ELSE
                        SET v_pph21_terutang = v_pph21_terutang + (4500000000 * 0.30);
                        SET v_pph21_terutang = v_pph21_terutang + ((v_pkp - 5000000000) * 0.35);
                    END IF;
                END IF;
            END IF;
        END IF;
    END IF;
    
    -- Hitung selisih (kurang bayar / lebih bayar)
    IF v_pph21_terutang > v_pph21_dipotong THEN
        SET v_pph21_kurang_bayar = v_pph21_terutang - v_pph21_dipotong;
        SET v_pph21_lebih_bayar = 0;
        SET v_status_spt = 'Kurang Bayar';
    ELSEIF v_pph21_terutang < v_pph21_dipotong THEN
        SET v_pph21_kurang_bayar = 0;
        SET v_pph21_lebih_bayar = v_pph21_dipotong - v_pph21_terutang;
        SET v_status_spt = 'Lebih Bayar';
    ELSE
        SET v_pph21_kurang_bayar = 0;
        SET v_pph21_lebih_bayar = 0;
        SET v_status_spt = 'Nihil';
    END IF;
    
    -- Insert atau Update data spt_tahunan
    INSERT INTO spt_tahunan (
        wajib_pajak_id, tahun_pajak, status_ptkp,
        gaji_setahun, tunjangan_setahun, bonus_thr_setahun, bruto_setahun,
        biaya_jabatan_setahun, iuran_pensiun_setahun, netto_setahun,
        ptkp, pkp, pph21_terutang, pph21_dipotong,
        pph21_kurang_bayar, pph21_lebih_bayar, status_spt, tanggal_lapor
    ) VALUES (
        p_wajib_pajak_id, p_tahun_pajak, v_status_ptkp,
        v_gaji_setahun, v_tunjangan_setahun, v_bonus_thr_setahun, v_bruto_setahun,
        v_biaya_jabatan_setahun, v_iuran_pensiun_setahun, v_netto_setahun,
        v_ptkp, v_pkp, v_pph21_terutang, v_pph21_dipotong,
        v_pph21_kurang_bayar, v_pph21_lebih_bayar, v_status_spt, CURRENT_TIMESTAMP()
    )
    ON DUPLICATE KEY UPDATE
        status_ptkp = v_status_ptkp,
        gaji_setahun = v_gaji_setahun,
        tunjangan_setahun = v_tunjangan_setahun,
        bonus_thr_setahun = v_bonus_thr_setahun,
        bruto_setahun = v_bruto_setahun,
        biaya_jabatan_setahun = v_biaya_jabatan_setahun,
        iuran_pensiun_setahun = v_iuran_pensiun_setahun,
        netto_setahun = v_netto_setahun,
        ptkp = v_ptkp,
        pkp = v_pkp,
        pph21_terutang = v_pph21_terutang,
        pph21_dipotong = v_pph21_dipotong,
        pph21_kurang_bayar = v_pph21_kurang_bayar,
        pph21_lebih_bayar = v_pph21_lebih_bayar,
        status_spt = v_status_spt,
        tanggal_lapor = CURRENT_TIMESTAMP();
END$$
DELIMITER ;
