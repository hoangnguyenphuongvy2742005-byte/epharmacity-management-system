-- Script kiểm tra dữ liệu để debug vấn đề cập nhật kho
USE [NhaThuocDB1]
GO

-- 1. Kiểm tra dữ liệu trong Chitietdonhang
PRINT N'=== KIỂM TRA DỮ LIỆU CHITIETDONHANG ==='
SELECT TOP 10 
    Madonhang,
    Masanpham,
    Soluong,
    DonGia,
    Tongtiensanpham
FROM Chitietdonhang
ORDER BY Madonhang DESC;

-- 2. Kiểm tra dữ liệu trong Sanphamthuoc
PRINT N'=== KIỂM TRA DỮ LIỆU SANPHAMTHUOC ==='
SELECT TOP 10 
    Mahanghoa,
    Tenhang,
    Soluong,
    Kho
FROM Sanphamthuoc
ORDER BY Mahanghoa;

-- 3. Kiểm tra mapping giữa Chitietdonhang và Sanphamthuoc
PRINT N'=== KIỂM TRA MAPPING GIỮA CHITIETDONHANG VÀ SANPHAMTHUOC ==='
SELECT 
    ct.Madonhang,
    ct.Masanpham,
    ct.Soluong,
    sp.Mahanghoa,
    sp.Tenhang,
    sp.Soluong as SoluongKho
FROM Chitietdonhang ct
LEFT JOIN Sanphamthuoc sp ON ct.Masanpham = sp.Mahanghoa
WHERE ct.Madonhang IN (1, 2, 6, 7, 8, 9)
ORDER BY ct.Madonhang;

-- 4. Kiểm tra dữ liệu trong YeuCauHoanDon
PRINT N'=== KIỂM TRA DỮ LIỆU YEUCAUHOANDON ==='
SELECT 
    ID,
    Madonhang,
    Sodienthoaikhachhang,
    Lydo,
    YeuCau,
    Trangthai
FROM YeuCauHoanDon
ORDER BY ID;

-- 5. Test query cập nhật kho (không thực thi)
PRINT N'=== TEST QUERY CẬP NHẬT KHO ==='
SELECT 
    'UPDATE Sanphamthuoc SET Soluong = Soluong + ' + CAST(ct.Soluong AS VARCHAR(10)) + 
    ' WHERE Mahanghoa = ''' + ct.Masanpham + '''' as UpdateQuery
FROM Chitietdonhang ct
WHERE ct.Madonhang IN (1, 2, 6, 7, 8, 9)
ORDER BY ct.Madonhang;

