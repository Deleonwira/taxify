-- Migration: Create Master Pajak Tables
-- Date: 2025-12-14
-- Description: Tables for storing configurable tax rules (PTKP and progressive tax rates)

-- ========================================
-- Table: master_ptkp
-- Stores PTKP values per status code
-- ========================================
CREATE TABLE IF NOT EXISTS `master_ptkp` (
    `id` INT(11) NOT NULL AUTO_INCREMENT,
    `kode_status` VARCHAR(10) NOT NULL,
    `keterangan` VARCHAR(100) NOT NULL,
    `nilai_tahunan` DECIMAL(15,2) NOT NULL,
    `is_active` TINYINT(1) DEFAULT 1,
    `updated_at` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`id`),
    UNIQUE KEY `kode_status` (`kode_status`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Insert default PTKP values (PMK No. 101/PMK.010/2016)
INSERT INTO `master_ptkp` (`kode_status`, `keterangan`, `nilai_tahunan`) VALUES
('TK0', 'Tidak Kawin, 0 Tanggungan', 54000000.00),
('TK1', 'Tidak Kawin, 1 Tanggungan', 58500000.00),
('TK2', 'Tidak Kawin, 2 Tanggungan', 63000000.00),
('TK3', 'Tidak Kawin, 3 Tanggungan', 67500000.00),
('K0', 'Kawin, 0 Tanggungan', 58500000.00),
('K1', 'Kawin, 1 Tanggungan', 63000000.00),
('K2', 'Kawin, 2 Tanggungan', 67500000.00),
('K3', 'Kawin, 3 Tanggungan', 72000000.00)
ON DUPLICATE KEY UPDATE 
    keterangan = VALUES(keterangan),
    nilai_tahunan = VALUES(nilai_tahunan);

-- ========================================
-- Table: master_tarif_pph
-- Stores progressive tax rate layers (UU HPP 2021)
-- ========================================
CREATE TABLE IF NOT EXISTS `master_tarif_pph` (
    `id` INT(11) NOT NULL AUTO_INCREMENT,
    `lapisan` INT(11) NOT NULL,
    `batas_bawah` DECIMAL(20,2) NOT NULL,
    `batas_atas` DECIMAL(20,2) NOT NULL,
    `tarif_persen` DECIMAL(5,2) NOT NULL,
    `keterangan` VARCHAR(100) DEFAULT NULL,
    `is_active` TINYINT(1) DEFAULT 1,
    `updated_at` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    PRIMARY KEY (`id`),
    UNIQUE KEY `lapisan` (`lapisan`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Insert default progressive tax rates (UU HPP 2021)
INSERT INTO `master_tarif_pph` (`lapisan`, `batas_bawah`, `batas_atas`, `tarif_persen`, `keterangan`) VALUES
(1, 0.00, 60000000.00, 5.00, 'Lapisan 1: 0 - 60 Juta'),
(2, 60000000.00, 250000000.00, 15.00, 'Lapisan 2: 60 Juta - 250 Juta'),
(3, 250000000.00, 500000000.00, 25.00, 'Lapisan 3: 250 Juta - 500 Juta'),
(4, 500000000.00, 5000000000.00, 30.00, 'Lapisan 4: 500 Juta - 5 Milyar'),
(5, 5000000000.00, 999999999999.00, 35.00, 'Lapisan 5: > 5 Milyar')
ON DUPLICATE KEY UPDATE 
    batas_bawah = VALUES(batas_bawah),
    batas_atas = VALUES(batas_atas),
    tarif_persen = VALUES(tarif_persen),
    keterangan = VALUES(keterangan);
