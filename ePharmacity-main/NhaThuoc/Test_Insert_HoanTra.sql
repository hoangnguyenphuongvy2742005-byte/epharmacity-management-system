-- Test insert vào bảng DanhSachHoanTra
-- Thay đổi các giá trị này cho phù hợp với dữ liệu thực tế của bạn

DECLARE @TestOrderId INT = 1; -- Thay bằng mã đơn hàng thực tế
DECLARE @TestPhone NVARCHAR(20) = '0123456789'; -- Thay bằng SĐT thực tế
DECLARE @TestEmployee NVARCHAR(50) = 'NV001'; -- Thay bằng mã nhân viên thực tế

-- Test insert
INSERT INTO DanhSachHoanTra 
(Madonhang, Sodienthoaikhachhang, Manhanvien, PhantramHoanTra, TongTienDonHang, SoTienHoanTra)
VALUES (@TestOrderId, @TestPhone, @TestEmployee, 75, 1000000, 750000);

-- Kiểm tra kết quả
SELECT * FROM DanhSachHoanTra WHERE Madonhang = @TestOrderId;

-- Xóa dữ liệu test (nếu muốn)
-- DELETE FROM DanhSachHoanTra WHERE Madonhang = @TestOrderId;

