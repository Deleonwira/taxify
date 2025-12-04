-- ============================================
-- TRIGGER UNTUK OTOMATIS UPDATE SPT TAHUNAN
-- ============================================
-- File ini berisi stored procedure dan trigger untuk otomatis menghitung
-- dan memasukkan data ke tabel spt_tahunan setiap kali bukti_potong dibuat/diubah/dihapus

DELIMITER $$

-- ============================================
-- Stored Procedure: Kalkulasi SPT Tahunan
-- ============================================
-- Procedure ini akan menghitung total tahunan dari semua bukti_potong
-- untuk satu wajib pajak pada tahun tertentu

DROP PROCEDURE IF EXISTS sp_kalkulasi_spt_tahunan$$

CREATE PROCEDURE sp_kalkulasi_spt_tahunan(
    IN p_wp_npwp VARCHAR(24),
    IN p_tahun_pajak INT
)
BEGIN
    DECLARE v_gaji_setahun DECIMAL(15,2);
    DECLARE v_tunjangan_setahun DECIMAL(15,2);
    DECLARE v_bonus_thr_setahun DECIMAL(15,2);
    DECLARE v_bruto_setahun DECIMAL(15,2);
    DECLARE v_biaya_jabatan_setahun DECIMAL(15,2);
    DECLARE v_iuran_pensiun_setahun DECIMAL(15,2);
    DECLARE v_netto_setahun DECIMAL(15,2);
    DECLARE v_pph21_dipotong DECIMAL(15,2);
    DECLARE v_status_ptkp VARCHAR(10);
    DECLARE v_ptkp DECIMAL(15,2);
    DECLARE v_pkp DECIMAL(15,2);
    DECLARE v_pph21_terutang DECIMAL(15,2);
    DECLARE v_pph21_kurang_bayar DECIMAL(15,2);
    DECLARE v_pph21_lebih_bayar DECIMAL(15,2);
    DECLARE v_status_spt VARCHAR(20);
    DECLARE v_count_bukti INT;
    
    -- Hitung total bukti potong untuk wajib pajak di tahun tersebut
    SELECT COUNT(*) INTO v_count_bukti
    FROM bukti_potong
    WHERE wp_npwp = p_wp_npwp 
      AND masa_tahun = p_tahun_pajak;
    
    -- Jika tidak ada bukti potong, hapus data SPT tahunan (jika ada)
    IF v_count_bukti = 0 THEN
        DELETE FROM spt_tahunan
        WHERE wp_npwp = p_wp_npwp 
          AND tahun_pajak = p_tahun_pajak;
    ELSE
        -- Ambil status PTKP dari tabel pekerjaan (asumsi: status PTKP sama untuk semua pekerjaan WP)
        SELECT COALESCE(status_ptkp, 'TK0') INTO v_status_ptkp
        FROM pekerjaan
        WHERE wp_npwp = p_wp_npwp
        LIMIT 1;
        
        -- Hitung total agregasi dari semua bukti potong tahun tersebut
        SELECT 
            COALESCE(SUM(gaji_pokok), 0),
            COALESCE(SUM(tunjangan), 0),
            COALESCE(SUM(bonus_thr), 0),
            COALESCE(SUM(bruto_total), 0),
            COALESCE(SUM(biaya_jabatan), 0),
            COALESCE(SUM(iuran_pensiun), 0),
            COALESCE(SUM(netto_total), 0),
            COALESCE(SUM(pph21_terutang), 0)
        INTO 
            v_gaji_setahun,
            v_tunjangan_setahun,
            v_bonus_thr_setahun,
            v_bruto_setahun,
            v_biaya_jabatan_setahun,
            v_iuran_pensiun_setahun,
            v_netto_setahun,
            v_pph21_dipotong
        FROM bukti_potong
        WHERE wp_npwp = p_wp_npwp 
          AND masa_tahun = p_tahun_pajak;
        
        -- Hitung PTKP berdasarkan status (untuk setahun)
        -- Nilai PTKP 2024 (sesuaikan jika ada perubahan kebijakan)
        SET v_ptkp = CASE v_status_ptkp
            WHEN 'TK0' THEN 54000000  -- Tidak Kawin, tidak ada tanggungan
            WHEN 'TK1' THEN 58500000  -- Tidak Kawin, 1 tanggungan
            WHEN 'TK2' THEN 63000000  -- Tidak Kawin, 2 tanggungan
            WHEN 'TK3' THEN 67500000  -- Tidak Kawin, 3 tanggungan
            WHEN 'K0'  THEN 58500000  -- Kawin, tidak ada tanggungan
            WHEN 'K1'  THEN 63000000  -- Kawin, 1 tanggungan
            WHEN 'K2'  THEN 67500000  -- Kawin, 2 tanggungan
            WHEN 'K3'  THEN 72000000  -- Kawin, 3 tanggungan
            ELSE 54000000
        END;
        
        -- Hitung PKP (Penghasilan Kena Pajak)
        SET v_pkp = GREATEST(v_netto_setahun - v_ptkp, 0);
        
        -- Hitung PPh21 Terutang Tahunan (menggunakan tarif progresif 2024)
        -- Layer 1: 0-60 juta = 5%
        -- Layer 2: 60-250 juta = 15%
        -- Layer 3: 250-500 juta = 25%
        -- Layer 4: 500-5 miliar = 30%
        -- Layer 5: > 5 miliar = 35%
        SET v_pph21_terutang = 0;
        
        IF v_pkp > 0 THEN
            -- Layer 1: 0 - 60 juta (5%)
            IF v_pkp <= 60000000 THEN
                SET v_pph21_terutang = v_pkp * 0.05;
            ELSE
                SET v_pph21_terutang = 60000000 * 0.05;
                
                -- Layer 2: 60 - 250 juta (15%)
                IF v_pkp <= 250000000 THEN
                    SET v_pph21_terutang = v_pph21_terutang + ((v_pkp - 60000000) * 0.15);
                ELSE
                    SET v_pph21_terutang = v_pph21_terutang + (190000000 * 0.15);
                    
                    -- Layer 3: 250 - 500 juta (25%)
                    IF v_pkp <= 500000000 THEN
                        SET v_pph21_terutang = v_pph21_terutang + ((v_pkp - 250000000) * 0.25);
                    ELSE
                        SET v_pph21_terutang = v_pph21_terutang + (250000000 * 0.25);
                        
                        -- Layer 4: 500 juta - 5 miliar (30%)
                        IF v_pkp <= 5000000000 THEN
                            SET v_pph21_terutang = v_pph21_terutang + ((v_pkp - 500000000) * 0.30);
                        ELSE
                            SET v_pph21_terutang = v_pph21_terutang + (4500000000 * 0.30);
                            
                            -- Layer 5: > 5 miliar (35%)
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
            wp_npwp,
            tahun_pajak,
            status_ptkp,
            gaji_setahun,
            tunjangan_setahun,
            bonus_thr_setahun,
            bruto_setahun,
            biaya_jabatan_setahun,
            iuran_pensiun_setahun,
            netto_setahun,
            ptkp,
            pkp,
            pph21_terutang,
            pph21_dipotong,
            pph21_kurang_bayar,
            pph21_lebih_bayar,
            status_spt,
            tanggal_lapor
        ) VALUES (
            p_wp_npwp,
            p_tahun_pajak,
            v_status_ptkp,
            v_gaji_setahun,
            v_tunjangan_setahun,
            v_bonus_thr_setahun,
            v_bruto_setahun,
            v_biaya_jabatan_setahun,
            v_iuran_pensiun_setahun,
            v_netto_setahun,
            v_ptkp,
            v_pkp,
            v_pph21_terutang,
            v_pph21_dipotong,
            v_pph21_kurang_bayar,
            v_pph21_lebih_bayar,
            v_status_spt,
            CURRENT_TIMESTAMP()
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
    END IF;
END$$

-- ============================================
-- Trigger: AFTER INSERT bukti_potong
-- ============================================
DROP TRIGGER IF EXISTS trg_bukti_potong_after_insert$$

CREATE TRIGGER trg_bukti_potong_after_insert
AFTER INSERT ON bukti_potong
FOR EACH ROW
BEGIN
    CALL sp_kalkulasi_spt_tahunan(NEW.wp_npwp, NEW.masa_tahun);
END$$

-- ============================================
-- Trigger: AFTER UPDATE bukti_potong
-- ============================================
DROP TRIGGER IF EXISTS trg_bukti_potong_after_update$$

CREATE TRIGGER trg_bukti_potong_after_update
AFTER UPDATE ON bukti_potong
FOR EACH ROW
BEGIN
    -- Kalkulasi ulang untuk tahun yang baru (jika tahun berubah)
    CALL sp_kalkulasi_spt_tahunan(NEW.wp_npwp, NEW.masa_tahun);
    
    -- Jika tahun atau NPWP berubah, kalkulasi ulang untuk tahun yang lama juga
    IF (NEW.masa_tahun != OLD.masa_tahun) OR (NEW.wp_npwp != OLD.wp_npwp) THEN
        CALL sp_kalkulasi_spt_tahunan(OLD.wp_npwp, OLD.masa_tahun);
    END IF;
END$$

-- ============================================
-- Trigger: AFTER DELETE bukti_potong
-- ============================================
DROP TRIGGER IF EXISTS trg_bukti_potong_after_delete$$

CREATE TRIGGER trg_bukti_potong_after_delete
AFTER DELETE ON bukti_potong
FOR EACH ROW
BEGIN
    CALL sp_kalkulasi_spt_tahunan(OLD.wp_npwp, OLD.masa_tahun);
END$$

DELIMITER ;

-- ============================================
-- Menambahkan UNIQUE KEY pada spt_tahunan
-- ============================================
-- Agar INSERT ON DUPLICATE KEY UPDATE berfungsi

ALTER TABLE spt_tahunan
ADD UNIQUE KEY unique_wp_tahun (wp_npwp, tahun_pajak);
