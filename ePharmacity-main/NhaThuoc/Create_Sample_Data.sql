-- Script tạo dữ liệu mẫu cho YeuCauHoanDon
-- Tạo dữ liệu đồng bộ với lịch sử đơn hàng

USE [NhaThuocDB1]
GO

-- Xóa dữ liệu cũ
DELETE FROM YeuCauHoanDon;
PRINT N'Đã xóa hết dữ liệu cũ trong YeuCauHoanDon.';

-- Tạo dữ liệu mới từ lịch sử đơn hàng
INSERT INTO YeuCauHoanDon (Madonhang, Sodienthoaikhachhang, Lydo, Mota, YeuCau, Ngaytao)
SELECT
    d.Madonhang,
    d.Sodienthoaikhachhang,
    CASE
        WHEN d.Madonhang = 9 THEN N'Nhận sai hàng'
        WHEN d.Madonhang = 8 THEN N'Thiếu hàng'
        WHEN d.Madonhang = 7 THEN N'Hàng đã qua sử dụng'
        WHEN d.Madonhang = 6 THEN N'Hàng lỗi, không hoạt động'
        WHEN d.Madonhang = 2 THEN N'Bể, vỡ'
        WHEN d.Madonhang = 1 THEN N'Nhận sai hàng'
        ELSE N'Lý do khác'
    END as Lydo,
    CASE
        WHEN d.Madonhang = 9 THEN N'Khách hàng nhận sai loại thuốc'
        WHEN d.Madonhang = 8 THEN N'Thiếu một số lượng thuốc trong đơn hàng'
        WHEN d.Madonhang = 7 THEN N'Sản phẩm đã có dấu hiệu sử dụng'
        WHEN d.Madonhang = 6 THEN N'Thuốc không có hiệu quả như mong đợi'
        WHEN d.Madonhang = 2 THEN N'Vỏ thuốc bị vỡ trong quá trình vận chuyển'
        WHEN d.Madonhang = 1 THEN N'Khách hàng nhận sai loại thuốc'
        ELSE N'Thuốc không hoạt động đúng chức năng'
    END as Mota,
    N'Hoàn trả' as YeuCau,
    DATEADD(day, -ABS(d.Madonhang % 10), GETDATE()) as Ngaytao
FROM Donhang d
ORDER BY d.Madonhang DESC;

PRINT N'Đã tạo ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + N' yêu cầu hoàn trả từ lịch sử đơn hàng';

-- Kiểm tra dữ liệu đã tạo
SELECT 
    ID,
    Madonhang,
    Sodienthoaikhachhang,
    Lydo,
    Mota,
    YeuCau,
    Ngaytao
FROM YeuCauHoanDon
ORDER BY Ngaytao DESC;

PRINT N'Script hoàn thành! Dữ liệu đã được tạo thành công.';
