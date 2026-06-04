-- Script kiểm tra mapping giữa Chitietdonhang và Sanphamthuoc
USE [NhaThuocDB1]
GO

-- 1. Kiểm tra tất cả Masanpham trong Chitietdonhang
PRINT N'=== TẤT CẢ MASANPHAM TRONG CHITIETDONHANG ==='
SELECT DISTINCT Masanpham 
FROM Chitietdonhang 
ORDER BY Masanpham;

-- 2. Kiểm tra tất cả Mahanghoa trong Sanphamthuoc
PRINT N'=== TẤT CẢ MAHANGHOA TRONG SANPHAMTHUOC ==='
SELECT DISTINCT Mahanghoa 
FROM Sanphamthuoc 
ORDER BY Mahanghoa;

-- 3. Kiểm tra mapping giữa hai bảng
PRINT N'=== MAPPING GIỮA CHITIETDONHANG VÀ SANPHAMTHUOC ==='
SELECT 
    ct.Masanpham,
    sp.Mahanghoa,
    sp.Tenhang,
    sp.Kho,
    CASE 
        WHEN sp.Mahanghoa IS NULL THEN 'KHÔNG TÌM THẤY'
        ELSE 'TÌM THẤY'
    END as Status
FROM (SELECT DISTINCT Masanpham FROM Chitietdonhang) ct
LEFT JOIN Sanphamthuoc sp ON ct.Masanpham = sp.Mahanghoa
ORDER BY ct.Masanpham;

-- 4. Kiểm tra dữ liệu chi tiết của các đơn hàng có trong YeuCauHoanDon
PRINT N'=== CHI TIẾT CÁC ĐƠN HÀNG CÓ TRONG YEUCAUHOANDON ==='
SELECT 
    y.Madonhang,
    ct.Masanpham,
    ct.Soluong,
    sp.Mahanghoa,
    sp.Tenhang,
    sp.Kho,
    CASE 
        WHEN sp.Mahanghoa IS NULL THEN 'KHÔNG TÌM THẤY SẢN PHẨM'
        ELSE 'TÌM THẤY SẢN PHẨM'
    END as Status
FROM YeuCauHoanDon y
JOIN Chitietdonhang ct ON y.Madonhang = ct.Madonhang
LEFT JOIN Sanphamthuoc sp ON ct.Masanpham = sp.Mahanghoa
ORDER BY y.Madonhang, ct.Masanpham;

-- 5. Test query cập nhật kho cho từng sản phẩm
PRINT N'=== TEST QUERY CẬP NHẬT KHO ==='
SELECT 
    'UPDATE Sanphamthuoc SET Kho = Kho + ' + CAST(ct.Soluong AS VARCHAR(10)) + 
    ' WHERE Mahanghoa = ''' + ct.Masanpham + '''' as UpdateQuery,
    ct.Masanpham,
    ct.Soluong,
    sp.Tenhang,
    sp.Kho as KhoHienTai
FROM YeuCauHoanDon y
JOIN Chitietdonhang ct ON y.Madonhang = ct.Madonhang
LEFT JOIN Sanphamthuoc sp ON ct.Masanpham = sp.Mahanghoa
ORDER BY y.Madonhang, ct.Masanpham;


