-- Script kiểm tra mapping chính xác giữa Chitietdonhang và Sanphamthuoc
USE [NhaThuocDB1]
GO

-- 1. Kiểm tra tất cả Masanpham trong Chitietdonhang
PRINT N'=== TẤT CẢ MASANPHAM TRONG CHITIETDONHANG ==='
SELECT DISTINCT Masanpham 
FROM Chitietdonhang 
ORDER BY Masanpham;

-- 2. Kiểm tra tất cả ID và Mahanghoa trong Sanphamthuoc
PRINT N'=== TẤT CẢ ID VÀ MAHANGHOA TRONG SANPHAMTHUOC ==='
SELECT DISTINCT ID, Mahanghoa 
FROM Sanphamthuoc 
ORDER BY ID;

-- 3. Kiểm tra mapping giữa hai bảng
PRINT N'=== MAPPING GIỮA CHITIETDONHANG VÀ SANPHAMTHUOC ==='
SELECT 
    ct.Masanpham,
    sp.ID,
    sp.Mahanghoa,
    sp.Tenhang,
    sp.Kho,
    CASE 
        WHEN sp.ID IS NULL AND sp.Mahanghoa IS NULL THEN 'KHÔNG TÌM THẤY'
        WHEN sp.ID = ct.Masanpham THEN 'MAP VỚI ID'
        WHEN sp.Mahanghoa = ct.Masanpham THEN 'MAP VỚI MAHANGHOA'
        ELSE 'MAP KHÔNG RÕ'
    END as MappingType
FROM (SELECT DISTINCT Masanpham FROM Chitietdonhang) ct
LEFT JOIN Sanphamthuoc sp ON (ct.Masanpham = sp.ID OR ct.Masanpham = sp.Mahanghoa)
ORDER BY ct.Masanpham;

-- 4. Kiểm tra dữ liệu chi tiết của các đơn hàng có trong YeuCauHoanDon
PRINT N'=== CHI TIẾT CÁC ĐƠN HÀNG CÓ TRONG YEUCAUHOANDON ==='
SELECT 
    y.Madonhang,
    ct.Masanpham,
    ct.Soluong,
    sp.ID,
    sp.Mahanghoa,
    sp.Tenhang,
    sp.Kho,
    CASE 
        WHEN sp.ID IS NULL AND sp.Mahanghoa IS NULL THEN 'KHÔNG TÌM THẤY SẢN PHẨM'
        WHEN sp.ID = ct.Masanpham THEN 'MAP VỚI ID'
        WHEN sp.Mahanghoa = ct.Masanpham THEN 'MAP VỚI MAHANGHOA'
        ELSE 'MAP KHÔNG RÕ'
    END as MappingType
FROM YeuCauHoanDon y
JOIN Chitietdonhang ct ON y.Madonhang = ct.Madonhang
LEFT JOIN Sanphamthuoc sp ON (ct.Masanpham = sp.ID OR ct.Masanpham = sp.Mahanghoa)
ORDER BY y.Madonhang, ct.Masanpham;

-- 5. Test query cập nhật kho cho từng sản phẩm
PRINT N'=== TEST QUERY CẬP NHẬT KHO ==='
SELECT 
    'UPDATE Sanphamthuoc SET Kho = Kho + ' + CAST(ct.Soluong AS VARCHAR(10)) + 
    ' WHERE ID = ' + CAST(ct.Masanpham AS VARCHAR(10)) + ' OR Mahanghoa = ''' + ct.Masanpham + '''' as UpdateQuery,
    ct.Masanpham,
    ct.Soluong,
    sp.Tenhang,
    sp.Kho as KhoHienTai,
    CASE 
        WHEN sp.ID = ct.Masanpham THEN 'MAP VỚI ID'
        WHEN sp.Mahanghoa = ct.Masanpham THEN 'MAP VỚI MAHANGHOA'
        ELSE 'KHÔNG MAP'
    END as MappingType
FROM YeuCauHoanDon y
JOIN Chitietdonhang ct ON y.Madonhang = ct.Madonhang
LEFT JOIN Sanphamthuoc sp ON (ct.Masanpham = sp.ID OR ct.Masanpham = sp.Mahanghoa)
ORDER BY y.Madonhang, ct.Masanpham;


