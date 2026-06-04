-- Kiểm tra xem bảng DanhSachHoanTra có tồn tại không
SELECT 
    TABLE_NAME,
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'DanhSachHoanTra'
ORDER BY ORDINAL_POSITION;

-- Nếu bảng không tồn tại, chạy script tạo bảng:
/*
CREATE TABLE DanhSachHoanTra (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Madonhang INT NOT NULL,
    Sodienthoaikhachhang NVARCHAR(20) NOT NULL,
    Manhanvien NVARCHAR(50) NOT NULL,
    PhantramHoanTra INT NOT NULL,
    TongTienDonHang DECIMAL(18,2) NOT NULL,
    SoTienHoanTra DECIMAL(18,2) NOT NULL,
    NgayHoanTra DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (Madonhang) REFERENCES Donhang(Madonhang)
);
*/

-- Kiểm tra dữ liệu trong bảng (nếu có)
SELECT COUNT(*) as 'Số lượng bản ghi' FROM DanhSachHoanTra;

-- Xem dữ liệu mẫu (nếu có)
SELECT TOP 5 * FROM DanhSachHoanTra ORDER BY NgayHoanTra DESC;

