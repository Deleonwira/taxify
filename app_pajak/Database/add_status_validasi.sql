-- ============================================
-- Script: Add status_validasi column to users table
-- Description: For admin validation workflow
-- ============================================

-- Add column status_validasi
ALTER TABLE users 
ADD COLUMN status_validasi ENUM('pending', 'approved', 'rejected') 
DEFAULT 'pending' 
AFTER tipe_user;

-- Update existing users to 'approved' (they were already validated before this feature)
UPDATE users SET status_validasi = 'approved' WHERE status_validasi = 'pending';

-- Verify
SELECT npwp, nama, tipe_user, status_validasi FROM users;
