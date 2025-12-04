# Dokumentasi Trigger SPT Tahunan

## 📋 Deskripsi

Sistem ini mengotomasi kalkulasi SPT Tahunan setiap kali ada perubahan pada tabel `bukti_potong`. Data bulanan dari bukti potong akan diagregasi secara otomatis ke dalam tabel `spt_tahunan`.

## 🎯 Cara Kerja

### 1. **Stored Procedure: `sp_kalkulasi_spt_tahunan`**

Procedure ini melakukan:
- **Agregasi data bulanan**: Menjumlahkan semua bukti potong untuk satu wajib pajak di tahun tertentu
- **Perhitungan PTKP**: Mengambil status PTKP dari tabel `pekerjaan` dan menghitung nilai PTKP tahunan
- **Perhitungan PKP**: Menghitung Penghasilan Kena Pajak (netto - PTKP)
- **Perhitungan PPh21 Terutang**: Menggunakan tarif progresif 2024
- **Perhitungan selisih**: Menentukan Kurang Bayar, Lebih Bayar, atau Nihil
- **Insert/Update**: Memasukkan atau memperbarui data di tabel `spt_tahunan`

### 2. **Trigger Otomatis**

**3 Trigger yang dibuat:**

| Trigger | Event | Fungsi |
|---------|-------|--------|
| `trg_bukti_potong_after_insert` | AFTER INSERT | Kalkulasi SPT saat bukti potong baru dibuat |
| `trg_bukti_potong_after_update` | AFTER UPDATE | Kalkulasi ulang saat bukti potong diubah |
| `trg_bukti_potong_after_delete` | AFTER DELETE | Kalkulasi ulang saat bukti potong dihapus |

## 💰 Tarif Pajak yang Digunakan

Menggunakan tarif progresif PPh21 tahun 2024:

| Layer | PKP (Penghasilan Kena Pajak) | Tarif |
|-------|------------------------------|-------|
| 1 | 0 - 60 juta | 5% |
| 2 | 60 - 250 juta | 15% |
| 3 | 250 - 500 juta | 25% |
| 4 | 500 juta - 5 miliar | 30% |
| 5 | > 5 miliar | 35% |

## 📊 Nilai PTKP yang Digunakan

| Status | Deskripsi | Nilai PTKP Tahunan |
|--------|-----------|-------------------|
| TK0 | Tidak Kawin, 0 tanggungan | Rp 54.000.000 |
| TK1 | Tidak Kawin, 1 tanggungan | Rp 58.500.000 |
| TK2 | Tidak Kawin, 2 tanggungan | Rp 63.000.000 |
| TK3 | Tidak Kawin, 3 tanggungan | Rp 67.500.000 |
| K0 | Kawin, 0 tanggungan | Rp 58.500.000 |
| K1 | Kawin, 1 tanggungan | Rp 63.000.000 |
| K2 | Kawin, 2 tanggungan | Rp 67.500.000 |
| K3 | Kawin, 3 tanggungan | Rp 72.000.000 |

## 🔧 Cara Instalasi

### 1. **Jalankan Script SQL**

```bash
mysql -u root -p app_pajak < trigger_spt_tahunan.sql
```

Atau melalui phpMyAdmin:
1. Buka phpMyAdmin
2. Pilih database `app_pajak`
3. Klik tab "SQL"
4. Copy-paste isi file `trigger_spt_tahunan.sql`
5. Klik "Go"

### 2. **Verifikasi Instalasi**

Cek apakah trigger sudah terpasang:

```sql
-- Cek stored procedure
SHOW PROCEDURE STATUS WHERE Db = 'app_pajak';

-- Cek trigger
SHOW TRIGGERS FROM app_pajak;
```

## 🧪 Testing

### Test 1: Insert Bukti Potong Baru

```sql
-- Insert bukti potong untuk test
INSERT INTO bukti_potong (
    nomor_bukti, perusahaan_id, wp_npwp, masa_bulan, masa_tahun,
    gaji_pokok, tunjangan, bonus_thr, bruto_total,
    biaya_jabatan, iuran_pensiun, netto_total,
    ptkp, pkp, pph21_terutang
) VALUES (
    'BP-2025-TEST-001', 1, '789', 4, 2025,
    6000000, 500000, 0, 6500000,
    300000, 50000, 6150000,
    5400000, 750000, 75000
);

-- Cek apakah data SPT Tahunan otomatis terupdate
SELECT * FROM spt_tahunan WHERE wp_npwp = '789' AND tahun_pajak = 2025;
```

### Test 2: Update Bukti Potong

```sql
-- Update bukti potong
UPDATE bukti_potong 
SET gaji_pokok = 7000000, bruto_total = 7500000
WHERE nomor_bukti = 'BP-2025-TEST-001';

-- Cek apakah SPT Tahunan otomatis terupdate
SELECT * FROM spt_tahunan WHERE wp_npwp = '789' AND tahun_pajak = 2025;
```

### Test 3: Delete Bukti Potong

```sql
-- Delete bukti potong
DELETE FROM bukti_potong WHERE nomor_bukti = 'BP-2025-TEST-001';

-- Cek apakah SPT Tahunan otomatis terupdate
SELECT * FROM spt_tahunan WHERE wp_npwp = '789' AND tahun_pajak = 2025;
```

## 🔄 Kalkulasi Manual (Jika Diperlukan)

Jika ingin kalkulasi ulang untuk wajib pajak tertentu:

```sql
-- Kalkulasi ulang SPT Tahunan untuk NPWP '789' tahun 2025
CALL sp_kalkulasi_spt_tahunan('789', 2025);
```

Kalkulasi ulang semua data (untuk migrasi data lama):

```sql
-- Kalkulasi ulang semua wajib pajak
INSERT INTO spt_tahunan (wp_npwp, tahun_pajak, status_ptkp, gaji_setahun, tunjangan_setahun, bonus_thr_setahun, bruto_setahun, biaya_jabatan_setahun, iuran_pensiun_setahun, netto_setahun, ptkp, pkp, pph21_terutang, pph21_dipotong, pph21_kurang_bayar, pph21_lebih_bayar, status_spt)
SELECT 
    wp_npwp,
    masa_tahun,
    (SELECT status_ptkp FROM pekerjaan WHERE pekerjaan.wp_npwp = bukti_potong.wp_npwp LIMIT 1),
    SUM(gaji_pokok),
    SUM(tunjangan),
    SUM(bonus_thr),
    SUM(bruto_total),
    SUM(biaya_jabatan),
    SUM(iuran_pensiun),
    SUM(netto_total),
    0, 0, 0, SUM(pph21_terutang), 0, 0, 'Nihil'
FROM bukti_potong
GROUP BY wp_npwp, masa_tahun
ON DUPLICATE KEY UPDATE 
    gaji_setahun = VALUES(gaji_setahun),
    tunjangan_setahun = VALUES(tunjangan_setahun),
    bonus_thr_setahun = VALUES(bonus_thr_setahun);
```

Atau gunakan loop untuk setiap WP:

```sql
-- Untuk setiap kombinasi wp_npwp dan tahun
DELIMITER $$
CREATE PROCEDURE sp_recalculate_all()
BEGIN
    DECLARE done INT DEFAULT FALSE;
    DECLARE v_npwp VARCHAR(24);
    DECLARE v_tahun INT;
    
    DECLARE cur CURSOR FOR 
        SELECT DISTINCT wp_npwp, masa_tahun 
        FROM bukti_potong;
    
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;
    
    OPEN cur;
    
    read_loop: LOOP
        FETCH cur INTO v_npwp, v_tahun;
        IF done THEN
            LEAVE read_loop;
        END IF;
        
        CALL sp_kalkulasi_spt_tahunan(v_npwp, v_tahun);
    END LOOP;
    
    CLOSE cur;
END$$
DELIMITER ;

-- Jalankan kalkulasi ulang semua
CALL sp_recalculate_all();
```

## ⚠️ Catatan Penting

1. **Status PTKP**: Sistem mengambil status PTKP dari tabel `pekerjaan`. Pastikan data di tabel `pekerjaan` selalu update.

2. **Tarif Pajak**: Jika ada perubahan tarif pajak dari pemerintah, edit nilai di stored procedure `sp_kalkulasi_spt_tahunan`.

3. **Performance**: Untuk database dengan jutaan record, pertimbangkan untuk menggunakan background job instead of trigger.

4. **Unique Key**: Sistem menambahkan UNIQUE KEY pada kolom `(wp_npwp, tahun_pajak)` di tabel `spt_tahunan` untuk mencegah duplikasi.

## 🐛 Troubleshooting

### Error: "Duplicate entry"
- Pastikan unique key sudah ditambahkan pada tabel `spt_tahunan`
- Jalankan: `ALTER TABLE spt_tahunan DROP INDEX unique_wp_tahun;` lalu jalankan ulang script

### Trigger tidak berfungsi
- Cek apakah trigger sudah terpasang: `SHOW TRIGGERS FROM app_pajak;`
- Cek log error MySQL: `SHOW WARNINGS;`

### Kalkulasi tidak sesuai
- Verifikasi data di tabel `bukti_potong`
- Jalankan manual: `CALL sp_kalkulasi_spt_tahunan('npwp', tahun);`
- Cek hasil di tabel `spt_tahunan`

## 📞 Support

Jika ada pertanyaan atau masalah, silakan review kode di file `trigger_spt_tahunan.sql`.
