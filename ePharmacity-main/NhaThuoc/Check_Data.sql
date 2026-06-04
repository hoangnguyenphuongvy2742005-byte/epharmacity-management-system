-- Script kiểm tra dữ liệu trong YeuCauHoanDon
USE [NhaThuocDB1]
GO

-- Kiểm tra số lượng dữ liệu
SELECT COUNT(*) as SoLuongYeuCau FROM YeuCauHoanDon;

-- Hiển thị tất cả dữ liệu
SELECT 
    ID,
    Madonhang,
    Sodienthoaikhachhang,
    Manhanvien,
    Lydo,
    Mota,
    YeuCau,
    Ngaytao
FROM YeuCauHoanDon
ORDER BY Ngaytao DESC;

-- Kiểm tra bảng Donhang có dữ liệu không
SELECT COUNT(*) as SoLuongDonHang FROM Donhang;

-- Kiểm tra bảng Thongtinkhachhang có dữ liệu không
SELECT COUNT(*) as SoLuongKhachHang FROM Thongtinkhachhang;

