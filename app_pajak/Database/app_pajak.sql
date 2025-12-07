-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 07, 2025 at 07:57 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `app_pajak`
--

DELIMITER $$
--
-- Procedures
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_kalkulasi_spt_tahunan` (IN `p_wp_npwp` VARCHAR(24), IN `p_tahun_pajak` INT)   BEGIN
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

DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `bukti_potong`
--

CREATE TABLE `bukti_potong` (
  `id` int(11) NOT NULL,
  `nomor_bukti` varchar(50) DEFAULT NULL,
  `perusahaan_id` int(11) NOT NULL,
  `wp_npwp` varchar(24) NOT NULL,
  `masa_bulan` int(11) NOT NULL,
  `masa_tahun` int(11) NOT NULL,
  `gaji_pokok` decimal(15,2) DEFAULT 0.00,
  `tunjangan` decimal(15,2) DEFAULT 0.00,
  `bonus_thr` decimal(15,2) DEFAULT 0.00,
  `bruto_total` decimal(15,2) DEFAULT 0.00,
  `biaya_jabatan` decimal(15,2) DEFAULT 0.00,
  `iuran_pensiun` decimal(15,2) DEFAULT 0.00,
  `netto_total` decimal(15,2) DEFAULT 0.00,
  `ptkp` decimal(15,2) DEFAULT 0.00,
  `pkp` decimal(15,2) DEFAULT 0.00,
  `pph21_terutang` decimal(15,2) DEFAULT 0.00,
  `created_at` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `bukti_potong`
--

INSERT INTO `bukti_potong` (`id`, `nomor_bukti`, `perusahaan_id`, `wp_npwp`, `masa_bulan`, `masa_tahun`, `gaji_pokok`, `tunjangan`, `bonus_thr`, `bruto_total`, `biaya_jabatan`, `iuran_pensiun`, `netto_total`, `ptkp`, `pkp`, `pph21_terutang`, `created_at`) VALUES
(10, 'BP-2025-TEST-001', 1, '789', 4, 2025, 7000000.00, 500000.00, 0.00, 7500000.00, 300000.00, 50000.00, 6150000.00, 5400000.00, 750000.00, 75000.00, '2025-12-04 00:58:44'),
(11, 'BP-2025-TEST-002', 3, '789', 4, 2025, 6000000.00, 500000.00, 0.00, 6500000.00, 300000.00, 50000.00, 6150000.00, 5400000.00, 750000.00, 75000.00, '2025-12-04 01:01:59'),
(12, 'BP-2025-001', 1, '789', 1, 2025, 5000000.00, 1000000.00, 0.00, 6000000.00, 300000.00, 100000.00, 5600000.00, 54000000.00, 0.00, 0.00, '2025-12-05 07:46:44');

--
-- Triggers `bukti_potong`
--
DELIMITER $$
CREATE TRIGGER `trg_bukti_potong_after_delete` AFTER DELETE ON `bukti_potong` FOR EACH ROW BEGIN
    CALL sp_kalkulasi_spt_tahunan(OLD.wp_npwp, OLD.masa_tahun);
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trg_bukti_potong_after_insert` AFTER INSERT ON `bukti_potong` FOR EACH ROW BEGIN
    CALL sp_kalkulasi_spt_tahunan(NEW.wp_npwp, NEW.masa_tahun);
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trg_bukti_potong_after_update` AFTER UPDATE ON `bukti_potong` FOR EACH ROW BEGIN
    -- Kalkulasi ulang untuk tahun yang baru (jika tahun berubah)
    CALL sp_kalkulasi_spt_tahunan(NEW.wp_npwp, NEW.masa_tahun);
    
    -- Jika tahun atau NPWP berubah, kalkulasi ulang untuk tahun yang lama juga
    IF (NEW.masa_tahun != OLD.masa_tahun) OR (NEW.wp_npwp != OLD.wp_npwp) THEN
        CALL sp_kalkulasi_spt_tahunan(OLD.wp_npwp, OLD.masa_tahun);
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `pekerjaan`
--

CREATE TABLE `pekerjaan` (
  `id` int(11) NOT NULL,
  `wp_npwp` varchar(24) NOT NULL,
  `perusahaan_id` int(11) NOT NULL,
  `gaji_pokok` decimal(15,2) DEFAULT 0.00,
  `status_ptkp` varchar(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `pekerjaan`
--

INSERT INTO `pekerjaan` (`id`, `wp_npwp`, `perusahaan_id`, `gaji_pokok`, `status_ptkp`) VALUES
(3, '789', 1, 6000000.00, 'TK0');

-- --------------------------------------------------------

--
-- Table structure for table `perusahaan`
--

CREATE TABLE `perusahaan` (
  `id` int(11) NOT NULL,
  `owner_npwp` varchar(24) NOT NULL,
  `nama_perusahaan` varchar(150) NOT NULL,
  `npwp_perusahaan` varchar(30) DEFAULT NULL,
  `alamat` text DEFAULT NULL,
  `kota` varchar(100) DEFAULT NULL,
  `no_telepon` varchar(20) DEFAULT NULL,
  `email_perusahaan` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `perusahaan`
--

INSERT INTO `perusahaan` (`id`, `owner_npwp`, `nama_perusahaan`, `npwp_perusahaan`, `alamat`, `kota`, `no_telepon`, `email_perusahaan`) VALUES
(1, '222222222222222', 'PT Maju Mundur', '012345678901234', 'Jl. Sudirman No 12', 'Jakarta Selatan', '0219876543', 'office@majumundur.com'),
(2, '000000000000000', 'Freelance / Tidak Terdaftar', NULL, NULL, NULL, NULL, NULL),
(3, '222222222222222', 'PT ABC', '0912738123', 'JL. Lamda Raya', 'Jakarta Selatan', '0219876543', 'office@majumundur.com');

-- --------------------------------------------------------

--
-- Table structure for table `spt_tahunan`
--

CREATE TABLE `spt_tahunan` (
  `id` int(11) NOT NULL,
  `wp_npwp` varchar(24) NOT NULL,
  `tahun_pajak` int(11) NOT NULL,
  `status_ptkp` varchar(10) DEFAULT NULL,
  `gaji_setahun` decimal(15,2) DEFAULT 0.00,
  `tunjangan_setahun` decimal(15,2) DEFAULT 0.00,
  `bonus_thr_setahun` decimal(15,2) DEFAULT 0.00,
  `bruto_setahun` decimal(15,2) DEFAULT 0.00,
  `biaya_jabatan_setahun` decimal(15,2) DEFAULT 0.00,
  `iuran_pensiun_setahun` decimal(15,2) DEFAULT 0.00,
  `netto_setahun` decimal(15,2) DEFAULT 0.00,
  `ptkp` decimal(15,2) DEFAULT 0.00,
  `pkp` decimal(15,2) DEFAULT 0.00,
  `pph21_terutang` decimal(15,2) DEFAULT 0.00,
  `pph21_dipotong` decimal(15,2) DEFAULT 0.00,
  `pph21_kurang_bayar` decimal(15,2) DEFAULT 0.00,
  `pph21_lebih_bayar` decimal(15,2) DEFAULT 0.00,
  `status_spt` enum('Lebih Bayar','Kurang Bayar','Nihil') DEFAULT 'Nihil',
  `tanggal_lapor` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `spt_tahunan`
--

INSERT INTO `spt_tahunan` (`id`, `wp_npwp`, `tahun_pajak`, `status_ptkp`, `gaji_setahun`, `tunjangan_setahun`, `bonus_thr_setahun`, `bruto_setahun`, `biaya_jabatan_setahun`, `iuran_pensiun_setahun`, `netto_setahun`, `ptkp`, `pkp`, `pph21_terutang`, `pph21_dipotong`, `pph21_kurang_bayar`, `pph21_lebih_bayar`, `status_spt`, `tanggal_lapor`) VALUES
(3, '789', 2025, 'TK0', 18000000.00, 2000000.00, 0.00, 20000000.00, 900000.00, 200000.00, 17900000.00, 54000000.00, 0.00, 0.00, 150000.00, 0.00, 150000.00, 'Lebih Bayar', '2025-12-05 07:46:44');

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `npwp` varchar(24) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `nama` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `tipe_user` enum('admin','pemberi_kerja','wajib_pajak') NOT NULL,
  `status_validasi` enum('pending','approved','rejected') DEFAULT 'pending',
  `no_telepon` varchar(20) DEFAULT NULL,
  `alamat` text DEFAULT NULL,
  `nik` varchar(20) DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`npwp`, `password_hash`, `nama`, `email`, `tipe_user`, `status_validasi`, `no_telepon`, `alamat`, `nik`, `created_at`) VALUES
('000000000000000', '', 'Tidak Ada Owner', '', 'admin', 'pending', NULL, NULL, NULL, '2025-12-07 10:45:55'),
('111111111111111', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Super Admin', 'admin@taxify.com', 'admin', 'approved', '08110000001', 'Jakarta Pusat', NULL, '2025-12-03 15:31:20'),
('123456789100000', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Hamza Deleon Wiradarma', 'hamzadeleonw123@gmail.com', 'wajib_pajak', 'approved', '083103293225', 'Lamda Raya', NULL, '2025-12-07 12:39:29'),
('222222222222222', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'PT Maju Mundur (HRD)', 'hrd@majumundur.com', 'pemberi_kerja', 'pending', '08120000002', 'Jakarta Selatan', NULL, '2025-12-03 15:31:20'),
('789', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Andi Pegawai', 'andi@gmail.com', 'wajib_pajak', 'approved', '08130000003', 'Depok', '3201010101010001', '2025-12-03 15:31:20');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `bukti_potong`
--
ALTER TABLE `bukti_potong`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `nomor_bukti` (`nomor_bukti`),
  ADD KEY `perusahaan_id` (`perusahaan_id`),
  ADD KEY `wp_npwp` (`wp_npwp`);

--
-- Indexes for table `pekerjaan`
--
ALTER TABLE `pekerjaan`
  ADD PRIMARY KEY (`id`),
  ADD KEY `wp_npwp` (`wp_npwp`),
  ADD KEY `perusahaan_id` (`perusahaan_id`);

--
-- Indexes for table `perusahaan`
--
ALTER TABLE `perusahaan`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `npwp_perusahaan` (`npwp_perusahaan`),
  ADD KEY `owner_npwp` (`owner_npwp`);

--
-- Indexes for table `spt_tahunan`
--
ALTER TABLE `spt_tahunan`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `unique_wp_tahun` (`wp_npwp`,`tahun_pajak`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`npwp`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `bukti_potong`
--
ALTER TABLE `bukti_potong`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT for table `pekerjaan`
--
ALTER TABLE `pekerjaan`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `perusahaan`
--
ALTER TABLE `perusahaan`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `spt_tahunan`
--
ALTER TABLE `spt_tahunan`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `bukti_potong`
--
ALTER TABLE `bukti_potong`
  ADD CONSTRAINT `bukti_potong_ibfk_1` FOREIGN KEY (`perusahaan_id`) REFERENCES `perusahaan` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `bukti_potong_ibfk_2` FOREIGN KEY (`wp_npwp`) REFERENCES `users` (`npwp`) ON DELETE CASCADE;

--
-- Constraints for table `pekerjaan`
--
ALTER TABLE `pekerjaan`
  ADD CONSTRAINT `pekerjaan_ibfk_1` FOREIGN KEY (`wp_npwp`) REFERENCES `users` (`npwp`) ON DELETE CASCADE,
  ADD CONSTRAINT `pekerjaan_ibfk_2` FOREIGN KEY (`perusahaan_id`) REFERENCES `perusahaan` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `perusahaan`
--
ALTER TABLE `perusahaan`
  ADD CONSTRAINT `perusahaan_ibfk_1` FOREIGN KEY (`owner_npwp`) REFERENCES `users` (`npwp`) ON DELETE CASCADE;

--
-- Constraints for table `spt_tahunan`
--
ALTER TABLE `spt_tahunan`
  ADD CONSTRAINT `spt_tahunan_ibfk_1` FOREIGN KEY (`wp_npwp`) REFERENCES `users` (`npwp`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
