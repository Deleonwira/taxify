-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 03, 2025 at 06:40 PM
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
(4, 'BP-2025-0001', 1, '789', 1, 2025, 6000000.00, 500000.00, 0.00, 6500000.00, 300000.00, 50000.00, 6150000.00, 5400000.00, 750000.00, 75000.00, '2025-12-03 15:51:03'),
(5, 'BP-2025-0002', 1, '444444444444444', 1, 2025, 5500000.00, 300000.00, 0.00, 5800000.00, 290000.00, 50000.00, 5460000.00, 5400000.00, 60000.00, 6000.00, '2025-12-03 15:51:14'),
(7, 'BP-2025-0003', 1, '789', 2, 2025, 5500000.00, 300000.00, 0.00, 5800000.00, 290000.00, 50000.00, 5460000.00, 5400000.00, 60000.00, 6000.00, '2025-12-03 15:51:14'),
(9, 'BP-2025-0004', 3, '789', 3, 2025, 5500000.00, 300000.00, 0.00, 5800000.00, 290000.00, 50000.00, 5460000.00, 5400000.00, 60000.00, 6000.00, '2025-12-03 15:51:14');

-- --------------------------------------------------------

--
-- Table structure for table `pekerjaan`
--

CREATE TABLE `pekerjaan` (
  `id` int(11) NOT NULL,
  `wp_npwp` varchar(24) NOT NULL,
  `perusahaan_id` int(11) NOT NULL,
  `jabatan` varchar(100) DEFAULT NULL,
  `status_kepegawaian` enum('Tetap','Kontrak','Freelance','Resign') DEFAULT 'Tetap',
  `gaji_pokok` decimal(15,2) DEFAULT 0.00,
  `status_ptkp` varchar(10) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `pekerjaan`
--

INSERT INTO `pekerjaan` (`id`, `wp_npwp`, `perusahaan_id`, `jabatan`, `status_kepegawaian`, `gaji_pokok`, `status_ptkp`) VALUES
(3, '789', 1, 'Staff IT', 'Tetap', 6000000.00, 'TK0'),
(4, '444444444444444', 1, 'Marketing', 'Kontrak', 5500000.00, 'K0');

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
  `no_telepon` varchar(20) DEFAULT NULL,
  `alamat` text DEFAULT NULL,
  `nik` varchar(20) DEFAULT NULL,
  `created_at` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`npwp`, `password_hash`, `nama`, `email`, `tipe_user`, `no_telepon`, `alamat`, `nik`, `created_at`) VALUES
('111111111111111', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Super Admin', 'admin@taxify.com', 'admin', '08110000001', 'Jakarta Pusat', NULL, '2025-12-03 15:31:20'),
('222222222222222', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'PT Maju Mundur (HRD)', 'hrd@majumundur.com', 'pemberi_kerja', '08120000002', 'Jakarta Selatan', NULL, '2025-12-03 15:31:20'),
('444444444444444', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Budi Pratama', 'budi@gmail.com', 'wajib_pajak', '08140000004', 'Bogor', '3202020202020002', '2025-12-03 15:31:20'),
('789', 'ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f', 'Andi Pegawai', 'andi@gmail.com', 'wajib_pajak', '08130000003', 'Depok', '3201010101010001', '2025-12-03 15:31:20');

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
  ADD KEY `wp_npwp` (`wp_npwp`);

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
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `pekerjaan`
--
ALTER TABLE `pekerjaan`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `perusahaan`
--
ALTER TABLE `perusahaan`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `spt_tahunan`
--
ALTER TABLE `spt_tahunan`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

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
