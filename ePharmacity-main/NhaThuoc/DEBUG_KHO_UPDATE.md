# 🔍 **DEBUG: KIỂM TRA TẠI SAO KHO KHÔNG CẬP NHẬT**

## 🚨 **VẤN ĐỀ:**
Khi thực hiện hoàn trả hàng, số lượng hàng trong kho không được cập nhật đúng.

## 🔧 **CÁCH DEBUG:**

### **Bước 1: Chạy script kiểm tra dữ liệu**
```sql
-- Chạy file: DEBUG_KHO_UPDATE.sql
-- Script này sẽ kiểm tra:
-- 1. Dữ liệu trong Chitietdonhang
-- 2. Dữ liệu trong Sanphamthuoc  
-- 3. Mapping giữa Chitietdonhang và Sanphamthuoc
-- 4. Dữ liệu trong YeuCauHoanDon
-- 5. Test query cập nhật kho
```

### **Bước 2: Mở ứng dụng và test với debug**
1. **Mở Visual Studio**
2. **Chạy project**
3. **Đăng nhập:** `NhanVienBanHang_0987654321` / `123456`
4. **Click "Xử Lý Hoàn Trả"**
5. **Click vào một dòng** để chọn yêu cầu
6. **Click "Chấp nhận"** - Sẽ hiển thị form chọn phần trăm hoàn trả
7. **Chọn phần trăm hoàn trả** (50%, 75%, hoặc 100%)
8. **Click "Xác nhận"** - Sẽ hiển thị thông báo xác nhận
9. **Click "Yes"** - Sẽ xử lý hoàn trả

### **Bước 3: Kiểm tra các thông báo debug**
Khi xử lý hoàn trả, sẽ có các thông báo debug:
1. **"Sản phẩm: [Masanpham], Số lượng hoàn trả: [Số lượng]"**
2. **"Đã cập nhật [Số dòng] dòng cho sản phẩm [Masanpham]"**

### **Bước 4: Phân tích kết quả**

#### **Nếu có thông báo "Đã cập nhật 0 dòng":**
- **Vấn đề:** Không tìm thấy sản phẩm trong bảng `Sanphamthuoc`
- **Nguyên nhân:** `Masanpham` trong `Chitietdonhang` không khớp với `Mahanghoa` trong `Sanphamthuoc`
- **Giải pháp:** Kiểm tra mapping giữa hai bảng

#### **Nếu có thông báo "Đã cập nhật 1 dòng":**
- **Vấn đề:** Query chạy đúng nhưng kho không cập nhật
- **Nguyên nhân:** Có thể là vấn đề về transaction hoặc cache
- **Giải pháp:** Kiểm tra lại dữ liệu trong database

#### **Nếu không có thông báo debug:**
- **Vấn đề:** Không có dữ liệu trong `Chitietdonhang` cho đơn hàng này
- **Nguyên nhân:** Đơn hàng không có chi tiết sản phẩm
- **Giải pháp:** Kiểm tra dữ liệu trong `Chitietdonhang`

## 🎯 **CÁC NGUYÊN NHÂN CÓ THỂ:**

### **1. Mapping sai giữa bảng:**
- `Chitietdonhang.Masanpham` không khớp với `Sanphamthuoc.Mahanghoa`
- Cần kiểm tra dữ liệu trong cả hai bảng

### **2. Dữ liệu không tồn tại:**
- Đơn hàng không có chi tiết sản phẩm trong `Chitietdonhang`
- Sản phẩm không tồn tại trong `Sanphamthuoc`

### **3. Vấn đề về transaction:**
- Transaction không được commit đúng cách
- Có lỗi trong quá trình xử lý

### **4. Vấn đề về cache:**
- Dữ liệu được cập nhật nhưng không hiển thị ngay
- Cần refresh hoặc restart ứng dụng

## 🚀 **GIẢI PHÁP NHANH:**

### **Nếu muốn test ngay:**
1. **Chạy script:** `DEBUG_KHO_UPDATE.sql`
2. **Kiểm tra kết quả** để xem mapping có đúng không
3. **Test ứng dụng** với debug messages
4. **Kiểm tra dữ liệu** trong database sau khi xử lý

### **Nếu vẫn không được:**
1. **Kiểm tra dữ liệu** trong `Chitietdonhang` và `Sanphamthuoc`
2. **Kiểm tra mapping** giữa `Masanpham` và `Mahanghoa`
3. **Kiểm tra transaction** có được commit đúng không

## 📋 **THÔNG TIN DEBUG:**

Sau khi chạy ứng dụng, hãy cho tôi biết:
1. **Có thông báo debug nào xuất hiện không?**
2. **Thông báo debug nói gì?**
3. **Kết quả của script DEBUG_KHO_UPDATE.sql là gì?**
4. **Dữ liệu trong kho có thay đổi không?**

Tôi sẽ giúp bạn sửa vấn đề dựa trên thông tin này!

