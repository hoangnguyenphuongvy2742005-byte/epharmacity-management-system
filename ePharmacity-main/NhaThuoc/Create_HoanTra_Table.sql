-- Tạo bảng DanhSachHoanTra để lưu thông tin các đơn hoàn trả
CREATE TABLE DanhSachHoanTra (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    Madonhang INT NOT NULL,
    Sodienthoaikhachhang NVARCHAR(20) NOT NULL,
    Manhanvien NVARCHAR(50) NOT NULL,
    PhantramHoanTra INT NOT NULL, -- 50, 75, hoặc 100
    TongTienDonHang DECIMAL(18,2) NOT NULL,
    SoTienHoanTra DECIMAL(18,2) NOT NULL, -- Số tiền thực tế được hoàn trả
    NgayHoanTra DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (Madonhang) REFERENCES Donhang(Madonhang)
);

-- Tạo index để tìm kiếm nhanh
CREATE INDEX IX_DanhSachHoanTra_Madonhang ON DanhSachHoanTra(Madonhang);
CREATE INDEX IX_DanhSachHoanTra_NgayHoanTra ON DanhSachHoanTra(NgayHoanTra);

