-- Test INSERT trực tiếp vào DanhSachHoanTra
-- Thay đổi các giá trị này cho phù hợp với dữ liệu thực tế

-- Kiểm tra dữ liệu hiện tại
SELECT COUNT(*) as 'Số bản ghi hiện tại' FROM DanhSachHoanTra;

-- Test INSERT với dữ liệu mẫu
INSERT INTO DanhSachHoanTra 
(Madonhang, Sodienthoaikhachhang, Manhanvien, PhantramHoanTra, TongTienDonHang, SoTienHoanTra)
VALUES (999, '0123456789', 'TEST_EMPLOYEE', 75, 1000000, 750000);

-- Kiểm tra kết quả
SELECT * FROM DanhSachHoanTra WHERE Madonhang = 999;

-- Xóa dữ liệu test
DELETE FROM DanhSachHoanTra WHERE Madonhang = 999;

-- Kiểm tra lại
SELECT COUNT(*) as 'Số bản ghi sau khi xóa' FROM DanhSachHoanTra;

