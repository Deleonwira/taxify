-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 14, 2025 at 08:19 AM
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
-- Database: `app_pajak_v2`
--

DELIMITER $$
--
-- Procedures
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_kalkulasi_spt_tahunan` (IN `p_wajib_pajak_id` INT, IN `p_tahun_pajak` INT)   BEGIN
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

-- --------------------------------------------------------

--
-- Table structure for table `admin`
--

CREATE TABLE `admin` (
  `id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `nama` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `no_telepon` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `admin`
--

INSERT INTO `admin` (`id`, `user_id`, `nama`, `email`, `no_telepon`) VALUES
(1, 1, 'Super Admin', 'admin@taxify.com', '08110000001');

-- --------------------------------------------------------

--
-- Table structure for table `bukti_potong`
--

CREATE TABLE `bukti_potong` (
  `id` int(11) NOT NULL,
  `nomor_bukti` varchar(50) DEFAULT NULL,
  `pekerjaan_id` int(11) NOT NULL,
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
  `created_at` datetime DEFAULT current_timestamp(),
  `created_by` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `bukti_potong`
--

INSERT INTO `bukti_potong` (`id`, `nomor_bukti`, `pekerjaan_id`, `masa_bulan`, `masa_tahun`, `gaji_pokok`, `tunjangan`, `bonus_thr`, `bruto_total`, `biaya_jabatan`, `iuran_pensiun`, `netto_total`, `ptkp`, `pkp`, `pph21_terutang`, `created_at`, `created_by`) VALUES
(1, 'BP-2025-01-6302', 1, 1, 2025, 9000000.00, 0.00, 0.00, 9000000.00, 450000.00, 0.00, 8550000.00, 4500000.00, 4050000.00, 202500.00, '2025-12-11 10:47:27', 1),
(2, 'BP-2025-02-2868', 1, 2, 2025, 9000000.00, 450000.00, 0.00, 9450000.00, 472500.00, 0.00, 8977500.00, 4500000.00, 4477500.00, 223875.00, '2025-12-11 11:10:23', 1),
(3, 'BP-2025-03-1568', 1, 3, 2025, 9000000.00, 0.00, 0.00, 9000000.00, 450000.00, 0.00, 8550000.00, 4500000.00, 4050000.00, 202500.00, '2025-12-11 11:10:35', 1),
(4, 'BP-2025-04-4780', 1, 4, 2025, 9000000.00, 750000.00, 0.00, 9750000.00, 487500.00, 0.00, 9262500.00, 4500000.00, 4762500.00, 238125.00, '2025-12-11 11:10:59', 1),
(5, 'BP-2025-01-5890', 3, 1, 2025, 10000000.00, 0.00, 0.00, 10000000.00, 500000.00, 0.00, 9500000.00, 4875000.00, 4625000.00, 231250.00, '2025-12-11 11:11:37', 1);

--
-- Triggers `bukti_potong`
--
DELIMITER $$
CREATE TRIGGER `trg_bukti_potong_after_delete` AFTER DELETE ON `bukti_potong` FOR EACH ROW BEGIN
    DECLARE v_wp_id INT;
    SELECT wajib_pajak_id INTO v_wp_id FROM pekerjaan WHERE id = OLD.pekerjaan_id;
    CALL sp_kalkulasi_spt_tahunan(v_wp_id, OLD.masa_tahun);
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trg_bukti_potong_after_insert` AFTER INSERT ON `bukti_potong` FOR EACH ROW BEGIN
    DECLARE v_wp_id INT;
    SELECT wajib_pajak_id INTO v_wp_id FROM pekerjaan WHERE id = NEW.pekerjaan_id;
    CALL sp_kalkulasi_spt_tahunan(v_wp_id, NEW.masa_tahun);
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trg_bukti_potong_after_update` AFTER UPDATE ON `bukti_potong` FOR EACH ROW BEGIN
    DECLARE v_wp_id_new INT;
    DECLARE v_wp_id_old INT;
    
    SELECT wajib_pajak_id INTO v_wp_id_new FROM pekerjaan WHERE id = NEW.pekerjaan_id;
    SELECT wajib_pajak_id INTO v_wp_id_old FROM pekerjaan WHERE id = OLD.pekerjaan_id;
    
    CALL sp_kalkulasi_spt_tahunan(v_wp_id_new, NEW.masa_tahun);
    
    IF (NEW.masa_tahun != OLD.masa_tahun) OR (v_wp_id_new != v_wp_id_old) THEN
        CALL sp_kalkulasi_spt_tahunan(v_wp_id_old, OLD.masa_tahun);
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `bukti_potong_freelance`
--

CREATE TABLE `bukti_potong_freelance` (
  `id` int(11) NOT NULL,
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
  `created_at` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `pekerjaan`
--

CREATE TABLE `pekerjaan` (
  `id` int(11) NOT NULL,
  `wajib_pajak_id` int(11) NOT NULL,
  `perusahaan_id` int(11) NOT NULL,
  `jabatan` varchar(100) DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `pekerjaan`
--

INSERT INTO `pekerjaan` (`id`, `wajib_pajak_id`, `perusahaan_id`, `jabatan`, `created_at`) VALUES
(1, 1, 1, 'Staff IT', '2025-12-09 16:26:38'),
(2, 1, 2, 'Freelance Developer', '2025-12-09 16:26:38'),
(3, 3, 1, 'Software Engineer', '2025-12-11 11:00:05'),
(4, 4, 4, 'Product Manager', '2025-12-11 11:00:05'),
(6, 5, 5, 'Marketing Manager', '2025-12-11 11:00:05'),
(7, 6, 1, 'Finance Staff', '2025-12-11 11:00:05'),
(9, 7, 6, 'IT Consultant', '2025-12-11 11:00:05'),
(10, 8, 6, 'HR Manager', '2025-12-11 11:00:05'),
(12, 9, 7, 'Creative Director', '2025-12-11 11:00:05'),
(13, 10, 7, 'Graphic Designer', '2025-12-11 11:00:05');

-- --------------------------------------------------------

--
-- Table structure for table `pemberi_kerja`
--

CREATE TABLE `pemberi_kerja` (
  `id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `perusahaan_id` int(11) NOT NULL,
  `nama` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `no_telepon` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `pemberi_kerja`
--

INSERT INTO `pemberi_kerja` (`id`, `user_id`, `perusahaan_id`, `nama`, `email`, `no_telepon`) VALUES
(1, 2, 1, 'Budi HRD', 'hrd@majumundur.com', '08120000002');

-- --------------------------------------------------------

--
-- Table structure for table `perusahaan`
--

CREATE TABLE `perusahaan` (
  `id` int(11) NOT NULL,
  `nama_perusahaan` varchar(150) NOT NULL,
  `npwp_perusahaan` varchar(30) DEFAULT NULL,
  `alamat` text DEFAULT NULL,
  `kota` varchar(100) DEFAULT NULL,
  `no_telepon` varchar(20) DEFAULT NULL,
  `email_perusahaan` varchar(100) DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `perusahaan`
--

INSERT INTO `perusahaan` (`id`, `nama_perusahaan`, `npwp_perusahaan`, `alamat`, `kota`, `no_telepon`, `email_perusahaan`, `created_at`) VALUES
(1, 'PT Maju Mundur', '012345678901234', 'Jl. Sudirman No 12', 'Jakarta Selatan', '0219876543', 'office@majumundur.com', '2025-12-09 16:26:38'),
(2, 'Freelance / Tidak Terdaftar', NULL, NULL, NULL, NULL, NULL, '2025-12-09 16:26:38'),
(3, 'PT ABC', '0912738123', 'JL. Lamda Raya', 'Jakarta Selatan', '0219876543', 'office@abc.com', '2025-12-09 16:26:38'),
(4, 'PT Teknologi Maju', '01.234.567.8-901.000', 'Jl. Gatot Subroto Kav. 12', 'Jakarta Selatan', '0215551234', 'info@teknologimaju.co.id', '2025-12-11 11:00:05'),
(5, 'CV Berkah Abadi', '02.345.678.9-012.000', 'Jl. Ahmad Yani No. 45', 'Surabaya', '0315556789', 'admin@berkah-abadi.com', '2025-12-11 11:00:05'),
(6, 'PT Global Solusi', '03.456.789.0-123.000', 'Jl. Sudirman No. 100', 'Jakarta Pusat', '0215559999', 'contact@globalsolusi.id', '2025-12-11 11:00:05'),
(7, 'CV Kreatif Digital', '04.567.890.1-234.000', 'Jl. Diponegoro No. 23', 'Bandung', '0225553333', 'hello@kreatifdigital.net', '2025-12-11 11:00:05'),
(8, 'PT Nusantara Jaya', '05.678.901.2-345.000', 'Jl. Pemuda No. 78', 'Semarang', '0245557777', 'hrd@nusantarajaya.co.id', '2025-12-11 11:00:05'),
(10, 'astra', '112980123', NULL, NULL, NULL, NULL, '2025-12-13 02:29:44');

-- --------------------------------------------------------

--
-- Table structure for table `spt_tahunan`
--

CREATE TABLE `spt_tahunan` (
  `id` int(11) NOT NULL,
  `wajib_pajak_id` int(11) NOT NULL,
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

INSERT INTO `spt_tahunan` (`id`, `wajib_pajak_id`, `tahun_pajak`, `status_ptkp`, `gaji_setahun`, `tunjangan_setahun`, `bonus_thr_setahun`, `bruto_setahun`, `biaya_jabatan_setahun`, `iuran_pensiun_setahun`, `netto_setahun`, `ptkp`, `pkp`, `pph21_terutang`, `pph21_dipotong`, `pph21_kurang_bayar`, `pph21_lebih_bayar`, `status_spt`, `tanggal_lapor`) VALUES
(1, 1, 2025, 'TK0', 36000000.00, 1200000.00, 0.00, 37200000.00, 1860000.00, 0.00, 35340000.00, 54000000.00, 0.00, 0.00, 867000.00, 0.00, 867000.00, 'Lebih Bayar', '2025-12-11 11:10:59'),
(5, 3, 2025, 'K0', 10000000.00, 0.00, 0.00, 10000000.00, 500000.00, 0.00, 9500000.00, 58500000.00, 0.00, 0.00, 231250.00, 0.00, 231250.00, 'Lebih Bayar', '2025-12-11 11:11:37');

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `username` varchar(50) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `tipe_user` enum('admin','pemberi_kerja','wajib_pajak') NOT NULL,
  `is_active` tinyint(1) DEFAULT 1,
  `created_at` datetime DEFAULT current_timestamp(),
  `last_login` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`id`, `username`, `password_hash`, `tipe_user`, `is_active`, `created_at`, `last_login`) VALUES
(1, 'admin', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'admin', 1, '2025-12-09 16:26:38', NULL),
(2, 'hrd_majumundur', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'pemberi_kerja', 1, '2025-12-09 16:26:38', NULL),
(3, 'andi_pegawai', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'wajib_pajak', 1, '2025-12-09 16:26:38', NULL),
(4, 'hamza_wp', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'wajib_pajak', 1, '2025-12-09 16:26:38', NULL),
(5, 'wp_budi_santoso', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'wajib_pajak', 1, '2025-12-11 11:00:05', NULL),
(6, 'wp_siti_rahayu', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'wajib_pajak', 1, '2025-12-11 11:00:05', NULL),
(7, 'wp_agus_wijaya', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'wajib_pajak', 1, '2025-12-11 11:00:05', NULL),
(8, 'wp_dewi_lestari', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'wajib_pajak', 1, '2025-12-11 11:00:05', NULL),
(9, 'wp_ahmad_hidayat', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'wajib_pajak', 1, '2025-12-11 11:00:05', NULL),
(10, 'wp_rina_permata', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'wajib_pajak', 1, '2025-12-11 11:00:05', NULL),
(11, 'wp_joko_prasetyo', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'wajib_pajak', 1, '2025-12-11 11:00:05', NULL),
(12, 'wp_maya_sari', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'wajib_pajak', 1, '2025-12-11 11:00:05', NULL),
(13, 'jamalmusiala', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'wajib_pajak', 1, '2025-12-11 15:29:08', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `wajib_pajak`
--

CREATE TABLE `wajib_pajak` (
  `id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `npwp` varchar(24) NOT NULL,
  `nik` varchar(20) NOT NULL,
  `nama` varchar(100) NOT NULL,
  `email` varchar(100) NOT NULL,
  `no_telepon` varchar(20) DEFAULT NULL,
  `alamat` text DEFAULT NULL,
  `status_ptkp` varchar(10) DEFAULT 'TK0',
  `status_validasi` enum('pending','approved','rejected') DEFAULT 'pending'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `wajib_pajak`
--

INSERT INTO `wajib_pajak` (`id`, `user_id`, `npwp`, `nik`, `nama`, `email`, `no_telepon`, `alamat`, `status_ptkp`, `status_validasi`) VALUES
(1, 3, '789', '3201010101010001', 'Andi Pegawai', 'andi@gmail.com', '08130000003', 'Depok', 'TK0', 'approved'),
(2, 4, '123456789100000', '3201010101010002', 'Hamza Deleon Wiradarma', 'hamzadeleonw123@gmail.com', '083103293225', 'Lamda Raya', 'TK0', 'approved'),
(3, 5, '123456789010005', '3201010101010005', 'Budi Santoso', 'budi_santoso@email.com', '081200000005', 'Jakarta', 'K0', 'approved'),
(4, 6, '123456789010006', '3201010101010006', 'Siti Rahayu', 'siti_rahayu@email.com', '081200000006', 'Jakarta', 'K1', 'approved'),
(5, 7, '123456789010007', '3201010101010007', 'Agus Wijaya', 'agus_wijaya@email.com', '081200000007', 'Jakarta', 'K0', 'approved'),
(6, 8, '123456789010008', '3201010101010008', 'Dewi Lestari', 'dewi_lestari@email.com', '081200000008', 'Jakarta', 'K1', 'approved'),
(7, 9, '123456789010009', '3201010101010009', 'Ahmad Hidayat', 'ahmad_hidayat@email.com', '081200000009', 'Jakarta', 'TK0', 'approved'),
(8, 10, '123456789010010', '3201010101010010', 'Rina Permata', 'rina_permata@email.com', '081200000010', 'Jakarta', 'TK0', 'approved'),
(9, 11, '123456789010011', '3201010101010011', 'Joko Prasetyo', 'joko_prasetyo@email.com', '081200000011', 'Jakarta', 'K0', 'approved'),
(10, 12, '123456789010012', '3201010101010012', 'Maya Sari', 'maya_sari@email.com', '081200000012', 'Jakarta', 'TK0', 'approved'),
(18, 13, '127309127309172', '1234567891000000', 'Jamal Musiala', 'jamalprakasa@gmail.com', '083103293225', 'lalasd', 'K1', 'pending');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `admin`
--
ALTER TABLE `admin`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `user_id` (`user_id`);

--
-- Indexes for table `bukti_potong`
--
ALTER TABLE `bukti_potong`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `nomor_bukti` (`nomor_bukti`),
  ADD KEY `pekerjaan_id` (`pekerjaan_id`),
  ADD KEY `created_by` (`created_by`);

--
-- Indexes for table `bukti_potong_freelance`
--
ALTER TABLE `bukti_potong_freelance`
  ADD PRIMARY KEY (`id`),
  ADD KEY `wajib_pajak_id` (`wajib_pajak_id`);

--
-- Indexes for table `pekerjaan`
--
ALTER TABLE `pekerjaan`
  ADD PRIMARY KEY (`id`),
  ADD KEY `wajib_pajak_id` (`wajib_pajak_id`),
  ADD KEY `perusahaan_id` (`perusahaan_id`);

--
-- Indexes for table `pemberi_kerja`
--
ALTER TABLE `pemberi_kerja`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `user_id` (`user_id`),
  ADD KEY `perusahaan_id` (`perusahaan_id`);

--
-- Indexes for table `perusahaan`
--
ALTER TABLE `perusahaan`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `npwp_perusahaan` (`npwp_perusahaan`);

--
-- Indexes for table `spt_tahunan`
--
ALTER TABLE `spt_tahunan`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `unique_wp_tahun` (`wajib_pajak_id`,`tahun_pajak`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `username` (`username`);

--
-- Indexes for table `wajib_pajak`
--
ALTER TABLE `wajib_pajak`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `user_id` (`user_id`),
  ADD UNIQUE KEY `npwp` (`npwp`),
  ADD UNIQUE KEY `nik` (`nik`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `admin`
--
ALTER TABLE `admin`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `bukti_potong`
--
ALTER TABLE `bukti_potong`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `bukti_potong_freelance`
--
ALTER TABLE `bukti_potong_freelance`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `pekerjaan`
--
ALTER TABLE `pekerjaan`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `pemberi_kerja`
--
ALTER TABLE `pemberi_kerja`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `perusahaan`
--
ALTER TABLE `perusahaan`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `spt_tahunan`
--
ALTER TABLE `spt_tahunan`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `wajib_pajak`
--
ALTER TABLE `wajib_pajak`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `admin`
--
ALTER TABLE `admin`
  ADD CONSTRAINT `admin_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `bukti_potong`
--
ALTER TABLE `bukti_potong`
  ADD CONSTRAINT `bukti_potong_ibfk_1` FOREIGN KEY (`pekerjaan_id`) REFERENCES `pekerjaan` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `bukti_potong_ibfk_2` FOREIGN KEY (`created_by`) REFERENCES `pemberi_kerja` (`id`) ON DELETE SET NULL;

--
-- Constraints for table `bukti_potong_freelance`
--
ALTER TABLE `bukti_potong_freelance`
  ADD CONSTRAINT `bukti_potong_freelance_ibfk_1` FOREIGN KEY (`wajib_pajak_id`) REFERENCES `wajib_pajak` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `pekerjaan`
--
ALTER TABLE `pekerjaan`
  ADD CONSTRAINT `pekerjaan_ibfk_1` FOREIGN KEY (`wajib_pajak_id`) REFERENCES `wajib_pajak` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `pekerjaan_ibfk_2` FOREIGN KEY (`perusahaan_id`) REFERENCES `perusahaan` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `pemberi_kerja`
--
ALTER TABLE `pemberi_kerja`
  ADD CONSTRAINT `pemberi_kerja_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `pemberi_kerja_ibfk_2` FOREIGN KEY (`perusahaan_id`) REFERENCES `perusahaan` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `spt_tahunan`
--
ALTER TABLE `spt_tahunan`
  ADD CONSTRAINT `spt_tahunan_ibfk_1` FOREIGN KEY (`wajib_pajak_id`) REFERENCES `wajib_pajak` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `wajib_pajak`
--
ALTER TABLE `wajib_pajak`
  ADD CONSTRAINT `wajib_pajak_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
