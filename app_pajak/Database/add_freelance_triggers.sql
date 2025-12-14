-- ================================================================
-- Triggers untuk tabel bukti_potong_freelance
-- Import file ini ke phpMyAdmin untuk menambahkan triggers
-- yang akan auto-update kalkulasi SPT Tahunan
-- ================================================================

-- Drop triggers jika sudah ada (agar bisa re-import)
DROP TRIGGER IF EXISTS `trg_bukti_potong_freelance_after_insert`;
DROP TRIGGER IF EXISTS `trg_bukti_potong_freelance_after_update`;
DROP TRIGGER IF EXISTS `trg_bukti_potong_freelance_after_delete`;

-- ================================================================
-- Trigger: After INSERT
-- Kalkulasi ulang SPT saat data freelance baru dimasukkan
-- ================================================================
DELIMITER $$
CREATE TRIGGER `trg_bukti_potong_freelance_after_insert` 
AFTER INSERT ON `bukti_potong_freelance` 
FOR EACH ROW 
BEGIN
    CALL sp_kalkulasi_spt_tahunan(NEW.wajib_pajak_id, NEW.masa_tahun);
END$$
DELIMITER ;

-- ================================================================
-- Trigger: After UPDATE
-- Kalkulasi ulang SPT saat data freelance diubah
-- ================================================================
DELIMITER $$
CREATE TRIGGER `trg_bukti_potong_freelance_after_update`
AFTER UPDATE ON `bukti_potong_freelance`
FOR EACH ROW
BEGIN
    CALL sp_kalkulasi_spt_tahunan(NEW.wajib_pajak_id, NEW.masa_tahun);
    
    -- Jika tahun atau wajib_pajak berubah, kalkulasi ulang data lama juga
    IF (NEW.masa_tahun != OLD.masa_tahun) OR (NEW.wajib_pajak_id != OLD.wajib_pajak_id) THEN
        CALL sp_kalkulasi_spt_tahunan(OLD.wajib_pajak_id, OLD.masa_tahun);
    END IF;
END$$
DELIMITER ;

-- ================================================================
-- Trigger: After DELETE
-- Kalkulasi ulang SPT saat data freelance dihapus
-- ================================================================
DELIMITER $$
CREATE TRIGGER `trg_bukti_potong_freelance_after_delete`
AFTER DELETE ON `bukti_potong_freelance`
FOR EACH ROW
BEGIN
    CALL sp_kalkulasi_spt_tahunan(OLD.wajib_pajak_id, OLD.masa_tahun);
END$$
DELIMITER ;

-- ================================================================
-- Selesai! Triggers berhasil dibuat.
-- ================================================================
